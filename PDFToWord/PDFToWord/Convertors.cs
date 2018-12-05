using System;
using System.Drawing;
using System.Drawing.Imaging;
using Spire.Pdf;
using Spire.Doc;
using Spire.Doc.Documents;
using System.Windows.Forms;

namespace PDFToWord
{
    class Convertors
    {
        #region PDF转换为Word\PNG\HTML等格式
        /// <summary>
        /// PDF转换为Word\PNG\HTML等格式
        /// </summary>
        /// <param name="pdfFile">原始PDF文件（包括路径）</param>
        /// <param name="otherFile">转换后的文件（包括路径）</param>
        /// <param name="format">转换的格式</param>
        public void PDFConversion(String pdfFile, String otherFile, string format)
        {
            PdfDocument document = new PdfDocument();
            document.LoadFromFile(pdfFile);
            switch (format)
            {
                case "WORD":
                    try
                    {
                        document.SaveToFile(otherFile, Spire.Pdf.FileFormat.DOC);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case "PNG":
                    for (int i = 0; i < document.Pages.Count; i++)
                    {
                        string fileName = String.Format("img-{0}.png", (i+1).ToString("000"));
                        using (Image image = document.SaveAsImage(i, Spire.Pdf.Graphics.PdfImageType.Metafile, 300, 300))
                        {
                            System.IO.DirectoryInfo dinfo = new System.IO.DirectoryInfo(otherFile.Substring(0, otherFile.LastIndexOf('.') + 1));
                            dinfo.Create();
                            image.Save(dinfo.FullName + "\\" + fileName, ImageFormat.Png);
                        }
                    }
                    break;
                case "HTML":
                    document.SaveToFile(otherFile, Spire.Pdf.FileFormat.HTML);
                    break;
            }
        }
        #endregion

        #region Word转换为PDF\PNG\RTF\HTML等格式
        /// <summary>
        /// Word转换为PDF\PNG\RTF\HTML等格式
        /// </summary>
        /// <param name="docFile">原始Word文件（包括路径）</param>
        /// <param name="otherFile">转换后的文件（包括路径）</param>
        /// <param name="format">转换的格式</param>
        public void WordConversion(String docFile, String otherFile, string format)
        {
            Document document = new Document(docFile, Spire.Doc.FileFormat.Auto);
            switch (format)
            {
                case "PDF":
                    document.SaveToFile(otherFile, Spire.Doc.FileFormat.PDF);
                    break;
                case "PNG":
                    Image[] Images = document.SaveToImages(ImageType.Bitmap);
                    if (Images != null && Images.Length > 0)
                    {
                        if (Images.Length == 1)
                        {
                            Images[0].Save(otherFile, ImageFormat.Bmp);
                        }
                        else
                        {
                            for (int i = 0; i<Images.Length; i++)
                            {
                                String fileName = String.Format("img-{0}.png", (i+1).ToString("000"));
                                System.IO.DirectoryInfo dinfo = new System.IO.DirectoryInfo(otherFile.Substring(0, otherFile.LastIndexOf('.') + 1));
                                dinfo.Create();
                                Images[i].Save(dinfo.FullName+"\\"+fileName, ImageFormat.Png);
                            }
                        }
                    }
                    break;
                case "RTF":
                    document.SaveToFile(otherFile, Spire.Doc.FileFormat.Rtf);
                    break;
                case "HTML":
                    document.SaveToFile(otherFile, Spire.Doc.FileFormat.Html);
                    break;
            }
        }
        #endregion
    }
}
