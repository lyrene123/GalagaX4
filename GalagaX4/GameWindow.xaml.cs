using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using WpfAnimatedGif;

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
        Button resumeBtn;
        Button saveBtn;
        Button loadBtn;
        Image life;
        GameSound sound = new GameSound(@"pack://application:,,,/GalagaX4;Component/audio/Game_Over.wav", true);

        bool load;
        Level1 lv1;


        public GameWindow()
        {
            this.Closed += GameWindow_Closed;
            this.Closing += GameWindow_Closing;

            InitializeComponent();
            backgroundImage.Width = 860;
            backgroundImage.Height = 650;
            /* mediaElement.Source = new Uri("audio/main2.wav", UriKind.Relative);
             mediaElement.BeginInit();
             mediaElement.Position = TimeSpan.FromSeconds(1);
             //mediaElement.Stop();
             mediaElement.Volume = 0.07;
             //mediaElement.MediaOpened += new RoutedEventHandler(Element_MediaOpened);
             mediaElement.Play();
             mediaElement.MediaEnded += new RoutedEventHandler(Element_MediaEnded);
             //mediaElement.Play();
             //

     */
            Image playerPic = new Image();
            playerPic.Source = UtilityMethods.LoadImage("pics/galaga_ship.png");
            ImageBehavior.SetAnimatedSource(playerPic, playerPic.Source);
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

        public GameWindow(Boolean load)
        {
            this.Closed += GameWindow_Closed;
            this.Closing += GameWindow_Closing;

            InitializeComponent();
            this.load = load;

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
            if (load == true)
            {
                lv1 = new Level1(this, canvas, player, true);
            }
            else
            {
                Image playerPic = new Image();
                playerPic.Source = UtilityMethods.LoadImage("pics/galaga_ship.png");
                playerPic.Width = 42;
                playerPic.Height = 46;
                canvas.Children.Add(playerPic);
                Canvas.SetLeft(playerPic, 405);
                Canvas.SetTop(playerPic, 500);
                Point playerPoint = new Point(27, 490);
                player = new Player(playerPoint, playerPic, canvas, 15);

                lv1 = new Level1(this, canvas, player);
                lv1.Play();
            }
            // Level4 lv4 = new Level4(this, canvas, player);
            //lv4.Play();

            KeyDown += new KeyEventHandler(MyGrid_KeyDown);

            DecrementColdDown();
        }

        public void buyLives()
        {
            this.checkLife = new DispatcherTimer(DispatcherPriority.Normal);
            checkLife.Interval = TimeSpan.FromMinutes(1);
            checkLife.Tick += new EventHandler(giveLife);
            checkLife.Start();
        }

        private void giveLife(object sender, EventArgs e)
        {
            // MessageBox.Show("giveLife");
            if (player.GetLives() <= 2)
            {
                // MessageBox.Show("need life");
                this.life = new Image();
                this.life.Width = 34;
                this.life.Height = 26;
                canvas.Children.Add(this.life);
                Canvas.SetLeft(this.life, 750);
                Canvas.SetTop(this.life, 10);
                this.lifeTimer = new DispatcherTimer(DispatcherPriority.Normal);
                lifeTimer.Interval = TimeSpan.FromMilliseconds(150);
                lifeTimer.Tick += new EventHandler(sendLife);
                lifeTimer.Start();
            }
        }

        private void sendLife(object sender, EventArgs e)
        {
            // MessageBox.Show("lifee");
            double posLifeY = Canvas.GetTop(this.life);
            double posLifeX = Canvas.GetLeft(this.life);
            double posPlayerX = Canvas.GetLeft(player.GetImage());
            double posPlayerY = Canvas.GetTop(player.GetImage());

            Rect rectLife = new Rect(posLifeX, posLifeY, this.life.Width - 5, this.life.Height - 5);
            Rect rectPlayer = new Rect(posPlayerX, posPlayerY, this.life.Width - 5, this.life.Height - 5);

            if (posLifeY <= 530)
            {
                Canvas.SetTop(this.life, posLifeY += 8);
                this.life.Source = UtilityMethods.LoadImage("pics/galaga_ship.png");

                if (rectPlayer.IntersectsWith(rectLife))
                {
                    //MessageBox.Show("entered!");
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
                if (load == true)
                {
                    LoadLevels.getStaticPlayer().Move();
                }
                else
                {
                    player.Move();

                }
            }

            if (Player.ColdDown < progressBar.Maximum)
            {
                if (load == true)
                {
                    if (LoadLevels.getStaticPlayer().getEnemiesSize() != 0 && this.isPause == false)
                    {
                        LoadLevels.getStaticPlayer().Shoot();
                    }
                    else
                    {
                        Player.ColdDown = 10;
                    }
                }
                else
                {
                    if (player.getEnemiesSize() != 0 && this.isPause == false)
                    {
                        player.Shoot();
                    }
                    else
                    {
                        Player.ColdDown = 10;
                    }
                }

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
            if (load == true)
            {
                if (LoadLevels.getStaticPlayer().GetLives() == 0)
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
            else
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
        }

        async void BackToMainWindow()
        {
            this.coldDownTimer.Stop();
            await Task.Delay(10000);
            this.Hide();
            var mainWindow = new MainWindow();
            player.explosionSoundEffect.StopSound();
            player.explosionSoundEffect.Dispose();
            sound.StopSound();
            sound.Dispose();
            mainWindow.Show();
            //sound.StopSound();
            //sound.Dispose();
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
            //this.lifeTimer.Start();
            //this.checkLife.Start();
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
            if (this.player.getCurrentLevel() == 4)
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
                    else if (allEnemies[i].GetType() == typeof(Commander))
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
            //this.lifeTimer.Stop();
            //this.checkLife.Stop();
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
            if (this.player.getCurrentLevel() == 4)
            {
                if (Level4.timerRandom != null)
                {
                    Level4.timerRandom.Stop();
                }
            }

            List<Enemies> allEnemies = this.player.getEnemiesList();
            if (allEnemies != null)
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
                    else if (allEnemies[i].GetType() == typeof(Commander))
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
            if (isPause == false)
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
                this.resumeBtn = new Button();
                canvas.Children.Add(resumeBtn);
                resumeBtn.Width = 300;
                resumeBtn.Height = 50;
                Canvas.SetLeft(resumeBtn, 300);
                Canvas.SetTop(resumeBtn, 200);
                resumeBtn.Content = "RESUME GAME";
                resumeBtn.FontSize = 25;
                resumeBtn.FontWeight = FontWeights.Bold;
                resumeBtn.Background = Brushes.DimGray;
                resumeBtn.Click += playPauseBtn_Click;

                this.saveBtn = new Button();
                canvas.Children.Add(saveBtn);
                saveBtn.Width = 300;
                saveBtn.Height = 50;
                Canvas.SetLeft(saveBtn, 300);
                Canvas.SetTop(saveBtn, 272);
                saveBtn.Content = "SAVE GAME";
                saveBtn.FontSize = 25;
                saveBtn.FontWeight = FontWeights.Bold;
                saveBtn.Background = Brushes.DimGray; ;
                saveBtn.Click += saveBtn_Click;

                this.loadBtn = new Button();
                canvas.Children.Add(loadBtn);
                loadBtn.Width = 300;
                loadBtn.Height = 50;
                Canvas.SetLeft(loadBtn, 300);
                Canvas.SetTop(loadBtn, 345);
                loadBtn.Content = "LOAD NEW GAME";
                loadBtn.FontSize = 25;
                loadBtn.FontWeight = FontWeights.Bold;
                loadBtn.Background = Brushes.DimGray;
                loadBtn.Click += loadBtn_Click;
            }

            if (action.Equals("remove"))
            {
                canvas.Children.Remove(this.resumeBtn);
                canvas.Children.Remove(this.saveBtn);
                canvas.Children.Remove(this.loadBtn);
            }
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            var gameWindow = new GameWindow(true);
            this.Hide();
            gameWindow.Show();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (load)
            {
                if (LoadLevels.getStaticPlayer().getCurrentLevel() == 1)
                {
                    lv1.saveLevel1();

                }
                else if (LoadLevels.getStaticPlayer().getCurrentLevel() == 2)
                {
                    Level2.saveLevel2(this.player, false);
                }
            }
            else
            {

                if (player.getCurrentLevel() == 1)
                {
                    lv1.saveLevel1();

                }
                else if (player.getCurrentLevel() == 2)
                {
                    Level2.saveLevel2(this.player, false);
                }

            }
        }
    }
}
