using System;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace API_Gateway.Kafka
{
    public class KafkaConsumer : IHostedService
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private IConsumer<Ignore, string> _consumer;

        public KafkaConsumer(ILogger<KafkaConsumer> logger)
        {
            _logger = logger;
            _logger.LogInformation("Initializing consumer...");
            var config = new ConsumerConfig()
            {
                BootstrapServers = "localhost:29092",
                GroupId = "PostServiceGroup"
            };
            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _logger.LogInformation("Initialized consumer...");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("Posts");
            _logger.LogInformation("Subscribed to Posts...");

            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(cancellationToken);

                        var message = consumeResult.Message.Value;

                        _logger.LogInformation($"Received message: {message}");
                    }
                    catch (OperationCanceledException ex)
                    {
                        _logger.LogError($"Operation was cancelled: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error processing Kafka message: {ex.Message}");
                    }
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Closing consumer...");
            _consumer.Close();
            return Task.CompletedTask;
        }
    }
}