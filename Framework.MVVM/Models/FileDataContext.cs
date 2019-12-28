using Framework.MVVM.Abstracts;
using Framework.MVVM.Models.Abstracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Framework.MVVM.Models
{
    /// <summary>
    /// Classe abstraite pour la gestion d'un contexte de données avec sauvegarde dans un unique fichier au format json.
    /// </summary>
    public abstract class FileDataContext : ObservableObject, IFileDataContext
    {
        #region Fiels

        /// <summary>
        /// Chemin du fichier de données.
        /// </summary>
        private string _FilePath;

        #endregion

        #region Properties

        /// <summary>
        /// Obtient le chemin du fichier de données.
        /// </summary>
        public string FilePath { get => this._FilePath; private set => this.SetProperty(nameof(this.FilePath), ref this._FilePath, value); }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="FileDataContext"/>.
        /// </summary>
        /// <param name="filePath">Chemin du fichier de données.</param>
        public FileDataContext(string filePath)
        {
            this._FilePath = filePath;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Créer un élément du type spécifié et l'ajoute au contexte de données.
        /// </summary>
        /// <typeparam name="T">Type de l'élément à créer.</typeparam>
        /// <returns>Retourne un nouvel élément du type spécifié.</returns>
        public abstract T CreateItem<T>()
            where T : IObservableObject;

        /// <summary>
        /// Obtient la collection des éléments du type spécifié.
        /// </summary>
        /// <typeparam name="T">Type de l'élément de la collection.</typeparam>
        /// <returns>Collection des éléments du type spécifié.</returns>
        public abstract ObservableCollection<T> GetItems<T>()
            where T : IObservableObject;

        /// <summary>
        /// Sauvegarde le contexte dans un fichier.
        /// </summary>
        public virtual void Save()
        {
            File.WriteAllText(this.FilePath, JsonConvert.SerializeObject(this));
        }

        /// <summary>
        /// Charge le contexte de données depuis le chemin spécifié ou retourne le contexte par défaut.
        /// </summary>
        /// <typeparam name="T">Type du contexte de données.</typeparam>
        /// <param name="filePath">Chemin du fichier de données.</param>
        /// <param name="defaultContext">Instance à utiliser si le chemin ne peut pas être ouvert.</param>
        /// <returns>Instance du contexte de données.</returns>
        public static T Load<T>(string filePath, T defaultContext)
            where T : FileDataContext
        {
            T dataContext;

            try
            {
                dataContext = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
            }
            catch
            {
                dataContext = defaultContext;
            }

            dataContext.FilePath = filePath;

            return dataContext;
        }

        /// <summary>
        /// Détermine si le contexte de données peut être sauvegardé.
        /// </summary>
        /// <returns>Détermine si le contexte de données peut être sauvegardé.</returns>
        public virtual bool CanSave() => true;

        #endregion
    }
}
