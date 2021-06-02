using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RatRaceGame.LogicLayer
{
    public abstract class Punter
    {
        public int Cash { set; get; }

        public bool Busted { set; get; }

        public Bet Bet { set; get; }

        public Label Label { set; get; }

        public RadioButton RadioButton { set; get; }

        public string Name { set; get; }

        public bool Winner { set; get; }

        public TextBox TextBox { set; get; }
    }
}
