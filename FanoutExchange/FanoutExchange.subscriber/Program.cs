using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://pmopgutd:edmooHf15DdFBhXhm4ZtLQ3UCbSp40xQ@chimpanzee.rmq.cloudamqp.com/pmopgutd");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

// Her bir Instance (Consumer'in) kendisine ait kuyruğu olması için random kuyruk isimleri belirledik.
var randomQueueName = channel.QueueDeclare().QueueName;

// var randomQueueName = "log-database-save-queue";
// channel.QueueDeclare(randomQueueName, true, false, false);

// Uygulama her ayağa kalktığında ilgili kuyruk oluşacak, uygulama down olduğunda kuyruk silinecek.
channel.QueueBind(randomQueueName,"logs-fanout","",null);

channel.BasicQos(0,1,false);

var consumer = new EventingBasicConsumer(channel);

Console.WriteLine("Loglar dinleniyor...");

consumer.Received += (sender, eventArgs) =>
{
    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

    Thread.Sleep(1500);
    
    Console.WriteLine("Gelen Mesaj:" + message);
    
    channel.BasicAck(eventArgs.DeliveryTag,false);
};
channel.BasicConsume(randomQueueName, false, consumer);

Console.ReadLine();