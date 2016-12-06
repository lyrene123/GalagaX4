using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace GalagaX4
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        DispatcherTimer coldDownTimer;
        DispatcherTimer checkLife;
        DispatcherTimer lifeTimer;
        Player player;
        bool isPause;
        Button resume;
        Button save;
        Button load;
        Image life;


        public GameWindow()
        {
            this.Closed += GameWindow_Closed;
            this.Closing += GameWindow_Closing;
                                    
            InitializeComponent();
            backgroundImage.Width = 860;
            backgroundImage.Height = 650;
            mediaElement.Source = new Uri("audio/main2.wav", UriKind.Relative);
            mediaElement.BeginInit();
            mediaElement.Position = TimeSpan.FromSeconds(1);
            //mediaElement.Stop();
            mediaElement.Volume = 0.07;
            //mediaElement.MediaOpened += new RoutedEventHandler(Element_MediaOpened);
            mediaElement.Play();
            mediaElement.MediaEnded += new RoutedEventHandler(Element_MediaEnded);
            //mediaElement.Play();
            //


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

           // Level4 lv2 = new Level4(this, canvas, player);
            //lv2.Play();

            KeyDown += new KeyEventHandler(MyGrid_KeyDown);
           // buyLives();

            DecrementColdDown();
        }


        public void buyLives()
        {
            this.life = new Image();
            this.life.Width = 34;
            this.life.Height = 26;
            canvas.Children.Add(this.life);
            Canvas.SetLeft(this.life, 400);
            Canvas.SetTop(this.life, 10);

            this.checkLife = new DispatcherTimer(DispatcherPriority.Normal);
            checkLife.Interval = TimeSpan.FromSeconds(15);
            checkLife.Tick += new EventHandler(giveLife);
            checkLife.Start();
        }

        private void giveLife(object sender, EventArgs e)
        {
            MessageBox.Show("lifee");
            if (player.GetLives() <=2)
            {
                MessageBox.Show("lifee");
                this.lifeTimer = new DispatcherTimer(DispatcherPriority.Normal);
                lifeTimer.Interval = TimeSpan.FromMilliseconds(150);
                lifeTimer.Tick += new EventHandler(sendLife);
                lifeTimer.Start();
            }
        }

        private void sendLife(object sender, EventArgs e)
        {
           // MessageBox.Show("lifee");
            double posY = Canvas.GetTop(this.life);
            double posX = Canvas.GetLeft(this.life);
            Rect rectLife = new Rect(posX, posY, this.life.Width + 20, this.life.Height + 20);
            Rect rectPlayer = new Rect(this.player.GetPoint().X, this.player.GetPoint().Y, this.player.GetImage().Width - 5, this.player.GetImage().Height - 5);
            
            if (posY <= 500)
            {
                Canvas.SetTop(this.life, posY += 10);
                this.life.Source = UtilityMethods.LoadImage("pics/galaga_ship.png");

                if (rectPlayer.IntersectsWith(rectLife))
                {
                    MessageBox.Show("hello");
                    this.lifeTimer.Stop();
                    player.addLife();
                    canvas.Children.Remove(this.life);
                }
            }
            else
            {
                canvas.Children.Remove(this.life);
            }
        }

        private void GameWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void GameWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void MyGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.isPause == false)
            {
                player.Move();
            }

            if (Player.ColdDown < progressBar.Maximum)
            {
                if (player.getEnemiesSize() != 0 && this.isPause == false)
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
            coldDownTimer.Interval = TimeSpan.FromMilliseconds(500);
            coldDownTimer.Tick += new EventHandler(DecrementColdDown);
            coldDownTimer.Start();
        }

        void DecrementColdDown(Object sender, EventArgs e)
        {
            if (this.isPause == false)
            {
                if (Player.ColdDown > 0)
                {
                    Player.ColdDown -= 0.1;
                }
                else
                {
                    Player.ColdDown = 0;
                }

                if (Player.ColdDown >= 8)
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
                mediaElement.Stop();
                mediaElement.Source = null;
                sound.playSoundLooping();
                BackToMainWindow();
            }
        }

        async void BackToMainWindow()
        {
            this.coldDownTimer.Stop();
            await Task.Delay(3000);
            this.Hide();
            var mainWindow = new MainWindow();
            mainWindow.Show();
            sound.StopSound();
            //this.Close();
        }

        private void Element_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(1);
            mediaElement.Play();
        }
        private void Element_MediaOpened(object sender, RoutedEventArgs e)
        {
            
            //mediaElement.Play();
        }
        private void playBtn_Click()
        {

            this.isPause = false;
            if (this.player.getCurrentLevel() == 1)
            {
                if (Level1.timerRandom != null)
                {
                    Level1.timerRandom.Start();
                }
            }
            if (this.player.getCurrentLevel() == 2)
            {
                if (Level2.timerRandom != null)
                {
                    Level2.timerRandom.Start();
                }
            }
            if (this.player.getCurrentLevel() == 3)
            {
                if (Level3.timerRandom != null)
                {
                    Level3.timerRandom.Start();
                }
            }
            if(this.player.getCurrentLevel() == 4)
            {
                if (Level4.timerRandom != null)
                {
                    Level4.timerRandom.Start();
                }
            }

            List<Enemies> allEnemies = this.player.getEnemiesList();
            if (allEnemies.Count != 0)
            {
                for (int i = 0; i < allEnemies.Count; i++)
                {
                    if (allEnemies[i].GetType() == typeof(Bug))
                    {
                        ((Bug)allEnemies[i]).restartMove();
                    }
                    else if (allEnemies[i].GetType() == typeof(SpaceShip))
                    {
                        ((SpaceShip)allEnemies[i]).restartMove();
                        ((SpaceShip)allEnemies[i]).restartShoot();
                    }
                    else if(allEnemies[i].GetType() == typeof(Commander))
                    {
                        ((Commander)allEnemies[i]).restartMove();
                        ((Commander)allEnemies[i]).restartShoot();
                    }
                    else
                    {

                    }
                }
            }

            List<Bullet> allBullets = Bullet.getBulletList;
            if (allBullets.Count != 0)
            {
                for (int i = 0; i < allBullets.Count; i++)
                {
                    allBullets[i].restartShootDown();
                    allBullets[i].restartShootLeft();
                    allBullets[i].restartShootRight();
                    allBullets[i].restartShootUp();
                }
            }

            Menu("remove");
        }

        private void pauseBtn_Click()
        {
            if (this.player.getCurrentLevel() == 1)
            {
                if (Level1.timerRandom != null)
                {
                    Level1.timerRandom.Stop();
                }
            }
            if (this.player.getCurrentLevel() == 2)
            {
                if (Level2.timerRandom != null)
                {
                    Level2.timerRandom.Stop();
                }
            }
            if (this.player.getCurrentLevel() == 3)
            {
                if (Level3.timerRandom != null)
                {
                    Level3.timerRandom.Stop();
                }
            }
            if(this.player.getCurrentLevel() == 4)
            {
                if (Level4.timerRandom != null)
                {
                    Level4.timerRandom.Stop();
                }
            }

            List<Enemies> allEnemies = this.player.getEnemiesList();
            if (allEnemies!=null)
            {
                for (int i = 0; i < allEnemies.Count; i++)
                {
                    if (allEnemies[i].GetType() == typeof(Bug))
                    {
                        ((Bug)allEnemies[i]).stopMove();
                    }
                    else if (allEnemies[i].GetType() == typeof(SpaceShip))
                    {
                        ((SpaceShip)allEnemies[i]).stopMove();
                        ((SpaceShip)allEnemies[i]).StopShoot();
                    }
                    else if(allEnemies[i].GetType() == typeof(Commander))
                    {
                        ((Commander)allEnemies[i]).stopMove();
                        ((Commander)allEnemies[i]).stopShoot();
                    }
                    else 
                    {

                    }
                }
            }

            List<Bullet> allBullets = Bullet.getBulletList;
            if (allBullets.Count != 0)
            {
                for (int i = 0; i < allBullets.Count; i++)
                {
                    allBullets[i].StopShootDown();
                    allBullets[i].StopShootLeft();
                    allBullets[i].StopShootRight();
                    allBullets[i].StopShootUp();
                }
            }
            Menu("display");
        }

        private void playPauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if(isPause == false)
            {
                isPause = true;
                playPauseBtn.IsEnabled = false;
                pauseBtn_Click();
            }
            else
            {
                isPause = false;
                playPauseBtn.IsEnabled = true;
                playBtn_Click();
            }
        }

        private void Menu(String action)
        {
            if (action.Equals("display"))
            {
                this.resume = new Button();
                canvas.Children.Add(resume);
                resume.Width = 300;
                resume.Height = 50;
                Canvas.SetLeft(resume, 300);
                Canvas.SetTop(resume, 200);
                resume.Content = "RESUME GAME";
                resume.FontSize = 25;
                resume.FontWeight = FontWeights.Bold;
                resume.Background = Brushes.DimGray;
                resume.Click += playPauseBtn_Click;

                this.save = new Button();
                canvas.Children.Add(save);
                save.Width = 300;
                save.Height = 50;
                Canvas.SetLeft(save, 300);
                Canvas.SetTop(save, 272);
                save.Content = "SAVE GAME";
                save.FontSize = 25;
                save.FontWeight = FontWeights.Bold;
                save.Background = Brushes.DimGray; ;
                save.Click += saveBtn_Click;

                this.load = new Button();
                canvas.Children.Add(load);
                load.Width = 300;
                load.Height = 50;
                Canvas.SetLeft(load, 300);
                Canvas.SetTop(load, 345);
                load.Content = "LOAD NEW GAME";
                load.FontSize = 25;
                load.FontWeight = FontWeights.Bold;
                load.Background = Brushes.DimGray;
                load.Click += loadBtn_Click;
            }

            if (action.Equals("remove"))
            {
                canvas.Children.Remove(this.resume);
                canvas.Children.Remove(this.save);
                canvas.Children.Remove(this.load);
            }
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
