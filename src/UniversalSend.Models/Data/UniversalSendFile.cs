using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Data
{
    public class UniversalSendFile//用来表示传输过程文件的抽象类
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public string FileType { get; set; }
        public string TransferToken { get; set; } //用来与传输中数据进行匹配的Token，也就是传输请求时生成的那一个Token
    }

    public class UniversalSendFileManager
    {
        public static UniversalSendFile GetUniversalSendFileFromFileRequestDataAndToken(FileRequestData fileRequestData,string token)
        {
            UniversalSendFile universalSendFile = new UniversalSendFile();
            universalSendFile.Id = fileRequestData.Id;
            universalSendFile.FileName = fileRequestData.FileName;
            universalSendFile.FileType = fileRequestData.FileType;
            universalSendFile.Size = fileRequestData.Size;
            universalSendFile.TransferToken = token;
            return universalSendFile;
        }
    }
}
