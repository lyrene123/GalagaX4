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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        Player player;

        public GameWindow()
        {
            InitializeComponent();

            Image playerPic = new Image();
            playerPic.Source = UtilityMethods.LoadImage("pics/galaga_ship.png");
            playerPic.Width = 42;
            playerPic.Height = 46;
            canvas.Children.Add(playerPic);
            Canvas.SetLeft(playerPic, 405);
            Canvas.SetTop(playerPic, 500);
            Point playerPoint = new Point(27, 490);
            player = new Player(playerPoint, playerPic, canvas, 15);

            /* Level1 lv1 = new Level1(this, canvas, player);
             lv1.Play();*/
            Level2 lv2 = new Level2(this, this.canvas, this.player);
            lv2.Play();

            KeyDown += new KeyEventHandler(MyGrid_KeyDown);
            DecrementColdDown();
        }

        
        public void MyGrid_KeyDown(object sender, KeyEventArgs e)
        {
            player.Move();

            if(Player.ColdDown < progressBar.Maximum)
            {
                player.Shoot();
            }
            else
            {
                Player.ColdDown = 10;
            }
            
            //label.Content = "Cold down : " + Player.ColdDown;
            //progressBar.Value = Player.ColdDown;
        }

        void DecrementColdDown()
        {
            DispatcherTimer coldDownTimer = new DispatcherTimer();
            coldDownTimer.Interval = TimeSpan.FromMilliseconds(250);
            coldDownTimer.Tick += new EventHandler(DecrementColdDown);
            coldDownTimer.Start();
        }

        void DecrementColdDown(Object sender, EventArgs e)
        {
            if(Player.ColdDown > 0)
            {
                Player.ColdDown -= 0.1;
            }
            else
            {
                Player.ColdDown = 0;
            }
            
            label.Content = "Cold down : " + Player.ColdDown;
            progressBar.Value = Player.ColdDown;
        }
       
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /*TimeSpan ts = new TimeSpan(0, 0, 0, 0, 6);
            GameWindow media = new GameWindow();
            media.mediaElement.Position = ts;
            media.mediaElement.Volume = 6.6;
            media.mediaElement.SpeedRatio = 1.5;
            media.mediaElement.Source = new Uri(@"D:\GalagaX4_Backup\GalagaX4\audio\mainbacksound.wav", UriKind.Absolute);
            media.mediaElement.Play();
            */
        }
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            /*TimeSpan ts = new TimeSpan(0, 0, 0, 0, 0);
            mediaElement.Position = ts;
            mediaElement.Volume = 6.6;
            //mediaElement.SpeedRatio = 0.0001;
            mediaElement.Source = new Uri(@"D:\GalagaX4_Backup\GalagaX4\audio\mainbacksound.wav", UriKind.Absolute);
            mediaElement.Play();
            */
        }

    }
}
