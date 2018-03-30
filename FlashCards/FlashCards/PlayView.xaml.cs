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
    public sealed partial class PlayView : Page
    {
        public CardManager manager;
        public enum Type { Vocab, Mult, Mat };

        public PlayView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            manager = (CardManager)e.Parameter;
            manager.Shuffle();
            manager.LoadCurrentCard();
            DisplayCard();
        }

        private void DisplayCard()
        {
            Question.Text = manager.GetQuestionText();
            object info = manager.GetAnwerInfo();
            if (info is string)
            {
                ChangeCardType(Type.Vocab);
            }
            else if (info is List<string>)
            {
                ChangeCardType(Type.Mult);
                List<string> ops = (List<string>)info;
                for (int i = 0; i < ops.Count; i++)
                {
                    RadioButton but = (RadioButton)MultPanel.Children[i];
                    but.Content = ops[i];                  
                }
            }
            else if (info is Dictionary<string, string>)
            {
                ChangeCardType(Type.Mat);
                List<string> keys = manager.GetMatchKeys();
                List<string> values = manager.GetMatchValues();
                for (int i = 0; i < keys.Count; i++)
                {
                    ComboBox cb = (ComboBox)MatchingPanel.Children[i];
                    cb.Header = keys[i];
                    cb.ItemsSource = values;
                }
            }
            else { }
        }
        private void ChangeCardType(Type t)
        {
            if(t == Type.Vocab)
            {
                VocabBox.Visibility = Visibility.Visible;
                MatchingPanel.Visibility = Visibility.Collapsed;
                MultPanel.Visibility = Visibility.Collapsed;
            }
            else if (t == Type.Mult)
            {
                VocabBox.Visibility = Visibility.Collapsed;
                MatchingPanel.Visibility = Visibility.Collapsed;
                MultPanel.Visibility = Visibility.Visible;
            }
            else if (t == Type.Mat)
            {
                VocabBox.Visibility = Visibility.Collapsed;
                MatchingPanel.Visibility = Visibility.Visible;
                MultPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if(VocabBox.Visibility == Visibility.Visible)
            {
                Response.Text = manager.CompareAnswer(VocabBox.Text);
            }
            else if (MultPanel.Visibility == Visibility.Visible)
            {
                string text = "";
                foreach(RadioButton but in MultPanel.Children)
                {
                    if (but.IsChecked == true)
                        text = but.Content.ToString();
                }
                Response.Text = manager.CompareAnswer(text);
            }
            else if (MatchingPanel.Visibility == Visibility.Visible)
            {
                Dictionary<string, string> pairs = new Dictionary<string, string>();
                try
                {
                    for (int i = 0; i < MatchingPanel.Children.Count; i++)
                    {
                        ComboBox cb = (ComboBox)MatchingPanel.Children[i];
                        pairs.Add(cb.Header.ToString(), cb.SelectedValue.ToString());
                    }

                    Response.Text = manager.CompareAnswer(pairs);
                }
                catch (NullReferenceException nre)
                {
                    Response.Text = "Not all matches were selected, try again.";
                }
                
            }
            else { }
             Points.Text = manager.GetPoints();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Response.Text = "";
            manager.NextCard();
            DisplayCard();
        }

        private void ShowAns_Click(object sender, RoutedEventArgs e)
        {
            if(manager.IsEmpty()) { }
            else
            {
                string ans = manager.GetRealAnswer();
                string resp = "The real answer is:";
                if (ans == String.Empty)
                {
                    Dictionary<string, string> pairs = (Dictionary<string, string>)manager.GetAnwerInfo();
                    foreach (KeyValuePair<string, string> p in pairs)
                    {
                        resp += String.Format($" {p.Key}-{p.Value}, ");
                    }
                    Response.Text = resp;
                }
                else
                {
                    Response.Text = String.Format($"{resp} {ans}");
                }
            }
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), manager);
        }
    }
}
