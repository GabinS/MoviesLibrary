using Framework.MVVM.Models.Abstracts;
using Framework.MVVM.ViewModels.Abstracts;
using MoviesLibrary.ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesLibrary.ClientApp.ViewModels.Abstracts
{
    public interface IViewModelMovies : IViewModelList<SearchResult, IDataContext>
    {

    }
}
