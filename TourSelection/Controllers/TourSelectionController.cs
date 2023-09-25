using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text.Json;

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
    private IRabbitConnection _connection; // "direct", "fanout", "headers", "topic"

    private readonly ILogger<TourSelectionController> _logger;

    public TourSelectionController(ILogger<TourSelectionController> logger, IRabbitConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    [HttpPost]
    public IActionResult Post([FromBody] TourActionModel model)
    {
        string exchange = "topic_logs";
        string exchangeType = "topic";
        string routingKey = "";
        string message = "";

        try {
            message = JsonSerializer.Serialize(model);
            routingKey = "tour." + model.TourAction;
            _logger.LogInformation("trying to send");
            _connection.Send(exchange, exchangeType, routingKey, message);
            
            return Ok("Successfully sent: " + message);
        }
        catch (Exception e) {
            _logger.LogInformation(e.Message);
            _logger.LogCritical("No connection to RabbitMQ for: " + message);
            return Ok("Sending failed for: " + message);
        }
    }
}
