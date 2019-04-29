using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace a9t9Ocr
{
    class ImageLoader
    {
        public List<ImageClass> LoadImages(List<string> paths)
        {
            BitmapImage bitmap;
            var imagesList = new List<ImageClass>();

            var count = paths.Count();
            foreach (var name in paths)
            {
                try
                {
                    var uri = new Uri(name);
                    if (count == 1)
                    {
                        bitmap = new BitmapImage(uri).CloneCurrentValue();
                    }
                    else
                    {
                        bitmap = new BitmapImage(uri);
                    }
                    imagesList.Add(
                        new ImageClass
                        {
                            FilePath = name,
                            Image = bitmap
                        });
                }
                catch
                {
                    // ignored
                }
            }
            return imagesList;
        }
    }
}
