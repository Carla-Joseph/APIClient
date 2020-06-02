using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APIClient
{
    class Player
    {
        public int Id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string position { get; set; }
        public int height_feet { get; set; }
        public int height_inches { get; set; }
        public int weight_pounds { get; set; }
        // public Team Team { get; set; }
    }
    class Response
    {
        [JsonPropertyName("data")]
        public List<Player> players { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)

        {
            var client = new HttpClient();

            var responseAsStream = await client.GetStreamAsync("https://www.balldontlie.io/api/v1/players/");

            var responseWithPlayers = await JsonSerializer.DeserializeAsync<Response>(responseAsStream);
            var players = responseWithPlayers.players;

            Console.WriteLine(players.Count);

        }
    }
}
