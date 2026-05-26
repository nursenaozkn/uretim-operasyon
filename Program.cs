using UretimOperasyon.Services;

var builder = WebApplication.CreateBuilder(args);

// PaaS sağlayıcıları (Render, Railway, Azure vb.) dinlenecek portu PORT ortam
// değişkeniyle verir. Varsa onu kullan, yoksa yerelde 5080.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

builder.Services.AddRazorPages();
builder.Services.AddSingleton<OperasyonRaporServisi>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
