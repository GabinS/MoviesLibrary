﻿using Framework.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace MoviesLibrary.ClientApp.Models
{
    public class SearchResult : ObservableObject
    {
        #region Fields

        /// <summary>
        /// Liste des films recherchés
        /// </summary>
        private ObservableCollection<Movie> _Search;
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
        /// <summary>
        /// Obtient ou défini la liste des films recherchés
        /// </summary>
        public ObservableCollection<Movie> Search { get => this._Search; set => this.SetProperty(nameof(this.Search), ref this._Search, value); }
        /// <summary>
        /// Obtient ou défini le nombre total de résultat
        /// </summary>
        public int totalResults { get => this._totalResults; set => this.SetProperty(nameof(this.totalResults), ref this._totalResults, value); }
        /// <summary>
        /// Obtient ou défini l'état de la recherche
        /// </summary>
        public bool Response { get => this._Response; set => this.SetProperty(nameof(this.Response), ref this._Response, value); }

        #endregion

        #region Constructor

        public SearchResult() { }

        #endregion

    }
}
