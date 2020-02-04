using Framework.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MoviesLibrary.ClientApp.Models
{
    public class SearchResult : ObservableObject
    {
        #region Fields

        /// <summary>
        /// liste des films recherchés
        /// </summary>
        private List<Movie> _Search;
        /// <summary>
        /// Nombre total de résultat
        /// </summary>
        private int _totalResults;
        /// <summary>
        /// Etat de la recherche
        /// </summary>
        private bool _Response;

        #endregion

        #region properties

        public List<Movie> Search { get => this._Search; set => this.SetProperty(nameof(this.Search), ref this._Search, value); }
        public int totalResults { get => this._totalResults; set => this.SetProperty(nameof(this.totalResults), ref this._totalResults, value); }
        public bool Response { get => this._Response; set => this.SetProperty(nameof(this.Response), ref this._Response, value); }

        #endregion

        #region Constructor

        public SearchResult() { }

        #endregion

        #region Methods

        public List<Movie> GetMoviesSearch() => this._Search;

        #endregion
    }
}
