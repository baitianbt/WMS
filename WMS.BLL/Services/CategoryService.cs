using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.DAL.Repositories;
using WMS.Model;

namespace WMS.BLL.Services
{
    public class CategoryService : IService<Category>
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryService(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            try
            {
                // 获取所有分类
                var categories = await _categoryRepository.GetAllAsync();
                
                // 构建分类树结构
                BuildCategoryTree(categories);
                
                return categories;
            }
            catch (Exception ex)
            {
                Logger.Error("获取所有分类失败", ex);
                throw;
            }
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            try
            {
                // 获取分类
                var category = await _categoryRepository.GetByIdAsync(id);
                
                if (category != null && category.ParentId.HasValue)
                {
                    // 加载父分类
                    category.Parent = await _categoryRepository.GetByIdAsync(category.ParentId.Value);
                }
                
                return category;
            }
            catch (Exception ex)
            {
                Logger.Error($"获取分类(ID:{id})失败", ex);
                throw;
            }
        }

        public async Task<int> AddAsync(Category entity)
        {
            try
            {
                // 验证分类信息
                ValidateCategory(entity);
                
                // 检查分类名称是否已存在
                var existingCategory = await _categoryRepository.GetByNameAsync(entity.Name);
                if (existingCategory != null)
                {
                    throw new Exception($"分类名称 '{entity.Name}' 已存在");
                }
                
                // 设置创建和更新时间
                entity.CreatedTime = DateTime.Now;
                entity.UpdatedTime = DateTime.Now;
                
                // 添加分类
                return await _categoryRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                Logger.Error("添加分类失败", ex);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Category entity)
        {
            try
            {
                // 验证分类信息
                ValidateCategory(entity);
                
                // 检查分类是否存在
                var existingCategory = await _categoryRepository.GetByIdAsync(entity.Id);
                if (existingCategory == null)
                {
                    throw new Exception($"分类(ID:{entity.Id})不存在");
                }
                
                // 检查分类名称是否已被其他分类使用
                var categoryWithSameName = await _categoryRepository.GetByNameAsync(entity.Name);
                if (categoryWithSameName != null && categoryWithSameName.Id != entity.Id)
                {
                    throw new Exception($"分类名称 '{entity.Name}' 已被其他分类使用");
                }
                
                // 检查是否形成循环引用
                if (entity.ParentId.HasValue && entity.ParentId.Value == entity.Id)
                {
                    throw new Exception("分类不能将自己设为父分类");
                }
                
                // 设置更新时间
                entity.UpdatedTime = DateTime.Now;
                
                // 更新分类
                return await _categoryRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                Logger.Error($"更新分类(ID:{entity.Id})失败", ex);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                // 检查分类是否存在
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    throw new Exception($"分类(ID:{id})不存在");
                }
                
                // 检查是否有子分类
                var hasChildren = await _categoryRepository.HasChildrenAsync(id);
                if (hasChildren)
                {
                    throw new Exception($"分类 '{category.Name}' 不能删除，因为它有子分类");
                }
                
                // 检查是否有关联的产品
                var hasProducts = await _categoryRepository.HasProductsAsync(id);
                if (hasProducts)
                {
                    throw new Exception($"分类 '{category.Name}' 不能删除，因为它有关联的产品");
                }
                
                // 删除分类
                return await _categoryRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Logger.Error($"删除分类(ID:{id})失败", ex);
                throw;
            }
        }

        private void BuildCategoryTree(IEnumerable<Category> categories)
        {
            if (categories == null || !categories.Any())
                return;
            
            var categoryDict = categories.ToDictionary(c => c.Id);
            
            foreach (var category in categories)
            {
                category.Children = new List<Category>();
            }
            
            foreach (var category in categories)
            {
                if (category.ParentId.HasValue && categoryDict.TryGetValue(category.ParentId.Value, out var parent))
                {
                    category.Parent = parent;
                    parent.Children.Add(category);
                }
            }
        }

        private void ValidateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new Exception("分类名称不能为空");
            
            if (category.ParentId.HasValue)
            {
                // 检查父分类是否存在
                var parent = _categoryRepository.GetByIdAsync(category.ParentId.Value).Result;
                if (parent == null)
                {
                    throw new Exception($"父分类(ID:{category.ParentId.Value})不存在");
                }
            }
        }
    }
} 