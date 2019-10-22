using System;

namespace B18_Ex02_Navot203538608_Orr032504888
{
    public struct Move
    {
        public Possition m_From;        //this is the starting point
        public Possition m_To;           //this is the ending point  
        public Boolean m_IsEatingMove;   //marks a move as an eating move
        
        public Move(Possition i_From, Possition i_To, Boolean i_Eat)    
        {
            this.m_From = i_From;
            this.m_To = i_To;
            m_IsEatingMove = i_Eat;
        }
        public String MoveToString()   //for printing moves to console 
        {
            String strMove = "";
            strMove += m_From.possitionToString() + ">" + m_To.possitionToString();
            return strMove;
        }
    }
}