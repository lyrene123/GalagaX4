using Galaga;
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
        DispatcherTimer timer;

        Player player;
        Boss boss = null;
        Point playerPoint;
        double playerSpeed = 15;
        //Animation playerAnimation;
        /*
        SpaceShip spaceShip;
        Animation spaceShipAnim;
        Point spaceShipPoint;
        int spaceShipShootFrequency = 3;

        //Bug bee;
        //Animation beeAnimation;
        Animation commanderAnimation;
       // Point beePos;
        Commander commander;
        */
        SpaceShip[] ships;
        Commander[] commanders;
        List<int> arr1 = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        List<int> arr2 = new List<int>() { 0, 1, 2, 3 };
        bool exists1 = false;
        bool exists2 = false;
        List<Enemies> enemies;
        
        double shootFrequency = 0.001;
        public GameWindow()
        {
            InitializeComponent();
            KeyDown += new KeyEventHandler(MyCanvas_KeyDown);

            //player creation
            Image playerPic = new Image();
            playerPic.Source = UtilityMethods.LoadImage("pics/galaga_ship.png");
            playerPic.Width = 42;
            playerPic.Height = 46;
            MyCanvas.Children.Add(playerPic);
            Canvas.SetLeft(playerPic, 405);
            Canvas.SetTop(playerPic, 500);
            playerPoint.X = 27;
            playerPoint.Y = 490;
            player = new Player(playerPoint, playerPic, MyCanvas, playerSpeed);

            this.enemies = new List<Enemies>();

            //bee creation
            BitmapImage[] beeImages = { UtilityMethods.LoadImage("pics/bee0.png"),
                    UtilityMethods.LoadImage("pics/bee1.png") };

            Image[] beesPic = new Image[] {this.beePic, this.beePic1, this.beePic2,
                                        this.beePic3, this.beePic4, this.beePic5,
                                        this.beePic6, this.beePic7};
            Bug[] bees = new Bug[8];
            for(int i =0; i< beesPic.Length; i++)
            {
                Point beePos = new Point();
                beePos.X = Canvas.GetLeft(beesPic[i]);
                beePos.Y = Canvas.GetTop(beesPic[i]);
                Animation beeAnimation = new Animation(beesPic[i], beeImages, true);
                Bug bee = new Bug(beePos, beesPic[i], MyCanvas, beeAnimation);
                bees[i] = bee;
                enemies.Add(bees[i]);
                bees[i].setTarget(player);
                bees[i].Fly(200);
            }


            //redbugs creation
            BitmapImage[] bugImages = { UtilityMethods.LoadImage("pics/redBug0.png"),
                    UtilityMethods.LoadImage("pics/redBug1.png") };
            Image[] bugsPic = new Image[] {this.redBugPic, this.redBugPic1, this.redBugPic2,
                                        this.redBugPic3, this.redBugPic4, this.redBugPic5,
                                        this.redBugPic6, this.redBugPic7, this.redBugPic8};
            Bug[] redbugs = new Bug[9];
            for (int i = 0; i < bugsPic.Length; i++)
            {
                Point bugPos = new Point();
                bugPos.X = Canvas.GetLeft(bugsPic[i]);
                bugPos.Y = Canvas.GetTop(bugsPic[i]);
                Animation bugAnimation = new Animation(bugsPic[i], bugImages, true);
                Bug bug = new Bug(bugPos, bugsPic[i], MyCanvas, bugAnimation);
                redbugs[i] = bug;
                enemies.Add(redbugs[i]);
                redbugs[i].setTarget(player);
                redbugs[i].Fly(200);
            }

            //spaceship creation
            BitmapImage[] shipImages = { UtilityMethods.LoadImage("pics/spaceShip.png") };
            Image[] shipsPic = new Image[] {this.spaceShipPic, this.spaceShipPic1, this.spaceShipPic2,
                                        this.spaceShipPic3, this.spaceShipPic4, this.spaceShipPic5,
                                        this.spaceShipPic6, this.spaceShipPic7};
            this. ships = new SpaceShip[shipsPic.Length];
            for (int i = 0; i < ships.Length; i++)
            {
                Point shipPos = new Point();
                shipPos.X = Canvas.GetLeft(shipsPic[i]);
                shipPos.Y = Canvas.GetTop(shipsPic[i]);
                Animation shipAnimation = new Animation(shipsPic[i], shipImages, true);
                SpaceShip ship = new SpaceShip(shipPos, shipsPic[i], MyCanvas, shipAnimation);
                ships[i] = ship;
                enemies.Add(ships[i]);
                ships[i].setTarget(player);
                ships[i].Fly(200);
               // ships[i].Shoot(200);
            }

            //commander
            BitmapImage[] commanderImages = { UtilityMethods.LoadImage("pics/commander.png"), UtilityMethods.LoadImage("pics/commander2.png") };
            Image[] commanderPic = new Image[] {this.commanderPic, this.commanderPic1, this.commanderPic2,
                                        this.commanderPic3};
            this.commanders = new Commander[4];
            for (int i = 0; i < commanderPic.Length; i++)
            {
                Point commanderPos = new Point();
                commanderPos.X = Canvas.GetLeft(commanderPic[i]);
                commanderPos.Y = Canvas.GetTop(commanderPic[i]);
                Animation commanderAnimation = new Animation(commanderPic[i], commanderImages, true);
                Commander commander = new Commander(commanderPos, commanderPic[i], MyCanvas, commanderAnimation);
                commanders[i] = commander;
                enemies.Add(commanders[i]);
                commanders[i].setTarget(player);
                commanders[i].Fly(200);
            }

            Point bossPos = new Point();
            bossPos.X = 400;
            bossPos.Y = 200;
            Animation bossAnimation;
            Image bossPic = new Image();
            bossPic.Source = UtilityMethods.LoadImage("pics/boss/boss1.png");
            MyCanvas.Children.Add(bossPic);

            BitmapImage[] bossImages = { UtilityMethods.LoadImage("pics/boss/boss1.png"),
                    UtilityMethods.LoadImage("pics/boss/boss2.png"), UtilityMethods.LoadImage("pics/boss/boss3.png"),};
            bossAnimation = new Animation(bossPic, bossImages, true);
            this.boss = new Boss(bossPos, bossPic, MyCanvas, bossAnimation);
            Animation.Initiate(bossAnimation, 500);

            enemies.Add(boss);

            this.boss.Fly(200);
            this.boss.Shoot(200);
        









        player.SetEnemyTarget(enemies);
             Update();

            //ships[1].Shoot(600);




        }

        void Update()
        {
            timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(ShootUpdate);
            timer.Start();
            label.Content = "shootFrequency : " + shootFrequency;
        }

        private void ShootUpdate(object sender, EventArgs e)
        {
            /*label.Content = "oldNum : " + oldNum;
            if(ships[oldNum].IsShoot() == true)
            {
                ships[oldNum].StopShoot();
               
            }
            if(commanders[oldNum2].isShoot() == true)
            {
                commanders[oldNum2].stopShoot();
            }

            Random rand = new Random();
            int num1 = rand.Next(ships.Length);
            oldNum = num1;
            if(ships[num1].IsShoot() == true)
            {
                ships[num1].StopShoot();
            }
            else
            {
                if(ships[num1].IsDead() != true)
                {
                    ships[num1].Shoot(10);
                }
            }

            int num2 = rand.Next(commanders.Length);
            oldNum2 = num2;
            if (commanders[num2].isShoot() == true)
            {
                commanders[num2].stopShoot();
            }
            else
            {
                if (commanders[num2].IsDead() != true)
                {
                    commanders[num2].Shoot(10);
                }
            }*/
            
            Random rand = new Random();
            int num1 = rand.Next(ships.Length);//8      
                if (arr1.Contains(num1))
                {
                    this.arr1.Remove(num1);
                    this.exists1 = true;
                }
            

            if (this.exists1 == true)
            {
               // this.arr1.Add(num1);
                if (ships[num1].IsShoot() == true)
                {
                     ships[num1].StopShoot();
                }
                else
                {
                    ships[num1].Shoot(10);       
                }
                this.exists1 = false;
            }

            for(int i=0; i<ships.Length; i++)
            {
                if(ships[i].IsDead() == true)
                {
                    this.arr1.Remove(i);
                }
            }
            //label1.Content = "num1 : " + num1;
            //oldNum = num1;
            /////
           

            int num2 = rand.Next(4);
           
                if (arr2.Contains(num2))
                {
                    this.exists2 = true;
                    this.arr2.Remove(num2);
                }
            

            if(this.exists2 == true)
            {
                if (commanders[num2].isShoot() == true)
                {
                    commanders[num2].stopShoot();
                }
                else
                {
                      commanders[num2].Shoot(20);

                 }
                this.exists2 = false;
            }
            for (int i = 0; i < commanders.Length; i++)
            {
                if (commanders[i].IsDead() == true)
                {
                    this.arr2.Remove(i);
                }
            }

            // oldNum2 = num2;
            shootFrequency = 8;
            label.Content = "shootFrequency : " + shootFrequency;
            timer.Interval = TimeSpan.FromSeconds(shootFrequency);

            label.Content = "size : " + arr1.Count;
            if (this.enemies.Count == 0)
            {
                this.timer.Stop();
            }
        }

        void MyCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            this.player.Move(sender, e);
            this.player.Shoot(sender, e);
        }
    }
}
