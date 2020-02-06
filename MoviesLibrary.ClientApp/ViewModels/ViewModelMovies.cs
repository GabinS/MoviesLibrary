using Framework.MVVM.Models.Abstracts;
using Framework.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MoviesLibrary.ClientApp.Models;
using MoviesLibrary.ClientApp.ViewModels.Abstracts;
using MoviesLibrary.ClientApp.API;
using System;
using System.Collections.Generic;
using System.Text;
using Framework.MVVM;

namespace MoviesLibrary.ClientApp.ViewModels
{
    /// <summary>
    /// Vue-modèle pour l'affichage des films provenant de l'api OMDb.
    /// </summary>
    public class ViewModelMovies : ViewModelList<SearchResult, IDataContext>, IViewModelMovies
    {
        #region Fields

        /// <summary>
        /// Fournisseur de service de l'application.
        /// </summary>
        private readonly IServiceProvider _ServiceProvider;

        /// <summary>
        /// Commande pour fermer lancer une recherche.
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

        #endregion

        #region Properties

        /// <summary>
        /// Obtient le titre du vue-modèle
        /// </summary>
        public string Title => "Les Films";

        public RelayCommand SearchCommand => this._SearchCommand;
        public RelayCommand PreviousPageCommand => this._PreviousPageCommand;
        public RelayCommand NextPageCommand => this._NextPageCommand;
        public Pagination Pagination { get => this._Pagination; private set => this._Pagination = value; }
        public string Search { get => this._Search; set => this.SetProperty(nameof(this.Search), ref this._Search, value); }
        public string Year { get => this._Year; set => this.SetProperty(nameof(this.Year), ref this._Year, value); }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ViewModelMovies"/>
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de service de l'application.</param>
        public ViewModelMovies(IServiceProvider serviceProvider)
            : base(serviceProvider.GetService<IDataContext>())
        {
            this._SearchCommand = new RelayCommand(this.SearchMovie, this.CanSearch);
            this._PreviousPageCommand = new RelayCommand(this.PreviousPage, this.CanPreviousPage);
            this._NextPageCommand = new RelayCommand(this.NextPage, this.CanNextPage);
            this._Pagination = new Pagination();
            this._Search = "";
            this._Year = "";

            this._ServiceProvider = serviceProvider;
            this.LoadData();
        }

        #endregion

        #region Methods

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
        protected virtual void SearchMovie(object search)
        {
            SearchMovie();
            if (ItemsSource[0] != null)
            {
                int maxPage = (ItemsSource[0].totalResults - ItemsSource[0].totalResults % 10) / 10;
                if (ItemsSource[0].totalResults % 10 != 0) maxPage++;
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

        #region PreviousPageCommand

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
                int year = (this.Year.Length == 4) ? Convert.ToInt32(this.Year): 0;
                this.ItemsSource[0] = OmdbAPI.SearchFilm(this.Search, this.Pagination.IndexPage, year);
            }
        }

        #endregion
    }
}
