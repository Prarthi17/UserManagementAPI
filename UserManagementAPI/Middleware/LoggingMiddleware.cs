namespace UserManagementAPI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            // Log Request
            Console.WriteLine($"[Request] {context.Request.Method} {context.Request.Path}");

            await _next(context);

            // Log Response
            Console.WriteLine($"[Response] Status Code: {context.Response.StatusCode}");
        }
    }
}
