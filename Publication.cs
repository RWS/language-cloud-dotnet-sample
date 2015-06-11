using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalTravelInfo
{
    public class Publication
    {
        public Publication()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
    }
}