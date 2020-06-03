using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ConsoleTables;

namespace APIClient
{
    class Player
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        [JsonPropertyName("position")]
        public string Position { get; set; }
        [JsonPropertyName("height_feet")]
        public int? HeightFeet { get; set; }
        [JsonPropertyName("height_inches")]
        public int? HeightInches { get; set; }
        [JsonPropertyName("weight_pounds")]
        public int? WeightPounds { get; set; }
        public Team Team { get; set; }
    }

    class Team
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("conference")]
        public string Conference { get; set; }
        [JsonPropertyName("division")]
        public string Division { get; set; }
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
    class Response
    {
        [JsonPropertyName("data")]
        public List<Player> players { get; set; }
    }

    class TeamResponse
    {
        [JsonPropertyName("data")]
        public List<Team> teams { get; set; }
    }


    class Program
    {

        static async Task ShowAllPlayersAsync(string token)
        {
            var client = new HttpClient();

            var responseAsStream = await client.GetStreamAsync("https://www.balldontlie.io/api/v1/players/?access_token");

            var responseWithPlayers = await JsonSerializer.DeserializeAsync<Response>(responseAsStream);

            var players = responseWithPlayers.players;

            var table = new ConsoleTable("id", "first_name", "last_name", "height_feet", "height_inches", "position");

            foreach (var player in players)
            {
                table.AddRow(player.Id, player.FirstName, player.LastName, player.HeightFeet, player.HeightInches, player.Position);

                Console.WriteLine($"Player {player.FirstName} {player.LastName} plays the {player.Position} position.");
            }
            table.Write();
        }

        static async Task ShowAllTeamsAsync(string token)
        {
            var client = new HttpClient();

            var responseAsStream = await client.GetStreamAsync("https://www.balldontlie.io/api/v1/teams/");

            var responseWithTeams = await JsonSerializer.DeserializeAsync<TeamResponse>(responseAsStream);

            var teams = responseWithTeams.teams;

            var table = new ConsoleTable("id", "abbreviation", "full_name", "division", "city", "conference", "name");

            foreach (var team in teams)
            {
                table.AddRow(team.Id, team.FullName, team.Abbreviation, team.Division, team.City, team.Conference, team.Name);
            }
            table.Write();
        }
        static async Task Main(string[] args)
        {

            var accessToken = "";

            if (args.Length == 0)
            {
                Console.Write("What is the name of the list, players or teams?");
                accessToken = Console.ReadLine();
            }
            else
            {
                accessToken = args[0];
            }

            var keepGoing = true;
            while (keepGoing)
            {
                Console.Write("Search (A)ll players, All (T)eams, or (Q)uit: ");
                var choice = Console.ReadLine().ToUpper();

                switch (choice)
                {
                    case "Q":
                        keepGoing = false;
                        break;

                    case "A":
                        await ShowAllPlayersAsync(accessToken);
                        break;

                    case "T":
                        await ShowAllTeamsAsync(accessToken);
                        break;
                }
            }

        }

    }
}

