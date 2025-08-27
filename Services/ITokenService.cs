using BornOtomasyonApi.Models;

namespace BornOtomasyonApi.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}
