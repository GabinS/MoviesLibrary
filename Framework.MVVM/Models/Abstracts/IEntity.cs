using Framework.MVVM.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Framework.MVVM.Models
{
    /// <summary>
    /// Interface d'une entité avec un identifiant.
    /// </summary>
    public interface IEntity : IObservableObject, IEditableObject
    {
        #region Properties

        /// <summary>
        /// Obtient ou définit l'identifiant de l'élément.
        /// </summary>
        long Identifier { get; set; }

        #endregion
    }
}
