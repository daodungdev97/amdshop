using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository;
using Ecom.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Ecom.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Ecom.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    options =>
    {
        // options.SignIn.RequireConfirmedAccount = true;//confirm qua email
        //options.Password.RequireDigit = false; // Không bắt phải có số
        //options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
        //options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
        //options.Password.RequireUppercase = false; // Không bắt buộc chữ in
        //options.Password.RequiredLength = 1; // Số ký tự tối thiểu của password
        //options.Password.RequiredUniqueChars = 0;
        options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true; // Email là duy nhất

         // Cấu hình Lockout - khóa user
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
        options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lầ thì khóa
        options.Lockout.AllowedForNewUsers = true;
    }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "750645303422818";
    option.AppSecret = "99ba9e7284ca68943f47b07da5e082fa";
});
builder.Services.AddDistributedMemoryCache();//DistributedMemoryCache là một bộ nhớ cache được lưu trữ trong bộ nhớ và được chia sẻ giữa các phiên bản của ứng dụng.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);//, tức là sau khi phiên không được sử dụng trong 100 phút, nó sẽ hết hiệu lực và bị xóa đi.
    options.Cookie.HttpOnly = true;//Cờ HttpOnly chỉ cho phép cookie được truy cập qua giao thức HTTP và không cho phép truy cập qua JavaScript, giúp ngăn chặn tấn công Cross-Site Scripting (XSS).
    options.Cookie.IsEssential = true;//. Cờ IsEssential chỉ ra rằng cookie này là cần thiết cho hoạt động của ứng dụng và không nên bị vô hiệu hóa ngay cả khi người dùng từ chối chấp nhận cookie.
});

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
SeedDatabase();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initializer();
    }


}
