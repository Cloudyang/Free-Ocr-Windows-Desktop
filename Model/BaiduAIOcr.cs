using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Baidu.Aip.Ocr;
using Tesseract;

namespace a9t9Ocr
{
    class BaiduAIOcr : ITesseractOrc
    {
        // 设置APPID/AK/SK
        private string APP_ID = "15980837";
        private string API_KEY = "ouEQVpi1PUY6gSoMNT7VWTNT";
        private string SECRET_KEY = "YLUOOB7dHCB3EAvgXNVGl21D49mGSblt";

        private Ocr client;
        public string Language { get; set; }
        private readonly string _pathToTestData = @"tessdata"; // Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\(a9t9)OcrDesktop\tessdata";

        public BaiduAIOcr(string lang)
        {
            Language = lang;
            client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间
        }

        public void ChangeLanguage(string lang)
        {
            Language = lang;
        }
        public string RecognizeOneImage(ImageClass image)
        {
            if (image == null)
                return string.Empty;
            return BeginRecognize(image.FilePath);
        }

        public List<string> RecognizeFewImages(List<ImageClass> images)
        {
            return images.Select(imageClass => BeginRecognize(imageClass.FilePath)).ToList();
        }
        
        private string BeginRecognize(string pathToImage)
        {
            try
            {
                if (!File.Exists(pathToImage))
                    return "Image not found";
                var image = File.ReadAllBytes(pathToImage);
                // 如果有可选参数
                var options = new Dictionary<string, object>{
                    {"language_type", "CHN_ENG"},
                    {"detect_direction", "true"},
                    {"detect_language", "true"},
                    {"probability", "false"}
                };
                // 带参数调用通用文字识别, 图片参数为本地图片
                var result = client.GeneralBasic(image, options);
                if (result.Properties().Any(p=>p.Name=="error_code"))
                    return "无法正常识别!";
                var words= result["words_result"]
                        .Select(w => w.Value<string>("words"));
                return string.Join("\n",words);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                MessageBox.Show(e.StackTrace);
                return "识别错误!";
            }
        }
    }
}
