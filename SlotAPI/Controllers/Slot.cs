using System.Net;
using Microsoft.AspNetCore.Mvc;
using SlotAPI.DataStore;
using SlotAPI.Domains;
using SlotAPI.Models;

namespace SlotAPI.Controllers
{

    [ApiController]
    public class Slot : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IReel _reel;
        private readonly IGame _game;
        private readonly ITransaction _transaction;
        private readonly IAccount _account;

        public Slot(ApplicationDbContext applicationDbContext, IReel reel, IGame game, ITransaction transaction, IAccount account)
        {
            _applicationDbContext = applicationDbContext;
            _reel = reel;
            _game = game;
            _transaction = transaction;
            _account = account;
            _applicationDbContext.Database.EnsureCreated();

        }

        [HttpPost]
        [Route("api/register")]
        public ActionResult Registration([FromBody]Registration registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = _account.Register(registration.Username, registration.Password);

            return Ok(response);
        }

        [HttpPost]
        [Route("api/auth/{playerId}")]
        public ActionResult Auth([FromRoute]int playerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = _account.GenerateToken(playerId);

            return Ok(response);
        }

        [HttpPost]
        [Route("api/spin")]
        public ActionResult Spin([FromBody]SpinRequest spinRequest)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var tokenRequest = Request.Headers["Authorization"].ToString();
            var currentToken = _account.GetToken(spinRequest.PlayerId);

            if (tokenRequest != currentToken)
            {
                return Unauthorized();
            }

            var reelResults = _game.Spin();

            var gameId = _game.GenerateGameId();

            var errorMessage = _game.CheckIfPlayerWin(reelResults, spinRequest.Bet, spinRequest.PlayerId);

            var transaction = string.Empty;
            decimal balance = -1;

            if (string.IsNullOrEmpty(errorMessage.ErrorMessage))
            {
                transaction = _transaction.GetLastTransactionHistoryByPlayer(spinRequest.PlayerId).Transaction;
                balance = _account.GetBalance(spinRequest.PlayerId);
            }

            var response = new SpinResponse()
            {
                ErrorMessage = errorMessage.ErrorMessage,
                Balance = balance,
                Transaction = transaction
            };

            return Ok(response);
        }
    }
}
