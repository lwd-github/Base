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
        /// 获取树的所有分支
        /// </summary>
        [Fact]
        public void GetBranches()
        {
            var datas = GetDataSource();

            var result = TreeHelper.GetBranches<MenuTree, TreeBranch<MenuTree>>(datas);
        }


        /// <summary>
        /// 根据节点名称查询树分支
        /// </summary>
        [Fact]
        public void GetBranchesByNodeName()
        {
            var datas = GetDataSource();
            var result = TreeHelper.GetBranches<MenuTree, TreeBranch<MenuTree>>(datas, "2");
        }


        /// <summary>
        /// 反转整数
        /// </summary>
        [Fact]
        public void ReverseIntegerTest()
        {
            int result = ReverseInteger(1213);
            result = ReverseInteger(-1213);
        }


        /// <summary>
        /// 是否回文数
        /// </summary>
        [Fact]
        public void IsPalindromeTest()
        {
            var result = IsPalindrome1(-1221);
            result = IsPalindrome1(1);
            result = IsPalindrome1(0);
            result = IsPalindrome1(121);
            result = IsPalindrome1(120);
            result = IsPalindrome1(1221);

            result = IsPalindrome2(-1221);
            result = IsPalindrome2(1);
            result = IsPalindrome2(0);
            result = IsPalindrome2(121);
            result = IsPalindrome2(120);
            result = IsPalindrome2(1221);
        }


        /// <summary>
        /// 罗马数字转整数
        /// </summary>
        [Fact]
        public void RomanToIntTest()
        {
            var result = RomanToInt("XXVII");
            result = RomanToInt("IV");
            result = RomanToInt("IX");
            result = RomanToInt("XCIX");
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


        /// <summary>
        /// 合并有序链表
        /// </summary>
        [Fact]
        public void MergeOrderedLinkedListTest()
        {
            var l1 = new ListNode { Val = 1, Next = new ListNode { Val = 3, Next = new ListNode { Val = 5 } } };
            var l2 = new ListNode { Val = 2, Next = new ListNode { Val = 4, Next = new ListNode { Val = 6 } } };
            var r1 = MergeOrderedLinkedList1(l1, l2);

            l1 = new ListNode { Val = 1, Next = new ListNode { Val = 3, Next = new ListNode { Val = 5 } } };
            l2 = new ListNode { Val = 2, Next = new ListNode { Val = 4, Next = new ListNode { Val = 6 } } };
            var r2 = MergeOrderedLinkedList2(l1, l2);
        }


        /// <summary>
        /// 删除排序数组中的重复项
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void RemoveDuplicatesTest()
        {
            var lst = new[] { 0, 1 };
            var r = RemoveDuplicates(lst);
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


        /// <summary>
        /// 反转整数
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public int ReverseInteger(int val)
        {
            int result = 0;
            var x = val;

            while (x != 0)
            {
                result = result * 10 + x % 10;
                x /= 10;
            }

            return result;
        }


        /// <summary>
        /// 是否回文数
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool IsPalindrome1(int val)
        {
            if (val < 0 || (val % 10 == 0 && val != 0)) return false;

            var reverse = val.ToString().Reverse();
            return string.Concat(reverse) == val.ToString();
        }


        /// <summary>
        /// 是否回文数
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool IsPalindrome2(int val)
        {
            // 特殊情况：
            // 如上所述，当 x < 0 时，x 不是回文数。
            // 同样地，如果数字的最后一位是 0，为了使该数字为回文，
            // 则其第一位数字也应该是 0
            // 只有 0 满足这一属性
            if (val < 0 || (val % 10 == 0 && val != 0)) return false;

            int revertedNumber = 0;

            while (val > revertedNumber)
            {
                revertedNumber = revertedNumber * 10 + val % 10;
                val /= 10;
            }

            return val == revertedNumber || val == revertedNumber / 10;
        }


        /// <summary>
        /// 罗马数字转整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int RomanToInt(string str)
        {
            Dictionary<char, int> symbolValues = new Dictionary<char, int>
            {
                {'I', 1},
                {'V', 5},
                {'X', 10},
                {'L', 50},
                {'C', 100},
                {'D', 500},
                {'M', 1000}
            };

            int result = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (i < str.Length - 1 && symbolValues[str[i]] < symbolValues[str[i + 1]])
                {
                    result -= symbolValues[str[i]];
                }
                else
                {
                    result += symbolValues[str[i]];
                }
            }

            return result;
        }


        /// <summary>
        /// 获取最长公共前缀
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetLongestCommonPrefix(List<string> input)
        {
            //var commonPrefix = input[0];

            //for (int i = 1; i < input.Count; i++)
            //{
            //    var except = commonPrefix.Intersect(input[i]);
            //    commonPrefix = string.Concat(except);

            //    if (commonPrefix.IsNullOrWhiteSpace()) break;
            //}

            //return commonPrefix;

            //获取长度最小
            var min = input.Aggregate((a, b) => a.Length <= b.Length ? a : b);
            var minLength = input.Min(t => t.Length);
            string result = string.Empty;

            for (int i = 0; i < minLength; i++)
            {
                int j = 0;
                for (; j < input.Count - 1; j++)
                {
                    if (input[j][i] != input[j + 1][i]) return result;
                }

                result += input[0][i];
            }

            return result;
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


        /// <summary>
        /// 合并有序链表（递归）
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public ListNode MergeOrderedLinkedList1(ListNode l1, ListNode l2)
        {
            if (l1 == null)
            {
                return l2;
            }

            if (l2 == null)
            {
                return l1;
            }

            if (l1.Val < l2.Val)
            {
                l1.Next = MergeOrderedLinkedList1(l1.Next, l2);
                return l1;
            }
            else
            {
                l2.Next = MergeOrderedLinkedList1(l1, l2.Next);
                return l2;
            }
        }


        /// <summary>
        /// 合并有序链表（迭代）
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public ListNode MergeOrderedLinkedList2(ListNode l1, ListNode l2)
        {
            ListNode prehead = new ListNode(-1);
            ListNode prev = prehead;

            while (l1 != null && l2 != null)
            {
                if (l1.Val <= l2.Val)
                {
                    prev.Next = l1;
                    l1 = l1.Next;
                }
                else
                {
                    prev.Next = l2;
                    l2 = l2.Next;
                }
                prev = prev.Next;
            }

            // 合并后 l1 和 l2 最多只有一个还未被合并完，我们直接将链表末尾指向未合并完的链表即可
            prev.Next = l1 == null ? l2 : l1;

            return prehead.Next;
        }


        /// <summary>
        /// 删除排序数组中的重复项
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int RemoveDuplicates(int[] nums)
        {
            int n = nums.Length;
            if (n == 0)
            {
                return 0;
            }
            int fast = 1, slow = 1;
            while (fast < n)
            {
                if (nums[fast] != nums[fast - 1])
                {
                    nums[slow] = nums[fast];
                    ++slow;
                }
                ++fast;
            }
            return slow;
        }
    }


    public class MenuTree : BaseTree
    {
        public string Url { get; set; }
    }

    public class MenuTreeView : TreeView
    {
        public string Url { get; set; }
    }


    /// <summary>
    /// 链表类
    /// </summary>
    public class ListNode
    {
        public ListNode()
        {
        }

        public ListNode(int val)
        {
            this.Val = val;
        }

        public int Val { get; set; }

        public ListNode Next { get; set; }
    }
}
