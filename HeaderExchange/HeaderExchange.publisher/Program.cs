using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://pmopgutd:edmooHf15DdFBhXhm4ZtLQ3UCbSp40xQ@chimpanzee.rmq.cloudamqp.com/pmopgutd");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-topic", ExchangeType.Topic, true);

Random rnd = new Random();
Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogNames log1 = (LogNames)rnd.Next(1, 5);
    LogNames log2 = (LogNames)rnd.Next(1, 5);
    LogNames log3 = (LogNames)rnd.Next(1, 5);

    var routeKey = $"{log1}.{log2}.{log3}";

    string message = $"log-type {log1}-{log2}-{log3}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish("logs-topic", routeKey, null, messageBody);

    Console.WriteLine($"Log Gönderilmiştir : {message}");
});
Console.ReadLine();

public enum LogNames
{
    Critical = 1,
    Error = 2,
    Warning = 3,
    Info = 4
}