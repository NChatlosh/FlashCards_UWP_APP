using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards
{
   public class MultChoiceCard : FlashCard
    {
        public MultChoiceCard(Question q) : base(q)
        {
        }

        public MultChoiceCard()
        {

        }
        public override object GetAnswer()
        {
            return question.answer.Options;
        }
        public override string GetRealAnswerText()
        {
            return question.answer.RealAnswer;
        }

        public override bool CompareAnswer(object answer)
        {
            string ans = (string)answer;
            if (ans == question.answer.RealAnswer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
