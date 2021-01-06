using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaderboardPlayersAPI
{
    public class Player
    {
        public int id { get; set; }
        public string nickname { get; set; }
        public decimal points { get; set; }
    }
}
