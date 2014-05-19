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
        public void UploadDirectoryCheck(ref FolderInfo folderInfo)
        {
            try
            {
                string checkFile = string.Empty;
                if (folderInfo.Type == CategoryType.Gallery)
                {
                    checkFile = Path.Combine(folderInfo.Path, "cover.jpg");
                    if (!File.Exists(checkFile))
                    {
                        folderInfo.CheckResult = "缺少封面文件cover.jpg";
                        return;
                    }

                    checkFile = Path.Combine(folderInfo.Path, "info.txt");
                    if (!File.Exists(checkFile))
                    {
                        folderInfo.CheckResult = "缺少封面文件描述文件info.txt";
                        return;                      
                    }

                    if (folderInfo.IsExistVideo)
                    {
                        checkFile = Path.Combine(folderInfo.Path, "video.rar");
                        if (!File.Exists(checkFile))
                        {
                            folderInfo.CheckResult = "缺少视频文件video.rar";
                            return;        
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
                                    folderInfo.CheckResult = "信息文件info.txt格式不正确";
                                    return;
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
                                    folderInfo.CheckResult = "信息文件info.txt格式不正确";
                                    return;
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
                                folderInfo.CheckResult = string.Format("业务无法识别该表:{0}", folderInfo.StoreTableName);
                                return;
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
                                folderInfo.CheckResult = string.Format("失量图文件{0}缺少缩略图文件{1}", file, thumbFile);
                                return;
                            }
                        }
                    }
                }             
            }
            catch(Exception ex)
            {
                folderInfo.CheckResult = "异常:"+ex.Message;
                return;
            }

         
        }
    }
}
