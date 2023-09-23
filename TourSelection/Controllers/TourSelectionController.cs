using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

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

    private readonly ILogger<TourSelectionController> _logger;

    public TourSelectionController(ILogger<TourSelectionController> logger, IRabbitConnection connection)
    {
        _logger = logger;
        channel = connection.getChannel();
    }

    [HttpPost]
    public IActionResult Post([FromBody] TourActionModel model)
    {
        try {
            var routingKey = "tour." + model.TourAction;
            string message = JsonSerializer.Serialize(model);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "topic_logs",
                                routingKey: routingKey,
                                basicProperties: null,
                                body: body);
            
            return Ok(200);
        }
        catch (Exception e) {
            return BadRequest();
        }
    }
}
