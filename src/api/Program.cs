using api.Configuration;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureGraphClient(builder.Configuration);

builder.Services.Configure<SecretOptions>(
    builder.Configuration.GetSection(SecretOptions.Section));
builder.Services.AddSingleton<ICredentialsSource, CredentialsSource>();

builder.Services.Configure<MailOptions>(
    builder.Configuration.GetSection(MailOptions.Section));
builder.Services.AddScoped<IMailSender, MailSender>();

builder.Services.Configure<TokenizerOptions>(
    builder.Configuration.GetSection(TokenizerOptions.Section));
builder.Services.AddScoped<ITokenizer, Tokenizer>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind("AzureAdB2C", options);

            options.TokenValidationParameters.NameClaimType = "name";
        },
        options => { builder.Configuration.Bind("AzureAdB2C", options); });

builder.Services.AddRouting(o => o.LowercaseUrls = true);
builder.Services.AddAuthorization();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy("default", blder =>
{
    blder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("default");

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Templates")),
    RequestPath = new PathString("/Templates")
});

app.Run();
