using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalTravelInfo.Model
{
    public class TranslateFileResult
    {
        public string Id { get; set; }
        public string Status { get; set; }

        public Result Result { get; set; }
        
    }
       

    public class Result
    {
        public string DownloadURL { get; set; }
        public double? TrustScore { get; set; }
    }
}