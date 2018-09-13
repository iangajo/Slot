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
                Username = "user00003",
                Password = "123"
            };

            var json = JsonConvert.SerializeObject(registration);

            var buffer = Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = client.PostAsync("http://localhost:5000/api/register", byteContent);

            //var r = client.GetAsync("http://localhost:5000/api/symbolstats");

            //using (var content = r.Result.Content)
            //{
            //    var x = content.ReadAsStringAsync();
            //    Console.WriteLine(x.Result);

            //}

            if (response.IsCompletedSuccessfully)
            {
                Console.WriteLine("Done");
            }

            var a = response.Result.Content.ReadAsStringAsync();

            Console.WriteLine(a.Result);

            Console.ReadLine();

            
        }
    }
}
