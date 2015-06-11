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
    public partial class Publish : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void PublishButton_Click(object sender, EventArgs e)
        {
            Page.RegisterAsyncTask(new PageAsyncTask(InitiatePublishing));
        }

        private async Task InitiatePublishing()
        {
            // get uploaded file
            byte[] data = new byte[FileUpload.FileContent.Length];
            FileUpload.FileContent.Read(data, 0, data.Length);
            
            // capture options
            PublishingTask task = new PublishingTask
                {
                    FileName = FileUpload.FileName,
                    FileContent = data,
                    FromLanguage = "eng",
                    ToLanguage = "fra",
                    Description = DescriptionTextBox.Text,
                    Title = TitleTextBox.Text
                };
            
            PublishingService service = new PublishingService();

            InitiatePublishingResult result =  await service.InitiatePublishing(task);

            Response.Redirect(String.Format("Published.aspx?method={0}&price=${1}&trustscore={2}&header={3}", result.Method, result.Price, result.TrustScore, result.Method == "Machine Translation" ? "Your arcticle has been published!" : "Your article will be published after human translation.") );
        }
    }
}