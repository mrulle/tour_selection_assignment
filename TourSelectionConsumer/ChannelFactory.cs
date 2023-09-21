using RabbitMQ.Client;

public class ChannelFactory
{
    ConnectionFactory _factory;
    IConnection connection;
    public ChannelFactory(string host, int port)
    {
        _factory = new ConnectionFactory { HostName = host, Port = port };
        connection = _factory.CreateConnection();
    }

    private IConnection CreateConnection()
    {
        return _factory.CreateConnection();
    }

    public IModel CreateChannel()
    {
        return connection.CreateModel();
    }
}