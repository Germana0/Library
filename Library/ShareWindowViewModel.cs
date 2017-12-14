using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Web;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Library
{
    class ShareWindowViewModel : INotifyPropertyChanged
    {
        private string _url;
        public string URL
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged("URL");
            }
        }
        public Command OnPageLoad { get; set; }
        private string post;
        public ShareWindowViewModel(string _post)
        {
            post = _post;
            OnPageLoad = new Command();
            OnPageLoad.ExecFunc = MakePost;
            URL = "https://oauth.vk.com/authorize?response_type=token&scope=wall&client_id=6292556";
        }
        private void MakePost(object parameter)
        {
            if (parameter == null || !(parameter is WebBrowser)) MessageBox.Show("wrong parameter"); return;
            Uri uri = ((WebBrowser)parameter).Source;
            if (string.IsNullOrEmpty(((WebBrowser)parameter).Source.ToString())) return;
            string fragment = uri.Fragment.TrimStart('#');
            string token = HttpUtility.ParseQueryString(fragment).Get("access_token");
            string error = HttpUtility.ParseQueryString(fragment).Get("error");
            string error_description = HttpUtility.ParseQueryString(fragment).Get("error_description");
            if (error != null || error=="")  { MessageBox.Show(error_description); MessageBox.Show("there is error"); return; }
            if (token == null) MessageBox.Show("token is null"); return;
            string url = $"https://api.vk.com/method/wall.post?access_token={token}&message={post}";
            try
            {
                using (StreamReader reader = new StreamReader(new WebClient().OpenRead(url)))
                {
                    string response = reader.ReadToEnd();
                    if (response.ToLower().Contains("error"))
                        MessageBox.Show("Your result was not posted");
                    else
                        Process.Start("https://vk.com/id0");
                }
            }
            catch (Exception i)
            {
                MessageBox.Show(i.Message);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
