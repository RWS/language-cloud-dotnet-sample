using LocalTravelInfo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LocalTravelInfo
{
    public class PublishingTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] FileContent { get; set; }
        public string FileName { get; set; }

        public string TranslatedTitle { get; set; }
        public string TranslatedDescription { get; set; }


        public string FromLanguage { get; set; }

        public string ToLanguage { get; set; }

        public HumanTranslationQuote Quote { get; set; }

        public Stream GetFileContentStream()
        {
            if (FileContent == null)
                return null;
                        
            return new MemoryStream(FileContent);
        }
    }
}