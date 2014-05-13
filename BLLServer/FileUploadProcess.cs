using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Model;
using DAL;
using System.Data.Common;
using System.Data;

namespace BLLServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class FileUploadProcess : BLLBase, IFileUpload, IDisposable
    {
        private Dictionary<int, DateTime> _dicAuth;

        public FileUploadProcess()
        {
            Console.WriteLine("文件处理程序启动");
            _dicAuth = new Dictionary<int, DateTime>();
        }

        public void Dispose()
        {
            Console.WriteLine("文件处理程序终止");
        }

        private bool Authentication(string token, int fileServerId, string account, string passWord)
        {
            bool isPermission = false;

            if (_dicAuth.ContainsKey(fileServerId) && DateTime.Now.Subtract(_dicAuth[fileServerId]).Hours < 24)
            {
                isPermission = true;                
                return isPermission;
            }            

            if(token != ServerConfig.Token)
            {
                isPermission = false;
                return isPermission;
            }

            DbCommand cmd = _dataBaseAccess.CreateCommand();
            cmd.CommandText = "Proc_FileServer_Authentication";

            DbParameter param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@FileServerId";
            param.DbType = DbType.Int32;
            param.Value = fileServerId;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@Account";
            param.DbType = DbType.String;
            param.Value = account;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@PassWord";
            param.DbType = DbType.String;
            param.Value = Tools.MD5Encrypt(passWord);
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@FileServerCount";
            param.DbType = DbType.Int32;      
            param.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(param);

            _dataBaseAccess.ExecuteCommand(cmd);

            int fileServerCount = Convert.ToInt32(cmd.Parameters["@FileServerCount"].Value);

            if (fileServerCount > 0)
            {
                _dicAuth[fileServerId] = DateTime.Now;
                isPermission = true;
            }

            return isPermission;
        }

        public CommonResponse DirectoryCreate(DirectoryCreateQuest requestMessage)
        {
            CommonResponse responseMessage = new CommonResponse();
            responseMessage.IsSuccessed = false;
      
            try
            {
                bool isPermission = Authentication(requestMessage.Token, requestMessage.FileServerId, requestMessage.Account, requestMessage.PassWord);
                if (!isPermission)
                {
                    responseMessage.IsSuccessed = false;
                    responseMessage.ResultMessage = "您没有访问该文件服务器接口的权限!";
                    return responseMessage;
                }

                Directory.CreateDirectory(requestMessage.OriginalAbsoluteFileDirectory);
                Directory.CreateDirectory(requestMessage.ThumbAbsoluteFileDirectory);

                responseMessage.IsSuccessed = true;
                responseMessage.ResultMessage = string.Empty;
            }
            catch (Exception ex)
            {
                responseMessage.IsSuccessed = false;
                responseMessage.ResultMessage = ex.ToString();
                Tools.LogWrite(ex.ToString());
            }

            return responseMessage;
        }

        private void GalleryCreateParamAdd(ref DbCommand cmd, GalleryCreateQuest requestMessage)
        {
            DbParameter param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@CategoryId";
            param.DbType = DbType.Int32;
            param.Value = requestMessage.CategoryId;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@FileServerId";
            param.DbType = DbType.Int32;
            param.Value = requestMessage.FileServerId;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@GalleryName";
            param.DbType = DbType.String;
            param.Value = requestMessage.GalleryName;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@ConverRelativeFilePath";
            param.DbType = DbType.String;
            param.Value = requestMessage.ConverRelativeFilePath;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@BundlingRelativeFilePath";
            param.DbType = DbType.String;
            param.Value = requestMessage.BundlingRelativeFilePath;
            cmd.Parameters.Add(param);      

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@PageCount";
            param.DbType = DbType.Int32;
            param.Value = requestMessage.PageCount;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@Introudce";
            param.DbType = DbType.String;
            param.Value = requestMessage.Introudce;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@VideoRelativeFilePath";
            param.DbType = DbType.String;
            param.Value = requestMessage.VideoRelativeFilePath;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@Designer";
            param.DbType = DbType.String;
            param.Value = requestMessage.Designer;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@Address";
            param.DbType = DbType.String;
            param.Value = requestMessage.Address;
            cmd.Parameters.Add(param); 

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@GalleryId";
            param.DbType = DbType.Int32;
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@Result";
            param.DbType = DbType.String;
            param.Size = 200;
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);
        }  

        public CommonResponse GalleryCreate(GalleryCreateQuest requestMessage)
        {
            CommonResponse responseMessage = new CommonResponse();
            responseMessage.IsSuccessed = false;

            try
            {
                bool isPermission = Authentication(requestMessage.Token, requestMessage.FileServerId, requestMessage.Account, requestMessage.PassWord);
                if (!isPermission)
                {
                    responseMessage.IsSuccessed = false;
                    responseMessage.ResultMessage = "您没有访问该文件服务器接口的权限!";
                    return responseMessage;
                }

                Directory.CreateDirectory(requestMessage.OriginalAbsoluteFileDirectory);
                Directory.CreateDirectory(requestMessage.ThumbAbsoluteFileDirectory);

                DbCommand cmd = _dataBaseAccess.CreateCommand();
                GalleryCreateParamAdd(ref cmd, requestMessage);

                switch (requestMessage.StoreTableName)
                {
                    case "Album_Brand":    //品牌画册
                        {
                            cmd.CommandText = "Proc_Album_Brand_InsertOrUpdate";
                            break;
                        }
                    case "Album_Magazine": //服装杂志
                        {
                            cmd.CommandText = "Proc_Album_Magazine_InsertOrUpdate";
                            break;
                        }
                    case "Album_Book":     //款式书籍
                        {
                            cmd.CommandText = "Proc_Album_Book_InsertOrUpdate";
                            break;
                        }
                    case "Album_Confer":     //发布会
                        {
                            cmd.CommandText = "Proc_Album_Confer_InsertOrUpdate";
                            break;
                        }
                    default:
                        {
                            responseMessage.IsSuccessed = false;
                            responseMessage.ResultMessage = string.Format("switch条件无法匹配数据表:{0}", requestMessage.StoreTableName);
                            return responseMessage;
                        }
                }

                _dataBaseAccess.ExecuteCommand(cmd);
                string galleryId = cmd.Parameters["@GalleryId"].Value.ToString();
                string result = cmd.Parameters["@Result"].Value.ToString();

                responseMessage.IsSuccessed = string.IsNullOrEmpty(result);
                responseMessage.ResultMessage = responseMessage.IsSuccessed ? galleryId : result;
            }
            catch (Exception ex)
            {
                responseMessage.IsSuccessed = false;
                responseMessage.ResultMessage = ex.ToString();
                Tools.LogWrite(ex.ToString());
            }

            return responseMessage;
        }

        private void FileInsertParamAdd(ref DbCommand cmd, FileUploadQuest requestMessage)
        {
            DbParameter param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@StoreTableName";
            param.DbType = DbType.String;
            param.Value = requestMessage.StoreTableName;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@CategoryId";
            param.DbType = DbType.Int32;
            param.Value = requestMessage.CategoryId;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@FileServerId";
            param.DbType = DbType.Int32;
            param.Value = requestMessage.FileServerId;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@ThumbRelativeFilePath";
            param.DbType = DbType.String;
            param.Value = "Small" + requestMessage.CategoryRelativePath + "sm_" + requestMessage.FileName;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@OriginalRelativeFilePath";
            param.DbType = DbType.String;
            param.Value = "Big" + requestMessage.CategoryRelativePath + requestMessage.FileName;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@CategoryName";
            param.DbType = DbType.String;
            param.Value = requestMessage.CategoryName;
            cmd.Parameters.Add(param);
        }

        public CommonResponse FileUpLoad(FileUploadQuest requestMessage)
        {
            CommonResponse responseMessage = new CommonResponse();
            responseMessage.IsSuccessed = false;
            string originalFileSavePath = string.Empty;
            string thumbFileSavePath = string.Empty;
            bool isOriginalFileExists = true;

            try
            {
                bool isPermission = Authentication(requestMessage.Token, requestMessage.FileServerId, requestMessage.Account, requestMessage.PassWord);
                if (!isPermission)
                {
                    responseMessage.IsSuccessed = false;
                    responseMessage.ResultMessage = "您没有访问该文件服务器接口的权限!";
                    return responseMessage;
                }

                string fileName = requestMessage.FileName.ToLower();
                string fileExtenName = Path.GetExtension(fileName).ToLower();

                originalFileSavePath = requestMessage.OriginalFileServerRootDirectory + requestMessage.CategoryAbsolutePath + fileName;

                isOriginalFileExists = File.Exists(originalFileSavePath);

                if (fileName.Substring(0, 3) == "sm_") //缩略图
                {
                    // 直接复制到缩略图路径       
                    thumbFileSavePath = requestMessage.ThumbFileServerRootDirectory + requestMessage.CategoryAbsolutePath + fileName;
                    using (FileStream outputStream = new FileStream(thumbFileSavePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        requestMessage.FileData.CopyTo(outputStream);
                        outputStream.Flush();
                        responseMessage.IsSuccessed = true;
                        responseMessage.ResultMessage = string.Empty;
                    }
                }
                else if (fileName == "video.rar" || fileName == "cover.jpg" )
                {
                    //直接保存到原始图即可
                    using (FileStream outputStream = new FileStream(originalFileSavePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        requestMessage.FileData.CopyTo(outputStream);
                        outputStream.Flush();
                        responseMessage.IsSuccessed = true;
                        responseMessage.ResultMessage = string.Empty;
                    }
                }
                else
                {
                    //即要保存原始图，又要生成并保存缩略图                  
                    using (FileStream outputStream = new FileStream(originalFileSavePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        requestMessage.FileData.CopyTo(outputStream);
                        outputStream.Flush();
                        responseMessage.IsSuccessed = true;
                        responseMessage.ResultMessage = string.Empty;
                    }

                    //不是失量文件，则要生成缩略图
                    if (!ServerConfig.VectorPictureExtenName.ToLower().Contains(fileExtenName))
                    {                       
                        thumbFileSavePath = requestMessage.ThumbFileServerRootDirectory + requestMessage.CategoryAbsolutePath + "sm_" + fileName;
                        Tools.PictureProcess(originalFileSavePath, thumbFileSavePath, ServerConfig.ThumbPictureWidth, ServerConfig.ThumbPictureHeight);
                    }

                    // if 文件之前不存在，则添加记录到数据库
                    if (!isOriginalFileExists)
                    {
                        DbCommand cmd = _dataBaseAccess.CreateCommand();
                        cmd.CommandText = "Proc_Files_Insert";
                        FileInsertParamAdd(ref cmd, requestMessage);
                        _dataBaseAccess.ExecuteCommand(cmd);                        
                    }
                }         
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                try
                {
                    if (!string.IsNullOrEmpty(originalFileSavePath))
                    {
                        File.Delete(originalFileSavePath);
                    }

                    if (!string.IsNullOrEmpty(thumbFileSavePath))
                    {
                        File.Delete(thumbFileSavePath);
                    }
                }
                catch(Exception exception)
                {
                    Tools.LogWrite(exception.ToString());
                }

                responseMessage.IsSuccessed = false;
                responseMessage.ResultMessage = ex.ToString();
            }
            finally
            {
                requestMessage.FileData.Dispose();
            }
            return responseMessage;
        }

        public CommonResponse FileBundling(GalleryBundlingQuest requestMessage)
        {
            CommonResponse responseMessage = new CommonResponse();
            responseMessage.IsSuccessed = false;
            string packageSavePath = string.Empty;

            try
            {
                bool isPermission = Authentication(requestMessage.Token, requestMessage.FileServerId, requestMessage.Account, requestMessage.PassWord);
                if (!isPermission)
                {
                    responseMessage.IsSuccessed = false;
                    responseMessage.ResultMessage = "您没有访问该文件服务器接口的权限!";
                    return responseMessage;
                }

                if (!Directory.Exists(requestMessage.OriginalAbsoluteFileDirectory))
                {
                    responseMessage.IsSuccessed = false;
                    responseMessage.ResultMessage = string.Format("待打包目录:{0} 不存在!" , requestMessage.OriginalAbsoluteFileDirectory);
                    return responseMessage;
                }

                packageSavePath = Path.Combine(requestMessage.OriginalAbsoluteFileDirectory, "Package.zip");
                File.Delete(packageSavePath);

                string fileName = string.Empty;
                List<string> listFile = new List<string>();                
                foreach (string file in Directory.GetFiles(requestMessage.OriginalAbsoluteFileDirectory))
                {
                    fileName = Path.GetFileName(file).ToLower();
                    if (fileName != "video.rar" && fileName != "cover.jpg")
                    {
                        listFile.Add(file);
                    }
                }

                if (listFile.Count > 0)
                {
                    Tools.FileCompress(listFile, packageSavePath);
                    responseMessage.IsSuccessed = true;
                    responseMessage.ResultMessage = string.Empty;
                }
                else
                {
                    responseMessage.IsSuccessed = false;
                    responseMessage.ResultMessage = string.Format("待打包目录:{0} 为空目录!" ,requestMessage.OriginalAbsoluteFileDirectory);
                }
            }
            catch (Exception ex)
            {
                responseMessage.IsSuccessed = false;
                responseMessage.ResultMessage = ex.ToString();
                Tools.LogWrite(ex.ToString());
            }

            return responseMessage;
        }

        public DataTableResponse GetCategoryInfo(string token, int fileServerId, string account, string passWord)
        {
            DataTableResponse responseMessage = new DataTableResponse();

            DataTable dt = new DataTable();         
            
            try
            {
                bool isPermission = Authentication(token, fileServerId, account, passWord);
                if (!isPermission)
                {
                    dt.TableName = "Category";
                    dt.Columns.Add("Error", typeof(string));

                    responseMessage.IsSuccessed = false;
                    responseMessage.ResultMessage = "您没有访问该文件服务器接口的权限!";
                    responseMessage.DataTable = dt;
                    return responseMessage;
                }

                DbCommand cmd = _dataBaseAccess.CreateCommand();
                cmd.CommandText = "Proc_Category_GetAll";
                dt = _dataBaseAccess.GetDataTable(cmd);
                dt.TableName = "Category";

                responseMessage.DataTable = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    responseMessage.IsSuccessed = true;
                    responseMessage.ResultMessage = string.Empty;
                }
                else
                {
                    responseMessage.IsSuccessed = false;
                    responseMessage.ResultMessage = "目录信息表为空!";
                }                
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                dt.TableName = "Category";
                dt.Columns.Add("Error", typeof(string));

                responseMessage.IsSuccessed = false;
                responseMessage.ResultMessage = ex.Message;
                responseMessage.DataTable = dt;
                Tools.LogWrite(ex.ToString());
            }

            return responseMessage;
        }


        public DataTableResponse GetFileServerInfoByEndPoint(string token, string wcfAddress, string account, string passWord)
        {
            DataTableResponse responseMessage = new DataTableResponse();

            DataTable dt = new DataTable();

            try
            {
                bool isPermission = token == ServerConfig.Token;
                if (!isPermission)
                {
                    dt.TableName = "FileServer";
                    dt.Columns.Add("Error", typeof(string));

                    responseMessage.IsSuccessed = false;
                    responseMessage.ResultMessage = "您没有访问该文件服务器接口的权限!";
                    responseMessage.DataTable = dt;
                    return responseMessage;
                }

                DbCommand cmd = _dataBaseAccess.CreateCommand();
                cmd.CommandText = "Proc_FileServer_GetByEndPoint";

                DbParameter param = _dataBaseAccess.CreateParameter();
                param.ParameterName = "@WCFAddress";
                param.DbType = DbType.String;
                param.Value = wcfAddress;
                cmd.Parameters.Add(param);

                param = _dataBaseAccess.CreateParameter();
                param.ParameterName = "@Account";
                param.DbType = DbType.String;
                param.Value = account;
                cmd.Parameters.Add(param);

                param = _dataBaseAccess.CreateParameter();
                param.ParameterName = "@PassWord";
                param.DbType = DbType.String;
                param.Value = Tools.MD5Encrypt(passWord);
                cmd.Parameters.Add(param);

                dt = _dataBaseAccess.GetDataTable(cmd);
                dt.TableName = "FileServer";

                responseMessage.DataTable = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    responseMessage.IsSuccessed = true;
                    responseMessage.ResultMessage = string.Empty;
                }
                else
                {
                    responseMessage.IsSuccessed = false;
                    responseMessage.ResultMessage = string.Format("{0}从服务器信息表获取不到内容！", wcfAddress);
                }              
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                dt.TableName = "FileServer";
                dt.Columns.Add("Error", typeof(string));

                responseMessage.IsSuccessed = false;
                responseMessage.ResultMessage = ex.Message;
                responseMessage.DataTable = dt;
                Tools.LogWrite(ex.ToString());
            }

            return responseMessage;
        }
    
    }
}
