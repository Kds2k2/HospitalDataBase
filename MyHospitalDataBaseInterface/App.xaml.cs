using MyHospitalDataBaseInterface.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyHospitalDataBaseInterface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //var splashScreen = new SplashScreen("resources/SplashScreen.png");
            //splashScreen.Show(false);

            LoginWindow mainWindow = new LoginWindow();

            //Thread.Sleep((int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            //splashScreen.Close(TimeSpan.FromSeconds(0));

            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
