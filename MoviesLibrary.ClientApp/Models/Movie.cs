using Framework.MVVM;
using System;
using System.Text;

namespace MoviesLibrary.ClientApp.Models
{
    public class Movie : ObservableObject
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
        /// <summary>
        /// Obtient ou défini l'année du film
        /// </summary>
        public int Year { get => this._Year; set => this.SetProperty(nameof(this.Year), ref this._Year, value); }
        /// <summary>
        /// Obtient ou défini l'identifiant du film
        /// </summary>
        public string imdbID { get => this._imdbID; set => this.SetProperty(nameof(this.imdbID), ref this._imdbID, value); }
        /// <summary>
        /// Obtient ou défini le l'url du poster du film
        /// </summary>
        public string Poster { get => this._Poster; set => this.SetProperty(nameof(this.Poster), ref this._Poster, value); }

        #endregion

        #region Constructor

        public Movie() { }

        #endregion
    }
}
