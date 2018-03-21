namespace Budget.Web
{
    using AutoMapper;
    using Budget.Data;
    using Budget.Data.Models;
    using Budget.Data.Models.Enums;
    using Budget.Services;
    using Budget.Services.Contracts;
    using Budget.Web.Infrastructure.ColorGenerator;
    using Budget.Web.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.Globalization;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();

            services.AddDbContext<BudgetDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<BudgetDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IColorGenerator, ColorGenerator>();
            services.AddAutoMapper();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IUserCategoryService, UserCategoryService>();
            services.AddTransient<IUserService, UserService>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services
                .AddMvc(options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResources));
                })
                ;

            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.AreaViewLocationFormats.Add("/Areas/Public/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseAuthentication();

            IList<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("bg-BG"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "area",
                    template: "{area:exists=Public}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            SeedDatabase(app);
        }

        private async void SeedDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<BudgetDbContext>();
                await context.Database.EnsureCreatedAsync();
                if (!await context.Categories.AnyAsync())
                {
                    var colorGenerator = app.ApplicationServices.GetService<IColorGenerator>();
                    await context.Categories.AddRangeAsync(GetCategoriesToSeed(colorGenerator));

                    await context.SaveChangesAsync();
                }
            }
        }

        private static IEnumerable<Category> GetCategoriesToSeed(IColorGenerator colorGenerator)
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Salary",
                    TransactionType = TransactionType.Income,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                },
                new Category
                {
                    Name = "Deposit",
                    TransactionType = TransactionType.Income,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                },
                new Category
                {
                    Name = "Savings",
                    TransactionType = TransactionType.Income,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                },
                new Category
                {
                    Name = "Bills",
                    TransactionType = TransactionType.Expense,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                },
                new Category
                {
                    Name = "Car",
                    TransactionType = TransactionType.Expense,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                },
                new Category
                {
                    Name = "Transport",
                    TransactionType = TransactionType.Expense,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                },
                new Category
                {
                    Name = "Education",
                    TransactionType = TransactionType.Expense,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                },
                new Category
                {
                    Name = "Sports",
                    TransactionType = TransactionType.Expense,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                },
                new Category
                {
                    Name = "Food",
                    TransactionType = TransactionType.Expense,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                },
                new Category
                {
                    Name = "Home",
                    TransactionType = TransactionType.Expense,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                },
                new Category
                {
                    Name = "Eating out",
                    TransactionType = TransactionType.Expense,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                },
                new Category
                {
                    Name = "Personal",
                    TransactionType = TransactionType.Expense,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                },
                new Category
                {
                    Name = "Health",
                    TransactionType = TransactionType.Expense,
                    IsPrimary = true,
                    RgbColorValue = colorGenerator.GetColor()
                }
            };

            return categories;
        }
    }
}
