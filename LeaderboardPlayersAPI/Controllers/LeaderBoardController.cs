using LeaderboardPlayersAPI.DTO;
using LeaderboardPlayersAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LeaderboardPlayersAPI.Controllers
{
    [Route("[controller]")]
    public class LeaderBoardController : Controller
    {
        private readonly LeaderBoardService _leaderBoardService;

        public LeaderBoardController(LeaderBoardService leaderBoardService)
        {
            _leaderBoardService = leaderBoardService;
        }

        [HttpGet]
        [Route("ListLeaderBoard")]
        public ActionResult ListLeaderBoard()
        {
            var response = _leaderBoardService.SelectAll();
            if (response.StatusCode == HttpStatusCode.NoContent) {
                return StatusCode((int)response.StatusCode);
                // return NoContent();
            }
            return StatusCode((int)response.StatusCode, response);
            // Another option would be:
            // return Ok(response.data);
            // My question is why you choosed to create different class?
        }

        [HttpGet]
        [Route("GetPlayer")]
        public ActionResult GetPlayer([FromQuery]int id)
        {
            var response = _leaderBoardService.GetPlayer(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        [Route("InsertPlayer")]
        public ActionResult InsertPlayer([FromBody] PlayerDTO newPlayer)
        {
            var response = _leaderBoardService.InsertPlayer(newPlayer);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Route("EditPlayer")]
        public ActionResult EditPlayer([FromQuery]int id,[FromBody] PlayerDTO playerDTO)
        {
            var player = PlayerDTO.DTOtoPlayer(playerDTO);
            player.id = id;
            var response = _leaderBoardService.UpdatePlayer(player);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete]
        [Route("DeletePlayer")]
        public ActionResult DeletePlayer([FromQuery]int id)
        {
            var response = _leaderBoardService.Delete(id);
            return StatusCode((int)response.StatusCode, response);
        }

    }
}
