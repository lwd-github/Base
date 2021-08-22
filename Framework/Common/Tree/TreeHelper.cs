using Framework.Common.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Common.Tree
{
    public static class TreeHelper
    {
        /// <summary>
        /// 创建树
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="trees"></param>
        /// <returns></returns>
        public static T2 BuildTree<T1, T2>(IEnumerable<T1> trees) where T1 : BaseTree where T2 : TreeView
        {
            var root = trees.FirstOrDefault(x => x.ParentId.IsNullOrWhiteSpace()).Map<T2>(); //根节点

            if (root.IsNotNull())
            {
                //栈解递归
                var stack = new Stack<T2>();
                root.Path = $"/{root.Id}/";
                root.Level = 1;
                stack.Push(root);

                while (stack.Any())
                {
                    var tree = stack.Pop();
                    tree.Children = trees.Where(it => it.ParentId == tree.Id).OrderBy(t => t.Sort).Map<List<T2>>();

                    foreach (var child in tree.Children)
                    {
                        child.Parent = tree;
                        child.Path = $"{tree.Path}{child.Id}/";
                        child.Level = tree.Level + 1;
                        stack.Push((T2)child);
                    }
                }
            }

            return root;
        }


        /// <summary>
        /// 查找树
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="id">查找的树Id</param>
        /// <returns></returns>
        public static T Find<T>(this T tree, string id) where T : TreeView
        {
            if (tree?.Id == id)
            {
                return tree;
            }

            if (tree?.Children?.Any() == true)
            {
                foreach (var child in tree.Children)
                {
                    var result = Find(child, id);

                    if (result != null)
                    {
                        return (T)result;
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// 新增树节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T Add<T>(this T tree, T node) where T : TreeView
        {
            var parent = tree.Find(node.ParentId);

            if (parent.Children.IsNull())
            {
                parent.Children = new List<TreeView>();
            }

            node.Level = parent.Level + 1;
            node.Path = $"{parent.Path}{node.Id}/";
            node.Parent = parent;
            node.Sort = node.Sort -1;
            var children = parent.Children.ToList();
            children.Insert(node.Sort, node);
            parent.Children = children;

            return node;
        }


        /// <summary>
        /// 更新树节点(只更新当前节点，不影响子节点)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T Update<T>(this T tree, T node) where T : TreeView
        {
            var result = tree.Find(node.Id);
            node.Parent = result.Parent;
            node.Children = result.Children;
            tree.Delete(result.Id);
            tree.Add(node);
            return node;
        }


        /// <summary>
        /// 删除树节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tree"></param>
        /// <param name="id"></param>
        public static void Delete<T>(this T tree, string id) where T : TreeView
        {
            var result = tree.Find(id);
            var children = result.Parent.Children.ToList();
            children.Remove(result);
            result.Parent.Children = children;
        }
    }
}
