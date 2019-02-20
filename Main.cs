using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace IHelloWord
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            var cobrinha = new Cobrinha(30, 100, 30);
            cobrinha.run();

        }
    }

    class Cobrinha
    {
        private List<int[]> listMove;
        private int mapWidth;
        private int mapHeight;
        private int pp_X = 0;
        private int pp_Y = 0;
        private int milliSeconds;
        private int[,] map;
        private Random random;
        private const int GROUND = 0;
        private const int SNAKE = 1;
        private const int FOOD = 2;
        private enum moveState: int{
            TOP = 0,
            RIGHT = 1,
            BOTTOM = 2,
            LEFT = 3
        }
        //0 -> Top
        //1 -> Right
        //2 -> Bottom
        //3 -> Left
        private int moveDirection = 1;

        public Cobrinha(int mapWidth, int mapHeight, int milliSeconds)
        {
            this.milliSeconds = milliSeconds;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.map = new int[this.mapWidth, this.mapHeight];
            this.random = new Random();

            this.map[this.pp_X, this.pp_Y] = SNAKE;
            this.listMove = new List<int[]>();
            this.listMove.Add(new int[] { 0 ,0});

            Console.SetWindowSize(1200, 1200);

        }

        public void run ()
        {
            this.generateFood();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    switch (key.KeyChar)
                    {

                        case 'w':
                            if (this.moveDirection != (int)moveState.BOTTOM)
                            {
                                this.moveDirection = (int)moveState.TOP;
                            }
                            break;

                        case 'd':
                            if (this.moveDirection != (int)moveState.LEFT)
                            {
                                this.moveDirection = (int)moveState.RIGHT;
                            }
                            break;

                        case 's':
                            if (this.moveDirection != (int)moveState.TOP)
                            {
                                this.moveDirection = (int)moveState.BOTTOM;
                            }
                            break;

                        case 'a':
                            if (this.moveDirection != (int)moveState.RIGHT)
                            {
                                this.moveDirection = (int)moveState.LEFT;
                            }
                            break;
                    }
                }

                //Zerar os caminhos já andados
                //this.cleanMap();
                //Render the map every frame second
                this.printMap();
                this.cleanMap();
                this.move();
                Thread.Sleep(this.milliSeconds);

            }
        }

        private void move()
        {
            switch (this.moveDirection)
            {
                case 0:
                    this.pp_X--;
                    break;
                case 1:
                    this.pp_Y++;
                    break;
                case 2:
                    this.pp_X++;
                    break;
                case 3:
                    this.pp_Y--;
                    break;
            }

            //Se for para fora do mapa, remanejar
            this.outOfMap();

            //Caso Tenha Ocorrido colisao
            this.haveConflict(this.pp_X, this.pp_Y);

            //Add move a list
            this.listMove.Add (new int[] { this.pp_X, this.pp_Y });

            //Verificar se e pixel de comida
            if (this.map[this.pp_X, this.pp_Y] != FOOD)
            {
                this.listMove.RemoveAt(0);
            }
            else
            {
                this.generateFood();
            }

            //Printar a lista no map
            this.movePrint();

        }

        //Print the body parts of snake
        private void movePrint()
        {
            //for (int i = (this.listMove.Count - 1); i >= 1; i--)
            //{
            //    this.map[this.listMove[i][0], this.listMove[i][1]] = SNAKE;
            //}
            for (int i = 0; i < this.listMove.Count; i++)
            {
                this.map[this.listMove[i][0], this.listMove[i][1]] = SNAKE;
            }
        }

        private void outOfMap()
        {
            if (this.pp_X >= this.mapWidth)
            {
                this.pp_X = 0;
            }else if (this.pp_X < 0)
            {
                this.pp_X = (this.mapWidth - 1);
            }else if (this.pp_Y >= this.mapHeight)
            {
                this.pp_Y = 0;
            }else if (this.pp_Y < 0)
            {
                this.pp_Y = (this.mapHeight - 1);
            }
        }

        private void printMap()
        {
            Console.WriteLine("\n\n\n\n");
            for (int i = 0; i < this.mapWidth; i++)
            {
                for (int j = 0; j < this.mapHeight; j++)
                {
                    switch (this.map[i, j])
                    {
                        case GROUND:
                            Console.Write("-" + " ");
                            break;
                        case SNAKE:
                            Console.Write("o" + " ");
                            break;

                        case FOOD:
                            Console.Write("x" + " ");
                            break;
                    }
                }
                Console.WriteLine("");
            }
            Console.WriteLine("\n\n\n\n");
        }

        private bool eatFood()
        {
            return true;
        }

        private void generateFood()
        {
            // var x = this.random.Next(this.mapWidth);
            // var y = this.random.Next(this.mapHeight);
            var list = new List<int[]>();

            for (int i = 0; i < this.mapWidth; i++)
            {
                for (int j = 0; j < this.mapHeight; j++)
                {
                    if (this.map[i, j] == GROUND)
                    {
                        list.Add(new int[] { i, j});
                    }
                }
            }
            var index = this.random.Next(list.Count);
            this.map[list[index][0], list[index][1]] = FOOD;
        }

        private void cleanMap()
        {
            for (int i = 0; i < this.listMove.Count; i++)
            {
                this.map[this.listMove[i][0], this.listMove[i][1]] = GROUND;
            }
        }

        private void haveConflict(int x, int y)
        {
            for (int i = 0; i < this.listMove.Count; i++)
            {
                if (this.listMove[i][0] == x && this.listMove[i][1] == y)
                {
                    this.theEnd();
                }
            }
        }

        private void theEnd() {
            Console.WriteLine("|-------\n|Você perdeu, Acabou xD\n|-------");
            Environment.Exit(0);
            return;
        }


    }

}
