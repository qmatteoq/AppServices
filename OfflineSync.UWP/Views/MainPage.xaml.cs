using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using OfflineSync.PCL;
using OfflineSync.PCL.Model;

namespace OfflineSync.UWP.Views
{
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<TodoItem> todoItems { get; set; }

        private string userId;

        public MainPage()
        {
            this.InitializeComponent();
            (Application.Current as UWP.App).MobileServiceHelper = new MobileServiceHelper();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            PasswordVault vault = new PasswordVault();
            bool isUserLogged = false;
            try
            {
                var resources = vault.FindAllByResource("UserId");
                isUserLogged = true;
            }
            catch
            {
                isUserLogged = false;
            }

            if (!isUserLogged)
            {
                await LoginAsync();
            }
            else
            {
                var resources = vault.FindAllByResource("UserId");
                PasswordCredential user = resources.FirstOrDefault();
                user.RetrievePassword();
                (Application.Current as UWP.App).MobileServiceHelper.SetLoggedUser(user.Password);
                userId = user.Password;
            }

            await (Application.Current as UWP.App).MobileServiceHelper.InitializeDatabaseAsync();
            await Refresh();
        }

        private async Task LoginAsync()
        {
            MobileServiceUser user = await (Application.Current as UWP.App).MobileServiceHelper.Client.LoginAsync(MobileServiceAuthenticationProvider.Twitter);
            (Application.Current as UWP.App).MobileServiceHelper.SetLoggedUser(user.UserId);
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = new PasswordCredential("UserId", "id", user.UserId);
            vault.Add(credential);
            userId = user.UserId;
        }

        private async void OnAddClicked(object sender, RoutedEventArgs e)
        {
            TodoItem item = new TodoItem
            {
                Text = Name.Text,
                Complete = false,
                UserId = userId
            };

            await (Application.Current as UWP.App).MobileServiceHelper.AddItemAsync(item);
            await (Application.Current as UWP.App).MobileServiceHelper.SyncAsync();
            todoItems.Add(item);
        }

        private void OnNoteSelected(object sender, ItemClickEventArgs e)
        {
            TodoItem selectedItem = e.ClickedItem as TodoItem;
            Frame.Navigate(typeof(DetailView), selectedItem);
        }

        private async void OnRefreshClicked(object sender, RoutedEventArgs e)
        {
            await Refresh();
        }

        public async Task Refresh()
        {
            await (Application.Current as UWP.App).MobileServiceHelper.SyncAsync();

            var items = await (Application.Current as UWP.App).MobileServiceHelper.GetItemsAsync();
            todoItems = new ObservableCollection<TodoItem>(items);
            Items.ItemsSource = todoItems;
        }
    }
}
