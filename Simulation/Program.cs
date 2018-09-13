using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Simulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("10 Million Request will start in 10 seconds");
            Thread.Sleep(10000);

            var client = new HttpClient();

            var random = new Random();
            var suffix = random.Next(1, 10000);
            var registration = new Register()
            {
                Username = $"user{suffix}",
                Password = "Password123!"
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

        public static T PostAsJson<T, T1>(T1 request, string url)
        {
            using (var client = new HttpClient())
            {
                var jsonSerializeObject = JsonConvert.SerializeObject(request);
                var buffer = Encoding.UTF8.GetBytes(jsonSerializeObject);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync(url, byteContent);

                var jsonResult = response.Result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(jsonResult.Result);
            }
        }
    }
}
