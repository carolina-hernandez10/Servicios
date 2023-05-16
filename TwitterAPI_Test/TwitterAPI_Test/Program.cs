using Newtonsoft.Json;
using System;

namespace TwitterAPI_Test
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using RestSharp;


    class Program
    {
        static async Task Main()
        {
            // Set your Twitter API credentials
            var apiKey = "oiGHCGydv8J9DTOig1eO2vqVN";
            var apiSecretKey = "DjG4Tgm7C3noZoKEXSzei5APbG3cGTstrc3yJx1VbKH5qAiXET";

            // Encode the credentials
            var encodedCredentials = Convert.ToBase64String(
                System.Text.Encoding.GetEncoding("ISO-8859-1")
                    .GetBytes(apiKey + ":" + apiSecretKey)
            );

            // Set the Twitter OAuth2 URL
            var oauthUrl = "https://api.twitter.com/oauth2/token";

            // Create the HttpClient
            var httpClient = new HttpClient();

            var bearerToken = string.Empty;

            // Prepare the request content
            var requestContent = new StringContent("grant_type=client_credentials",
                System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            // Add the Authorization header
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + encodedCredentials);

            // Send the request
            var response = await httpClient.PostAsync(oauthUrl, requestContent);

            // Process the response
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var bearerTokenResponse = JsonConvert.DeserializeObject<BearerTokenResponse>(responseContent);
                bearerToken = bearerTokenResponse.AccessToken;

                // Use the bearer token to make authenticated requests to Twitter API 1.1
                Console.WriteLine("Bearer Token: " + bearerToken);
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }

            // Set the Twitter API URL
            var url = "https://api.twitter.com/1.1/statuses/user_timeline.json";

            // Set the screen name of the user whose timeline you want to fetch
            var screenName = "infoautopista";

            // Set the bearer token      

            // Create the REST client and request
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            // Add the request parameters
            request.AddParameter("screen_name", screenName);

            // Add the Authorization header with the bearer token
            request.AddHeader("Authorization", "Bearer " + bearerToken);

            // Execute the request
            var response2 = client.Execute(request);

            // Process the response
            if (response2.IsSuccessful)
            {
                dynamic tweets = JsonConvert.DeserializeObject(response2.Content);
                foreach (var tweet in tweets)
                {
                    Console.WriteLine(tweet.text);
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }
    }
}


class BearerTokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }
}


