using RabbitMQ.Client;

public interface IRabbitConnection {
    IModel getChannel();
}