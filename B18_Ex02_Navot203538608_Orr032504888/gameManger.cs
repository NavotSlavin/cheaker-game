using System;
using System.Collections.Generic;
using System.Linq;

namespace B18_Ex02_Navot203538608_Orr032504888
{
    public class Player
    {
        public string m_Name;
        public int m_Score;
        public int m_OverAllScore;
        public Boolean m_IsComputer;
        public Soilder.Group m_Group;
        public Player()
        {
            this.m_Name = "";
            this.m_Score = 0;
            this.m_OverAllScore = 0;
            this.m_IsComputer = false;
            this.m_Group = Soilder.Group.Non;
        }

        public Player(String i_Name, Boolean i_IsComputer, Soilder.Group i_Colur)
        {
            this.m_Name = i_Name;
            this.m_Score = 0;
            this.m_OverAllScore = 0;
            this.m_IsComputer = i_IsComputer;
            this.m_Group = i_Colur;
        }
    }
    class GameManger
    {
        //game manger data members 
        public Player player1;
        public Player player2;
        public CheckersBoard playingBoard;
        public Player turn;
        public Player prevTurn;
        public Boolean endGameCondision;
        public Boolean endtournemnetCondision;
        public Move previosMove;

        public void startGame()
        {
            //create user interface 
            UserInterface userInterface = new UserInterface();

            //welcome message
            userInterface.GameEntry();

            //initealize player 
            String playerName = userInterface.getPlayerName();
            this.player1 = new Player(playerName, false, Soilder.Group.Black);

            //get board size
            int boardSize = userInterface.getSizeOfBoard();

            //set player two 
            if (userInterface.isPlayer2Computer())
            {
                player2 = new Player("computer", true, Soilder.Group.White);
            }
            else
            {
                player2 = new Player(userInterface.getPlayerName(), false, Soilder.Group.White);
            }

            this.endtournemnetCondision = false;

            while (!endtournemnetCondision)
            {
                //start the board - needs to move to constractor
                this.playingBoard = new CheckersBoard(boardSize);
                playingBoard.InitWhiteTeam();
                playingBoard.InitBlackTeam();
                this.endGameCondision = false;
                this.turn = player1;
                this.prevTurn = player2;
                //start the game 


                Move PlayerMove = new Move();
                Boolean firstMove = true;

                while (!endGameCondision)   //change to do while
                {
                    //print cuurent sqreen - first time move is empty 
                    userInterface.upDateOutPut(playingBoard, prevTurn, previosMove, turn, firstMove);
                    firstMove = false;
                    Boolean stopTheGame = false; //in case the user wants to quit and can
                    List<Move> currentValidMoves = new List<Move>();
                    if (this.turn.m_Group == Soilder.Group.Black)
                    {
                        currentValidMoves = this.playingBoard.getAllVaildTeamMoves(playingBoard.m_BlackSoilders);
                    }
                    else
                    {
                        currentValidMoves = this.playingBoard.getAllVaildTeamMoves(playingBoard.m_WhiteSoilders);
                    }
                    //if the current player is a computer use AI class 
                    if (turn.m_IsComputer)
                    {
                        PlayerMove = AI.GenerateRandomMove(currentValidMoves);
                    }
                    else
                    {
                        PlayerMove = userInterface.getValidMoveFromUser(currentValidMoves); //if user want out move is an empty move
                        
                        while (userInterface.m_UserWantsToQuit) //uses a public flag to check -maybe we will need to change this 
                        {
                            if (checkIfPlayerCanQuit(turn)) //a gamemaneger methode to calculate score
                            {
                                stopTheGame = true;
                                userInterface.m_UserWantsToQuit = false; //set the flag back to false for next game 
                                break;
                            }
                            else
                            {
                                userInterface.notifyPlayer("You can not quit because you have more or the same number of points ");
                                userInterface.m_UserWantsToQuit = false; //set the flag back to false 
                                PlayerMove = userInterface.getValidMoveFromUser(currentValidMoves);  
                            }
                        }
                    }
                    if (stopTheGame)
                    {
                        break; //end the game and go to score calculation
                    }
                    //move with move or eat
                    if (PlayerMove.m_IsEatingMove)
                    {
                        playingBoard.Eat(PlayerMove);
                        //if last move has more eating moves if it does eat again
                        Boolean stillEating = true;
                        while (stillEating)
                        {
                            Soilder eatingSoilder = playingBoard.soilderAt(PlayerMove.m_To);
                            List<Move> PossibleEatingMoves = playingBoard.getVaildMoves(eatingSoilder);
                            if (PossibleEatingMoves.Count != 0)
                            {
                                if (PossibleEatingMoves.First().m_IsEatingMove)
                                {
                                    userInterface.upDateOutPut(playingBoard, turn, PlayerMove, turn, firstMove); //print the same turn
                                    PlayerMove = userInterface.getValidMoveFromUser(PossibleEatingMoves);
                                    playingBoard.Eat(PlayerMove);
                                }
                                else
                                {
                                    stillEating = false;
                                }
                            }
                            else
                            {
                                stillEating = false;
                            }
                        }
                    }
                    else
                    {
                        playingBoard.Move(PlayerMove);
                    }
                    //up date score
                    upDateScore();
                    //up date winning condision
                    upDateEndGameCondision();
                    //turn change
                    swapTurns(PlayerMove);
                }
                //game ended 
                Player winner = player1;
                Boolean teko = false;
                if (player1.m_Score < player2.m_Score)
                {
                    winner = player2;
                }
                if (player1.m_Score == player2.m_Score)
                {
                    teko = true;
                }
                userInterface.endOfGame(player1, player2, winner, teko);

                //update overall score and ask user if he wants to continue
                upDateOverAllScore();  //this also resets game score
                if (!userInterface.wantAnotherGame())
                {
                    endtournemnetCondision = true;
                }
            }
            //end of over all game - send user final result and termenate program
            Player bigWinner = player1;
            Boolean draw = false;
            if (player1.m_OverAllScore < player2.m_OverAllScore)
            {
                bigWinner = player2;
            }
            if (player1.m_OverAllScore == player2.m_OverAllScore)
            {
                draw = true;
            }
            userInterface.endOftournament(player1, player2, bigWinner, draw);
            Console.WriteLine("press any key to exit");
            Console.Read();
        }

        private void swapTurns(Move i_NewPrevMove)
        {
            this.previosMove = i_NewPrevMove;
            Player pointerToPrevTurn = prevTurn;
            prevTurn = turn;
            turn = pointerToPrevTurn;
        }

        private void upDateOverAllScore()
        {
            player1.m_OverAllScore += player1.m_Score;
            player1.m_Score = 0;
            player2.m_OverAllScore += player2.m_Score;
            player2.m_Score = 0;
        }

        private void upDateEndGameCondision()
        {
            Boolean gameEnded = false;
            //check if all soilders of onr of the groups have been eaten 
            if (playingBoard.m_WhiteSoilders.Count == 0 || playingBoard.m_BlackSoilders.Count == 0)
            {
                gameEnded = true;
            }
            //check if next player has no more moves
            if (prevTurn.m_Group == Soilder.Group.Black)
            {
                if (playingBoard.getAllVaildTeamMoves(playingBoard.m_BlackSoilders).Count == 0)
                {
                    gameEnded = true;
                }
            }
            else
            {
                if (playingBoard.getAllVaildTeamMoves(playingBoard.m_WhiteSoilders).Count == 0)
                {
                    gameEnded = true;
                }
            }
            //check if both sides are left with only one king
            if ((playingBoard.m_WhiteSoilders.Count == 1) && (playingBoard.m_BlackSoilders.Count == 1)
                && (playingBoard.m_WhiteSoilders.First().IsKing) && (playingBoard.m_BlackSoilders.First().IsKing))
            {
                gameEnded = true;
            }
            this.endGameCondision = gameEnded;
        }

        private void upDateScore()
        {
            //count the soilders in the player one list
           int player1Score = playingBoard.m_BlackSoilders.Count;
            foreach (Soilder soilder in playingBoard.m_BlackSoilders)
            {
                if (soilder.IsKing)
                {
                    player1Score += 3; //add three for each king
                }
            }
            player1.m_Score = player1Score;
            //count the soilders in the player 2 list
            int player2Score = playingBoard.m_WhiteSoilders.Count;
            foreach (Soilder soilder in playingBoard.m_WhiteSoilders)
            {
                if (soilder.IsKing)
                {
                    player2Score += 3; //add three for each king
                }
            }
            player2.m_Score = player2Score;
        }
        private bool checkIfPlayerCanQuit(Player i_Turn)
        {
            return (i_Turn.m_Score < prevTurn.m_Score);
        }
    }
}