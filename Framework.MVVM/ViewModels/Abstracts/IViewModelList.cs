using Framework.MVVM.Abstracts;
using Framework.MVVM.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Framework.MVVM.ViewModels.Abstracts
{
    public interface IViewModelList<T> : IViewModelWithDataContext<T>
        where T : IDataContext
    {
        #region Properties

        /// <summary>
        /// Obtient la commande pour ajouter une personne.
        /// </summary>
        RelayCommand AddCommand { get; }

        /// <summary>
        /// Obtient la commande pour supprimer une personne passée en paramètre ou la personne sélectionnée.
        /// </summary>
        RelayCommand DeleteCommand { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Méthode de chargement des données.
        /// </summary>
        void LoadData();
        
        #endregion
    }
}
