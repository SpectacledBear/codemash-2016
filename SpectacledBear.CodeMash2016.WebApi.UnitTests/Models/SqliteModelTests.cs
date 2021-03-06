﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.UnitTests.Models
{
    public class SqliteModelTests
    {
        [Theory]
        [MemberData("ResultsData")]
        public void Result_ReturnsSpecifiedValue(string result)
        {
            SqliteModel model = new SqliteModel(null, 0, new string[0], 0, result);

            Assert.Equal(result, model.Result);
        }

        [Theory]
        [MemberData("LongData")]
        public void QueryResponseTime_ReturnsSpecifiedValue(long time)
        {
            SqliteModel model = new SqliteModel(null, time, new string[0], 0, null);

            Assert.Equal(time, model.QueryResponseTime);
        }

        [Theory]
        [MemberData("LongData")]
        public void TotalChanges_ReturnsSpecifiedValue(long changes)
        {
            SqliteModel model = new SqliteModel(null, 0, new string[0], changes, null);

            Assert.Equal(changes, model.TotalChanges);
        }

        [Theory]
        [MemberData("VersionData")]
        public void SqliteVersion_ReturnsSpecifiedValue(string version)
        {
            SqliteModel model = new SqliteModel(version, 0, new string[0], 0, null);

            Assert.Equal(version, model.SqliteVersion);
        }

        [Theory]
        [MemberData("TablesData")]
        public void Tables_WithValues_ReturnsSpecifiedValue(string[] tables, int count)
        {
            SqliteModel model = new SqliteModel(null, 0, tables, 0, null);

            Assert.Equal(count, model.Tables.Count());
            Assert.Equal(tables[0], model.Tables.First());
        }

        [Fact]
        public void Tables_WithoutValues_ReturnsSpeifiedValue()
        {
            string[] tables = new string[0];

            SqliteModel model = new SqliteModel(null, 0, tables, 0, null);

            Assert.Equal(0, model.Tables.Count());
        }

        public static IEnumerable<object[]> VersionData
        {
            get
            {
                return new[]
                {
                    new object[] { "3.8.11.1" },
                    new object[] { null },
                    new object[] { "" },
                    new object[] { "some version" },
                    new object[] { "Bacon ipsum dolor amet chicken ham meatball spare ribs salami, capicola beef ribs." }   // Generated with baconipsum.com
                };
            }
        }

        public static IEnumerable<object[]> LongData
        {
            get
            {
                return new[]
                {
                    new object[] { 0 },
                    new object[] { 1 },
                    new object[] { -1 },
                    new object[] { Int64.MaxValue },
                    new object[] { Int64.MinValue }
                };
            }
        }

        public static IEnumerable<object[]> TablesData
        {
            get
            {
                return new[]
                {
                    new object[] { new string[] { "some table" }, 1 },
                    new object[] { new string[] { " some table", "some other table" }, 2 },
                };
            }
        }

        public static IEnumerable<object[]> ResultsData
        {
            get
            {
                return new[]
                {
                    new object[] { "pass" },
                    new object[] { null },
                    new object[] { "" },
                    new object[] { "some result" },
                    new object[] { "Bacon ipsum dolor amet chicken ham meatball spare ribs salami, capicola beef ribs." }   // Generated with baconipsum.com
                };
            }
        }
    }
}
