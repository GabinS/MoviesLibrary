using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.MVVM.Models.Abstracts
{
    public interface IFileDataContext : IDataContext
    {
        #region Properties

        /// <summary>
        /// Obtient le chemin du fichier de données.
        /// </summary>
        string FilePath { get; }

        #endregion
    }
}
