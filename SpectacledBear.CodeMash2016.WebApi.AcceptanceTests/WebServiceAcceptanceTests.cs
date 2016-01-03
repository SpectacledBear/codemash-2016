using System.Web.Http;
using Machine.Specifications;
using Moq;
using SpectacledBear.CodeMash2016.WebApi.Controllers;
using SpectacledBear.CodeMash2016.WebApi.Data;
using SpectacledBear.CodeMash2016.WebApi.Models;
using It = Machine.Specifications.It;

namespace SpectacledBear.CodeMash2016.WebApi.AcceptanceTests
{
    [Tags("WebServiceSpecs")]
    [Subject("WebServiceApi")]
    public class Duplicate_hobbit_names_not_accepted
    {
        const string NAME = "Some hobbit";
        const string FAMILY_NAME = "Some family name";
        const int BIRTH_YEAR = 1;
        const int DEATH_YEAR = 2;
        const long ID = 3;

        // Arrange
        Establish context = () =>
        {
            TestHobbit = new Hobbit(NAME, FAMILY_NAME, BIRTH_YEAR, DEATH_YEAR, ID);

            Mock<IDataManager<Hobbit>> mockDataManager = new Mock<IDataManager<Hobbit>>();
            long someId;
            mockDataManager.Setup(dm => dm.TryGet(TestHobbit, out someId)).Returns(true);

            Controller = new HobbitController(mockDataManager.Object);
        };

        static HobbitController Controller;
        static Hobbit TestHobbit;
        static HttpResponseException ResultException;

        // Act
        Because of = () => ResultException = Catch.Only<HttpResponseException>(() => Controller.Post(TestHobbit));

        // Assert (Specifications)
        It Adding_a_second_hobbit_with_the_same_name_is_a_failure = () => ResultException.ShouldBeOfExactType<HttpResponseException>();
        It Adding_a_second_hobbit_with_the_same_name_is_a_bad_request = () => ResultException.Response.StatusCode.ShouldEqual(System.Net.HttpStatusCode.BadRequest);
    }
}
