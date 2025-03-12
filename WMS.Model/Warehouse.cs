using System;
using System.Collections.Generic;

namespace WMS.Model
{
    /// <summary>
    /// 仓库信息模型
    /// </summary>
    public class Warehouse
    {
        /// <summary>
        /// 仓库ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 仓库代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 仓库地址
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 仓库负责人
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 仓库容量
        /// </summary>
        public decimal Capacity { get; set; }

        /// <summary>
        /// 仓库描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
} 