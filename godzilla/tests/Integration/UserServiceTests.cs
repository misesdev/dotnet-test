using api.Service;
using api.Models;
using api.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Tests.Services;
 
[TestClass]
public class UserServiceTests
{
    private static AppDbContex _context = null!;
    private static UserService _userService = null!;
    private static User _testUser = null!;

    [ClassInitialize]
    public static async Task ClassInitialize(TestContext testContext)
    {
        var options = new DbContextOptionsBuilder<AppDbContex>()
            .UseInMemoryDatabase("UserServiceTestDb")
            .Options;

        _context = new AppDbContex(options);
        var passwordService = new PasswordService();
        _userService = new UserService(_context, passwordService);

        _testUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Elisson Vale",
            Email = "evale@gmail.com",
            PasswordHash = "hash",
            Salt = "salt",
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(_testUser);
        await _context.SaveChangesAsync();
    }

    [TestMethod]
    public async Task GetUser_ShouldReturnUser_WhenIdExists()
    {
        var response = await _userService.GetUser(_testUser.Id);

        Assert.IsTrue(response.Success);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(_testUser.Id, response.Data!.Id);
    }

    [TestMethod]
    public async Task GetUser_ShouldReturnFail_WhenIdDoesNotExist()
    {
        var response = await _userService.GetUser(Guid.NewGuid());

        Assert.IsFalse(response.Success);
        Assert.AreEqual("Usuário não encontrado!", response.Message);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateUser_WhenUserExists()
    {
        var updatedDto = new UserDTO
        {
            Id = _testUser.Id,
            Name = "Mises Dev",
            Email = "mises@dev.com"
        };

        var response = await _userService.UpdateAsync(updatedDto);

        Assert.IsTrue(response.Success);
        Assert.AreEqual("Usuário atualizado com sucesso!", response.Message);
        Assert.AreEqual("Mises Dev", response.Data!.Name);
        Assert.AreEqual("mises@dev.com", response.Data.Email);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldFail_WhenUserDoesNotExist()
    {
        var fakeDto = new UserDTO
        {
            Id = Guid.NewGuid(),
            Name = "Ghost",
            Email = "ghost@example.com"
        };

        var response = await _userService.UpdateAsync(fakeDto);

        Assert.IsFalse(response.Success);
        Assert.AreEqual("Usuário inexistente!", response.Message);
    }
}


