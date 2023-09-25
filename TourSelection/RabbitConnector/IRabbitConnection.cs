using RabbitMQ.Client;

public interface IRabbitConnection {
    IModel GetChannel();
}