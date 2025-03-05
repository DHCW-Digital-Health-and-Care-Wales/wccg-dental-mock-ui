using System.Text.Json;
using Microsoft.Extensions.Options;
using WCCG.DentalMock.UI.Configuration;
using WCCG.DentalMock.UI.Configuration.OptionValidators;
using WCCG.DentalMock.UI.Extensions;
using WCCG.DentalMock.UI.Middleware;
using WCCG.DentalMock.UI.Swagger;

var builder = WebApplication.CreateBuilder(args);

//EReferralsApiConfig
builder.Services.AddOptions<EReferralsApiConfig>().Bind(builder.Configuration.GetSection(EReferralsApiConfig.SectionName));
builder.Services.AddSingleton<IValidateOptions<EReferralsApiConfig>, ValidateEReferralsApiConfigOptions>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => { options.OperationFilter<SwaggerProcessMessageOperationFilter>(); });

builder.Services.AddApplicationInsights(builder.Environment.IsDevelopment(), builder.Configuration);
builder.Services.AddSingleton(new JsonSerializerOptions().ForFhirExtended());

builder.Services.AddHttpClients();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseMiddleware<ResponseMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
