using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Runtime.CompilerServices;

namespace TourSelection.Controllers;

[ApiController]
[Route("[controller]")]
public class TourSelectionController : ControllerBase
{
    private static readonly string[] Commands = new[]
    {
        "booked", "cancelled"
    };
    private IModel channel;
    private IRabbitConnection _connection;

    private readonly ILogger<TourSelectionController> _logger;

    public TourSelectionController(ILogger<TourSelectionController> logger, IRabbitConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    [HttpPost]
    public IActionResult Post([FromBody] TourActionModel model)
    {
        channel = _connection.GetChannel();
        var routingKey = "";
        var message = "";

        try {
            routingKey = "tour." + model.TourAction;
            _logger.LogInformation("trying to send");
            var body = Encoding.UTF8.GetBytes(model.ToString());
            channel.BasicPublish(exchange: "topic_logs",
                                routingKey: routingKey,
                                basicProperties: null,
                                body: body);
            return Ok("Successfully sent: " + message);
        }
        catch (Exception e) {
            _logger.LogInformation(e.Message);
            _logger.LogCritical("No connection to RabbitMQ for: " + message);
            return Ok("Sending failed for: " + message);
        }
    }
}
