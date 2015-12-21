using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Moq;
using Xunit;
using SpectacledBear.CodeMash2016.WebApi.Controllers;
using SpectacledBear.CodeMash2016.WebApi.Data;
using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.UnitTests.Controllers
{
    public class HobitControllerTests
    {
        private const string NAME = "Some name";
        private const string FAMILY_NAME = "Some family name";
        private const int BIRTH_YEAR = 1;
        private const int DEATH_YEAR = 2;
        private const long ID = 3;

        private Hobbit _testHobbit = new Hobbit(NAME, FAMILY_NAME, BIRTH_YEAR, DEATH_YEAR, ID);

        [Fact]
        public void Get_ReturnsAllHobbits()
        {
            Hobbit[] _singleHobbitArray = new Hobbit[] { _testHobbit };

            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.GetAll()).Returns(_singleHobbitArray);

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            IEnumerable<Hobbit> hobbit = controller.Get();

            Assert.Equal(_singleHobbitArray.Count(), hobbit.Count());
            Assert.Equal(NAME, hobbit.First().Name);
        }

        [Fact]
        public void Get_ReturnsSingleHobbit()
        {
            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.TryGet(ID, out _testHobbit)).Returns(true);

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            Hobbit hobbit = controller.Get(ID);

            Assert.Equal(_testHobbit.Name, hobbit.Name);
        }

        [Fact]
        public void Get_ThrowsException_ForUnknownId()
        {
            Hobbit nullHobbit = null;
            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.TryGet(ID, out nullHobbit)).Returns(false);

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            Assert.Throws<HttpResponseException>(() => controller.Get(ID));
        }

        [Fact]
        public void Post_ReturnsHobbit()
        {
            long someId;
            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.TryGet(_testHobbit, out someId)).Returns(false);
            mockHobbitManager.Setup(m => m.Insert(_testHobbit)).Returns(_testHobbit);

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            Hobbit hobbit = controller.Post(_testHobbit);

            Assert.Equal(_testHobbit.Name, hobbit.Name);
        }

        [Fact]
        public void Post_ThrowsException_WhenHobbitAlreadyExists()
        {
            long someId;
            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.TryGet(_testHobbit, out someId)).Returns(true);

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            Assert.Throws<HttpResponseException>(() => controller.Post(_testHobbit));
        }

        [Fact]
        public void Post_ThrowsException_WhenInsertFails()
        {
            long someId;
            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.TryGet(_testHobbit, out someId)).Returns(false);
            mockHobbitManager.Setup(m => m.Insert(_testHobbit));    // returns null

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            Assert.Throws<HttpResponseException>(() => controller.Post(_testHobbit));
        }

        [Fact]
        public void Put_ReturnsHobbit()
        {
            long someId = ID;
            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.TryGet(_testHobbit, out someId)).Returns(true);
            mockHobbitManager.Setup(m => m.Update(_testHobbit, ID)).Returns(_testHobbit);

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            Hobbit hobbit = controller.Put(ID, _testHobbit);

            Assert.Equal(_testHobbit.Name, hobbit.Name);
        }

        [Fact]
        public void Put_ThrowsException_WhenHobbitDoesNotExist()
        {
            long someId;
            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.TryGet(_testHobbit, out someId)).Returns(false);

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            Assert.Throws<HttpResponseException>(() => controller.Put(ID, _testHobbit));
        }

        [Fact]
        public void Put_ThrowsExceptio_WhenExistingHobbitIdMismatchesIdProvided()
        {
            long someId = 1;
            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.TryGet(_testHobbit, out someId)).Returns(false);

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            Assert.Throws<HttpResponseException>(() => controller.Post(_testHobbit));
        }

        [Fact]
        public void Put_ThrowsException_WhenUpdateFails()
        {
            long someId;
            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.TryGet(_testHobbit, out someId)).Returns(false);
            mockHobbitManager.Setup(m => m.Update(_testHobbit, ID));    // returns null

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            Assert.Throws<HttpResponseException>(() => controller.Post(_testHobbit));
        }

        [Fact]
        public void Delete_CompletesSuccessfully()
        {
            Hobbit someHobbit;
            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.TryGet(ID, out someHobbit)).Returns(true);
            mockHobbitManager.Setup(m => m.Delete(ID)).Returns(true);

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            controller.Delete(ID);

            mockHobbitManager.Verify(m => m.Delete(ID), Times.Once());
        }

        [Fact]
        public void Delete_ThrowsException_WhenHobbitDoesNotExist()
        {
            long someId;
            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.TryGet(_testHobbit, out someId)).Returns(false);

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            Assert.Throws<HttpResponseException>(() => controller.Delete(ID));
        }

        [Fact]
        public void Delete_ThrowsException_WhenDeleteFails()
        {
            Hobbit someHobbit;
            Mock<IDataManager<Hobbit>> mockHobbitManager = new Mock<IDataManager<Hobbit>>();
            mockHobbitManager.Setup(m => m.TryGet(ID, out someHobbit)).Returns(true);
            mockHobbitManager.Setup(m => m.Delete(ID)).Returns(false);

            HobbitController controller = new HobbitController(mockHobbitManager.Object);

            Assert.Throws<HttpResponseException>(() => controller.Delete(ID));
        }
    }
}
