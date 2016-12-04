using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        DispatcherTimer coldDownTimer;
        DispatcherTimer gameOverTimer;

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

            Level1 lv1 = new Level1(this, canvas, player);
            lv1.Play();

           
            KeyDown += new KeyEventHandler(MyGrid_KeyDown);
            
            DecrementColdDown();
        }

        
        public void MyGrid_KeyDown(object sender, KeyEventArgs e)
        {
            player.Move();

            if(Player.ColdDown < progressBar.Maximum)
            {
                if (player.getEnemiesSize() != 0)
                {
                    player.Shoot();
                }
            }
            else
            {
                Player.ColdDown = 10;
            }
        }

        void DecrementColdDown()
        {
            coldDownTimer = new DispatcherTimer(DispatcherPriority.Normal);
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
            
            if(Player.ColdDown >= 8)
            {
                progressBar.Foreground = Brushes.Red;
            }
            else
            {
                progressBar.Foreground = Brushes.Blue;
            }
            //label.Content = "Cold down : " + Player.ColdDown;
            progressBar.Value = Player.ColdDown;

            GameOver();
        }

        void GameOver()
        {
            if (player.GetLives() == 0)
            {
                Image gameOverPic = new Image();
                gameOverPic.Height = 200;
                gameOverPic.Width = 250;
                this.canvas.Children.Add(gameOverPic);
                Canvas.SetTop(gameOverPic, 200);
                Canvas.SetLeft(gameOverPic, 300);
                gameOverPic.Source = UtilityMethods.LoadImage("pics/gameOver.png");

                BackToMainWindow();
            }
        }

        async void BackToMainWindow()
        {
            this.coldDownTimer.Stop();
            await Task.Delay(2000);
            
            this.Hide();
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Element_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
        }

    }
}
