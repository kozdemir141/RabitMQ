using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://pmopgutd:edmooHf15DdFBhXhm4ZtLQ3UCbSp40xQ@chimpanzee.rmq.cloudamqp.com/pmopgutd");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

// Exchange Declare ettim, Kuyruk(Queue) Subscriber tarafında declare edilecek.
channel.ExchangeDeclare("logs-fanout",ExchangeType.Fanout,true);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    string message = $"log {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("logs-fanout", "", null, messageBody);
    
    Console.WriteLine($"Mesaj Gönderilmiştir : {message}");
});

Console.ReadLine();