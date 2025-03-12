using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using WMS.BLL;
using WMS.BLL.Services;
using WMS.DAL;
using WMS.DAL.Repositories;
using WMS.Model;

namespace WMS.UI
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static User CurrentUser { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // 加载配置
                ConfigManager.LoadConfig();
                
                // 配置服务
                var services = new ServiceCollection();
                ConfigureServices(services);
                ServiceProvider = services.BuildServiceProvider();
                
                // 显示登录窗口
                using (var loginForm = ServiceProvider.GetRequiredService<LoginForm>())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        // 登录成功，保存当前用户
                        CurrentUser = loginForm.CurrentUser;
                        
                        // 记录应用程序启动日志
                        Logger.Info($"应用程序启动，用户: {CurrentUser.Username}，角色: {CurrentUser.Role}");
                        
                        // 运行主窗体
                        Application.Run(ServiceProvider.GetRequiredService<MainForm>());
                        
                        // 记录应用程序退出日志
                        Logger.Info($"应用程序退出，用户: {CurrentUser.Username}");
                        
                        // 记录用户登出
                        var userService = ServiceProvider.GetRequiredService<UserService>();
                        userService.LogoutAsync(CurrentUser.Id, DateTime.Now).Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"应用程序启动失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Critical("应用程序启动失败", ex);
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // 获取连接字符串
            string connectionString = ConfigManager.Config.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Server=localhost;Database=WMS;Trusted_Connection=True;";
                ConfigManager.UpdateConnectionString(connectionString);
            }

            // 注册 DAL 服务
            services.AddSingleton(new DatabaseConnection(connectionString));
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Warehouse>, WarehouseRepository>();
            services.AddScoped<IRepository<Inventory>, InventoryRepository>();
            services.AddScoped<UserRepository>();

            // 注册 BLL 服务
            services.AddScoped<IService<Product>, ProductService>();
            services.AddScoped<IService<Warehouse>, WarehouseService>();
            services.AddScoped<IService<Inventory>, InventoryService>();
            services.AddScoped<UserService>();

            // 注册窗体
            services.AddScoped<LoginForm>();
            services.AddScoped<MainForm>();
            services.AddTransient<ProductForm>();
            services.AddTransient<WarehouseForm>();
            services.AddTransient<InventoryForm>();
        }
    }
}