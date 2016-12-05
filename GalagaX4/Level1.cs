using System;
using System.Collections.Generic;
using System.Linq;
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
    class Level1
    {
        Window window;
        Canvas canvas;
       GameWindow gamewindow;               
        static DispatcherTimer timerRandomShoot;
        int spaceX = 0;

        Player player;

        SpaceShip[] ships;
        Commander[] commanders;
        List<int> arr1 = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        List<int> arr2 = new List<int>() { 0, 1, 2, 3 };
        bool exists1 = false;
        bool exists2 = false;
        List<Enemies> enemies;
        Image lv1Pic;


        public Level1(Window window, Canvas canvas, Player player)
        {
            this.window = window;
            this.canvas = canvas;
            this.player = player;
            enemies = new List<Enemies>();
            this.player.updateCurrentLevel(1);
        }

        public static DispatcherTimer timerRandom
        {
            get { return timerRandomShoot; }
        }

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

        public async void Play()
        {
            DisplayLevel();
            await Task.Delay(2000);
            this.canvas.Children.Remove(lv1Pic);

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
                // ships[i].Shoot(200);
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
                //commanderPic[i].Source = UtilityMethods.LoadImage("pics/spaceShip.png");
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


        void StartGame()
        {
            timerRandomShoot = new DispatcherTimer(DispatcherPriority.Normal);
            timerRandomShoot.Interval = TimeSpan.FromSeconds(1);
            timerRandomShoot.Tick += new EventHandler(ShootUpdate);
            timerRandomShoot.Start();
        }

        private void ShootUpdate(object sender, EventArgs e)
        {
            BackToMainWindown();

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
            
                Level2 lv2 = new Level2(this.window, this.canvas, this.player);
                lv2.Play();
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
    }
}
