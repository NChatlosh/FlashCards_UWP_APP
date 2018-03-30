using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FlashCards
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public CardManager manager = new CardManager();
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                manager = (CardManager)e.Parameter;
            }
            catch(InvalidCastException ie)
            {
                Status.Text = "No card set currently loaded";
            }
            finally
            {

            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditView), manager);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PlayView), manager);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            CardManager man = new CardManager();
            Frame.Navigate(typeof(EditView), man);
        }

        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".json");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
               string contents = await Windows.Storage.FileIO.ReadTextAsync(file);
                manager.DeserializeSet(contents);
               Status.Text =  "Successfully opened " + file.Name;
            }
            else
            {
                Status.Text = "failed to open file";
            }
        }
    }
}
