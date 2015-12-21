using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SpectacledBear.CodeMash2016.WebApi.Data;
using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.UnitTests.Data
{
    public class HobbitDataManagerTests
    {
        private const string NAME = "Some name";
        private const string FAMILY_NAME = "Some family name";
        private const int BIRTH_YEAR = 1;
        private const int DEATH_YEAR = 2;
        private const long ID = 3;

        private readonly Hobbit _testHobbit = new Hobbit(NAME, FAMILY_NAME, BIRTH_YEAR, DEATH_YEAR, ID);

        [Fact]
        public void Insert_ReturnsNull_WhenZeroDatabaseRowsAffected()
        {
            Mock<IDbDataParameter> mockParameter = new Mock<IDbDataParameter>();
            Mock<IDataParameterCollection> mockParameterCollection = new Mock<IDataParameterCollection>();
            Mock<IDbCommand> mockCommand = new Mock<IDbCommand>();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(0);
            mockCommand.Setup(cmd => cmd.CreateParameter()).Returns(mockParameter.Object);
            mockCommand.Setup(cmd => cmd.Parameters).Returns(mockParameterCollection.Object);
            Mock<IDbConnection> mockConnection = new Mock<IDbConnection>();
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);

            HobbitDataManager dataManager = new HobbitDataManager(mockConnection.Object);

            Hobbit hobbit = dataManager.Insert(_testHobbit);

            Assert.Null(hobbit);
            mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
        }

        [Fact]
        public void Update_ReturnsNull_WhenZeroDatabaseRowsAffected()
        {
            Mock<IDbDataParameter> mockParameter = new Mock<IDbDataParameter>();
            Mock<IDataParameterCollection> mockParameterCollection = new Mock<IDataParameterCollection>();
            Mock<IDbCommand> mockCommand = new Mock<IDbCommand>();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(0);
            mockCommand.Setup(cmd => cmd.CreateParameter()).Returns(mockParameter.Object);
            mockCommand.Setup(cmd => cmd.Parameters).Returns(mockParameterCollection.Object);
            Mock<IDbConnection> mockConnection = new Mock<IDbConnection>();
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);

            HobbitDataManager dataManager = new HobbitDataManager(mockConnection.Object);

            Hobbit hobbit = dataManager.Update(_testHobbit, ID);

            Assert.Null(hobbit);
            mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsFalse_WhenZeroDatabaseRowsAffected()
        {
            Mock<IDbDataParameter> mockParameter = new Mock<IDbDataParameter>();
            Mock<IDataParameterCollection> mockParameterCollection = new Mock<IDataParameterCollection>();
            Mock<IDbCommand> mockCommand = new Mock<IDbCommand>();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(0);
            mockCommand.Setup(cmd => cmd.CreateParameter()).Returns(mockParameter.Object);
            mockCommand.Setup(cmd => cmd.Parameters).Returns(mockParameterCollection.Object);
            Mock<IDbConnection> mockConnection = new Mock<IDbConnection>();
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);

            HobbitDataManager dataManager = new HobbitDataManager(mockConnection.Object);

            bool deleted = dataManager.Delete(ID);

            Assert.False(deleted);
            mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
        }
    }
}
