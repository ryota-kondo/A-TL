using ATL.ViewModels;
using Xamarin.Forms;

namespace ATL.Views
{
    public partial class MenuPage : ContentPage
    {
        private MasterDetailMainPageViewModel ViewModel => this.BindingContext as MasterDetailMainPageViewModel;
        public MenuPage()
        {
            InitializeComponent();
        }
        private async void ListViewMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await this.ViewModel.PageChangeAsync(e.SelectedItem as Models.MenuItem);
        }

    }
}
