using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatRaceGame.LogicLayer
{
    public static class Factory
    {
        public static Punter GetAPunter(int code)
        {
            if (code == 1)
            {
                return new George();
            }
            else if (code == 2)
            {
                return new Lucas();
            }
            else
            {
                return new Jack();
            }
        }
    }
}
