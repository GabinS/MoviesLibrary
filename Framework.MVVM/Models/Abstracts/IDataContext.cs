using Framework.MVVM.Abstracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Framework.MVVM.Models.Abstracts
{
    public interface IDataContext : IObservableObject
    {
        #region Methods

        /// <summary>
        /// Détermine si le contexte de données peut être sauvegardé.
        /// </summary>
        /// <returns>Détermine si le contexte de données peut être sauvegardé.</returns>
        bool CanSave();

        /// <summary>
        /// Sauvegarde le contexte de données.
        /// </summary>
        void Save();

        /// <summary>
        /// Créer un élément du type spécifié et l'ajoute au contexte de données.
        /// </summary>
        /// <typeparam name="T">Type de l'élément à créer.</typeparam>
        /// <returns>Retourne un nouvel élément du type spécifié.</returns>
        T CreateItem<T>()
            where T : IObservableObject;

        /// <summary>
        /// Obtient la collection des éléments du type spécifié.
        /// </summary>
        /// <typeparam name="T">Type de l'élément de la collection.</typeparam>
        /// <returns>Collection des éléments du type spécifié.</returns>
        ObservableCollection<T> GetItems<T>()
            where T : IObservableObject;

        #endregion
    }
}
