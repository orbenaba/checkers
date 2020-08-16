using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Damka
{
    class Board
    {
        private Cell[,] board;
        public Board()
        {
            board = new Cell[8, 8];
            int x = 400;
            int y = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Cell c = new Cell();
                    c.SetX(x);
                    c.SetY(y);
                    board[i, j] = c;
                    x +=100;
                }
                y += 100;
                x = 400;
            }

            FillBlackCubes();
            FillRedCubes();
        }
        //copy constructor
        public Board(Board other)
        {
            board = new Cell[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Cell c = new Cell(other.board[i, j].GetX(), other.board[i, j].GetY(), other.board[i, j].IsBlack(), other.board[i, j].GetFill());
                    board[i, j] = c;
                }
            }
        }
        public void FillBlackCubes()
        {
            for (int i = 5; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (i % 2 != 0 && j % 2 == 0 || i % 2 == 0 && j % 2 != 0)
                        board[i, j].SetFill(1);
        }

        public void FillRedCubes()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 8; j++)
                    if (i % 2 != 0 && j % 2 == 0 || i % 2 == 0 && j % 2 != 0)
                        board[i, j].SetFill(2);
        }      
        public Cell[,] GetBoard()
        {
            return this.board;
        }

        //score=0->tie
        //score<0->black win
        //score>0->red win
        public int PaintBoard(Graphics g)
        {
            int score = 0,temp;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    board[i, j].PaintCell(g);
                    if (i % 2 == 0 && j % 2 == 0 || i % 2 != 0 && j % 2 != 0)
                        board[i, j].PaintFillCell(g, 0);
                    else
                    {
                        board[i, j].PaintFillCell(g, 1);
                        board[i, j].SetBlack();
                        temp = board[i, j].GetFill();
                        if (temp!= 0)
                        {
                            board[i, j].PaintCube(g);
                            if (temp == 1)//black soldier
                                score--;
                            else if (temp == 2)//red soldier
                                score++;
                            else if (temp == 3)//black king
                                score -= 3;
                            else if (temp == 4)//red king
                                score += 3;
                        }
                    }
                }
            return score;
        }
        public int countRed()
        {
            int count = 0, i, j;
            for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                    if (board[i, j].GetFill() == 2 || board[i, j].GetFill() == 4)
                        count++;
            return count;
        }
        public int countBlack()
        {
            int count = 0, i, j;
            for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                    if (board[i, j].GetFill() == 1 || board[i, j].GetFill() == 3)
                        count++;
            return count;
        }
    }
}

    
