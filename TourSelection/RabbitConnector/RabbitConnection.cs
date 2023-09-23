using RabbitMQ.Client;

public class RabbitConnection : IRabbitConnection
{
    private string host = "localhost";
    private int port = 6000;
    private ConnectionFactory _factory;
    private IConnection connection;
    private static IModel channel;
    public RabbitConnection() {
        _factory = new ConnectionFactory{ HostName = host, Port = port };
        connection = _factory.CreateConnection();
    }

    public IModel createChannel()
    {
        channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);
        return channel;
    }

    public IModel getChannel() {
        if (channel is not null) {
            return channel;
        }
        channel = createChannel();
        return channel;
    }
}