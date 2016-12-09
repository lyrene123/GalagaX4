using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GalagaX4
{
    [Serializable()]
    public class SerializeGameObj
    {

        List<String> shipPath;
        List<String> commanderPath;
        List<String> bugPath;

        List<Point> shipPoint;
        List<Point> commanderPoint;
        List<Point> bugPoint;

        List<int> shipInt;
        List<int> commanderInt;
        List<int> bugInt;
        List<double> minXShip = new List<double>();
        List<double> maxXShip = new List<double>();
        List<double> minXCom = new List<double>();
        List<double> maxXCom = new List<double>();
        List<double> minXBug = new List<double>();
        List<double> maxXBug = new List<double>();

        int lives;
        int coins;
        int level;

        public int GetLives
        {
            get { return this.lives; }
        }
        public int GetCoins
        {
            get { return this.coins; }
        }
        public int GetLevel
        {
            get { return this.level; }
        }

        public List<Point> GetShipPoint
        {
            get { return this.shipPoint; }
        }

        public List<Point> GetCommanderPoint
        {
            get { return this.commanderPoint; }
        }
        public List<Point> GetBugPoint
        {
            get { return this.bugPoint; }
        }

        public List<String> GetShipPath
        {
            get { return this.shipPath; }
        }
        public List<String> GetCommanderPath
        {
            get { return this.commanderPath; }
        }
        public List<String> GetBugPath
        {
            get { return this.bugPath; }
        }


        public List<int> GetShipInt
        {
            get { return this.shipInt; }
        }
        public List<int> GetCommanderInt
        {
            get { return this.commanderInt; }
        }
        public List<int> GetBugInt
        {
            get { return this.bugInt; }
        }

        public List<double> GetShipMin
        {
            get { return this.minXShip; }
        }
        public List<double> GetShipMax
        {
            get { return this.maxXShip; }
        }
        public List<double> GetComMin
        {
            get { return this.minXCom; }
        }
        public List<double> GetComMax
        {
            get { return this.maxXCom; }
        }
        public List<double> GetBugMin
        {
            get { return this.minXBug; }
        }
        public List<double> GetBugMax
        {
            get { return this.maxXBug; }
        }



        public SerializeGameObj(List<int> shipInt, List<Point> shipPoint, List<String> shipPath,
            List<int> commanderInt, List<Point> commanderPoint, List<String> commanderPath,
            List<int> bugInt, List<Point> bugPoint, List<String> bugPath, List<double> minXShip,
        List<double> maxXShip,
        List<double> minXCom,
        List<double> maxXCom,
        List<double> minXBug,
        List<double> maxXBug, int coins, int lives, int level)
        {
            this.coins = coins;
            this.lives = lives;
            this.level = level;
            this.shipInt = shipInt;
            this.commanderInt = commanderInt;
            this.bugInt = bugInt;

            this.shipPoint = shipPoint;
            this.commanderPoint = commanderPoint;
            this.bugPoint = bugPoint;

            this.shipPath = shipPath;
            this.commanderPath = commanderPath;
            this.bugPath = bugPath;

            this.minXShip = minXShip;
            this.maxXShip = maxXShip;
            this.minXCom = minXCom;
            this.maxXCom = maxXCom;
            this.minXBug = minXBug;
            this.maxXBug = maxXBug;

        }




    }
}
