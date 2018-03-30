using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace FlashCards
{
   public class CardManager
    {
        public List<FlashCard> Cards;
        public int Points;
        public FlashCard CurrentCard;
        public int CurrentID;
        private JsonSerializerSettings settings;


        public CardManager()
        {
            Cards = new List<FlashCard>();
            Points = 0;
            CurrentCard = new FlashCard();
            CurrentID = 0;
            settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All };
        }

        public bool IsEmpty()
        {
            if(Cards.Count != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Shuffle()
        {
            Cards.Shuffle();
        }

        public string SerializeSet(string fileName)
        {
            string serialized = JsonConvert.SerializeObject(Cards, settings);
            return serialized;
        }
        public void DeserializeSet(string contents)
        {
            List<FlashCard> flashCards = JsonConvert.DeserializeObject<List<FlashCard>>(contents, settings);
            Cards = flashCards;
        }

        public string GetPoints()
        {
            return String.Format($"Points: {Points.ToString()}"); ;
        }

        public string CompareAnswer(object answer)
        {
            if(CurrentCard.CompareAnswer(answer))
            {
                Points += 10;
                return "Correct! Great Job";
            }
            else
            {
                Points -= 5;
                return "Yikes! Sorry but that is incorrect.";
            }

        }
        public string GetRealAnswer()
        {
            return CurrentCard.GetRealAnswerText();
        }
        public List<string> GetMatchKeys()
        {
            if(CurrentCard is MatchingCard)
            {
                MatchingCard c = (MatchingCard)CurrentCard;
                return c.GetMatchKeys();
            }
            else
            {
                return null;
            }
        }
        public List<string> GetMatchValues()
        {
            if (CurrentCard is MatchingCard)
            {
                MatchingCard c = (MatchingCard)CurrentCard;
                return c.GetMatchValues();
            }
            else
            {
                return null;
            }
        }
        public object GetAnwerInfo()
        {
            if (Cards.Count != 0)
                return CurrentCard.GetAnswer();
            else
                return null;
        }
        

        public string GetQuestionText()
        {
            if (Cards.Count != 0)
                return CurrentCard.GetQuestion();
            else
                return String.Empty;
        }
        public void NextCard()
        {
            if(CurrentID != (Cards.Count -1))
            {
                CurrentID++;
                LoadCurrentCard();
            }
            else
            {
                CurrentID = 0;
                LoadCurrentCard();
            }
            
        }

        public string RemoveCard()
        {
            if(Cards.Count != 0)
            {
                Cards.Remove(CurrentCard);
                if(Cards.Count != 0)
                {
                    if(CurrentID >= Cards.Count)
                    {
                        CurrentID = (Cards.Count -1);
                    }
                    LoadCurrentCard();
                }
                else
                {
                    return "No more cards in the set.";
                }
                
                return "Successfully removed flash card.";
            }
            else
            {
                return "No cards to be removed.";
            }
        }

        public void PreviousCard()
        {
            if(Cards.Count != 0)
            {
                if(CurrentID != 0)
                {
                    CurrentID--;
                    LoadCurrentCard();
                }
            }
        }

        public void LoadCurrentCard()
        {
            if(Cards.Count != 0)
            {
                CurrentCard = Cards[CurrentID];
            }
        }

        public void AddCard(FlashCard card)
        {
            Cards.Add(card);
        }

        public string CreateCard(string QuestionText, string AnswerText)
        {
            if(QuestionText != "" && AnswerText != "")
            {
                VocabCard card = new VocabCard(new Question(new Answer(0, AnswerText), QuestionText));
                AddCard(card);
                return "Vocabulary card succesfully created.";
            }
            else
            {
                return "Could not create card because not all fields were filled.";
            }
        }
        public string CreateCard(string QuestionText, string real, List<string> options)
        {
            if(QuestionText != "" && options.Count!=0)
            {
                Answer ans = new Answer(real, options);
                MultChoiceCard card = new MultChoiceCard(new Question(ans, QuestionText));
                AddCard(card);
                return "Multiple choice card succesfully created.";
            }
            else
            {
                return "Could not create card because not all fields were filled.";
            }
        }
        public string CreateCard(string QuestionText, List<string> keys, List<string> values)
        {
            if(keys.Count != 0 && values.Count != 0 && QuestionText != "")
            {
                Dictionary<string, string> pairs = new Dictionary<string, string>();
                for (int i = 0; i < keys.Count; i++)
                {
                    pairs.Add(keys[i], values[i]);
                }
                MatchingCard card = new MatchingCard(new Question(new Answer(pairs), QuestionText));
                AddCard(card);
                return "Matching card succesfully created.";
            }
            else
            {
                return "Could not create card because not all fields were filled.";
            }
        }
    }
}
