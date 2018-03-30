using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards
{
   public class Answer
    {
        public int Id;
        public string RealAnswer;
        public List<string> Options;
        public Dictionary<string, string> Pairs;

        public Answer()
        {

        }
        public Answer(int id, string text)//vocab
        {
            Id = id;
            RealAnswer = text;
        }
        public Answer(string text, List<string> ops)//mult
        {
            Id = 0;
            RealAnswer = text;
            Pairs = null;
            Options = ops;
        }
        public Answer(Dictionary<string, string> pairs)//matching
        {
            Id = 0;
            RealAnswer = String.Empty;
            Pairs = pairs;
            Options = null;
        }
    }
}
