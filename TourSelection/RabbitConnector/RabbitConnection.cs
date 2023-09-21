using RabbitMQ.Client;

public class RabbitConnection : IRabbitConnection
{
    private string host = "localhost";
    private int port = 6000;
    private ConnectionFactory _factory;
    private IConnection connection;
    public RabbitConnection() {
        _factory = new ConnectionFactory{ HostName = host, Port = port };
        connection = _factory.CreateConnection();
    }

    public IModel createChannel()
    {
        return connection.CreateModel();
    }
}