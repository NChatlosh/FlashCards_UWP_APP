using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards
{
   public class MatchingCard : FlashCard
    {
        public MatchingCard(Question q) : base(q)
        {
            
        }
        public MatchingCard()
        {

        }

        public override object GetAnswer()
        {
            return question.answer.Pairs;
        }

        public List<string> GetMatchKeys()
        {
            if (question.answer.Pairs != null)
            {
                List<string> ops = new List<string>();
                foreach (KeyValuePair<string, string> pair in question.answer.Pairs)
                {
                    ops.Add(pair.Key);
                }
                return ops;
            }
            else
            {
                return null;
            }
        }
        public List<string> GetMatchValues()
        {
            if (question.answer.Pairs != null)
            {
                List<string> ops = new List<string>();
                foreach (KeyValuePair<string, string> pair in question.answer.Pairs)
                {
                    ops.Add(pair.Value);
                }
                return ops;
            }
            else
            {
                return null;
            }
        }

        public override bool CompareAnswer(object answer)
        {
            bool correct = true;
            Dictionary<string, string> pairs = (Dictionary<string, string>)answer;
           foreach(KeyValuePair<string, string> p in pairs)
            {
                if (p.Value != question.answer.Pairs[p.Key])
                {
                    correct = false;
                    break;
                }
            }
            return correct;
        }


    }
}
