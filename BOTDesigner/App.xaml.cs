using BOTDesigner.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BOTDesigner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);



            //initialize the splash screen and set it as the application main window
            var splashScreen = new Trial();
            this.MainWindow = splashScreen;
            splashScreen.Show();



            //in order to ensure the UI stays responsive, we need to
            //do the work on a different thread
            Task.Factory.StartNew(() =>
            {
                //we need to do the work in batches so that we can report progress
                //for (int i = 1; i <= 100; i++)
                //{
                //    //simulate a part of work being done
                //    System.Threading.Thread.Sleep(20);



                //    //because we're not on the UI thread, we need to use the Dispatcher
                //    //associated with the splash screen to update the progress bar
                //    splashScreen.Dispatcher.Invoke(() => splashScreen.Progress = i);
                //}

                System.Threading.Thread.Sleep(7000);

                //once we're done we need to use the Dispatcher
                //to create and show the main window
                this.Dispatcher.Invoke(() =>
                {
                    //initialize the main window, set it as the application main window
                    //and close the splash screen

                    //var loginWindow = new LoginWindow();
                    //this.MainWindow = loginWindow;
                    //loginWindow.Show();

                    var mainWindow = new MainWindow();
                    this.MainWindow = mainWindow;
                    mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                    mainWindow.WindowState = WindowState.Maximized;
                    //       mainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; //addednow
                    mainWindow.MaxHeight = SystemParameters.WorkArea.Height; //addednow
                    mainWindow.Show();



                    splashScreen.Close();
                    mainWindow.WindowStyle = WindowStyle.None;
                    // mainWindow.MaxHeight = SystemParameters.WorkArea.Height; //addednow
                    mainWindow.IsFirstLaunch = false;





                });
            });
        }




        public void OnPublish(int flag)
        {


            //initialize the splash screen and set it as the application main window
            var splashScreen = new Publishing();
            var currentWindow = this.MainWindow;
            this.MainWindow = splashScreen;
            splashScreen.Show();

            //in order to ensure the UI stays responsive, we need to
            //do the work on a different thread
            Task.Factory.StartNew(() =>
            {
                //we need to do the work in batches so that we can report progress
                for (int i = 1; i <= 100; i++)
                {
                    //simulate a part of work being done
                    System.Threading.Thread.Sleep(10);

                    //because we're not on the UI thread, we need to use the Dispatcher
                    //associated with the splash screen to update the progress bar
                    splashScreen.Dispatcher.Invoke(() => splashScreen.Progress = i);
                }

                //once we're done we need to use the Dispatcher
                //to create and show the main window
                this.Dispatcher.Invoke(() =>
                {
                    //initialize the main window, set it as the application main window
                    //and close the splash screen
                    //var mainWindow = new MainWindow();
                    //this.MainWindow = mainWindow;
                    this.MainWindow = currentWindow;
                    currentWindow.Show();

                    //  mainWindow.Show();

                    splashScreen.Close();

                    if (flag == 1)
                    {
                        MessageBox.Show("File has been Published Successfully!!");
                    }
                    else
                    {
                        MessageBox.Show("Error Occurred while Publishing the File!!");
                    }

                });
            });
        }







  //      public void OnLogout()
  //      {
            
  //          //initialize the splash screen and set it as the application main window
  // //         var mainWindow = this.MainWindow;
  //          LoginWindow login = new LoginWindow();
           
  ////          this.MainWindow = login;
  //          login.Show();
  //  //        mainWindow.Close();



  //          //in order to ensure the UI stays responsive, we need to
  //          //do the work on a different thread
  //          //Task.Factory.StartNew(() =>
  //          //{
  //          //    //we need to do the work in batches so that we can report progress
  //          //    //for (int i = 1; i <= 100; i++)
  //          //    //{
  //          //    //    //simulate a part of work being done
  //          //    //    System.Threading.Thread.Sleep(30);



  //          //    //    //because we're not on the UI thread, we need to use the Dispatcher
  //          //    //    //associated with the splash screen to update the progress bar
  //          //    //    splashScreen.Dispatcher.Invoke(() => splashScreen.Progress = i);
  //          //    //}



  //          //    //once we're done we need to use the Dispatcher
  //          //    //to create and show the main window
  //          //    this.Dispatcher.Invoke(() =>
  //          //    {
  //          //        //initialize the main window, set it as the application main window
  //          //        //and close the splash screen
  //          //        MainWindow.Close();
  //          //        var loginWindow = new LoginWindow();
  //          //        this.MainWindow = loginWindow;
  //          //        loginWindow.Show();



  //          //        // var mainWindow = new MainWindow();
  //          //        // this.MainWindow = mainWindow;


  //          //        // mainWindow.Show();

  //          //        // curr.Close();







  //          //    });
  //          //});
  //      }

       
    }
}
