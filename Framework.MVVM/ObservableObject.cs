using Framework.MVVM.Abstracts;
using System;
using System.ComponentModel;

namespace Framework.MVVM
{
    public class ObservableObject : IObservableObject
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Methods

        /// <summary>
        /// Déclenche l'événement <see cref="PropertyChanging"/>.
        /// </summary>
        /// <param name="propertyName">Nom de la propriété qui va changer.</param>
        protected virtual void OnPropertyChanging(string propertyName)
        {
            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Déclenche l'événement <see cref="PropertyChanged"/>.
        /// </summary>
        /// <param name="propertyName">Nom de la propriété qui a changée.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Modifie la valeur d'un attribut et déclenche les événements <see cref="PropertyChanging"/> et <see cref="PropertyChanged"/>.
        /// </summary>
        /// <typeparam name="T">Type de l'attribut.</typeparam>
        /// <param name="propertyName">Nom de la propriété associé à l'attribut.</param>
        /// <param name="field">Référence vers l'attribut à modifier.</param>
        /// <param name="value">Nouvelle valeur de l'attribut.</param>
        protected void SetProperty<T>(string propertyName, ref T field, T value)
        {
            if ((field == null && value != null) || field?.Equals(value) == false)
            {
                this.OnPropertyChanging(propertyName);
                field = value;
                this.OnPropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Modifie la valeur d'un attribut et déclenche les événements <see cref="PropertyChanging"/> et <see cref="PropertyChanged"/>.
        /// </summary>
        /// <typeparam name="T">Type de l'attribut.</typeparam>
        /// <param name="propertyName">Nom de la propriété associé à l'attribut.</param>
        /// <param name="field">Référence vers l'attribut à modifier.</param>
        /// <param name="value">Nouvelle valeur de l'attribut.</param>
        protected void SetProperty<T>(string propertyName, Func<T> get, Action<T> set, T value)
        {
            if ((get() == null && value != null) || get()?.Equals(value) == false)
            {
                this.OnPropertyChanging(propertyName);
                set(value);
                this.OnPropertyChanged(propertyName);
            }
        }

        #endregion
    }
}
