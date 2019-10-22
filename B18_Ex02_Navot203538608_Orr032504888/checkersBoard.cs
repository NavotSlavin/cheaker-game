using System;
using System.Collections.Generic;

namespace B18_Ex02_Navot203538608_Orr032504888
{
    public class CheckersBoard
    {
        //data members 
        private int m_Size;
        public Soilder[,] m_board;           //2D array of soilders
        public List<Soilder> m_WhiteSoilders;
        public List<Soilder> m_BlackSoilders;
        public CheckersBoard()
        {
            m_Size = 0;
            m_board = null;
            m_WhiteSoilders = new List<Soilder>();
            m_BlackSoilders = new List<Soilder>();
        }
        public CheckersBoard(int i_Size)
        {
            m_Size = i_Size;
            m_board = new Soilder[m_Size, m_Size];
            m_WhiteSoilders = new List<Soilder>();
            m_BlackSoilders = new List<Soilder>();
        }
        //public properties
        public int Size
        {
            get
            {
                return m_Size;
            }
            set
            {
                m_Size = value;
            }
        }
        public void Move(Move i_Move)
        {
            //use move to change the board  
            m_board[(int)i_Move.m_To.m_Row, (int)i_Move.m_To.m_Column] = m_board[(int)i_Move.m_From.m_Row, (int)i_Move.m_From.m_Column];
            //update the possition soilder
            Possition temp = i_Move.m_To;
            soilderAt(i_Move.m_To).Possition = temp;
            m_board[(int)i_Move.m_From.m_Row, (int)i_Move.m_From.m_Column] = null;
            //update is king field 
            upDateIsKing(soilderAt(i_Move.m_To));
        }
        private void upDateIsKing(Soilder i_Soilder)
        {
            if (((int)i_Soilder.m_Possition.m_Row == 0) || ((int)i_Soilder.m_Possition.m_Row == m_Size - 1)) //end of board condision
            {
                i_Soilder.IsKing = true;
            }
        }
        public void Eat(Move i_Move)
        {
            //move the to the eating piece
            m_board[(int)i_Move.m_To.m_Row, (int)i_Move.m_To.m_Column] = m_board[(int)i_Move.m_From.m_Row, (int)i_Move.m_From.m_Column];
            //update the possition soilder
            Possition temp = i_Move.m_To;
            soilderAt(i_Move.m_To).Possition = temp;
            m_board[(int)i_Move.m_From.m_Row, (int)i_Move.m_From.m_Column] = null;
            upDateIsKing(soilderAt(i_Move.m_To));
            //find the dead soilder - remove from board
            Soilder toBeRemoved;
            int xVal = (int)i_Move.m_From.m_Row - (int)i_Move.m_To.m_Row;
            int yVal = (int)i_Move.m_From.m_Column - (int)i_Move.m_To.m_Column;
            if (xVal < 0) //this means we are eating down
            {
                if (yVal < 0) //this means we are eating down and right
                {
                    toBeRemoved = m_board[(int)i_Move.m_From.m_Row + 1, (int)i_Move.m_From.m_Column + 1];
                }
                else         //this means we are eating down and left 
                {
                    toBeRemoved = m_board[(int)i_Move.m_From.m_Row + 1, (int)i_Move.m_From.m_Column - 1];
                }
            }
            else       //this means we are eating up 
            {
                if (yVal < 0) //this means we are eating up and right
                {
                    toBeRemoved = m_board[(int)i_Move.m_From.m_Row - 1, (int)i_Move.m_From.m_Column + 1];
                }
                else         //this means we are eating up and left 
                {
                    toBeRemoved = m_board[(int)i_Move.m_From.m_Row - 1, (int)i_Move.m_From.m_Column - 1];
                }
            }
            this.DeleteSoilder(toBeRemoved);
        }
        public List<Move> getAllVaildTeamMoves(List<Soilder> i_Soilders)
        {
            List<Move> allVaildMoves = new List<Move>();  //an empty list in case ther are no valid moves 
            foreach (Soilder Mover in i_Soilders)         //runs on all soilders for valid moves
            {
                allVaildMoves.AddRange(this.getVaildMoves(Mover)); //union with all valid moves 
            }
            allVaildMoves = fillterMoves(allVaildMoves);
            return allVaildMoves;
        }

        private List<Move> fillterMoves(List<Move> i_Moves)
        {
            Boolean MustMove = false;
            List<Move> onlyEatingMoves = new List<Move>();
            //checks if any of the moves are eating moves and updates a flag
            foreach (Move move in i_Moves)
            {
                if (move.m_IsEatingMove == true)
                {
                    MustMove = true;
                    break;
                }
            }
            if (MustMove)  //if one eating move found is a eating move remove all non eating moves
            {
                foreach (Move move in i_Moves)
                {
                    if (move.m_IsEatingMove)
                    {
                        onlyEatingMoves.Add(move);
                    }
                }
                i_Moves = onlyEatingMoves;
            }
            return i_Moves;
        }

        public List<Move> getVaildMoves(Soilder i_Mover)
        {
            List<Move> validMovesForOnePeice = new List<Move>();
            if (i_Mover.Team == Soilder.Group.White || i_Mover.IsKing == true) //if the mover is king color doesnt matter
            {
                validMovesForOnePeice.AddRange(createWhitePlayerMoves(i_Mover));
                validMovesForOnePeice.AddRange(createWhitePlayerEatingMoves(i_Mover));
            }
            if (i_Mover.Team == Soilder.Group.Black || i_Mover.IsKing == true)
            {
                validMovesForOnePeice.AddRange(createBlackPlayerMoves(i_Mover));
                validMovesForOnePeice.AddRange(createBlackPlayerEatingMoves(i_Mover));
            }
            validMovesForOnePeice = fillterMoves(validMovesForOnePeice); //if one of the moves is an eating move remove all non eating moves
            return validMovesForOnePeice;
        }

        private List<Move> createBlackPlayerMoves(Soilder i_Mover)
        {
            Move newLegalMove;
            List<Move> moves = new List<Move>();        //an empty list to hold possible moves 
            //create the two possible possitions in the right diriction
            Possition right_up = new Possition((int)i_Mover.m_Possition.m_Column + 1, (int)i_Mover.m_Possition.m_Row - 1);
            Possition left_up = new Possition((int)i_Mover.m_Possition.m_Column - 1, (int)i_Mover.m_Possition.m_Row - 1);
            //check in both directions that the wanted square is empty
            if (possitionOnBoardAndEmpty(right_up, m_Size))
            {
                newLegalMove = new Move(i_Mover.m_Possition, right_up,false);//set as not eating move
                moves.Add(newLegalMove);
            }
            if (possitionOnBoardAndEmpty(left_up, m_Size))
            {
                newLegalMove = new Move(i_Mover.m_Possition, left_up,false);//set as not eating move
                moves.Add(newLegalMove);
            }
            return moves;
        }

        private List<Move> createBlackPlayerEatingMoves(Soilder i_Mover)
        {
            Move newLegalMove;
            List<Move> moves = new List<Move>(); //an empty list to hold possible moves
            //create the two possible possitions in the right diriction and possions of the victim 
            Possition double_right_up = new Possition((int)i_Mover.m_Possition.m_Column + 2, (int)i_Mover.m_Possition.m_Row - 2);
            Possition double_left_up = new Possition((int)i_Mover.m_Possition.m_Column - 2, (int)i_Mover.m_Possition.m_Row - 2);
            Possition victem_right_up = new Possition((int)i_Mover.m_Possition.m_Column + 1, (int)i_Mover.m_Possition.m_Row - 1);
            Possition victem_left_up = new Possition((int)i_Mover.m_Possition.m_Column - 1, (int)i_Mover.m_Possition.m_Row - 1);
            //check in both directions that the wanted square is empty on board , that victem existis, and is of a different group  
            if (possitionOnBoardAndEmpty(double_right_up, m_Size) &&       //if the sqare is on the board and free
               (soilderAt(victem_right_up) != null) &&                    //if the victim sqare is not null 
               (i_Mover.Team != soilderAt(victem_right_up).Team))  //if the victem is of a differnt team
            {
                newLegalMove = new Move(i_Mover.m_Possition, double_right_up,true); //set as eating move
                moves.Add(newLegalMove);
            }
            if (possitionOnBoardAndEmpty(double_left_up, m_Size) &&       //if the sqare is on the board and free
               (soilderAt(victem_left_up) != null) &&                    //if the victim sqare is not null 
               (i_Mover.Team != soilderAt(victem_left_up).Team))  //if the victem is of a differnt team
            {
                newLegalMove = new Move(i_Mover.m_Possition, double_left_up,true); //set as eating move
                moves.Add(newLegalMove);
            }
            return moves;
        }

        private List<Move> createWhitePlayerEatingMoves(Soilder i_Mover)
        {
            Move newLegalMove;
            //an empty list to hold possible moves 
            List<Move> moves = new List<Move>();
            //create the two possible possitions in the right diriction and possions of the victim 
            Possition double_right_down = new Possition((int)i_Mover.m_Possition.m_Column + 2, (int)i_Mover.m_Possition.m_Row + 2);
            Possition double_left_down = new Possition((int)i_Mover.m_Possition.m_Column - 2, (int)i_Mover.m_Possition.m_Row + 2);
            Possition victem_right_down = new Possition((int)i_Mover.m_Possition.m_Column + 1, (int)i_Mover.m_Possition.m_Row + 1);
            Possition victem_left_down = new Possition((int)i_Mover.m_Possition.m_Column - 1, (int)i_Mover.m_Possition.m_Row + 1);

            //check in both directions that the wanted square is empty on board , that victem existis, and is of a different group  
            if (possitionOnBoardAndEmpty(double_right_down, m_Size) &&       //if the sqare is on the board and free
               (soilderAt(victem_right_down) != null) &&                    //if the victim sqare is not null 
               (i_Mover.Team != soilderAt(victem_right_down).Team))  //if the victem is of a differnt team
            {
                newLegalMove = new Move(i_Mover.m_Possition, double_right_down,true); //set as eating move
                moves.Add(newLegalMove);
            }
            if (possitionOnBoardAndEmpty(double_left_down, m_Size) &&     //if the sqare is on the board and free
               (soilderAt(victem_left_down) != null) &&                    //if the victim sqare is not null 
               (i_Mover.Team != soilderAt(victem_left_down).Team))  //if the victem is of a differnt team
            {
                newLegalMove = new Move(i_Mover.m_Possition, double_left_down,true); //set as eating move
                moves.Add(newLegalMove);
            }
            return moves;
        }
        private List<Move> createWhitePlayerMoves(Soilder i_Mover)
        {
            Move newLegalMove;
            //an empty list to hold possible moves 
            List<Move> moves = new List<Move>();
            //create the two possible possitions in the right diriction
            Possition right_down = new Possition((int)i_Mover.m_Possition.m_Column + 1, (int)i_Mover.m_Possition.m_Row + 1);
            Possition left_down = new Possition((int)i_Mover.m_Possition.m_Column - 1, (int)i_Mover.m_Possition.m_Row + 1);
            //check in both directions that the wanted square is empty
            if (possitionOnBoardAndEmpty(right_down, m_Size))
            {
                newLegalMove = new Move(i_Mover.m_Possition, right_down,false);//set as not eating move
                moves.Add(newLegalMove);
            }
            if (possitionOnBoardAndEmpty(left_down, m_Size))
            {
                newLegalMove = new Move(i_Mover.m_Possition, left_down,false); //set as not eating move
                moves.Add(newLegalMove);
            }
            return moves;
        }

        private bool possitionOnBoardAndEmpty(Possition i_Possition, int i_SizeOfBoard)
        {
            Boolean answer = false;
            //check if possition is on board 
            if (i_Possition.isOnBoard(i_SizeOfBoard))
            {
                answer = true;
            }
            if (answer)
            {
                answer = (m_board[(int)i_Possition.m_Row, (int)i_Possition.m_Column] == null); //checks if a given position on the board is empty
            }
            return answer;
        }
        public Soilder soilderAt(Possition i_Position)
        {
            return m_board[(int)i_Position.m_Row, (int)i_Position.m_Column];
        }

        public void printBoardToConsole()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            int size = this.m_Size + 1;   //+1 place for letters
            Console.Write("   ");         //for indentetion

            //first loop only for private case for first row
            //print the first row A B C ...
            char colLetter = 'A';
            for (int j = 0; j < this.m_Size; j++)
            {
                Console.Write(string.Format("{0}   ", colLetter));
                colLetter++;
            }

            Console.Write(Environment.NewLine);
            Console.Write(" "); //for indentetion

            //print the rest of the board
            for (int k = 0; k < (this.m_Size * 4) + 1; k++)
            {
                Console.Write("=");
            }
            Console.Write(Environment.NewLine);

            char rowLetter = 'a';
            for (int i = 0; i < this.m_Size; i++)
            {
                String tempRow = string.Format("{0}|{1}", rowLetter, getCurrentRowToString(i));
                rowLetter++;
                Console.Write(tempRow);

                Console.Write(Environment.NewLine);
                Console.Write(" "); //for indentetion

                for (int k = 0; k < (this.m_Size * 4) + 1; k++)
                {
                    Console.Write("=");
                }
                Console.Write(Environment.NewLine);
            }
        }

        private String getCurrentRowToString(int i_Row)
        {
            String answer = "";
            for (int i = 0; i < this.Size; i++)
            {
                answer += string.Format(" {0} |", checkForPlayingPiece(i_Row, i));
            }
            return answer;
        }

        private string checkForPlayingPiece(int i_Row, int i_Column)
        {
            string sqaure;
            if (m_board[i_Row, i_Column] != null)
            {
                Soilder target = this.m_board[i_Row, i_Column];
                if (target.Team == Soilder.Group.Black)
                {
                    if (target.IsKing)
                    {
                        sqaure = "K";
                    }
                    else
                    {
                        sqaure = "X";
                    }
                }
                else
                {
                    if (target.IsKing)
                    {
                        sqaure = "U";
                    }
                    else
                    {
                        sqaure = "O";
                    }
                }
            }
            else
            {
                sqaure = " ";
            }
            return sqaure;
        }
        public void InsertSoilder(Soilder i_Soilder)
        {
            //adding soilder to the correct list
            if (i_Soilder.Team == Soilder.Group.Black)
            {
                m_BlackSoilders.Add(i_Soilder);
            }
            else
            {
                m_WhiteSoilders.Add(i_Soilder);
            }
            //adding soilder to the board
            m_board[(int)i_Soilder.Possition.m_Row, (int)i_Soilder.Possition.m_Column] = i_Soilder;
        }

        public void DeleteSoilder(Soilder i_Soilder)
        {
            //removing soilder from the correct list
            if (i_Soilder.Team == Soilder.Group.Black)
            {
                m_BlackSoilders.Remove(i_Soilder);
            }
            else
            {
                m_WhiteSoilders.Remove(i_Soilder);
            }
            m_board[(int)i_Soilder.Possition.m_Row, (int)i_Soilder.Possition.m_Column] = null;
        }
        public void InitBlackTeam()
        {
            for (int i = ((this.m_Size + 2) / 2); i < this.m_Size; i++)
            {
                for (int j = 0; j < this.m_Size; j++)
                {
                    if (i % 2 != 0)
                    {
                        if (j % 2 == 0)
                        {
                            Possition tempPossition = new Possition(j, i);
                            Soilder tempSoilder = new Soilder(Soilder.Group.Black, tempPossition);
                            InsertSoilder(tempSoilder);
                        }
                    }
                    else
                    {
                        if (j % 2 != 0)
                        {
                            Possition tempPossition = new Possition(j, i);
                            Soilder tempSoilder = new Soilder(Soilder.Group.Black, tempPossition);
                            InsertSoilder(tempSoilder);
                        }

                    }
                }
            }
        }
        public void InitWhiteTeam()
        {
            for (int i = 0; i < (this.m_Size - 2) / 2; i++)
            {
                for (int j = 0; j < this.m_Size; j++)
                {
                    //if the row is even
                    if (i % 2 == 0)
                    {
                        if (j % 2 != 0)
                        {
                            Possition tempPossition = new Possition(j, i);
                            Soilder tempSoilder = new Soilder(Soilder.Group.White, tempPossition);
                            InsertSoilder(tempSoilder);
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            Possition tempPossition = new Possition(j, i);
                            Soilder tempSoilder = new Soilder(Soilder.Group.White, tempPossition);
                            InsertSoilder(tempSoilder);
                        }
                    }
                }
            }
        }
    }
}