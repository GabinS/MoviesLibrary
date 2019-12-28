using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Framework.MVVM
{
    public  class RelayCommand : ICommand
    {
        #region Events

        /// <summary>
        /// Déclenché automatiquement lorsque <see cref="CommandManager.RequerySuggested"/> est déclenché.
        /// Permet de dire aux éléments de l'interface graphique qu'ils doivent évaluer si la méthode 
        /// <see cref="Execute(object)"/> peut être appelée en appelant <see cref="CanExecute(object)"/>.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Méthode à rappeler lorsque la méthode <see cref="RelayCommand.Execute(object)"/> est appelée.
        /// </summary>
        private readonly Action<object> _Execute;

        /// <summary>
        /// Méthode à rappeler lorsque la méthode <see cref="RelayCommand.CanExecute(object)"/> est appelée.
        /// </summary>
        private readonly Func<object, bool> _CanExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Méthode à rappeler pour exécuter la commande.</param>
        /// <param name="canExecute">Méthode à rappeler pour vérifier si la commande peut être exécutée.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this._Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._CanExecute = canExecute;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Détermine si la commande peut être exécutée.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        /// <returns>Détermine si la commande peut être exécutée.</returns>
        public bool CanExecute(object parameter)
        {
            return this._CanExecute?.Invoke(parameter) ?? true;
        }

        /// <summary>
        /// Méthode d'exécution de la commande.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande</param>
        public void Execute(object parameter)
        {
            this._Execute(parameter);
        }

        #endregion
    }
}
