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
        private IFileUpload _proxy;
        private FileServerInfo _fileServerInfo;
        private ListBox _listBoxUploadDirectory;
        private ListBox _listBoxSucessfulDirectory;
        private ListBox _listBoxFailDirectory;

        private Thread _threadUpload;
        private bool _isStart;

        public BLLUpload(IFileUpload proxy, FileServerInfo fileServerInfo, ListBox listBoxUploadDirectory, ListBox listBoxSucessfulDirectory, ListBox listBoxFailDirectory)
        {
            _proxy = proxy;
            _fileServerInfo = fileServerInfo;
            _listBoxUploadDirectory = listBoxUploadDirectory;
            _listBoxSucessfulDirectory = listBoxSucessfulDirectory;
            _listBoxFailDirectory = listBoxFailDirectory;
            _isStart = false;
        }

        private CommonResponse DirectoryCreate(FolderInfo uploadFolderInfo)
        {
            string[] paths = uploadFolderInfo.LocalPath.Split(new char[] { '\\' });
            string localDirectory = paths[paths.Length - 1];

            DirectoryCreateRequest request = new DirectoryCreateRequest();
            request.FileServerId = _fileServerInfo.Id;
            request.Token = ClientConfig.Token;            
            request.Account = ClientConfig.Account;
            request.PassWord = ClientConfig.PassWord;
            request.OriginalAbsoluteFileDirectory = _fileServerInfo.OriginalFileServerRootDirectory + uploadFolderInfo.LevelPath.Replace("|", "\\\\") + "\\\\" + localDirectory;
            request.ThumbAbsoluteFileDirectory = _fileServerInfo.ThumbFileServerRootDirectory + uploadFolderInfo.LevelPath.Replace("|", "\\\\") + "\\\\" + localDirectory;
            CommonResponse response = _proxy.DirectoryCreate(request);
            return response;
        }

        private CommonResponse GalleryCreate(FolderInfo uploadFolderInfo)
        {
            string[] paths = uploadFolderInfo.LocalPath.Split(new char[] { '\\' });
            string localDirectory = paths[paths.Length - 1];

            GalleryCreateRequest request = new GalleryCreateRequest();
            request.Token = ClientConfig.Token;
            request.Account = ClientConfig.Account;
            request.PassWord = ClientConfig.PassWord;
            request.CategoryId = uploadFolderInfo.CategoryId;
            request.FileServerId = _fileServerInfo.Id;
            request.GalleryName = uploadFolderInfo.GalleryName;
            request.ConverRelativeFilePath = "/Small" + uploadFolderInfo.LevelPath.Replace("|", "/") + "/Cover.jpg";            
            request.BundlingRelativeFilePath = "/Big" + uploadFolderInfo.LevelPath.Replace("|", "/") + "/Package.zip";
            request.PageCount = uploadFolderInfo.PageCount;
            request.Introudce = uploadFolderInfo.Introudce;
            request.VideoRelativeFilePath = "/Big" + uploadFolderInfo.LevelPath.Replace("|", "/") + "/video.rar";
            request.Designer = uploadFolderInfo.Designer;
            request.Address = uploadFolderInfo.Address;
            request.OriginalAbsoluteFileDirectory = _fileServerInfo.OriginalFileServerRootDirectory + uploadFolderInfo.LevelPath.Replace("|", "\\\\") + "\\\\" + localDirectory;
            request.ThumbAbsoluteFileDirectory = _fileServerInfo.ThumbFileServerRootDirectory + uploadFolderInfo.LevelPath.Replace("|", "\\\\") + "\\\\" + localDirectory;
            request.StoreTableName = uploadFolderInfo.StoreTableName;

            CommonResponse response = _proxy.GalleryCreate(request);
            return response;
        }

        private bool DirectoryProcess(int itemIndex, FolderInfo uploadFolderInfo)
        {
            CommonResponse response;
            if (uploadFolderInfo.Type == CategoryType.Picture)
            {
                response = DirectoryCreate(uploadFolderInfo);
            }
            else
            {
                response = GalleryCreate(uploadFolderInfo);
            }

            if (!response.IsSuccessful)
            {
                uploadFolderInfo.UploadResult.AppendLine(response.ResultMessage);
                _listBoxFailDirectory.Items.Add(uploadFolderInfo);
                _listBoxUploadDirectory.Items.RemoveAt(itemIndex);
            }

            return response.IsSuccessful;
        }

        private void UpLoadProcess()
        {
            try
            {
                while (_isStart)
                {
                    FolderInfo uploadFolderInfo = new FolderInfo();

                    for (int index = 0; index < _listBoxUploadDirectory.Items.Count; index++)
                    {
                        try
                        {
                            uploadFolderInfo = _listBoxUploadDirectory.Items[index] as FolderInfo;
                            uploadFolderInfo.UploadResult = new StringBuilder();

                            //目录、相册创建                        
                           if (!DirectoryProcess(index, uploadFolderInfo))
                           {
                               continue;
                           }

                            //文件上传
                            //文件属性置为只读
                            //相册文件打包
                          

                            //目录属性置为只读

                            _listBoxSucessfulDirectory.Items.Add(uploadFolderInfo);
                            _listBoxUploadDirectory.Items.RemoveAt(index);
                        }
                        catch (ThreadInterruptedException)
                        {

                        }
                        catch (Exception ex)
                        {
                            //转移到失败目录
                            uploadFolderInfo.UploadResult.AppendLine(ex.Message);
                            _listBoxFailDirectory.Items.Add(uploadFolderInfo);
                            _listBoxUploadDirectory.Items.RemoveAt(index);
                        }
                    }

                    Thread.Sleep(5000);
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
                    checkFile = Path.Combine(folderInfo.LocalPath, "cover.jpg");
                    if (!File.Exists(checkFile))
                    {
                        folderInfo.CheckResult = "缺少封面文件cover.jpg";
                        return;
                    }

                    checkFile = Path.Combine(folderInfo.LocalPath, "info.txt");
                    if (!File.Exists(checkFile))
                    {
                        folderInfo.CheckResult = "缺少描述文件info.txt";
                        return;
                    }

                    if (folderInfo.IsExistVideo)
                    {
                        checkFile = Path.Combine(folderInfo.LocalPath, "video.rar");
                        if (!File.Exists(checkFile))
                        {
                            folderInfo.CheckResult = "缺少视频文件video.rar";
                            return;
                        }
                    }

                    string infoContent = File.ReadAllText(Path.Combine(folderInfo.LocalPath, "info.txt")).Trim().Replace("\r", "").Replace("\n", "");
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
                    foreach (string file in Directory.GetFiles(folderInfo.LocalPath))
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