using System.DirectoryServices.ActiveDirectory;
using System.Net.Http.Headers;
using System.Net;
using System.Web.Http;
using System.Drawing.Text;

namespace http_response_to_testbox
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            buttonPost.Click += onPost;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            #region G L Y P H
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Fonts",
                "glyphs.ttf");
            privateFontCollection.AddFontFile(path);
            var fontFamily = privateFontCollection.Families[0];
            Glyphs = new Font(fontFamily, 20F);
            #endregion G L Y P H

            buttonPost.Font = Glyphs;
            buttonPost.Text = "\uE800";
            buttonPost.BackColor = Color.Aqua;
        }
        public static Font Glyphs { get; private set; }
        PrivateFontCollection privateFontCollection = new PrivateFontCollection();

        private async void onPost(object? sender, EventArgs e)
        {
            try
            {
                UseWaitCursor = true;
                buttonPost.BackColor = Color.LightGreen;
                var response = await _controller.MockPostPayloadEventsOp("38D6FF5-F89C", "records", "Asgard");
                if((response.Headers != null) && (response.Headers.CacheControl != null))
                {
                    textBox3.Text = $"{response.Headers.CacheControl.MaxAge}";
                }  
            }
            finally
            {
                UseWaitCursor = false;
                Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y);
            }
        }
        MockCdataController _controller = new MockCdataController();
    }
    public class MockCdataController : ApiController
    {
        public async Task<HttpResponseMessage> MockPostPayloadEventsOp(string SN, string table, string OpStamp)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://stackoverflow.com/q/75310027/5438626"); 
                response.Content = new StringContent("OK", System.Text.Encoding.UTF8);
                response.Headers.CacheControl = new CacheControlHeaderValue()
                {
                    MaxAge = TimeSpan.FromMinutes(3.5)
                };
                return response;
            }
        }
    }
}