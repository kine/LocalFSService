using Newtonsoft.Json;
using Navertica.Services.NVRLocalFSService.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Navertica.Services.NVRLocalFSService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LocalFSService" in both code and config file together.
    [ServiceBehavior]
    public class LocalFSService : ILocalFSServiceContract
    {

        public bool CheckIfFolderExists(string Path)
        {
            CheckAllowedPath(Path);
            try
            {
                Path = RootPath(Path);

                var result = Directory.Exists(Path);

                return result;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return false;
        }

        private void CheckAllowedPath(string path)
        {
            var defaultPath = Settings.Default.RootPath;
            if (Path.IsPathRooted(path))
            {
                if (!path.ToUpper().Contains(defaultPath.ToUpper().TrimEnd('\\') + "\\"))
                {
                    var exception = new ArgumentException(String.Format(@"Path {0} is not subpath of {1}", path, defaultPath));
                    throw new WebFaultException<ArgumentException>(exception, System.Net.HttpStatusCode.BadRequest);
                }
            }
        }
        private string RootPath(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                var defaultPath = Settings.Default.RootPath;
                return Path.Combine(defaultPath, path);
            }
            return path;
        }

        public void CreateFolder(string ParentFolderName, string FolderName)
        {
            CheckAllowedPath(ParentFolderName);
            CheckAllowedPath(FolderName);
            try
            {
                ParentFolderName = RootPath(ParentFolderName);
                var NewPath = Path.Combine(ParentFolderName, FolderName);
                Directory.CreateDirectory(NewPath);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }


        }

        public void DeleteFile(string Path)
        {
            CheckAllowedPath(Path);
            try
            {
                Path = RootPath(Path);
                File.Delete(Path);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

        }

        public Stream DownloadFile(string Path)
        {
            try
            {
                CheckAllowedPath(Path);
                Path = RootPath(Path);
                var resultStream = new MemoryStream();
                var fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
                fileStream.CopyTo(resultStream);
                fileStream.Close();
                resultStream.Seek(0, SeekOrigin.Begin);
                return resultStream;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return null;
        }

        public MyFileInfo[] GetListOfFiles(string Path, string Pattern="")
        {
            CheckAllowedPath(Path);
            try
            {
                if (Pattern == null)
                {
                    Pattern = "*.*";
                }
                Path = RootPath(Path);
                var files = Directory.GetFiles(Path,Pattern);
                var directories = Directory.GetDirectories(Path);
                var result = new List<MyFileInfo>();
                foreach (var f in files)
                {
                    result.Add(new MyFileInfo() { Name = f, IsDirectory = false });
                }
                foreach (var d in directories)
                {
                    result.Add(new MyFileInfo(){ Name = d, IsDirectory = true});
                }
                return result.ToArray();
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return null ;
        }

        public void MoveFile(string Path, string NewPath)
        {
            CheckAllowedPath(Path);
            CheckAllowedPath(NewPath);
            try
            {
                Path = RootPath(Path);
                NewPath = RootPath(NewPath);
                File.Move(Path, NewPath);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
        }

        public void UploadFile(string Path, Stream Content)
        {
            CheckAllowedPath(Path);
            try
            {

                Path = RootPath(Path);

                var fileStream = new FileStream(Path, FileMode.CreateNew, FileAccess.Write);
                //Content.Seek(0, SeekOrigin.Begin);
                Content.CopyTo(fileStream);
                fileStream.Close();
            }
            catch (IOException ex)
            {
                throw new WebFaultException<IOException>(ex, System.Net.HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
        }

        private void ThrowException(Exception ex)
        {
            var myex = new Exception(ex.Message);
            throw new WebFaultException<Exception>(myex, System.Net.HttpStatusCode.BadRequest);
        }
    }
}
