using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GalagaX4
{
    class Level4
    {
        Window window;
        Canvas canvas;
        Player player;

        Boss boss;

        List<Enemies> enemies;

        Image lv4Pic;

        public Level4(Window window, Canvas canvas, Player player)
        {
            this.window = window;
            this.canvas = canvas;
            this.player = player;

            enemies = new List<Enemies>();
        }

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

        public async void Play()
        {
            DisplayLevel();
            await Task.Delay(2000);
            this.canvas.Children.Remove(lv4Pic);

            Point bossPos = new Point();
            bossPos.X = 300;
            bossPos.Y = 100;
            Animation bossAnimation;
            Image bossPic = new Image();
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
            this.boss.Shoot(1);

            player.SetEnemyTarget(enemies);
        }

    }
}
