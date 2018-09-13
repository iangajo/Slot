using System;
using System.Collections.Generic;
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

            var controller = new Slot(mockIGame.Object, mockITransaction.Object, mockIAccount.Object);

            controller.HttpContext.Request.Headers.Add("Authorization", "mockHeader");

            mockIAccount.Setup(m => m.GetToken(It.IsAny<int>()))
                .Returns("mockHeader");

            mockIGame.Setup(m => m.Spin());
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
    }
}
