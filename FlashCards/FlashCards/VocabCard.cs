using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards
{
    public class VocabCard : FlashCard
    {
        public VocabCard(Question q) : base(q)
        {
        }

        public VocabCard()
        {

        }

        public override object GetAnswer()
        {
            return question.answer.RealAnswer;
        }
        public override string GetRealAnswerText()
        {
            return this.GetAnswer().ToString();
        }
        public override bool CompareAnswer(object answer)
        {
            string ans = (string)answer;
            if (ans.ToUpper() == question.answer.RealAnswer.ToUpper())
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
