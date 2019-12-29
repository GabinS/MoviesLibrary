using Framework.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesLibrary.ClientApp.Models
{
    class Movie : ObservableObject
    {
        #region Fields

        /// <summary>
        /// Titre du film
        /// </summary>
        private string _Title;
        /// <summary>
        /// Année de sortie du film
        /// </summary>
        private int _Year;
        /// <summary>
        /// Identifiant du film dans l'api OMDb (http://www.omdbapi.com)
        /// </summary>
        private string _imdbID;
        /// <summary>
        /// Poster du film
        /// </summary>
        private string _Poster;

        #endregion

        #region properties

        /// <summary>
        /// Obtient ou défini le titre du film
        /// </summary>
        public string Title { get => this._Title; set => this.SetProperty(nameof(this.Title), ref this._Title, value); }
        public int Year { get => this._Year; set => this.SetProperty(nameof(this.Year), ref this._Year, value); }
        public string imdbID { get => this._imdbID; set => this.SetProperty(nameof(this.imdbID), ref this._imdbID, value); }
        public string Poster { get => this._Poster; set => this.SetProperty(nameof(this.Poster), ref this._Poster, value); }
        
        #endregion

        #region Constructor

        public Movie() { }

        #endregion
    }
}
