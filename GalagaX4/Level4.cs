using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GalagaX4
{
    /// <summary>
    /// The Level4 Class instantiates a new Game,
    /// creating all the elements on the screen 
    /// necessary to play the game such as the Player and 
    /// enemies and also creates the final Boss Enemy.
    /// It also creates all the patherns of the 
    /// enemies and of the boss for the last level of the game.
    /// </summary>
    class Level4
    {
        Window window;
        Canvas canvas;
        bool load;
        int round = 1;
        static DispatcherTimer timerRandomShoot;
        int spaceX = 0;

        Player player;

        SpaceShip[] ships;
        SpaceShip[] ufos;
        Commander[] commanders;
        List<int> shipsNum = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        List<int> commandersNum = new List<int>() { 0, 1, 2, 3 };
        List<int> ufosNum = new List<int>() { 0, 1, 2, 3, 4 };
        bool exists1 = false;
        bool exists2 = false;
        bool exists3 = false;
        List<Enemies> enemies;

        Boss boss;
        Image lv4Pic;
        /// <summary>
        /// Level4 Class Constructor. It cosntructs the new window (screen),
        /// a new Canvas and the Player.
        /// </summary>
        /// <param name="window"> A new window to receive all elements fo the game and manage the screen</param>
        /// <param name="canvas">Area within the window which you can position all elements by using coordinates that are relative to the Canvas area.</param>
        /// <param name="player">The main player of the Game</param>
        public Level4(Window window, Canvas canvas, Player player)
        {
            this.window = window;
            this.canvas = canvas;
            this.player = player;
            if(player != null)
            {
                this.player.updateCurrentLevel(4);
            }
            enemies = new List<Enemies>();
        }

        /// <summary>
        /// The static timerRandom method returns 
        /// a DispatcherTimer Object related to the random shooting.
        /// </summary>
        public static DispatcherTimer timerRandom
        {
            get { return timerRandomShoot; }
        }
        /// <summary>
        /// The DisplayLevel method displays an image on the canvas indicating the Level of the game.
        /// </summary>
        void DisplayLevel()
        {
            lv4Pic = new Image();
            lv4Pic.Height = 50;
            lv4Pic.Width = 140;
            this.canvas.Children.Add(lv4Pic);
            Canvas.SetTop(lv4Pic, 200);
            Canvas.SetLeft(lv4Pic, 350);
            lv4Pic.Source = UtilityMethods.LoadImage("pics/level4.png");
        }
        /// <summary>
        /// The Play Method creates and displays all enemies and the player on 
        /// the Canvas (screen).
        /// </summary>
        public async void Play()
        {
            Player.ColdDown = 0;

            if (this.round == 1)
            {
                DisplayLevel();
                await Task.Delay(2000);
                this.canvas.Children.Remove(lv4Pic);
            }
            else
            {
                this.shipsNum = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
                this.commandersNum = new List<int>() { 0, 1, 2, 3 };
                this.ufosNum = new List<int>() { 0, 1, 2, 3, 4 };
                this.exists1 = false;
                this.exists2 = false;
                this.exists3 = false;
                this.spaceX = 0;

                await Task.Delay(1500);
            }

            if (this.round == 1)
            {
                lv4Pic.Height = 40;
                lv4Pic.Width = 100;
                this.canvas.Children.Add(lv4Pic);
                Canvas.SetLeft(lv4Pic, 0);
                Canvas.SetTop(lv4Pic, 0);
                lv4Pic.Source = UtilityMethods.LoadImage("pics/level4.png");
            }

            //bee creation
            BitmapImage[] beeImages = { UtilityMethods.LoadImage("pics/bee0.png"),
                    UtilityMethods.LoadImage("pics/bee1.png") };
            Image[] beesPic = new Image[8];
            //int spaceX = 0;
            bool isDive = false;
            Bug[] bees = new Bug[beesPic.Length];
            for (int i = 0; i < beesPic.Length; i++)
            {
                beesPic[i] = new Image();
                beesPic[i].Width = 34;
                beesPic[i].Height = 26;
                canvas.Children.Add(beesPic[i]);
                Canvas.SetLeft(beesPic[i], 181 + spaceX);
                Canvas.SetTop(beesPic[i], 185);
                spaceX += 60;

                Point beePos = new Point();
                beePos.X = Canvas.GetLeft(beesPic[i]);
                beePos.Y = Canvas.GetTop(beesPic[i]);
                Animation beeAnimation = new Animation(beesPic[i], beeImages, true);
                Bug bee = new Bug(beePos, beesPic[i], canvas, beeAnimation);
                bees[i] = bee;
                enemies.Add(bees[i]);
                bees[i].setTarget(player);
                bees[i].setDive(isDive);

                if (isDive) isDive = false;
                else isDive = true;

                bees[i].setDiveFrequency(10);
                bees[i].setMoveDownFrequency(45);
                bees[i].Fly(120);
            }

            //---------------------------------------------------------------------------
            //redbugs creation
            BitmapImage[] bugImages = { UtilityMethods.LoadImage("pics/redBug0.png"),
                    UtilityMethods.LoadImage("pics/redBug1.png") };
            Image[] bugsPic = new Image[9];
            Bug[] redbugs = new Bug[bugsPic.Length];
            spaceX = 0;
            isDive = false;
            for (int i = 0; i < bugsPic.Length; i++)
            {
                bugsPic[i] = new Image();
                bugsPic[i].Width = 34;
                bugsPic[i].Height = 26;
                canvas.Children.Add(bugsPic[i]);
                Canvas.SetLeft(bugsPic[i], 154 + spaceX);
                Canvas.SetTop(bugsPic[i], 145);
                spaceX += 60;

                Point bugPos = new Point();
                bugPos.X = Canvas.GetLeft(bugsPic[i]);
                bugPos.Y = Canvas.GetTop(bugsPic[i]);
                Animation bugAnimation = new Animation(bugsPic[i], bugImages, true);
                Bug bug = new Bug(bugPos, bugsPic[i], canvas, bugAnimation);
                redbugs[i] = bug;
                enemies.Add(redbugs[i]);
                redbugs[i].setTarget(player);
                redbugs[i].setDive(isDive);

                if (isDive) isDive = false;
                else isDive = true;

                redbugs[i].setDiveFrequency(10);
                redbugs[i].setMoveDownFrequency(45);
                redbugs[i].setMoveCounter(2);
                redbugs[i].Fly(120);
            }

            //---------------------------------------------------------------------------
            //ships creation
            Image[] shipsPic = new Image[8];
            ships = new SpaceShip[shipsPic.Length];
            isDive = false;
            spaceX = 0;
            for (int i = 0; i < ships.Length; i++)
            {
                shipsPic[i] = new Image();
                shipsPic[i].Source = UtilityMethods.LoadImage("pics/spaceShip.png");
                shipsPic[i].Width = 34;
                shipsPic[i].Height = 26;
                canvas.Children.Add(shipsPic[i]);
                Canvas.SetLeft(shipsPic[i], 181 + spaceX);
                Canvas.SetTop(shipsPic[i], 105);
                spaceX += 60;

                Point shipPos = new Point();
                shipPos.X = Canvas.GetLeft(shipsPic[i]);
                shipPos.Y = Canvas.GetTop(shipsPic[i]);

                SpaceShip ship = new SpaceShip(shipPos, shipsPic[i], canvas);
                ships[i] = ship;
                enemies.Add(ships[i]);
                ships[i].setTarget(player);
                ships[i].setDive(isDive);

                if (isDive) isDive = false;
                else isDive = true;

                ships[i].setDiveFrequency(10);
                ships[i].setMoveDownFrequency(45);
                ships[i].Fly(120);
                // ships[i].Shoot(200);
            }

            //---------------------------------------------------------------------------
            //UFO creation
            Image[] ufoPics = new Image[5];
            this.ufos = new SpaceShip[ufoPics.Length];
            isDive = false;
            spaceX = 0;
            for (int i = 0; i < ufoPics.Length; i++)
            {
                ufoPics[i] = new Image();
                ufoPics[i].Source = UtilityMethods.LoadImage("pics/UFO.png");
                ufoPics[i].Width = 34;
                ufoPics[i].Height = 26;
                canvas.Children.Add(ufoPics[i]);
                Canvas.SetLeft(ufoPics[i], 200 + spaceX);
                Canvas.SetTop(ufoPics[i], 65);
                spaceX += 60;

                Point ufoPos = new Point();
                ufoPos.X = Canvas.GetLeft(ufoPics[i]);
                ufoPos.Y = Canvas.GetTop(ufoPics[i]);

                SpaceShip ufo = new SpaceShip(ufoPos, ufoPics[i], canvas);
                ufos[i] = ufo;
                enemies.Add(ufos[i]);
                ufos[i].setTarget(player);
                ufos[i].setMoveCounter(2);
                ufos[i].setDive(isDive);

                if (isDive) isDive = false;
                else isDive = true;

                ufos[i].setDiveFrequency(10);
                ufos[i].setMoveDownFrequency(45);
                ufos[i].Fly(120);
                // ships[i].Shoot(200);
            }

            //---------------------------------------------------------------------------
            //commanders creation
            BitmapImage[] commanderImages = { UtilityMethods.LoadImage("pics/commander.png"),
                                        UtilityMethods.LoadImage("pics/commander2.png") };
            Image[] commanderPic = new Image[4];
            commanders = new Commander[commanderPic.Length];
            spaceX = 0;
            for (int i = 0; i < commanderPic.Length; i++)
            {
                commanderPic[i] = new Image();
                //commanderPic[i].Source = UtilityMethods.LoadImage("pics/spaceShip.png");
                commanderPic[i].Width = 34;
                commanderPic[i].Height = 26;
                canvas.Children.Add(commanderPic[i]);
                Canvas.SetLeft(commanderPic[i], 260 + spaceX);
                Canvas.SetTop(commanderPic[i], 30);
                spaceX += 90;

                Point commanderPos = new Point();
                commanderPos.X = Canvas.GetLeft(commanderPic[i]);
                commanderPos.Y = Canvas.GetTop(commanderPic[i]);
                Animation commanderAnimation = new Animation(commanderPic[i], commanderImages, true);
                Commander commander = new Commander(commanderPos, commanderPic[i], canvas, commanderAnimation);
                commanders[i] = commander;
                enemies.Add(commanders[i]);
                commanders[i].setTarget(player);
                commanders[i].setMoveDownFrequency(45);
                commanders[i].Fly(120);
            }

            if (this.round == 5)
            {
                displayBoss();
            }

            player.SetEnemyTarget(enemies);
            StartGame();
        }
        /// <summary>
        /// The StartGame method instantiates a DispatcherTimer Class which 
        /// will be used to control the interval of all enemies shooting execution 
        /// in the game of this specific level.
        /// </summary>
        void StartGame()
        {
            timerRandomShoot = new DispatcherTimer(DispatcherPriority.Normal);
            timerRandomShoot.Interval = TimeSpan.FromSeconds(1);
            timerRandomShoot.Tick += new EventHandler(ShootUpdate);
            timerRandomShoot.Start();
        }
        /// <summary>
        /// The shootUpdate method controls randomly the
        /// shooting of the enemies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShootUpdate(object sender, EventArgs e)
        {
            //-------------------------------
            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i].IsDead() == true)
                {
                    shipsNum.Remove(i);
                }
            }

            for (int i = 0; i < commanders.Length; i++)
            {
                if (commanders[i].IsDead() == true)
                {
                    commandersNum.Remove(i);
                }
            }

            for (int i = 0; i < ufos.Length; i++)
            {
                if (ufos[i].IsDead() == true)
                {
                    ufosNum.Remove(i);
                }
            }

            //--------------------------------
            Random rand = new Random();
            int num1 = rand.Next(ships.Length);
            if (shipsNum.Contains(num1))
            {
                shipsNum.Remove(num1);
                exists1 = true;
            }
            if (exists1 == true)
            {
                if (ships[num1].IsDead() == true)
                {
                    shipsNum.Remove(num1);
                }
                else
                {
                    if (ships[num1].isShoot() == false)
                    {
                        ships[num1].Shoot(4);
                    }
                }

                exists1 = false;
            }

            //------------------------------------
            int num2 = rand.Next(commanders.Length);
            if (commandersNum.Contains(num2))
            {
                exists2 = true;
                commandersNum.Remove(num2);
            }
            if (exists2 == true)
            {
                if (commanders[num2].IsDead() == true)
                {
                    commandersNum.Remove(num2);
                }
                else
                {
                    if (commanders[num2].isShoot() == false)
                    {
                        commanders[num2].Shoot(8);
                    }
                }
                exists2 = false;
            }

            //------------------------------------
            int num3 = rand.Next(ufos.Length);
            if (ufosNum.Contains(num3))
            {
                exists3 = true;
                ufosNum.Remove(num3);
            }
            if (exists3 == true)
            {
                if (ufos[num3].IsDead() == true)
                {
                    ufosNum.Remove(num3);
                }
                else
                {
                    if (ufos[num3].isShoot() == false)
                    {
                        ufos[num3].Shoot(2);
                    }
                }
                exists3 = false;
            }

            //----------------------------------------
            if (this.enemies.Count <= 1)
            {
                timerRandomShoot.Stop();
                if (this.round >= 1 && this.round <= 4)
                {
                    round++;
                    if (this.round != 4)
                    {
                        Play();
                    }
                    else
                    {
                        displayBoss();
                    }
                }
                else
                {
                    this.canvas.Children.Remove(lv4Pic);

                    Image missionAccomplished = new Image();
                    missionAccomplished.Height = 300;
                    missionAccomplished.Width = 500;
                    this.canvas.Children.Add(missionAccomplished);
                    Canvas.SetTop(missionAccomplished, 150);
                    Canvas.SetLeft(missionAccomplished, 170);
                    BitmapImage[] missionAccomplishedSources = { UtilityMethods.LoadImage("pics/missAccomplised_blue.png")
                                , UtilityMethods.LoadImage("pics/missAccomplised_white.png") };
                    Animation missionAccomplishedAnim = new Animation(missionAccomplished, missionAccomplishedSources, true);
                    Animation.Initiate(missionAccomplishedAnim, 100);
                    //missAccomplished.Source = UtilityMethods.LoadImage("pics/level4.png");
                }
            }
        }
        /// <summary>
        /// The displayBoss method creates the final Boss enemy and
        /// defines all its movements and shooting patherns.
        /// </summary>
        private void displayBoss()
        {
            Point bossPos = new Point();
            bossPos.X = 300;
            bossPos.Y = 100;
            Animation bossAnimation;
            Image bossPic = new Image();
            bossPic.Height = 120;
            bossPic.Width = 140;
            bossPic.Source = UtilityMethods.LoadImage("pics/boss/boss.png");
            canvas.Children.Add(bossPic);
            Canvas.SetLeft(bossPic, 200);

            BitmapImage[] bossImages = { UtilityMethods.LoadImage("pics/boss/boss1.png"),
                    UtilityMethods.LoadImage("pics/boss/boss2.png"), UtilityMethods.LoadImage("pics/boss/boss3.png"),};
            bossAnimation = new Animation(bossPic, bossImages, true);
            this.boss = new Boss(bossPos, bossPic, canvas, bossAnimation);
            Animation.Initiate(bossAnimation, 500);

            enemies.Add(boss);
            boss.setTarget(player);
            this.boss.Fly(200);
            this.boss.Shoot(2);
        }

        public static void saveLevel4(Player player, bool load)
        {
            String fileName = "GalagaSavedGame.bin";
            BinaryFormatter formatter = null;
            Stream stream = null;
            SerializeGameObj game = null;

          
            int coins;
            int lives;
            int level;

            if (load)
            {
                coins = LoadLevels.getStaticPlayer().getCoins();
                lives = LoadLevels.getStaticPlayer().GetLives();
                level = LoadLevels.getStaticPlayer().getCurrentLevel();
            }
            else
            {
                coins = player.getCoins();
                lives = player.GetLives();
                level = player.getCurrentLevel();
            }

            game = new SerializeGameObj(coins, lives, level);

            try
            {
                formatter = new BinaryFormatter();
                stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, game);
                stream.Close();
                MessageBox.Show("Game has been saved");
            }
            catch (SerializationException e)
            {
                MessageBox.Show("An error occured and the current game was not able to be saved.");
                MessageBox.Show(e.Message);
            }
        }

        public void setRound(int round)
        {
            this.round = round;
        }
        public void setLoad(bool load)
        {
            this.load = load;
        }

        public static void loadLevel4(Canvas canvas, Window window, Player player)
        {
            String fileName = "GalagaSavedGame.bin";
            BinaryFormatter reader = null;
            Stream stream = null;
            SerializeGameObj game = null;
           

            try
            {
                reader = new BinaryFormatter();
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

                game = (SerializeGameObj)reader.Deserialize(stream);


                LoadLevels loadlvl1 = new LoadLevels(game.GetCoins, game.GetLives, game.GetLevel);
                player = loadlvl1.getPlayer();
            }
            catch (SerializationException e)
            {
                MessageBox.Show("An error occured and the current game was not able to be LOADED.");
                MessageBox.Show(e.Message);
            }



        }

    }
}
