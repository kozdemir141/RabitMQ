using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://pmopgutd:edmooHf15DdFBhXhm4ZtLQ3UCbSp40xQ@chimpanzee.rmq.cloudamqp.com/pmopgutd");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.BasicQos(0,1,false);

var consumer = new EventingBasicConsumer(channel);

// autoAck = İlgili mesajları kuyruktan silmezf
channel.BasicConsume("hello-queue", false, consumer);

consumer.Received += (sender, eventArgs) =>
{
    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

    Thread.Sleep(1500);
    
    Console.WriteLine("Gelen Mesaj:" + message);
    
    // İlgili mesajları artık silebilirsin.
    channel.BasicAck(eventArgs.DeliveryTag,false);
};

Console.ReadLine();


