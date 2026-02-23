using ActViewer.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<ActDb>();
builder.Services.AddScoped<ActRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapGet("/", ctx =>
{
    ctx.Response.Redirect("/Contacts");
    return Task.CompletedTask;
});

app.Run();
