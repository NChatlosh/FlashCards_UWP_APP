using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace FlashCards
{
    
    public class FlashCard 
    {
        
        public Question question;
        public FlashCard()
        {

        }

        public FlashCard(Question q)
        {
            question = q;
        }

        public virtual string GetRealAnswerText()
        {
            return String.Empty;
        }
        public virtual object GetAnswer()
        {
            return null;
        }

        public virtual string GetQuestion()
        {
            return question.Text;
        }

        public virtual bool CompareAnswer(object answer)
        {
            return false;
        }

    }
}
