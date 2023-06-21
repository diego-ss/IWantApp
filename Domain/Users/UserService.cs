using System.Security.Claims;

namespace IWantApp.Domain.Users;

public class UserService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager)
    {
        this._userManager = userManager;
    }

    public async Task<(IdentityResult, string)> Create(string name, string email, string password, List<Claim> claims)
    {
        // criando novo usuário
        var client = new IdentityUser { UserName = name, Email = email };
        var result = await _userManager.CreateAsync(client, password);

        if (!result.Succeeded)
            return (result, String.Empty);

        return (await _userManager.AddClaimsAsync(client, claims), client.Id);
    }
}
