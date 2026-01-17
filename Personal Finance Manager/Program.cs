using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_Manager.Data;
using Personal_Finance_Manager.Data.Repositories;
using Personal_Finance_Manager.Observers;
using Personal_Finance_Manager.Services;
using Personal_Finance_Manager.Undo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; 
})
.AddRoles<IdentityRole>()                           
.AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();

builder.Services.AddSingleton<UndoService>();
builder.Services.AddSingleton<TransactionEventBus>();
builder.Services.AddSingleton<MessageService>();

builder.Services.AddSingleton<AuditLogObserver>();
builder.Services.AddSingleton<MessageObserver>();
builder.Services.AddScoped<ReportGenerator>();


var app = builder.Build();

var bus = app.Services.GetRequiredService<TransactionEventBus>();
bus.Subscribe(app.Services.GetRequiredService<AuditLogObserver>());
bus.Subscribe(app.Services.GetRequiredService<MessageObserver>());


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<UserRoleMiddleware>();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

await Personal_Finance_Manager.Data.IdentitySeed.Seed(app.Services);

app.Run();
