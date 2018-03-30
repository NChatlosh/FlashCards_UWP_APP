using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards
{
   public class Question
    {
        public Answer answer;
        public string Text;
        

        public Question(Answer ans, string text)
        {
            answer = ans;
            Text = text;
        }
       
    }
}
