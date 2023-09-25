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
    private readonly ILogger<RabbitConnection> _logger;

    public RabbitConnection(ILogger<RabbitConnection> logger) {
        _logger = logger;
        _factory = new ConnectionFactory { HostName = host, Port = port, RequestedHeartbeat = TimeSpan.FromSeconds(16) };
        
    }
    private IConnection CreateConnection()
    {
        IConnection createdConnection = null;
        while (!isConnected)
        {
            try
            {
                // https://www.rabbitmq.com/heartbeats.html <-- docs says values between 5 and 20 seconds are optimal
                createdConnection = _factory.CreateConnection();
                isConnected = true;
                _logger.LogInformation("createConnection(): Connected to RabbitMQ at " + host + ":" + port);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                _logger.LogInformation("createConnection(): Connection attempt to RabbitMQ at " + host + ":" + port + " failed");
            }
            Thread.Sleep(1500);
        }
        return createdConnection;
    }
    public IModel CreateChannel()
    {
        if (isConnected) {
            _logger.LogInformation("createChannel(): is connected");
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);
            return channel;
        }
        else {
            _logger.LogInformation("createChannel(): no connection to RabbitMQ");
            throw new Exception("no connection to RabbitMQ");
        }
    }



    private void OnConnectionLost(object? sender, ShutdownEventArgs reason) {
        isConnected = false;
        connection = CreateConnection();
        channel = CreateChannel();
    }

    public IModel GetChannel() {
        if (channel is not null && connection is not null) {
            _logger.LogInformation("getChannel(): channel exists");
            return channel;
        }
        connection = CreateConnection();
        connection.ConnectionShutdown += OnConnectionLost;
        channel = CreateChannel();
        _logger.LogInformation("getChannel(): channel created");
        return channel;
    }
}