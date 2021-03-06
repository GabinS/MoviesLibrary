﻿using System;
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
        /// <summary>
        /// Attribut qui détermine si le film est en favorie
        /// </summary>
        private bool _IsFavorite;
        /// <summary>
        /// Attribut qui détermine si le film à été vue
        /// </summary>
        private bool _IsView;

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
        /// <summary>
        /// Obtient ou défini le film est en favorie
        /// </summary>
        public bool IsFavorite { get => this._IsFavorite; set => this.SetProperty(nameof(this.IsFavorite), ref this._IsFavorite, value); }
        /// <summary>
        /// Obtient ou défini le film à été vue
        /// </summary>
        public bool IsView { get => this._IsView; set => this.SetProperty(nameof(this.IsView), ref this._IsView, value); }

        #endregion

        #region Constructor

        public MovieDetails() {
            this._IsFavorite = false;
            this._IsView = false;
        }

        #endregion
    }
}
