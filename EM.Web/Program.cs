using EM.Repository.Repositories;
using FirebirdSql.Data.FirebirdClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<FbConnection>(_ => new FbConnection(builder.Configuration.GetConnectionString("DefaultDatabase") ?? throw new InvalidOperationException("Connection string 'DefaultDatabase' not found.")));

builder.Services.AddScoped<AlunoRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	//app.UseExceptionHandler("/Aluno/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Aluno}/{action=Index}/{id?}");

app.Run();
