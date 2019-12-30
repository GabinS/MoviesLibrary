using Framework.MVVM.Models.Abstracts;
using Framework.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MoviesLibrary.ClientApp.Models;
using MoviesLibrary.ClientApp.ViewModels.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesLibrary.ClientApp.ViewModels
{
    /// <summary>
    /// Vue-modèle pour l'affichage des films provenant de l'api OMDb.
    /// </summary>
    public class ViewModelMovies : ViewModelList<Movie, IDataContext>, IViewModelMovies
    {
        #region Fields

        /// <summary>
        /// Fournisseur de service de l'application.
        /// </summary>
        private readonly IServiceProvider _ServiceProvider;

        #endregion


        #region Properties

        /// <summary>
        /// Obtient le titre du vue-modèle
        /// </summary>
        public string Title => "Les Films";

        #endregion

        #region Constructor

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ViewModelMovies"/>
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de service de l'application.</param>
        public ViewModelMovies(IServiceProvider serviceProvider)
            : base(serviceProvider.GetService<IDataContext>())
        {
            this._ServiceProvider = serviceProvider;
            this.LoadData();
        }

        #endregion
    }
}
