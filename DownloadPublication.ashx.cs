using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LocalTravelInfo
{
    /// <summary>
    /// Summary description for DownloadPublication
    /// </summary>
    public class DownloadPublication : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request.QueryString["id"].ToString();

            Publication p = PublicationsRepository.Instance.GetPublication(new Guid(id));

            context.Response.ContentType = "application/octet-stream";
            context.Response.AppendHeader("content-disposition", "filename=" + p.FileName);
                        
            using (Stream s = PublicationsRepository.Instance.DownloadPublication(p))
            {
                s.CopyTo(context.Response.OutputStream);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}