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
    public partial class InventoryForm : Form
    {
        private readonly IService<Inventory> _inventoryService;
        private readonly IService<Product> _productService;
        private readonly IService<Warehouse> _warehouseService;
        private List<Inventory> _inventories;
        private List<Product> _products;
        private List<Warehouse> _warehouses;
        private Inventory _selectedInventory;

        public InventoryForm(
            IService<Inventory> inventoryService,
            IService<Product> productService,
            IService<Warehouse> warehouseService)
        {
            InitializeComponent();
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _warehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
            
            // Set form properties
            this.Text = "Inventory Management";
            this.Size = new System.Drawing.Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            
            // Initialize controls
            InitializeControls();
            
            // Load data
            LoadDataAsync();
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

            // Create DataGridView for inventories
            dgvInventories = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvInventories.SelectionChanged += DgvInventories_SelectionChanged;
            mainLayout.Controls.Add(dgvInventories, 0, 0);

            // Create panel for inventory details
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
                RowCount = 8,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 30F), new ColumnStyle(SizeType.Percent, 70F) }
            };
            detailsPanel.Controls.Add(detailsLayout);

            // Add labels and controls
            detailsLayout.Controls.Add(new Label { Text = "Product:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 0);
            cboProduct = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            detailsLayout.Controls.Add(cboProduct, 1, 0);

            detailsLayout.Controls.Add(new Label { Text = "Warehouse:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 1);
            cboWarehouse = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            detailsLayout.Controls.Add(cboWarehouse, 1, 1);

            detailsLayout.Controls.Add(new Label { Text = "Location:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 2);
            txtLocation = new TextBox { Dock = DockStyle.Fill };
            detailsLayout.Controls.Add(txtLocation, 1, 2);

            detailsLayout.Controls.Add(new Label { Text = "Quantity:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 3);
            txtQuantity = new TextBox { Dock = DockStyle.Fill };
            detailsLayout.Controls.Add(txtQuantity, 1, 3);

            detailsLayout.Controls.Add(new Label { Text = "Min Quantity:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 4);
            txtMinQuantity = new TextBox { Dock = DockStyle.Fill };
            detailsLayout.Controls.Add(txtMinQuantity, 1, 4);

            detailsLayout.Controls.Add(new Label { Text = "Max Quantity:", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, 0, 5);
            txtMaxQuantity = new TextBox { Dock = DockStyle.Fill };
            detailsLayout.Controls.Add(txtMaxQuantity, 1, 5);

            // Add buttons panel
            FlowLayoutPanel buttonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };
            detailsLayout.Controls.Add(buttonsPanel, 1, 7);

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

        private async void LoadDataAsync()
        {
            try
            {
                // Load products
                _products = (await _productService.GetAllAsync()).ToList();
                cboProduct.DataSource = null;
                cboProduct.DisplayMember = "Name";
                cboProduct.ValueMember = "Id";
                cboProduct.DataSource = _products;

                // Load warehouses
                _warehouses = (await _warehouseService.GetAllAsync()).ToList();
                cboWarehouse.DataSource = null;
                cboWarehouse.DisplayMember = "Name";
                cboWarehouse.ValueMember = "Id";
                cboWarehouse.DataSource = _warehouses;

                // Load inventories
                _inventories = (await _inventoryService.GetAllAsync()).ToList();
                BindInventoriesGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindInventoriesGrid()
        {
            dgvInventories.DataSource = null;
            
            // Create a custom view model for the grid
            var inventoryViewModels = _inventories.Select(i => new
            {
                i.Id,
                ProductCode = i.Product?.Code,
                ProductName = i.Product?.Name,
                WarehouseName = i.Warehouse?.Name,
                i.Location,
                i.Quantity,
                i.MinQuantity,
                i.MaxQuantity
            }).ToList();
            
            dgvInventories.DataSource = inventoryViewModels;
            
            // Configure columns
            if (dgvInventories.Columns.Contains("Id"))
                dgvInventories.Columns["Id"].Visible = false;
        }

        private void DgvInventories_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvInventories.SelectedRows.Count > 0)
            {
                int selectedId = (int)dgvInventories.SelectedRows[0].Cells["Id"].Value;
                _selectedInventory = _inventories.FirstOrDefault(i => i.Id == selectedId);
                DisplayInventoryDetails();
            }
        }

        private void DisplayInventoryDetails()
        {
            if (_selectedInventory != null)
            {
                cboProduct.SelectedValue = _selectedInventory.ProductId;
                cboWarehouse.SelectedValue = _selectedInventory.WarehouseId;
                txtLocation.Text = _selectedInventory.Location;
                txtQuantity.Text = _selectedInventory.Quantity.ToString();
                txtMinQuantity.Text = _selectedInventory.MinQuantity.ToString();
                txtMaxQuantity.Text = _selectedInventory.MaxQuantity.ToString();
            }
        }

        private void ClearInventoryDetails()
        {
            if (cboProduct.Items.Count > 0) cboProduct.SelectedIndex = 0;
            if (cboWarehouse.Items.Count > 0) cboWarehouse.SelectedIndex = 0;
            txtLocation.Text = string.Empty;
            txtQuantity.Text = "0";
            txtMinQuantity.Text = "0";
            txtMaxQuantity.Text = "0";
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboProduct.SelectedValue == null || cboWarehouse.SelectedValue == null)
                {
                    MessageBox.Show("Product and Warehouse are required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtQuantity.Text, out decimal quantity) ||
                    !decimal.TryParse(txtMinQuantity.Text, out decimal minQuantity) ||
                    !decimal.TryParse(txtMaxQuantity.Text, out decimal maxQuantity))
                {
                    MessageBox.Show("Quantity, Min Quantity, and Max Quantity must be valid numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_selectedInventory == null)
                {
                    // Add new inventory
                    var newInventory = new Inventory
                    {
                        ProductId = (int)cboProduct.SelectedValue,
                        WarehouseId = (int)cboWarehouse.SelectedValue,
                        Location = txtLocation.Text,
                        Quantity = quantity,
                        MinQuantity = minQuantity,
                        MaxQuantity = maxQuantity
                    };

                    int newId = await _inventoryService.AddAsync(newInventory);
                    newInventory.Id = newId;
                    newInventory.Product = _products.FirstOrDefault(p => p.Id == newInventory.ProductId);
                    newInventory.Warehouse = _warehouses.FirstOrDefault(w => w.Id == newInventory.WarehouseId);
                    _inventories.Add(newInventory);
                    MessageBox.Show("Inventory added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Update existing inventory
                    _selectedInventory.ProductId = (int)cboProduct.SelectedValue;
                    _selectedInventory.WarehouseId = (int)cboWarehouse.SelectedValue;
                    _selectedInventory.Location = txtLocation.Text;
                    _selectedInventory.Quantity = quantity;
                    _selectedInventory.MinQuantity = minQuantity;
                    _selectedInventory.MaxQuantity = maxQuantity;

                    await _inventoryService.UpdateAsync(_selectedInventory);
                    _selectedInventory.Product = _products.FirstOrDefault(p => p.Id == _selectedInventory.ProductId);
                    _selectedInventory.Warehouse = _warehouses.FirstOrDefault(w => w.Id == _selectedInventory.WarehouseId);
                    MessageBox.Show("Inventory updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                BindInventoriesGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving inventory: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            _selectedInventory = null;
            ClearInventoryDetails();
            dgvInventories.ClearSelection();
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedInventory == null)
            {
                MessageBox.Show("Please select an inventory record to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete this inventory record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _inventoryService.DeleteAsync(_selectedInventory.Id);
                    _inventories.Remove(_selectedInventory);
                    BindInventoriesGrid();
                    ClearInventoryDetails();
                    _selectedInventory = null;
                    MessageBox.Show("Inventory record deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting inventory: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Designer variables
        private DataGridView dgvInventories;
        private ComboBox cboProduct;
        private ComboBox cboWarehouse;
        private TextBox txtLocation;
        private TextBox txtQuantity;
        private TextBox txtMinQuantity;
        private TextBox txtMaxQuantity;
        private Button btnSave;
        private Button btnNew;
        private Button btnDelete;
    }
} 