using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Model;

namespace BLLClient
{
    public class BLLUpload
    {
        public string UploadDirectoryCheck(ref FolderInfo folderInfo)
        {
            try
            {
                string checkFile = string.Empty;
                if (folderInfo.Type == CategoryType.Gallery)
                {
                    checkFile = Path.Combine(folderInfo.Path, "cover.jpg");
                    if (!File.Exists(checkFile))
                    {
                        return string.Format("目录{0}缺少封面文件cover.jpg", folderInfo.Path);
                    }

                    checkFile = Path.Combine(folderInfo.Path, "info.txt");
                    if (!File.Exists(checkFile))
                    {
                        return string.Format("目录{0}缺少描述文件info.txt", folderInfo.Path);
                    }

                    if (folderInfo.IsExistVideo)
                    {
                        checkFile = Path.Combine(folderInfo.Path, "video.rar");
                        if (!File.Exists(checkFile))
                        {
                            return string.Format("目录{0}缺少视频文件video.rar", folderInfo.Path);
                        }
                    }
                }

                if (folderInfo.IsExistVector)
                {
                    foreach (string file in Directory.GetFiles(folderInfo.Path))
                    {
                        string fileExtenName = Path.GetExtension(file);
                        if (ClientConfig.VectorPictureExtenName.ToLower().Contains(fileExtenName))
                        {
                            string directory = Path.GetDirectoryName(file);
                            string thumbFile = Path.Combine(directory, ".jpg");
                            if (!File.Exists(thumbFile))
                            {
                                return string.Format("失量图文件{0}缺少缩略图文件{1}", file, thumbFile);
                            }
                        }
                    }
                }

                string infoContent = File.ReadAllText(Path.Combine(folderInfo.Path, "info.txt")).Trim();
                string[] columns = infoContent.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                switch (folderInfo.StoreTableName)
                {
                    case "Album_Brand":    //品牌画册                    
                    case "Album_Magazine": //服装杂志                  
                    case "Album_Book":     //款式书籍
                        {
                            if (columns.Count() != 3)
                            {
                                return string.Format("信息文件｛0}格式不正确！", Path.Combine(folderInfo.Path, "info.txt"));
                            }

                            folderInfo.GalleryName = columns[0];
                            folderInfo.PageCount = int.Parse(columns[1]);
                            folderInfo.Introudce = columns[2];
                            folderInfo.Designer = string.Empty;
                            folderInfo.Address = string.Empty;
                            break;
                        }
                    case "Album_Confer":     //发布会
                        {
                            if (columns.Count() != 4)
                            {
                                return string.Format("信息文件｛0}格式不正确！", Path.Combine(folderInfo.Path, "info.txt"));
                            }

                            folderInfo.GalleryName = columns[0];
                            folderInfo.Designer = columns[1];
                            folderInfo.Address = columns[2];
                            folderInfo.Introudce = columns[3];
                            folderInfo.PageCount = 0;
                            break;
                        }
                    default:
                        {
                            return string.Format("目录{0}，业务无法识别该表:{1}", folderInfo.Path, folderInfo.StoreTableName);
                        }
                }

                return string.Empty;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

         
        }
    }
}
