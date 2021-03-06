﻿using Framework.MVVM.Models.Abstracts;
using Framework.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MoviesLibrary.ClientApp.Models;
using MoviesLibrary.ClientApp.ViewModels.Abstracts;
using MoviesLibrary.ClientApp.API;
using System;
using Framework.MVVM;
using System.Collections.Generic;

namespace MoviesLibrary.ClientApp.ViewModels
{
    /// <summary>
    /// Vue-modèle pour l'affichage des films provenant de l'api OMDb.
    /// </summary>
    public class ViewModelSearchMovies : ViewModelList<Movie, IDataContext>, IViewModelMovies
    {
        #region Fields

        /// <summary>
        /// Vue-modèle de la liste de film personnelle.
        /// </summary>
        private IViewModelMyMovies _ViewModelMyMovies;

        /// <summary>
        /// Commande pour lancer une recherche.
        /// </summary>
        private readonly RelayCommand _SearchCommand;
        
        /// <summary>
        /// Commande pour aller à la page précédente
        /// </summary>
        private readonly RelayCommand _PreviousPageCommand;

        /// <summary>
        /// Commande pour aller à la page suivante
        /// </summary>
        private readonly RelayCommand _NextPageCommand;

        /// <summary>
        /// Structure de pagination
        /// </summary>
        private Pagination _Pagination;

        /// <summary>
        /// Recherche
        /// </summary>
        private string _Search;

        /// <summary>
        /// Année de la recherche
        /// </summary>
        private string _Year;

        /// <summary>
        /// Nombre de résultat d'une recherche
        /// </summary>
        private int _TotalResults;

        #endregion

        #region Properties

        /// <summary>
        /// Obtient le titre du vue-modèle
        /// </summary>
        public string Title => "Rechecher Un Film";
        /// <summary>
        /// Obtient ou défini le view modèle <see cref="ViewModelMyMovies"/>.
        /// </summary>
        public IViewModelMyMovies ViewModelMyMovies { get => this._ViewModelMyMovies; private set => this.SetProperty(nameof(this.ViewModelMyMovies), ref this._ViewModelMyMovies, value); }

        /// <summary>
        /// Obtient la commande AddCommand de <see cref="ViewModelMyMovies"/>.
        /// </summary>
        public override RelayCommand AddCommand => this.ViewModelMyMovies.AddCommand;

        /// <summary>
        /// Obtient la commande SearchCommand.
        /// </summary>
        public RelayCommand SearchCommand => this._SearchCommand;
        /// <summary>
        /// Obtient la commande PreviousPageCommand.
        /// </summary>
        public RelayCommand PreviousPageCommand => this._PreviousPageCommand;
        /// <summary>
        /// Obtient la commande NextPageCommand.
        /// </summary>
        public RelayCommand NextPageCommand => this._NextPageCommand;

        /// <summary>
        /// Obtient ou défini le modèle de pagination.
        /// </summary>
        public Pagination Pagination { get => this._Pagination; private set => this._Pagination = value; }
        /// <summary>
        /// Obtient ou défini la recherche par titre.
        /// </summary>
        public string Search { get => this._Search; set => this.SetProperty(nameof(this.Search), ref this._Search, value); }
        /// <summary>
        /// Obtient ou défini la recherche avec année.
        /// </summary>
        public string Year { get => this._Year; set => this.SetProperty(nameof(this.Year), ref this._Year, value); }
        /// <summary>
        /// Obtient ou défini le total de resultat de la recherche en cours.
        /// </summary>
        public int TotalResults { get => this._TotalResults; set => this.SetProperty(nameof(this.TotalResults), ref this._TotalResults, value); }


        #endregion

        #region Constructor

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ViewModelMovies"/>
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de service de l'application.</param>
        public ViewModelSearchMovies(IServiceProvider serviceProvider)
            : base(serviceProvider.GetService<IDataContext>())
        {
            this._ViewModelMyMovies = serviceProvider.GetService<IViewModelMyMovies>();
            this._SearchCommand = new RelayCommand(this.SearchMovie, this.CanSearch);
            this._PreviousPageCommand = new RelayCommand(this.PreviousPage, this.CanPreviousPage);
            this._NextPageCommand = new RelayCommand(this.NextPage, this.CanNextPage);
            this._Pagination = new Pagination();
            this._Search = "";
            this._Year = "";
            this._TotalResults = -1;

            this.LoadData();
        }

        #endregion

        #region Methods

        public override void LoadData()
        {
            this.SearchMovie();
        }

        #region SearchCommand

        /// <summary>
        /// Methode qui détermine si la commande <see cref="SearchCommand"/> peut être exécutée.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        /// <returns>Détermine si la commande peut être exécutée.</returns>
        protected virtual bool CanSearch(object parameter) => true;

        /// <summary>
        /// Méthode d'exécution de la commande <see cref="SearchCommand"/>.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        protected virtual void SearchMovie(object parameter)
        {
            SearchMovie();
            if (ItemsSource != null)
            {
                int maxPage = (this.TotalResults - this.TotalResults % 10) / 10;
                if (this.TotalResults % 10 != 0) maxPage++;
                this.Pagination.Refresh(1, maxPage);
            }
        }

        #endregion

        #region PreviousPageCommand

        /// <summary>
        /// Methode qui détermine si la commande <see cref="PreviousPageCommand"/> peut être exécutée.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        /// <returns>Détermine si la commande peut être exécutée.</returns>
        protected virtual bool CanPreviousPage(object parameter) => true;

        /// <summary>
        /// Méthode d'exécution de la commande <see cref="PreviousPageCommand"/>.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        protected virtual void PreviousPage(object search)
        {
            this.Pagination.GoToPreviousPage();
            SearchMovie();
        }

        #endregion

        #region NextPageCommand

        /// <summary>
        /// Methode qui détermine si la commande <see cref="NextPageCommand"/> peut être exécutée.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        /// <returns>Détermine si la commande peut être exécutée.</returns>
        protected virtual bool CanNextPage(object parameter) => true;

        /// <summary>
        /// Méthode d'exécution de la commande <see cref="NextPageCommand"/>.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        protected virtual void NextPage(object search)
        {
            this.Pagination.GoToNextPage();
            SearchMovie();
        }

        #endregion
        
        /// <summary>
        /// Appel la recherche de OmdbAPI
        /// </summary>
        /// <param name="search">Fiml recherché</param>
        private void SearchMovie()
        {
            if (this.Search != "")
            {
                if (this.ItemsSource != null) this.ItemsSource.Clear();
                this.TotalResults = 0;
                int year = (this.Year.Length == 4) ? Convert.ToInt32(this.Year) : 0;
                SearchResult searchResult = OmdbAPI.SearchFilm(this.Search, this.Pagination.IndexPage, year);
                if (searchResult != null)
                {
                    this.ItemsSource = searchResult.Search;
                    this.TotalResults = searchResult.totalResults;
                }
            }
        }

        #endregion
    }
}
