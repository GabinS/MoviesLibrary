using Framework.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.MVVM.Models
{
    /// <summary>
    /// Représente une entité avec un identifiant.
    /// </summary>
    public abstract class Entity : ObservableObject, IEntity
    {
        #region Fields

        /// <summary>
        /// Identifiant de l'entité.
        /// </summary>
        private long _Identifier;

        #endregion

        #region Properties

        /// <summary>
        /// Obtient ou définit l'identifiant de l'élément.
        /// </summary>
        public long Identifier { get => this._Identifier; set => this.SetProperty(nameof(this.Identifier), ref this._Identifier, value); }

        /// <summary>
        /// Commence l'édition d'une entité.
        /// </summary>
        public abstract void BeginEdit();

        /// <summary>
        /// Annule les modifications effectuées sur l'entité.
        /// </summary>
        public abstract void CancelEdit();

        /// <summary>
        /// Valide les modifications effectuées sur l'entité.
        /// </summary>
        public abstract void EndEdit();

        #endregion
    }
}
