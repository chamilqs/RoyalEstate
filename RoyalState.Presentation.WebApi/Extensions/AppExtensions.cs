using RoyalState.Presentation.WebApi.Middlewares;

namespace RoyalState.WebApi.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwagggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "RoyalState API");
            });
        }
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
