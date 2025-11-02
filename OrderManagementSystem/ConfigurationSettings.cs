using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Entity.Security;
using OrderManagementSystem.Services.Repository;

namespace OrderManagementSystem
{
    public class ConfigurationSettings
    {
        public static void ConfigSettings(IServiceCollection services, String connectionString)
        {
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(connectionString));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IPurchaseItemRepository, PurchaseItemRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddScoped<IMenuRepository, MenuRepository>();
        }
    }
}
