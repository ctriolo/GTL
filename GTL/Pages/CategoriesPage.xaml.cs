using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace GTL {
    public partial class CategoriesPage : PhoneApplicationPage {
        public CategoriesPage() {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            string category = "";
            if (NavigationContext.QueryString.TryGetValue("category", out category)) {
                int index = 0;
                switch (category) {
                case "GYM": index = (int)MainViewModel.CATEGORIES.GYM; break;
                case "TAN": index = (int)MainViewModel.CATEGORIES.TAN; break;
                case "LAUNDRY": index = (int)MainViewModel.CATEGORIES.LAUNDRY; break;
                }
                CategoriesPivot.SelectedIndex = index;
            }
        }

        private void Select(object sender, EventArgs e) {
            string id = (string)(sender as StackPanel).Tag;
            NavigationService.Navigate(new Uri("/Pages/VenuePage.xaml?id=" + id, UriKind.Relative));
        }

        private void map_Tap(object sender, System.Windows.Input.GestureEventArgs e) {

        }

        
    }
}