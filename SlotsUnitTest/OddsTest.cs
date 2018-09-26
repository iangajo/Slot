using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using SlotAPI.DataStores;
using SlotAPI.Domains;
using SlotAPI.Domains.Impl;
using Xunit;

namespace SlotsUnitTest
{
    public class OddsTest
    {
        #region S7_Odds

        [Fact]
        public void S7_Five_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S7", 5, 1);

            Assert.Equal(1000, result);
        }

        [Fact]
        public void S7_Four_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S7", 4, 1);

            Assert.Equal(300, result);
        }

        [Fact]
        public void S7_Three_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S7", 3, 1);

            Assert.Equal(50, result);
        }

        [Fact]
        public void S7_Two_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S7", 2, 1);

            Assert.Equal(0, result);
        }

        #endregion

        #region S6_Odds

        [Fact]
        public void S6_Five_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S6", 5, 1);

            Assert.Equal(200, result);
        }

        [Fact]
        public void S6_Four_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S6", 4, 1);

            Assert.Equal(60, result);
        }

        [Fact]
        public void S6_Three_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S6", 3, 1);

            Assert.Equal(30, result);
        }

        [Fact]
        public void S6_Two_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S6", 2, 1);

            Assert.Equal(0, result);
        }

        #endregion

        #region S5_Odds

        [Fact]
        public void S5_Five_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S5", 5, 1);

            Assert.Equal(150, result);
        }

        [Fact]
        public void S5_Four_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S5", 4, 1);

            Assert.Equal(50, result);
        }

        [Fact]
        public void S5_Three_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S5", 3, 1);

            Assert.Equal(30, result);
        }

        [Fact]
        public void S5_Two_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S5", 2, 1);

            Assert.Equal(0, result);
        }

        #endregion

        #region S4_Odds

        [Fact]
        public void S4_Five_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S4", 5, 1);

            Assert.Equal(130, result);
        }

        [Fact]
        public void S4_Four_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S4", 4, 1);

            Assert.Equal(40, result);
        }

        [Fact]
        public void S4_Three_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S4", 3, 1);

            Assert.Equal(20, result);
        }

        [Fact]
        public void S4_Two_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S4", 2, 1);

            Assert.Equal(0, result);
        }

        #endregion

        #region S3_Odds

        [Fact]
        public void S3_Five_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S3", 5, 1);

            Assert.Equal(100, result);
        }

        [Fact]
        public void S3_Four_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S3", 4, 1);

            Assert.Equal(40, result);
        }

        [Fact]
        public void S3_Three_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S3", 3, 1);

            Assert.Equal(20, result);
        }

        [Fact]
        public void S3_Two_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S3", 2, 1);

            Assert.Equal(0, result);
        }

        #endregion

        #region S2_Odds

        [Fact]
        public void S2_Five_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S2", 5, 1);

            Assert.Equal(80, result);
        }

        [Fact]
        public void S2_Four_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S2", 4, 1);

            Assert.Equal(30, result);
        }

        [Fact]
        public void S2_Three_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S2", 3, 1);

            Assert.Equal(10, result);
        }

        [Fact]
        public void S2_Two_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S2", 2, 1);

            Assert.Equal(0, result);
        }

        #endregion

        #region S1_Odds

        [Fact]
        public void S1_Five_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S1", 5, 1);

            Assert.Equal(80, result);
        }

        [Fact]
        public void S1_Four_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S1", 4, 1);

            Assert.Equal(30, result);
        }

        [Fact]
        public void S1_Three_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S1", 3, 1);

            Assert.Equal(10, result);
        }

        [Fact]
        public void S1_Two_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S1", 2, 1);

            Assert.Equal(0, result);
        }

        #endregion

        #region S0_Odds

        [Fact]
        public void S0_Five_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S0", 5, 1);

            Assert.Equal(60, result);
        }

        [Fact]
        public void S0_Four_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S0", 4, 1);

            Assert.Equal(30, result);
        }

        [Fact]
        public void S0_Three_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S0", 3, 1);

            Assert.Equal(5, result);
        }

        [Fact]
        public void S0_Two_Match_Odds()
        {
            var win = new Win();

            var result = win.GetWin("S0", 2, 1);

            Assert.Equal(0, result);
        }

        #endregion
    }
}
