using System.Net;
using System.Threading.Tasks;
using API_Gateway.Kafka;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API_Gateway.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly ILogger<GatewayController> _logger;
        private readonly KafkaProducer _producer;

        public GatewayController(ILogger<GatewayController> logger, KafkaProducer producer)
        {
            _logger = logger;
            _producer = producer;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("RECEIVED REQUEST TO GATEWAY");
            return Ok("Hello");
        }

        /*[HttpGet("Message/Send")]
        public async Task<IActionResult> SendMessage()
        {
            await _producer.SendMessage("bzbz");
            return Ok("bzbz");
        }*/
    }
}
