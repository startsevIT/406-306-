using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod());
app.UseSwagger();
app.UseSwaggerUI();

BusSheduleRepository repo = new();

var shedulePath = "/shedule";
app.MapPost(shedulePath, (BusShedule shedule)
    => repo.Create(shedule));
app.MapGet(shedulePath, () 
    => repo.Read());
app.MapPut(shedulePath + "/{id}", (Guid id, BusSheduleUpdateDTO dto)
    => repo.Update(id, dto.StopComplex, dto.DateTime, dto.BusNumber));
app.MapDelete(shedulePath + "/{id}", (Guid id) 
    => repo.Delete(id));

app.Run();


public class AuthService(UserRepo repo)
{
    public void Register(UserRegisterDTO dto)
    {
        if (TryIdentification(dto.Login, out User? user))
            throw new Exception("Такой логин уже занят");

        repo.Create(new User
        {
            Id = Guid.NewGuid(),
            Login = dto.Login,
            HashedPassword = HashPassword(dto.Password),
            Email = dto.Email,
            Phone = dto.Phone,
            Age = dto.Age,
        });
    }
    public string Login(UserLoginDTO dto)
    {
        if (!TryAuthetication(dto, out User? user))
            throw new Exception("Неверные логин или пароль");

        return GenerateToken(user);
    }
    public bool TryIdentification(string login, out User? user)
    {
        user = null;
        try
        {
            user = repo.Read(login);
        }
        catch
        {
            return false;
        }
        return true;
    }
    public bool TryAuthetication(UserLoginDTO dto, out User? result)
    {
       result = null;
       if(!TryIdentification(dto.Login, out User? user))
            return false;

       if(!VirifyPassword(dto.Password, user!.HashedPassword))
            return false;

       result = user;
       return true;
    }

    public bool Authorization(string token)
    {
        throw new NotImplementedException();
    }


    private string HashPassword(string password)
    {
        var hasher = new PasswordHasher<User>();
        return hasher.HashPassword(null, password);
    }

    private bool VirifyPassword(string password, string hashedPassword)
    {
        var hasher = new PasswordHasher<User>();

        var result = hasher.VerifyHashedPassword(null, hashedPassword, password);
        if (result == PasswordVerificationResult.Success)
            return true;
        return false;
    }

    private string GenerateToken(User? user)
    {
        throw new NotImplementedException();
    }
}

public class UserRepo()
{
    private List<User> Users { get; set; }

    public void Create(User user)
    {
        Users.Add(user);
    }

    public User Read(string login)
    {
        var result = Users.SingleOrDefault(x => x.Login == login);
        if (result == null)
            throw new Exception("Такой пользователь не найден");

        return result;
    }
}

public record UserLoginDTO(string Login, string Password);
public record UserRegisterDTO(string Login, string Password, string Email, string Phone, int Age);

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string HashedPassword { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int Age { get; set; }
}

//Вахтомову - 2 пятерки