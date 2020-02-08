using Framework.MVVM.Abstracts;
using Framework.MVVM.Models.Abstracts;
using Framework.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MoviesLibrary.ClientApp.ViewModels.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesLibrary.ClientApp.ViewModels
{
    public class ViewModelSettings : ViewModelList<IObservableObject, IDataContext>, IViewModelSettings
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Obtient le titre du vue-modèle
        /// </summary>
        public string Title => "Paramètres";

        #endregion

        #region Constructor

        public ViewModelSettings(IServiceProvider serviceProvider)
            : base(serviceProvider.GetService<IDataContext>())
        {

        }

        public override void LoadData() {}

        #endregion
    }
}
