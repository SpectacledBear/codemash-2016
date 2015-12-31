using System.Collections.Generic;
using System.Linq;
using SpectacledBear.CodeMash2016.WebApi.Controllers;
using SpectacledBear.CodeMash2016.WebApi.Data;
using SpectacledBear.CodeMash2016.WebApi.Models;
using Moq;
using Xunit;

namespace SpectacledBear.CodeMash2016.WebApi.UnitTests.Controllers
{
    public class MonitoringControllerTests
    {
        private const string VERSION = "Some version";
        private const long RESPONSE_TIME = 1;
        private const string TABLE = "Some table";
        private const long CHANGES = 2;
        private const string RESULT = "Some result";

        private readonly SqliteModel _testSqliteModel = new SqliteModel(VERSION, RESPONSE_TIME, new string[] { TABLE }, CHANGES, RESULT);

        [Fact]
        public void Get_ReturnsAllMonitoringItems()
        {
            SqliteModel[] monitoringArray = new SqliteModel[] { _testSqliteModel };

            Mock<IDataManager<SqliteModel>> mockDataManager = new Mock<IDataManager<SqliteModel>>();
            mockDataManager.Setup(m => m.GetAll()).Returns(monitoringArray);

            MonitoringController controller = new MonitoringController(mockDataManager.Object);

            IEnumerable<SqliteModel> models = controller.Get();

            Assert.Equal(monitoringArray.Count(), models.Count());
            Assert.Equal(VERSION, models.First().SqliteVersion);
        }
    }
}
