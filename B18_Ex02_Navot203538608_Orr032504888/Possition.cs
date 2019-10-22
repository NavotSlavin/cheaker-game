using System;

namespace B18_Ex02_Navot203538608_Orr032504888
{
    public struct Possition
    {
        public Column m_Column;
        public Row m_Row;
        public enum Column { A, B, C, D, E, F, G, H, I, K };
        public enum Row { a, b, c, d, e, f, g, h, i, k };
        //change private or public
        public Possition(int i_Column, int i_Row)
        {
            m_Column = (Column)i_Column;
            m_Row = (Row)i_Row;
        }
        public Boolean isOnBoard(int i_SizeOfBoard)
        {
            Boolean checkOnBoard = true;
            if (((int)m_Column >= i_SizeOfBoard) || ((int)m_Column < 0))
            {
                checkOnBoard = false;
            }
            if ((int)m_Row >= i_SizeOfBoard || (int)(m_Row) < 0)
            {
                checkOnBoard = false;
            }
            return checkOnBoard;
        }

        public String possitionToString()
        {
            String strPossition = "";
            strPossition += m_Column.ToString() + m_Row.ToString();
            return strPossition;
        }
    }
}