using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards
{
   public static class Extensions
    {
        //got the code for this method from http://stackoverflow.com/questions/273313/randomize-a-listt
        // the first reply gives this algorthm based on the Fisher-Yates shuffle alrgorthm
        //using this as a static extension class for the purpose of shuffling the flash card list everythime the user plays through the cards
        private static Random random = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int i = list.Count;
            while(i > 1)
            {
                i--;
                int n = random.Next(i+1);
                T value = list[n];
                list[n] = list[i];
                list[i] = value;
            }
        }
    }
}
