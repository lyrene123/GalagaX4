
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
        /// <summary>
        /// The Mainwindow Constructor will invoke the InitializeComponent method
        /// and load the Main window of the Game. 
        /// </summary>

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


            BitmapImage[] loadGameSoures = { UtilityMethods.LoadImage("pics/loadGame_blue.png")
                    , UtilityMethods.LoadImage("pics/loadGame_white.png") };
            Animation loadGameAnim = new Animation(loadGamePic, loadGameSoures, true);
            Animation.Initiate(loadGameAnim, 150);
        }
        /// <summary>
        /// The MainWindow_Closing Occurs immediately after the main window is closed.
        /// It Terminates the process and returns an exit code to the operating system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        /// <summary>
        /// The MainWindow_Closed Occurs immediately after the main window is about to close.
        /// The method is called right after the MainWindow_Closing method is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        /// <summary>
        /// The button_Click event handler method creates a new instance of the GameWindow Object. 
        /// After it occurs the main window is hidden and the new instance of GameWindow is shown 
        /// on the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            var gameWindow = new GameWindow(false);
            this.Hide();
            gameWindow.Show();
                                    
            
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