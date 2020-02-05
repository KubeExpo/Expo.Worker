using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expo.Worker.Workers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Expo.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(
            ILogger<Worker> logger)
        {
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ConnectionFactory connectionFactory = new ConnectionFactory
                {
                    HostName = "arch-pc",
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672
                };
                var connection = connectionFactory.CreateConnection();
                var channel = connection.CreateModel();
                // accept only one unack-ed message at a time

                // uint prefetchSize, ushort prefetchCount, bool global
                channel.BasicQos(0, 1, false);
                MessageReceiver messageReceiver = new MessageReceiver(channel);
                channel.BasicConsume("topic.account.queue", false, messageReceiver);
                Console.ReadLine();

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}