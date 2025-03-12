using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.DAL.Repositories;
using WMS.Model;

namespace WMS.BLL.Services
{
    public class ProductService : IService<Product>
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;

        public ProductService(ProductRepository productRepository, CategoryRepository categoryRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                // 获取所有产品
                var products = await _productRepository.GetAllAsync();
                
                // 加载分类信息
                await LoadCategoriesForProducts(products);
                
                return products;
            }
            catch (Exception ex)
            {
                Logger.Error("获取所有产品失败", ex);
                throw;
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                // 获取产品
                var product = await _productRepository.GetByIdAsync(id);
                
                if (product != null)
                {
                    // 加载分类信息
                    product.Category = await _categoryRepository.GetByIdAsync(product.CategoryId);
                }
                
                return product;
            }
            catch (Exception ex)
            {
                Logger.Error($"获取产品(ID:{id})失败", ex);
                throw;
            }
        }

        public async Task<int> AddAsync(Product entity)
        {
            try
            {
                // 验证产品信息
                ValidateProduct(entity);
                
                // 检查产品代码是否已存在
                var existingProduct = await _productRepository.GetByCodeAsync(entity.Code);
                if (existingProduct != null)
                {
                    throw new Exception($"产品代码 '{entity.Code}' 已存在");
                }
                
                // 设置创建和更新时间
                entity.CreatedTime = DateTime.Now;
                entity.UpdatedTime = DateTime.Now;
                
                // 添加产品
                return await _productRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                Logger.Error("添加产品失败", ex);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Product entity)
        {
            try
            {
                // 验证产品信息
                ValidateProduct(entity);
                
                // 检查产品是否存在
                var existingProduct = await _productRepository.GetByIdAsync(entity.Id);
                if (existingProduct == null)
                {
                    throw new Exception($"产品(ID:{entity.Id})不存在");
                }
                
                // 检查产品代码是否已被其他产品使用
                var productWithSameCode = await _productRepository.GetByCodeAsync(entity.Code);
                if (productWithSameCode != null && productWithSameCode.Id != entity.Id)
                {
                    throw new Exception($"产品代码 '{entity.Code}' 已被其他产品使用");
                }
                
                // 设置更新时间
                entity.UpdatedTime = DateTime.Now;
                
                // 更新产品
                return await _productRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                Logger.Error($"更新产品(ID:{entity.Id})失败", ex);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                // 检查产品是否存在
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    throw new Exception($"产品(ID:{id})不存在");
                }
                
                // 检查产品是否可以删除（例如，是否有库存记录）
                var canDelete = await _productRepository.CanDeleteAsync(id);
                if (!canDelete)
                {
                    throw new Exception($"产品 '{product.Name}' 不能删除，因为它有关联的库存记录");
                }
                
                // 删除产品
                return await _productRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Logger.Error($"删除产品(ID:{id})失败", ex);
                throw;
            }
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchText)
        {
            try
            {
                // 搜索产品
                var products = await _productRepository.SearchAsync(searchText);
                
                // 加载分类信息
                await LoadCategoriesForProducts(products);
                
                return products;
            }
            catch (Exception ex)
            {
                Logger.Error($"搜索产品(条件:{searchText})失败", ex);
                throw;
            }
        }

        private async Task LoadCategoriesForProducts(IEnumerable<Product> products)
        {
            if (products == null || !products.Any())
                return;
            
            // 获取所有分类
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id);
            
            // 为每个产品设置分类信息
            foreach (var product in products)
            {
                if (categoryDict.TryGetValue(product.CategoryId, out var category))
                {
                    product.Category = category;
                }
            }
        }

        private void ValidateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            
            if (string.IsNullOrWhiteSpace(product.Code))
                throw new Exception("产品代码不能为空");
            
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new Exception("产品名称不能为空");
            
            if (product.CategoryId <= 0)
                throw new Exception("必须选择有效的产品分类");
            
            if (product.UnitPrice < 0)
                throw new Exception("产品单价不能为负数");
            
            if (product.MinStock < 0)
                throw new Exception("最小库存不能为负数");
            
            if (product.MaxStock < product.MinStock)
                throw new Exception("最大库存不能小于最小库存");
        }
    }
} 