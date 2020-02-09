using Framework.MVVM;
using Framework.MVVM.Models.Abstracts;
using Framework.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MoviesLibrary.ClientApp.API;
using MoviesLibrary.ClientApp.Models;
using MoviesLibrary.ClientApp.ViewModels.Abstracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MoviesLibrary.ClientApp.ViewModels
{
    /// <summary>
    /// Vue-modèle pour l'affichage des films de ma collection.
    /// </summary>
    public class ViewModelMyMovies : ViewModelList<MovieDetails, IDataContext>, IViewModelMyMovies
    {
        #region Fields

        /// <summary>
        /// Recherche
        /// </summary>
        private string _Search;

        /// <summary>
        /// Commande pour lancer une recherche.
        /// </summary>
        private readonly RelayCommand _SearchCommand;

        #endregion

        #region Properties

        /// <summary>
        /// Obtient le titre du vue-modèle
        /// </summary>
        public string Title => "Ma Collection";

        public string Search { get => this._Search; set => this.SetProperty(nameof(this.Search), ref this._Search, value); }
        public RelayCommand SearchCommand => this._SearchCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ViewModelMyMovies"/>
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de service de l'application.</param>
        public ViewModelMyMovies(IDataContext dataContext)
            : base(dataContext)
        {
            this._SearchCommand = new RelayCommand(this.SearchMovie, this.CanSearch);
            this._Search = "";
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
        protected virtual void SearchMovie(object parameter)
        {
            this.ItemsSource = this.DataContext.GetItems<MovieDetails>();
            if (parameter != null && parameter.ToString() != "" && this.ItemsSource != null)
            {
                ObservableCollection<MovieDetails> moviesSearch = new ObservableCollection<MovieDetails>();
                this.DataContext.GetItems<MovieDetails>().ToList().ForEach(m =>
                {
                    if (m.Title.ToLower().Contains(parameter.ToString().ToLower())) moviesSearch.Add(m);
                });
                this.ItemsSource = moviesSearch;
            }
        }

        #endregion

        #region AddCommand

        /// <summary>
        ///  Méthode d'exécution de la commande <see cref="AddCommand"/>.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        protected override void Add(object parameter)
        {
            MovieDetails movie = OmdbAPI.GetFilm((parameter as Movie).imdbID);            
            if (this.DataContext.GetItems<MovieDetails>().FirstOrDefault(m => m.imdbID == movie.imdbID) == null)
            {
                this.DataContext.GetItems<MovieDetails>().Insert(0, movie as MovieDetails);
                this.DataContext.Save();
            }
        }

        #endregion

        #endregion
    }
}
