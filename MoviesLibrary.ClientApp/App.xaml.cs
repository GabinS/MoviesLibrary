using Framework.MVVM.Models;
using Framework.MVVM.Models.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using MoviesLibrary.ClientApp.Models;
using MoviesLibrary.ClientApp.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace MoviesLibrary.ClientApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string _FilePath = @"C:\Temp\movielibrary.json";
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Récupère le language par défaut du support de l'utilisateur
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            ServiceCollection serviceCollection = new ServiceCollection();

            //Création du contexte de données de l'application.
            serviceCollection.AddSingleton<IDataContext, MoviesLibraryContext>(sp => FileDataContext.Load(this._FilePath, new MoviesLibraryContext(this._FilePath)));

            //TODO Création des vue-modèle.


            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            MainWindow window = new MainWindow();

            //TODO Affectation du context à la fenêtre principale.


            window.Show();
        }
    }
}
