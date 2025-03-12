using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using WMS.BLL;
using WMS.Model;
using Timer = System.Windows.Forms.Timer;

namespace WMS.UI
{
    public partial class MainForm : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private User _currentUser;
        private ToolStripStatusLabel _lblUserInfo;
        private ToolStripStatusLabel _lblDateTime;
        private System.Windows.Forms.Timer _timer;

        public MainForm(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _currentUser = Program.CurrentUser;
            
            // Set form properties
            this.Text = ConfigManager.Config.SystemName;
            this.WindowState = FormWindowState.Maximized;
            
            // Initialize UI with user data
            UpdateUserInfoLabel();
            
            // Start timer for clock
            StartTimer();
            
            // Set menu permissions based on user role
            SetMenuPermissions();
            
            // Record log
            Logger.Info($"Main form loaded, user: {_currentUser.Username}");
        }

        private void InitializeMenu()
        {
            // Create main menu
            MenuStrip mainMenu = new MenuStrip();
            this.MainMenuStrip = mainMenu;
            this.Controls.Add(mainMenu);

            // Create menu items
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit", null, (s, e) => Application.Exit());
            fileMenu.DropDownItems.Add(exitMenuItem);

            ToolStripMenuItem masterDataMenu = new ToolStripMenuItem("Master Data");
            ToolStripMenuItem productsMenuItem = new ToolStripMenuItem("Products", null, OpenProductForm);
            ToolStripMenuItem warehousesMenuItem = new ToolStripMenuItem("Warehouses", null, OpenWarehouseForm);
            masterDataMenu.DropDownItems.Add(productsMenuItem);
            masterDataMenu.DropDownItems.Add(warehousesMenuItem);

            ToolStripMenuItem inventoryMenu = new ToolStripMenuItem("Inventory");
            ToolStripMenuItem inventoryManagementMenuItem = new ToolStripMenuItem("Inventory Management", null, OpenInventoryForm);
            inventoryMenu.DropDownItems.Add(inventoryManagementMenuItem);

            ToolStripMenuItem operationsMenu = new ToolStripMenuItem("Operations");
            ToolStripMenuItem inboundMenuItem = new ToolStripMenuItem("Inbound Orders");
            ToolStripMenuItem outboundMenuItem = new ToolStripMenuItem("Outbound Orders");
            operationsMenu.DropDownItems.Add(inboundMenuItem);
            operationsMenu.DropDownItems.Add(outboundMenuItem);

            ToolStripMenuItem reportsMenu = new ToolStripMenuItem("Reports");
            ToolStripMenuItem inventoryReportMenuItem = new ToolStripMenuItem("Inventory Report");
            ToolStripMenuItem movementReportMenuItem = new ToolStripMenuItem("Movement Report");
            reportsMenu.DropDownItems.Add(inventoryReportMenuItem);
            reportsMenu.DropDownItems.Add(movementReportMenuItem);

            ToolStripMenuItem systemMenu = new ToolStripMenuItem("System");
            ToolStripMenuItem userManagementMenuItem = new ToolStripMenuItem("User Management");
            ToolStripMenuItem changePasswordMenuItem = new ToolStripMenuItem("Change Password");
            ToolStripMenuItem settingsMenuItem = new ToolStripMenuItem("System Settings");
            ToolStripMenuItem logViewerMenuItem = new ToolStripMenuItem("Log Viewer");
            systemMenu.DropDownItems.Add(userManagementMenuItem);
            systemMenu.DropDownItems.Add(changePasswordMenuItem);
            systemMenu.DropDownItems.Add(settingsMenuItem);
            systemMenu.DropDownItems.Add(logViewerMenuItem);

            // Add menu items to main menu
            mainMenu.Items.Add(fileMenu);
            mainMenu.Items.Add(masterDataMenu);
            mainMenu.Items.Add(inventoryMenu);
            mainMenu.Items.Add(operationsMenu);
            mainMenu.Items.Add(reportsMenu);
            mainMenu.Items.Add(systemMenu);
        }
        
        private void InitializeStatusBar()
        {
            // Create status bar
            StatusStrip statusStrip = new StatusStrip();
            this.Controls.Add(statusStrip);
            
            // Create user info label
            _lblUserInfo = new ToolStripStatusLabel();
            UpdateUserInfoLabel();
            statusStrip.Items.Add(_lblUserInfo);
            
            // Create separator
            statusStrip.Items.Add(new ToolStripSeparator());
            
            // Create date time label
            _lblDateTime = new ToolStripStatusLabel();
            UpdateDateTimeLabel();
            statusStrip.Items.Add(_lblDateTime);
        }
        
        private void StartTimer()
        {
            _timer = new Timer();
            _timer.Interval = 1000; // 1 second
            _timer.Tick += (s, e) => UpdateDateTimeLabel();
            _timer.Start();
        }
        
        private void UpdateUserInfoLabel()
        {
            _lblUserInfo.Text = $"Current user: {_currentUser.FullName} ({GetRoleName(_currentUser.Role)})";
        }
        
        private void UpdateDateTimeLabel()
        {
            _lblDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        
        private string GetRoleName(UserRole role)
        {
            switch (role)
            {
                case UserRole.Administrator:
                    return "System Administrator";
                case UserRole.Manager:
                    return "Warehouse Manager";
                case UserRole.Operator:
                    return "Operator";
                case UserRole.Viewer:
                    return "Viewer";
                default:
                    return "Unknown";
            }
        }
        
        private void SetMenuPermissions()
        {
            // Set menu permissions based on user role
            // This is a simple example, actual implementation may require more complex permission control
            switch (_currentUser.Role)
            {
                case UserRole.Administrator:
                    // Administrator has all permissions
                    break;
                case UserRole.Manager:
                    // Manager can view all data but cannot manage users or system settings
                    DisableMenuItem("System/User Management");
                    DisableMenuItem("System/System Settings");
                    break;
                case UserRole.Operator:
                    // Operator can only perform basic operations
                    DisableMenuItem("System/User Management");
                    DisableMenuItem("System/System Settings");
                    DisableMenuItem("System/Log Viewer");
                    break;
                case UserRole.Viewer:
                    // Viewer can only view data and cannot perform any modification operations
                    DisableMenuItem("Master Data", false);
                    DisableMenuItem("Operations", false);
                    DisableMenuItem("System/User Management");
                    DisableMenuItem("System/System Settings");
                    DisableMenuItem("System/Log Viewer");
                    break;
            }
        }
        
        private void DisableMenuItem(string menuPath, bool disableChildren = true)
        {
            string[] parts = menuPath.Split('/');
            if (parts.Length == 0) return;
            
            // Find main menu item
            ToolStripMenuItem mainMenuItem = null;
            foreach (ToolStripItem item in this.MainMenuStrip.Items)
            {
                if (item is ToolStripMenuItem menuItem && menuItem.Text == parts[0])
                {
                    mainMenuItem = menuItem;
                    break;
                }
            }
            
            if (mainMenuItem == null) return;
            
            if (parts.Length == 1)
            {
                // Disable entire main menu
                mainMenuItem.Enabled = false;
                return;
            }
            
            // Find submenu item
            ToolStripMenuItem subMenuItem = null;
            foreach (ToolStripItem item in mainMenuItem.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem && menuItem.Text == parts[1])
                {
                    subMenuItem = menuItem;
                    break;
                }
            }
            
            if (subMenuItem == null) return;
            
            // Disable submenu
            subMenuItem.Enabled = false;
            
            // If needed, disable all submenu items
            if (disableChildren)
            {
                foreach (ToolStripItem item in subMenuItem.DropDownItems)
                {
                    if (item is ToolStripMenuItem menuItem)
                    {
                        menuItem.Enabled = false;
                    }
                }
            }
        }

        private void OpenProductForm(object sender, EventArgs e)
        {
            try
            {
                var productForm = _serviceProvider.GetRequiredService<ProductForm>();
                productForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open product management window: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("Failed to open product management window", ex);
            }
        }

        private void OpenWarehouseForm(object sender, EventArgs e)
        {
            try
            {
                var warehouseForm = _serviceProvider.GetRequiredService<WarehouseForm>();
                warehouseForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open warehouse management window: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("Failed to open warehouse management window", ex);
            }
        }

        private void OpenInventoryForm(object sender, EventArgs e)
        {
            try
            {
                var inventoryForm = _serviceProvider.GetRequiredService<InventoryForm>();
                inventoryForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open inventory management window: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("Failed to open inventory management window", ex);
            }
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            
            // Record log
            Logger.Info($"Main form closed, user: {_currentUser.Username}");
        }
    }
} 