using System;
using System.Collections.Generic;
using Moq;
using SlotAPI.DataStores;
using SlotAPI.Domains;
using SlotAPI.Domains.Impl;
using SlotAPI.Models;
using Xunit;

namespace SlotsUnitTest
{
    public class GameTest
    {
        [Fact]
        public void CheckWin_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "1,1", "1,2", "1,3", "1,4" });

            var slots = new string[3, 5];

            slots[1, 0] = "S7";
            slots[1, 1] = "S7";
            slots[1, 2] = "S7";
            slots[1, 3] = "S7";
            slots[1, 4] = "S7";

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void CheckWin_Lose()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "1,1", "1,2", "1,3", "1,4" });

            var slots = new string[3, 5];

            slots[1, 0] = "S0";
            slots[1, 1] = "S1";
            slots[1, 2] = "S2";
            slots[1, 3] = "S3";
            slots[1, 4] = "S4";

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(!result);
        }

        [Fact]
        public void CheckWin_Win_Cascaded()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "1,1", "1,2", "1,3", "1,4" });

            var slots = new string[3, 5];

            slots[1, 0] = "S7";
            slots[1, 1] = "S7";
            slots[1, 2] = "S7";
            slots[1, 3] = "S7";
            slots[1, 4] = "S7";

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), true, It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void CheckBonus_HasBonus()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIAccountCreditsDataStore.Setup(m => m.CreditBonusSpin(It.IsAny<int>())).Returns(() => new BaseResponse()
            {
                Success = true
            }).Verifiable();

            var slots = new string[3, 5];

            slots[1, 0] = "Bonus";
            slots[1, 1] = "Bonus";
            slots[1, 2] = "Bonus";
            slots[1, 3] = "S3";
            slots[1, 4] = "S4";

            game.CheckIfPlayerSpinHasFreeBonusSpin(slots, It.IsAny<int>());

            mockIAccountCreditsDataStore.VerifyAll();
        }

        [Fact]
        public void CheckBonus_NoBonus()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            var slots = new string[3, 5];

            slots[1, 0] = "S0";
            slots[1, 1] = "S1";
            slots[1, 2] = "S2";
            slots[1, 3] = "S3";
            slots[1, 4] = "S4";

            game.CheckIfPlayerSpinHasFreeBonusSpin(slots, It.IsAny<int>());

            mockIAccountCreditsDataStore.VerifyAll();
        }

        [Fact]
        public void CheckIfPlayerHasBonusSpin_HasBonusSpin()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIAccountCreditsDataStore.Setup(m => m.GetPlayerSpinBonus(It.IsAny<int>())).Returns(6);
            mockIAccountCreditsDataStore.Setup(m => m.DebitBonusSpin(It.IsAny<int>()))
                .Returns(() => new BaseResponse()
                {
                    Success = true
                });

            var betAmount = It.IsAny<decimal>();
            game.CheckIfPlayerHasBonusSpin(ref betAmount, It.IsAny<int>());

            mockIAccountCreditsDataStore.VerifyAll();
        }

        [Fact]
        public void CheckIfPlayerHasBonusSpin_HasNoBonusSpin()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIAccountCreditsDataStore.Setup(m => m.GetPlayerSpinBonus(It.IsAny<int>())).Returns(0);
            mockIAccountCreditsDataStore.Setup(m => m.Debit(It.IsAny<int>(), It.IsAny<decimal>()))
                .Returns(() => new BaseResponse()
                {
                    Success = true
                });

            var betAmount = It.IsAny<decimal>();
            game.CheckIfPlayerHasBonusSpin(ref betAmount, It.IsAny<int>());

            mockIAccountCreditsDataStore.VerifyAll();
        }

        [Fact]
        public void RemoveSymbolsInTheWheelArray()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            var listOfWinArray = new List<string>();
            listOfWinArray.Add("1,0");

            game.RemoveSymbolsInTheWheelArray(listOfWinArray);

            mockIAccountCreditsDataStore.VerifyAll();
        }

        [Fact]
        public void WinLineMatch()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWin(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>())).Returns(100);
            mockIAccountCreditsDataStore.Setup(m => m.Credit(It.IsAny<int>(), It.IsAny<decimal>()))
                .Returns(() => new BaseResponse()
                {
                    Success = true
                });

            mockITransactionHistoryDataStore.Setup(m => m.AddTransactionHistory(It.IsAny<decimal>(), It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

            mockIStatisticDataStore.Setup(m => m.PayLineStat(It.IsAny<int>()));
            mockIStatisticDataStore.Setup(m => m.SymbolStat(It.IsAny<string>(), It.IsAny<int>()));

            game.CreditAndRecordWinningCombinations(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>());

            mockIAccountCreditsDataStore.VerifyAll();
        }

        [Fact]
        public void Spin()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(new Reel(), mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWin(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>())).Returns(100);
            mockIAccountCreditsDataStore.Setup(m => m.Credit(It.IsAny<int>(), It.IsAny<decimal>()))
                .Returns(() => new BaseResponse()
                {
                    Success = true
                });

            mockITransactionHistoryDataStore.Setup(m => m.AddTransactionHistory(It.IsAny<decimal>(), It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

            mockIStatisticDataStore.Setup(m => m.PayLineStat(It.IsAny<int>()));
            mockIStatisticDataStore.Setup(m => m.SymbolStat(It.IsAny<string>(), It.IsAny<int>()));

            var results = game.Spin(It.IsAny<int>(), It.IsAny<decimal>());

            Assert.True(results != null);

        }
    }
}
