using Chepyxland.Data;
using Microsoft.EntityFrameworkCore;

namespace Chepyxland.Services;

public class AuthorizeService(ApplicationDbContext context)
{
    public async Task<bool> IsUserExist(string login, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Login == login && user.Password == password);

        return user != null;
    }

    public async Task RegisterUser(string login, string name, string password)
    {
        var user = new User() { Login = login, Password = password, Username = name };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task AddTokenToUser(string login, string token)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Login == login);

        if (user == null)
            throw new InvalidOperationException("User not found");

        user.Token = token;

        await context.SaveChangesAsync();
    }
}