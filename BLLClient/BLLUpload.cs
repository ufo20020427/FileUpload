using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common;
using Model;

namespace BLLClient
{
    public class BLLUpload
    {
        private FileServerInfo _fileServerInfo;
        private ListBox _listBoxUploadDirectory;
        private ListBox _listBoxSucessfulDirectory;
        private ListBox _listBoxFailDirectory;     

        private Thread _threadUpload;
        private bool _isStart;

        public BLLUpload(FileServerInfo fileServerInfo, ListBox listBoxUploadDirectory, ListBox listBoxSucessfulDirectory, ListBox listBoxFailDirectory)
        {
            _fileServerInfo = fileServerInfo;
            _listBoxUploadDirectory = listBoxUploadDirectory;
            _listBoxSucessfulDirectory = listBoxSucessfulDirectory;
            _listBoxFailDirectory = listBoxFailDirectory;
            _isStart = false;
        }

        private void UpLoadProcess()
        {
            try
            {
                while (_isStart)
                {
                    foreach (var item in _listBoxUploadDirectory.Items)
                    {
                        try
                        {
                         //目录、相册创建
                        //文件上传
                        //文件属性置为只读
                        //目录文件打包
                        //目录属性置为只读
                        }
                        catch(Exception ex)
                        {
                            //转移到失败目录
                        }       
                    }
                }
            }
            catch (ThreadInterruptedException)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Tools.LogWrite(ex.ToString());
            }
        }

        public void Start()
        {
            try
            {
                _isStart = true;
                _threadUpload = new Thread(UpLoadProcess);
                _threadUpload.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Tools.LogWrite(ex.ToString());
            }
        }

        public void Stop()
        {
            try
            {
                _isStart = false;

                if (_threadUpload != null && _threadUpload.IsAlive)
                {
                    _threadUpload.Abort();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Tools.LogWrite(ex.ToString());
            }
        }

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
                        folderInfo.CheckResult = "缺少描述文件info.txt";
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

                    string infoContent = File.ReadAllText(Path.Combine(folderInfo.Path, "info.txt")).Trim().Replace("\r", "").Replace("\n", "");
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
                                folderInfo.CheckResult = string.Format("无法识别该表:{0}", folderInfo.StoreTableName);
                                return;
                            }
                    }
                }

                if (folderInfo.IsExistVector)
                {
                    string vectorPictureExtenName = ClientConfig.VectorPictureExtenName.ToLower();
                    foreach (string file in Directory.GetFiles(folderInfo.Path))
                    {
                        string fileExtenName = Path.GetExtension(file).ToLower();
                        if (vectorPictureExtenName.Contains(fileExtenName))
                        {
                            string directory = Path.GetDirectoryName(file);
                            string fileName = Path.GetFileNameWithoutExtension(file);
                            string thumbFile = Path.Combine(directory, "sm_" + fileName + ".jpg");
                            if (!File.Exists(thumbFile))
                            {
                                folderInfo.CheckResult = string.Format("失量图缺少缩略图文件{0}", thumbFile);
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                folderInfo.CheckResult = "异常:" + ex.Message;
                return;
            }


        }
    }
}