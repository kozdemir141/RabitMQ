using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://pmopgutd:edmooHf15DdFBhXhm4ZtLQ3UCbSp40xQ@chimpanzee.rmq.cloudamqp.com/pmopgutd");

using var connection = factory.CreateConnection();


var channel = connection.CreateModel();

var randomQueueName = channel.QueueDeclare().QueueName;
//Ortasında Error olan route key leri temsil eder.Başlangıç ve bitişindeki string değerler önemli değildir.
var routeKey = "Info.#";
channel.QueueBind(randomQueueName, "logs-topic", routeKey);

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);

Console.WriteLine("Loglar dinleniyor...");

consumer.Received += (sender, eventArgs) =>
{
    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

    Thread.Sleep(1500);

    Console.WriteLine("Gelen Mesaj:" + message);

    //File.AppendAllText("log-critical.txt", message + "\n");

    channel.BasicAck(eventArgs.DeliveryTag, false);
};
channel.BasicConsume(randomQueueName, false, consumer);

Console.ReadLine();