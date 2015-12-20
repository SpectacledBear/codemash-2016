using System;
using System.Collections.Generic;
using Xunit;
using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.UnitTests.Models
{
    public class HobbitTests
    {
        [Theory]
        [MemberData("NamesData")]
        public void Name_StoresProvidedValue(string name)
        {
            Hobbit hobbit = new Hobbit(name, null, 0, 0, 0);

            Assert.Equal(name, hobbit.Name);
        }

        [Theory]
        [MemberData("NamesData")]
        public void FamilyName_StoresProvidedValue(string name)
        {
            Hobbit hobbit = new Hobbit(name, null, 0, 0, 0);

            Assert.Equal(name, hobbit.Name);
        }

        [Theory]
        [MemberData("YearsData")]
        public void BirthYear_StoresProvidedValue(int year)
        {
            Hobbit hobbit = new Hobbit(null, null, year, 0, 0);

            Assert.Equal(year, hobbit.BirthYear);
        }

        [Theory]
        [MemberData("YearsData")]
        public void DeathYear_StoresProvidedValue(int year)
        {
            Hobbit hobbit = new Hobbit(null, null, 0, year, 0);

            Assert.Equal(year, hobbit.DeathYear);
        }

        public static IEnumerable<object[]> NamesData
        {
            get
            {
                return new[]
                {
                    new object[] { "Some name" },
                    new object[] { "孙子" },   // Sun Tzu
                    new object[] { null },
                    new object[] { "" }
                };
            }
        }

        public static IEnumerable<object[]> YearsData
        {
            get
            {
                return new[]
                {
                    new object[] { 0 },
                    new object[] { 1 },
                    new object[] { -1 },
                    new object[] { Int32.MaxValue },
                    new object[] { Int32.MinValue }
                };
            }
        }
    }
}
