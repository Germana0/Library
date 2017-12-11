using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.IO;
using System.Diagnostics;
using System;

namespace Library
{
    public partial class ShareWindow : Window
    {
        public ShareWindow(string post)
        {
            InitializeComponent();
            DataContext = new ShareWindowViewModel(post);
        }
    }
}
