using Framework.IdGenerator;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest.Pdf
{
    public class PdfTest
    {
        [Fact]
        public void FindImageInPDF()
        {
            //byte[] bytes = File.ReadAllBytes(@"C:\Users\future\Desktop\XK3190-DS9.pdf");
            //byte[] bytes = File.ReadAllBytes(@"C:\Users\future\Desktop\20211015导入单据（单图片）.pdf");
            byte[] bytes = File.ReadAllBytes(@"C:\Users\future\Desktop\20211015导入单据（多图片-8张）.pdf");
            string base64 = Convert.ToBase64String(bytes);

            RandomAccessFileOrArray randomAccessFileOrArray = new RandomAccessFileOrArray(bytes);
            using (PdfReader pdfReader = new PdfReader(randomAccessFileOrArray, null))
            {
                IdWorker idWorker = new IdWorker(1, 1);
                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    PdfDictionary pg = pdfReader.GetPageN(i);
                    // recursively search pages, forms and groups for images.
                    PdfObject pdfObject = FindImageInPDFDictionary(pg);

                    if (pdfObject != null)
                    {
                        int XrefIndex = Convert.ToInt32(((PRIndirectReference)pdfObject).Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                        PdfObject pdfObj = pdfReader.GetPdfObject(XrefIndex);
                        PdfStream pdfStrem = (PdfStream)pdfObj;
                        byte[] data = PdfReader.GetStreamBytesRaw((PRStream)pdfStrem);
                        if ((data != null))
                        {
                            using (MemoryStream memStream = new MemoryStream(data))
                            {
                                memStream.Position = 0;
                                Image img = Image.FromStream(memStream);
                                if (img.Height > img.Width)
                                {
                                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                }

                                using (var ms = new MemoryStream())
                                {
                                    img.Save(ms, ImageFormat.Jpeg);

                                    using (FileStream fs = new FileStream($"C:\\Users\\future\\Desktop\\20\\{idWorker.NextId()}.jpg", FileMode.OpenOrCreate))
                                    {
                                        BinaryWriter w = new BinaryWriter(fs);
                                        w.Write(ms.ToArray());
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        private static PdfObject FindImageInPDFDictionary(PdfDictionary pg)
        {
            PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));

            if(res != null)
            {
                PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
                if (xobj != null)
                {
                    foreach (PdfName name in xobj.Keys)
                    {
                        PdfObject obj = xobj.Get(name);
                        if (obj.IsIndirect())
                        {
                            PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
                            PdfName type = (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));

                            //image at the root of the pdf
                            if (PdfName.IMAGE.Equals(type))
                            {
                                return obj;
                            }// image inside a form
                            else if (PdfName.FORM.Equals(type))
                            {
                                return FindImageInPDFDictionary(tg);
                            } //image inside a group
                            else if (PdfName.GROUP.Equals(type))
                            {
                                return FindImageInPDFDictionary(tg);
                            }
                        }
                    }
                }
            }
            
            return null;
        }
    }
}
