using B18_Ex02_Navot203538608_Orr032504888;
using System;
using System.Collections.Generic;

namespace B18_Ex02_Navot203538608_Orr032504888
{
    public class UserInterface
    {

        //data members 

        public Boolean m_UserWantsToQuit;

        public UserInterface()
        {
            m_UserWantsToQuit = false;
        }

        public void GameEntry()
        {
            Console.WriteLine("Welcome to Damka Game! may the odds be ever in your favor");
        }

        //field player1Name is valid and holds the name
        public String getPlayerName()
        {
            String playerName = "";
            Console.WriteLine("Enter player name");
            playerName = Console.ReadLine();
            while (!nameIsValid(playerName))
            {
                Console.WriteLine("Name is invalid, please enter another name");
                playerName = Console.ReadLine();
            }
            return playerName;
        }

        //field sizeOfBoard
        public int getSizeOfBoard()
        {
            int sizeOfBoard = 0;
            String sizeOfBoardStr = "";
            Console.WriteLine("Choose the size of the board(6, 8, 10)");
            sizeOfBoardStr = Console.ReadLine();
            while (!sizeOfBoardIsValid(sizeOfBoardStr))
            {
                Console.WriteLine("number isn't valid, enter another number");
                sizeOfBoardStr = Console.ReadLine();
            }
            sizeOfBoard = int.Parse(sizeOfBoardStr);
            return sizeOfBoard;
        }

        //computer or player 2 field
        public Boolean isPlayer2Computer()
        {
            Console.WriteLine("Do you want to play against the computer? y/n");
            String answer = Console.ReadLine();
            Boolean player2IsComputer = false;
            Boolean Continue = false;
            while (!Continue)
            {
                if (answer.Length == 1 && answer[0] == 'y')
                {
                    player2IsComputer = true;
                    break;
                }
                else if (answer.Length == 1 && answer[0] == 'n')
                {
                    player2IsComputer = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Answer isn't valid, please enter y/n");
                }
                answer = Console.ReadLine();
            }
            return player2IsComputer;
        }

        public Boolean userWantToQuit()
        {
            Console.WriteLine("Do you want to quit y/n?");
            String answer = Console.ReadLine();
            Boolean correctAnswer = false;
            if (validAnswer(answer))
            {
                if (answer[0] == 'y')
                {
                    this.m_UserWantsToQuit = true;
                    correctAnswer = true;
                }
                else
                {
                    correctAnswer = false;
                }
            }
            return correctAnswer;
        }

        public Boolean wantAnotherGame()
        {
            Console.WriteLine("Do you want to play another game y/n?");
            String answer = Console.ReadLine();
            Boolean correctAnswer = false;
            if (validAnswer(answer))
            {
                if (answer[0] == 'y')
                {
                    correctAnswer = true;
                }
                else
                {
                    correctAnswer = false;
                }
            }
            return correctAnswer;
        }

        //clear board with dll
        public void upDateOutPut(CheckersBoard i_Board, Player i_PrevPlayer, Move i_PrevMove, Player i_CurrentPlayer, Boolean i_FirstMove)
        //if you recive no previus move it means first message format  
        {
            if (i_FirstMove)
            {
                i_Board.printBoardToConsole();
                Console.WriteLine(i_CurrentPlayer.m_Name + "'s " + "turn:");
            }
            else
            {
                i_Board.printBoardToConsole();
                Console.WriteLine(i_PrevPlayer.m_Name + "'s move was (" + teamSign(i_PrevPlayer) + "):" + i_PrevMove.MoveToString());
                Console.WriteLine(i_CurrentPlayer.m_Name + "'s " + "Turn" + "(" + teamSign(i_CurrentPlayer) + "):");
            }
        }

        //print the string to console
        public void notifyPlayer(string i_StringToPrint)
        {
            Console.WriteLine(i_StringToPrint);
        }

        //show the results of the game
        public void endOfGame(Player i_Player1, Player i_Player2, Player i_Winner, Boolean i_IsTie)
        {
            String resultOfGame = "";
            //player 1 win game
            if (!i_IsTie)
            {
                resultOfGame = String.Format(
               "End of game, {5} won.{0}{1} number of points: {2}{0}{3} number of points: {4}", Environment.NewLine, i_Player1.m_Name,
               i_Player1.m_Score, i_Player2.m_Name, i_Player2.m_Score, i_Winner.m_Name);
                //player 2 win game
            }
            else //tie game
            {
                resultOfGame = String.Format("End of game, game is tie.{0}{1} number of points: {2}{0}{3} number of points: {4}", Environment.NewLine,
                i_Player1.m_Name, i_Player1.m_Score, i_Player2.m_Name, i_Player2.m_Score, i_Player2.m_Name);
            }
            Console.WriteLine(resultOfGame);
        }

        //show the result of the tournement
        public void endOftournament(Player i_Player1, Player i_Player2, Player i_Winner, Boolean i_IsTie)
        {
            String resultOfGame = "";
            //player 1 win game
            if (!i_IsTie)
            {
                resultOfGame = String.Format(
               "End of game, {5} won.{0}{1} number of points: {2}{0}{3} number of points: {4}", Environment.NewLine, i_Player1.m_Name,
               i_Player1.m_OverAllScore, i_Player2.m_Name, i_Player2.m_OverAllScore, i_Winner.m_Name);
                //player 2 win game
            }
            else //tie game
            {
                resultOfGame = String.Format("End of game, game is tie.{0}{1} number of points: {2}{0}{3} number of points: {4}", Environment.NewLine,
                i_Player1.m_Name, i_Player1.m_OverAllScore, i_Player2.m_Name, i_Player2.m_OverAllScore, i_Player2.m_Name);
            }
            Console.WriteLine(resultOfGame);
        }
        private Boolean moveIsValidSyntax(String i_PlayerMove)
        {
            Boolean validFlag = true;
            char[] player1Arr = i_PlayerMove.ToCharArray();
            if (i_PlayerMove.Length != 5)
            {
                validFlag = false;
            }
            else
            {
                for (int i = 0; i < i_PlayerMove.Length; i++)
                {
                    if (i == 0 || i == 3)
                    {
                        if (player1Arr[i] <= 64 || player1Arr[i] > 90)
                        {
                            validFlag = false; //the char isn't capital letter
                        }
                    }
                    if (i == 1 || i == 4)
                    {
                        if (player1Arr[i] < 97 || player1Arr[i] > 122)
                        {
                            validFlag = false; // the char isn't small letter
                        }
                    }
                    if (i == 2)
                    {
                        if (player1Arr[i] != 62)
                        {
                            validFlag = false; //the char isn't '>'
                        }
                    }
                }
            }
            return validFlag;
        }
        public Move getValidMoveFromUser(List<Move> i_LegalMove)
        {
            String moveToPlay = "";
            Move currentMove = new Move();
            Boolean correctMove = false;
            //Console.WriteLine("Please enter next move.");
            //checks if the move is leagal from all the moves the players has
            while (!correctMove)
            {
                moveToPlay = Console.ReadLine();
                //one of the players want to quit
                if ((moveToPlay.Length == 1) && moveToPlay[0] == 'Q')
                {
                    this.m_UserWantsToQuit = true;
                    break;
                }
                //checks the syntax of the move
                if (moveIsValidSyntax(moveToPlay))
                {
                    currentMove = strToMove(moveToPlay);
                    if (i_LegalMove.Contains(currentMove))
                    {
                        correctMove = true;
                    }
                    else
                    {
                        Console.WriteLine("Ilegal Move . Please try again: ");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Move format (COLrow>COLrow). Please try again: ");
                }
            }
            return currentMove;
        }

        //list conatins move

        public Move strToMove(String i_MoveToPlay)
        {
            int col1 = i_MoveToPlay[0] - 65;
            int row1 = i_MoveToPlay[1] - 97;
            int col2 = i_MoveToPlay[3] - 65;
            int row2 = i_MoveToPlay[4] - 97;
            Boolean isEatingMove = false;
            Possition from = new Possition(col1, row1);
            Possition to = new Possition(col2, row2);
            if (Math.Abs(col1 - col2) == 2 && Math.Abs(row1 - row2) == 2)
            {
                isEatingMove = true;
            }
            Move nextMove = new Move(from, to, isEatingMove);
            return nextMove;
        }

        private String teamSign(Player i_PlayerGroup)
        {
            String signOfPlayerGroup = "";
            if (i_PlayerGroup.m_Group == Soilder.Group.Black)
            {
                signOfPlayerGroup = "X";
            }
            else
            {
                signOfPlayerGroup = "O";
            }
            return signOfPlayerGroup;
        }


        private Boolean nameIsValid(String i_PlayerName)
        {
            Boolean nameFlag = true;
            if (i_PlayerName.Length > 20 || i_PlayerName == "")
            {
                nameFlag = false;
            }
            return nameFlag;
        }
        private Boolean sizeOfBoardIsValid(String i_SizeOfBoard)
        {
            Boolean sizeFlag = true;
            if (i_SizeOfBoard != "6" && i_SizeOfBoard != "8" && i_SizeOfBoard != "10")
            {
                sizeFlag = false;
            }
            return sizeFlag;
        }

        private Boolean validAnswer(String i_TempAnswer)
        {
            String answer = i_TempAnswer;
            Boolean correctAnswer = false;
            while (!correctAnswer)
            {
                if (answer.Length != 1)
                {
                    Console.WriteLine("Answer isn't valid, please enter y/n");
                }
                else if (answer[0] == 'y')
                {
                    correctAnswer = true;
                }
                else if (answer[0] == 'n')
                {
                    correctAnswer = true;
                }
                else
                {
                    Console.WriteLine("Answer isn't valid, please enter y/n");
                }

                answer = Console.ReadLine();
            }
            return correctAnswer;

        }

    }
}