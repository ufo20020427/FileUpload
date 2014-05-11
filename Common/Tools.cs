using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace Common
{
    public static class Tools
    {
        private static Object thisLock = new Object();
        private static readonly string logPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log";

        public static void LogWrite(string message)
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            string logFile = string.Format(@"{0}\{1}.log", logPath, DateTime.Now.ToString("yyyy-MM-dd"));

            lock (thisLock)
            {
                using (StreamWriter sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(string.Format("{0}{1}{2}{3}", DateTime.Now.ToString(), "\r\n", message, "\r\n"));
                    sw.Flush();
                    sw.Close();
                }
            }
        }


        public static void FileCompress(List<string> listFile, string savePath)
        {
            using (ZipFile zip = new ZipFile(System.Text.Encoding.Default))
            {
                zip.AddFiles(listFile, @"\");
                zip.Save(savePath);
            }
        }
 
        public static string MD5Encrypt(string content)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(content));
            string strs = Convert.ToBase64String(result);
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (byte hashByte in result)
            {
                builder.Append(hashByte.ToString("x2"));
            }
            return builder.ToString();
        }


        /// <summary>
        /// 图片缩略
        /// </summary>
        /// <param name="picturePathOld">原图片路径</param>
        /// <param name="picturePathNew">原图片路径</param>
        /// <param name="widthNew">新宽度</param>
        /// <param name="heightNew">新高度</param>
        /// <returns></returns>
        public static void PictureProcess(string picturePathOld, string picturePathNew, int widthNew, int heightNew)
        {
            using (Image sourceImage = Image.FromFile(picturePathOld))
            {
                int widthOld = sourceImage.Width;
                int heightOld = sourceImage.Height;

                //原图宽度与高度均超过新图
                if (widthOld > widthNew && heightOld > heightNew)
                {
                    //判断是宽更长还是高更长，以便以它做标准

                    if ((decimal)heightOld / (decimal)widthOld > (decimal)heightNew / (decimal)widthNew)
                    {
                        //如果是高更长，设置高为最大限制值，宽等比例缩小
                        widthNew = (int)((decimal)widthOld * ((decimal)heightNew / (decimal)heightOld));
                    }
                    else
                    {
                        //如果是宽更长，设置宽为最大限制值，高等比例缩小
                        heightNew = (int)((decimal)heightOld * ((decimal)widthNew / (decimal)widthOld));
                    }
                }
                else if (widthOld > widthNew && heightOld <= heightNew)  //只是宽度超过新图
                {
                    //以新宽度作为标准，原高等比缩小
                    heightNew = (int)((decimal)heightOld * ((decimal)widthNew / (decimal)widthOld));
                }
                else if (widthOld <= widthNew && heightOld > heightNew) //只是高度超过新图
                {
                    //以新高度作为标准，原宽等比缩小
                    widthNew = (int)((decimal)widthOld * ((decimal)heightNew / (decimal)heightOld));
                }
                else  //均没超过，直接复制
                {
                    File.Copy(picturePathOld, picturePathNew, true);
                }

                File.Delete(picturePathNew);

                using (Bitmap bitmap = new Bitmap(widthNew, heightNew))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                        //设置高质量,低速度呈现平滑程度
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        //清空画布并以透明背景色填充

                        g.Clear(Color.Transparent);

                        //在指定位置并且按指定大小绘制原图片的指定部分
                        g.DrawImage(sourceImage, new Rectangle(0, 0, widthNew, heightNew), new Rectangle(0, 0, widthOld, heightOld),
                            System.Drawing.GraphicsUnit.Pixel);

                        bitmap.Save(picturePathNew, ImageFormat.Jpeg);
                    }
                }
            }
        
        }
    }
}
