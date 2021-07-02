using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Common.Extension;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest.DataStructure
{
    public class StackTest
    {
        [Fact]
        public void BuildTree()
        {
            var datas = GetDataSource();

            var root = datas.Where(x => x.ParentId <= 0 && x.Level == 0).Map<List<TreeDto>>(); //第一级

            var stack = new Stack<TreeDto>(root); //栈解递归，首批顶级对象入栈

            while (stack.Any())
            {
                var item = stack.Pop();
                item.Children = datas.Where(it => it.ParentId == item.Id).Map<List<TreeDto>>();

                foreach (var child in item.Children)
                {
                    stack.Push(child);
                }
            }
        }

        private List<Tree> GetDataSource()
        {
            return new List<Tree> {
                new Tree { Id = 1, ParentId = 0, Name ="T1", Level =0},
                new Tree { Id = 2, ParentId = 0, Name ="T2", Level =0},
                new Tree { Id = 3, ParentId = 0, Name ="T3", Level =0},
                new Tree { Id = 4, ParentId = 1, Name ="T1-1", Level =1},
                new Tree { Id = 5, ParentId = 2, Name ="T2-1", Level =1},
                new Tree { Id = 6, ParentId = 2, Name ="T2-2", Level =1},
                new Tree { Id = 7, ParentId = 3, Name ="T3-1", Level =1},
                new Tree { Id = 8, ParentId = 4, Name ="T1-1-1", Level =2}
            };
        }
    }

    public class Tree
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
    }

    public class TreeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public List<TreeDto> Children { get; set; }
    }
}
