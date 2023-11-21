using DAL.Models;

namespace BLL.Services.Contracts;

public interface ITokenManager
{
    string GenerateToken(User user);
}