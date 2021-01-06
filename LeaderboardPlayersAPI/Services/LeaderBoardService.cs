using LeaderboardPlayersAPI.DTO;
using LeaderboardPlayersAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LeaderboardPlayersAPI.Services
{
    public class LeaderBoardService
    {
        private readonly RedisManager _redisManager;
        private int incrementalIdPlayer { get; set; }

        public LeaderBoardService(RedisManager redisManager)
        {
            _redisManager = redisManager;
            incrementalIdPlayer = _redisManager.getNumberOfElements();
        }

        public ServiceResponse<Player> InsertPlayer(PlayerDTO playerDTO)
        {
            var player = PlayerDTO.DTOtoPlayer(playerDTO);
            player.id = incrementalIdPlayer + 1;

            var deserializedPlayer = JsonConvert.SerializeObject(player);
            var isInserted = _redisManager.Insert(player.id.ToString(), deserializedPlayer).Result;
            if (!isInserted) {
                return new ServiceResponse<Player> // This should be done on the Application layer wich is the controller. 
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Retorno = player,
                    Message = "Não foi possivel inserir o jogador!",
                };
            }

            return new ServiceResponse<Player> // This should be done on the Application layer wich is the controller.
            {
                StatusCode = HttpStatusCode.OK,
                Retorno = player,
                Message = "Jogador cadastrado com sucesso!",
            };
        }

        public ServiceResponse<Player> GetPlayer(int id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                return new ServiceResponse<Player> // This should be done on the Application layer wich is the controller.
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Retorno = null,
                    Message = "Informe um id valido para buscar!",
                };
            }

            var playerFound = _redisManager.Select(id.ToString()).Result;
            if (string.IsNullOrEmpty(playerFound))
            {
                return new ServiceResponse<Player> // This should be done on the Application layer wich is the controller.
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Retorno = null,
                    Message = "Não foi encontrado jogador com o id informado!",
                };
            }

            var playerDeserialized = JsonConvert.DeserializeObject<Player>(playerFound);
            return new ServiceResponse<Player> // This should be done on the Application layer wich is the controller.
            {
                StatusCode = HttpStatusCode.OK,
                Retorno = playerDeserialized,
                Message = "Jogador encontrado com sucesso!",
            };
        }

        public ServiceResponse<List<Player>> SelectAll()
        {
            var playersFound = _redisManager.SelectAll().Result;

            var playersDeserialized = new List<Player>();
            foreach (var player in playersFound)
            {
                Player playerDeserialized = JsonConvert.DeserializeObject<Player>(player);
                playersDeserialized.Add(playerDeserialized);
            }

            var oderedPlayers = playersDeserialized.OrderByDescending(it => it.points).ToList();

            return new ServiceResponse<List<Player>> // This should be done on the Application layer wich is the controller.
            {
                StatusCode = oderedPlayers.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NoContent,
                Retorno = oderedPlayers,
            };
        }

        public ServiceResponse<Player> UpdatePlayer(Player player)
        {
            if (string.IsNullOrWhiteSpace(player.id.ToString())) // This should be done on the Application layer wich is the controller.
            {
                return new ServiceResponse<Player>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Retorno = player,
                    Message = "Informe um id valido para atualizar!",
                };
            }

            var deserializedPlayer = JsonConvert.SerializeObject(player);
            var isUpdated = _redisManager.Update(player.id.ToString(), deserializedPlayer).Result;
            if (!isUpdated)
            {
                return new ServiceResponse<Player> // This should be done on the Application layer wich is the controller.
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Retorno = player,
                    Message = "Não existe jogador com o id informado para ser atualizado!",
                };
            }

            return new ServiceResponse<Player> // This should be done on the Application layer wich is the controller.
            {
                StatusCode = HttpStatusCode.OK,
                Retorno = player,
                Message = "Jogador atualizado com sucesso!",
            };
        }

        public ServiceResponse<bool> Delete(int id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                return new ServiceResponse<bool> // This should be done on the Application layer wich is the controller.
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Retorno = false,
                    Message = "Informe um id valido para remover!",
                };
            }

            var playerDeleted = _redisManager.Delete(id.ToString()).Result;
            if (!playerDeleted)
            {
                return new ServiceResponse<bool> // This should be done on the Application layer wich is the controller.
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Retorno = playerDeleted,
                    Message = "Não existe jogador com o id informado para ser removido!",
                };
            }

            return new ServiceResponse<bool> // This should be done on the Application layer wich is the controller.
            {
                StatusCode = HttpStatusCode.OK,
                Retorno = playerDeleted,
                Message = "Jogador removido com sucesso!",
            };
        }
    }
}
