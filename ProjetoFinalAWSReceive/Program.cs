using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace ProjetoFinalAWSReceive
{
    class Program
    {
        static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            string nomeFila = "partida";
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: nomeFila,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("Mensagem recebida: {0}", message);
                        channel.BasicAck(ea.DeliveryTag, false);
                        Thread.Sleep(1000);

                };

                channel.BasicConsume(queue: "partida",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine("Todas as mensagens lidas");
                Console.ReadLine();
            }
        }
    }
}
