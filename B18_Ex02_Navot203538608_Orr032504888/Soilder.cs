using System;

namespace B18_Ex02_Navot203538608_Orr032504888
{
    public class Soilder
    {
        //data memberes of a single checkers playing pice
        private Group m_Group;           //black or white 
        private Boolean m_IsKing;        // true if the soilder is a king
        public Possition m_Possition;    // the current possition  
        public enum Group { Black, White, Non }

        //defult constructor for a soilder
        public Soilder()
        {
            this.m_Group = 0;
            this.m_IsKing = false;
        }
        //a counstractor for a soilder
        public Soilder(Group i_Team, Possition i_StartingLocation)
        {
            m_Group = i_Team;
            m_Possition = i_StartingLocation;
            m_IsKing = false;
        }

        //public properties
        public Boolean IsKing
        {
            get
            {
                return m_IsKing;
            }
            set
            {
                m_IsKing = value;
            }

        }

        public Group Team
        {
            get
            {
                return m_Group;
            }
            set
            {
                m_Group = value;
            }
        }

        public Possition Possition
        {
            get
            {
                return m_Possition;
            }
            set
            {
                m_Possition = value;
            }
        }

    }
}