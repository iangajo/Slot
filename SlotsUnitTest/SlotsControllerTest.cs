using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SlotAPI.Controllers;
using SlotAPI.Domains;
using SlotAPI.Models;
using Xunit;

namespace SlotsUnitTest
{
    public class SlotsControllerTest
    {
        [Fact]
        public void Registration()
        {
            var mockIGame = new Mock<IGame>();
            var mockITransaction = new Mock<ITransaction>();
            var mockIAccount = new Mock<IAccount>();

            var controller = new Slot(mockIGame.Object, mockITransaction.Object, mockIAccount.Object);

            mockIAccount.Setup(m => m.Register(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => new RegistrationResponse()
                {
                    PlayerId = 1,
                    Success = true,
                    ErrorMessage = string.Empty
                });

            var response = controller.Registration(new Registration()) as OkObjectResult;

            var value = Assert.IsType<RegistrationResponse>(response.Value);

            Assert.Equal(1, value.PlayerId);
            Assert.True(value.Success);
            Assert.Empty(value.ErrorMessage);
        }

        [Fact]
        public void Auth()
        {
            var mockIGame = new Mock<IGame>();
            var mockITransaction = new Mock<ITransaction>();
            var mockIAccount = new Mock<IAccount>();

            var controller = new Slot(mockIGame.Object, mockITransaction.Object, mockIAccount.Object);

            mockIAccount.Setup(m => m.GenerateToken(It.IsAny<int>()))
                .Returns(() =>new AuthResponse()
            {
                Token  = "randomGUID"
            });

            var response = controller.Auth(It.IsAny<int>()) as OkObjectResult;

            var value = Assert.IsType<AuthResponse>(response.Value);

            Assert.Equal("Bearer", value.Type);
            Assert.Equal("randomGUID", value.Token);
            Assert.Equal(1800, value.Expires);
        }

        [Fact]
        public void Spin()
        {
            var mockIGame = new Mock<IGame>();
            var mockITransaction = new Mock<ITransaction>();
            var mockIAccount = new Mock<IAccount>();

            

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Authorization", "BearerMockHeader");

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new Slot(mockIGame.Object, mockITransaction.Object, mockIAccount.Object)
            {
                ControllerContext = controllerContext
            };

            mockIAccount.Setup(m => m.GetToken(It.IsAny<int>()))
                .Returns("MockHeader");

            mockIGame.Setup(m => m.Spin()).Returns(() => new List<ReelResult>());
            mockIGame.Setup(m => m.CheckIfPlayerWin(It.IsAny<List<ReelResult>>(), It.IsAny<decimal>(), It.IsAny<int>())).Returns(new BaseResponse()
            {
                ErrorMessage = string.Empty,
                Success = true
            });

            mockITransaction.Setup(m => m.GetLastTransactionHistoryByPlayer(It.IsAny<int>())).Returns(() =>
                new TransactionHistory()
                {
                    Transaction = "Win",
                });

            mockIAccount.Setup(m => m.GetBalance(It.IsAny<int>())).Returns(100);

            var response = controller.Spin(new SpinRequest()) as OkObjectResult;

            var value = Assert.IsType<SpinResponse>(response.Value);

            Assert.Equal("Win", value.Transaction);
            Assert.Equal(100, value.Balance);
        }

        [Fact]
        public void GetPlayerState()
        {
            var mockIGame = new Mock<IGame>();
            var mockITransaction = new Mock<ITransaction>();
            var mockIAccount = new Mock<IAccount>();


            var controller = new Slot(mockIGame.Object, mockITransaction.Object, mockIAccount.Object);

            mockITransaction.Setup(m => m.GetLastTransactionHistoryByPlayer(It.IsAny<int>()))
                .Returns(() => new TransactionHistory()
                {
                    Transaction = "Win"
                });

            mockIAccount.Setup(m => m.GetBalance(It.IsAny<int>())).Returns(100);

            var response = controller.GetPlayerState(It.IsAny<int>()) as OkObjectResult;

            var value = Assert.IsType<SpinResponse>(response.Value);

            Assert.Equal("Win", value.Transaction);
            Assert.Equal(100, value.Balance);
        }


        [Fact]
        public void GetPayLineWinStats()
        {
            var mockIGame = new Mock<IGame>();
            var mockITransaction = new Mock<ITransaction>();
            var mockIAccount = new Mock<IAccount>();


            var controller = new Slot(mockIGame.Object, mockITransaction.Object, mockIAccount.Object);

            mockITransaction.Setup(m => m.GetPayLineStats())
                .Returns(() => new List<PayLineStat>()
                {
                    new PayLineStat(),
                    new PayLineStat(),
                    new PayLineStat(),
                    new PayLineStat(),
                    new PayLineStat()
                });

            var response = controller.GetPayLineWinStats() as OkObjectResult;

            var value = Assert.IsType<List<PayLineStat>>(response.Value);

            Assert.Equal(5, value.Count);

        }

        [Fact]
        public void GetSymbolWinStats()
        {
            var mockIGame = new Mock<IGame>();
            var mockITransaction = new Mock<ITransaction>();
            var mockIAccount = new Mock<IAccount>();


            var controller = new Slot(mockIGame.Object, mockITransaction.Object, mockIAccount.Object);

            mockITransaction.Setup(m => m.GetSymbolStats())
                .Returns(() => new List<SymbolStat>()
                {
                    new SymbolStat(),
                    new SymbolStat(),
                    new SymbolStat(),
                    new SymbolStat(),
                    new SymbolStat()
                });

            var response = controller.GetSymbolWinStats() as OkObjectResult;

            var value = Assert.IsType<List<SymbolStat>>(response.Value);

            Assert.Equal(5, value.Count);

        }

        [Fact]
        public void GetPlayerTotalWin()
        {
            var mockIGame = new Mock<IGame>();
            var mockITransaction = new Mock<ITransaction>();
            var mockIAccount = new Mock<IAccount>();


            var controller = new Slot(mockIGame.Object, mockITransaction.Object, mockIAccount.Object);

            mockITransaction.Setup(m => m.GetPlayerTotalWinAmount(It.IsAny<int>()))
                .Returns(new WinAmount()
                {
                    Amount = 1000.00m
                });

            var response = controller.GetPlayerTotalWin(It.IsAny<int>()) as OkObjectResult;

            var value = Assert.IsType<WinAmount>(response.Value);

            Assert.Equal(1000.00m, value.Amount);

        }
    }
}
