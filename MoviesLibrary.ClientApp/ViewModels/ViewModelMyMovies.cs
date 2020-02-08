using Framework.MVVM.Models.Abstracts;
using Framework.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MoviesLibrary.ClientApp.API;
using MoviesLibrary.ClientApp.Models;
using MoviesLibrary.ClientApp.ViewModels.Abstracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MoviesLibrary.ClientApp.ViewModels
{
    /// <summary>
    /// Vue-modèle pour l'affichage des films de ma collection.
    /// </summary>
    public class ViewModelMyMovies : ViewModelList<MovieDetails, IDataContext>, IViewModelMyMovies
    {
        #region Properties

        /// <summary>
        /// Obtient le titre du vue-modèle
        /// </summary>
        public string Title => "Ma Collection";

        #endregion
        
        #region Constructor

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ViewModelMyMovies"/>
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de service de l'application.</param>
        public ViewModelMyMovies(IDataContext dataContext)
            : base(dataContext)
        {
            this.LoadData();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Déclenche l'événement <see cref="PropertyChanged"/>.
        /// </summary>
        /// <param name="propertyName">Nom de la propriété qui a changée.</param>

        #region AddCommand

        /// <summary>
        ///  Méthode d'exécution de la commande <see cref="AddCommand"/>.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        protected override void Add(object parameter)
        {
            MovieDetails movie = OmdbAPI.GetFilm((parameter as Movie).imdbID);
            bool exist = false;
            foreach (var m in this.DataContext.GetItems<MovieDetails>())
            {
                if (m.imdbID == movie.imdbID)
                {
                    exist = true;
                    break;
                }
            }
            if (!exist)
            {
                this.DataContext.GetItems<MovieDetails>().Insert(0, movie as MovieDetails);
                this.DataContext.Save();
            }
        }

        #endregion

        #endregion
    }
}
