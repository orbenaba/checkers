using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
namespace Damka
{
    public partial class Game : Form
    {
        int rexi;
        Graphics g;
        int TotalRed = 0, TotalRedAfter, TotalBlack = 0, TotalBlackAfter;
        Board game = new Board();
        bool ligal = false;
        int oldI, oldJ, newI, newJ;
        int t = 0;
        int countBlackEaten = 0, countRedEaten = 0;
        bool sw = false;
        int sumr = 12;
        int sumb = 12;
        SoundPlayer lose;
        SoundPlayer win;
        SoundPlayer rekasound;
        public Game()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            rexi = game.PaintBoard(g);
            label1.Refresh();
        }

        private void Game_Load(object sender, EventArgs e)
        {
            lose = new SoundPlayer("lose.wav");
            win = new SoundPlayer("win.wav");
            rekasound = new SoundPlayer("rekasound.wav");
        }

        private void Game_MouseDown(object sender, MouseEventArgs e)
        {
            g = CreateGraphics();
            this.ligal = false;//המהלך לא חוקי
            //לבדוק שזה התבצע בתחומי הלוח
            int x = e.X;
            int y = e.Y;
            if (x >= 400 && x <= 1200 && y >= 0 && y <= 800)
            {
                //זיהוי שורה ועמודה
                int j = (x - 400) / 100;
                int i = y / 100;
                oldI = i;
                oldJ = j;
                //האם זוהי משבצת שחורה
                if (game.GetBoard()[i, j].IsBlack())
                {
                    //האם יש בה אבן שמתאימה לתור
                    if ((t == 0 && game.GetBoard()[i, j].GetFill() == 1) || (t == 1 && game.GetBoard()[i, j].GetFill() == 2))
                        ligal = true;
                    if ((t == 0 && game.GetBoard()[i, j].GetFill() == 3) || (t == 1 && game.GetBoard()[i, j].GetFill() == 4))
                        ligal = true;
                }
            }
        }

        private void Game_MouseUp(object sender, MouseEventArgs e)
        {
            int threats = Threats(), flag = 0;//count how much threats
            Board oldBoard = new Board(game);//remember old board for burnts
            TotalBlack = game.countBlack();
            TotalRed = game.countRed();
            g = CreateGraphics();
            sw = false;
            if (ligal == true)
            {
                //האם אנחנו בתחומי המטריצה
                int x = e.X;
                int y = e.Y;
                if (x >= 400 && x <= 1200 && y >= 0 && y <= 800)
                {
                    //זיהוי שורה ועמודה
                    int j = (x - 400) / 100;
                    int i = y / 100;
                    newI = i;
                    newJ = j;
                    KingEats();
                    if (sw == false)
                        KingsMoves();
                    if (sw == false)//מחפש אכילה משולשת
                        TripleEat();
                    if (sw == false)
                        DoubleEat();//מחפש אכילה כפולה
                    if (sw == false)
                        Eat();//מחפש אכילה רגילה
                    if (sw == false)
                    {
                        //האם המשבצת שחורה
                        if (game.GetBoard()[i, j].IsBlack() == true && game.GetBoard()[i, j].GetFill() == 0)
                            //האם המהלך תקין ביחס לתור
                            if ((t == 0 && oldI - newI == 1 && Math.Abs(oldJ - newJ) == 1) || (t == 1 && newI - oldI == 1 && Math.Abs(oldJ - newJ) == 1))
                            {
                                //מבצעים את התור
                                //1. מרוקנים את התא הקודם
                                game.GetBoard()[oldI, oldJ].SetFill(0);
                                //2. ממלאים את התא החדש
                                if (t == 0)//black switch to red
                                {
                                    game.GetBoard()[newI, newJ].SetFill(1);
                                    t = (t * -1) + 1;
                                }
                                else//red switch to black
                                {
                                    game.GetBoard()[newI, newJ].SetFill(2);
                                    t = (t * -1) + 1;
                                }
                            }
                        TotalBlackAfter = game.countBlack();
                        TotalRedAfter = game.countRed();
                        if (t == 0)
                        {
                            for (int h = 0; h < 6; h++)
                            {
                                for (int g = 1; g < 7; g++)
                                {
                                    //red eat like secondiagonal
                                    if (g > 1 && oldBoard.GetBoard()[h, g].GetFill() == 2 && (oldBoard.GetBoard()[h + 1, g - 1].GetFill() == 1 || oldBoard.GetBoard()[h + 1, g - 1].GetFill() == 3) && oldBoard.GetBoard()[h + 2, g - 2].GetFill() == 0)
                                    {
                                        if (game.GetBoard()[h, g].GetFill() == 0&& game.GetBoard()[h + 1, g + 1].GetFill() == 2)
                                        {
                                            game.GetBoard()[h + 1, g + 1].SetFill(0);
                                            label3.Location = new Point(400 + 100 * (g + 1), 100 * (h + 1));
                                            label3.Visible = true;
                                            timer1.Start();
                                            flag = 1;
                                            break;
                                        }
                                    }
                                    //red eat like main diagonal
                                    if (g < 6 && oldBoard.GetBoard()[h, g].GetFill() == 2 && (oldBoard.GetBoard()[h + 1, g + 1].GetFill() == 1 || oldBoard.GetBoard()[h + 1, g + 1].GetFill() == 3) && oldBoard.GetBoard()[h + 2, g + 2].GetFill() == 0)
                                        if (game.GetBoard()[h + 1, g - 1].GetFill() == 2 && game.GetBoard()[h, g].GetFill() == 0)
                                        {
                                            game.GetBoard()[h + 1, g - 1].SetFill(0);
                                            label3.Location = new Point(400 + 100 * (g - 1), 100 * (h + 1));
                                            label3.Visible = true;
                                            timer1.Start();
                                            flag = 1;
                                            break;
                                        }
                                }
                                if (flag == 1)
                                    break;
                            }
                        }
                        if (t == 1)
                        {
                            for (int h = 2; h < 8; h++)
                            {
                                for (int g = 0; g < 8; g++)
                                {
                                    //black eat secondiagonal 
                                    if (g > 0 && g < 6 && oldBoard.GetBoard()[h, g].GetFill() == 1 && (oldBoard.GetBoard()[h - 1, g + 1].GetFill() == 2 || oldBoard.GetBoard()[h - 1, g + 1].GetFill() == 4) && oldBoard.GetBoard()[h - 2, g + 2].GetFill() == 0)
                                        if (game.GetBoard()[h - 1, g - 1].GetFill() == 1 && game.GetBoard()[h, g].GetFill() == 0)
                                        {
                                            game.GetBoard()[h - 1, g - 1].SetFill(0);
                                            label2.Location = new Point(400 + 100 * (g - 1), 100 * (h - 1));
                                            label2.Visible = true;
                                            timer2.Start();
                                            flag = 1;
                                            break;
                                        }
                                    if (g > 1 && g < 7 && oldBoard.GetBoard()[h, g].GetFill() == 1 && (oldBoard.GetBoard()[h - 1, g - 1].GetFill() == 2 || oldBoard.GetBoard()[h - 1, g - 1].GetFill() == 4) && oldBoard.GetBoard()[h - 2, g - 2].GetFill() == 0)
                                        if (game.GetBoard()[h - 1, g + 1].GetFill() == 1 && game.GetBoard()[h, g].GetFill() == 0)
                                        {
                                            game.GetBoard()[h - 1, g + 1].SetFill(0);
                                            label2.Location = new Point(400 + 100 * (g + 1), 100 * (h - 1));
                                            label2.Visible = true;
                                            timer2.Start();
                                            flag = 1;
                                            break;
                                        }
                                }
                                if (flag == 1)
                                    break;
                            }
                        }
                        if (threats > 0)//if you didnt eat but you threat before move
                        {
                            if (t == 1 && flag == 0)//black
                            {
                                if (TotalBlack == TotalBlackAfter)//you need to burn a red 
                                    for (int h = 0; h < 8; h++)
                                    {
                                        for (int g = 0; g < 8; g++)
                                        {
                                            if (h > 1 && g < 6 && oldBoard.GetBoard()[h, g].GetFill() == 1 && (oldBoard.GetBoard()[h - 1, g + 1].GetFill() == 2 || oldBoard.GetBoard()[h - 1, g + 1].GetFill() == 4) && oldBoard.GetBoard()[h - 2, g + 2].GetFill() == 0)
                                            {
                                                game.GetBoard()[h, g].SetFill(0);
                                                label2.Location = new Point(400 + 100 * g, 100 * h);
                                                label2.Visible = true;
                                                timer2.Start();
                                                flag = 1;
                                                break;
                                            }
                                            if (h > 1 && g > 1 && oldBoard.GetBoard()[h, g].GetFill() == 1 && (oldBoard.GetBoard()[h - 1, g - 1].GetFill() == 2 || oldBoard.GetBoard()[h - 1, g - 1].GetFill() == 4) && oldBoard.GetBoard()[h - 2, g - 2].GetFill() == 0)
                                            {
                                                game.GetBoard()[h, g].SetFill(0);
                                                label2.Location = new Point(400 + 100 * g, 100 * h);
                                                label2.Visible = true;
                                                timer2.Start();
                                                flag = 1;
                                                break;
                                            }
                                        }
                                        if (flag == 1)
                                            break;
                                    }
                            }
                            if (t == 0 && flag == 0)
                            {
                                if (TotalRed == TotalRedAfter)//you need to burn black
                                    for (int h = 0; h < 8; h++)
                                    {
                                        for (int g = 0; g < 8; g++)
                                        {
                                            if (h < 6 && g < 6 && oldBoard.GetBoard()[h, g].GetFill() == 2 && (oldBoard.GetBoard()[h + 1, g + 1].GetFill() == 1 || oldBoard.GetBoard()[h + 1, g + 1].GetFill() == 3) && (oldBoard.GetBoard()[h + 2, g + 2].GetFill() == 0))
                                            {
                                                game.GetBoard()[h, g].SetFill(0);
                                                label3.Location = new Point(400 + 100 * g, 100 * h);
                                                label3.Visible = true;
                                                timer1.Start();
                                                flag = 1;
                                                break;
                                            }
                                            if (h < 6 && g > 1 && oldBoard.GetBoard()[h, g].GetFill() == 2 && (oldBoard.GetBoard()[h + 1, g - 1].GetFill() == 1 || oldBoard.GetBoard()[h + 1, g - 1].GetFill() == 3) && oldBoard.GetBoard()[h + 2, g - 2].GetFill() == 0)
                                            {
                                                game.GetBoard()[h, g].SetFill(0);
                                                label3.Location = new Point(400 + 100 * g, 100 * h);
                                                label3.Visible = true;
                                                timer1.Start();
                                                flag = 1;
                                                break;
                                            }
                                        }
                                        if (flag == 1)
                                            break;
                                    }
                            }
                        }
                    }
                }
                if (newI == 0 && game.GetBoard()[newI, newJ].GetFill() == 1)
                    game.GetBoard()[newI, newJ].SetFill(3);
                if (newI == 7 && game.GetBoard()[newI, newJ].GetFill() == 2)
                    game.GetBoard()[newI, newJ].SetFill(4);
            }
            ligal = false;
            game.PaintBoard(g);
            int ezer1 = sumb;
            int ezer2 = sumr;
            CheckBlack();
            countBlackEaten = ezer1 - sumb;
            CheckRed();
            countRedEaten = ezer2 - sumr;
            PaintEatenBlack();
            PaintEatenRed();
            RedWin();
            BlackWin();
            string s = "";
            if (t == 0)
                s = "Black Turn";
            else
                s = "Red Turn";
            label1.Text = s;
        }

        public int Threats()
        {
            //BLACK threat red/red queen right
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (t == 0 && i > 1 && j < 6)
                        if (game.GetBoard()[i, j].GetFill() == 1 && (game.GetBoard()[i - 1, j + 1].GetFill() == 2 || game.GetBoard()[i - 1, j + 1].GetFill() == 4) && game.GetBoard()[i - 2, j + 2].GetFill() == 0)
                            return 1;
                    //BLACK threat red/red queen left
                    if (t == 0 && i > 1 && j > 1)
                        if (game.GetBoard()[i, j].GetFill() == 1 && (game.GetBoard()[i - 1, j - 1].GetFill() == 2 || game.GetBoard()[i - 1, j - 1].GetFill() == 4) && game.GetBoard()[i - 2, j - 2].GetFill() == 0)
                            return 1;
                    //red eat right
                    if (t == 1 && i < 6 && j < 6)
                        if (game.GetBoard()[i, j].GetFill() == 2 && (game.GetBoard()[i + 1, j + 1].GetFill() == 1 || game.GetBoard()[i + 1, j + 1].GetFill() == 3) && game.GetBoard()[i + 2, j + 2].GetFill() == 0)
                            return 1;
                    //red eat left
                    if (t == 1 && i < 6 && j > 1)
                        if (game.GetBoard()[i, j].GetFill() == 2 && (game.GetBoard()[i + 1, j - 1].GetFill() == 1 || game.GetBoard()[i + 1, j - 1].GetFill() == 3) && game.GetBoard()[i + 2, j - 2].GetFill() == 0)
                            return 1;
                }
            return 0;
        }
        public void Eat()
        {
            Eat1();
            Eat2();
            Eat3();
            Eat4();
        }
        public void Eat1()
        {
            if (t == 0 && oldI - newI == 2 && Math.Abs(oldJ - newJ) == 2 && newJ > oldJ)
                if (game.GetBoard()[newI + 1, newJ - 1].GetFill() == 2 || game.GetBoard()[newI + 1, newJ - 1].GetFill() == 4)
                    if (game.GetBoard()[newI, newJ].IsBlack() == true && game.GetBoard()[newI, newJ].GetFill() == 0)
                    {
                        game.GetBoard()[newI, newJ].SetFill(1);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        game.GetBoard()[newI + 1, newJ - 1].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        public void Eat2()
        {
            if (t == 0 && oldI - newI == 2 && Math.Abs(oldJ - newJ) == 2 && newJ < oldJ)
            {
                if (game.GetBoard()[newI + 1, newJ + 1].GetFill() == 2 || game.GetBoard()[newI + 1, newJ + 1].GetFill() == 4)
                {
                    if (game.GetBoard()[newI, newJ].IsBlack() == true && game.GetBoard()[newI, newJ].GetFill() == 0)
                    {
                        game.GetBoard()[newI, newJ].SetFill(1);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        game.GetBoard()[newI + 1, newJ + 1].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
                }
            }
        }
        public void Eat3()
        {
            if (t == 1 && newI - oldI == 2 && Math.Abs(oldJ - newJ) == 2 && newJ > oldJ)
            {
                if (game.GetBoard()[newI - 1, newJ - 1].GetFill() == 1 || game.GetBoard()[newI - 1, newJ - 1].GetFill() == 3)
                {
                    if (game.GetBoard()[newI, newJ].IsBlack() == true && game.GetBoard()[newI, newJ].GetFill() == 0)
                    {
                        game.GetBoard()[newI, newJ].SetFill(2);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        game.GetBoard()[newI - 1, newJ - 1].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
                }
            }
        }
        public void Eat4()
        {
            if (t == 1 && newI - oldI == 2 && Math.Abs(oldJ - newJ) == 2 && newJ < oldJ)
            {
                if (game.GetBoard()[newI - 1, newJ + 1].GetFill() == 1 || game.GetBoard()[newI - 1, newJ + 1].GetFill() == 3)
                {
                    if (game.GetBoard()[newI, newJ].IsBlack() == true && game.GetBoard()[newI, newJ].GetFill() == 0)
                    {
                        game.GetBoard()[newI, newJ].SetFill(2);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        game.GetBoard()[newI - 1, newJ + 1].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
                }
            }
        }

        public void DoubleEat()
        {
            //check if there is a double eat
            if (oldJ != 7 && ((t == 0 && oldI - newI == 4 && oldJ - newJ == 0) || (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4) && oldI - newI == 4 && oldJ - newJ == 0))//מקוביה שחורה לצד ימין
                EatDouble1();
            if ((t == 0 && oldI - newI == 4 && newJ - oldJ == 4) || (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4) && oldI - newI == 4 && newJ - oldJ == 4)
                EatDouble2();
            if ((oldJ != 0) && ((t == 0 && oldI - newI == 4 && oldJ - newJ == 4) || (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4) && oldI - newI == 4 && oldJ - newJ == 4))//מקוביה שחורה לצד שמאל
                EatDouble3();
            if ((t == 0 && oldI - newI == 4 && oldJ - newJ == 0) || (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4) && oldI - newI == 4 && oldJ - newJ == 0)
                EatDouble4();
            if ((t == 1 && newI - oldI == 4 && newJ - oldJ == 4) || (t == 0 && game.GetBoard()[oldI, oldJ].GetFill() == 3) && newI - oldI == 4 && newJ - oldJ == 4)//מקוביה אדומה לצד ימין
                EatDouble5();
            if ((t == 1 && newI - oldI == 4 && newJ - oldJ == 0) || (t == 0 && game.GetBoard()[oldI, oldJ].GetFill() == 3) && newI - oldI == 4 && newJ - oldJ == 0)
                EatDouble6();
            if ((t == 1 && newI - oldI == 4 && oldJ - newJ == 4) || (t == 0 && game.GetBoard()[oldI, oldJ].GetFill() == 3) && newI - oldI == 4 && oldJ - newJ == 4)//קוביה אדומה לצד שמאל
                EatDouble7();
            if ((oldJ != 0) && ((t == 1 && newI - oldI == 4 && oldJ - newJ == 0) || (t == 0 && game.GetBoard()[oldI, oldJ].GetFill() == 3) && newI - oldI == 4 && oldJ - newJ == 0))
                EatDouble8();
        }
        //black eat like 1 second diagonal 1 main diagonal
        public void EatDouble1()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if (((game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 4) && (game.GetBoard()[newI + 1, newJ + 1].GetFill() == 2|| game.GetBoard()[newI + 1, newJ + 1].GetFill() == 4) || ((game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 3) && (game.GetBoard()[newI + 1, newJ + 1].GetFill() == 1|| game.GetBoard()[newI + 1, newJ + 1].GetFill() == 3))))
                    if (game.GetBoard()[oldI - 2, oldJ + 2].IsBlack() && game.GetBoard()[oldI - 2, oldJ + 2].GetFill() == 0)
                    {
                        //אם יש מבצע
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[newI + 1, newJ + 1].SetFill(0);
                        game.GetBoard()[oldI - 1, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //black eat like second diagonal
        public void EatDouble2()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if (((game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 4) && (game.GetBoard()[newI + 1, newJ - 1].GetFill() == 2|| game.GetBoard()[newI + 1, newJ - 1].GetFill() == 4) || (game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 3) && (game.GetBoard()[newI + 1, newJ - 1].GetFill() == 1|| game.GetBoard()[newI + 1, newJ - 1].GetFill() == 3)))
                    if (game.GetBoard()[oldI - 2, oldJ + 2].IsBlack() && game.GetBoard()[oldI - 2, oldJ + 2].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[newI + 1, newJ - 1].SetFill(0);
                        game.GetBoard()[oldI - 1, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //black eat like main diagonal
        public void EatDouble3()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if (((game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 4) && (game.GetBoard()[newI + 1, newJ + 1].GetFill() == 2|| game.GetBoard()[newI + 1, newJ + 1].GetFill() == 4) || (game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 3) && (game.GetBoard()[newI + 1, newJ + 1].GetFill() == 1|| game.GetBoard()[newI + 1, newJ + 1].GetFill() == 3)))
                    if (game.GetBoard()[oldI - 2, oldJ - 2].IsBlack() && game.GetBoard()[oldI - 2, oldJ - 2].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[newI + 1, newJ + 1].SetFill(0);
                        game.GetBoard()[oldI - 1, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //black eat like 1 main diagonal 1 second diagonal
        public void EatDouble4()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() == true && game.GetBoard()[newI, newJ].GetFill() == 0)
                if (((game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 4) && (game.GetBoard()[newI + 1, newJ - 1].GetFill() == 2|| game.GetBoard()[newI + 1, newJ - 1].GetFill() == 4) || (game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 3) && (game.GetBoard()[newI + 1, newJ - 1].GetFill() == 1|| game.GetBoard()[newI + 1, newJ - 1].GetFill() == 1)))
                    if (game.GetBoard()[oldI - 2, oldJ - 2].IsBlack() == true && game.GetBoard()[oldI - 2, oldJ - 2].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[newI + 1, newJ - 1].SetFill(0);
                        game.GetBoard()[oldI - 1, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }

        //red eat like main diagonal
        public void EatDouble5()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if (((game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 3) && (game.GetBoard()[newI - 1, newJ - 1].GetFill() == 1|| game.GetBoard()[newI - 1, newJ - 1].GetFill() == 3) || (game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 4) && (game.GetBoard()[newI - 1, newJ - 1].GetFill() == 2|| game.GetBoard()[newI - 1, newJ - 1].GetFill() == 4)))
                    if (game.GetBoard()[oldI + 2, oldJ + 2].IsBlack() && game.GetBoard()[oldI + 2, oldJ + 2].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[newI - 1, newJ - 1].SetFill(0);
                        game.GetBoard()[oldI + 1, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //red eat like 1 main diagonal 1 second diagonal
        public void EatDouble6()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() == true && game.GetBoard()[newI, newJ].GetFill() == 0)
                if (oldJ != 7)
                    if (((game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 3) && (game.GetBoard()[newI - 1, newJ + 1].GetFill() == 1|| game.GetBoard()[newI - 1, newJ + 1].GetFill() == 3) || (game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 4) && (game.GetBoard()[newI - 1, newJ + 1].GetFill() == 2|| game.GetBoard()[newI - 1, newJ + 1].GetFill() == 4)))
                        if (game.GetBoard()[oldI + 2, oldJ + 2].IsBlack() == true && game.GetBoard()[oldI + 2, oldJ + 2].GetFill() == 0)
                        {
                            if (t == 0)
                            {
                                if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                    game.GetBoard()[newI, newJ].SetFill(1);
                                if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                    game.GetBoard()[newI, newJ].SetFill(3);
                            }
                            if (t == 1)
                            {
                                if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                    game.GetBoard()[newI, newJ].SetFill(2);
                                if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                    game.GetBoard()[newI, newJ].SetFill(4);
                            }
                            game.GetBoard()[newI - 1, newJ + 1].SetFill(0);
                            game.GetBoard()[oldI + 1, oldJ + 1].SetFill(0);
                            game.GetBoard()[oldI, oldJ].SetFill(0);
                            t = (t * -1) + 1;
                            sw = true;
                        }
        }
        //red eat like second diagonal
        public void EatDouble7()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() == true && game.GetBoard()[newI, newJ].GetFill() == 0)
                if (((game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 3) && (game.GetBoard()[newI - 1, newJ + 1].GetFill() == 1|| game.GetBoard()[newI - 1, newJ + 1].GetFill() == 3) || (game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 4) && (game.GetBoard()[newI - 1, newJ + 1].GetFill() == 2|| game.GetBoard()[newI - 1, newJ + 1].GetFill() == 4)))
                    if (game.GetBoard()[oldI + 2, oldJ - 2].IsBlack() == true && game.GetBoard()[oldI + 2, oldJ - 2].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[newI - 1, newJ + 1].SetFill(0);
                        game.GetBoard()[oldI + 1, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //red eat like 1 second diagonal 1 main diagonal
        public void EatDouble8()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() == true && game.GetBoard()[newI, newJ].GetFill() == 0)
                if (((game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 3) && (game.GetBoard()[newI - 1, newJ - 1].GetFill() == 1|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 3) || (game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 4) && (game.GetBoard()[newI - 1, newJ - 1].GetFill() == 2|| game.GetBoard()[newI - 1, newJ - 1].GetFill() == 4)))
                    if (game.GetBoard()[oldI + 2, oldJ - 2].IsBlack() == true && game.GetBoard()[oldI + 2, oldJ - 2].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[newI - 1, newJ - 1].SetFill(0);
                        game.GetBoard()[oldI + 1, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }

        //check if there is a triple eat
        public void TripleEat()
        {
            if (!(oldI > 1 && oldI < 6))//suitable range
            {
                if ((t == 1 && newI - oldI == 6 && newJ - oldJ == 6) || (t == 0 && game.GetBoard()[oldI, oldJ].GetFill() == 3 && newI - oldI == 6 && newJ - oldJ == 6))
                    TripleEatRed1();
                else if ((t == 1 && newI - oldI == 6 && newJ - oldJ == 2) || (t == 0 && game.GetBoard()[oldI, oldJ].GetFill() == 3 && newI - oldI == 6 && newJ - oldJ == 2))
                    TripleEatRed2();
                else if ((oldJ != 7) && ((t == 1 && newI - oldI == 6 && newJ - oldJ == -2) || (t == 0 && game.GetBoard()[oldI, oldJ].GetFill() == 3 && newI - oldI == 6 && newJ - oldJ == -2)))
                    TripleEatRed3();
                else if ((t == 1 && newI - oldI == 6 && newJ - oldJ == -6) || (t == 0 && game.GetBoard()[oldI, oldJ].GetFill() == 3 && newI - oldI == 6 && newJ - oldJ == -6))
                    TripleEatRed4();
                else if ((t == 1 && newI - oldI == 6 && newJ - oldJ == -2) || (t == 0 && game.GetBoard()[oldI, oldJ].GetFill() == 3 && newI - oldI == 6 && newJ - oldJ == -2))
                {
                    TripleEatRed8();
                    TripleEatRed5();
                }
                else if ((oldJ != 0) && ((t == 1 && newI - oldI == 6 && newJ - oldJ == 2) || (t == 0 && game.GetBoard()[oldI, oldJ].GetFill() == 3 && newI - oldI == 6 && newJ - oldJ == 2)))
                    TripleEatRed6();
                else if ((t == 1 && newI - oldI == 6 && newJ - oldJ == 2) || (t == 0 && game.GetBoard()[oldI, oldJ].GetFill() == 3 && newI - oldI == 6 && newJ - oldJ == 2))
                    TripleEatRed7();
                else if ((t == 0 && newI - oldI == -6 && newJ - oldJ == 6) || (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4 && newI - oldI == -6 && newJ - oldJ == 6))
                    TripleEatBlack1();
                else if ((t == 0 && newI - oldI == -6 && newJ - oldJ == 2) || (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4 && newI - oldI == -6 && newJ - oldJ == 2))
                    TripleEatBlack2();
                else if ((oldJ != 7) && ((t == 0 && newI - oldI == -6 && newJ - oldJ == -2) || (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4 && newI - oldI == -6 && newJ - oldJ == -2)))
                    TripleEatBlack3();
                else if ((t == 0 && newI - oldI == -6 && newJ - oldJ == -6) || (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4 && newI - oldI == -6 && newJ - oldJ == -6))
                    TripleEatBlack4();
                else if ((t == 0 && newI - oldI == -6 && newJ - oldJ == -2) || (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4 && newI - oldI == -6 && newJ - oldJ == -2))
                    TripleEatBlack5();
                else if ((oldJ != 0) && ((t == 0 && newI - oldI == -6 && newJ - oldJ == 2) || (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4 && newI - oldI == -6 && newJ - oldJ == 2)))
                    TripleEatBlack6();
                else if ((t == 0 && newI - oldI == -6 && newJ - oldJ == 2) || (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4 && newI - oldI == -6 && newJ - oldJ == 2))
                    TripleEatBlack7();
                else if ((t == 0 && newI - oldI == -6 && newJ - oldJ == -2) || (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4 && newI - oldI == -6 && newJ - oldJ == -2))
                    TripleEatBlack8();
            }
        }
        //red eat like main diagonal 
        public void TripleEatRed1()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 3) && (game.GetBoard()[oldI + 3, oldJ + 3].GetFill() == 1|| game.GetBoard()[oldI + 3, oldJ + 3].GetFill() == 3) && (game.GetBoard()[oldI + 5, oldJ + 5].GetFill() == 1|| game.GetBoard()[oldI + 5, oldJ + 5].GetFill() == 3) || (game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 4)&& (game.GetBoard()[oldI + 3, oldJ + 3].GetFill() == 2||game.GetBoard()[oldI + 3, oldJ + 3].GetFill() == 4) && (game.GetBoard()[oldI + 5, oldJ + 5].GetFill() == 2|| game.GetBoard()[oldI + 5, oldJ + 5].GetFill() == 4))
                    if (game.GetBoard()[oldI + 2, oldJ + 2].GetFill() == 0 && game.GetBoard()[oldI + 4, oldJ + 4].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI + 1, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI + 3, oldJ + 3].SetFill(0);
                        game.GetBoard()[oldI + 5, oldJ + 5].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //red eat like 2 main diagonal 1 second diagonal
        public void TripleEatRed2()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 3) && (game.GetBoard()[oldI + 3, oldJ + 3].GetFill() == 1|| game.GetBoard()[oldI + 3, oldJ + 3].GetFill() == 3) && (game.GetBoard()[oldI + 5, oldJ + 3].GetFill() == 1|| game.GetBoard()[oldI + 5, oldJ + 3].GetFill() == 3) || (game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 4) && (game.GetBoard()[oldI + 3, oldJ + 3].GetFill() == 2|| game.GetBoard()[oldI + 3, oldJ + 3].GetFill() == 4) && (game.GetBoard()[oldI + 5, oldJ + 3].GetFill() == 2|| game.GetBoard()[oldI + 5, oldJ + 3].GetFill() == 4))
                    if (game.GetBoard()[oldI + 2, oldJ + 2].GetFill() == 0 && game.GetBoard()[oldI + 4, oldJ + 4].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI + 1, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI + 3, oldJ + 3].SetFill(0);
                        game.GetBoard()[oldI + 5, oldJ + 3].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //red eat like 1 main diagonal 2 second diagonal
        public void TripleEatRed3()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 1 || game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 3) && (game.GetBoard()[oldI + 3, oldJ + 1].GetFill() == 1 || game.GetBoard()[oldI + 3, oldJ + 1].GetFill() == 3) && (game.GetBoard()[oldI + 5, oldJ - 1].GetFill() == 1 || game.GetBoard()[oldI + 5, oldJ - 1].GetFill() == 3) || (game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 2 || game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 4) && (game.GetBoard()[oldI + 3, oldJ + 1].GetFill() == 2 || game.GetBoard()[oldI + 3, oldJ + 1].GetFill() == 4) && (game.GetBoard()[oldI + 5, oldJ - 1].GetFill() == 2 || game.GetBoard()[oldI + 5, oldJ - 1].GetFill() == 4))
                {
                    if (t == 0)
                    {
                        if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                            game.GetBoard()[newI, newJ].SetFill(1);
                        if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                            game.GetBoard()[newI, newJ].SetFill(3);
                    }
                    if (t == 1)
                    {
                        if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                            game.GetBoard()[newI, newJ].SetFill(2);
                        if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                            game.GetBoard()[newI, newJ].SetFill(4);
                    }
                    game.GetBoard()[oldI + 1, oldJ + 1].SetFill(0);
                    game.GetBoard()[oldI + 3, oldJ + 1].SetFill(0);
                    game.GetBoard()[oldI + 5, oldJ - 1].SetFill(0);
                    game.GetBoard()[oldI, oldJ].SetFill(0);
                    t = (t * -1) + 1;
                    sw = true;
                }
        }
        //red eat like second diagonal
        public void TripleEatRed4()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 3) && (game.GetBoard()[oldI + 3, oldJ - 3].GetFill() == 1|| game.GetBoard()[oldI + 3, oldJ - 3].GetFill() == 3)&& (game.GetBoard()[oldI + 5, oldJ - 5].GetFill() == 1|| game.GetBoard()[oldI + 5, oldJ - 5].GetFill() == 3) || (game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 4) && (game.GetBoard()[oldI + 3, oldJ - 3].GetFill() == 2|| game.GetBoard()[oldI + 3, oldJ - 3].GetFill() == 4) && (game.GetBoard()[oldI + 5, oldJ - 5].GetFill() == 2|| game.GetBoard()[oldI + 5, oldJ - 5].GetFill() == 4))
                    if (game.GetBoard()[oldI + 2, oldJ - 2].GetFill() == 0 && game.GetBoard()[oldI + 4, oldJ - 4].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI + 1, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI + 3, oldJ - 3].SetFill(0);
                        game.GetBoard()[oldI + 5, oldJ - 5].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //red eat like 2 second diagonal 1 main diagonal
        public void TripleEatRed5()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 3) && (game.GetBoard()[oldI + 3, oldJ - 3].GetFill() == 1|| game.GetBoard()[oldI + 3, oldJ - 3].GetFill() == 3) && (game.GetBoard()[oldI + 5, oldJ - 3].GetFill() == 1|| game.GetBoard()[oldI + 5, oldJ - 3].GetFill() == 3) || (game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 4) && (game.GetBoard()[oldI + 3, oldJ - 3].GetFill() == 2|| game.GetBoard()[oldI + 3, oldJ - 3].GetFill() == 4) && (game.GetBoard()[oldI + 5, oldJ - 3].GetFill() == 2|| game.GetBoard()[oldI + 5, oldJ - 3].GetFill() == 4))
                    if (game.GetBoard()[oldI + 2, oldJ - 2].GetFill() == 0 && game.GetBoard()[oldI + 4, oldJ - 4].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI + 1, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI + 3, oldJ - 3].SetFill(0);
                        game.GetBoard()[oldI + 5, oldJ - 3].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //red eat like 1 second diagonal 2 main diagonal
        public void TripleEatRed6()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 3) && (game.GetBoard()[oldI + 3, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI + 3, oldJ - 1].GetFill() == 3) && (game.GetBoard()[oldI + 5, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI + 5, oldJ + 1].GetFill() == 3) || (game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 4) && (game.GetBoard()[oldI + 3, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI + 3, oldJ - 1].GetFill() == 4) && (game.GetBoard()[oldI + 5, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI + 5, oldJ + 1].GetFill() == 4))
                    if (game.GetBoard()[oldI + 2, oldJ - 2].GetFill() == 0 && game.GetBoard()[oldI + 4, oldJ].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI + 1, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI + 3, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI + 5, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //red eat like main zigzag
        public void TripleEatRed7()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 3) && (game.GetBoard()[oldI + 3, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI + 3, oldJ + 1].GetFill() == 3) && (game.GetBoard()[oldI + 5, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI + 5, oldJ + 1].GetFill() == 3) || (game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI + 1, oldJ + 1].GetFill() == 4) && (game.GetBoard()[oldI + 3, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI + 3, oldJ + 1].GetFill() == 4) && (game.GetBoard()[oldI + 5, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI + 5, oldJ + 1].GetFill() == 4))
                    if (game.GetBoard()[oldI + 2, oldJ + 2].GetFill() == 0 && game.GetBoard()[oldI + 4, oldJ].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI + 1, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI + 3, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI + 5, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //red eat like second zigzag
        public void TripleEatRed8()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 3) && (game.GetBoard()[oldI + 3, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI + 3, oldJ - 1].GetFill() == 3) && (game.GetBoard()[oldI + 5, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI + 5, oldJ - 1].GetFill() == 3) || (game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI + 1, oldJ - 1].GetFill() == 4) && (game.GetBoard()[oldI + 3, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI + 3, oldJ - 1].GetFill() == 4) && (game.GetBoard()[oldI + 5, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI + 5, oldJ - 1].GetFill() == 4))
                    if (game.GetBoard()[oldI + 2, oldJ - 2].GetFill() == 0 && game.GetBoard()[oldI + 4, oldJ].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI + 1, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI + 3, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI + 5, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }

        //black eat like second diagonal
        public void TripleEatBlack1()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 4) && (game.GetBoard()[oldI - 3, oldJ + 3].GetFill() == 2|| game.GetBoard()[oldI - 3, oldJ + 3].GetFill() == 4) && (game.GetBoard()[oldI - 5, oldJ + 5].GetFill() == 2|| game.GetBoard()[oldI - 5, oldJ + 5].GetFill() == 4) || (game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 3) && (game.GetBoard()[oldI - 3, oldJ + 3].GetFill() == 1|| game.GetBoard()[oldI - 3, oldJ + 3].GetFill() == 3) && (game.GetBoard()[oldI - 5, oldJ + 5].GetFill() == 1|| game.GetBoard()[oldI - 5, oldJ + 5].GetFill() == 3))
                    if (game.GetBoard()[oldI - 2, oldJ + 2].GetFill() == 0 && game.GetBoard()[oldI - 4, oldJ + 4].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI - 1, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI - 3, oldJ + 3].SetFill(0);
                        game.GetBoard()[oldI - 5, oldJ + 5].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //black eat like 2 second diagonal 1 main diagonal
        public void TripleEatBlack2()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 4) && (game.GetBoard()[oldI - 3, oldJ + 3].GetFill() == 2|| game.GetBoard()[oldI - 3, oldJ + 3].GetFill() == 4) && (game.GetBoard()[oldI - 5, oldJ + 3].GetFill() == 2|| game.GetBoard()[oldI - 5, oldJ + 3].GetFill() == 4) || (game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 1 || game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 3)&& (game.GetBoard()[oldI - 3, oldJ + 3].GetFill() == 1|| game.GetBoard()[oldI - 3, oldJ + 3].GetFill() == 3) && (game.GetBoard()[oldI - 5, oldJ + 3].GetFill() == 1|| game.GetBoard()[oldI - 5, oldJ + 3].GetFill() == 3))
                    if (game.GetBoard()[oldI - 2, oldJ + 2].GetFill() == 0 && game.GetBoard()[oldI - 4, oldJ + 4].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI - 1, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI - 3, oldJ + 3].SetFill(0);
                        game.GetBoard()[oldI - 5, oldJ + 3].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //black eat like 1 second diagonal 2 main diagonal
        public void TripleEatBlack3()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 2 || game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 4)&& (game.GetBoard()[oldI - 3, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI - 3, oldJ + 1].GetFill() == 4) && (game.GetBoard()[oldI - 5, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI - 5, oldJ - 1].GetFill() == 4) || (game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 3) && (game.GetBoard()[oldI - 3, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI - 3, oldJ + 1].GetFill() == 3) && (game.GetBoard()[oldI - 5, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI - 5, oldJ - 1].GetFill() == 3))
                    if (game.GetBoard()[oldI - 2, oldJ + 2].GetFill() == 0 && game.GetBoard()[oldI - 4, oldJ + 4].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI - 1, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI - 3, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI - 5, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //black eat like main diagonal
        public void TripleEatBlack4()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 4) && (game.GetBoard()[oldI - 3, oldJ - 3].GetFill() == 2|| game.GetBoard()[oldI - 3, oldJ - 3].GetFill() == 4) && (game.GetBoard()[oldI - 5, oldJ - 5].GetFill() == 2|| game.GetBoard()[oldI - 5, oldJ - 5].GetFill() == 4) || (game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 3) && (game.GetBoard()[oldI - 3, oldJ - 3].GetFill() == 1|| game.GetBoard()[oldI - 3, oldJ - 3].GetFill() == 3) && (game.GetBoard()[oldI - 5, oldJ - 5].GetFill() == 1|| game.GetBoard()[oldI - 5, oldJ - 5].GetFill() == 3))
                    if (game.GetBoard()[oldI - 2, oldJ - 2].GetFill() == 0 && game.GetBoard()[oldI - 4, oldJ - 4].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI - 1, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI - 3, oldJ - 3].SetFill(0);
                        game.GetBoard()[oldI - 5, oldJ - 5].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //black eat like 2 main diagonal 1 second diagonal
        public void TripleEatBlack5()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 4) && (game.GetBoard()[oldI - 3, oldJ - 3].GetFill() == 2|| game.GetBoard()[oldI - 3, oldJ - 3].GetFill() == 4) && (game.GetBoard()[oldI - 5, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI - 5, oldJ - 1].GetFill() == 4) || (game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 3) && (game.GetBoard()[oldI - 3, oldJ - 3].GetFill() == 1|| game.GetBoard()[oldI - 3, oldJ - 3].GetFill() == 3) && (game.GetBoard()[oldI - 5, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI - 5, oldJ - 1].GetFill() == 3))
                    if (game.GetBoard()[oldI - 2, oldJ - 2].GetFill() == 0 && game.GetBoard()[oldI - 4, oldJ - 2].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI - 1, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI - 3, oldJ - 3].SetFill(0);
                        game.GetBoard()[oldI - 5, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //black eat like 1 main diagonal 2 second diagonal
        public void TripleEatBlack6()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 4) &&(game.GetBoard()[oldI - 3, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI - 3, oldJ - 1].GetFill() == 4) && (game.GetBoard()[oldI - 5, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI - 5, oldJ + 1].GetFill() == 4) || (game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 3) && (game.GetBoard()[oldI - 3, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI - 3, oldJ - 1].GetFill() == 3) && (game.GetBoard()[oldI - 5, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI - 5, oldJ + 1].GetFill() == 3))
                    if (game.GetBoard()[oldI - 2, oldJ - 2].GetFill() == 0 && game.GetBoard()[oldI - 4, oldJ].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI - 1, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI - 3, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI - 5, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //black eat like second zigazg
        public void TripleEatBlack7()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 4) && (game.GetBoard()[oldI - 3, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI - 3, oldJ + 1].GetFill() == 4) && (game.GetBoard()[oldI - 5, oldJ + 1].GetFill() == 2|| game.GetBoard()[oldI - 5, oldJ + 1].GetFill() == 4) || (game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI - 1, oldJ + 1].GetFill() == 3) && (game.GetBoard()[oldI - 3, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI - 3, oldJ + 1].GetFill() == 3) && (game.GetBoard()[oldI - 5, oldJ + 1].GetFill() == 1|| game.GetBoard()[oldI - 5, oldJ + 1].GetFill() == 3))
                    if (game.GetBoard()[oldI - 2, oldJ + 2].GetFill() == 0 && game.GetBoard()[oldI - 4, oldJ].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI - 1, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI - 3, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI - 5, oldJ + 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }
        //black eat like main zigazg
        public void TripleEatBlack8()
        {
            if (game.GetBoard()[newI, newJ].IsBlack() && game.GetBoard()[newI, newJ].GetFill() == 0)
                if ((game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 4) && (game.GetBoard()[oldI - 3, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI - 3, oldJ - 1].GetFill() == 4) &&( game.GetBoard()[oldI - 5, oldJ - 1].GetFill() == 2|| game.GetBoard()[oldI - 5, oldJ - 1].GetFill() == 4) || (game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI - 1, oldJ - 1].GetFill() == 3) && (game.GetBoard()[oldI - 3, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI - 3, oldJ - 1].GetFill() == 3) && (game.GetBoard()[oldI - 5, oldJ - 1].GetFill() == 1|| game.GetBoard()[oldI - 5, oldJ - 1].GetFill() == 3))
                    if (game.GetBoard()[oldI - 2, oldJ - 2].GetFill() == 0 && game.GetBoard()[oldI - 4, oldJ].GetFill() == 0)
                    {
                        if (t == 0)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 1)
                                game.GetBoard()[newI, newJ].SetFill(1);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 3)
                                game.GetBoard()[newI, newJ].SetFill(3);
                        }
                        if (t == 1)
                        {
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 2)
                                game.GetBoard()[newI, newJ].SetFill(2);
                            if (game.GetBoard()[oldI, oldJ].GetFill() == 4)
                                game.GetBoard()[newI, newJ].SetFill(4);
                        }
                        game.GetBoard()[oldI - 1, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI - 3, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI - 5, oldJ - 1].SetFill(0);
                        game.GetBoard()[oldI, oldJ].SetFill(0);
                        t = (t * -1) + 1;
                        sw = true;
                    }
        }

        public void KingsMoves()
        {
            g = CreateGraphics();
            //תזוזה של מלך
            if (KingLigal() == true)
            {
                bool sw1 = true;
                //מבצע תזוזה של מלך שחור
                if (game.GetBoard()[oldI, oldJ].GetFill() == 3 && t == 0)
                {
                    if (newI > oldI && newJ > oldJ)//בדיקה חדשה
                    {
                        for (int i = oldI; i < newI; i++)
                            for (int j = oldJ; j < newJ; j++)
                            {
                                i++;
                                if (game.GetBoard()[i, j].GetFill() == 1)
                                    sw1 = false;
                            }
                        if (sw1 == true)
                        {
                            game.GetBoard()[newI, newJ].SetFill(3);
                            game.GetBoard()[oldI, oldJ].SetFill(0);
                            sw = true;
                            t = (t * -1) + 1;
                        }
                    }
                    if (newI > oldI && newJ < oldJ)//בדיקה נוספת
                    {
                        for (int i = oldI; i < newI; i++)
                            for (int j = oldJ; j > newJ; j--)
                            {
                                i++;
                                if (game.GetBoard()[i, j].GetFill() == 1)
                                    sw1 = false;
                            }
                        if (sw1 == true)
                        {
                            game.GetBoard()[newI, newJ].SetFill(3);
                            game.GetBoard()[oldI, oldJ].SetFill(0);
                            sw = true;
                            t = (t * -1) + 1;
                        }
                    }
                    if (newI < oldI && newJ > oldJ)//בדיקה נוספת
                    {
                        for (int i = oldI; i > newI; i--)
                            for (int j = oldJ; j < newJ; j++)
                            {
                                i--;
                                if (game.GetBoard()[i, j].GetFill() == 1)
                                    sw1 = false;
                            }
                        if (sw1 == true)
                        {
                            game.GetBoard()[newI, newJ].SetFill(3);
                            game.GetBoard()[oldI, oldJ].SetFill(0);
                            sw = true;
                            t = (t * -1) + 1;
                        }
                    }
                    if (newI < oldI && newJ < oldJ)//בדיקה נוספת
                    {
                        for (int i = oldI; i > newI; i--)
                            for (int j = oldJ; j > newJ; j--)
                            {
                                i--;
                                if (game.GetBoard()[i, j].GetFill() == 1)
                                    sw1 = false;
                            }
                        if (sw1 == true)
                        {
                            game.GetBoard()[newI, newJ].SetFill(3);
                            game.GetBoard()[oldI, oldJ].SetFill(0);
                            sw = true;
                            t = (t * -1) + 1;
                        }
                    }
                }
                //מבצע תזוזה של מלך אדום
                if (game.GetBoard()[oldI, oldJ].GetFill() == 4 && t == 1)
                {
                    bool sw2 = true;
                    if (newI > oldI && newJ > oldJ)//בדיקה חדשה
                    {
                        for (int i = oldI; i < newI; i++)
                            for (int j = oldJ; j < newJ; j++)
                            {
                                i++;
                                if (game.GetBoard()[i, j].GetFill() == 2)
                                    sw2 = false;
                            }
                        if (sw2 == true)
                        {
                            game.GetBoard()[newI, newJ].SetFill(4);
                            game.GetBoard()[oldI, oldJ].SetFill(0);
                            sw = true;
                            t = (t * -1) + 1;
                        }
                    }
                    if (newI > oldI && newJ < oldJ)//בדיקה חדשה
                    {
                        for (int i = oldI; i < newI; i++)
                            for (int j = oldJ; j > newJ; j--)
                            {
                                i++;
                                if (game.GetBoard()[i, j].GetFill() == 2)
                                    sw2 = false;
                            }
                        if (sw2 == true)
                        {
                            game.GetBoard()[newI, newJ].SetFill(4);
                            game.GetBoard()[oldI, oldJ].SetFill(0);
                            sw = true;
                            t = (t * -1) + 1;
                        }
                    }
                    if (newI < oldI && newJ > oldJ)//בדיקה חדשה
                    {
                        for (int i = oldI; i > newI; i--)
                            for (int j = oldJ; j < newJ; j++)
                            {
                                i--;
                                if (game.GetBoard()[i, j].GetFill() == 2)
                                    sw2 = false;
                            }
                        if (sw2 == true)
                        {
                            game.GetBoard()[newI, newJ].SetFill(4);
                            game.GetBoard()[oldI, oldJ].SetFill(0);
                            sw = true;
                            t = (t * -1) + 1;
                        }
                    }
                    if (newI < oldI && newJ < oldJ)//בדיקה חדשה
                    {
                        for (int i = oldI; i > newI; i--)
                            for (int j = oldJ; j > newJ; j--)
                            {
                                i--;
                                if (game.GetBoard()[i, j].GetFill() == 2)
                                    sw2 = false;
                            }
                        if (sw2 == true)
                        {
                            game.GetBoard()[newI, newJ].SetFill(4);
                            game.GetBoard()[oldI, oldJ].SetFill(0);
                            sw = true;
                            t = (t * -1) + 1;
                        }
                    }
                }
            }
        }

        public bool KingLigal()
        {
            //פעולה המחזירה אמת אם התזוזה של המלך חוקית
            if (Math.Abs(newI - oldI) == Math.Abs(newJ - oldJ) && game.GetBoard()[newI, newJ].IsBlack())
                if (game.GetBoard()[newI, newJ].GetFill() == 0 && (game.GetBoard()[oldI, oldJ].GetFill() == 3 || game.GetBoard()[oldI, oldJ].GetFill() == 4))
                {
                    if (newI > oldI && newJ > oldJ)
                    {
                        for (int i = 0; i < newI - oldI; i++)
                        {
                            if (game.GetBoard()[oldI + i, oldJ + i].GetFill() == game.GetBoard()[oldI + i + 1, oldJ + i + 1].GetFill())
                                if (!(game.GetBoard()[oldI + i, oldJ + i].GetFill() == 0 && game.GetBoard()[oldI + i + 1, oldJ + i + 1].GetFill() == 0))
                                    return false;
                            if ((t == 0 && (game.GetBoard()[oldI + i + 1, oldJ + i + 1].GetFill() == 1 || game.GetBoard()[oldI + i + 1, oldJ + i + 1].GetFill() == 3))|| (t == 1 && (game.GetBoard()[oldI + i + 1, oldJ + i + 1].GetFill() == 2 || game.GetBoard()[oldI + i + 1, oldJ + i + 1].GetFill() == 4)))
                                return false;
                        }
                        return true;
                    }
                    if(newI>oldI&&newJ<oldJ)
                    {
                        for (int i = 0; i < newI - oldI; i++)
                        {
                            if (game.GetBoard()[oldI + i, oldJ - i].GetFill() == game.GetBoard()[oldI + i + 1, oldJ - i - 1].GetFill())
                                if (!(game.GetBoard()[oldI + i, oldJ - i].GetFill() == 0 && game.GetBoard()[oldI + i + 1, oldJ - i - 1].GetFill() == 0))
                                    return false;
                            if ((t == 0 && (game.GetBoard()[oldI + i + 1, oldJ - i - 1].GetFill() == 1 || game.GetBoard()[oldI + i + 1, oldJ - i - 1].GetFill() == 3)) || (t == 1 && (game.GetBoard()[oldI + i + 1, oldJ - i - 1].GetFill() == 4 || game.GetBoard()[oldI + i + 1, oldJ - i - 1].GetFill() == 2)))
                                return false;
                        }
                        return true;
                    }
                    if (newI < oldI && newJ > oldJ)
                    {
                        for (int i = 0; i < newJ - oldJ; i++)
                        {
                            if (game.GetBoard()[oldI - i, oldJ + i].GetFill() == game.GetBoard()[oldI - i - 1, oldJ + i + 1].GetFill())
                                if (!(game.GetBoard()[oldI - i, oldJ + i].GetFill() == 0 && game.GetBoard()[oldI - i - 1, oldJ + i + 1].GetFill() == 0))
                                    return false;
                            if ((t == 0 && (game.GetBoard()[oldI - i - 1, oldJ + i + 1].GetFill() == 1 || game.GetBoard()[oldI - i - 1, oldJ + i + 1].GetFill() == 3)) || (t == 1 && (game.GetBoard()[oldI - i - 1, oldJ + i + 1].GetFill() == 2 || game.GetBoard()[oldI - i - 1, oldJ + i + 1].GetFill() == 4)))
                                return false;
                        }
                        return true;
                    }
                    if(newI<oldI&&newJ<oldJ)
                    {
                        for (int i = 0; i < oldI - newI; i++)
                        {
                            if (game.GetBoard()[oldI - i, oldJ - i].GetFill() == game.GetBoard()[oldI - i - 1, oldJ - i - 1].GetFill())
                                if (!(game.GetBoard()[oldI - i, oldJ - i].GetFill() == 0 && game.GetBoard()[oldI - i - 1, oldJ - i - 1].GetFill() == 0))
                                    return false;
                            if ((t == 0 && (game.GetBoard()[oldI - i - 1, oldJ - i - 1].GetFill() == 1 || game.GetBoard()[oldI - i - 1, oldJ - i - 1].GetFill() == 3)) || (t == 1 && (game.GetBoard()[oldI - i - 1, oldJ - i - 1].GetFill() == 2 || game.GetBoard()[oldI - i - 1, oldJ - i - 1].GetFill() == 4)))
                                return false;
                        }
                        return true;
                    }
                }
            //אחרת תחזיר שקר
            return false;
        }

        public void KingEats()
        {
            if (KingLigal() == true)
            {
                if (t == 0 && game.GetBoard()[oldI, oldJ].GetFill() == 3)//אכילה של מלך שחור
                {
                    int xezer = 0;
                    int yezer = 0;
                    if (newI > oldI && newJ > oldJ)//בדיקת אכילה חדשה
                    {
                        for (int i = oldI - 1; i < newI; i++)
                            for (int j = oldJ; j < newJ; j++)
                            {
                                i++;
                                if ((game.GetBoard()[i, j].GetFill() == 2 || game.GetBoard()[i, j].GetFill() == 4) && game.GetBoard()[i + 1, j + 1].GetFill() == 0)
                                {
                                    game.GetBoard()[i, j].SetFill(0);
                                    xezer = i;
                                    yezer = j;
                                    game.GetBoard()[oldI, oldJ].SetFill(0);
                                    sw = true;
                                }
                                if ((game.GetBoard()[i, j].GetFill() == 2 || game.GetBoard()[i, j].GetFill() == 4) && (game.GetBoard()[i + 1, j + 1].GetFill() == 2 || game.GetBoard()[i + 1, j + 1].GetFill() == 4))
                                {
                                    i = newI;
                                    j = newJ;
                                }
                            }
                        if (xezer != 0 && yezer != 0)
                        {
                            game.GetBoard()[newI, newJ].SetFill(3);
                            t = (t * -1) + 1;
                        }
                        int ezer = sumr;
                        for (int i = 0; i < 7; i++)
                            for (int j = 0; j < 7; j++)
                                if (game.GetBoard()[i, j].GetFill() == 1)
                                    sumr = sumr++;
                        countRedEaten = ezer - sumr;
                    }
                    if (newI > oldI && newJ < oldJ)//בדיקת אכילה חדשה
                    {
                        int xezer1 = 0;
                        int yezer1 = 0;
                        for (int i = oldI - 1; i < newI; i++)
                            for (int j = oldJ; j > newJ; j--)
                            {
                                i++;
                                if ((game.GetBoard()[i, j].GetFill() == 2 || game.GetBoard()[i, j].GetFill() == 4) && game.GetBoard()[i + 1, j - 1].GetFill() == 0)
                                {
                                    game.GetBoard()[i, j].SetFill(0);
                                    xezer1 = i;
                                    yezer1 = j;
                                    game.GetBoard()[oldI, oldJ].SetFill(0);
                                    sw = true;
                                }
                                if ((game.GetBoard()[i, j].GetFill() == 2 || game.GetBoard()[i, j].GetFill() == 4) && (game.GetBoard()[i + 1, j + 1].GetFill() == 2 || game.GetBoard()[i + 1, j + 1].GetFill() == 4))
                                {
                                    i = newI;
                                    j = newJ;
                                }
                            }
                        if (xezer1 != 0 && yezer1 != 0)
                        {
                            game.GetBoard()[newI, newJ].SetFill(3);
                            t = (t * -1) + 1;
                        }
                        int ezer = sumr;
                        for (int i = 0; i < 7; i++)
                            for (int j = 0; j < 7; j++)
                                if (game.GetBoard()[i, j].GetFill() == 1)
                                    sumr = sumr++;
                        countRedEaten = ezer - sumr;
                    }
                    if (newI < oldI && newJ < oldJ)//בדיקת אכילה חדשה
                    {
                        int xezer2 = 0;
                        int yezer2 = 0;
                        for (int i = oldI + 1; i > newI; i--)
                            for (int j = oldJ; j > newJ; j--)
                            {
                                i--;
                                if ((game.GetBoard()[i, j].GetFill() == 2 || game.GetBoard()[i, j].GetFill() == 4) && game.GetBoard()[i - 1, j - 1].GetFill() == 0)
                                {
                                    game.GetBoard()[i, j].SetFill(0);
                                    xezer2 = i;
                                    yezer2 = j;
                                    game.GetBoard()[oldI, oldJ].SetFill(0);
                                    sw = true;
                                }
                                if ((game.GetBoard()[i, j].GetFill() == 2 || game.GetBoard()[i, j].GetFill() == 4) && (game.GetBoard()[i - 1, j - 1].GetFill() == 2 || game.GetBoard()[i - 1, j - 1].GetFill() == 4))
                                {
                                    i = newI;
                                    j = newJ;
                                }
                            }
                        if (xezer2 != 0 && yezer2 != 0)
                        {
                            game.GetBoard()[newI, newJ].SetFill(3);
                            t = (t * -1) + 1;
                        }
                        int ezer = sumr;
                        for (int i = 0; i < 7; i++)
                            for (int j = 0; j < 7; j++)
                                if (game.GetBoard()[i, j].GetFill() == 1)
                                    sumr = sumr++;
                        countRedEaten = ezer - sumr;
                    }
                    if (newI < oldI && newJ > oldJ)//בדיקת אכילה חדשה
                    {
                        int xezer3 = 0;
                        int yezer3 = 0;
                        for (int i = oldI + 1; i > newI; i--)
                        {
                            for (int j = oldJ; j < newJ; j++)
                            {
                                i--;
                                if ((game.GetBoard()[i, j].GetFill() == 2 || game.GetBoard()[i, j].GetFill() == 4) && game.GetBoard()[i - 1, j + 1].GetFill() == 0)
                                {
                                    game.GetBoard()[i, j].SetFill(0);
                                    xezer3 = i;
                                    yezer3 = j;
                                    game.GetBoard()[oldI, oldJ].SetFill(0);
                                    sw = true;
                                }
                                if ((game.GetBoard()[i, j].GetFill() == 2 || game.GetBoard()[i, j].GetFill() == 4) && (game.GetBoard()[i - 1, j - 1].GetFill() == 2 || game.GetBoard()[i - 1, j - 1].GetFill() == 4))
                                {
                                    i = newI;
                                    j = newJ;
                                }
                            }
                        }
                        if (xezer3 != 0 && yezer3 != 0)
                        {
                            game.GetBoard()[newI, newJ].SetFill(3);
                            t = (t * -1) + 1;
                        }
                        int ezer = sumr;
                        for (int i = 0; i < 7; i++)
                            for (int j = 0; j < 7; j++)
                                if (game.GetBoard()[i, j].GetFill() == 1)
                                    sumr = sumr++;
                        countRedEaten = ezer - sumr;
                    }
                }
                if (t == 1 && game.GetBoard()[oldI, oldJ].GetFill() == 4)//אכילה של מלך אדום
                {
                    if (newI > oldI && newJ > oldJ)//בדיקת אכילה חדשה
                    {
                        int xezer4 = 0;
                        int yezer4 = 0;
                        for (int i = oldI - 1; i < newI; i++)
                        {
                            for (int j = oldJ; j < newJ; j++)
                            {
                                i++;
                                if ((game.GetBoard()[i, j].GetFill() == 1 || game.GetBoard()[i, j].GetFill() == 3) && game.GetBoard()[i + 1, j + 1].GetFill() == 0)
                                {
                                    game.GetBoard()[i, j].SetFill(0);
                                    xezer4 = i;
                                    yezer4 = j;
                                    game.GetBoard()[oldI, oldJ].SetFill(0);
                                    sw = true;
                                }
                                if ((game.GetBoard()[i, j].GetFill() == 1 || game.GetBoard()[i, j].GetFill() == 3) && (game.GetBoard()[i + 1, j + 1].GetFill() == 1 || game.GetBoard()[i + 1, j + 1].GetFill() == 3))
                                {
                                    i = newI;
                                    j = newJ;
                                }
                            }
                        }

                        if (xezer4 != 0 && yezer4 != 0)
                        {
                            game.GetBoard()[newI, newJ].SetFill(4);
                            t = (t * -1) + 1;
                        }
                        int ezer = sumr;
                        for (int i = 0; i < 7; i++)
                            for (int j = 0; j < 7; j++)
                                if (game.GetBoard()[i, j].GetFill() == 1)
                                    sumr = sumr++;
                        countRedEaten = ezer - sumr;
                    }
                    if (newI > oldI && newJ < oldJ)//בדיקת אכילה חדשה
                    {
                        int xezer5 = 0;
                        int yezer5 = 0;
                        for (int i = oldI - 1; i < newI; i++)
                            for (int j = oldJ; j > newJ; j--)
                            {
                                i++;
                                if ((game.GetBoard()[i, j].GetFill() == 1 || game.GetBoard()[i, j].GetFill() == 3) && game.GetBoard()[i + 1, j + 1].GetFill() == 0)
                                {
                                    game.GetBoard()[i, j].SetFill(0);
                                    xezer5 = i;
                                    yezer5 = j;
                                    game.GetBoard()[oldI, oldJ].SetFill(0);
                                    sw = true;
                                }
                                if ((game.GetBoard()[i, j].GetFill() == 1 || game.GetBoard()[i, j].GetFill() == 3) && (game.GetBoard()[i + 1, j + 1].GetFill() == 1 || game.GetBoard()[i + 1, j + 1].GetFill() == 3))
                                {
                                    i = newI;
                                    j = newJ;
                                }
                            }
                        if (xezer5 != 0 && yezer5 != 0)
                        {
                            game.GetBoard()[newI, newJ].SetFill(4);
                            t = (t * -1) + 1;
                        }
                        int ezer = sumr;
                        for (int i = 0; i < 7; i++)
                            for (int j = 0; j < 7; j++)
                                if (game.GetBoard()[i, j].GetFill() == 1)
                                    sumr = sumr++;
                        countRedEaten = ezer - sumr;
                    }
                    if (newI < oldI && newJ < oldJ)//בדיקת אכילה חדשה
                    {
                        int xezer6 = 0;
                        int yezer6 = 0;
                        for (int i = oldI + 1; i > newI; i--)
                            for (int j = oldJ; j > newJ; j--)
                            {
                                i--;
                                if ((game.GetBoard()[i, j].GetFill() == 1 || game.GetBoard()[i, j].GetFill() == 3) && game.GetBoard()[i - 1, j - 1].GetFill() == 0)
                                {
                                    game.GetBoard()[i, j].SetFill(0);
                                    xezer6 = i;
                                    yezer6 = j;
                                    game.GetBoard()[oldI, oldJ].SetFill(0);
                                    sw = true;
                                }
                                if ((game.GetBoard()[i, j].GetFill() == 1 || game.GetBoard()[i, j].GetFill() == 3) && (game.GetBoard()[i - 1, j - 1].GetFill() == 1 || game.GetBoard()[i - 1, j - 1].GetFill() == 3))
                                {
                                    i = newI;
                                    j = newJ;
                                }
                            }
                        if (xezer6 != 0 && yezer6 != 0)
                        {
                            game.GetBoard()[newI, newJ].SetFill(4);
                            t = (t * -1) + 1;
                        }
                        int ezer = sumr;
                        for (int i = 0; i < 7; i++)
                            for (int j = 0; j < 7; j++)
                                if (game.GetBoard()[i, j].GetFill() == 1)
                                    sumr = sumr++;
                        countRedEaten = ezer - sumr;
                    }
                    if (newI < oldI && newJ > oldJ)//בדיקת אכילה חדשה
                    {
                        int xezer7 = 0;
                        int yezer7 = 0;
                        for (int i = oldI + 1; i > newI; i--)
                            for (int j = oldJ; j < newJ; j++)
                            {
                                i--;
                                if ((game.GetBoard()[i, j].GetFill() == 1 || game.GetBoard()[i, j].GetFill() == 3) && game.GetBoard()[i - 1, j - 1].GetFill() == 0)
                                {
                                    game.GetBoard()[i, j].SetFill(0);
                                    xezer7 = i;
                                    yezer7 = j;
                                    game.GetBoard()[oldI, oldJ].SetFill(0);
                                    sw = true;
                                }
                                if ((game.GetBoard()[i, j].GetFill() == 1 || game.GetBoard()[i, j].GetFill() == 3) && (game.GetBoard()[i - 1, j - 1].GetFill() == 1 || game.GetBoard()[i - 1, j - 1].GetFill() == 3))
                                {
                                    i = newI;
                                    j = newJ;
                                }
                            }
                        if (xezer7 != 0 && yezer7 != 0)
                        {
                            game.GetBoard()[newI, newJ].SetFill(4);
                            t = (t * -1) + 1;
                        }
                        int ezer = sumr;
                        for (int i = 0; i < 7; i++)
                            for (int j = 0; j < 7; j++)
                                if (game.GetBoard()[i, j].GetFill() == 1)
                                    sumr = sumr++;
                        countRedEaten = ezer - sumr;
                    }
                }
            }
        }

        public void PaintEatenBlack()
        {
            int x1 = 1300;
            int y1 = 0;
            y1 = y1 + 110 * (12 - sumb);
            if (countBlackEaten > 1)
            {
                y1 = y1 - 110;
            }
            if ((12 - sumb) > 6)
            {
                x1 = x1 + 110;
                y1 = 0;
                y1 = y1 + 110 * (6 - sumb);
                if (countBlackEaten > 1)
                {
                    y1 = y1 - 110;
                }
            }
            Image pic;
            if (12 - sumb == 7 && countBlackEaten > 1)
            {
                pic = Image.FromFile("Simple_Black.png");
                x1 = 1300;
                y1 = 660;
                g.DrawImage(pic, x1, y1);
                x1 = x1 + 110;
                y1 = 110;
                g.DrawImage(pic, x1, y1);
                countBlackEaten = 0;
            }
            for (int i = 0; i < countBlackEaten; i++)
                if (countBlackEaten > 1)
                {
                    pic = Image.FromFile("Simple_Black.png");
                    g.DrawImage(pic, x1, y1);
                    y1 = y1 + 110;
                }
                else
                {
                    pic = Image.FromFile("Simple_Black.png");
                    g.DrawImage(pic, x1, y1);
                }
            countBlackEaten = 0;
        }

        public void PaintEatenRed()
        {
            int x1 = 200;
            int y1 = 0;
            y1 = y1 + 110 * (12 - sumr);
            if (countRedEaten > 1)
                y1 = y1 - 110;
            if ((12 - sumr) > 6)
            {
                x1 = x1 - 110;
                y1 = 0;
                y1 = y1 + 110 * (6 - sumr);
                if (countRedEaten > 1)
                    y1 = y1 - 110;
            }
            Image pic;
            if (12 - sumr == 7 && countRedEaten > 1)
            {
                pic = Image.FromFile("Simple_Red.png");
                x1 = 200;
                y1 = 660;
                g.DrawImage(pic, x1, y1);
                x1 = x1 - 110;
                y1 = 110;
                g.DrawImage(pic, x1, y1);
                countRedEaten = 0;
            }
            for (int i = 0; i < countRedEaten; i++)
                if (countRedEaten > 1)
                {
                    pic = Image.FromFile("Simple_Red.png");
                    g.DrawImage(pic, x1, y1);
                    y1 = y1 + 110;
                }
                else
                {
                    pic = Image.FromFile("Simple_Red.png");
                    g.DrawImage(pic, x1, y1);
                }
            countRedEaten = 0;
        }

        public void RedWin()
        {
            if (sumb == 0)
            {
                win.Play();
                MessageBox.Show("The Red Win");
                this.Close();
                rekasound.Stop();
            }
        }

        public void BlackWin()
        {
            if (sumr == 0)
            {
                win.Play();
                MessageBox.Show("The Black Win");
                this.Close();
                rekasound.Stop();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        public void CheckRed()
        {
            sumr = 0;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (game.GetBoard()[i, j].GetFill() == 2 || game.GetBoard()[i, j].GetFill() == 4)
                        sumr = sumr + 1;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Visible = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label2.Visible = false;
        }

        public void CheckBlack()
        {
            sumb = 0;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (game.GetBoard()[i, j].GetFill() == 1 || game.GetBoard()[i, j].GetFill() == 3)
                        sumb = sumb + 1;
        }

        private void יציאהToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show("האם אתה בטוח שברצונך לצאת", "יציאה", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    rekasound.Stop();
                    this.Close();
                    break;
                case DialogResult.No:
                    MessageBox.Show("תודה שבחרת להישאר");
                    break;
            }
        }

        private void משחקחדשToolStripMenuItem_Click(object sender, EventArgs e)
        {
            g = CreateGraphics();
            countBlackEaten = 0;
            countRedEaten = 0;
            PaintEatenBlack();
            PaintEatenRed();
            game = new Board();
            game.PaintBoard(g);
            t = 0;
            string s = "Black Turn";
            label1.Text = s;
            Refresh();
        }

        private void הוראותToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Horaot a = new Horaot();
            a.Show();
        }

        private void התחלמוזיקהToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rekasound.Play();
        }

        private void עצורמוזיקהToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rekasound.Stop();
        }
        //stop game
        private void button1_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show("האם אתה רוצה לעצור את המשחק?", "בטוח", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    if (rexi < 0)
                    {
                        win.Play();
                        MessageBox.Show("The Black Win");
                    }
                    if (rexi > 0)
                    {
                        win.Play();
                        MessageBox.Show("The Red win");
                    }
                    if (rexi == 0)
                        MessageBox.Show("TIE!!!!!!!!!!");
                    this.Close();
                    break;
            }
        }
    }
}