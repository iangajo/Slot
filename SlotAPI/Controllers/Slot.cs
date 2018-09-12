using Microsoft.AspNetCore.Mvc;
using SlotAPI.DataStore;
using SlotAPI.Domains;
using System.Linq;
using SlotAPI.DataStores;
using SlotAPI.Models;

namespace SlotAPI.Controllers
{

    [ApiController]
    public class Slot : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IReel _reel;
        private readonly IGame _game;
        private readonly IAccountCredits _accountCredits;
        private readonly ITransactionHistory _transactionHistory;

        public Slot(ApplicationDbContext applicationDbContext, IReel reel, IGame game, IAccountCredits accountCredits, ITransactionHistory transactionHistory)
        {
            _applicationDbContext = applicationDbContext;
            _reel = reel;
            _game = game;
            _accountCredits = accountCredits;
            _transactionHistory = transactionHistory;
            _applicationDbContext.Database.EnsureCreated();

        }

        [HttpPost]
        [Route("api/spin")]
        public ActionResult Spin([FromBody]SpinRequest spinRequest)
        {
            var reelResults = _game.Spin();

            var gameId = _game.GenerateGameId();

            var errorMessage = _game.CheckIfPlayerWin(reelResults, spinRequest.Bet, spinRequest.PlayerId);

            var transaction = string.Empty;
            decimal balance = -1;

            if (string.IsNullOrEmpty(errorMessage.ErrorMessage))
            {
                transaction = _transactionHistory.GetLastTransactionHistoryByPlayer(spinRequest.PlayerId).Transaction;
                balance = _accountCredits.GetBalance(spinRequest.PlayerId);
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
