
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GalagaX4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //test2
        
        public MainWindow()
        {
            this.Closed += MainWindow_Closed;
            this.Closing += MainWindow_Closing;
            

            InitializeComponent();

            BitmapImage[] newGameSources = { UtilityMethods.LoadImage("pics/newGame_white.png")
                    , UtilityMethods.LoadImage("pics/newGame_lightBlue.png") };
            Animation newGameAnimation = new Animation(newGamePic, newGameSources, true);
            Animation.Initiate(newGameAnimation, 100);

            BitmapImage[] titleSources = { UtilityMethods.LoadImage("pics/GameTitle_Blue.png")
                    , UtilityMethods.LoadImage("pics/GameTitle_lightBlue.png") };
            Animation titleAnimation = new Animation(titlePic, titleSources, true);
            Animation.Initiate(titleAnimation, 150);

            
            
            
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var gameWindow = new GameWindow(false);
            this.Hide();
            gameWindow.Show();
            //this.Close();
            gameWindow.mediaElement.BeginInit();
            gameWindow.mediaElement.Position = TimeSpan.FromMilliseconds(0);
            gameWindow.mediaElement.Play();
                         
            
        }



       

        private void loadingGame(object sender, RoutedEventArgs e)
        {

            var gameWindow = new GameWindow(true);
            this.Hide();
            gameWindow.Show();
            //this.Close();
            gameWindow.mediaElement.BeginInit();
            gameWindow.mediaElement.Position = TimeSpan.FromMilliseconds(0);
            gameWindow.mediaElement.Play();
        }
    }
}