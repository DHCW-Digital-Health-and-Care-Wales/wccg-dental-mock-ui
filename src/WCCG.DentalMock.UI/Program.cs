using Microsoft.Extensions.Options;
using WCCG.DentalMock.UI.Configuration;
using WCCG.DentalMock.UI.Configuration.OptionValidators;
using WCCG.DentalMock.UI.Extensions;
using WCCG.DentalMock.UI.Middleware;
using WCCG.DentalMock.UI.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsights(builder.Environment.IsDevelopment(), builder.Configuration);

//EReferralsApiConfig
builder.Services.AddOptions<EReferralsApiConfig>().Bind(builder.Configuration.GetSection(EReferralsApiConfig.SectionName));
builder.Services.AddSingleton<IValidateOptions<EReferralsApiConfig>, ValidateEReferralsApiConfigOptions>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => { options.OperationFilter<SwaggerProcessMessageOperationFilter>(); });

builder.Services.AddHttpClients();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseMiddleware<ResponseMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
