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

#### Requêtage d'une API

Pour récupéré les données qui serviront à la page de recherche de film on requête l'API `omdbApi` (https://www.omdbapi.com).
La classe `MoviesLibrary.ClientApp.API.OmdbAPI` permet de requêter cette API grâce à une clé enregistrer dans la classe `MoviesLibrary.ClientApp.API.APIKey`.
C'est aussi avec ces classe que l'on récupère les détails d'un film.

``` csharp
public class OmdbAPI
{
    static HttpClient client = new HttpClient();
    private static string baseUrl = $"http://www.omdbapi.com?apikey={APIKey.GetAPIKey}&type=movie";

    // Recherche une liste de film par titre
    public static SearchResult SearchFilm(string search, int page = 1, int year = 0)
    {
        string stringYear = year != 0 ? $"&y={year}" : "";
        string url = $"{baseUrl}&s={search}&page={page}{stringYear}";
        using (var client = new WebClient())
        {
            string responce = client.DownloadString(url);
            SearchResult result = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchResult>(responce);

            if (result.Response) return result;
        }
        return null;
    }

    // Récupère un film détaillé par son identifiant
    public static MovieDetails GetFilm(string imdbID)
    {
        string url = $"{baseUrl}&i={imdbID}";
        using (var client = new WebClient())
        {
            string responce = client.DownloadString(url);
            MovieDetails result = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieDetails>(responce);

            if (result != null) return result;
        }
        return null;
    }
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

Chaque vue-modèle est représenté par un contrôle utilisateur définis dans le dossier `.\Views\`.

#### Vue `SearchMoviesView`

Cette vue permet de naviguer dans une collection de films récupée par un système de recherche.
Le context de donnée utilisé est `ViewModelSearchMovies`.

La barre de recherche, en haut du contenu du `TabControl`, est constituée d'un `TextBox` pour le titre, d'un autre pour préciser la recherche avec une date et d'un `Button` branchés par `Binding` sur la commande `SearchCommand` pour déclencher la recherche.

``` csharp
<!--#region Search Bar -->
<Grid Grid.Row="0" Background="{DynamicResource MahApps.Brushes.Accent2}">
    <Grid Margin="0,10">
        <Grid.ColumnDefinitions>
            ...
        </Grid.ColumnDefinitions>

        <StackPanel Grid.Column="1" Orientation="Horizontal">
            <TextBlock Text="Nom du film" Margin="5,0" FontSize="16" VerticalAlignment="Center"/>
            <TextBox Name="SearchBox" Text="{Binding Search, UpdateSourceTrigger=PropertyChanged}"
                    VerticalAlignment="Center" Padding="10" MinWidth="300" FontSize="16"/>
        </StackPanel>

        <StackPanel Grid.Column="3" Orientation="Horizontal">
            <TextBlock Text="Date" Margin="10,0" FontSize="16" VerticalAlignment="Center"/>
            <TextBox MaxLength="4" Text="{Binding Year, UpdateSourceTrigger=PropertyChanged}" MinWidth="150"
                        FontSize="16" VerticalContentAlignment="Center" PreviewTextInput="NumberValidationTextBox"/>
        </StackPanel>
        <Button Grid.Column="5" Content="Rechercher un film" Padding="50,10" Command="{Binding SearchCommand}"/>
    </Grid>
</Grid>
<!--#endregion-->
```

Au centre de la vue un `ItemsControl` permet de lister la collection de film récupérée avec la recherche.
Chaque film de la collection sont affichés par block qui sont constitués de l'image du poster associé, d'un titre, d'une année et d'un `Button` branchés par `Binding` sur la commande `AddCommand` permettant d'ajouter le film à la collection personnelle.
L'affichage de la collection en générale est accompagné par un `WrapPanel` pour un visuel plus ergonomique.

Si aucune recherche n'a encore été faite le message "Faite une recherche de film" apparait avec un `TextBlock` et si il n'y a pas de résultat après une recherche c'est le message "Aucun résultat" qui apparait.

``` csharp
<!--#region Liste Films -->
<Grid Grid.Row="1">

    <!--#region Liste vide -->
    <TextBlock VerticalAlignment="Center" HorizontalAlignment="Center" Text="Faite une recherche de film" FontSize="40" FontStyle="Italic">
        <TextBlock.Style>
            <Style TargetType="TextBlock">
                <Setter Property="Visibility" Value="Hidden"/>
                <Style.Triggers>
                    <DataTrigger Binding="{Binding TotalResults}" Value="-1">
                        <Setter Property="Visibility" Value="Visible"/>
                    </DataTrigger>
                </Style.Triggers>
            </Style>
        </TextBlock.Style>
    </TextBlock>
    <TextBlock VerticalAlignment="Center" HorizontalAlignment="Center" Text="Aucun résultat" FontSize="40" FontStyle="Italic">
        <TextBlock.Style>
            <Style TargetType="TextBlock">
                <Setter Property="Visibility" Value="Hidden"/>
                <Style.Triggers>
                    <DataTrigger Binding="{Binding TotalResults}" Value="0">
                        <Setter Property="Visibility" Value="Visible"/>
                    </DataTrigger>
                </Style.Triggers>
            </Style>
        </TextBlock.Style>
    </TextBlock>
    <!--#endregion-->

    <ScrollViewer ScrollViewer.VerticalScrollBarVisibility="Hidden">
        <ItemsControl ItemsSource="{Binding Path=ItemsSource}" HorizontalAlignment="Center">
            <ItemsControl.ItemTemplate>
                <DataTemplate>
                    <Border Margin="30" Background="{DynamicResource MahApps.Brushes.Accent3}" Width="260">
                        <Grid>
                            <Grid.RowDefinitions>
                                ...
                            </Grid.RowDefinitions>

                            <Image Grid.Row="0" Source="{Binding Poster}" VerticalAlignment="Top"/>
                            <Grid Grid.Row="1" Margin="20,10,20,20">
                                <Grid.RowDefinitions>
                                    <RowDefinition Height="*"/>
                                    <RowDefinition Height="auto"/>
                                </Grid.RowDefinitions>

                                <StackPanel Grid.Row="0" VerticalAlignment="Center">
                                    <TextBlock Text="{Binding Title}" TextAlignment="Center" TextWrapping="Wrap"/>
                                    <TextBlock Text="{Binding Year}"  HorizontalAlignment="Center" FontStyle="Italic" FontSize="14" Margin="0,5,0,0"/>
                                </StackPanel>
                                <Button Grid.Row="1" Content="Ajouter à ma liste" HorizontalAlignment="Stretch" Padding="10" Margin="0,10,0,0"
                                        Command="{Binding Path=DataContext.AddCommand, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType=ItemsControl}}"
                                        CommandParameter="{Binding}"/>
                            </Grid>

                        </Grid>
                    </Border>
                </DataTemplate>
            </ItemsControl.ItemTemplate>
            <ItemsControl.ItemsPanel>
                <ItemsPanelTemplate>
                    <WrapPanel/>
                </ItemsPanelTemplate>
            </ItemsControl.ItemsPanel>
        </ItemsControl>
    </ScrollViewer>

</Grid>

<!--#endregion-->
```

En bas de la fenêtre il y a une barre servant à la pagination, elle est constitué d'un `TextBlock` indiquant quelle est la page actuelle, un `Button` branchés par `Binding` sur la commande `PreviousPageCommand` pour aller à la page précédente et d'un `Button` branchés par `Binding` sur la commande `NextPageCommand` pour aller à la page suivante.
Si aucune recheche n'est en cours ou si il n'y a pas de résultat ces éléments ne sont pas affichés.

``` csharp
<!--#region Pagination -->
<Grid Grid.Row="2" Background="{DynamicResource MahApps.Brushes.Accent2}">
    <Grid Margin="0,10">
        <Grid.Style>
            <Style TargetType="Grid">
                <Setter Property="Visibility" Value="Visible"/>
                <Style.Triggers>
                    <DataTrigger Binding="{Binding Pagination.MaxPage}" Value="0">
                        <Setter Property="Visibility" Value="Hidden"/>
                    </DataTrigger>
                </Style.Triggers>
            </Style>
        </Grid.Style>
        <Grid.ColumnDefinitions>
            ...
        </Grid.ColumnDefinitions>

        <Button Grid.Column="1" Padding="20,10" Command="{Binding PreviousPageCommand}">
            <StackPanel Orientation="Horizontal">
                <iconPacks:PackIconMaterial Kind="SkipPrevious" VerticalAlignment="Center" HorizontalAlignment="Center"/>
                <TextBlock Text="Précédent" Margin="10,0,0,0" FontSize="12"/>
            </StackPanel>
        </Button>
        <TextBlock Grid.Column="3" FontWeight="Bold" FontSize="14"  Padding="20,10">
            <TextBlock.Text>
                <MultiBinding StringFormat="Page {0} / {1}">
                    <Binding Path="Pagination.IndexPage" UpdateSourceTrigger="PropertyChanged" Mode="TwoWay"/>
                    <Binding Path="Pagination.MaxPage" UpdateSourceTrigger="PropertyChanged" Mode="TwoWay"/>
                </MultiBinding>
            </TextBlock.Text>
        </TextBlock>
        <Button Grid.Column="5" Padding="20,10" Command="{Binding NextPageCommand}">
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="Suivant" Margin="0,0,10,0" FontSize="12"/>
                <iconPacks:PackIconMaterial Kind="SkipNext" VerticalAlignment="Center" HorizontalAlignment="Center"/>
            </StackPanel>
        </Button>
    </Grid>
</Grid>
<!--#endregion-->
```

#### Vue `MyMoviesView`

Cette vue permet de consulter les films de la collection personnelle.
Le context de donnée utilisé est `ViewModelMyMovies`.

La barre de recherche, en haut du contenu du `TabControl`, est constituée d'un `TextBox` pour le titre et d'un `Button` branchés par `Binding` sur la commande `SearchCommand` pour filter les films par leur titre.
Un `TextBlock` est positionné à gauche pour indiquer combien de films sont enregistrer dans la collection.

``` csharp
<!--#region Infos Bar -->
<Grid Grid.Row="0" Background="{DynamicResource MahApps.Brushes.Accent2}">
    <Grid Margin="0,10">
        <Grid.ColumnDefinitions>
            ...
        </Grid.ColumnDefinitions>

        <TextBlock Grid.Column="1" Text="{Binding ItemsSource.Count, StringFormat='Vous avez ajouté {0} films'}" Margin="10,0" FontSize="20" VerticalAlignment="Center" HorizontalAlignment="Center"/>

        <TextBox Grid.Column="3" Text="{Binding Search}" Padding="10" MinWidth="300" FontSize="16"/>

        <Button Grid.Column="5" Content="Rechercher un film" Padding="50,10" Command="{Binding SearchCommand}"  CommandParameter="{Binding Search}"/>

    </Grid>
</Grid>
<!--#endregion-->
```

Au centre de la vue un `ItemsControl` permet de lister la collection de film d'un manière similaire à la vue `SearchMoviesView`.
Chaque film de la collection sont affichés par block qui sont constitués de l'image du poster associé, d'un titre, d'une date de sortie, d'une durée, du nom du directeur, des genres, des nom des acteurs principaux  et d'un `Button` branchés par `Binding` sur la commande `DeleteCommand` permettant de supprimer le film à la collection personnelle. Il y a aussi un `ToggleButton` pour mettre un film en favoris et un autre pour préciser si on l'a déjà vue ou non.

si il n'y a pas de film enregistré le message "Aucun Film Enregistré" apparait via un `TextBlock`.

``` csharp
<!--#region Liste Films -->

    <Grid Grid.Row="1">

        <!--#region Liste vide -->
        <TextBlock VerticalAlignment="Center" HorizontalAlignment="Center" Text="Aucun Film Enregistré" FontSize="40" FontStyle="Italic">
            <TextBlock.Style>
                <Style TargetType="TextBlock">
                    <Setter Property="Visibility" Value="Hidden"/>
                    <Style.Triggers>
                        <DataTrigger Binding="{Binding ItemsSource.Count}" Value="0">
                            <Setter Property="Visibility" Value="Visible"/>
                        </DataTrigger>
                    </Style.Triggers>
                </Style>
            </TextBlock.Style>
        </TextBlock>
        <!--#endregion-->

        <ScrollViewer ScrollViewer.VerticalScrollBarVisibility="Hidden">
            <ItemsControl ItemsSource="{Binding Source={StaticResource MovieViewSource}, Mode=OneWay, UpdateSourceTrigger=PropertyChanged}" HorizontalAlignment="Center"
                            IsTextSearchEnabled="True" TextSearch.TextPath="{Binding Title}" TextSearch.Text="{Binding ElementName=SearchBox, Path=Text}">
                <ItemsControl.ItemTemplate>
                    <DataTemplate>
                        <Border Margin="20" Background="{DynamicResource MahApps.Brushes.Accent3}" Height="400" Width="580">
                            <Grid>
                                <Grid.ColumnDefinitions>
                                    ...
                                </Grid.ColumnDefinitions>

                                <Grid Grid.Column="0" Background="{DynamicResource MahApps.Brushes.Badged.DisabledBackground}">
                                    <Image Source="{Binding Poster}" MaxWidth="275"/>
                                </Grid>

                                <Grid Grid.Column="1" Margin="20,10,20,20">
                                    <Grid.RowDefinitions>
                                        ...
                                    </Grid.RowDefinitions>

                                    <!--#region Favoris -->
                                    <Grid Grid.Row="0">
                                        <ToggleButton IsChecked="{Binding IsFavorite}" Background="Transparent" BorderBrush="Transparent" VerticalAlignment="Center" HorizontalAlignment="Center"
                                                        Command="{Binding Path=DataContext.SaveCommand, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType=ItemsControl}}">
                                            <ToggleButton.Content>
                                                <Grid>
                                                    <iconPacks:PackIconMaterial Kind="HeartOutline" VerticalAlignment="Center" HorizontalAlignment="Center" Width="25" Height="25">
                                                        <iconPacks:PackIconMaterial.Style>
                                                            <Style TargetType="iconPacks:PackIconMaterial">
                                                                <Setter Property="Visibility" Value="Hidden"/>
                                                                <Style.Triggers>
                                                                    <DataTrigger Binding="{Binding IsFavorite}" Value="false">
                                                                        <Setter Property="Visibility" Value="Visible"/>
                                                                    </DataTrigger>
                                                                </Style.Triggers>
                                                            </Style>
                                                        </iconPacks:PackIconMaterial.Style>
                                                    </iconPacks:PackIconMaterial>
                                                    <iconPacks:PackIconMaterial Kind="Heart" VerticalAlignment="Center" HorizontalAlignment="Center" Width="25" Height="25">
                                                        <iconPacks:PackIconMaterial.Style>
                                                            <Style TargetType="iconPacks:PackIconMaterial">
                                                                <Setter Property="Visibility" Value="Hidden"/>
                                                                <Style.Triggers>
                                                                    <DataTrigger Binding="{Binding IsFavorite}" Value="true">
                                                                        <Setter Property="Visibility" Value="Visible"/>
                                                                    </DataTrigger>
                                                                </Style.Triggers>
                                                            </Style>
                                                        </iconPacks:PackIconMaterial.Style>
                                                    </iconPacks:PackIconMaterial>
                                                </Grid>
                                            </ToggleButton.Content>
                                        </ToggleButton>
                                    </Grid>
                                    <!--#endregion-->

                                    <!--#region Informations -->
                                    <TextBlock Grid.Row="1" Text="{Binding Title}" TextAlignment="Center" VerticalAlignment="Center" TextWrapping="Wrap" FontSize="18"/>

                                    <ScrollViewer Grid.Row="2" VerticalScrollBarVisibility="Auto">
                                        <StackPanel VerticalAlignment="Center">
                                            <StackPanel Orientation="Horizontal" Margin="0,0,0,10">
                                                <TextBlock Text="Date de sortie : " FontSize="14" FontStyle="Italic"/>
                                                <TextBlock Text="{Binding Released}" VerticalAlignment="Center"/>
                                            </StackPanel>

                                            <StackPanel Orientation="Horizontal" Margin="0,0,0,10">
                                                <TextBlock Text="Durée : " FontSize="14" FontStyle="Italic"/>
                                                <TextBlock Text="{Binding Runtime}" VerticalAlignment="Center"/>
                                            </StackPanel>

                                            <StackPanel Margin="0,0,0,10">
                                                <TextBlock Text="Genres : " FontSize="14" FontStyle="Italic"/>
                                                <TextBlock Text="{Binding Genre}" TextWrapping="Wrap"/>
                                            </StackPanel>

                                            <StackPanel Orientation="Horizontal" Margin="0,0,0,10">
                                                <TextBlock Text="Réalisateur : " FontSize="14" FontStyle="Italic"/>
                                                <TextBlock Text="{Binding Director}" VerticalAlignment="Center"/>
                                            </StackPanel>

                                            <StackPanel Margin="0,0,0,10">
                                                <TextBlock Text="Acteurs principaux : " FontSize="14" FontStyle="Italic"/>
                                                <TextBlock Text="{Binding Actors}" TextWrapping="Wrap"/>
                                            </StackPanel>
                                        </StackPanel>
                                    </ScrollViewer>
                                    <!--#endregion-->

                                    <!--#region Actions -->

                                    <!--#region Vue -->
                                    <Grid Grid.Row="3">
                                        <ToggleButton IsChecked="{Binding IsView}" Background="Transparent" BorderBrush="Transparent" VerticalAlignment="Center" HorizontalAlignment="Left"
                                                        Command="{Binding Path=DataContext.SaveCommand, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType=ItemsControl}}">
                                            <ToggleButton.Content>
                                                <StackPanel Orientation="Horizontal">
                                                    <Grid>
                                                        <iconPacks:PackIconMaterial Kind="CheckboxBlankOutline" VerticalAlignment="Center" HorizontalAlignment="Center" Width="20" Height="20">
                                                            <iconPacks:PackIconMaterial.Style>
                                                                <Style TargetType="iconPacks:PackIconMaterial">
                                                                    <Setter Property="Visibility" Value="Hidden"/>
                                                                    <Style.Triggers>
                                                                        <DataTrigger Binding="{Binding IsView}" Value="false">
                                                                            <Setter Property="Visibility" Value="Visible"/>
                                                                        </DataTrigger>
                                                                    </Style.Triggers>
                                                                </Style>
                                                            </iconPacks:PackIconMaterial.Style>
                                                        </iconPacks:PackIconMaterial>
                                                        <iconPacks:PackIconMaterial Kind="CheckboxMarkedOutline" VerticalAlignment="Center" HorizontalAlignment="Center" Width="20" Height="20">
                                                            <iconPacks:PackIconMaterial.Style>
                                                                <Style TargetType="iconPacks:PackIconMaterial">
                                                                    <Setter Property="Visibility" Value="Hidden"/>
                                                                    <Style.Triggers>
                                                                        <DataTrigger Binding="{Binding IsView}" Value="true">
                                                                            <Setter Property="Visibility" Value="Visible"/>
                                                                        </DataTrigger>
                                                                    </Style.Triggers>
                                                                </Style>
                                                            </iconPacks:PackIconMaterial.Style>
                                                        </iconPacks:PackIconMaterial>
                                                    </Grid>
                                                    <TextBlock Text="Vue" FontSize="16" Margin="5,0"/>
                                                </StackPanel>
                                            </ToggleButton.Content>
                                        </ToggleButton>
                                    </Grid>
                                    <!--#endregion-->

                                    <Button Grid.Row="4" Content="Retirer de ma liste" HorizontalAlignment="Stretch" Padding="10" Margin="0,10,0,0"
                                    Command="{Binding Path=DataContext.DeleteCommand, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType=ItemsControl}}"
                                    CommandParameter="{Binding}"/>
                                    <!--#endregion-->

                                </Grid>

                            </Grid>
                        </Border>
                    </DataTemplate>
                </ItemsControl.ItemTemplate>
                <ItemsControl.ItemsPanel>
                    <ItemsPanelTemplate>
                        <WrapPanel/>
                    </ItemsPanelTemplate>
                </ItemsControl.ItemsPanel>
            </ItemsControl>
        </ScrollViewer>
    </Grid>

</Grid>

<!--#endregion-->
```

### Vue-modèle

#### Injection des dépendances

Les vues-modèles ont été développés sur le principe de la dépendance faible.
Chaque vue-modèle ne connait donc pas l'instance concrète des autres vues-modèles ainsi que la classe concrète du contexte de données utilisés.
La seule dépendance forte réside dans l'utilisation des classes concrètes du modèles de données.

L'injection de dépendance nécessite pour chaque vue-modèle de déclarer une interface qui décrit le comportement attendu du vue-modèle.

Les vues-modèles sont déclarés dans le répertoire `.\ViewModels\` et les interfaces dans `.\ViewModels\Abstracts\`

L'injection des dépendances est réalisés de deux manières :

- Passage de l'ensemble des dépendances par contructeur
- Passage du `System.IServiceProvider` dans le constructeur pour permettre au vue-modèle de résoudre à tous moment ses dépendances.

##### Passage de l'ensemble des dépendances par contructeur

Par exemple, le vue-modèle `ViewModelMyMovies` ne dépend que d'un `IDataContext`.
La résolution de cette dépendance n'est pas réalisée par le vue-modèle lui-même mais par la classe qui instancie le vue-modèle.

``` csharp
public ViewModelMyMovies(IDataContext dataContext)
    : base(dataContext)
{
    ...
    this.LoadData();
}
```

``` csharp
private string _FilePath = @"C:\Temp\movielibrary.json";
private void Application_Startup(object sender, StartupEventArgs e)
{
    ...
    ServiceCollection serviceCollection = new ServiceCollection();

    //Création du contexte de données de l'application.
    serviceCollection.AddSingleton<IDataContext, MoviesLibraryContext>(sp => FileDataContext.Load(this._FilePath, new MoviesLibraryContext(this._FilePath)));


    serviceCollection.AddTransient<IViewModelMyMovies, ViewModelMyMovies>(sp => new ViewModelMyMovies(sp.GetService<IDataContext>()));

    ...
}
```

##### Passage du `System.IServiceProvider` dans le constructeur

Par exemple, le vue-modèle `ViewModelMain` dépend que directement d'un `IServiceProvider`.
Ceci permet au vue-modèle ensuite de demander au fournisseur de service de résoudre ses dépendances.

``` csharp
public ViewModelMain(IServiceProvider serviceProvider)
    : base(serviceProvider.GetService<IDataContext>())
{
    this._ServiceProvider = serviceProvider;

    this._ViewModelMovies = this._ServiceProvider.GetService<IViewModelMovies>();
    this._ViewModelMyMovies = this._ServiceProvider.GetService<IViewModelMyMovies>();

    this._ExitCommand = new RelayCommand(this.Exit, this.CanExit);
    this.LoadData();
}
```

``` csharp
private string _FilePath = @"C:\Temp\movielibrary.json";
private void Application_Startup(object sender, StartupEventArgs e)
{
    ...
    ServiceCollection serviceCollection = new ServiceCollection();

    //Création du contexte de données de l'application.
    serviceCollection.AddSingleton<IDataContext, MoviesLibraryContext>(sp => FileDataContext.Load(this._FilePath, new MoviesLibraryContext(this._FilePath)));

    serviceCollection.AddTransient<IViewModelMain, ViewModelMain>(sp => new ViewModelMain(sp));

    ...
}
```

##### Gestion des instances par le fournisseur de service

L'ensemble des services des vues-modèles sont déclarés avec la méthode `AddTransient`, ce qui signifie qu'à chaque résolution, le fournisseur de service retourne une nouvelle instance du vue-modèle demandé.
Par contre, le contexte de données doit être commun à l'ensemble de l'application, le service est donc déclaré avec la méthode `AddSingleton`, ce qui signifie qu'à chaque résolution, le fournisseur de service retoune l'instance unique du contexte de données.

``` csharp
private string _FilePath = @"C:\Temp\movielibrary.json";
private void Application_Startup(object sender, StartupEventArgs e)
{
    ...

    ServiceCollection serviceCollection = new ServiceCollection();

    //Création du contexte de données de l'application.
    serviceCollection.AddSingleton<IDataContext, MoviesLibraryContext>(sp => FileDataContext.Load(this._FilePath, new MoviesLibraryContext(this._FilePath)));

    //Création des vue-modèle.
    serviceCollection.AddTransient<IViewModelMain, ViewModelMain>(sp => new ViewModelMain(sp));
    serviceCollection.AddTransient<IViewModelMovies, ViewModelSearchMovies>(sp => new ViewModelSearchMovies(sp));
    serviceCollection.AddTransient<IViewModelMyMovies, ViewModelMyMovies>(sp => new ViewModelMyMovies(sp.GetService<IDataContext>()));

    //Construction du fournisseur de service à partir de la définition des services disponibles.
    ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

    //Création de l'instance de la fenêtre principale.
    MainWindow window = new MainWindow();
    //Injection du vue-modèle de la fenêtre.
    window.DataContext = serviceProvider.GetService<IViewModelMain>();
    //Affichage de la fenêtre principale.
    window.Show();
}
```

#### Architecture

Les vues-modèles respectent l'architecture suivante :

``` xml
<ViewModelMain>
  <ViewModelMain.ItemsSource>

    <ViewModelSearchMovies/>

    <ViewModelMyMovies/>

  </ViewModelMain.ItemsSource>
</ViewModelMain>
```

#### Vue-modèle `ViewModelMain`
Ce vue-modèle hérite de la classe `CoursWPF.MVVM.ViewModels.ViewModelList<IObservableObject, IDataContext>`.

Il dispose donc d'une collection observable `ItemsSource` et d'un `SelectedItem` de type `IObservableObject` ainsi que du contexte de données.
Ce dernier ne sera pas réèlement utilisé puisque `ViewModelMain` est un vue-modèle structurel (utilisé pour structurer l'interface graphique).

Le vue-modèle expose et implémente la commande `ExitCommand` qui permet de fermer l'application
Il implémente aussi la commande `SaveCommand`qui est chargé de sauvegarder le contexte de données.

``` csharp
public ViewModelMain(IServiceProvider serviceProvider)
    : base(serviceProvider.GetService<IDataContext>())
{
    this._ServiceProvider = serviceProvider;

    this._ViewModelMovies = this._ServiceProvider.GetService<IViewModelMovies>();
    this._ViewModelMyMovies = this._ServiceProvider.GetService<IViewModelMyMovies>();

    this._ExitCommand = new RelayCommand(this.Exit, this.CanExit);
    this.LoadData();
}
```

#### Vue-modèle `ViewModelSearchMovies`

Ce vue-modèle hérite de la classe `MoiesLibrary.ClientApp.ViewModels.ViewModelList<Movie, IDataContext>`.
Il représente dans une collection de compte film.
La commande `AddCommand` est suchergé par celle du view modèle `ViewModelMyMovies`
Ce vue-modèle est également en charge modifier le contenue de cette collection avec la command de recherche `SearchCommand` utilisant les paramètres `Search` et `Year`.
Une gestion de pagination est aussi inmplémenté avec les commandes `PreviousPageCommand` et `NextPageCommand`.
Le nombre Total de films d'une recherche est enregister dans `TotalResults`.

``` csharp
public ViewModelSearchMovies(IServiceProvider serviceProvider)
    : base(serviceProvider.GetService<IDataContext>())
{
    this._ViewModelMyMovies = serviceProvider.GetService<IViewModelMyMovies>();
    this._SearchCommand = new RelayCommand(this.SearchMovie, this.CanSearch);
    this._PreviousPageCommand = new RelayCommand(this.PreviousPage, this.CanPreviousPage);
    this._NextPageCommand = new RelayCommand(this.NextPage, this.CanNextPage);
    this._Pagination = new Pagination();
    this._Search = "";
    this._Year = "";
    this._TotalResults = -1;

    this.LoadData();
}
```

#### Vue-modèle `ViewModelMyMovies`

Ce vue-modèle hérite de la classe `MoiesLibrary.ClientApp.ViewModels.ViewModelList<MovieDetails, IDataContext>`.
Il représente dans une collection de compte film détaillés, elle peut être filtrer par la commande `SearchCommand` avec le paramètre `Search`.

``` csharp
public ViewModelMyMovies(IDataContext dataContext)
    : base(dataContext)
{
    this._SearchCommand = new RelayCommand(this.SearchMovie, this.CanSearch);
    this._Search = "";
    this.LoadData();
}
```
