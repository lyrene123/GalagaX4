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
    /// Interaction logic for GameWindow.xaml. 
    /// The GameWindow class sets the galaga game by creating the player,
    /// by setting the background music and by calling the level 1 to start off
    /// the play. The GameWindow class implements the codes that will handle
    /// events such as the pause and play button click and the regular check of
    /// extra life to give to the player if necessary
    /// </summary>
    public partial class GameWindow : Window
    {
        DispatcherTimer coldDownTimer;
        DispatcherTimer checkLife; //timer to check if player needs more life or not
        DispatcherTimer lifeTimer; //timer to make the life icon appear
        Player player;
        bool isPause;
        Button resume;
        Button save;
        Button load;
        Image life;
        bool loaded;
        Level1 lv1;
        GameSound sound = new GameSound(@"pack://application:,,,/GalagaX4;Component/audio/Game_Over.wav", true);
        /// <summary>
        /// The GameWindow constructor sets the overral initial state of the game
        /// by creating the player, by setting the background music, the background image
        /// and by calling the first level of the game
        /// </summary>
        public GameWindow()
        {
            this.Closed += GameWindow_Closed;
            this.Closing += GameWindow_Closing;

            InitializeComponent();
            backgroundImage.Width = 860;
            backgroundImage.Height = 650;
            mediaElement.Source = new Uri("audio/main2.wav", UriKind.Relative);
            mediaElement.BeginInit();
            mediaElement.Position = TimeSpan.FromSeconds(1); ;
            mediaElement.Volume = 0.07;
            mediaElement.Play();
            mediaElement.MediaEnded += new RoutedEventHandler(Element_MediaEnded);

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

            lv1 = new Level1(this, canvas, player);
            lv1.Play();
            //Level2 lv2 = new Level2(this, canvas, player);
            //lv2.Play();
            //Level3 lv3 = new Level3(this, canvas, player);
            //lv3.Play();
           // Level4 lv4 = new Level4(this, canvas, player);
            //lv4.Play();

            KeyDown += new KeyEventHandler(MyGrid_KeyDown);
            // buyLives();

            DecrementColdDown();
        }
        public GameWindow(Boolean load)
        {
            this.Closed += GameWindow_Closed;
            this.Closing += GameWindow_Closing;

            InitializeComponent();
            this.loaded = load;

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
           

            if (loaded == true)
            {
              
                    lv1 = new Level1(this, canvas, player, true);
           
            }
            else
            {
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
                lv1 = new Level1(this, canvas, player);
                lv1.Play();
            }
            // Level4 lv4 = new Level4(this, canvas, player);
            //lv4.Play();

            KeyDown += new KeyEventHandler(MyGrid_KeyDown);

            DecrementColdDown();
        }

        /// <summary>
        /// The buyLives method initiates the checkLife timer and will
        /// call the giveLife event method handler
        /// </summary>
        public void buyLives()
        {
            this.checkLife = new DispatcherTimer(DispatcherPriority.Normal);
            checkLife.Interval = TimeSpan.FromMinutes(1);
            checkLife.Tick += new EventHandler(giveLife);
            checkLife.Start();
        }

        /// <summary>
        /// giveLife method event handler handles the regular checking 
        /// if the player is in need of an extra life. If it's the case
        /// then, the method will initiate the lifeTimer timer and will call
        /// the sendLife method handler event
        /// </summary>
        /// <param name="sender">object raising the event</param>
        /// <param name="e">the giveLife event raised </param>
        private void giveLife(object sender, EventArgs e)
        {
            if (player.GetLives() < 2)
            {
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

        /// <summary>
        /// sendLife method handles the provision of an extra life for the player.
        /// An image of a spaceship will appear and the method will make it move down
        /// in order for the player to have the choice to take it by checking any
        /// collision
        /// </summary>
        /// <param name="sender">object that raised the event</param>
        /// <param name="e">the sendLife event that was raised</param>
        private void sendLife(object sender, EventArgs e)
        {
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

                //check if the player touched the extra life image
                if (rectPlayer.IntersectsWith(rectLife))
                {
                    this.lifeTimer.Stop();
                    if (player.getCoins() >= 200 && player.GetLives() < 2)
                    {
                        player.addLife();
                    }
                    canvas.Children.Remove(this.life);
                }
            }
            else
            {
                canvas.Children.Remove(this.life);
            }
        }

        /// <summary>
        /// GameWindow_Closing method closes the game window by exiting the environment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void MyGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.isPause == false)
            {
                if (loaded)
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
                if (loaded)
                {
                    if (LoadLevels.getStaticPlayer().getEnemiesSize() != 0 && this.isPause == false)
                    {
                        LoadLevels.getStaticPlayer().Shoot();
                    }
                }
                else
                {
                    if (player.getEnemiesSize() != 0 && this.isPause == false)
                    {
                        player.Shoot();
                    }
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
            if (loaded)
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
                    mediaElement.Stop();
                    mediaElement.Source = null;
                    LoadLevels.getStaticPlayer().shootSoundEffect.StopSound();
                    LoadLevels.getStaticPlayer().shootSoundEffect.Dispose();
                    sound.playSoundLooping();
                    sound.Dispose();
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
                    mediaElement.Stop();
                    mediaElement.Source = null;
                    player.shootSoundEffect.StopSound();
                    player.shootSoundEffect.Dispose();
                    sound.playSoundLooping();
                    sound.Dispose();
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
            // this.lifeTimer.Start();
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
            if (loaded == true)
            {
                if (LoadLevels.getStaticPlayer().getCurrentLevel() == 1)
                {
                    if (Level1.timerRandom != null)
                    {
                        Level1.timerRandom.Stop();
                    }
                }
                if (LoadLevels.getStaticPlayer().getCurrentLevel() == 2)
                {
                    if (Level2.timerRandom != null)
                    {
                        Level2.timerRandom.Stop();
                    }
                }
                if (LoadLevels.getStaticPlayer().getCurrentLevel() == 3)
                {
                    if (Level3.timerRandom != null)
                    {
                        Level3.timerRandom.Stop();
                    }
                }
                if (LoadLevels.getStaticPlayer().getCurrentLevel() == 4)
                {
                    if (Level4.timerRandom != null)
                    {
                        Level4.timerRandom.Stop();
                    }
                }
            }
            else
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
                if (this.player.getCurrentLevel() == 4)
                {
                    if (Level4.timerRandom != null)
                    {
                        Level4.timerRandom.Stop();
                    }
                }
            }
            List<Enemies> allEnemies = new List<Enemies>();
            if (loaded)
            {
                allEnemies = LoadLevels.getStaticPlayer().getEnemiesList();

            }
            else
            {
                allEnemies = this.player.getEnemiesList();
            }
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
            var gameWindow = new GameWindow(true);
            this.Hide();
            gameWindow.Show();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
           // MessageBox.Show(""+LoadLevels.getStaticPlayer().getCurrentLevel());
            //MessageBox.Show("" + player.getCurrentLevel());
            if (loaded)
            {
                if (LoadLevels.getStaticPlayer().getCurrentLevel() == 1)
                {
                    lv1.saveLevel1();
                }
                else if (LoadLevels.getStaticPlayer().getCurrentLevel() == 2)
                {
                    Level2.saveLevel2(this.player, true);
                }
                else if (LoadLevels.getStaticPlayer().getCurrentLevel() == 3)
                {
                    Level3.saveLevel3(this.player, true);
                }
                else if (LoadLevels.getStaticPlayer().getCurrentLevel() == 4)
                {
                    Level4.saveLevel4(this.player, true);
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
                else if (player.getCurrentLevel() == 3)
                {
                    Level3.saveLevel3(this.player, false);
                }
                else if (player.getCurrentLevel() == 4)
                {
                    Level4.saveLevel4(this.player, false);
                }

            }

        }
    }
}
