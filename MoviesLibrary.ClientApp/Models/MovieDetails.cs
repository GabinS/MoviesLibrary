using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesLibrary.ClientApp.Models
{
    public class MovieDetails : Movie
    {
        #region Fields

        /// <summary>
        /// Date de sortie du film
        /// </summary>
        private string _Released;
        /// <summary>
        /// Durée du film
        /// </summary>
        private string _Runtime;
        /// <summary>
        /// Genres du film
        /// </summary>
        private string _Genre;
        /// <summary>
        /// Réalisateur du film
        /// </summary>
        private string _Director;
        /// <summary>
        /// Acteurs du film
        /// </summary>
        private string _Actors;

        #endregion

        #region properties

        /// <summary>
        /// Obtient ou défini la date de sortie du film
        /// </summary>
        public string Released { get => this._Released; set => this.SetProperty(nameof(this.Released), ref this._Released, value); }
        /// <summary>
        /// Obtient ou défini la durée du film
        /// </summary>
        public string Runtime { get => this._Runtime; set => this.SetProperty(nameof(this.Runtime), ref this._Runtime, value); }
        /// <summary>
        /// Obtient ou défini les genres du film
        /// </summary>
        public string Genre { get => this._Genre; set => this.SetProperty(nameof(this.Genre), ref this._Genre, value); }
        /// <summary>
        /// Obtient ou défini le réalisateur du film
        /// </summary>
        public string Director { get => this._Director; set => this.SetProperty(nameof(this.Director), ref this._Director, value); }
        /// <summary>
        /// Obtient ou défini les acteurs du film
        /// </summary>
        public string Actors { get => this._Actors; set => this.SetProperty(nameof(this.Actors), ref this._Actors, value); }

        #endregion

        #region Constructor

        public MovieDetails() { }

        #endregion
    }
}
