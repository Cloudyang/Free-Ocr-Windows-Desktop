﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using PdfiumViewer;

namespace a9t9Ocr
{
    class ImageConverter : IImageConverter
    {
        private string _oldDirectory = string.Empty;
        private int _count = 0;
        public void ClearTempImages()
        {
            try
            {
                if (Directory.Exists(_oldDirectory + _count))
                    Directory.Delete(_oldDirectory + _count, true);
               
            }
            catch
            {
                // ignored
            }
            finally
            {
                Directory.CreateDirectory(_oldDirectory + ++_count);
            }
        }
        public List<string> PathToConvertedImages(string pathToPdf)
        {
            var resultImages = new List<string>();
            const int desiredXDpi = 300;
            const int desiredYDpi = 300;

            string inputPdfPath = pathToPdf;
                var directoryInfo = new FileInfo(inputPdfPath).Directory;
                if (directoryInfo == null) return resultImages;

                string outputPath = directoryInfo.FullName + "\\tempimages\\";
                _oldDirectory = outputPath;
                ClearTempImages();

            //var lastInstalledVersion = GhostscriptVersionInfo.GetLastInstalledVersion(
            //    GhostscriptLicense.GPL | GhostscriptLicense.AFPL,
            //    GhostscriptLicense.GPL);

            //var rasterizer = new GhostscriptRasterizer();
            //rasterizer.Open(inputPdfPath, lastInstalledVersion, false);

            //for (int pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
            //{
            //    string pageFilePath = Path.Combine(outputPath + _count + @"\", @"Page-" + pageNumber + @".tiff");
            //    Image img = rasterizer.GetPage(desiredXDpi, desiredYDpi, pageNumber);
            //    img.Save(pageFilePath, ImageFormat.Tiff);
            //    resultImages.Add(pageFilePath);
            //}

            FileStream pdf = File.Open(pathToPdf, FileMode.Open);
            using (var document = PdfDocument.Load(pdf))
            {
                for (int i = 0; i < document.PageCount; i++)
                {
                    using (var img = document.Render(i, desiredXDpi, desiredYDpi, true))
                    {
                        string pageFilePath = Path.Combine(outputPath + _count + @"\", @"Page-" + i + @".jpg");
                        img.Save(pageFilePath, ImageFormat.Jpeg);
                        resultImages.Add(pageFilePath);
                    }
                }
            }


            return resultImages;
        }
    }
}
