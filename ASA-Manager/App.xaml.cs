using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ARKServerCreationTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool newConfig = false;
            ASCTConfiguration config = ASCTConfiguration.LoadConfig();

            if (newConfig = (config == null)) config = new ASCTConfiguration();
            Current.Properties["globalConfig"] = config;

            if (newConfig)
            {
                ConfigurationWindow configWindow = new ConfigurationWindow(true);
                configWindow.Show();
            }
            else
            {
                MainWindow main = new MainWindow();

                main.Show();
            }
        }
    }
}
