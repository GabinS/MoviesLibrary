using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesLibrary.ClientApp.API
{
    class APIKey
    {
        private static APIKey instance = null;
        private static string _key = null;

        private APIKey()
        {
            _key = "34ad5063"; // Ma clé API
        }

        /// <summary>
        /// Récupère l'instance de la clé API
        /// </summary>
        public static string GetAPIKey
        {
            get
            {
                if (instance == null)
                {
                    instance = new APIKey();
                }
                return _key;
            }
        }
    }
}
