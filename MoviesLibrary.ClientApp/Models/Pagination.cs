using Framework.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesLibrary.ClientApp.Models
{
    public class Pagination : ObservableObject
    {
        #region Fields

        /// <summary>
        /// Page précédente
        /// </summary>
        private int _IndexPage;
        /// <summary>
        /// Page suivante
        /// </summary>
        private int _PreviousPage;
        /// <summary>
        /// Page sélectionnée
        /// </summary>
        private int _NextPage;
        /// <summary>
        /// Nombre de page
        /// </summary>
        private int _MaxPage;

        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou défini la page précédente
        /// </summary>
        public int IndexPage { get => this._IndexPage; set => this.SetProperty(nameof(this.IndexPage), ref this._IndexPage, value); }
        /// <summary>
        /// Obtient ou défini la page suivante
        /// </summary>
        public int PreviousPage { get => this._PreviousPage; set => this.SetProperty(nameof(this.PreviousPage), ref this._PreviousPage, value); }
        /// <summary>
        /// Obtient ou défini la page sélectionnée
        /// </summary>
        public int NextPage { get => this._NextPage; set => this.SetProperty(nameof(this.NextPage), ref this._NextPage, value); }
        /// <summary>
        /// Obtient ou défini le nombre de page
        /// </summary>
        public int MaxPage { get => this._MaxPage; set => this.SetProperty(nameof(this.MaxPage), ref this._MaxPage, value); }

        #endregion

        #region Constructor

        public Pagination() {
            this._IndexPage = 1;
            this._PreviousPage = 1;
            this._NextPage = 1;
            this._MaxPage = 1;
        }

        /// <summary>
        /// Rafraichi la pagination
        /// </summary>
        /// <param name="indexPage">Numéro de page sélectionnée</param>
        /// <param name="maxPage">Nombre de page max</param>
        public void Refresh(int indexPage, int maxPage = 0)
        {
            this.IndexPage = indexPage;
            this.MaxPage = maxPage != 0 ? maxPage : this.MaxPage;
            this.PreviousPage = (this.IndexPage > 1) ? this.IndexPage - 1 : 1;
            this.NextPage = (this.IndexPage < this.MaxPage) ? this.IndexPage + 1 : this.MaxPage;
        }

        /// <summary>
        /// Change la page sélectionée vers la page précédente
        /// </summary>
        public void GoToPreviousPage()
        {
            Refresh(this.PreviousPage);
        }
        /// <summary>
        /// Change la page sélectionée vers la page suivante
        /// </summary>
        public void GoToNextPage()
        {
            Refresh(this.NextPage);
        }

        #endregion
    }
}
