using MoviesLibrary.ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoviesLibrary.ClientApp.API
{
    public class OmdbAPI
    {
        static HttpClient client = new HttpClient();
        private static string baseUrl = $"http://www.omdbapi.com?apikey={APIKey.GetAPIKey}&type=movie";

        private static async Task Run()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://www.omdbapi.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static SearchResult SearchFilm(string search, int page = 1, int year = 0)
        {
            string stringYear = year != 0 ? $"&y={year}" : "";
            string url = $"{baseUrl}&s={search}&page={page}{stringYear}";
            using (var client = new WebClient())
            {
                string responce = client.DownloadString(url);
                SearchResult result = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchResult>(responce);

                if (result.Response)
                {
                    return result;
                }
                Console.WriteLine("Une erreur est survenue");
            }
            return null;

        }

        public static Movie GetFilm(string imdbID)
        {
            string url = $"{baseUrl}&i={imdbID}";
            using (var client = new WebClient())
            {
                string responce = client.DownloadString(url);
                Movie result = Newtonsoft.Json.JsonConvert.DeserializeObject<Movie>(responce);

                if (result != null)
                {
                    return result;
                }
                Console.WriteLine("Une erreur est survenue");
            }
            return null;

        }
    }
}
