using BookStore.Domain.Entities;

namespace Bookstore.Application.Services.Contracts;

public interface ITokenManager
{
    string GenerateToken(User user);
}