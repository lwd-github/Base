using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace XUnitTest.Excel
{
    public class ExcelTest
    {
        [Fact]
        public void ExcelToPdf()
        {
            //Nuget安装：Spire.XLS （for.NET）：生成的文档有版权信息
            //Nuget安装：FreeSpire.XLS：免费，有页数限制，最多生成10页
            //string excelToPDFPath = Path.Combine("C:\\Users\\future\\Desktop\\", "test.pdf");
            //Workbook workbook = new Workbook();
            //workbook.LoadFromFile("C:\\Users\\future\\Desktop\\test.xls", ExcelVersion.Version97to2003);
            //workbook.SaveToFile(excelToPDFPath, Spire.Xls.FileFormat.PDF);


            Workbook workbook = new Workbook();
            FileStream fileStream = new FileStream("C:\\Users\\future\\Desktop\\test.xls", FileMode.Open);
            workbook.LoadFromStream(fileStream, ExcelVersion.Version97to2003);
            MemoryStream outputStream = new MemoryStream();
            workbook.SaveToStream(outputStream, Spire.Xls.FileFormat.PDF);

            //保存文件
            FileStream fs = new FileStream("C:\\Users\\future\\Desktop\\test.pdf", FileMode.OpenOrCreate);
            BinaryWriter w = new BinaryWriter(fs);
            w.Write(outputStream.GetBuffer().ToArray());
            fs.Close();
        }
    }
}
