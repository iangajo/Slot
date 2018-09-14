using System;
using System.Collections.Generic;
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
            Console.WriteLine("Slots simulation will start in 10 seconds");
            Console.WriteLine("You must clear the stats table to be accurate");
            Thread.Sleep(10000);

            var random = new Random();
            var suffix = random.Next(1, 10000);
            var registration = new Register()
            {
                Username = $"user{suffix}",
                Password = "Password123!"
            };


            var registrationResponse = PostAsJson<RegistrationResponse, Register>(registration, "http://localhost:62942/api/register");

            Console.WriteLine();

            Console.WriteLine($"Registration Completed with PlayerId:{registrationResponse.PlayerId}");

            Console.WriteLine("Getting Authentication Token");

            var authResponse = PostAsJson<AuthResponse, BaseResponse>(null,
                $"http://localhost:62942/api/auth/{registrationResponse.PlayerId}");

            Console.WriteLine($"Authentication Token: {authResponse.access_token}");

            Console.WriteLine();

            Console.WriteLine("Starting 10 Million Spin Simulation");

            var spinRequest = new SpinRequest()
            {
                PlayerId = registrationResponse.PlayerId,
                Bet = 1
            };

            Console.WriteLine();
            Console.WriteLine();
            for (var i = 0; i < 10000000; i++)
            {
                PostAsJson<SpinResponse, SpinRequest>(spinRequest, "http://localhost:62942/api/spin",
                    authResponse.access_token);

                Console.SetCursorPosition(0, 9);
                Console.WriteLine($"Spin: {i}");
            }

            Console.WriteLine("Spin Completed");

            Console.WriteLine();
            Console.WriteLine("Retrieving RTP");
            var winAmountResponse =
                GetAsJson<WinAmount>($"http://localhost:62942/api/totalwinamount/{registrationResponse.PlayerId}");

            Console.WriteLine($"Player Total Win Amount is: {winAmountResponse.Amount}");

            Console.WriteLine();
            Console.WriteLine("Retrieving Winning Combinations Hit rate");

            var payLinesStats =
                GetAsJson<List<PayLineStat>>($"http://localhost:62942/api/paylinestats");

            foreach (var item in payLinesStats)
            {
                Console.WriteLine($"PayLine: {item.Id}, HitRate: {item.Stat}");
            }

            Console.WriteLine();
            Console.WriteLine("Retrieving Prizes Hit Rate");

            var symbolStats =
                GetAsJson<List<SymbolStat>>($"http://localhost:62942/api/symbolstats");

            Console.WriteLine();

            foreach (var item in symbolStats)
            {
                Console.WriteLine($"Symbol: {item.Symbol}, Five of a Kind: {item.FiveKind}, Four of a Kind: {item.FourKind}, Three of a Kind: {item.ThreeKind}");
            }

            Console.WriteLine();

            Console.ReadLine();
        }

        public static T PostAsJson<T, T1>(T1 request, string url, string authToken = default(string))
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(authToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
                }

                var jsonSerializeObject = JsonConvert.SerializeObject(request);
                var buffer = Encoding.UTF8.GetBytes(jsonSerializeObject);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync(url, byteContent);

                var jsonResult = response.Result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(jsonResult.Result);
            }
        }

        public static T GetAsJson<T>(string url)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url);

                var jsonResult = response.Result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(jsonResult.Result);
            }
        }
    }
}
