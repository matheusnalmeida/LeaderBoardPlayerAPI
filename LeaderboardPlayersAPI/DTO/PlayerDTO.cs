using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaderboardPlayersAPI.DTO
{
    public class PlayerDTO
    {
        public int id { get; set; }
        public string nickname { get; set; }
        public decimal points { get; set; }

        public static Player DTOtoPlayer(PlayerDTO dtoPlayer) {
            return new Player()
            {
                nickname = dtoPlayer.nickname,
                points = dtoPlayer.points
            };
        }
        public static PlayerDTO PlayertoDTO(Player player)
        {
            return new PlayerDTO()
            {
                nickname = player.nickname,
                points = player.points
            };
        }
        
        // Not necessary but an different way of doing the same thing. 
        // Also, you could use implict operators. But be carefull because they hide the parsing. 
        // Only use it when parsing won't affect the data
        
        // public static explict operator Player(PlayerDto dtoPlayer)
        //     => new Player()
        //     {
        //         nickname = dtoPlayer.nickname,
        //         points = dtoPlayer.points
        //     };
           
        //public static explict operator PlayerDTO(Player player)
        //     => new PlayerDTO()
        //     {
        //         nickname = player.nickname,
        //         points = player.points
        //     };
    }
}
