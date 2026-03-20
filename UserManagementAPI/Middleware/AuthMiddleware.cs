namespace UserManagementAPI.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEY = "TechHive123";

        public AuthMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedKey) || extractedKey != APIKEY)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Invalid or missing API Key.");
                return;
            }
            await _next(context);
        }
    }
}
