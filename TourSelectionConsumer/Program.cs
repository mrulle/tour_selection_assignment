using System.Text;
using System.Threading;

string host = "localhost";
int port = 6000;

ChannelFactory channelFactory = new ChannelFactory(host, port);



string[] queueNames = {"tour.booked"};
SubscriberService emailService = new SubscriberService("EmailService", channelFactory.CreateChannel(), queueNames);
Thread emailReceiverThread = new Thread(emailService.Run);
emailReceiverThread.Start();

string[] otherQueueNames = { "tour.*" };
SubscriberService anotherService = new SubscriberService("OtherService", channelFactory.CreateChannel(), otherQueueNames);
Thread anotherServiceThread = new Thread(anotherService.Run);
anotherServiceThread.Start();


Console.WriteLine("Press [enter] to exit!");
Console.ReadLine();
Console.WriteLine("Initiating shutdown");
emailReceiverThread.Join();
anotherServiceThread.Join();

Console.WriteLine("Shutdown complete");