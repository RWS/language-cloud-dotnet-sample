using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalTravelInfo.Model
{
    public class CreateProjectResult
    {
        public Project Project { get; set; }
    }

    public class Project
    {
        public string Id { get; set; }
        public string UploadToken { get; set; }

        public List<Job> Jobs { get; set; }

    }

    public class Job
    {
        public string Id { get; set; }
        public string CurrentState { get; set; }
        public string TranslationFileId { get; set; }
    }
}