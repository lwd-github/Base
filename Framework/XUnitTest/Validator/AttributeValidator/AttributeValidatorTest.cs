using Xunit;
using Validator.AttributeValidator;
using Validator.AttributeValidator.Extension;
using System;

namespace XUnitTest.Validator.AttributeValidator
{
    public class AttributeValidatorTest
    {
        [Fact]
        public void Test1()
        {
            //**************** 验证 ****************
            Student student = new Student();
            student.Age = 2;
            student.Name = "afdadfadfadfadfadsa";
            student.Email = "a";
            Assert.True(student.Validate().Status);
        }
    }

    public class Student
    {
        const string text = "只能用常量，不能用变量";

        [System.ComponentModel.Description(text)]
        public int Id { get; private set; }

        [Require(ErrorMessage = "学生姓名为必填"), Length(10, ErrorMessage = "学生姓名的长度不能超过10个字符")]
        public string Name { get; set; }

        [Range(7, 100, ErrorMessage = "学生年龄必须在7~100的范围内")]
        public int Age { get; set; }

        [Require(ErrorMessage = "生日为必填")]
        public DateTime Birthday { get; set; }

        [Regex("^[a-z_0-9.-]{1,64}@([a-z0-9-]{1,200}.){1,5}[a-z]{1,6}$")]
        public string Email { get; set; }
    }
}
