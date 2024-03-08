using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace API_Gateway.Kafka
{
    public class KafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;
        private IProducer<Null, string> _producer;
        public KafkaProducer(ILogger<KafkaProducer> logger)
        {
            _logger = logger;
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:29092"
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendMessage(string value)
        {
            _logger.LogInformation("SENT VALUE: " + value);
            await _producer.ProduceAsync("Posts", new Message<Null, string>()
            {
                Value = value
            });
        }
    }
}
