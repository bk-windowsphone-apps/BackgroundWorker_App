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
    public partial class TextFileDownload : PhoneApplicationPage
    {
        BackgroundWorker bw = new BackgroundWorker();
        private HttpWebRequest webRequest;
        List<Uri> urls = new List<Uri>();
        private string filename = string.Empty;
        private int fname = 1;
        public TextFileDownload()
        {
            InitializeComponent();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            //bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            urls.Add(new Uri("http://www.sjsu.edu/robots.txt"));
            urls.Add(new Uri("http://cs.sjsu.edu/~austin/cs166-fall13/lab11.txt"));
            urls.Add(new Uri("http://cs.sjsu.edu/~austin/cs252-fall13/turkeyQuest.txt"));
            urls.Add(new Uri("http://www.engr.sjsu.edu/spartnik/newsletters/v5i1.txt"));
            urls.Add(new Uri("http://cs.sjsu.edu/~mak/archive/CS146-Summer2013/lectures/CountSortOutput.txt"));   

            base.OnNavigatedTo(e);
        }

        private void TextFile_Start_Button_Click(object sender, RoutedEventArgs e)
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
                if (value.IsAbsoluteUri)
                {
                    this.filename = System.IO.Path.GetFileName(value.AbsoluteUri);
                    //MessageBox.Show("file download : " + filename);
                    webRequest = (HttpWebRequest)HttpWebRequest.Create(value);
                    webRequest.BeginGetResponse(ResponseCallback, webRequest);
                }
            }


        }
        
        //HttpWebRequest handler
        private void ResponseCallback(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            if (request != null)
            {
                try
                {
                    WebResponse response = request.EndGetResponse(result);
                    Stream stm = response.GetResponseStream();
                    //WriteToFile(stm, this.filename);   // store in isolated s
                    WriteToFile(stm);
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
            string name = fname + ".txt";
            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isostream = iso.CreateFile("Shared/Transfers/" + name))
                {
                    fname++;
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