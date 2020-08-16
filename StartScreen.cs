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
    public partial class StartScreen : Form
    {
        SoundPlayer rekasound;
        public StartScreen()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game a = new Game();
            a.Show();
            rekasound.Play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Horaot a = new Horaot();
            a.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switch(MessageBox.Show("האם אתה בטוח שברצונך לצאת?", "יציאה", MessageBoxButtons.YesNo,MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                this.Close();
                break;

                case DialogResult.No:
                MessageBox.Show("תודה שבחרת להישאר");
                break;
            }
        }

        private void StartScreen_Load(object sender, EventArgs e)
        {
            rekasound = new SoundPlayer("rekasound.wav");
        }


    }
}
