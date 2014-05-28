using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        private int _fileCount;
        private Semaphore _semaphoreTask;
        private Thread _threadUpload;
        private bool _isRun;
        private object _syncLock = new object();
 
        private Action<FolderInfo, int> _delegateTurnToSucessful;
        private Action<FolderInfo, int> _delegateTurnToFail;

        private void UploadDirectoryTurnToSucessful(FolderInfo uploadFolderInfo, int itemIndex)
        {
            if (_listBoxUploadDirectory.InvokeRequired)
            {
                _listBoxUploadDirectory.Invoke(_delegateTurnToSucessful, uploadFolderInfo, itemIndex);                
            }
            else
            {
                _listBoxSucessfulDirectory.Items.Add(uploadFolderInfo);
                _listBoxUploadDirectory.Items.RemoveAt(itemIndex);
            }
        }

        private void UploadDirectoryTurnToFail(FolderInfo uploadFolderInfo, int itemIndex)
        {
            if (_listBoxUploadDirectory.InvokeRequired)
            {
                _listBoxUploadDirectory.Invoke(_delegateTurnToFail, uploadFolderInfo, itemIndex);
            }
            else
            {
                _listBoxFailDirectory.Items.Add(uploadFolderInfo);
                _listBoxUploadDirectory.Items.RemoveAt(itemIndex);
            }
        }

        public BLLUpload(IFileUpload proxy, FileServerInfo fileServerInfo, ListBox listBoxUploadDirectory, ListBox listBoxSucessfulDirectory, ListBox listBoxFailDirectory)
        {
            _proxy = proxy;
            _fileServerInfo = fileServerInfo;
            _listBoxUploadDirectory = listBoxUploadDirectory;
            _listBoxSucessfulDirectory = listBoxSucessfulDirectory;
            _listBoxFailDirectory = listBoxFailDirectory;
            _isRun = false;
            _semaphoreTask = new Semaphore(ClientConfig.MaxThread, ClientConfig.MaxThread);

            _delegateTurnToSucessful = UploadDirectoryTurnToSucessful;
            _delegateTurnToFail = UploadDirectoryTurnToFail;
        }

        public void Start()
        {
            try
            {
                _isRun = true;
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
                _isRun = false;

                if (_threadUpload != null && _threadUpload.IsAlive)
                {
                    _threadUpload.Abort();
                }
            }
            catch (ThreadAbortException ex)
            {
                MessageBox.Show(ex.Message);
                Tools.LogWrite(ex.ToString());
            }
        }

        public void UploadDirectoryCheck(ref FolderInfo folderInfo)
        {
            try
            {
                if (Directory.GetFiles(folderInfo.LocalPath).Count() == 0)
                {
                    folderInfo.CheckResult = "待上传目录为空";
                    return;
                }

                string checkFile = string.Empty;
                if (folderInfo.CategoryType == CategoryType.Gallery)
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

                    string infoContent = File.ReadAllText(Path.Combine(folderInfo.LocalPath, "info.txt"), Encoding.GetEncoding("gb2312")).Trim().Replace("\r", "").Replace("\n", "");
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

                string vectorPictureExtenName = ClientConfig.VectorPictureExtenName.ToLower();
                string pictureExtenName = ClientConfig.PictureExtenName.ToLower();

                int newFileCount = 0;
                foreach (string file in Directory.GetFiles(folderInfo.LocalPath))
                {
                    string fileExtenName = Path.GetExtension(file).ToLower();

                    if (folderInfo.IsExistVector)
                    {
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

                    if (!pictureExtenName.Contains(fileExtenName))
                    {
                        folderInfo.CheckResult = string.Format("{0}不是图片文件", file);
                        return;
                    }

                    if ((File.GetAttributes(file) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        continue;
                    }
                    newFileCount++;
                }// end foreach

                if (newFileCount == 0)
                {
                    folderInfo.CheckResult = "该目录没有未曾上传过的文件！";
                    return;
                }
            }
            catch (Exception ex)
            {
                folderInfo.CheckResult = "异常:" + ex.Message;
                return;
            }
        }

        private void UpLoadProcess()
        {
            try
            {
                while (_isRun && (DateTime.Now.Hour < ClientConfig.RestHourStart || DateTime.Now.Hour > ClientConfig.RestHourEnd))
                {
                    FolderInfo uploadFolderInfo = new FolderInfo();
                    int uploadDirectoryCount = 0;

                    lock (_listBoxUploadDirectory)
                    {
                        uploadDirectoryCount = _listBoxUploadDirectory.Items.Count - 1;
                    }

                    for (int index = uploadDirectoryCount; index >= 0; index--)
                    {
                        try
                        {
                            uploadFolderInfo = _listBoxUploadDirectory.Items[index] as FolderInfo;
                            uploadFolderInfo.IsRunning = true;
                            uploadFolderInfo.UploadResult.Clear();                           

                            //目录、相册创建                        
                            if (!RemoteDirectoryCreateProcess(ref uploadFolderInfo, index))
                            {
                                continue;
                            }

                            _fileCount = Directory.GetFiles(uploadFolderInfo.LocalPath).Count();

                            //文件上传
                            FileUploadProcess(uploadFolderInfo, index);  
                     
                            //这里要想办法阻塞
                            while(_fileCount > 0)
                            {
                                Thread.Sleep(5000);
                            }

                            //相册文件打包
                            if (!FileBundlingProcess(uploadFolderInfo, index))
                            {
                                continue;
                            }
                            
                            UploadDirectoryTurnProcess(uploadFolderInfo, index);
                        }
                        catch (ThreadInterruptedException)
                        {

                        }
                        catch (Exception ex)
                        {
                            //转移到失败目录
                            uploadFolderInfo.UploadResult.AppendLine(ex.Message);
                            UploadDirectoryTurnToFail(uploadFolderInfo, index);
                        }
                        finally
                        {
                            uploadFolderInfo.IsRunning = false;
                        }
                    }

                    Thread.Sleep(10000);
                } //  while (_isRun)
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Tools.LogWrite(ex.ToString());
            }
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
            request.ConverRelativeFilePath = "/Small" + uploadFolderInfo.LevelPath.Replace("|", "/") + "/" + localDirectory + "/Cover.jpg";
            request.BundlingRelativeFilePath = "/Big" + uploadFolderInfo.LevelPath.Replace("|", "/") + "/" + localDirectory + "/Package.zip";
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

        private bool RemoteDirectoryCreateProcess(ref FolderInfo uploadFolderInfo,int itemIndex)
        {
            CommonResponse response;
            if (uploadFolderInfo.CategoryType == CategoryType.Picture)
            {
                response = DirectoryCreate(uploadFolderInfo);
            }
            else
            {
                response = GalleryCreate(uploadFolderInfo);
                uploadFolderInfo.GalleryId = int.Parse(response.ResultMessage);
            }

            if (!response.IsSuccessful)
            {
                uploadFolderInfo.UploadResult.AppendLine(response.ResultMessage);
                UploadDirectoryTurnToFail(uploadFolderInfo, itemIndex);                
            }

            return response.IsSuccessful;
        }

        private void FileUpload(object sender)
        {
            _semaphoreTask.WaitOne();

            UploadInfo uploadInfo = null;
            FolderInfo uploadFolderInfo = null;
           
            try
            {
                uploadInfo = sender as UploadInfo;
                uploadFolderInfo = uploadInfo.FolderInfo;

                string[] paths = uploadFolderInfo.LocalPath.Split(new char[] { '\\' });
                string localDirectory = paths[paths.Length - 1];

                //文件上传时，如果是相册类型StoreName要转化
                CommonResponse response = new CommonResponse();
                using (FileStream fs = new FileStream(uploadInfo.FilePath, FileMode.Open, FileAccess.Read))
                {
                    FileUploadRequest request = new FileUploadRequest();
                    request.Token = ClientConfig.Token;
                    request.Account = ClientConfig.Account;
                    request.PassWord = ClientConfig.PassWord;
                    request.ThumbPictureWidth = ClientConfig.ThumbPictureWidth;
                    request.ThumbPictureHeight = ClientConfig.ThumbPictureHeight;
                    request.VectorPictureExtenName = ClientConfig.VectorPictureExtenName;
                    request.CategoryType = uploadFolderInfo.CategoryType;
                    request.CategoryId = uploadFolderInfo.CategoryId;
                    request.FileServerId = _fileServerInfo.Id;
                    request.OriginalFileServerRootDirectory = _fileServerInfo.OriginalFileServerRootDirectory;
                    request.ThumbFileServerRootDirectory = _fileServerInfo.ThumbFileServerRootDirectory;
                    request.CategoryAbsolutePath = uploadFolderInfo.LevelPath.Replace("|", "\\\\") + "\\\\" + localDirectory + "\\\\";
                    request.CategoryRelativePath = uploadFolderInfo.LevelPath.Replace("|", "/") + "/" + localDirectory + "/";
                    request.LevelCategoryName = uploadFolderInfo.LevelCategory;
                    if (uploadFolderInfo.CategoryType == CategoryType.Gallery)
                    {
                        request.StoreTableName = uploadFolderInfo.StoreTableName.Replace("Album", "Photo");
                    }
                    else
                    {
                        request.StoreTableName = uploadFolderInfo.StoreTableName;
                    }

                    request.FileName = Path.GetFileName(uploadInfo.FilePath);
                    request.FileData = fs;
                    response = _proxy.FileUpLoad(request);
                }

                if (response.IsSuccessful)
                {
                    //文件属性置为只读 
                    File.SetAttributes(uploadInfo.FilePath, FileAttributes.ReadOnly);
                }
                else
                {
                    string uploadResult = string.Format("{0},{1}", Path.GetFileName(uploadInfo.FilePath), response.ResultMessage);
                    uploadFolderInfo.UploadResult.AppendLine(uploadResult);
                }
            }
            catch(Exception ex)
            {
                string uploadResult = string.Format("{0},{1}", Path.GetFileName(uploadInfo.FilePath), ex.Message);
                uploadFolderInfo.UploadResult.AppendLine(uploadResult);
            }
            finally
            {
                lock (_syncLock)
                {
                    if (_fileCount > 0)
                    {
                        _fileCount--;
                    }
                }
                _semaphoreTask.Release();
            }
        }

        private void FileUploadProcess(FolderInfo uploadFolderInfo, int itemIndex)
        {
            foreach (string file in Directory.GetFiles(uploadFolderInfo.LocalPath))
            {
                try
                {
                    if ((File.GetAttributes(file) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        continue;
                    }

                    UploadInfo uploadInfo = new UploadInfo();
                    uploadInfo.FolderInfo = uploadFolderInfo;
                    uploadInfo.FilePath = file;

                    Task task = new Task(FileUpload, uploadInfo);
                    task.Start();
                }
                catch (Exception ex)
                {
                    string uploadResult = string.Format("{0},{1}", Path.GetFileName(file), ex.Message);
                    uploadFolderInfo.UploadResult.AppendLine(uploadResult);
                }
            }
        }

        private bool FileBundlingProcess(FolderInfo uploadFolderInfo, int itemIndex)
        {
            string[] paths = uploadFolderInfo.LocalPath.Split(new char[] { '\\' });
            string localDirectory = paths[paths.Length - 1];

            GalleryBundlingRequest request = new GalleryBundlingRequest();
            request.FileServerId = _fileServerInfo.Id;
            request.Token = ClientConfig.Token;
            request.Account = ClientConfig.Account;
            request.PassWord = ClientConfig.PassWord;
            request.OriginalAbsoluteFileDirectory = _fileServerInfo.OriginalFileServerRootDirectory + uploadFolderInfo.LevelPath.Replace("|", "\\\\") + "\\\\" + localDirectory;
            CommonResponse response = _proxy.FileBundling(request);        

            if (!response.IsSuccessful)
            {
                uploadFolderInfo.UploadResult.AppendLine(response.ResultMessage);
                UploadDirectoryTurnToFail(uploadFolderInfo, itemIndex);
            }

            return response.IsSuccessful;
        }

        private void UploadDirectoryTurnProcess(FolderInfo uploadFolderInfo, int itemIndex)
        {
            if (string.IsNullOrEmpty(uploadFolderInfo.UploadResult.ToString()))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(uploadFolderInfo.LocalPath);
                directoryInfo.Attributes = FileAttributes.ReadOnly;        

                UploadDirectoryTurnToSucessful(uploadFolderInfo, itemIndex);
            }
            else
            {
                UploadDirectoryTurnToFail(uploadFolderInfo, itemIndex);
            }
        }     
  
    }
}