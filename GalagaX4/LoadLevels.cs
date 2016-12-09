using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GalagaX4
{
    class LoadLevels
    {
        static DispatcherTimer timerRandomShoot;

        int spaceX;
        int round = 1;
        Image lv2Pic;
        Window window;
        Player player;
        Player newPlayer;
        static Player staticPlayer;
        List<SpaceShip> ships = new List<SpaceShip>();
        List<Commander> commanders = new List<Commander>();
        List<SpaceShip> ufos = new List<SpaceShip>();
        List<int> arr1 = new List<int>();
        List<int> arr2 = new List<int>();
        bool exists1 = false;
        bool exists2 = false;
        List<Enemies> enemies = new List<Enemies>();
        Canvas canvas;
        Bug[] bees;
        Bug[] redbugs;
        Image[] commanderPic;
        BitmapImage[] commanderImages = { UtilityMethods.LoadImage("pics/commander.png"), UtilityMethods.LoadImage("pics/commander2.png") };
        int coins;
        int level;
        int lives;

        public LoadLevels(List<int> shipInt, List<Point> shipPoint, List<String> shipPath,
            List<int> commanderInt, List<Point> commanderPoint, List<String> commanderPath,
            List<int> bugInt, List<Point> bugPoint, List<String> bugPath,
             Canvas canvas, Window window, List<double> minXShip,
        List<double> maxXShip,
        List<double> minXCom,
        List<double> maxXCom,
        List<double> minXBug,
        List<double> maxXBug, int coins, int lives, int level)
        {
            this.canvas = canvas;
            this.lives = lives;
            this.coins = coins;
            this.level = level;
            this.window = window;
            this.commanderPic = new Image[commanderPath.Count];

            Image playerPic = new Image();
            playerPic.Source = UtilityMethods.LoadImage("pics/galaga_ship.png");
            playerPic.Width = 42;
            playerPic.Height = 46;
            canvas.Children.Add(playerPic);
            Canvas.SetLeft(playerPic, 405);
            Canvas.SetTop(playerPic, 500);
            Point playerPoint = new Point(27, 490);
            player = new Player(playerPoint, playerPic, canvas, 15, lives, coins);


                loadCommanders(commanderInt, commanderPoint, commanderPath, minXCom, maxXCom, player);
                loadShip(shipInt, shipPoint, shipPath, maxXShip, minXShip, player);
                loadBug(bugInt, bugPoint, bugPath, maxXBug, minXBug, player);
                staticPlayer = player;
                player.setCurrentLevel(level);
                player.setCoins(coins);
                player.setLives(lives);
                staticPlayer.setCurrentLevel(level);
                staticPlayer.setCoins(coins);
                staticPlayer.setLives(lives);
       

            player.SetEnemyTarget(enemies);


            newPlayer = new Player(player);
            StartGame();
        }

        void StartGame()
        {
            timerRandomShoot = new DispatcherTimer(DispatcherPriority.Normal);
            timerRandomShoot.Interval = TimeSpan.FromSeconds(1);
            timerRandomShoot.Tick += new EventHandler(ShootUpdate);
            timerRandomShoot.Start();
        }

        public Player getPlayer()
        {
            return this.newPlayer;
        }

        public static Player getStaticPlayer()
        {
            return staticPlayer;
        }

        void DisplayLevel()
        {
            lv2Pic = new Image();
            lv2Pic.Height = 50;
            lv2Pic.Width = 140;
            this.canvas.Children.Add(lv2Pic);
            Canvas.SetTop(lv2Pic, 200);
            Canvas.SetLeft(lv2Pic, 350);
            lv2Pic.Source = UtilityMethods.LoadImage("pics/level2.png");
        }

        public async void Play()
        {
            if (this.round == 1)
            {
                DisplayLevel();
                await Task.Delay(2000);
                this.canvas.Children.Remove(lv2Pic);
            }
            else
            {
                this.arr1 = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
                this.arr2 = new List<int>() { 0, 1, 2, 3 };
                this.exists1 = false;
                this.exists2 = false;
                this.spaceX = 0;

                await Task.Delay(1500);
            }

        }
        private void ShootUpdate(object sender, EventArgs e)
        {
            BackToMainWindown();
            //-------------------------------
            for (int i = 0; i < ships.Count; i++)
            {
                if (ships[i].IsDead() == true)
                {
                    arr1.Remove(i);
                }
            }

            for (int i = 0; i < commanders.Count; i++)
            {
                if (commanders[i].IsDead() == true)
                {
                    arr2.Remove(i);
                }
            }

            //--------------------------------
            Random rand = new Random();
            int num1 = rand.Next(ships.Count);//8      
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
            if (enemies.Count == 0 && level == 1)
            {
                timerRandomShoot.Stop();

                Level2 lv2 = new Level2(this.window, this.canvas, this.player);
                lv2.setLoad(true);
                staticPlayer.updateCurrentLevel(2);
                lv2.Play();
            }
            else if (this.enemies.Count == 0 && level == 2)
            {
                if (round < 2)
                {
                    timerRandomShoot.Stop();

                    round++;
                    Level2 lv2 = new Level2(this.window, this.canvas, this.player);
                    lv2.setRound(round);
                    lv2.setLoad(true);
                    lv2.Play();
                }
                else
                {
                    if (round == 2)
                    {
                        timerRandomShoot.Stop();

                        Level3 lv3 = new Level3(this.window, this.canvas, this.player);
                        lv3.Play();
                    }
                }
            }
            else if (this.enemies.Count == 0 && level == 3)
                
            {
                if (round < 3)
                {
                    timerRandomShoot.Stop();

                    round++;
                    Level3 lv3 = new Level3(this.window, this.canvas, this.player);
                    lv3.setRound(round);
                    lv3.setLoad(true);
                    lv3.Play();
                }
                else
                {

                    if (round == 3)
                    {
                        timerRandomShoot.Stop();

                        Level4 lv4 = new Level4(this.window, this.canvas, this.player);
                        lv4.Play();
                    }
                }
            }

        }

        void BackToMainWindown()
        {
            if (player.GetLives() == 0)
            {
                Player player = new Player();
                this.window.Hide();
                //this.timerRandomShoot.Stop();
                //this.window.Close();

                var mainWindow = new MainWindow();
                //mainWindow.Show();
                //this.timerRandomShoot.Stop();
                //this.window.Close();
                //gamewindow.mediaElement.BeginInit();
                player.shootSoundEffect.Dispose();
            }
        }



        void loadCommanders(List<int> commanderInt, List<Point> commanderPoint, List<String> commanderPath,
            List<double> min, List<double> max, Player player)
        {
            int spaceX = 0;
            for (int i = 0; i < commanderInt.Count; i++)
            {
                if (commanderInt[i] == 1)
                {
                    commanderPic[i] = new Image();
                    commanderPic[i].Source = UtilityMethods.LoadImageFullPath(commanderPath[i]);
                    commanderPic[i].Width = 34;
                    commanderPic[i].Height = 26;
                    canvas.Children.Add(commanderPic[i]);
                    Canvas.SetLeft(commanderPic[i], commanderPoint[i].X + spaceX);
                    Canvas.SetTop(commanderPic[i], commanderPoint[i].Y);
                    spaceX += 90;
                    Point commanderPos = new Point();
                    commanderPos.X = commanderPoint[i].X;
                    commanderPos.Y = commanderPoint[i].Y;
                    Animation commanderAnimation = new Animation(commanderPic[i], commanderImages, true);
                    Commander commander = new Commander(commanderPos, commanderPic[i], canvas, commanderAnimation);
                    commander.setMaxX(max[i]);
                    commander.setMinX(min[i]);
                    commanders.Add(commander);
                    enemies.Add(commanders[i]);
                    commanders[i].setTarget(player);
                    commanders[i].Fly(200);
                    arr2.Add(i);

                }
            }

        }


        void loadShip(List<int> shipInt, List<Point> shipPoint, List<String> shipPath,
            List<double> max, List<double> min, Player player)
        {
            Image[] shipsPic = new Image[shipPath.Count];
            int spaceX = 0;
            for (int i = 0; i < shipInt.Count; i++)
            {

                if (shipInt[i] == 0)
                {
                    if (shipPath[i].Contains("spaceShip.png"))
                    {
                        shipsPic[i] = new Image();
                        shipsPic[i].Source = UtilityMethods.LoadImage("pics/spaceShip.png");
                        shipsPic[i].Width = 34;
                        shipsPic[i].Height = 26;
                        canvas.Children.Add(shipsPic[i]);
                        // Canvas.GetLeft(shipPoint[i]);
                        Canvas.SetLeft(shipsPic[i], shipPoint[i].X + spaceX);
                        Canvas.SetTop(shipsPic[i], shipPoint[i].Y);
                        spaceX += 60;
                        Point shipPos = new Point();
                        shipPos.X = shipPoint[i].X;
                        shipPos.Y = shipPoint[i].Y;

                        SpaceShip ship = new SpaceShip(shipPos, shipsPic[i], canvas);
                        ship.setMaxX(max[i]);
                        ship.setMinX(max[i]);
                        ships.Add(ship);
                        enemies.Add(ships[i]);
                        ships[i].setTarget(player);
                        ships[i].Fly(200);
                        // ships[i].Shoot(200);
                        arr1.Add(i);
                    }
                    else if (shipPath[i].Contains("UFO.png"))
                    {
                        Image[] ufoPics = new Image[shipInt.Count];
                        spaceX = 0;


                        ufoPics[i] = new Image();
                        ufoPics[i].Source = UtilityMethods.LoadImageFullPath(shipPath[i]);
                        ufoPics[i].Width = 34;
                        ufoPics[i].Height = 26;
                        canvas.Children.Add(ufoPics[i]);
                        Canvas.SetLeft(ufoPics[i], shipPoint[i].X + spaceX);
                        Canvas.SetTop(ufoPics[i], shipPoint[i].Y);
                        spaceX += 60;

                        Point ufoPos = new Point();
                        ufoPos.X = shipPoint[i].X;
                        ufoPos.Y = shipPoint[i].Y;

                        SpaceShip ufo = new SpaceShip(ufoPos, ufoPics[i], canvas);
                        ufo.setMaxX(max[i]);
                        ufo.setMinX(max[i]);
                        ufos.Add(ufo);


                    }
                }
            }
            for (int i = 0; i < ufos.Count; i++)
            {
                enemies.Add(ufos[i]);
                ufos[i].setTarget(player);
                ufos[i].setMoveCounter(2);
                ufos[i].Fly(180);
                // ships[i].Shoot(200);
            }

        }

        void loadBug(List<int> bugInt, List<Point> bugPoint, List<String> bugPath,
            List<double> max, List<double> min, Player player)
        {
            BitmapImage[] beeImages = { UtilityMethods.LoadImage("pics/bee0.png"),
                    UtilityMethods.LoadImage("pics/bee1.png") };

            BitmapImage[] bugImages = { UtilityMethods.LoadImage("pics/redBug0.png"),
                    UtilityMethods.LoadImage("pics/redBug1.png") };

            int spaceX = 0;
            Image[] beesPic = new Image[bugPath.Count];

            for (int i = 0; i < bugInt.Count; i++)
            {
                if (bugInt[i] == 2)
                {
                    if (bugPath[i].Contains("bee0.png"))
                    {
                        bees = new Bug[beesPic.Length];
                        beesPic[i] = new Image();
                        beesPic[i].Source = UtilityMethods.LoadImageFullPath(bugPath[i]);
                        beesPic[i].Width = 34;
                        beesPic[i].Height = 26;
                        canvas.Children.Add(beesPic[i]);
                        Canvas.SetLeft(beesPic[i], bugPoint[i].X + spaceX);
                        Canvas.SetTop(beesPic[i], bugPoint[i].Y);
                        spaceX += 60;

                        Point beePos = new Point();
                        beePos.X = bugPoint[i].X;
                        beePos.Y = bugPoint[i].Y;
                        Animation beeAnimation = new Animation(beesPic[i], beeImages, true);
                        Bug bee = new Bug(beePos, beesPic[i], canvas, beeAnimation);
                        bee.setMaxX(max[i]);
                        bee.setMinX(min[i]);
                        bees[i] = bee;
                        enemies.Add(bees[i]);
                        bees[i].setTarget(player);
                        bees[i].Fly(200);
                    }
                    else
                    {
                        Image[] bugsPic = new Image[beesPic.Length];
                        redbugs = new Bug[bugsPic.Length];
                        spaceX = 0;
                        bugsPic[i] = new Image();
                        bugsPic[i].Width = 34;
                        bugsPic[i].Height = 26;
                        canvas.Children.Add(bugsPic[i]);
                        Canvas.SetLeft(bugsPic[i], bugPoint[i].X + spaceX);
                        Canvas.SetTop(bugsPic[i], bugPoint[i].Y);
                        spaceX += 60;

                        Point bugPos = new Point();
                        bugPos.X = bugPoint[i].X;
                        bugPos.Y = bugPoint[i].Y;
                        Animation bugAnimation = new Animation(bugsPic[i], bugImages, true);
                        Bug bug = new Bug(bugPos, bugsPic[i], canvas, bugAnimation);
                        bug.setMaxX(max[i]);
                        bug.setMinX(min[i]);
                        redbugs[i] = bug;
                        enemies.Add(redbugs[i]);
                        redbugs[i].setTarget(player);
                        redbugs[i].Fly(200);
                    }
                }
            }
        }
        void setEnemies()
        {
            player.SetEnemyTarget(enemies);

            newPlayer = new Player(player);
            StartGame();

        }

    }
}