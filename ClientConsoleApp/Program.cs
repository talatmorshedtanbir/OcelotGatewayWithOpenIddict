using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using System.Net.Http.Json;

namespace ClientConsoleApp
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            
            using var client = new HttpClient(handler);
            const string email = "abcde@gmail.com", password = "12345678@Az";

            await CreateAccountAsync(client, email, password);

            var token = await GetTokenAsync(client, email, password);
            Console.WriteLine("Access token: {0}", token);
            Console.WriteLine();

            string url = "https://localhost:44336/food";

            var resource = await GetResourceAsync(client, token,url);
            Console.WriteLine("API response from FoodApi Server: {0}", resource);
            Console.WriteLine();

            url = "https://localhost:44336/cart";

            resource = await GetResourceAsync(client, token, url);
            Console.WriteLine("API response from CartApi Server: {0}", resource);

            Console.ReadLine();
        }

        public static async Task CreateAccountAsync(HttpClient client, string email, string password)
        {
            var response = await client.PostAsJsonAsync("https://localhost:44336/register", new { email, password });

            // Ignore 409 responses, as they indicate that the account already exists.
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }

        public static async Task<string> GetTokenAsync(HttpClient client, string email, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44336/token");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["username"] = email,
                ["password"] = password
            });

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);

            var payload = await response.Content.ReadFromJsonAsync<OpenIddictResponse>();

            if (!string.IsNullOrEmpty(payload.Error))
            {
                throw new InvalidOperationException("An error occurred while retrieving an access token.");
            }

            return payload.AccessToken;
        }

        public static async Task<string> GetResourceAsync(HttpClient client, string token, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
