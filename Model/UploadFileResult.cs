using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalTravelInfo.Model
{
    public class UploadFileResult
    {
        public List<StoredFile> StoredFiles { get; set; }
    }

    public class StoredFile
    {
        public string Id { get; set; }
    }
}