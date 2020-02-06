using Framework.MVVM.Models;
using MoviesLibrary.ClientApp.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MoviesLibrary.ClientApp.Models
{
    /// <summary>
    /// Contexte de données de l'application.
    /// </summary>
    class MoviesLibraryContext : FileDataContext
    {

        #region Fields

        /// <summary>
        /// Liste des films d'une recherche
        /// </summary>
        private ObservableCollection<SearchResult> _SearchMovies;
        /// <summary>
        /// Liste des films de ma collection
        /// </summary>
        private ObservableCollection<Movie> _MyMovies;

        #endregion

        #region Properties

        /// <summary>
        /// Obtient la liste des films d'une recherche
        /// </summary>
        public ObservableCollection<SearchResult> SearchMovies { get => this._SearchMovies; private set => this.SetProperty(nameof(this.SearchMovies), ref this._SearchMovies, value); }
        /// <summary>
        /// Obtient la liste des films de ma collection
        /// </summary>
        public ObservableCollection<Movie> MyMovies { get => this._MyMovies; private set => this.SetProperty(nameof(this.MyMovies), ref this._MyMovies, value); }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="MoviesLibraryContext"/>
        /// </summary>
        /// <param name="filePath">Chemin du fichier de données.</param>
        public MoviesLibraryContext(string filePath)
            : base(filePath)
        {
            this._SearchMovies = new ObservableCollection<SearchResult>(){ OmdbAPI.SearchFilm("Harry Potter")};
            //this._SearchMovies = new ObservableCollection<Movie>();
            this._MyMovies = new ObservableCollection<Movie>();
        }

        #endregion

        #region Methods

        public override T CreateItem<T>()
        {
            throw new NotImplementedException();
        }

        public override ObservableCollection<T> GetItems<T>()
        {
            ObservableCollection<T> result;

            if (typeof(T) == typeof(SearchResult))
            {
                result = this.SearchMovies as ObservableCollection<T>;
            }
            else if (typeof(T) == typeof(Movie))
            {
                result = this.MyMovies as ObservableCollection<T>;
            }
            else
            {
                throw new Exception("Le type spécifié n'est pas valide");
            }

            return result;
        }

        #endregion
    }
}
