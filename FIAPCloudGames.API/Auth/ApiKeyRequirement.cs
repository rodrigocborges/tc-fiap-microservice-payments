using Microsoft.AspNetCore.Authorization;

namespace FIAPCloudGames.API.Auth
{
    public class ApiKeyRequirement : IAuthorizationRequirement
    {
        public ApiKeyRequirement() { }
    }
}
