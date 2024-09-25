namespace US_Elections
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var allowAnyOriginPolicy = "_allowAnyOrigin";
 
            // Add services to the container.
 
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                                name: allowAnyOriginPolicy,
                                policy =>
                                {
                                    policy.AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                });
            });
 
            var app = builder.Build();
 
            app.UseCors(allowAnyOriginPolicy);
 
            app.UseSwagger();
            app.UseSwaggerUI();
 
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
            }
 
            app.UseStaticFiles();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
