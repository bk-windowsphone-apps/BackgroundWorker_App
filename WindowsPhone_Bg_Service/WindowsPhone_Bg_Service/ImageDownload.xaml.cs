using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO.IsolatedStorage;
using System.IO;
using System.ComponentModel;
using System.Windows.Resources;

namespace WindowsPhone_Bg_Service
{
    public partial class ImageDownload : PhoneApplicationPage
    {
        BackgroundWorker bw = new BackgroundWorker();
        private HttpWebRequest webRequest;
        List<Uri> urls = new List<Uri>();
        private string filename = string.Empty;
        private int fname = 1;

        public ImageDownload()
        {
            InitializeComponent();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            urls.Add(new Uri("http://www.sjsu.edu/sjsuhome/pics/paintings-on-campus.jpg"));
            urls.Add(new Uri("http://www.sjsu.edu/academics/pics/acedemics.jpg"));
            urls.Add(new Uri("http://www.sjsu.edu/discover/pics/administration.jpg"));
            urls.Add(new Uri("http://info.sjsu.edu/info/img/land_admission.jpg"));
            urls.Add(new Uri("http://www.sjsu.edu/sjsuhome/pics/black-history-month-021114.jpg"));
            urls.Add(new Uri("http://www.sjsu.edu/sjsuhome/pics/back-to-school-012414.jpg"));
            urls.Add(new Uri("http://www.sjsu.edu/sjsuhome/pics/hiep-012714.jpg"));
            urls.Add(new Uri("http://www.sjsu.edu/housingoptions/pics/campus-housing.jpg"));
            urls.Add(new Uri("http://www.sjsu.edu/future_students/pics/student-photographer.jpg"));
            urls.Add(new Uri("http://www.sjsu.edu/alumni/pics/scholarships_piggybank_660.jpg"));

            base.OnNavigatedTo(e);
        }

        private void Image_Start_Button_Click(object sender, RoutedEventArgs e)
        {
            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            }
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        //background worker event handler
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            foreach (Uri value in urls)
            {
                //string filename = string.Empty;
                if (value.IsAbsoluteUri)
                    filename = System.IO.Path.GetFileName(value.AbsoluteUri);
                //MessageBox.Show("file download : " + filename);
                webRequest = (HttpWebRequest)HttpWebRequest.Create(value);
                webRequest.BeginGetResponse(ImageResponseCallback, webRequest);
            }

            
        }
       
        //HttpWebRequest handler
        private void ImageResponseCallback(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            if (request != null)
            {
                try
                {
                    WebResponse response = request.EndGetResponse(result);
                    WriteToFile(response.GetResponseStream());
                }
                catch (WebException e)
                {
                    Console.Out.Write("Error: " + e.Message);
                    return;
                }
            }
        }

        private void WriteToFile(Stream stream)
        {
            string name = fname.ToString();// this.filename;
            string n = name + ".jpg";
            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isostream = iso.CreateFile("Shared/Transfers/"+n))
                {
                    fname += 1;
                    using (BinaryWriter BW = new BinaryWriter(isostream))
                    {
                        Stream S = stream;
                        long lg = S.Length;
                        byte[] bff = new byte[4048];
                        int Count = 0;
                        using (BinaryReader BR = new BinaryReader(S))
                        {
                            // read file in chunks in order to reduce memory consumption and increase performance
                            while (Count < lg)
                            {
                                int actual = BR.Read(bff, 0, bff.Length);
                                Count += actual;
                                BW.Write(bff, 0, actual);
                            }
                        }
                    }
               
                }
            }
        }//End-WriteToFile
    }
}