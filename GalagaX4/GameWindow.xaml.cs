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
        int spaceX = 0;

        Player player;

        SpaceShip[] ships;
        Commander[] commanders;
        List<int> arr1 = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        List<int> arr2 = new List<int>() { 0, 1, 2, 3 };
        bool exists1 = false;
        bool exists2 = false;
        List<Enemies> enemies;

        //double shootFrequency = 0.001;
        /*
        Player player;
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
        
        SpaceShip[] ships;
        Commander[] commanders;
        List<int> arr1 = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        List<int> arr2 = new List<int>() { 0, 1, 2, 3 };
        bool exists1 = false;
        bool exists2 = false;
        List<Enemies> enemies;
        
        double shootFrequency = 0.001;
        */

        //Canvas[] canvas;

        //Canvas lv1Canvas;
        public GameWindow()
        {
            InitializeComponent();
            KeyDown += new KeyEventHandler(MyGrid_KeyDown);

            lv1Canvas.Visibility = Visibility.Visible;
            lv2Canvas.Visibility = Visibility.Collapsed;

            enemies = new List<Enemies>();

            //player creation
            Image playerPic = new Image();
            playerPic.Source = UtilityMethods.LoadImage("pics/galaga_ship.png");
            playerPic.Width = 42;
            playerPic.Height = 46;
            lv1Canvas.Children.Add(playerPic);
            Canvas.SetLeft(playerPic, 405);
            Canvas.SetTop(playerPic, 500);
            Point playerPoint = new Point(27, 490); 
            player = new Player(playerPoint, playerPic, lv1Canvas, 15);

            //List<Enemies> enemies = new List<Enemies>();

            
            //bee creation
            BitmapImage[] beeImages = { UtilityMethods.LoadImage("pics/bee0.png"),
                    UtilityMethods.LoadImage("pics/bee1.png") };
            Image[] beesPic = new Image[8];
            //int spaceX = 0;

            Bug[] bees = new Bug[beesPic.Length];
            for (int i=0; i<beesPic.Length; i++)
            {
                beesPic[i] = new Image();
                beesPic[i].Width = 34;
                beesPic[i].Height = 26;
                lv1Canvas.Children.Add(beesPic[i]);
                Canvas.SetLeft(beesPic[i], 181 + spaceX);
                Canvas.SetTop(beesPic[i], 185);                
                spaceX += 60;

                Point beePos = new Point();
                beePos.X = Canvas.GetLeft(beesPic[i]);
                beePos.Y = Canvas.GetTop(beesPic[i]);
                Animation beeAnimation = new Animation(beesPic[i], beeImages, true);
                Bug bee = new Bug(beePos, beesPic[i], lv1Canvas, beeAnimation);
                bees[i] = bee;
                enemies.Add(bees[i]);
                bees[i].setTarget(player);
                bees[i].Fly(200);
            }

            BitmapImage[] bugImages = { UtilityMethods.LoadImage("pics/redBug0.png"),
                    UtilityMethods.LoadImage("pics/redBug1.png") };
            Image[] bugsPic = new Image[9];
            Bug[] redbugs = new Bug[bugsPic.Length];
            spaceX = 0;
            for (int i = 0; i < bugsPic.Length; i++)
            {
                bugsPic[i] = new Image();
                bugsPic[i].Width = 34;
                bugsPic[i].Height = 26;
                lv1Canvas.Children.Add(bugsPic[i]);
                Canvas.SetLeft(bugsPic[i], 154 + spaceX);
                Canvas.SetTop(bugsPic[i], 145);
                spaceX += 60;

                Point bugPos = new Point();
                bugPos.X = Canvas.GetLeft(bugsPic[i]);
                bugPos.Y = Canvas.GetTop(bugsPic[i]);
                Animation bugAnimation = new Animation(bugsPic[i], bugImages, true);
                Bug bug = new Bug(bugPos, bugsPic[i], lv1Canvas, bugAnimation);
                redbugs[i] = bug;
                enemies.Add(redbugs[i]);
                redbugs[i].setTarget(player);
                redbugs[i].Fly(200);
            }

            Image[] shipsPic = new Image[8];
            ships = new SpaceShip[shipsPic.Length];
            spaceX = 0;
            for (int i = 0; i < ships.Length; i++)
            {
                shipsPic[i] = new Image();
                shipsPic[i].Source = UtilityMethods.LoadImage("pics/spaceShip.png");
                shipsPic[i].Width = 34;
                shipsPic[i].Height = 26;
                lv1Canvas.Children.Add(shipsPic[i]);
                Canvas.SetLeft(shipsPic[i], 181 + spaceX);
                Canvas.SetTop(shipsPic[i], 105);
                spaceX += 60;

                Point shipPos = new Point();
                shipPos.X = Canvas.GetLeft(shipsPic[i]);
                shipPos.Y = Canvas.GetTop(shipsPic[i]);

                SpaceShip ship = new SpaceShip(shipPos, shipsPic[i], lv1Canvas);
                ships[i] = ship;
                enemies.Add(ships[i]);
                ships[i].setTarget(player);
                ships[i].Fly(200);
                // ships[i].Shoot(200);
            }

            BitmapImage[] commanderImages = { UtilityMethods.LoadImage("pics/commander.png"), UtilityMethods.LoadImage("pics/commander2.png") };
            Image[] commanderPic = new Image[4];
            commanders = new Commander[commanderPic.Length];
            spaceX = 0;
            for (int i = 0; i < commanderPic.Length; i++)
            {
                commanderPic[i] = new Image();
                //commanderPic[i].Source = UtilityMethods.LoadImage("pics/spaceShip.png");
                commanderPic[i].Width = 34;
                commanderPic[i].Height = 26;
                lv1Canvas.Children.Add(commanderPic[i]);
                Canvas.SetLeft(commanderPic[i], 260 + spaceX);
                Canvas.SetTop(commanderPic[i], 65);
                spaceX += 90;

                Point commanderPos = new Point();
                commanderPos.X = Canvas.GetLeft(commanderPic[i]);
                commanderPos.Y = Canvas.GetTop(commanderPic[i]);
                Animation commanderAnimation = new Animation(commanderPic[i], commanderImages, true);
                Commander commander = new Commander(commanderPos, commanderPic[i], lv1Canvas, commanderAnimation);
                commanders[i] = commander;
                enemies.Add(commanders[i]);
                commanders[i].setTarget(player);
                commanders[i].Fly(200);
            }
            
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
        }

        private void ShootUpdate(object sender, EventArgs e)
        {
            BackToMainWindown();
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
                this.timer.Stop();
            }
        }

        void MyGrid_KeyDown(object sender, KeyEventArgs e)
        {
            this.player.Move(sender, e);
            this.player.Shoot(sender, e);
        }
        void BackToMainWindown()
        {
            if (player.GetLives() == 0)
            {
                this.Hide();
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.timer.Stop();
                this.Close();
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /*TimeSpan ts = new TimeSpan(0, 0, 0, 0, 6);
            GameWindow media = new GameWindow();
            media.mediaElement.Position = ts;
            media.mediaElement.Volume = 6.6;
            media.mediaElement.SpeedRatio = 1.5;
            media.mediaElement.Source = new Uri(@"D:\GalagaX4_Backup\GalagaX4\audio\mainbacksound.wav", UriKind.Absolute);
            media.mediaElement.Play();
            */
        }
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            /*TimeSpan ts = new TimeSpan(0, 0, 0, 0, 0);
            mediaElement.Position = ts;
            mediaElement.Volume = 6.6;
            //mediaElement.SpeedRatio = 0.0001;
            mediaElement.Source = new Uri(@"D:\GalagaX4_Backup\GalagaX4\audio\mainbacksound.wav", UriKind.Absolute);
            mediaElement.Play();
            */
        }
    }
}
