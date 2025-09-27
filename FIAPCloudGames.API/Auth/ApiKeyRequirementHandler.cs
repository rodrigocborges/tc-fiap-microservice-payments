using Microsoft.AspNetCore.Authorization;

namespace FIAPCloudGames.API.Auth
{
    public class ApiKeyRequirementHandler : AuthorizationHandler<ApiKeyRequirement>
    {
        private readonly IConfiguration _configuration;
        private const string API_KEY_HEADER_NAME = "x-api-key";

        public ApiKeyRequirementHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            var httpContext = context.Resource as HttpContext;
            if (httpContext == null)
            {
                // Se não conseguir obter o contexto, falha.
                return Task.CompletedTask;
            }

            // Tenta extrair a API Key do header
            if (!httpContext.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var apiKeyFromHeader))
            {
                // Se o header não existir, falha.
                return Task.CompletedTask;
            }

            // Obtém a API Key esperada do appsettings.json
            var realAuthKey = _configuration["AuthKey"];

            // Compara a chave do header com a chave da configuração
            if (apiKeyFromHeader.Equals(realAuthKey))
            {
                // Se as chaves corresponderem, o requisito é satisfeito.
                context.Succeed(requirement);
            }

            // Se as chaves não corresponderem, a autorização falha (implicitamente).
            return Task.CompletedTask;
        }
    }
}
