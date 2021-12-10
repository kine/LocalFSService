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

    [DataContract]
    public class MyFileInfo
    {
        [DataMember]
        public string Name;
        [DataMember]
        public bool IsDirectory;

    }
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILocalFSService" in both code and config file together.
    [ServiceContract]
    public interface ILocalFSServiceContract
    {
        [OperationContract, WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        bool CheckIfFolderExists(String Path);

        [OperationContract, WebInvoke(Method = "POST",BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = "UploadFile?Path={Path}", ResponseFormat = WebMessageFormat.Json )]
        void UploadFile(String Path, Stream Content);

        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        Stream DownloadFile(String Path);

        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        void DeleteFile(String Path);

        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        void MoveFile(String Path, String NewPath);

        [OperationContract, WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        MyFileInfo[] GetListOfFiles(String Path, String Pattern="");

        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        void CreateFolder(String ParentFolderName, String FolderName);
    }

    public interface ILocalFSServiceChannel : ILocalFSServiceContract, IClientChannel { }
}
