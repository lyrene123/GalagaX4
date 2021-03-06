﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GalagaX4
{
    /// <summary>
    /// The Level1 Class instantiates a new Game,
    /// creating all the elements on the screen 
    /// necessary to play the game such as the Player and 
    /// enemies. It also creates all the patherns of the 
    /// enemies for level1 of the game.
    /// </summary>
    class Level1
    {
        Window window;
        Canvas canvas;
        bool load = false;

        static DispatcherTimer timerRandomShoot;
        int spaceX = 0;

        Player player;

        SpaceShip[] ships;
        Commander[] commanders;
        List<int> arr1 = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        List<int> arr2 = new List<int>() { 0, 1, 2, 3 };
        bool exists1 = false;
        bool exists2 = false;
        public bool levelOver = false;

        public List<Enemies> enemies;
        Image lv1Pic;

        /// <summary>
        /// Level1 Class Constructor. It cosntructs the new window (screen),
        /// a new Canvas and the Player.
        /// </summary>
        /// <param name="window"> A new window to receive all elements fo the game and manage the screen</param>
        /// <param name="canvas">Area within the window which you can position all elements by using coordinates that are relative to the Canvas area.</param>
        /// <param name="player">The main player of the Game</param>
        public Level1(Window window, Canvas canvas, Player player)
        {
            this.window = window;
            this.canvas = canvas;
            this.player = player;
            enemies = new List<Enemies>();
            this.player.updateCurrentLevel(1);

        }
        public Level1(Window window, Canvas canvas, Player player, Boolean load)
        {
            this.window = window;
            this.canvas = canvas;
            this.load = load;
            enemies = new List<Enemies>();
            if (load == true)
            {
                Level1.loadLevel1(this.canvas, this.window, player);
                this.player = player;
            }
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
            lv1Pic = new Image();
            lv1Pic.Height = 50;
            lv1Pic.Width = 140;
            this.canvas.Children.Add(lv1Pic);
            Canvas.SetTop(lv1Pic, 200);
            Canvas.SetLeft(lv1Pic, 350);
            lv1Pic.Source = UtilityMethods.LoadImage("pics/level1.png");
        }
        /// <summary>
        /// The Play Method creates and displays all enemies and the player on 
        /// the Canvas (screen).
        /// </summary>
        public async void Play()
        {
            Player.ColdDown = 0;

            DisplayLevel();
            await Task.Delay(2000);
            this.canvas.Children.Remove(lv1Pic);


            lv1Pic = new Image();
            lv1Pic.Height = 40;
            lv1Pic.Width = 100;
            this.canvas.Children.Add(lv1Pic);
            lv1Pic.Source = UtilityMethods.LoadImage("pics/level1.png");

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

                bees[i].Fly(300);
            }

            //---------------------------------------------------------------------------
            //red bugs creation
            BitmapImage[] bugImages = { UtilityMethods.LoadImage("pics/redBug0.png"),
                    UtilityMethods.LoadImage("pics/redBug1.png") };
            Image[] bugsPic = new Image[9];
            isDive = false;
            Bug[] redbugs = new Bug[bugsPic.Length];
            spaceX = 0;
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

                redbugs[i].Fly(300);
            }

            //---------------------------------------------------------------------------
            //spaceships creation
            Image[] shipsPic = new Image[8];
            ships = new SpaceShip[shipsPic.Length];
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
                ships[i].Fly(300);

            }

            //----------------------------------------------------------------------------
            //commanders creation
            BitmapImage[] commanderImages = { UtilityMethods.LoadImage("pics/commander.png"),
                UtilityMethods.LoadImage("pics/commander2.png") };
            Image[] commanderPic = new Image[4];
            commanders = new Commander[commanderPic.Length];
            spaceX = 0;
            for (int i = 0; i < commanderPic.Length; i++)
            {
                commanderPic[i] = new Image();
                commanderPic[i].Width = 34;
                commanderPic[i].Height = 26;
                canvas.Children.Add(commanderPic[i]);
                Canvas.SetLeft(commanderPic[i], 260 + spaceX);
                Canvas.SetTop(commanderPic[i], 65);
                spaceX += 90;

                Point commanderPos = new Point();
                commanderPos.X = Canvas.GetLeft(commanderPic[i]);
                commanderPos.Y = Canvas.GetTop(commanderPic[i]);
                Animation commanderAnimation = new Animation(commanderPic[i], commanderImages, true);
                Commander commander = new Commander(commanderPos, commanderPic[i], canvas, commanderAnimation);
                commanders[i] = commander;
                enemies.Add(commanders[i]);
                commanders[i].setTarget(player);
                commanders[i].Fly(300);
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
            //remove enemies destroyed from the arr1 or arr2 list
            //-------------------------------
            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i].IsDead() == true)
                {
                    arr1.Remove(i);
                }
            }

            for (int i = 0; i < commanders.Length; i++)
            {
                if (commanders[i].IsDead() == true)
                {
                    arr2.Remove(i);
                }
            }

            //--------------------------------
            Random rand = new Random();
            int num1 = rand.Next(ships.Length);//8      
            if (arr1.Contains(num1))
            {
                arr1.Remove(num1);
                exists1 = true;
            }
            if (exists1 == true)
            {
                if (ships[num1].IsDead() == true)
                {
                    arr1.Remove(num1);
                }
                else
                {
                    if (ships[num1].isShoot() == false)
                    {
                        ships[num1].Shoot(10);
                    }
                }

                exists1 = false;
            }

            //------------------------------------
            int num2 = rand.Next(4);
            if (arr2.Contains(num2))
            {
                exists2 = true;
                arr2.Remove(num2);
            }
            if (exists2 == true)
            {
                if (commanders[num2].IsDead() == true)
                {
                    arr2.Remove(num2);
                }
                else
                {
                    if (commanders[num2].isShoot() == false)
                    {
                        commanders[num2].Shoot(20);
                    }
                }
                exists2 = false;
            }

            //----------------------------------------
            if (this.enemies.Count == 0)
            {
                timerRandomShoot.Stop();

                this.canvas.Children.Remove(lv1Pic);

                Level2 lv2 = new Level2(this.window, this.canvas, this.player);
                lv2.Play();

            }
        }
        public void saveLevel1()
        {
            String fileName = "GalagaSavedGame.bin";
            BinaryFormatter formatter = null;
            Stream stream = null;
            SerializeGameObj game = null;

            List<int> shipInt = new List<int>();
            List<int> commanderInt = new List<int>();
            List<int> bugInt = new List<int>();

            List<Point> shipPoint = new List<Point>();
            List<Point> commanderPoint = new List<Point>();
            List<Point> bugPoint = new List<Point>();

            List<String> shipPath = new List<string>();
            List<String> commanderPath = new List<string>();
            List<String> bugPath = new List<string>();
            List<double> minXShip = new List<double>();
            List<double> maxXShip = new List<double>();
            List<double> minXCom = new List<double>();
            List<double> maxXCom = new List<double>();
            List<double> minXBug = new List<double>();
            List<double> maxXBug = new List<double>();
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

            if (load == false)
            {
                for (int i = 0; i < player.getEnemiesList().Count; i++)
                {
                    if (player.getEnemiesList()[i].GetType() == typeof(SpaceShip))
                    {
                        shipInt.Add(0);
                        shipPoint.Add(player.getEnemiesList()[i].GetPoint());
                        shipPath.Add(player.getEnemiesList()[i].GetImage().Source.ToString());
                        maxXShip.Add(player.getEnemiesList()[i].getMaxX());
                        minXShip.Add(player.getEnemiesList()[i].getMinX());
                    }
                    else if (player.getEnemiesList()[i].GetType() == typeof(Commander))
                    {
                        commanderInt.Add(1);
                        commanderPoint.Add(player.getEnemiesList()[i].GetPoint());
                        commanderPath.Add(player.getEnemiesList()[i].GetImage().Source.ToString());
                        maxXCom.Add(player.getEnemiesList()[i].getMaxX());
                        minXCom.Add(player.getEnemiesList()[i].getMinX());
                    }
                    else if (player.getEnemiesList()[i].GetType() == typeof(Bug))
                    {
                        bugInt.Add(2);
                        bugPoint.Add(player.getEnemiesList()[i].GetPoint());
                        bugPath.Add(player.getEnemiesList()[i].GetImage().Source.ToString());
                        maxXBug.Add(player.getEnemiesList()[i].getMaxX());
                        minXBug.Add(player.getEnemiesList()[i].getMinX());
                    }
                }

            }
            else if (load)
            {
                for (int i = 0; i < LoadLevels.getStaticPlayer().getEnemiesList().Count; i++)
                {
                    if (LoadLevels.getStaticPlayer().getEnemiesList()[i].GetType() == typeof(SpaceShip))
                    {
                        shipInt.Add(0);
                        shipPoint.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].GetPoint());
                        shipPath.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].GetImage().Source.ToString());
                        maxXShip.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].getMaxX());
                        minXShip.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].getMinX());
                    }
                    else if (LoadLevels.getStaticPlayer().getEnemiesList()[i].GetType() == typeof(Commander))
                    {
                        commanderInt.Add(1);
                        commanderPoint.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].GetPoint());
                        commanderPath.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].GetImage().Source.ToString());
                        maxXCom.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].getMaxX());
                        minXCom.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].getMinX());
                    }

                    else if (LoadLevels.getStaticPlayer().getEnemiesList()[i].GetType() == typeof(Bug))
                    {
                        bugInt.Add(2);
                        bugPoint.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].GetPoint());
                        bugPath.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].GetImage().Source.ToString());
                        maxXBug.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].getMaxX());
                        minXBug.Add(LoadLevels.getStaticPlayer().getEnemiesList()[i].getMinX());

                    }
                }

            }


            game = new SerializeGameObj(shipInt, shipPoint, shipPath, commanderInt, commanderPoint, commanderPath,
                bugInt, bugPoint, bugPath, minXShip, maxXShip, minXCom, maxXCom, minXBug, maxXBug,  coins, lives, level);

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



        public static void loadLevel1(Canvas canvas, Window window, Player player)
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

             //   if(game.GetLevel == 4)
               // {
                 //   LoadLevels load = new LoadLevels(game.GetCoins, game.GetLives, game.GetLevel);
                //}
                //else
                ///{
                    LoadLevels loadlvl1 = new LoadLevels(game.GetShipInt, game.GetShipPoint, game.GetShipPath, game.GetCommanderInt,
                      game.GetCommanderPoint, game.GetCommanderPath, game.GetBugInt, game.GetBugPoint, game.GetBugPath,
                       canvas, window, game.GetShipMin, game.GetShipMax, game.GetComMin, game.GetComMax, game.GetBugMin, game.GetBugMax
                       , game.GetCoins, game.GetLives, game.GetLevel);
                    player = loadlvl1.getPlayer();
                //}
               
            }
            catch (SerializationException e)
            {
                MessageBox.Show("An error occured and the current game was not able to be LOADED.");
                MessageBox.Show(e.Message);
            }



        }
    }
}
