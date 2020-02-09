using Framework.MVVM;
using Framework.MVVM.Abstracts;
using Framework.MVVM.Models.Abstracts;
using Framework.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Framework.MVVM.ViewModels.Abstracts;
using MoviesLibrary.ClientApp.ViewModels.Abstracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MoviesLibrary.ClientApp.API;

namespace MoviesLibrary.ClientApp.ViewModels
{
    public class ViewModelMain : ViewModelList<IObservableObject, IDataContext>, IViewModelMain
    {
        #region Fields

        /// <summary>
        /// Fournisseur de service de l'application.
        /// </summary>
        private readonly IServiceProvider _ServiceProvider;

        /// <summary>
        /// Vue-modèle de la page de recherche de films.
        /// </summary>
        private IViewModelMovies _ViewModelMovies;

        /// <summary>
        /// Vue-modèle de la page de ma collection de films.
        /// </summary>
        private IViewModelMyMovies _ViewModelMyMovies;

        /// <summary>
        /// Commande pour fermer l'application.
        /// </summary>
        private readonly RelayCommand _ExitCommand;

        #endregion

        #region Properties

        public IViewModelMovies ViewModelMovies { get => this._ViewModelMovies; private set => this.SetProperty(nameof(this.ViewModelMovies), ref this._ViewModelMovies, value); }

        public IViewModelMyMovies ViewModelMyMovies { get => this._ViewModelMyMovies; private set => this.SetProperty(nameof(this.ViewModelMyMovies), ref this._ViewModelMyMovies, value); }

        /// <summary>
        /// Obtient la commande pour fermer l'application.
        /// </summary>
        public RelayCommand ExitCommand => this._ExitCommand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ViewModelMain"/>.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de service de l'application.</param>
        public ViewModelMain(IServiceProvider serviceProvider)
            : base(serviceProvider.GetService<IDataContext>())
        {
            this._ServiceProvider = serviceProvider;

            this._ViewModelMovies = this._ServiceProvider.GetService<IViewModelMovies>();
            this._ViewModelMyMovies = this._ServiceProvider.GetService<IViewModelMyMovies>();

            this._ExitCommand = new RelayCommand(this.Exit, this.CanExit);
            this.LoadData();
        }

        #endregion

        #region Methods

        public override void LoadData()
        {
            this.ItemsSource = new ObservableCollection<IObservableObject>(new IObservableObject[] {  this._ViewModelMovies, this._ViewModelMyMovies });
            this.SelectedItem = this._ViewModelMovies; // vue-model sélectionné par défaut.
        }

        /// <summary>
        /// Déclenche l'événement <see cref="PropertyChanged"/>.
        /// </summary>
        /// <param name="propertyName">Nom de la propriété qui a changée.</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(this.SelectedItem):
                    (this.SelectedItem as IViewModelList<IDataContext>)?.LoadData();
                    break;
                default:
                    break;
            }
        }

        #region ExitCommand

        /// <summary>
        /// Methode qui détermine si la commande <see cref="ExitCommand"/> peut être exécutée.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        /// <returns>Détermine si la commande peut être exécutée.</returns>
        protected virtual bool CanExit(object parameter) => true;

        /// <summary>
        /// Méthode d'exécution de la commande <see cref="ExitCommand"/>.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        protected virtual void Exit(object parameter)
        {
            App.Current.Shutdown(0);
        }

        #endregion

        #endregion
    }
}
