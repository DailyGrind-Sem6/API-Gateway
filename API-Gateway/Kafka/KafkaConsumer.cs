using Confluent.Kafka;

namespace API_Gateway.Kafka;

public class KafkaConsumer
{
    private readonly ILogger<KafkaConsumer> _logger;

    public KafkaConsumer(ILogger<KafkaConsumer> logger)
    {
        _logger = logger;
        var config = new ConsumerConfig()
        {
            BootstrapServers = "localhost:29092",
            
        }
    }
}