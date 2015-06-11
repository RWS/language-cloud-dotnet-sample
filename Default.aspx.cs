using LocalTravelInfo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LocalTravelInfo
{
    public partial class ViewPublications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PublicationsList.DataSource = PublicationsRepository.Instance.GetPublications();
            PublicationsList.DataBind();
        }
    }
}