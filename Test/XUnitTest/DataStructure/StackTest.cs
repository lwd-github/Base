using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Common.Extension;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Text.RegularExpressions;
using Framework.Common.Tree;

namespace XUnitTest.DataStructure
{
    public class StackTest
    {
        /// <summary>
        /// 创建树
        /// </summary>
        [Fact]
        public MenuTreeView BuildTree()
        {
            var datas = GetDataSource();
            var result = TreeHelper.BuildTree<MenuTree, MenuTreeView>(datas);
            return result;
        }

        /// <summary>
        /// 查找树
        /// </summary>
        [Fact]
        public void FindTree()
        {
            var tree = BuildTree();
            var result = tree.Find("8");
            result = tree.Find("9");

            var results = FindTrees(tree, "8");
            results = FindTrees(tree, "9");
        }

        /// <summary>
        /// 新增树节点
        /// </summary>
        [Fact]
        public void AddTree()
        {
            var tree = BuildTree();
            var node = new MenuTreeView { Id = "9", ParentId = "8", Name = "T1-1-1-1", Url = "http:/9", Sort = 1 };
            tree.Add(node);
        }


        [Fact]
        public void UpdateTree()
        {
            var tree = BuildTree();
            var nodeAdd = new MenuTreeView { Id = "9", ParentId = "8", Name = "T1-1-1-1", Url = "http:/9", Sort = 1 };
            tree.Add(nodeAdd);

            var nodeUpdate = new MenuTreeView { Id = "8", ParentId = "4", Name = "T1-1-1update", Url = "http:/8/update", Sort = 1 };
            tree.Update(nodeUpdate);
        }


        /// <summary>
        /// 最长公共前缀
        /// </summary>
        [Fact]
        public void LongestCommonPrefix()
        {
            var strs = new List<string> { "flower", "flow", "flight" };
            var commonPrefix = GetLongestCommonPrefix(strs);

            strs = new List<string> { "dog", "racecar", "car" };
            commonPrefix = GetLongestCommonPrefix(strs);
        }


        /// <summary>
        /// 校验有效括号
        /// </summary>
        [Fact]
        public void SymbolPair()
        {
            var str = "\"我们\"是一年级（2）班的“优秀”学生[敬礼]";
            var result = IsSymbolPair(str);

            str = "([)]";
            result = IsSymbolPair(str);

            str = "{[]}";
            result = IsSymbolPair(str);
        }


        private List<MenuTree> GetDataSource()
        {
            return new List<MenuTree> {
                new MenuTree { Id = "0", ParentId = null, Name ="根节点", Url ="http:/0", Sort =0},
                new MenuTree { Id = "1", ParentId = "0", Name ="T1", Url ="http:/1", Sort =1},
                new MenuTree { Id = "2", ParentId = "0", Name ="T2", Url ="http:/2", Sort =2},
                new MenuTree { Id = "3", ParentId = "0", Name ="T3", Url ="http:/3", Sort =3},
                new MenuTree { Id = "4", ParentId = "1", Name ="T1-1", Url ="http:/4", Sort =1},
                new MenuTree { Id = "5", ParentId = "2", Name ="T2-1", Url ="http:/5", Sort =1},
                new MenuTree { Id = "6", ParentId = "2", Name ="T2-2", Url ="http:/6", Sort =2},
                new MenuTree { Id = "7", ParentId = "3", Name ="T3-1", Url ="http:/7", Sort =1},
                new MenuTree { Id = "8", ParentId = "4", Name ="T1-1-1", Url ="http:/8", Sort =1}
            };
        }


        /// <summary>
        /// 查找树(返回结果包含父级节点)
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<MenuTreeView> FindTrees(MenuTreeView tree, string id)
        {
            List<MenuTreeView> trees = new List<MenuTreeView>();
            var result = TreeHelper.Find(tree, id);

            if (result != null)
            {
                trees.Insert(0, result);
                var parent = result.Parent;

                while (parent != null)
                {
                    trees.Insert(0, (MenuTreeView)parent);
                    parent = parent.Parent;
                }
            }

            return trees;
        }


        private string GetLongestCommonPrefix(List<string> input)
        {
            var commonPrefix = input[0];

            for (int i = 1; i < input.Count; i++)
            {
                var except = commonPrefix.Intersect(input[i]);
                commonPrefix = string.Concat(except);

                if (commonPrefix.IsNullOrWhiteSpace()) break;
            }

            return commonPrefix;
        }


        /// <summary>
        /// 校验有效括号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool IsSymbolPair(string str)
        {
            str = Regex.Replace(str, "[^]‘’“”()（）[【】{}]", string.Empty);

            if (str == null || !str.Any() || str.Length % 2 != 0)
            {
                return false;
            }

            var stack = new Stack<char>();

            for (int i = 0; i < str.Length; i++)
            {
                if (symbolPair.ContainsKey(str[i]))
                {
                    if (!stack.Any() || stack.Pop() != symbolPair[str[i]])
                    {
                        return false;
                    }
                }
                else
                {
                    stack.Push(str[i]);
                }
            }

            return !stack.Any();
        }


        Dictionary<char, char> symbolPair = new Dictionary<char, char>
        {
            //{ '\'', '\''},
            { '’', '‘'},
            //{ '"', '"'},
            { '”', '“'},
            { ')', '('},
            { '）', '（'},
            { ']', '['},
            { '】', '【'},
            { '}', '{'}
        };
    }

    public class MenuTree : BaseTree
    {
        public string Url { get; set; }
    }

    public class MenuTreeView : TreeView
    {
        public string Url { get; set; }
    }
}
