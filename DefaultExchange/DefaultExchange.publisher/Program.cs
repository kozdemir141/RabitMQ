using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://pmopgutd:edmooHf15DdFBhXhm4ZtLQ3UCbSp40xQ@chimpanzee.rmq.cloudamqp.com/pmopgutd");

using var connection = factory.CreateConnection();

// RabitMq ya mesaj göndermek için bir kuyruğun olması lazım yoksa mesaj boşa düşer.
// IModel geriye dönüyor bu benim kanalıma(channel) karşılık geliyor.
// Bu Channel üzerinden rabbitMq ile iletişime geçebiliriz.
var channel = connection.CreateModel();

// Kuyruğu oluşturduk.
// durable = RabitMQ da oluşan kuyruklar fiziksel olarak tutulsun mu?
// exclusive = Bu kuyruğa sadece buradan oluşturulan kanal üzerinden mi erişilsin?
// autoDelete = Subscriber down olursa kuyruk silinsin mi?
channel.QueueDeclare("hello-queue", true, false, false);

// Tek seferde çalıştığında 50 tane mesaj kuyruğa gidicek.
Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    string message = $"Message {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);
    // Exchange kullanmadığımızdan dolayı default olarak default exchange kullanmış oluyoruz bu yüzden routing key imize kuyruğumuzdaki ismi vermemiz gerekiyor.

    Console.WriteLine($"Mesaj Gönderilmiştir : {message}");
});

Console.ReadLine();