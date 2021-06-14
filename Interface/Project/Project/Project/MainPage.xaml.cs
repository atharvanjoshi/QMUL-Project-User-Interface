using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace Project
{
    public partial class MainPage : ContentPage
    {
        static string token = "a1DQjzEuFusAAAAAAAAAAWnf1S2Zbekxm1h_9FJwB-xSxL1a6_5UBBf2RJCo6fZ9";
        public MainPage()
        {
            InitializeComponent();
        }

        /**public string GetReleases(string url)
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Add(RequestConstants.UserAgent, RequestConstants.UserAgentValue);
                var response = httpClient.GetStringAsync(new Uri(url)).Result;
                return response;
            }
        }**/

        private async void uploadButton_Clicked(object sender, EventArgs e)
        {
            //if (inputEntry.Text == null)
            //{
            //    reportLabel.Text = "Please enter a text";
            //    reportLabel.IsVisible = true;
            //}
            //else
            //{
            //    reportLabel.Text = this.GetReleases("https://date.nager.at/api/v2/publicholidays/" + DateTime.Now.Year.ToString() + "/" + (inputEntry.Text).ToUpper());
            //    reportLabel.IsVisible = true;
            //}
            try
            {
                FileResult fileResult = await FilePicker.PickAsync(
                                    new PickOptions
                                    {
                                        FileTypes = FilePickerFileType.Images,
                                        PickerTitle = "Pick an Image"
                                    });
                
                if (fileResult == null)
                {
                    reportLabel.Text = "No File Selected";
                    reportLabel.IsVisible = true;
                }
                else
                {
                    reportLabel.Text = "File Selected Successfully";
                    reportLabel.IsVisible = true;
                    var stream = await fileResult.OpenReadAsync();
                    //resultImage.Source = ImageSource.FromStream(() => stream);
                    //resultImage.IsVisible = true;
                    using (DropboxClient dbx = new DropboxClient(token))
                    {
                        string filename = "userPhoto.jpg";
                        Task<FileMetadata> updated = dbx.Files.UploadAsync("/" + filename, WriteMode.Overwrite.Instance, body: stream);
                        updated.Wait();
                        //var tx = dbx.Sharing.CreateSharedLinkWithSettingsAsync("/" + filename);
                        //tx.Wait();
                        //url = tx.Result.Url;
                        reportLabel.Text = "File Uploaded Successfully";
                    }
                }
                //reportLabel.Text = file.FileName;
                reportLabel.IsVisible = true;
                IsBusy = true;
            }
            catch (Exception)
            {
                reportLabel.Text = "No File Selected";
                reportLabel.IsVisible = true;
            }
        }

        private void takePhotoButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
