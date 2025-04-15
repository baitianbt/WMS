using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMS.BLL.Services;
using WMS.Model;
using Microsoft.Extensions.DependencyInjection;
using WMS.BLL;

namespace WMS.UI
{
    public partial class ProductForm : Form
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private List<Product> _products;
        private List<Category> _categories;
        private Product _currentProduct;
        private bool _isEditing = false;

        public ProductForm(ProductService productService, CategoryService categoryService)
        {
            InitializeComponent();
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            
            // 设置窗体属性
            this.Text = "产品管理";
            this.StartPosition = FormStartPosition.CenterParent;
            
            // 初始化控件事件
            InitializeEvents();
            
            // 加载数据
            LoadData();
        }

        private void InitializeEvents()
        {
            // 绑定按钮事件
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnSearch.Click += BtnSearch_Click;
            
            // 绑定数据网格事件
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;
            
            // 绑定文本框事件
            txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnSearch_Click(s, e); };
        }

        private async void LoadData()
        {
            try
            {
                // 显示加载中提示
                this.Cursor = Cursors.WaitCursor;
                
                // 加载分类数据
                _categories = (await _categoryService.GetAllAsync()).ToList();
                cboCategory.DataSource = null;
                cboCategory.DisplayMember = "Name";
                cboCategory.ValueMember = "Id";
                cboCategory.DataSource = _categories;
                
                // 加载产品数据
                await LoadProducts();
                
                // 设置控件状态
                SetControlState(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("加载产品数据失败", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async System.Threading.Tasks.Task LoadProducts(string searchText = null)
        {
            try
            {
                // 加载产品数据
                if (string.IsNullOrEmpty(searchText))
                {
                    _products = (await _productService.GetAllAsync()).ToList();
                }
                else
                {
                    _products = (await _productService.SearchAsync(searchText)).ToList();
                }
                
                // 绑定到数据网格
                dgvProducts.DataSource = null;
                dgvProducts.DataSource = _products;
                
                // 设置列标题和显示格式
                FormatDataGrid();
                
                // 如果有数据，选中第一行
                if (dgvProducts.Rows.Count > 0)
                {
                    dgvProducts.Rows[0].Selected = true;
                }
                else
                {
                    ClearProductDetails();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载产品数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("加载产品数据失败", ex);
            }
        }

        private void FormatDataGrid()
        {
            // 设置列标题和显示格式
            if (dgvProducts.Columns.Count > 0)
            {
                dgvProducts.Columns["Id"].HeaderText = "编号";
                dgvProducts.Columns["Id"].Width = 60;
                dgvProducts.Columns["Code"].HeaderText = "产品代码";
                dgvProducts.Columns["Code"].Width = 100;
                dgvProducts.Columns["Name"].HeaderText = "产品名称";
                dgvProducts.Columns["Name"].Width = 150;
                dgvProducts.Columns["Specification"].HeaderText = "规格";
                dgvProducts.Columns["Specification"].Width = 100;
                dgvProducts.Columns["CategoryId"].Visible = false;
                dgvProducts.Columns["Category"].HeaderText = "分类";
                dgvProducts.Columns["Category"].Width = 100;
                dgvProducts.Columns["UnitPrice"].HeaderText = "单价";
                dgvProducts.Columns["UnitPrice"].Width = 80;
                dgvProducts.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
                dgvProducts.Columns["Unit"].HeaderText = "单位";
                dgvProducts.Columns["Unit"].Width = 60;
                dgvProducts.Columns["MinStock"].HeaderText = "最小库存";
                dgvProducts.Columns["MinStock"].Width = 80;
                dgvProducts.Columns["MaxStock"].HeaderText = "最大库存";
                dgvProducts.Columns["MaxStock"].Width = 80;
                dgvProducts.Columns["Description"].HeaderText = "描述";
                dgvProducts.Columns["Description"].Width = 200;
                dgvProducts.Columns["IsActive"].HeaderText = "是否启用";
                dgvProducts.Columns["IsActive"].Width = 80;
                dgvProducts.Columns["CreatedTime"].HeaderText = "创建时间";
                dgvProducts.Columns["CreatedTime"].Width = 150;
                dgvProducts.Columns["UpdatedTime"].HeaderText = "更新时间";
                dgvProducts.Columns["UpdatedTime"].Width = 150;
            }
        }

        private void DgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0 && !_isEditing)
            {
                // 获取选中的产品
                _currentProduct = dgvProducts.SelectedRows[0].DataBoundItem as Product;
                
                // 显示产品详情
                DisplayProductDetails();
            }
        }

        private void DisplayProductDetails()
        {
            if (_currentProduct != null)
            {
                txtCode.Text = _currentProduct.Code;
                txtName.Text = _currentProduct.Name;
                txtSpecification.Text = _currentProduct.Specification;
                cboCategory.SelectedValue = _currentProduct.CategoryId;
                txtUnitPrice.Text = _currentProduct.UnitPrice.ToString("N2");
                txtUnit.Text = _currentProduct.Unit;
                txtMinStock.Text = _currentProduct.MinStock.ToString();
                txtMaxStock.Text = _currentProduct.MaxStock.ToString();
                txtDescription.Text = _currentProduct.Description;
                chkIsActive.Checked = _currentProduct.IsActive;
            }
        }

        private void ClearProductDetails()
        {
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtSpecification.Text = string.Empty;
            cboCategory.SelectedIndex = -1;
            txtUnitPrice.Text = "0.00";
            txtUnit.Text = string.Empty;
            txtMinStock.Text = "0";
            txtMaxStock.Text = "0";
            txtDescription.Text = string.Empty;
            chkIsActive.Checked = true;
        }

        private void SetControlState(bool isEditing)
        {
            _isEditing = isEditing;
            
            // 设置按钮状态
            btnAdd.Enabled = !isEditing;
            btnEdit.Enabled = !isEditing && dgvProducts.SelectedRows.Count > 0;
            btnDelete.Enabled = !isEditing && dgvProducts.SelectedRows.Count > 0;
            btnSave.Enabled = isEditing;
            btnCancel.Enabled = isEditing;
            
            // 设置数据网格状态
            dgvProducts.Enabled = !isEditing;
            
            // 设置文本框状态
            txtCode.ReadOnly = !isEditing;
            txtName.ReadOnly = !isEditing;
            txtSpecification.ReadOnly = !isEditing;
            cboCategory.Enabled = isEditing;
            txtUnitPrice.ReadOnly = !isEditing;
            txtUnit.ReadOnly = !isEditing;
            txtMinStock.ReadOnly = !isEditing;
            txtMaxStock.ReadOnly = !isEditing;
            txtDescription.ReadOnly = !isEditing;
            chkIsActive.Enabled = isEditing;
            
            // 设置搜索控件状态
            txtSearch.Enabled = !isEditing;
            btnSearch.Enabled = !isEditing;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // 清空产品详情
            ClearProductDetails();
            
            // 创建新产品对象
            _currentProduct = new Product
            {
                IsActive = true,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now
            };
            
            // 设置控件状态为编辑模式
            SetControlState(true);
            
            // 设置焦点
            txtCode.Focus();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (_currentProduct != null)
            {
                // 设置控件状态为编辑模式
                SetControlState(true);
                
                // 设置焦点
                txtCode.Focus();
            }
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_currentProduct != null)
            {
                // 确认删除
                DialogResult result = MessageBox.Show($"确定要删除产品 '{_currentProduct.Name}' 吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // 删除产品
                        await _productService.DeleteAsync(_currentProduct.Id);
                        
                        // 重新加载数据
                        await LoadProducts();
                        
                        // 提示成功
                        MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"删除失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Logger.Error("删除产品失败", ex);
                    }
                }
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (string.IsNullOrEmpty(txtCode.Text))
                {
                    MessageBox.Show("请输入产品代码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCode.Focus();
                    return;
                }
                
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("请输入产品名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }
                
                if (cboCategory.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择产品分类！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboCategory.Focus();
                    return;
                }
                
                // 更新产品对象
                _currentProduct.Code = txtCode.Text;
                _currentProduct.Name = txtName.Text;
                _currentProduct.Specification = txtSpecification.Text;
                _currentProduct.CategoryId = (int)cboCategory.SelectedValue;
                _currentProduct.UnitPrice = decimal.Parse(txtUnitPrice.Text);
                _currentProduct.Unit = txtUnit.Text;
                _currentProduct.MinStock = int.Parse(txtMinStock.Text);
                _currentProduct.MaxStock = int.Parse(txtMaxStock.Text);
                _currentProduct.Description = txtDescription.Text;
                _currentProduct.IsActive = chkIsActive.Checked;
                _currentProduct.UpdatedTime = DateTime.Now;
                
                // 保存产品
                if (_currentProduct.Id == 0)
                {
                    // 新增产品
                    await _productService.AddAsync(_currentProduct);
                }
                else
                {
                    // 更新产品
                    await _productService.UpdateAsync(_currentProduct);
                }
                
                // 重新加载数据
                await LoadProducts();
                
                // 设置控件状态
                SetControlState(false);
                
                // 提示成功
                MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error("保存产品失败", ex);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // 取消编辑
            if (_currentProduct != null && _currentProduct.Id > 0)
            {
                // 恢复显示
                DisplayProductDetails();
            }
            else
            {
                // 清空显示
                ClearProductDetails();
            }
            
            // 设置控件状态
            SetControlState(false);
        }

        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            // 搜索产品
            await LoadProducts(txtSearch.Text);
        }
    }
} 