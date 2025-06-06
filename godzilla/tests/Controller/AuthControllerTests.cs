using System.Net.Http.Json;
using System.Net;
using api.Models;
using api.Tests.Fakers;

namespace api.Tests.Controller;

[TestClass]
[TestCategory("Controller")]
public class AuthControllerTests
{
    private static RecordUser _recordUser = null!;
    private static HttpClient _client = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext contex)
    {
        _client = new HttpClient {
            BaseAddress = new Uri("http://localhost:8080") // ajuste conforme necessário
        };
        
        _recordUser = UserFaker.GenerateRecordUser();
    }

    [TestMethod]
    [Priority(1)]
    public async Task SignUp_ShouldReturnOk_WhenUserIsValid()
    {
        var user = new
        {
            name = _recordUser.Name,
            email = _recordUser.Email,
            password = _recordUser.Password
        };

        var response = await _client.PostAsJsonAsync("/auth/signup", user);

        var content = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, content);
    }

    [TestMethod]
    public async Task SignUp_ShouldReturnBadRequest_WhenUserIsInvalid()
    {
        var body = new
        {
            name = "", // inválido
            email = "email-invalido",
            password = "123"
        };

        var response = await _client.PostAsJsonAsync("/auth/signup", body);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Sign_ShouldReturnBadRequest_WhenCredentialIsInvalid()
    {
        var login = new
        {
            email = _recordUser.Email,
            password = "23#$Der#234Wwed"
        };

        var response = await _client.PostAsJsonAsync("/auth/sign", login);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    [Priority(2)]
    public async Task Sign_ShouldReturnOK_WhenCredentialIsValid()
    {
        var login = new
        {
            email = _recordUser.Email,
            password = _recordUser.Password
        };

        var response = await _client.PostAsJsonAsync("/auth/sign", login);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
