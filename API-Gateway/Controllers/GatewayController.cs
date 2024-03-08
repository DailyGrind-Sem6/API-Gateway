using System.Net;
using System.Threading.Tasks;
using API_Gateway.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API_Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly ILogger<GatewayController> _logger;
        private readonly KafkaProducer _producer;

        public GatewayController(ILogger<GatewayController> logger, KafkaProducer producer)
        {
            _logger = logger;
            _producer = producer;
        }

        [HttpGet("Message/Send")]
        public async Task<IActionResult> SendMessage()
        {
            await _producer.SendMessage("bzbz");
            return Ok("bzbz");
        }

        [HttpGet("Message/Sample")]
        public IActionResult SampleText()
        {
            return Ok("gang");
        }
    }
}
