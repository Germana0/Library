using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Library
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain; //берем приложение подписываем его на обрб ошибок ,делаем переменную котор содержит прилож целеком
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(globex);
        }

        static void globex(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            MessageBox.Show(e.Message);
        }
    }
}
