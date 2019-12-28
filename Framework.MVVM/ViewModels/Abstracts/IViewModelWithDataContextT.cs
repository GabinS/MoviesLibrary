using Framework.MVVM.Abstracts;
using Framework.MVVM.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Framework.MVVM.ViewModels.Abstracts
{
    /// <summary>
    /// Interface d'un vue-modèle avec un contexte de données.
    /// </summary>
    /// <typeparam name="T">Type du contexte de données.</typeparam>
    public interface IViewModelWithDataContext<T> : IObservableObject
        where T : IDataContext
    {
        #region Properties

        /// <summary>
        /// Obtient le contexte de données.
        /// </summary>
        IDataContext DataContext { get; }

        /// <summary>
        /// Obtient la commande pour sauvegarder les données.
        /// </summary>
        RelayCommand SaveCommand { get; }

        #endregion
    }
}
