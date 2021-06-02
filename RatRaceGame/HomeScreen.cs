using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RatRaceGame
{
    public partial class HomeScreen : Form
    {
        public HomeScreen()
        {
            InitializeComponent();
        }

        private void HomeScreen_Load(object sender, EventArgs e)
        {
            timerScreen.Start();
        }

        private void timerScreen_Tick(object sender, EventArgs e)
        {
            timerScreen.Stop();
            MainGame game = new MainGame();
            game.Show();
            Hide();        }
    }
}
