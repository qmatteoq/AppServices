using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using OfflineSync.PCL.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OfflineSync.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailView : Page
    {
        private TodoItem item;
        public DetailView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            item = e.Parameter as TodoItem;
            Notes.Text = item.Text;
        }

        private async void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            item.Text = Notes.Text;
            await (Application.Current as UWP.App).MobileServiceHelper.UpdateItemAsync(item);
            await (Application.Current as UWP.App).MobileServiceHelper.SyncAsync();
            Frame.GoBack();
        }

        private async void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            await (Application.Current as UWP.App).MobileServiceHelper.DeleteItemAsync(item);
            await (Application.Current as UWP.App).MobileServiceHelper.SyncAsync();
            Frame.GoBack();
        }
    }
}

