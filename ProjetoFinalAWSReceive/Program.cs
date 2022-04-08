﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ProjetoFinalAWSReceive
{
    class Program
    {
        static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "partida",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("Mensagem recebida: ", message);
                };
                channel.BasicConsume(queue: "partida",
                                     autoAck: true,
                                     consumer: consumer);
            }
        }
    }
}
