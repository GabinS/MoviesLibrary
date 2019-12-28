using Framework.MVVM;
using Framework.MVVM.Abstracts;
using Framework.MVVM.Models.Abstracts;
using Framework.MVVM.ViewModels.Abstracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Framework.MVVM.ViewModels
{
    /// <summary>
    /// Vue-modèle de liste dans un contexte de données.
    /// </summary>
    public class ViewModelList<T, U> : ViewModelWithDataContext<U>, IViewModelList<T, U>
        where T : IObservableObject
        where U : IDataContext
    {
        #region Fields

        /// <summary>
        /// Liste des éléments.
        /// </summary>
        private ObservableCollection<T> _ItemsSource;

        /// <summary>
        /// Elément sélectionné.
        /// </summary>
        private T _SelectedItem;

        /// <summary>
        /// Commande pour ajouter un élément.
        /// </summary>
        private readonly RelayCommand _AddCommand;

        /// <summary>
        /// Commande pour supprimer un élément passé en paramètre ou l'élément sélectionné.
        /// </summary>
        private readonly RelayCommand _DeleteCommand;

        #endregion

        #region Properties

        /// <summary>
        /// Obtient la liste des éléments.
        /// </summary>
        public ObservableCollection<T> ItemsSource 
        { 
            get => this._ItemsSource;
            protected set => this.SetProperty(nameof(this.ItemsSource), ref this._ItemsSource, value); 
        }

        /// <summary>
        /// Obtient ou définit l'élément sélectionné.
        /// </summary>
        public T SelectedItem
        {
            get => this._SelectedItem;
            set => this.SetProperty(nameof(this.SelectedItem), ref this._SelectedItem, value);
        }

        /// <summary>
        /// Obtient la commande pour ajouter un élément.
        /// </summary>
        public virtual RelayCommand AddCommand => this._AddCommand;

        /// <summary>
        /// Obtient la commande pour supprimer un élément passé en paramètre ou l'élément sélectionné.
        /// </summary>
        public virtual RelayCommand DeleteCommand => this._DeleteCommand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ViewModelListT"/>.
        /// </summary>
        /// <param name="dataContext">Contexte de données.</param>
        public ViewModelList(U dataContext)
            : base(dataContext)
        {
            this._AddCommand = new RelayCommand(this.Add, this.CanAdd);
            this._DeleteCommand = new RelayCommand(this.Delete, this.CanDelete);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Méthode de chargement des données.
        /// </summary>
        public override void LoadData()
        {
            base.LoadData();
            this.ItemsSource = new ObservableCollection<T>(this.DataContext.GetItems<T>());
        }

        #region AddCommand

        /// <summary>
        /// Méthode d'exécution de la commande <see cref="AddCommand"/>.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        protected virtual void Add(object parameter)
        {
            T itemToAdd = this.DataContext.CreateItem<T>();
            this.ItemsSource.Insert(0, itemToAdd);
            this.SelectedItem = itemToAdd;
        }

        /// <summary>
        /// Methode qui détermine si la commande <see cref="AddCommand"/> peut être exécutée.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        /// <returns>Détermine si la commande peut être exécutée.</returns>
        protected virtual bool CanAdd(object parameter) => true;

        #endregion

        #region DeleteCommand

        /// <summary>
        /// Méthode d'exécution de la commande <see cref="DeleteCommand"/>.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        protected virtual void Delete(object parameter)
        {
            T itemToDelete = (T)parameter ?? this.SelectedItem;

            if (itemToDelete != null)
            {
                this.ItemsSource.Remove(itemToDelete);
                this.DataContext.GetItems<T>().Remove(itemToDelete);
            }
        }

        /// <summary>
        /// Methode qui détermine si la commande <see cref="DeleteCommand"/> peut être exécutée.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        /// <returns>Détermine si la commande peut être exécutée.</returns>
        protected virtual bool CanDelete(object parameter) => parameter is T || this._SelectedItem != null;

        #endregion

        #endregion
    }
}
