using Moq;
using SlotAPI.DataStores;
using System;
using System.Collections.Generic;
using System.Text;
using SlotAPI.Domains;
using SlotAPI.Domains.Impl;
using Xunit;

namespace SlotsUnitTest
{
    public class PayLineTest
    {
        [Fact]
        public void PayLine1_Win()
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

            var slots = new string[3,5]
            {
                { "S0", "S1", "S2", "S3", "S4" }, 
                { "S7", "S7", "S7", "S7", "S7" }, 
                { "S1", "S2", "S3", "S4", "S5" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine2_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "0,0", "0,1", "0,2", "0,3", "0,4" });

            var slots = new string[3, 5]
            {
                { "S7", "S7", "S7", "S7", "S7" },
                { "S3", "S4", "S5", "S6", "S7" },
                { "S1", "S2", "S3", "S4", "S5" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine3_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "2,0", "2,1", "2,2", "2,3", "2,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S1", "S2", "S3", "S4" },
                { "S3", "S4", "S5", "S6", "S7" },
                { "S7", "S7", "S7", "S7", "S7" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine4_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "0,0", "0,1", "1,2", "1,3", "1,4" });

            var slots = new string[3, 5]
            {
                { "S7", "S7", "S0", "S3", "S4" },
                { "S1", "S2", "S7", "S7", "S7" },
                { "S3", "S5", "S6", "S0", "S2" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine5_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "2,0", "2,1", "1,2", "1,3", "1,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S1", "S2", "S3", "S4" },
                { "S1", "S2", "S7", "S7", "S7" },
                { "S7", "S7", "S0", "S1", "S2" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine6_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "0,0", "0,1", "2,2", "2,3", "2,4" });

            var slots = new string[3, 5]
            {
                { "S7", "S7", "S2", "S3", "S4" },
                { "S3", "S4", "S5", "S6", "S7" },
                { "S0", "S1", "S7", "S7", "S7" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine7_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "2,0", "2,1", "0,2", "0,3", "0,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S1", "S7", "S7", "S7" },
                { "S3", "S4", "S5", "S6", "S7" },
                { "S7", "S7", "S1", "S2", "S3" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine8_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "1,1", "0,2", "1,3", "2,4" });

            var slots = new string[3, 5]
            {
                { "S6", "S5", "S7", "S3", "S4" },
                { "S7", "S7", "S5", "S7", "S4" },
                { "S1", "S2", "S3", "S4", "S7" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine9_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "1,1", "2,2", "1,3", "0,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S1", "S2", "S3", "S7" },
                { "S7", "S7", "S5", "S7", "S7" },
                { "S1", "S2", "S7", "S4", "5" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine10_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "0,0", "1,1", "0,2", "0,3", "0,4" });

            var slots = new string[3, 5]
            {
                { "S7", "S1", "S7", "S7", "S7" },
                { "S0", "S7", "S1", "S2", "S3" },
                { "S2", "S3", "S4", "S5", "S6" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine11_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "2,0", "1,1", "2,2", "2,3", "2,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S1", "S2", "S3", "S4" },
                { "S3", "S7", "S5", "S6", "S7" },
                { "S7", "s4", "S7", "S7", "S7" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine12_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "0,1", "0,2", "0,3", "1,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S7", "S7", "S7", "S4" },
                { "S7", "S3", "S4", "S5", "S7" },
                { "S5", "S4", "S3", "S2", "S1" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine13_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "2,1", "2,2", "2,3", "1,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S1", "S2", "S3", "S4" },
                { "S7", "S3", "S4", "S5", "S7" },
                { "S5", "S7", "S7", "S7", "S1" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine14_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "0,1", "1,2", "0,3", "0,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S7", "S2", "S7", "S7" },
                { "S7", "S3", "S7", "S5", "S6" },
                { "S5", "S4", "S3", "S2", "S1" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine15_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "2,1", "1,2", "2,3", "2,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S1", "S2", "S3", "S4" },
                { "S7", "S3", "S7", "S5", "S6" },
                { "S5", "S7", "S3", "S7", "S7" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine16_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "0,0", "1,1", "1,2", "2,3", "2,4" });

            var slots = new string[3, 5]
            {
                { "S7", "S1", "S2", "S3", "S4" },
                { "S2", "S7", "S7", "S5", "S6" },
                { "S5", "S4", "S3", "S7", "S7" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine17_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "2,0", "1,1", "1,2", "0,3", "0,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S1", "S2", "S7", "S7" },
                { "S2", "S7", "S7", "S5", "S6" },
                { "S7", "S4", "S3", "S2", "S1" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine18_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "0,1", "2,2", "1,3", "1,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S7", "S2", "S3", "S4" },
                { "S7", "S3", "S4", "S7", "S7" },
                { "S5", "S4", "S7", "S2", "S1" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine19_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "2,1", "0,2", "1,3", "1,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S1", "S7", "S3", "S4" },
                { "S7", "S3", "S4", "S7", "S7" },
                { "S5", "S7", "S3", "S2", "S1" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine20_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "0,0", "1,1", "2,2", "2,3", "2,4" });

            var slots = new string[3, 5]
            {
                { "S7", "S1", "S2", "S3", "S4" },
                { "S2", "S7", "S4", "S5", "S6" },
                { "S5", "S4", "S7", "S7", "S7" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine21_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "2,0", "1,1", "0,2", "0,3", "0,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S1", "S7", "S7", "S7" },
                { "S2", "S7", "S4", "S5", "S6" },
                { "S7", "S4", "S3", "S2", "S1" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine22_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "0,0", "2,1", "0,2", "1,3", "2,4" });

            var slots = new string[3, 5]
            {
                { "S7", "S1", "S7", "S3", "S4" },
                { "S2", "S3", "S4", "S7", "S6" },
                { "S5", "S7", "S3", "S2", "S7" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine23_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "2,0", "0,1", "2,2", "1,3", "0,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S7", "S2", "S3", "S7" },
                { "S2", "S3", "S4", "S7", "S6" },
                { "S7", "S4", "S7", "S2", "S1" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine24_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "0,0", "2,1", "1,2", "0,3", "1,4" });

            var slots = new string[3, 5]
            {
                { "S7", "S1", "S2", "S7", "S4" },
                { "S2", "S3", "S7", "S5", "S7" },
                { "S5", "S7", "S3", "S2", "S1" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine25_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "2,0", "0,1", "1,2", "2,3", "1,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S7", "S2", "S3", "S4" },
                { "S2", "S3", "S7", "S5", "S7" },
                { "S7", "S4", "S3", "S7", "S1" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine26_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "0,0", "2,1", "2,2", "0,3", "0,4" });

            var slots = new string[3, 5]
            {
                { "S7", "S1", "S2", "S7", "S7" },
                { "S2", "S3", "S4", "S5", "S6" },
                { "S5", "S7", "S7", "S2", "S1" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine27_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "2,0", "0,1", "0,2", "2,3", "2,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S7", "S7", "S3", "S4" },
                { "S2", "S3", "S4", "S5", "S6" },
                { "S7", "S4", "S3", "S7", "S7" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine28_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "1,1", "1,2", "0,3", "0,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S1", "S2", "S7", "S7" },
                { "S7", "S7", "S7", "S5", "S6" },
                { "S5", "S4", "S3", "S2", "S1" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine29_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "1,0", "1,1", "1,2", "2,3", "2,4" });

            var slots = new string[3, 5]
            {
                { "S0", "S1", "S2", "S3", "S4" },
                { "S7", "S7", "S7", "S5", "S6" },
                { "S5", "S4", "S3", "S7", "S7" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

        [Fact]
        public void PayLine30_Win()
        {
            var mockIReel = new Mock<IReel>();
            var mockITransactionHistoryDataStore = new Mock<ITransactionHistoryDataStore>();
            var mockIAccountCreditsDataStore = new Mock<IAccountCreditsDataStore>();
            var mockIStatisticDataStore = new Mock<IStatisticsDataStore>();
            var mockIWin = new Mock<IWin>();

            var game = new Game(mockIReel.Object, mockITransactionHistoryDataStore.Object,
                mockIAccountCreditsDataStore.Object, mockIStatisticDataStore.Object, mockIWin.Object);

            mockIWin.Setup(m => m.GetWinningPayLine(It.IsAny<int>()))
                .Returns(() => new string[] { "0,0", "2,1", "2,2", "2,3", "2,4" });

            var slots = new string[3, 5]
            {
                { "S7", "S1", "S2", "S3", "S4" },
                { "S2", "S3", "S4", "S5", "S6" },
                { "S5", "S7", "S7", "S7", "S7" }
            };

            var result = game.CheckIfPlayerHasWinningCombinations(slots, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<decimal>(), It.IsAny<string>());

            Assert.True(result);
        }

    }
}
