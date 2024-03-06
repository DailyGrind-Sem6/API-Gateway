using API_Gateway.Kafka;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet(Name = "GetSampleText")]
        public async Task<string> Get()
        {
            await _producer.SendMessage("bzbz");
            return "bzbz";
        }
    }
}
