using Framework.MVVM.Models.Abstracts;
using Framework.MVVM.ViewModels.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.MVVM.ViewModels
{
    /// <summary>
    /// Vue-modèle avec un contexte de données.
    /// </summary>
    /// <typeparam name="T">Type du contexte de données.</typeparam>
    public class ViewModelWithDataContext<T> : ObservableObject, IViewModelWithDataContext<T>
        where T : IDataContext
    {
        #region Fields

        /// <summary>
        /// Contexte de données
        /// </summary>
        private IDataContext _DataContext;

        /// <summary>
        /// Commande pour sauvegarder les données.
        /// </summary>
        private readonly RelayCommand _SaveCommand;

        #endregion

        #region Properties

        /// <summary>
        /// Obtient le contexte de données.
        /// </summary>
        public IDataContext DataContext { get => this._DataContext; private set => this.SetProperty(nameof(this.DataContext), ref this._DataContext, value); }

        /// <summary>
        /// Obtient la commande pour sauvegarder les données.
        /// </summary>
        public RelayCommand SaveCommand => this._SaveCommand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ViewModelWithDataContext{T}"/>.
        /// </summary>
        /// <param name="dataContext">Contexte de données.</param>
        public ViewModelWithDataContext(IDataContext dataContext)
        {
            this._DataContext = dataContext;
            this._SaveCommand = new RelayCommand(this.Save, this.CanSave);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Méthode de chargement des données.
        /// </summary>
        public virtual void LoadData()
        {
            
        }

        #region SaveCommand

        /// <summary>
        /// Methode qui détermine si la commande <see cref="SaveCommand"/> peut être exécutée.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        /// <returns>Détermine si la commande peut être exécutée.</returns>
        protected virtual bool CanSave(object parameter) => this.DataContext.CanSave();

        /// <summary>
        /// Méthode d'exécution de la commande <see cref="SaveCommand"/>.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        protected virtual void Save(object parameter)
        {
            this.DataContext.Save();
        }

        #endregion

        #endregion
    }
}
