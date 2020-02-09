# MoviesLibrary

## Introduction

*MoviesLibrary* est une application de recherche de film implémentant une collection personnelle.
Cette application est développé en `WPF`, son architecture repose sur les principes du modèles `MVVM`.

`MoviesLibrary.ClientApp` est une application `WPF net core 3.1`.

## Dépendances

- [Mahapps.Metro](https://github.com/MahApps/MahApps.Metro) : Librairie graphique `WPF`.
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) : Librairie de sérialisation au format `JSON`.
- [Microsoft.Extensions.DependencyInjection](https://github.com/aspnet/Extensions) : Librairie d'injection de dépendance.
- CoursWPF.MVVM : Librairie `MVVM` pour `WPF` développée lors du cours.

## Architecture

L'architecture du projet repose sur les principes du modèles `MVVM`.
Le modèle définit trois couches :

- **Modèle** : Logique métier et données de l'application développée en langage `C#`
- **Vue** : Interface utilisateur développée en langage `XAML` et `C#`
- **Vue-modèle** : Logique de présentation développée en langage `C#`

### Modèle

En théorie, le modèle `MVVM` découpe la partie modèle en trois partie :

- **Modèle de présentation** : Classe de données compatible avec le système de binding. Prend en charge des éléments de présentation comme par exemple des propriétés calculées ou la validation de données.
- **Modèle de données** : Reflète le système de stockage de données.
- **Data Store** : Magasin de données.

#### DataStore de l'application

L'application utilise un fichier de données unique au format `JSON`, il représente le `DataStore`.

#### Modèle de données de l'application

Le modèle de données utilisé par l'application est le suivant :

- **`Movie` : Représente un film**
  - `imdbID` : Identifiant unique d'un film
  - `Title` : Titre d'un film
  - `Year` : Année de parution du film
  - `Poster` : Url du poster du film
- **`MovieDetails` : Représente un film détaillé (comprend la structure de `Movie`)**
  - `Released` : Date de sortie du film
  - `Runtime` : Durée du film
  - `Genre` : Genres du film
  - `Director` : Réalisateur du film
  - `Actors` : Acteurs du film
  - `IsFavorite` : Attribut qui détermine si le film est en favorie
  - `IsView` : Attribut qui détermine si le film à été vue
- **`Pagination` : Représente une structure de pagination**
  - `IndexPage` : Page sélectionnée
  - `PreviousPage` : Page précédente
  - `NextPage` : Page suivante
  - `MaxPage` : Nomber de pages
- **`SearchResult` : Représente une structure de résultat d'une requette de l'api `omdbAPI`**
  - `Search` : Liste des films recherchés
  - `totalResults` : Nombre total de résultat
  - `Response` : Etat de la recherche

#### Implémentation du modèle

La logique `MVVM` voudrait que le modèle de présentation et le modèle logique soit liés avec une dépendance faible.
L'application ne fait pas de réèl distinction entre le modèle de données et le modèle global.

Le modèle de l'application est défini dans le répertoire `.\Models\`.
Le modèle de l'application ne définit pas de couche d'abstraction (interfaces).

#### Implémentation du contexte de données

Le contexte de données est le conteneur des données de l'application.
Le contexte est représenté par la classe `CoursWPF.BankManager.BankManagerContext` qui possède la ligne d'héritage suivante :

```
- `System.Object`
- `CoursWPF.MVVM.ObservableObject` : `CoursWPF.MVVM.Abstracts.IObservableObject` (Objet compatible avec le moteur de `Binding`)
  - `CoursWPF.MVVM.Models.FileDataContext` : `CoursWPF.MVVM.Models.Abstracts.IFileDataContext` (Contexte de données qui prend en charge la sauvegarde dans un fichier unique au format `JSON`)
    - `MoviesLibrary.ClientApp.Models.MoviesLibraryContext` (Contexte de données de l'application)
```

Le contexte contient une collection observable pour chaque entité du modèle de données de l'application :

``` csharp
class MoviesLibraryContext : FileDataContext
{

    #region Fields

    private ObservableCollection<Movie> _SearchMovies;
    private ObservableCollection<MovieDetails> _MyMovies;

    #endregion

    #region Properties

    public ObservableCollection<Movie> SearchMovies { get => this._SearchMovies; private set => this.SetProperty(nameof(this.SearchMovies), ref this._SearchMovies, value); }

    public ObservableCollection<MovieDetails> MyMovies { get => this._MyMovies; private set => this.SetProperty(nameof(this.MyMovies), ref this._MyMovies, value); }

    #endregion

    #region Constructor

    public MoviesLibraryContext(string filePath)
        : base(filePath)
    {
        this._SearchMovies = new ObservableCollection<Movie>();
        this._MyMovies = new ObservableCollection<MovieDetails>();
    }

    #endregion

    #region Methods

    public override T CreateItem<T>()
    {
        IObservableObject createdItem;

        if (typeof(T) == typeof(Movie))
        {
            createdItem = new Movie();
            this.SearchMovies.Add(createdItem as Movie);
        }
        else if (typeof(T) == typeof(MovieDetails))
        {
            createdItem = new MovieDetails();
            this.MyMovies.Add(createdItem as MovieDetails);
        }
        else
        {
            throw new Exception("Le type spécifié n'est pas valide");
        }

        return (T)createdItem;
    }

    public override ObservableCollection<T> GetItems<T>()
    {
        ObservableCollection<T> result = new ObservableCollection<T>();

        if (typeof(T) == typeof(Movie))
        {
            result = this.SearchMovies as ObservableCollection<T>;
        }
        else if (typeof(T) == typeof(MovieDetails))
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
```

### Vue

#### Architecture graphique

La vue est construite avec l'architecture suivante :

``` xml
<MainWindow>    <!--ViewModelMain-->
    <Menu/>
    <TabControl>
      <TabItem>
          <SearchMoviesView/>   <!--ViewModelSearchMovies-->
      </TabItem>
      <TabItem>
          <MyMoviesView/>   <!--ViewModelMyMovies-->
      </TabItem>
  </TabControl>
<MainWindow> />
```

#### Styles

L'application se base principalement sur les styles graphiques définis par `Mahapps`.
Les styles de `Mahapps` sont fusionnés dans le fichier `.\App.xaml`.
L'application définie également sont propre dictionnaire de styles ainsi qu'un dictionnaire de `DataTemplate` (cf. ci-dessous).

``` xml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <!-- MahApps.Metro resource dictionaries. Make sure that all file names are Case Sensitive! -->
                <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml" />
                <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml" />
                <!-- Accent and AppTheme setting -->
                <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.Blue.xaml" />
                <ResourceDictionary Source="Resources/DataTemplates.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
```

#### Fenêtre principale

La fenêtre principale de l'application n'est pas instanciée en définissant la propriété `App.StartupUri` dans le fichier `.\App.xaml`.
L'instanciation est réalisée dans l'événement `App.Startup` implémenté dans le fichier `.App.xaml.cs` :

``` csharp
public partial class App : Application
{
    private string _FilePath = @"C:\Temp\movielibrary.json";
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        //Récupère le language par défaut du support de l'utilisateur
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

        ...

        //Création de l'instance de la fenêtre principale.
        MainWindow window = new MainWindow();
        //Injection du vue-modèle de la fenêtre.
        window.DataContext = serviceProvider.GetService<IViewModelMain>();
        //Affichage de la fenêtre principale.
        window.Show();
    }
}
```

Dans sa structure, la fenêtre principale contient un `TabControl` dont la source de données est liées par `Binding` au vue-modèle principal :

``` xml
<TabControl Grid.Row="1" ItemsSource="{Binding ItemsSource}" SelectedItem="{Binding SelectedItem}">
  <TabControl.ItemTemplate>
    <DataTemplate>
      <TextBlock Text="{Binding Title}"/>
    </DataTemplate>
  </TabControl.ItemTemplate>
</TabControl>
```

#### Liaison entre les vues et les vues-modèles

Le choix du `DataTemplate` utilisé pour représenter chaque vue-modèle est réalisé dans le dictionnaire de ressource `.\Resources\DataTemplates.xaml` :
``` xml
<DataTemplate DataType="{x:Type viewModels:ViewModelSearchMovies}">
    <local:SearchMoviesView/>
</DataTemplate>

<DataTemplate DataType="{x:Type viewModels:ViewModelMyMovies}">
    <local:MyMoviesView/>
</DataTemplate>
```

#### Vue `SearchMoviesView`

Cette vue permet de naviguer dans une collection de films récupée par un système de recherche.
Le context de donnée utilisé est `ViewModelSearchMovies`

#### Vue `MyMoviesView`

Chaque vue-modèle est représenté par un contrôle utilisateur définis dans le dossier `.\Views\`.

### Vue-modèle

## Conclusion
