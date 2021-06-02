using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RatRaceGame.LogicLayer;

namespace RatRaceGame
{
    public partial class MainGame : Form
    {
        Rat[] rats= new Rat[4];
        Punter[] punters = new Punter[3];
        Rat winnerRat;
        int stop;

        Timer[] timers = new Timer[4];
        public MainGame()
        {
            InitializeComponent();
            PrepareInitialData();
            SetBet();
            btnGameOver.Enabled = false;
            btnStartRace.Enabled = false;
        }

        private void PrepareInitialData()
        {
            rats[0] = new Rat() { Name = "Rat 1", TrackLength = 970, PictureBox = pictureCar1 };
            rats[1] = new Rat() { Name = "Rat 2", TrackLength = 970, PictureBox = pictureCar2 };
            rats[2] = new Rat() { Name = "Rat 3", TrackLength = 970, PictureBox = pictureCar3 };
            rats[3] = new Rat() { Name = "Rat 4", TrackLength = 970, PictureBox = pictureCar4 };
            punters[0] = Factory.GetAPunter(1);
            punters[1] = Factory.GetAPunter(2);
            punters[2] = Factory.GetAPunter(3);
            punters[0].Label = lblBets;
            punters[0].RadioButton = radioPunter1;
            punters[0].TextBox = txtPunter1;
            punters[1].Label = lblBets;
            punters[1].RadioButton = radioPunter2;
            punters[1].TextBox = txtPunter2;
            punters[2].Label = lblBets;
            punters[2].RadioButton = radioPunter3;
            punters[2].TextBox = txtPunter3;
            punters[0].RadioButton.Text = punters[0].Name;
            punters[1].RadioButton.Text = punters[1].Name;
            punters[2].RadioButton.Text = punters[2].Name;
            numericRatNo.Minimum = 1;
            numericRatNo.Maximum = 4;
            numericRatNo.Value = 1;
        }
       

        private void SetBet()
        {
            foreach(Punter punter in punters )
            {
                if (punter.Busted)
                {
                    punter.TextBox.Text = "BUSTED!!! LOST ALL AMOUNT IN BET";
                }
                else
                {
                    if (punter.Bet == null)
                    {
                        punter.TextBox.Text = punter.Name + " hasn't placed a bet";
                    }
                    else
                    {
                        punter.TextBox.Text = punter.Name + " bets $" + punter.Bet.Amount + " on " + punter.Bet.Rat.Name;
                    }
                    if (punter.RadioButton.Checked)
                    {
                        lblMaxBet.Text = "Max Bet is $" + punter.Cash.ToString();
                        btnPlaceBet.Text = "Place Bet for " + punter.Name;
                        numericBetAmount.Minimum = 1;
                        numericBetAmount.Maximum = punter.Cash;
                        numericBetAmount.Value = 1;
                    }
                }
            }
        }

      
        private void radioPunter_CheckedChanged(object sender, EventArgs e)
        {
            SetBet();
        }

        private void btnPlaceBet_Click(object sender, EventArgs e)
        {
            int count = 0;
            int total_active = 0;
            foreach(Punter punter in punters)
            {
                if(!punter.Busted)
                {
                    total_active++;
                }
                if(punter.RadioButton.Checked)
                {
                    if( punter.Bet == null )
                    {
                        int number = (int)numericRatNo.Value;
                        int amount = (int)numericBetAmount.Value;
                        bool alreadyPlaced = false;
                        foreach(Punter pun in punters)
                        {
                            if( pun.Bet != null && pun.Bet.Rat == rats[number-1])
                            {
                                alreadyPlaced = true;
                                break;
                            }
                        }
                        if (alreadyPlaced)
                        {
                            ShowMessageBox("This Rat is Already Taken by Punters");
                        }
                        else
                        {
                            punter.Bet = new Bet() { Amount = amount, Rat = rats[number - 1] };
                        }
                        
                    }
                    else
                    {
                        ShowMessageBox("You Already Place Bet for " + punter.Name);
                    }
                }
                if (punter.Bet != null)
                {
                    count++;
                }
            }
            if( count == total_active)
            {
                btnPlaceBet.Enabled = false;
                btnStartRace.Enabled = true;
            }
            SetBet();
        }

        private void btnStartRace_Click(object sender, EventArgs e)
        {
            for(int index = 0; index < timers.Length; index++)
            {
                timers[index] = new Timer();
                timers[index].Interval = 15;
                timers[index].Tick += Timer_Tick;
                timers[index].Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (sender is Timer)
            {
                Timer timer = sender as Timer;
                int index = 0;
                while( index < timers.Length)
                {
                    if(timers[index] == timer)
                    {
                        break;
                    }
                    index++;
                }
                PictureBox picture = rats[index].PictureBox;
                if(picture.Location.X + picture.Width > rats[index].TrackLength)
                {
                    timer.Stop();
                    stop++;
                    if(winnerRat==null)
                    {
                        winnerRat = rats[index];
                    }                    
                }
                else
                {
                    int jump = new Random().Next(1,15);
                    picture.Location = new Point(picture.Location.X + jump, picture.Location.Y);
                }
                
            }
            if (stop == timers.Length)
            {
                MessageBox.Show(winnerRat.Name + " is Won the Rat Race!!!");
                SetBet();
                foreach (Punter punter in punters)
                {
                    if (punter.Bet != null)
                    {
                        if (punter.Bet.Rat == winnerRat)
                        {
                            punter.Cash += punter.Bet.Amount;
                            punter.TextBox.Text = punter.Name + " Won and now has $" + punter.Cash;
                            punter.Winner = true;
                        }
                        else
                        {
                            punter.Cash -= punter.Bet.Amount;
                            if( punter.Cash == 0 )
                            {
                                punter.TextBox.Text = "BUSTED!!! LOST ALL AMOUNT IN BET";
                                punter.Busted = true;
                                punter.RadioButton.Enabled = false;
                                CheckPlayerBustedStatus();
                            }
                            else
                            {
                                punter.TextBox.Text = punter.Name + " Lost and now has $" + punter.Cash;
                            }                            
                        }
                    }
                }
                winnerRat = null;
                stop = 0;
                timers = new Timer[4];
                btnPlaceBet.Enabled = true;
                btnStartRace.Enabled = false;
                int count = 0;
                foreach(Punter punter in punters)
                {
                    if (punter.Busted)
                    {
                        count++;
                    }
                    if (punter.RadioButton.Enabled && punter.RadioButton.Checked)
                    {
                        lblMaxBet.Text = "Max Bet is $" + punter.Cash;
                    }
                    punter.Bet = null;
                    punter.Winner = false;
                }
                if(count==punters.Length)
                {
                    btnPlaceBet.Enabled = false;
                    btnGameOver.Enabled = true;
                }
                foreach(Rat rat in rats)
                {
                    rat.PictureBox.Location = new Point(1,rat.PictureBox.Location.Y);
                }
                CheckPlayerBustedStatus();
            }
        }

        private void btnGameOver_Click(object sender, EventArgs e)
        {
            ShowMessageBox("Bye Bye From Game");
            Application.Exit();
        }

        public void ShowMessageBox(string message)
        {
            MessageBox.Show(message, "Rat Race Game");
        }

        private void MainGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        public void CheckPlayerBustedStatus()
        {
            foreach (Punter punter in punters)
            {
                if (punter.Busted)
                {
                    punter.RadioButton.Checked = false;
                }
                else
                {
                    punter.RadioButton.Checked = true;
                }
            }
        }
    }
}
