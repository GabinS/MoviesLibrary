using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoviesLibrary.ClientApp.Views
{
    /// <summary>
    /// Logique d'interaction pour SearchMoviesView.xaml
    /// </summary>
    public partial class SearchMoviesView : UserControl
    {
        public SearchMoviesView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Accepte seulement les entrées numérique du clavier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
