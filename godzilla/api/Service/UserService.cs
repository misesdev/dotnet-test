using api.Data;
using api.Models;
using api.Models.Common;
using api.Extentios;

namespace api.Service;

public class UserService : BaseService<User> 
{
    private readonly PasswordService _password;
    public UserService(AppDbContex context, PasswordService password) : base(context) {
        this._password = password;
    }

    public async Task<Response<UserDTO>> GetUser(Guid id) 
    {
        var user = await base.GetByIdAsync(id);
        
        if(user == null)
            return Response<UserDTO>.Fail("Usuário não encontrado!");

        return Response<UserDTO>.Ok("", user.ToDto());
    }

}
