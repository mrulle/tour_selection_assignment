// using System.Runtime.CompilerServices;
// using System.Text;
// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;

// public class ConsumerEmailService {
//     IModel _channel;
//     static readonly string routingKey = "tour.booked";

//     public ConsumerEmailService(IModel channel, ) {
//         _channel = channel;
//     }

//     public void Receive() {

        
//         _channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);
//         // declare a server-named queue
//         var queueName = _channel.QueueDeclare().QueueName;

//         if (args.Length < 1)
//         {
//             Console.Error.WriteLine("Usage: {0} [binding_key...]",
//                                     Environment.GetCommandLineArgs()[0]);
//             Console.WriteLine(" Press [enter] to exit.");
//             Console.ReadLine();
//             Environment.ExitCode = 1;
//             return;
//         }

//         foreach (var bindingKey in args)
//         {
//             _channel.QueueBind(queue: queueName,
//                             exchange: "topic_logs",
//                             routingKey: bindingKey);
//         }

//         Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

//         var consumer = new EventingBasicConsumer(_channel);
//         consumer.Received += (model, ea) =>
//         {
//             var body = ea.Body.ToArray();
//             var message = Encoding.UTF8.GetString(body);
//             var routingKey = ea.RoutingKey;
//             Console.WriteLine($" [x] Received '{routingKey}':'{message}'");
//         };
//         _channel.BasicConsume(queue: queueName,
//                             autoAck: true,
//                             consumer: consumer);

//         Console.WriteLine(" Press [enter] to exit.");
//         Console.ReadLine();
//     }
// }