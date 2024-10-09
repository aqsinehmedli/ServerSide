
using System.Net;
using System.Net.Sockets;

Console.WriteLine("Server:");

var ip = IPAddress.Parse("192.168.1.99");
var port = 27001;
var endPoint = new IPEndPoint(ip, port);

var listener = new TcpListener(endPoint);

try
{
    listener.Start();
    Console.WriteLine("Listener starting...");
    while (true)
    {
        var client = listener.AcceptTcpClient();
        _ = Task.Run(() =>
        {
            var stream = client.GetStream();
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string path = $"ScreenShot_{timestamp}.png";

            using (var readFs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                int len = 0;
                var bytes = new byte[1024];
                while ((len = stream.Read(bytes, 0, bytes.Length)) > 0)
                {
                    readFs.Write(bytes, 0, len);
                }
            }
            if (File.Exists(path))
            {
                Console.WriteLine("Dosya başarıyla kaydedildi: " + path);
            }
            else
            {
                Console.WriteLine("Dosya kaydedilemedi.");
            }

            Console.WriteLine("File received");
            client.Close();

        });

    }

}
catch (Exception ex)
{

    Console.WriteLine(ex.Message);
}