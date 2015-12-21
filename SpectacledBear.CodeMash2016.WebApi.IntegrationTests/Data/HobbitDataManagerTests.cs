using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SpectacledBear.CodeMash2016.WebApi.Data;
using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.IntegrationTests.Data
{
    public class HobbitDataManagerTests
    {
        private const string NAME = "Test Hobbit name";
        private const string FAMILY_NAME = "Test Hobbit family name";
        private const int BIRTH_YEAR = 1;
        private const int DEATH_YEAR = 2;
        private const long ID = 0;  // default value

        private readonly Hobbit _testHobbit = new Hobbit(NAME, FAMILY_NAME, BIRTH_YEAR, DEATH_YEAR, ID);

        [Fact]
        public void GetAll_ReturnsHobbits()
        {
            HobbitDataManager dataManager = new HobbitDataManager();
            dataManager.Insert(_testHobbit);

            List<Hobbit> hobbits = dataManager.GetAll().ToList();

            Assert.NotNull(hobbits);
            Assert.True(hobbits.Count > 0);
        }

        [Fact]
        public void TryGet_ReturnsTrue_ForKnownHobbit()
        {
            Hobbit testHobbit = new Hobbit("TryGet_ReturnsTrue hobbit", FAMILY_NAME, BIRTH_YEAR, DEATH_YEAR, ID);

            HobbitDataManager dataManager = new HobbitDataManager();

            Hobbit insertedHobbit = dataManager.Insert(testHobbit);

            long hobbitId;
            bool foundHobbit = dataManager.TryGet(insertedHobbit, out hobbitId);

            Assert.True(foundHobbit);
            Assert.True(hobbitId != default(long));  // SQLite ids/rownums start at 1
        }

        [Fact]
        public void TryGet_ReturnsFalse_ForUnknownHobbit()
        {
            Hobbit testHobbit = new Hobbit("TryGet_ReturnsFalse hobbit", FAMILY_NAME, BIRTH_YEAR, DEATH_YEAR, ID);

            HobbitDataManager dataManager = new HobbitDataManager();

            long hobbitId;
            bool foundHobbit = dataManager.TryGet(testHobbit, out hobbitId);

            Assert.False(foundHobbit);
        }

        [Fact]
        public void Insert_ReturnsHobbit()
        {
            Hobbit testHobbit = new Hobbit("Insert hobbit", FAMILY_NAME, BIRTH_YEAR, DEATH_YEAR, ID);

            HobbitDataManager dataManager = new HobbitDataManager();

            Hobbit hobbit = dataManager.Insert(testHobbit);

            Assert.NotNull(hobbit);
            Assert.True(CompareHobbits(testHobbit, hobbit));
        }

        [Theory]
        [MemberData("HobbitsData")]
        public void Insert_ReturnsHobbit_WhenUsingDefaultValues(
            string name,
            string familyName,
            int birthYear,
            int deathYear,
            long id)
        {
            Hobbit testHobbit = new Hobbit("Insert " + name, familyName, birthYear, deathYear, id);

            HobbitDataManager dataManager = new HobbitDataManager();

            Hobbit hobbit = dataManager.Insert(testHobbit);

            Assert.NotNull(hobbit);
            Assert.True(CompareHobbits(testHobbit, hobbit));
        }

        [Fact]
        public void Insert_ReturnsNull_WhenNameIsNull()
        {
            Hobbit testHobbit = new Hobbit(null, FAMILY_NAME, BIRTH_YEAR, DEATH_YEAR, ID);

            HobbitDataManager dataManager = new HobbitDataManager();

            Hobbit hobbit = dataManager.Insert(testHobbit);

            Assert.Null(hobbit);
        }

        [Fact]
        public void Update_ReturnsUpdatedHobbit()
        {
            Hobbit testHobbit = new Hobbit("Insert hobbit", FAMILY_NAME, BIRTH_YEAR, DEATH_YEAR, ID);

            HobbitDataManager dataManager = new HobbitDataManager();
            Hobbit insertedHobbit = dataManager.Insert(testHobbit);

            Hobbit updatedHobbit = new Hobbit(
                "Inserted and updated" + insertedHobbit.Name,
                insertedHobbit.FamilyName,
                insertedHobbit.BirthYear,
                insertedHobbit.DeathYear,
                insertedHobbit.Id);
            Hobbit hobbit = dataManager.Update(updatedHobbit, updatedHobbit.Id);

            Assert.True(CompareHobbits(updatedHobbit, hobbit));
        }

        [Theory]
        [MemberData("HobbitsData")]
        public void Update_ReturnsHobbit_WhenUsingDefaultValues(
            string name,
            string familyName,
            int birthYear,
            int deathYear,
            long id)
        {
            Hobbit testHobbit = new Hobbit("Update " + name, familyName, birthYear, deathYear, id);

            HobbitDataManager dataManager = new HobbitDataManager();
            Hobbit insertedHobbit = dataManager.Insert(testHobbit);

            Hobbit updatedHobbit = new Hobbit(
                           "Updated and updated " + insertedHobbit.Name,
                           insertedHobbit.FamilyName,
                           insertedHobbit.BirthYear,
                           insertedHobbit.DeathYear,
                           insertedHobbit.Id);
            Hobbit hobbit = dataManager.Update(updatedHobbit, updatedHobbit.Id);

            Assert.NotNull(hobbit);
            Assert.True(CompareHobbits(updatedHobbit, hobbit));
        }

        [Fact]
        public void Update_ReturnsNull_WhenNameIsNull()
        {
            Hobbit testHobbit = new Hobbit(null, FAMILY_NAME, BIRTH_YEAR, DEATH_YEAR, ID);

            HobbitDataManager dataManager = new HobbitDataManager();

            Hobbit hobbit = dataManager.Update(testHobbit, ID);

            Assert.Null(hobbit);
        }

        [Fact]
        public void Delete_ReturnsTrue_ForKnownHobbit()
        {
            Hobbit testHobbit = new Hobbit("Deletable Hobbit", FAMILY_NAME, BIRTH_YEAR, DEATH_YEAR, ID);

            HobbitDataManager dataManager = new HobbitDataManager();

            Hobbit insertedHobbit = dataManager.Insert(testHobbit);
            bool deleted = dataManager.Delete(insertedHobbit.Id);
            Hobbit hobbit;
            bool foundHobbit = dataManager.TryGet(insertedHobbit.Id, out hobbit);

            Assert.True(deleted);
            Assert.False(foundHobbit);
        }

        [Fact]
        public void Delete_ReturnsFalse_ForUnknownHobbit()
        {
            HobbitDataManager dataManager = new HobbitDataManager();

            Random prng = new Random();
            bool idExists = true;
            long someId = 0;
            while (idExists)
            {
                someId = Convert.ToInt64(prng.Next());
                Hobbit someHobbit;
                idExists = dataManager.TryGet(someId, out someHobbit);
            }

            bool deleted = dataManager.Delete(someId);

            Assert.False(deleted);
        }

        public static IEnumerable<object[]> HobbitsData
        {
            get
            {
                return new[]
                {
                    new object[] { "some hobbit name", default(string), BIRTH_YEAR, DEATH_YEAR, ID },
                    new object[] { "some other hobbit name", FAMILY_NAME, default(int), DEATH_YEAR, ID },
                    new object[] { "yet another hobbit name", FAMILY_NAME, BIRTH_YEAR, default(int), ID },
                    new object[] { "how many hobbit names are there?", FAMILY_NAME, BIRTH_YEAR, DEATH_YEAR, default(long) }
                };

            }
        }

        private bool CompareHobbits(Hobbit baselineHobbit, Hobbit comparisonHobbit, bool compareId = false)
        {
            if (baselineHobbit.Name != comparisonHobbit.Name)
            {
                return false;
            }

            if (baselineHobbit.FamilyName != comparisonHobbit.FamilyName)
            {
                return false;
            }

            if (baselineHobbit.BirthYear != comparisonHobbit.BirthYear)
            {
                return false;
            }

            if (baselineHobbit.DeathYear != comparisonHobbit.DeathYear)
            {
                return false;
            }

            if (compareId && baselineHobbit.Id != comparisonHobbit.Id)
            {
                return false;
            }

            return true;
        }
    }
}
