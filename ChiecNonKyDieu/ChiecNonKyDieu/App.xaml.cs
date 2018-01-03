using ChiecNonKyDieu.Component;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ChiecNonKyDieu
{
    internal delegate void Invoker();
    public partial class App : Application
    {
        public App()
        {
            ApplicationInitialize = _applicationInitialize;
        }
        public static new App Current
        {
            get { return Application.Current as App; }
        }
        internal delegate void ApplicationInitializeDelegate(SplashScreen splashWindow);
        internal ApplicationInitializeDelegate ApplicationInitialize;
        private void _applicationInitialize(SplashScreen splashWindow)
        {
            QuestionManager.InitDic(splashWindow);

            // Create the main window, but on the UI thread.
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
            });
        }
    }
}
