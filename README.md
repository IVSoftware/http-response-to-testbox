Your question states that your goal is to **change textBox.Text control value in Winform** and your code indicates that you want to do this by processing an `HttpResponseMessage`. 

It seems to me that the `Form` that has the `textBox3` control might want to await the response so that it can meaningfully process its content and assign the value to the text box.

For a minimal example, mock the API request:

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
                    MaxAge = TimeSpan.FromMinutes(2)
                };
                return response;
            }
        }
    }

***
The `Form` that is in posession of `textBox3` could invoke something like this:


    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            buttonPost.Click += onPost;
        }
        private async void onPost(object? sender, EventArgs e)
        {
            try
            {
                UseWaitCursor = true;
                var response = await _controller.MockPostPayloadEventsOp("38D6FF5-F89C", "records", "Asgard");
                if((response.Headers != null) && (response.Headers.CacheControl != null))
                {
                    TimeSpan? maxAge = response.Headers.CacheControl.MaxAge;
                    textBox3.Text = $"{maxAge}";
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