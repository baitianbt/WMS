using System;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WMS.BLL;
using WMS.BLL.Services;
using WMS.Model;

namespace WMS.UI
{
    public partial class LoginForm : Form
    {
        private readonly UserService _userService;
        private User _currentUser;

        public User CurrentUser => _currentUser;

        public LoginForm(UserService userService)
        {
            InitializeComponent();
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            
            // Set form properties
            this.Text = ConfigManager.Config.SystemName + " - Login";
            
            // Load system information
            LoadSystemInfo();
            
            // Load saved username
            LoadSavedUsername();
        }

        private void LoadSystemInfo()
        {
            // Load system information, such as company name, logo, etc.
            lblTitle.Text = ConfigManager.Config.SystemName;
            lblVersion.Text = "Version: " + ConfigManager.Config.Version;
            
            if (!string.IsNullOrEmpty(ConfigManager.Config.CompanyName))
            {
                this.Text = ConfigManager.Config.CompanyName + " - " + ConfigManager.Config.SystemName;
            }
            
            // Load logo
            if (!string.IsNullOrEmpty(ConfigManager.Config.CompanyLogo) && System.IO.File.Exists(ConfigManager.Config.CompanyLogo))
            {
                try
                {
                    this.Icon = new Icon(ConfigManager.Config.CompanyLogo);
                }
                catch (Exception ex)
                {
                    Logger.Error("Failed to load company logo", ex);
                }
            }
        }

        private void LoadSavedUsername()
        {
            string savedUsername = ConfigManager.GetCustomSetting("LastUsername");
            if (!string.IsNullOrEmpty(savedUsername))
            {
                txtUsername.Text = savedUsername;
                txtPassword.Focus();
            }
        }

        private void SaveUsername()
        {
            if (chkRememberUsername.Checked && !string.IsNullOrEmpty(txtUsername.Text))
            {
                ConfigManager.AddOrUpdateCustomSetting("LastUsername", txtUsername.Text);
            }
            else
            {
                ConfigManager.AddOrUpdateCustomSetting("LastUsername", string.Empty);
            }
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable login button to prevent multiple clicks
                btnLogin.Enabled = false;
                
                // Validate input
                if (string.IsNullOrEmpty(txtUsername.Text))
                {
                    MessageBox.Show("Please enter username", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }
                
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Please enter password", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }
                
                // Get IP address
                string ipAddress = GetLocalIPAddress();
                
                // Login
                _currentUser = await _userService.LoginAsync(txtUsername.Text, txtPassword.Text, ipAddress);
                
                // Save username
                SaveUsername();
                
                // Login successful, close login form
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
            finally
            {
                // Restore login button
                btnLogin.Enabled = true;
            }
        }

        private string GetLocalIPAddress()
        {
            try
            {
                string hostName = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
                
                foreach (IPAddress ip in hostEntry.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                
                return "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }
    }
} 