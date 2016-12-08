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

            KeyDown += new KeyEventHandler(MyGrid_KeyDown);

            DecrementColdDown();
        }
        /// <summary>
        /// GameWindow overloaded constructor for the loaded version 
        /// game after saving
        /// </summary>
        /// <param name="load"></param>
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
            mediaElement.Volume = 0.07;
            mediaElement.Play();
            mediaElement.MediaEnded += new RoutedEventHandler(Element_MediaEnded);
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

            KeyDown += new KeyEventHandler(MyGrid_KeyDown);

            DecrementColdDown();
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
        /// GameWindow_Closed method shuts down the whole application
        /// </summary>
        /// <param name="sender">Object raising the GameWindow_Closed event</param>
        /// <param name="e">GameWindow_Closed event raised</param>
        private void GameWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// MyGrid_KeyDown method event handler handles the event raised
        /// if the space bar is pressed, the arrow keys are pressed and the
        /// cold down progress bar
        /// </summary>
        /// <param name="sender">Object raising the event </param>
        /// <param name="e">MyGrid_KeyDown raised event</param>
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

        /// <summary>
        /// DecrementColdDown method initialized the coldDownTimer timer
        /// for the cold down progress bar
        /// </summary>
        void DecrementColdDown()
        {
            coldDownTimer = new DispatcherTimer(DispatcherPriority.Normal);
            coldDownTimer.Interval = TimeSpan.FromMilliseconds(500);
            coldDownTimer.Tick += new EventHandler(DecrementColdDown);
            coldDownTimer.Start();
        }

        /// <summary>
        /// DecrementColdDown event handler method handles the 
        /// event the progress bar value is changed related to the
        /// shooting of the player. If the progress bar is completed,
        /// the player shooting is limited, if it decreases, the shooting
        /// of the player is increased.
        /// </summary>
        /// <param name="sender">Object raising the event</param>
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

        /// <summary>
        /// GameOver method checks if the player has no more lives left
        /// and if it's the case, then the game over display and sound will
        /// initialize and execute
        /// </summary>
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
                player.shootSoundEffect.StopSound();
                player.shootSoundEffect.Dispose();
                sound.playSoundLooping();
                sound.Dispose();
                BackToMainWindow();
            }
        }
        /// <summary>
        /// BackToMainWindow method returns to the main GalagaX4 window
        /// </summary>
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

        /// <summary>
        /// Element_MediaEnded method plays a particular music
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(1);
            mediaElement.Play();
        }

        private void Element_MediaOpened(object sender, RoutedEventArgs e)
        {

            //mediaElement.Play();
        }

        /// <summary>
        /// playBtn_Click method resumes the current movement, shooting of 
        /// player and enemies after pausing the game and will remove the menu
        /// displayed on the screen
        /// </summary>
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

        /// <summary>
        /// pauseBtn_Click method pauses all movement, shooting happening 
        /// on the game canvas after the pause button is clicked
        /// </summary>
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

        /// <summary>
        /// playPauseBtn_Click method checks if the pause/play has been
        /// clicked once or twice in order to know if the user wants to play
        /// the game of to pause the game
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">the event raised playPauseBtn_Click</param>
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
            if (loaded)
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
