using System.Text;
using System.Threading;

string host = "localhost";
int port = 6000;

ChannelFactory channelFactory = new ChannelFactory(host, port);



string[] queueNames = {"warning", "error"};
ConsumerEmailService receiver = new ConsumerEmailService(channelFactory.CreateChannel(), queueNames);
Thread receiverThread = new Thread(receiver.Run);
receiverThread.Start();


Console.WriteLine("Press [enter] to exit!");
Console.ReadLine();
Console.WriteLine("Initiating shutdown");
consumerThread.Join();
workerThread.Join();
subscriberThread.Join();
receiverThread.Join();

Console.WriteLine("Shutdown complete");