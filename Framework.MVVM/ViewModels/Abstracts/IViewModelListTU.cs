using Framework.MVVM.Abstracts;
using Framework.MVVM.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Framework.MVVM.ViewModels.Abstracts
{
    public interface IViewModelList<T, U> : IViewModelList<U>
        where T : IObservableObject
        where U : IDataContext
    {
        #region Properties

        /// <summary>
        /// Obtient la liste des personnes.
        /// </summary>
        ObservableCollection<T> ItemsSource { get; }

        /// <summary>
        /// Obtient ou définit la personne sélectionnée.
        /// </summary>
        T SelectedItem { get; set; }

        #endregion
    }
}
