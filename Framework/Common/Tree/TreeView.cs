using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Common.Tree
{
    public class TreeView: BaseTree
    {
        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public TreeView Parent { get; set; }

        /// <summary>
        /// 子级
        /// </summary>
        public IEnumerable<TreeView> Children { get; set; }
    }
}
