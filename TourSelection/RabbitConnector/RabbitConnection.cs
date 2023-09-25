using System.Runtime.InteropServices;
using RabbitMQ.Client;

public class RabbitConnection : IRabbitConnection
{
    // https://stackoverflow.com/questions/15033848/how-can-a-rabbitmq-client-tell-when-it-loses-connection-to-the-server
    private string host = "localhost";
    private int port = 6000;
    private ConnectionFactory _factory;
    private IConnection connection;

    private bool isConnected = false;
    private static IModel channel;
    public RabbitConnection() {
        _factory = new ConnectionFactory { HostName = host, Port = port };
        OnConnectionLost();
    }

    public IModel createChannel()
    {
        if (isConnected) {
            return connection.CreateModel();
        }
        else {
            throw new Exception("no connection to RabbitMQ");
        }

    }

    private void createConnection() {
        while (!isConnected){
            try {
                // https://www.rabbitmq.com/heartbeats.html <-- docs says values between 5 and 20 seconds are optimal
                _factory.RequestedHeartbeat = TimeSpan.FromSeconds(16);
                connection = _factory.CreateConnection();
                isConnected = true;
                Console.WriteLine("Connected to RabbitMQ at " + host + ":" + port);
            }
            catch (Exception e) {
                Console.WriteLine("Connection attempt to RabbitMQ at " + host + ":" + port + " failed");
            }
        }
    }

    private void OnConnectionLost() {
        isConnected = false;
        Thread t = new Thread(createConnection);
        t.Start();
        channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);
        
    }

    public IModel getChannel() {
        if (channel is not null) {
            return channel;
        }
        channel = createChannel();
        return channel;
    }
}