using System;
using System.Collections.Generic;
using System.Linq;

namespace B18_Ex02_Navot203538608_Orr032504888
{
    class AI
    {
        public static Move GenerateRandomMove(List<Move> legalMoves)  //static methode does not need an object 
        {
            Random random = new Random();                        //generates a random number 
            int randomIndex = random.Next(1, legalMoves.Count());
            return legalMoves.ElementAt(randomIndex - 1);        //return a random move from the list  
        }
    }
}