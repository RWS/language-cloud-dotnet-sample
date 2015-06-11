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
    public partial class Published : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MethodTextBox.Text = Page.Request.QueryString["method"].ToString();
            PriceTextBox.Text = Page.Request.QueryString["price"].ToString();
            TrustScoreTextBox.Text = Page.Request.QueryString["trustscore"].ToString();
        }
    }
}