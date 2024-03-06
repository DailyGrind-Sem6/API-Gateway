using Confluent.Kafka;

namespace API_Gateway.Kafka;

public class KafkaConsumer
{
    private readonly ILogger<KafkaConsumer> _logger;
    private IConsumer<Ignore, string> _consumer;
    public KafkaConsumer(ILogger<KafkaConsumer> logger)
    {
        _logger = logger;
        var config = new ConsumerConfig()
        {
            BootstrapServers = "localhost:29092",
        };
        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }
    
    protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("InventoryUpdates");

        while (!stoppingToken.IsCancellationRequested)
        {
            ProcessKafkaMessage(stoppingToken);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        _consumer.Close();
    }

    public void ProcessKafkaMessage(CancellationToken stoppingToken)
    {
        try
        {
            var consumeResult = _consumer.Consume(stoppingToken);

            var message = consumeResult.Message.Value;

            _logger.LogInformation($"Received message: {message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing Kafka message: {ex.Message}");
        }
    }
    
}