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

    public async Task<Response<UserDTO>> Register(RecordUser model) 
    {
        if(await base.Exists(u => u.Email == model.Email))
            return Response<UserDTO>.Fail("E-mail já existente! (Jamais seria exibido em cenário real)!");

        if(_password.WeakPassword(model.Password))
            return Response<UserDTO>.Fail("Senha muito fraca!");

        var entity = new User {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Email = model.Email,
            Salt = _password.GenerateSalt(),
            CreatedAt = DateTime.Now,
        };

        entity.PasswordHash = _password.HashPassword(model.Password, entity.Salt);

        await base.AddAsync(entity);

        return Response<UserDTO>.Ok("Usuario cadastrado com sucesso!", entity.ToDto());
    }

    public async Task<Response<UserDTO>> GetUser(Guid id) 
    {
        var user = await base.GetByIdAsync(id);
        
        if(user == null)
            return Response<UserDTO>.Fail("Usuário não encontrado!");

        return Response<UserDTO>.Ok("", user.ToDto());
    }

}
