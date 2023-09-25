using RabbitMQ.Client;

public interface IRabbitConnection {
    bool Send(string exchange, string exchangeType, string routingKey, string message);
}