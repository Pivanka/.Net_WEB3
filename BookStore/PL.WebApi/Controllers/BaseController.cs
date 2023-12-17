using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace PL.WebApi.Controllers;

public class BaseController : ControllerBase
{
    protected int UserId =>
        !User.Identity!.IsAuthenticated ? 0 : int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}