
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var gameWindow = new GameWindow();
            this.Hide();
            gameWindow.Show();
            this.Close();
            gameWindow.mediaElement.Play();
                        
            
        }
    }
}