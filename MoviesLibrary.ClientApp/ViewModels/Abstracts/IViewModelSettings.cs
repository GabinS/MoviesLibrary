using Framework.MVVM.Abstracts;
using Framework.MVVM.Models.Abstracts;
using Framework.MVVM.ViewModels.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesLibrary.ClientApp.ViewModels.Abstracts
{
    public interface IViewModelSettings : IViewModelList<IObservableObject, IDataContext>
    {

    }
}
