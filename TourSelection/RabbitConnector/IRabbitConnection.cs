using RabbitMQ.Client;

public interface IRabbitConnection {
    IModel createChannel();
}