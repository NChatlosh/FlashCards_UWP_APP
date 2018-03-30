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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FlashCards
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class EditView : Page
    {
        public CardManager manager;
        public enum Type {Vocab, Mult, Mat };
        public EditView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            manager = (CardManager)e.Parameter;
                manager.LoadCurrentCard();
                DisplayCard();
        }

        private void ChangeCard(Type t)
        {
            if (t == Type.Vocab)//vocab
            {
                AnswerBox.Visibility = Visibility.Visible;
                Matching.Visibility = Visibility.Collapsed;
                Multiple.Visibility = Visibility.Collapsed;
                RealAnswer.Visibility = Visibility.Collapsed;
            }
            else if (t == Type.Mult)//Mult
            {
                AnswerBox.Visibility = Visibility.Collapsed;
                Matching.Visibility = Visibility.Collapsed;
                Multiple.Visibility = Visibility.Visible;
                RealAnswer.Visibility = Visibility.Visible;
            }
            else if (t == Type.Mat) // Matching
            {
                AnswerBox.Visibility = Visibility.Collapsed;
                Matching.Visibility = Visibility.Visible;
                Multiple.Visibility = Visibility.Collapsed;
                RealAnswer.Visibility = Visibility.Collapsed;
            }
        }

        private void CardType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CardType.SelectedItem == V)
            {
                //vocab card
                ChangeCard(Type.Vocab);
            }
            else if (CardType.SelectedItem == MC)
            {
                //Multiple choice
                ChangeCard(Type.Mult);
            }
            else if (CardType.SelectedItem == Mt)
            {
                //Matching
                ChangeCard(Type.Mat);
            }
            ClearCard();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(CardType.SelectedItem == V)
            {
                Status.Text = manager.CreateCard(Question.Text, AnswerBox.Text);
            }
            else if(CardType.SelectedItem == MC)
            {
                List<string> ops = new List<string>();
                foreach(TextBox box in Multiple.Children)
                {
                    ops.Add(box.Text);
                }
                try
                {
                    string real = ops[RealAnswer.SelectedIndex];
                    Status.Text = manager.CreateCard(Question.Text, real, ops);
                }
                catch(ArgumentOutOfRangeException ae)
                {
                    Status.Text = "Failed to create card, you must select the real answer.";
                }
            }
            else if (CardType.SelectedItem == Mt)
            {
                List<string> keys = new List<string>();
                List<string> values = new List<string>();
                foreach(StackPanel panel in Matching.Children)
                {
                    int i = 0;                  
                        TextBox Keybox = (TextBox)panel.Children[i];
                        TextBox Vbox = (TextBox)panel.Children[i+1];
                        keys.Add(Keybox.Text);
                        values.Add(Vbox.Text);
                }
                Status.Text = manager.CreateCard(Question.Text, keys, values);
            }
        }

        private void ClearCard()
        {
            Question.Text = String.Empty;
            AnswerBox.Text = String.Empty;
            foreach (StackPanel panel in Matching.Children)
            {
                foreach (TextBox box in panel.Children)
                {
                    box.Text = String.Empty;
                }              
            }
            foreach (TextBox box in Multiple.Children)
            {
                box.Text = String.Empty;
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            ClearCard();
        }

        private void DisplayCard()
        {          
            object info = manager.GetAnwerInfo();
            if (info is string)
            {
                CardType.SelectedItem = V;
                AnswerBox.Text = manager.GetRealAnswer();
            }
            else if (info is List<string>)
            {
                CardType.SelectedItem = MC;
                List<string> ops = (List<string>)info;
                for (int i = 0; i < ops.Count; i++)
                {
                    TextBox box = (TextBox)Multiple.Children[i];
                    box.Text = ops[i];
                    if(ops[i] == manager.GetRealAnswer())
                    {
                        RealAnswer.SelectedIndex = i;
                    }
                }
            }
            else if (info is Dictionary<string,string>)
            {
                CardType.SelectedItem = Mt;
                List<string> keys = manager.GetMatchKeys();
                List<string> values = manager.GetMatchValues();
                int n = 0;
                foreach (StackPanel panel in Matching.Children)
                {
                    int i = 0;
                        TextBox Keybox = (TextBox)panel.Children[i];
                        TextBox Vbox = (TextBox)panel.Children[i + 1];                    
                        Keybox.Text = keys[n];
                        Vbox.Text = values[n];
                    n++;
                }
            }
            else { }
            Question.Text = manager.GetQuestionText();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            ClearCard();
            manager.NextCard();
                DisplayCard();

        }

        private async void Done_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("json", new List<string>() { ".json" });
                savePicker.SuggestedFileName = "New Document";


                Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                string contents = manager.SerializeSet(file.Name);
                await Windows.Storage.FileIO.WriteTextAsync(file, contents);
                Windows.Storage.Provider.FileUpdateStatus status =
            await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    Status.Text = "File " + file.Name + " was saved.";
                }
                else
                {
                    Status.Text = "File " + file.Name + " couldn't be saved.";
                }
            }
            catch (NullReferenceException ne)
            {
                Status.Text = "Operation canceled.";
            }
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), manager);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            manager.PreviousCard();
            ClearCard();
            DisplayCard();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Status.Text = manager.RemoveCard();
            ClearCard();
            DisplayCard();
        }
    }
}
