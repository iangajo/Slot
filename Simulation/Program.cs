using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Simulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
            var registration = new Register()
            {
                Username = "user00002",
                Password = "123"
            };

            var json = JsonConvert.SerializeObject(registration);

            var buffer = Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = client.PostAsync("http://localhost:5000/api/register", byteContent);

            if (response.IsCompleted)
            {
                Console.WriteLine("Done");
            }

            Console.ReadLine();

            
        }
    }
}
