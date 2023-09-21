using System.Text;
using System.Threading;

string host = "localhost";
int port = 6000;

ChannelFactory channelFactory = new ChannelFactory(host, port);


TutorialOneConsumer consumer = new TutorialOneConsumer(channelFactory.CreateChannel());
Thread consumerThread = new Thread(consumer.Run);
consumerThread.Start();

TutorialTwoWorker worker = new TutorialTwoWorker(channelFactory.CreateChannel());
Thread workerThread = new Thread(worker.Work);
workerThread.Start();

Tutorial_3_Subscriber subscriber = new Tutorial_3_Subscriber(channelFactory.CreateChannel());
Thread subscriberThread = new Thread(subscriber.Run);
subscriberThread.Start();

string[] queueNames = {"warning", "error"};
Tutorial4Receiver receiver = new Tutorial4Receiver(channelFactory.CreateChannel(), queueNames);
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