using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.Text;
using LocalTravelInfo.Model;
using LocalTravelInfo.Util;

namespace LocalTravelInfo
{
    public class PublishingService
    {
        public async Task<InitiatePublishingResult> InitiatePublishing(PublishingTask task)
        {
            InitiatePublishingResult result = new InitiatePublishingResult();

            // translate title and description first
            task.TranslatedTitle = await MachineTranslateText(task.Title, task.FromLanguage, task.ToLanguage);
            task.TranslatedDescription = await MachineTranslateText(task.Description, task.FromLanguage, task.ToLanguage);

            // send the document for machine translation first
            TranslateFileResult mtResult = await MachineTranslateFile(task.FileName, task.GetFileContentStream(), task.FromLanguage, task.ToLanguage);

            // is the quality ok?
            bool useMachineTranslation = mtResult.Result.TrustScore == null || mtResult.Result.TrustScore.Value > 2;
            result.TrustScore = mtResult.Result.TrustScore;
            if (useMachineTranslation)
            {
                // download translation
                Stream translatedFileStream = await Download(mtResult.Result.DownloadURL);

                // publish immediately
                PublicationsRepository.Instance.Add(new Publication
                {
                    Title = task.TranslatedTitle,
                    Description = task.TranslatedDescription,
                    FileName = task.FileName,
                }, translatedFileStream);
                
                result.Method = "Machine translation";
                result.Price = 0;
            }
            else
            {
                // get a human translation quote
                HumanTranslationQuote quote = await GetHumanTranslationQuote(task.Title, task.FileName, task.GetFileContentStream(), task.FromLanguage, task.ToLanguage);
                result.Method = "Human translation";
                result.Price = quote.Price;
                task.Quote = quote;
                
                // send for human translation
                HumanTranslationQueue.Instance.Add(task);
            }
                        
            return result;
        }

        private async Task<string> MachineTranslateText(string text, string fromLanguage, string toLanguage)
        {
            TranslateTextResult result = await PostJson<TranslateTextResult>("/translate",
                String.Format(
                @"{{
                     ""text""        : ""{0}"",
                     ""from""        : ""{1}"",
                     ""to""          : ""{2}"",
                     ""domainCode""  : ""MTMzMQ""
                 }}",
                text, fromLanguage, toLanguage));

            return result.Translation;
        }
        

        public async Task<TranslateFileResult> MachineTranslateFile(string fileName, Stream fileContent, string fromLanguage, string toLanguage)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add( new StreamContent(fileContent), "file", fileName);
            content.Add(new StringContent(fromLanguage), "from");
            content.Add(new StringContent(toLanguage), "to");
            content.Add(new StringContent("true"), "computeTrustScore");
            content.Add(new StringContent("MTMzMQ"), "domainCode");  // travel domain
            
            TranslateFileResult result = await Post<TranslateFileResult>("/file-translations", content);
            string id = result.Id;

            while (result.Status == "inProgress")
            {
                await Task.Delay(1000);
                result = await Get<TranslateFileResult>("/file-translations/" + id);
            }

            return result;
        }

        public async Task<HumanTranslationQuote> GetHumanTranslationQuote(string name, string fileName, Stream fileContent, string fromLanguage, string toLanguage)
        {
            // create project
            CreateProjectResult project = await PostJson<CreateProjectResult>("/projects",
                String.Format(
                @"{{
                     ""type""        : ""translationProject"",
                     ""from""        : ""{0}"",
                     ""to""          : ""{1}"",
                     ""name""        : ""LocalTravelInfo: {2}"",
                     ""description"" : ""LocalTravelInfo project for publication {3}"",
                     ""autoAccept""  : ""true""
                 }}"
                , fromLanguage, toLanguage, name, fileName));

            // upload file to project
            MultipartFormDataContent formData = new MultipartFormDataContent();
            StreamContent fileStreamContent = new StreamContent(fileContent);
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain"); 
            formData.Add(fileStreamContent, "file", fileName);
            formData.Add(new StringContent(project.Project.Id), "projectId");
                        
            UploadFileResult file = await Post<UploadFileResult>("/files", formData);

            // get project quote
            ProjectQuotesResult quotes = null;
            string projectId = project.Project.Id;
            do
            {
                quotes = await Get<ProjectQuotesResult>(String.Format("/projects/{0}/quotes", projectId));
                await Task.Delay(1000);
            } while (quotes.PricesDetails.Q4.ErrorCode == "processing");

            return new HumanTranslationQuote
            {
                Project = project,
                Price = quotes.PricesDetails.Q4.Price.Value
            };
        }

        public async Task SendHumanTranslation(PublishingTask task)
        {
            // submit for human translation
            HumanTranslationResult result = await PlaceHumanTranslationOrder(task.Quote.Project);

            // publish
            PublicationsRepository.Instance.Add(new Publication
            {
                Title = task.TranslatedTitle,
                Description = task.TranslatedDescription,
                FileName = task.FileName,
            }, result.TranslatedFile);
        }


        public async Task<HumanTranslationResult> PlaceHumanTranslationOrder(CreateProjectResult project)
        {
            
            // place order
            CreateProjectResult order = await PutJson<CreateProjectResult>(String.Format("/projects/{0}/orders", project.Project.Id),
                @"{
                     ""qualityLevel"" : ""Q4""
                 }");

            string jobId = order.Project.Jobs[0].Id;
            while (true)
            {
                await Task.Delay(1000);
                JobStatusResult jobStatus = await Get<JobStatusResult>(String.Format("/jobs/{0}", jobId));

                if (jobStatus.Job.CurrentState == "completed")
                {
                    // download translated file
                    Stream translatedFileStream = await Download(String.Format("/files/{0}/file", jobStatus.Job.TranslationFileId));

                    return new HumanTranslationResult
                    {
                        TranslatedFile = translatedFileStream
                    };
                }
            }
        }

        private async Task<R> Post<R>(string relativeRequestUri, HttpContent content) 
        {
            using (HttpRequestMessage request = CreateRequestMessage("https://lc-api.sdl.com" + relativeRequestUri, HttpMethod.Post))
            {
                request.Content = content;
                
                HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);
                string responseText = await response.Content.ReadAsStringAsync();
                
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<R>(responseText);
            }
        }

        private async Task<R> PostJson<R>(string relativeRequestUri, string json)
        {
            return await SendJson<R>(relativeRequestUri, json, HttpMethod.Post);
        }

        private async Task<R> PutJson<R>(string relativeRequestUri, string json)
        {
            return await SendJson<R>(relativeRequestUri, json, HttpMethod.Put);
        }

        private async Task<R> SendJson<R>(string relativeRequestUri, string json, HttpMethod method)
        {
            using (HttpRequestMessage request = CreateRequestMessage("https://lc-api.sdl.com" + relativeRequestUri, method))
            {
                request.Content = new StringContent(json, Encoding.ASCII, "application/json");
                
                HttpResponseMessage response = await httpClient.SendAsync(request);
                
                string responseText = await response.Content.ReadAsStringAsync();
                
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<R>(responseText);
            }
        }

        private async Task<R> Get<R>(string relativeRequestUri)
        {
            using (HttpRequestMessage request = CreateRequestMessage("https://lc-api.sdl.com" + relativeRequestUri, HttpMethod.Get))
            {
                
                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseText = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<R>(responseText);
            }
        }

        public async Task<Stream> Download(string relativeRequestUri)
        {
            string uri = relativeRequestUri.StartsWith("/")
                ? "https://lc-api.sdl.com" + relativeRequestUri
                : relativeRequestUri;

            using (HttpRequestMessage request = CreateRequestMessage(uri, HttpMethod.Get))
            {
                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                await response.Content.LoadIntoBufferAsync();
                return await response.Content.ReadAsStreamAsync();
            }
        }

        private HttpRequestMessage CreateRequestMessage(string requestUri, HttpMethod httpMethod)
        {
            HttpRequestMessage message = new HttpRequestMessage(httpMethod, requestUri);

            var apiKey =ConfigurationManager.AppSettings["ApiKey"];

            message.Headers.Add("Authorization", string.Format("LC apiKey={0}",apiKey));
            
            return message;
        }
        
        HttpClient httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));

        
    }
}