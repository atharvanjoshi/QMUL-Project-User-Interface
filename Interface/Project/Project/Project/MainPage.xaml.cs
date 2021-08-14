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
        static string token = "rpzSI2olZbMAAAAAAAAAAXN3DalttE8YrVVmpHr_sY39B49Ssjwh6VHHi-NEYYjj";
        static string url = "https://qmul-model-api.herokuapp.com/";
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
                    reportLabel.IsVisible = false;
                    sourceImg.IsVisible = false;
                    loader.IsRunning = true;
                    loader.IsVisible = true;

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
                    //reportLabel.Text = file.FileName;
                    //reportLabel.IsVisible = true;
                    using (var httpClient = new System.Net.Http.HttpClient())
                    {
                        //httpClient.DefaultRequestHeaders.Add(RequestConstants.UserAgent, RequestConstants.UserAgentValue);
                        var response = httpClient.GetStringAsync(new Uri(url)).Result;
                        string result = response.ToString().Replace("\\",string.Empty);
                        result = result.Replace("\"", string.Empty);
                        double result2 = Convert.ToDouble(result);
                        loader.IsVisible = false;
                        loader.IsRunning = false;
                        if(result2 < 0.5)
                        {
                            reportLabel.Text = "Benign";
                            reportLabel.TextColor = Xamarin.Forms.Color.Green;
                        }
                        else
                        {
                            reportLabel.Text = "Malignant";
                            reportLabel.TextColor = Xamarin.Forms.Color.Red;
                        }
                        reportLabel.FontSize = 30;
                        reportLabel.IsVisible = true;
                    }
                    
                }
                
                
                //IsBusy = true;
            }
            catch (Exception)
            {
                loader.IsVisible = false;
                loader.IsRunning = false;
                reportLabel.Text = "No File Selected";
                reportLabel.IsVisible = true;
            }
        }

        
    }
}
