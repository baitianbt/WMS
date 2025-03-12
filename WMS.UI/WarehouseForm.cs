using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMS.BLL.Services;
using WMS.Model;

namespace WMS.UI
{
    public partial class WarehouseForm : Form
    {
        private readonly IService<Warehouse> _warehouseService;
        private List<Warehouse> _warehouses;
        private Warehouse _selectedWarehouse;

        public WarehouseForm(IService<Warehouse> warehouseService)
        {
            InitializeComponent();
            _warehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
            
            // Set form properties
            this.Text = "Warehouse Management";
            this.Size = new System.Drawing.Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            
            // Initialize controls
            InitializeControls();
            
            // Load data
            LoadWarehousesAsync();
        }

        private void InitializeControls()
        {
            // Create layout panel
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            this.Controls.Add(mainLayout);

            // Create DataGridView for warehouses
            dgvWarehouses = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvWarehouses.SelectionChanged += DgvWarehouses_SelectionChanged;
            mainLayout.Controls.Add(dgvWarehouses, 0, 0);

            // Create panel for warehouse details
            Panel detailsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            mainLayout.Controls.Add(detailsPanel, 1, 0);

            // Create details layout
            TableLayoutPanel detailsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 9,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 30F), new ColumnStyle(SizeType.Percent, 70F) }
            };
            detailsPanel.Controls.Add(detailsLayout);

            // Add labels and textboxes
            detailsLayout.Controls.Add(new Label { Text = "Code:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 0);
            txtCode = new TextBox { Dock = DockStyle.Fill };
            detailsLayout.Controls.Add(txtCode, 1, 0);

            detailsLayout.Controls.Add(new Label { Text = "Name:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 1);
            txtName = new TextBox { Dock = DockStyle.Fill };
            detailsLayout.Controls.Add(txtName, 1, 1);

            detailsLayout.Controls.Add(new Label { Text = "Address:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 2);
            txtAddress = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 60 };
            detailsLayout.Controls.Add(txtAddress, 1, 2);
            detailsLayout.SetRowSpan(txtAddress, 2);

            detailsLayout.Controls.Add(new Label { Text = "Contact:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 4);
            txtContact = new TextBox { Dock = DockStyle.Fill };
            detailsLayout.Controls.Add(txtContact, 1, 4);

            detailsLayout.Controls.Add(new Label { Text = "Phone:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 5);
            txtPhone = new TextBox { Dock = DockStyle.Fill };
            detailsLayout.Controls.Add(txtPhone, 1, 5);

            detailsLayout.Controls.Add(new Label { Text = "Description:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 6);
            txtDescription = new TextBox { Dock = DockStyle.Fill, Multiline = true };
            detailsLayout.Controls.Add(txtDescription, 1, 6);

            detailsLayout.Controls.Add(new Label { Text = "Active:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 7);
            chkActive = new CheckBox { Dock = DockStyle.Fill };
            detailsLayout.Controls.Add(chkActive, 1, 7);

            // Add buttons panel
            FlowLayoutPanel buttonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };
            detailsLayout.Controls.Add(buttonsPanel, 1, 8);

            btnSave = new Button { Text = "Save", Width = 80 };
            btnNew = new Button { Text = "New", Width = 80 };
            btnDelete = new Button { Text = "Delete", Width = 80 };
            btnSave.Click += BtnSave_Click;
            btnNew.Click += BtnNew_Click;
            btnDelete.Click += BtnDelete_Click;

            buttonsPanel.Controls.Add(btnSave);
            buttonsPanel.Controls.Add(btnNew);
            buttonsPanel.Controls.Add(btnDelete);
        }

        private async void LoadWarehousesAsync()
        {
            try
            {
                _warehouses = (await _warehouseService.GetAllAsync()).ToList();
                BindWarehousesGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading warehouses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindWarehousesGrid()
        {
            dgvWarehouses.DataSource = null;
            dgvWarehouses.DataSource = _warehouses;
            
            // Configure columns
            dgvWarehouses.Columns["Id"].Visible = false;
            dgvWarehouses.Columns["CreatedTime"].Visible = false;
            dgvWarehouses.Columns["UpdatedTime"].Visible = false;
            
            if (dgvWarehouses.Columns.Contains("Description"))
                dgvWarehouses.Columns["Description"].Visible = false;
        }

        private void DgvWarehouses_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvWarehouses.SelectedRows.Count > 0)
            {
                _selectedWarehouse = (Warehouse)dgvWarehouses.SelectedRows[0].DataBoundItem;
                DisplayWarehouseDetails();
            }
        }

        private void DisplayWarehouseDetails()
        {
            if (_selectedWarehouse != null)
            {
                txtCode.Text = _selectedWarehouse.Code;
                txtName.Text = _selectedWarehouse.Name;
                txtAddress.Text = _selectedWarehouse.Address;
                txtContact.Text = _selectedWarehouse.Contact;
                txtPhone.Text = _selectedWarehouse.Phone;
                txtDescription.Text = _selectedWarehouse.Description;
                chkActive.Checked = _selectedWarehouse.IsActive;
            }
        }

        private void ClearWarehouseDetails()
        {
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtContact.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtDescription.Text = string.Empty;
            chkActive.Checked = true;
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCode.Text) || string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Code and Name are required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_selectedWarehouse == null)
                {
                    // Add new warehouse
                    var newWarehouse = new Warehouse
                    {
                        Code = txtCode.Text,
                        Name = txtName.Text,
                        Address = txtAddress.Text,
                        Contact = txtContact.Text,
                        Phone = txtPhone.Text,
                        Description = txtDescription.Text,
                        IsActive = chkActive.Checked
                    };

                    int newId = await _warehouseService.AddAsync(newWarehouse);
                    newWarehouse.Id = newId;
                    _warehouses.Add(newWarehouse);
                    MessageBox.Show("Warehouse added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Update existing warehouse
                    _selectedWarehouse.Code = txtCode.Text;
                    _selectedWarehouse.Name = txtName.Text;
                    _selectedWarehouse.Address = txtAddress.Text;
                    _selectedWarehouse.Contact = txtContact.Text;
                    _selectedWarehouse.Phone = txtPhone.Text;
                    _selectedWarehouse.Description = txtDescription.Text;
                    _selectedWarehouse.IsActive = chkActive.Checked;

                    await _warehouseService.UpdateAsync(_selectedWarehouse);
                    MessageBox.Show("Warehouse updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                BindWarehousesGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving warehouse: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            _selectedWarehouse = null;
            ClearWarehouseDetails();
            dgvWarehouses.ClearSelection();
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedWarehouse == null)
            {
                MessageBox.Show("Please select a warehouse to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete warehouse '{_selectedWarehouse.Name}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _warehouseService.DeleteAsync(_selectedWarehouse.Id);
                    _warehouses.Remove(_selectedWarehouse);
                    BindWarehousesGrid();
                    ClearWarehouseDetails();
                    _selectedWarehouse = null;
                    MessageBox.Show("Warehouse deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting warehouse: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Designer variables
        private DataGridView dgvWarehouses;
        private TextBox txtCode;
        private TextBox txtName;
        private TextBox txtAddress;
        private TextBox txtContact;
        private TextBox txtPhone;
        private TextBox txtDescription;
        private CheckBox chkActive;
        private Button btnSave;
        private Button btnNew;
        private Button btnDelete;
    }
} 