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
    public class FileUploadProcess : BLLBase, IFileUpload
    {
        private Dictionary<int, DateTime> _dicAuth;

        public FileUploadProcess()
        {      
            _dicAuth = new Dictionary<int, DateTime>();
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

        public CommonResponse DirectoryCreate(DirectoryCreateRequest request)
        {
            CommonResponse response = new CommonResponse();
            response.IsSuccessful = false;
      
            try
            {              
                bool isPermission = Authentication(request.Token, request.FileServerId, request.Account, request.PassWord);
                if (!isPermission)
                {
                    response.IsSuccessful = false;
                    response.ResultMessage = "您没有访问该文件服务器接口的权限!";
                    return response;
                }

                Directory.CreateDirectory(request.OriginalAbsoluteFileDirectory);
                Directory.CreateDirectory(request.ThumbAbsoluteFileDirectory);

                response.IsSuccessful = true;
                response.ResultMessage = string.Empty;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ResultMessage = ex.ToString();
                Tools.LogWrite(ex.ToString());
            }

            return response;
        }

        private void GalleryCreateParamAdd(ref DbCommand cmd, GalleryCreateRequest request)
        {
            DbParameter param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@CategoryId";
            param.DbType = DbType.Int32;
            param.Value = request.CategoryId;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@FileServerId";
            param.DbType = DbType.Int32;
            param.Value = request.FileServerId;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@GalleryName";
            param.DbType = DbType.String;
            param.Value = request.GalleryName;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@ConverRelativeFilePath";
            param.DbType = DbType.String;
            param.Value = request.ConverRelativeFilePath;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@BundlingRelativeFilePath";
            param.DbType = DbType.String;
            param.Value = request.BundlingRelativeFilePath;
            cmd.Parameters.Add(param);      

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@PageCount";
            param.DbType = DbType.Int32;
            param.Value = request.PageCount;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@Introudce";
            param.DbType = DbType.String;
            param.Value = request.Introudce;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@VideoRelativeFilePath";
            param.DbType = DbType.String;
            param.Value = request.VideoRelativeFilePath;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@Designer";
            param.DbType = DbType.String;
            param.Value = request.Designer;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@Address";
            param.DbType = DbType.String;
            param.Value = request.Address;
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

        public CommonResponse GalleryCreate(GalleryCreateRequest request)
        {
            CommonResponse response = new CommonResponse();
            response.IsSuccessful = false;

            try
            {
                bool isPermission = Authentication(request.Token, request.FileServerId, request.Account, request.PassWord);
                if (!isPermission)
                {
                    response.IsSuccessful = false;
                    response.ResultMessage = "您没有访问该文件服务器接口的权限!";
                    return response;
                }

                Directory.CreateDirectory(request.OriginalAbsoluteFileDirectory);
                Directory.CreateDirectory(request.ThumbAbsoluteFileDirectory);

                DbCommand cmd = _dataBaseAccess.CreateCommand();
                GalleryCreateParamAdd(ref cmd, request);

                switch (request.StoreTableName)
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
                            response.IsSuccessful = false;
                            response.ResultMessage = string.Format("业务逻辑无法识别该表:{0}", request.StoreTableName);
                            return response;
                        }
                }

                _dataBaseAccess.ExecuteCommand(cmd);
                string galleryId = cmd.Parameters["@GalleryId"].Value.ToString();
                string result = cmd.Parameters["@Result"].Value.ToString();

                response.IsSuccessful = string.IsNullOrEmpty(result);
                response.ResultMessage = response.IsSuccessful ? galleryId : result;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ResultMessage = ex.ToString();
                Tools.LogWrite(ex.ToString());
            }

            return response;
        }

        private void FileInsertParamAdd(ref DbCommand cmd, FileUploadRequest request)
        {
            DbParameter param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@StoreTableName";
            param.DbType = DbType.String;
            param.Value = request.StoreTableName;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@CategoryId";
            param.DbType = DbType.Int32;
            param.Value = request.CategoryId;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@FileServerId";
            param.DbType = DbType.Int32;
            param.Value = request.FileServerId;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@ThumbRelativeFilePath";
            param.DbType = DbType.String;
            param.Value = "/Small" + request.CategoryRelativePath + "sm_" + request.FileName;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@PreViewRelativeFilePath";
            param.DbType = DbType.String;
            param.Value = "/Big" + request.CategoryRelativePath + "big_" + request.FileName;
            cmd.Parameters.Add(param);            

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@OriginalRelativeFilePath";
            param.DbType = DbType.String;
            param.Value = "/Big" + request.CategoryRelativePath + request.FileName;
            cmd.Parameters.Add(param);

            param = _dataBaseAccess.CreateParameter();
            param.ParameterName = "@IsVector";
            param.DbType = DbType.Boolean;
            param.Value = request.IsVector;
            cmd.Parameters.Add(param);  
        }


        public CommonResponse FileUpLoad(FileUploadRequest request)
        { 
            CommonResponse response = new CommonResponse();
            response.IsSuccessful = false;
            string originalFileSavePath = string.Empty;
            string thumbFileSavePath = string.Empty;
            bool isOriginalFileExists = true;

            try
            {
                bool isPermission = Authentication(request.Token, request.FileServerId, request.Account, request.PassWord);
                if (!isPermission)
                {
                    response.IsSuccessful = false;
                    response.ResultMessage = "您没有访问该文件服务器接口的权限!";
                    return response;
                }            

                string fileName = request.FileName.ToLower();
                string fileExtenName = Path.GetExtension(fileName).ToLower();

                originalFileSavePath = request.OriginalFileServerRootDirectory + request.CategoryAbsolutePath + fileName;

                isOriginalFileExists = File.Exists(originalFileSavePath);

                if (fileName == "cover.jpg" || fileName.Substring(0, 3) == "sm_") //封面或缩略图
                {
                    // 直接复制到缩略图路径       
                    thumbFileSavePath = request.ThumbFileServerRootDirectory + request.CategoryAbsolutePath + fileName;
                    using (FileStream outputStream = new FileStream(thumbFileSavePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        request.FileData.CopyTo(outputStream);
                        outputStream.Flush();
                        response.IsSuccessful = true;
                        response.ResultMessage = string.Empty;
                    }
                }
                else if (fileName == "video.rar" || fileName.Substring(0, 4) == "big_") //视频或预览图
                {
                    //直接保存到原始图即可
                    using (FileStream outputStream = new FileStream(originalFileSavePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        request.FileData.CopyTo(outputStream);
                        outputStream.Flush();
                        response.IsSuccessful = true;
                        response.ResultMessage = string.Empty;
                    }
                }
                else  //即要保存原始图，又要生成缩略图      
                {                               
                    using (FileStream outputStream = new FileStream(originalFileSavePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        request.FileData.CopyTo(outputStream);
                        outputStream.Flush();
                        response.IsSuccessful = true;
                        response.ResultMessage = string.Empty;
                    }
                  
                    //不是失量文件，则要生成缩略图
                    if (!request.VectorPictureExtenName.ToLower().Contains(fileExtenName))
                    {                       
                        thumbFileSavePath = request.ThumbFileServerRootDirectory + request.CategoryAbsolutePath + "sm_" + fileName;
                        Tools.PictureProcess(originalFileSavePath, thumbFileSavePath, request.ThumbPictureWidth, request.ThumbPictureHeight);
                    }

                    // if 文件之前不存在，则添加记录到数据库
                    if (!isOriginalFileExists)
                    {
                        if (request.CategoryType == CategoryType.Picture)
                        {
                            DbCommand cmd = _dataBaseAccess.CreateCommand();
                            cmd.CommandText = "Proc_PictureFiles_Insert";
                            FileInsertParamAdd(ref cmd, request);

                            DbParameter param = _dataBaseAccess.CreateParameter();
                            param.ParameterName = "@LevelCategoryName";
                            param.DbType = DbType.String;
                            param.Value = request.LevelCategoryName;
                            cmd.Parameters.Add(param);

                            _dataBaseAccess.ExecuteCommand(cmd);  
                        }
                        else if (request.CategoryType == CategoryType.Gallery)
                        {
                            DbCommand cmd = _dataBaseAccess.CreateCommand();
                            cmd.CommandText = "Proc_GalleryFiles_Insert";
                            FileInsertParamAdd(ref cmd, request);

                            DbParameter param = _dataBaseAccess.CreateParameter();
                            param.ParameterName = "@FileName";
                            param.DbType = DbType.String;
                            param.Value = request.FileName;
                            cmd.Parameters.Add(param);

                            _dataBaseAccess.ExecuteCommand(cmd);  
                        }
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

                response.IsSuccessful = false;
                response.ResultMessage = ex.ToString();
            }
            finally
            {
                request.FileData.Dispose();
            }
            return response;
        }

        public CommonResponse FileBundling(GalleryBundlingRequest request)
        {
            CommonResponse response = new CommonResponse();
            response.IsSuccessful = false;
            string packageSavePath = string.Empty;

            try
            {
                bool isPermission = Authentication(request.Token, request.FileServerId, request.Account, request.PassWord);
                if (!isPermission)
                {
                    response.IsSuccessful = false;
                    response.ResultMessage = "您没有访问该文件服务器接口的权限!";
                    return response;
                }

                if (!Directory.Exists(request.OriginalAbsoluteFileDirectory))
                {
                    response.IsSuccessful = false;
                    response.ResultMessage = string.Format("待打包目录:{0} 不存在!" , request.OriginalAbsoluteFileDirectory);
                    return response;
                }

                packageSavePath = Path.Combine(request.OriginalAbsoluteFileDirectory, "Package.zip");
                File.Delete(packageSavePath);

                string fileName = string.Empty;
                List<string> listFile = new List<string>();                
                foreach (string file in Directory.GetFiles(request.OriginalAbsoluteFileDirectory))
                {
                    fileName = Path.GetFileName(file).ToLower();
                    if (fileName != "video.rar")
                    {
                        listFile.Add(file);
                    }
                }

                if (listFile.Count > 0)
                {
                    Tools.FileCompress(listFile, packageSavePath);
                    response.IsSuccessful = true;
                    response.ResultMessage = string.Empty;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.ResultMessage = string.Format("待打包目录:{0} 为空目录!" ,request.OriginalAbsoluteFileDirectory);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ResultMessage = ex.ToString();
                Tools.LogWrite(ex.ToString());
            }

            return response;
        }

        public DataTableResponse GetCategoryInfo(string token, int fileServerId, string account, string passWord)
        {
            DataTableResponse response = new DataTableResponse();

            DataTable dt = new DataTable();         
            
            try
            {
                bool isPermission = Authentication(token, fileServerId, account, passWord);
                if (!isPermission)
                {
                    dt.TableName = "Category";
                    dt.Columns.Add("Error", typeof(string));

                    response.IsSuccessful = false;
                    response.ResultMessage = "您没有访问该文件服务器接口的权限!";
                    response.DataTable = dt;
                    return response;
                }

                DbCommand cmd = _dataBaseAccess.CreateCommand();
                cmd.CommandText = "Proc_Category_GetAll";
                dt = _dataBaseAccess.GetDataTable(cmd);
                dt.TableName = "Category";

                response.DataTable = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    response.IsSuccessful = true;
                    response.ResultMessage = string.Empty;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.ResultMessage = "目录信息表为空!";
                }                
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                dt.TableName = "Category";
                dt.Columns.Add("Error", typeof(string));

                response.IsSuccessful = false;
                response.ResultMessage = ex.Message;
                response.DataTable = dt;
                Tools.LogWrite(ex.ToString());
            }

            return response;
        }

        public DataTableResponse GetFileServerInfoByEndPoint(string token, string wcfAddress, string account, string passWord)
        {
            DataTableResponse response = new DataTableResponse();

            DataTable dt = new DataTable();

            try
            {
                bool isPermission = token == ServerConfig.Token;
                if (!isPermission)
                {
                    dt.TableName = "FileServer";
                    dt.Columns.Add("Error", typeof(string));

                    response.IsSuccessful = false;
                    response.ResultMessage = "您没有访问该文件服务器接口的权限!";
                    response.DataTable = dt;
                    return response;
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

                response.DataTable = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    response.IsSuccessful = true;
                    response.ResultMessage = string.Empty;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.ResultMessage = string.Format("{0}从服务器信息表获取不到内容！", wcfAddress);
                }              
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                dt.TableName = "FileServer";
                dt.Columns.Add("Error", typeof(string));

                response.IsSuccessful = false;
                response.ResultMessage = ex.Message;
                response.DataTable = dt;
                Tools.LogWrite(ex.ToString());
            }

            return response;
        }

        public int Add(int a, int b)
        {
            Thread.Sleep(5000);  
            return a + b;  
        }

        public IAsyncResult BeginAdd(int a, int b, AsyncCallback callBack, object state)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        
        public int EndAdd(IAsyncResult ar) 
        { 
            throw new Exception("The method or operation is not implemented.");
        } 
    }
}
