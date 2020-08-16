using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Damka
{
    class Cell
    {
        private int x;
        private int y;
        private bool isblack;
        private int fill;
        //0 = משבצת ריקה
        //1 = קוביה שחורה
        //2 = קוביה אדומה
        //3 = מלך שחור
        //4 = מלך אדום
        public Cell()
        {
            this.x = 0;
            this.y = 0;
            this.isblack = false;
            this.fill = 0;
        }
        public Cell(int x,int y,bool isBlack,int fill)
        {
            this.x = x;
            this.y = y;
            this.isblack = isBlack;
            this.fill = fill;
        }
        public int GetX()
        {
            return this.x;
        }
        public void SetX(int x)
        {
            this.x = x;
        }
        public int GetY()
        {
            return this.y;
        }
        public void SetY(int y)
        {
            this.y = y;
        }
        public void SetBlack()
        {
            this.isblack = true;
        }
        public bool IsBlack()
        {
            return this.isblack;
        }
        public int GetFill()
        {
            return this.fill;
        }
        public void SetFill(int fill)
        {
            this.fill = fill; 
        }
        public void PaintCell(Graphics g)
        {
            Pen p = new Pen(Color.Black, 5);
            g.DrawRectangle(p, this.x, this.y, 100, 100);
        }
        public void PaintFillCell(Graphics g, int num)
        {
            SolidBrush brush = new SolidBrush(Color.White);
            SolidBrush brush1 = new SolidBrush(Color.Gray);
            if (num == 0)//רוצים לצבוע בלבן
                g.FillRectangle(brush, this.x + 1, this.y + 1, 99, 99);
            else//רוצים לצבוע באפור
                g.FillRectangle(brush1, this.x + 1, this.y + 1, 99, 99);
        }
        public void PaintCube(Graphics g)
        {
            Image pic;                        
            switch (this.fill)
            {
                case 1:
                    pic = Image.FromFile("Simple_Black.png");
                    g.DrawImage(pic, this.x, this.y);
                    break;
                case 2:
                    pic = Image.FromFile("Simple_Red.png");
                    g.DrawImage(pic, this.x, this.y);
                    break;
                case 3:
                    pic = Image.FromFile("Queen_Black.png");
                    g.DrawImage(pic, this.x, this.y);
                    break;
                case 4:
                    pic = Image.FromFile("Queen_Red.png");
                    g.DrawImage(pic, this.x, this.y);
                    break;
            }
        }
       
    }
}
