using System.DirectoryServices.ActiveDirectory;
using System.Net.Http.Headers;
using System.Net;

namespace http_response_to_testbox
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            const int SQUARE = 60;
            textBox3.Controls.Add(new Button
            {
                Size = new Size(60, 60),
                Location = new Point(
                    this.Width - 60,
                    this.Height - 60
                )
            });
        }
    }
    public class CdataController : ApiController
    {
        F_Main mainForm = new F_Main();

        public async Task<HttpResponseMessage> PostPayloadEventsOp(string SN, string table, string OpStamp)
        {
            using (var contentStream = await this.Request.Content.ReadAsStreamAsync())
            {
                contentStream.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(contentStream))
                {
                    string results = sr.ReadToEnd();
                    mainForm.TextBoxRequestMsg = results;
                }
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent("OK", System.Text.Encoding.UTF8);
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromMinutes(2)
            };
            return response;
        }
    }
}