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

        /// <summary>
        /// Recherche une liste de film par leur nom
        /// </summary>
        /// <param name="search">Recherche demandée</param>
        /// <param name="page">numéro de la page</param>
        /// <param name="year">Année à laquelle rechercher les films</param>
        /// <returns><see cref="SearchResult"/></returns>
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

        /// <summary>
        /// Récupère un film détaillé par son identifiant
        /// </summary>
        /// <param name="imdbID">Identifiant d'un film</param>
        /// <returns><see cref="MovieDetails"/></returns>
        public static MovieDetails GetFilm(string imdbID)
        {
            string url = $"{baseUrl}&i={imdbID}";
            using (var client = new WebClient())
            {
                string responce = client.DownloadString(url);
                MovieDetails result = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieDetails>(responce);

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
