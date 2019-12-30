using Framework.MVVM.Abstracts;
using Framework.MVVM.Models.Abstracts;
using Framework.MVVM.ViewModels.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesLibrary.ClientApp.ViewModels.Abstracts
{
    public interface IViewModelMain : IViewModelList<IObservableObject, IDataContext>
    {
        #region Properties

        /// <summary>
        /// Obtient le vue-modèle de la page de recheche de films.
        /// </summary>
        public IViewModelMovies ViewModelMovies  { get; }

        /// <summary>
        /// Obtient le vue-modèle de la page de ma collection de films.
        /// </summary>
        public IViewModelMyMovies ViewModelMyMovies { get; }

        #endregion
    }
}
