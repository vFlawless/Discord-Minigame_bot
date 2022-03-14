using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeDiscordBot.TicTacToeGame
{
    public class AILogic
    {
        private List<Field> gameGrid;
        private List<int> templateList;
        private int turnCounter;
        private static Random r = new Random();

        // Will make a move based on what difficulty that's been selected by the user
        public int MakeMove(List<Field> grid, string difficulty, int turn)
        {
            // Updates the local grid & turn every time the MakeMove method is called
            gameGrid = new List<Field>(grid);
            turnCounter = turn;

            // Checks if it is the bots first move
            if (turnCounter == 0)
            {
                return FirstMove();
            }

            if (turnCounter > 0 && difficulty == "easy")
            {
                int easyBot = InitEasyBot();

                return easyBot != -1 ? easyBot : PlaceRandomInEmpty();
            }

            if (turnCounter > 0 && difficulty == "medium")
            {
                int mediumBot = InitMediumBot();

                return mediumBot != -1 ? mediumBot : PlaceRandomInEmpty();
            }

            if (turnCounter > 0 && difficulty == "hard")
            {
                int hardBot = InitHardBot();

                return hardBot != -1 ? hardBot : PlaceRandomInEmpty();
            }

            return -1;
        }

        // Will iniate the bots first move
        private int FirstMove()
        {
            if (gameGrid[4].FieldValue == 0)
                return 4;
            else
            {
                int[] options = {0, 2, 6, 8};
                return options[r.Next(options.Length)];
            }
        }

        // Will place in an unoccupied spot
        private int PlaceRandomInEmpty()
        {
            int selection;
            do
            {
                selection = r.Next(gameGrid.Count);
            } while (gameGrid[selection].FieldValue != 0);


            return selection;
        }

        // Initiates the easy ai that will try to win and counter the player with 70% chance
        private int InitEasyBot()
        {
            int winGamePossible = CanBlockOrWin(2);

            if (winGamePossible != -1)
                return winGamePossible;

            int blockPlayer = r.Next(1, 11);

            if (blockPlayer <= 7)
            {
                int blockPlayerPossible = CanBlockOrWin(1);

                if (blockPlayerPossible != -1)
                    return blockPlayerPossible;
            }

            int tryToWin = TryToWin();
            if (tryToWin != -1)
                return tryToWin;

            return -1;
        }

        // Initiates the Medium bot with a random counter strategy
        private int InitMediumBot()
        {
            int winGamePossible = CanBlockOrWin(2);

            if (winGamePossible != -1)
                return winGamePossible;

            int blockPlayerPossible = CanBlockOrWin(1);

            if (blockPlayerPossible != -1)
                return blockPlayerPossible;

            if (turnCounter <= 2)
            {
                int checkRandomStrat = r.Next(1, 4);

                switch (checkRandomStrat)
                {
                    case 1:
                        int counterStrat1 = CounterStrat1();
                        if (counterStrat1 != -1)
                            return counterStrat1;
                        break;
                    case 2:
                        int counterStrat2 = CounterStrat2();
                        if (counterStrat2 != -1)
                            return counterStrat2;
                        break;
                    case 3:
                        int counterStrat3 = CounterStrat3();
                        if (counterStrat3 != -1)
                            return counterStrat3;
                        break;
                }
            }

            int tryToWin = TryToWin();
            if (tryToWin != -1)
                return tryToWin;

            return -1;
        }

        // Initiates the Hard bot with all the counter strategies. 
        private int InitHardBot()
        {
            int winGamePossible = CanBlockOrWin(2);

            if (winGamePossible != -1)
                return winGamePossible;

            int blockPlayerPossible = CanBlockOrWin(1);

            if (blockPlayerPossible != -1)
                return blockPlayerPossible;

            if (turnCounter <= 2)
            {
                int counterStrat1 = CounterStrat1();
                if (counterStrat1 != -1)
                    return counterStrat1;

                int counterStrat2 = CounterStrat2();
                if (counterStrat2 != -1)
                    return counterStrat2;

                int counterStrat3 = CounterStrat3();
                if (counterStrat3 != -1)
                    return counterStrat3;
            }

            int tryToWin = TryToWin();
            if (tryToWin != -1)
                return tryToWin;

            return -1;
        }

        /* Checks if user tries to trap the ai, by placing x in a corner and then in center across from it. for:
            _X_|___|___
            ___|_O_|___
               | X |
        */
        private int CounterStrat1()
        {
            templateList = new List<int>();

            for (int i = 0; i < gameGrid.Count; i++)
            {
                if (gameGrid[i].FieldValue == 1)
                {
                    templateList.Add(i);
                }
            }

            if (templateList.SequenceEqual(new List<int> { 3, 8 }) || templateList.SequenceEqual(new List<int> { 0, 7 }))
                return 6;

            if (templateList.SequenceEqual(new List<int> { 2, 3 }) || templateList.SequenceEqual(new List<int> { 1, 6 }))
                return 0;

            if (templateList.SequenceEqual(new List<int> { 0, 5 }) || templateList.SequenceEqual(new List<int> { 1, 8 }))
                return 2;

            if (templateList.SequenceEqual(new List<int> { 5, 6 }) || templateList.SequenceEqual(new List<int> { 2, 7 }))
                return 8;

            return -1;
        }


        /* Checks if user tries to trap the ai, by placing x in a corner and then in the corner across from it:
            _X_|___|___
            ___|_O_|___
               |   | x
        */
        private int CounterStrat2()
        {
            templateList = new List<int>();

            for (int i = 0; i < gameGrid.Count; i++)
            {
                if (gameGrid[i].FieldValue == 1)
                    templateList.Add(i);
            }

            if (templateList.SequenceEqual(new List<int> { 0, 8 }))
                return r.Next(0, 2) == 0 ? 3 : 7;

            if (templateList.SequenceEqual(new List<int> { 2, 6 }))
                return r.Next(0, 2) == 0 ? 1 : 5;

            return -1;
        }

        /* Checks if user tries to trap the ai, by placing x in two center blocks that are next to eachother:
           ___|_x_|___
           _x_|_O_|___
              |   | 
       */
        private int CounterStrat3()
        {
            templateList = new List<int>();

            for (int i = 0; i < gameGrid.Count; i++)
            {
                if (gameGrid[i].FieldValue == 1)
                    templateList.Add(i);
            }

            if(templateList.SequenceEqual(new List<int>{ 1, 3 }))
                return 0;

            if (templateList.SequenceEqual(new List<int> { 1, 5 }))
                return 2;

            if (templateList.SequenceEqual(new List<int> { 3, 7 }))
                return 6;

            if (templateList.SequenceEqual(new List<int> { 5, 7 }))
                return 8;

            return -1;
        }

        // Checks if there's a potential to win by lining up 3 in a row.
        private int TryToWin()
        {
            // Horizontal row 1
            if (gameGrid[0].FieldValue != 1 && gameGrid[1].FieldValue != 1 && gameGrid[2].FieldValue != 1)
            {
                if (gameGrid[0].FieldValue == 0)
                    return 0;
                if (gameGrid[1].FieldValue == 0)
                    return 1;
                else
                    return 2;
            }

            // Horizontal row 2
            if (gameGrid[3].FieldValue != 1 && gameGrid[4].FieldValue != 1 && gameGrid[5].FieldValue != 1)
            {
                if (gameGrid[3].FieldValue == 0)
                    return 3;
                if (gameGrid[4].FieldValue == 0)
                    return 4;
                else
                    return 5;
            }

            // Horizontal row 3
            if (gameGrid[6].FieldValue != 1 && gameGrid[7].FieldValue != 1 && gameGrid[8].FieldValue != 1)
            {
                if (gameGrid[6].FieldValue == 0)
                    return 6;
                if (gameGrid[7].FieldValue == 0)
                    return 7;
                else
                    return 8;
            }

            // Vertical row 1
            if (gameGrid[0].FieldValue != 1 && gameGrid[3].FieldValue != 1 && gameGrid[6].FieldValue != 1)
            {
                if (gameGrid[0].FieldValue == 0)
                    return 0;
                if (gameGrid[3].FieldValue == 0)
                    return 3;
                else
                    return 6;
            }

            // Vertical row 2
            if (gameGrid[1].FieldValue != 1 && gameGrid[4].FieldValue != 1 && gameGrid[7].FieldValue != 1)
            {
                if (gameGrid[1].FieldValue == 0)
                    return 1;
                if (gameGrid[4].FieldValue == 0)
                    return 4;
                else
                    return 7;
            }

            // Vertical row 3
            if (gameGrid[2].FieldValue != 1 && gameGrid[5].FieldValue != 1 && gameGrid[8].FieldValue != 1)
            {
                if (gameGrid[2].FieldValue == 0)
                    return 2;
                if (gameGrid[5].FieldValue == 0)
                    return 5;
                else
                    return 8;
            }

            // Diagonal row 1
            if (gameGrid[0].FieldValue != 1 && gameGrid[4].FieldValue != 1 && gameGrid[8].FieldValue != 1)
            {
                if (gameGrid[0].FieldValue == 0)
                    return 0;
                if (gameGrid[4].FieldValue == 0)
                    return 4;
                else
                    return 8;
            }

            // Diagonal row 2
            if (gameGrid[2].FieldValue != 1 && gameGrid[4].FieldValue != 1 && gameGrid[6].FieldValue != 1)
            {
                if (gameGrid[2].FieldValue == 0)
                    return 2;
                if (gameGrid[4].FieldValue == 0)
                    return 4;
                else
                    return 6;
            }

            return -1;
        }

        // Checks if it can either block the player or win the game going through a series of checks.
        // I'm sure this can be optimized in some way
        private int CanBlockOrWin(int num)
        {
            // Horizontal row 1
            if (gameGrid[0].FieldValue == num && gameGrid[1].FieldValue == num && gameGrid[2].FieldValue == 0 ||
                gameGrid[0].FieldValue == num && gameGrid[1].FieldValue == 0 && gameGrid[2].FieldValue == num ||
                gameGrid[0].FieldValue == 0 && gameGrid[1].FieldValue == num && gameGrid[2].FieldValue == num)
            {
                if (gameGrid[0].FieldValue == 0)
                    return 0;
                else if (gameGrid[1].FieldValue == 0)
                    return 1;
                else
                    return 2;
            }

            // Horizontal row 2
            if (gameGrid[3].FieldValue == num && gameGrid[4].FieldValue == num && gameGrid[5].FieldValue == 0 ||
                gameGrid[3].FieldValue == num && gameGrid[4].FieldValue == 0 && gameGrid[5].FieldValue == num ||
                gameGrid[3].FieldValue == 0 && gameGrid[4].FieldValue == num && gameGrid[5].FieldValue == num)
            {
                if (gameGrid[3].FieldValue == 0)
                    return 3;
                else if (gameGrid[4].FieldValue == 0)
                    return 4;
                else
                    return 5;
            }

            // Horizontal row 3
            if (gameGrid[6].FieldValue == num && gameGrid[7].FieldValue == num && gameGrid[8].FieldValue == 0 ||
                gameGrid[6].FieldValue == num && gameGrid[7].FieldValue == 0 && gameGrid[8].FieldValue == num ||
                gameGrid[6].FieldValue == 0 && gameGrid[7].FieldValue == num && gameGrid[8].FieldValue == num)
            {
                if (gameGrid[6].FieldValue == 0)
                    return 6;
                else if (gameGrid[7].FieldValue == 0)
                    return 7;
                else
                    return 8;
            }

            // Vertical row 1
            if (gameGrid[0].FieldValue == num && gameGrid[3].FieldValue == num && gameGrid[6].FieldValue == 0 ||
                gameGrid[0].FieldValue == num && gameGrid[3].FieldValue == 0 && gameGrid[6].FieldValue == num ||
                gameGrid[0].FieldValue == 0 && gameGrid[3].FieldValue == num && gameGrid[6].FieldValue == num)
            {
                if (gameGrid[0].FieldValue == 0)
                    return 0;
                else if (gameGrid[3].FieldValue == 0)
                    return 3;
                else
                    return 6;
            }

            // Vertical row 2
            if (gameGrid[1].FieldValue == num && gameGrid[4].FieldValue == num && gameGrid[7].FieldValue == 0 ||
                gameGrid[1].FieldValue == num && gameGrid[4].FieldValue == 0 && gameGrid[7].FieldValue == num ||
                gameGrid[1].FieldValue == 0 && gameGrid[4].FieldValue == num && gameGrid[7].FieldValue == num)
            {
                if (gameGrid[1].FieldValue == 0)
                    return 1;
                else if (gameGrid[4].FieldValue == 0)
                    return 4;
                else
                    return 7;
            }

            // Vertical row 3
            if (gameGrid[2].FieldValue == num && gameGrid[5].FieldValue == num && gameGrid[8].FieldValue == 0 ||
                gameGrid[2].FieldValue == num && gameGrid[5].FieldValue == 0 && gameGrid[8].FieldValue == num ||
                gameGrid[2].FieldValue == 0 && gameGrid[5].FieldValue == num && gameGrid[8].FieldValue == num)
            {
                if (gameGrid[2].FieldValue == 0)
                    return 2;
                else if (gameGrid[5].FieldValue == 0)
                    return 5;
                else
                    return 8;
            }

            // Diagonal row 1
            if (gameGrid[0].FieldValue == num && gameGrid[4].FieldValue == num && gameGrid[8].FieldValue == 0 ||
                gameGrid[0].FieldValue == num && gameGrid[4].FieldValue == 0 && gameGrid[8].FieldValue == num ||
                gameGrid[0].FieldValue == 0 && gameGrid[4].FieldValue == num && gameGrid[8].FieldValue == num)
            {
                if (gameGrid[0].FieldValue == 0)
                    return 0;
                else if (gameGrid[4].FieldValue == 0)
                    return 4;
                else
                    return 8;
            }

            // Diagonal row 2
            if (gameGrid[2].FieldValue == num && gameGrid[4].FieldValue == num && gameGrid[6].FieldValue == 0 ||
                gameGrid[2].FieldValue == num && gameGrid[4].FieldValue == 0 && gameGrid[6].FieldValue == num ||
                gameGrid[2].FieldValue == 0 && gameGrid[4].FieldValue == num && gameGrid[6].FieldValue == num)
            {
                if (gameGrid[2].FieldValue == 0)
                    return 2;
                else if (gameGrid[4].FieldValue == 0)
                    return 4;
                else
                    return 6;
            }

            return -1;
        }
    }
}
