using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Common.Tree
{
    public class TreeBranch<T> where T : BaseTree
    {
        /// <summary>
        /// 树分支：1级>2级>3级>...
        /// </summary>
        public List<T> Branches { get; set; }
    }
}
