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


        public SerializeGameObj(List<int> shipInt, List<Point> shipPoint, List<String> shipPath,
            List<int> commanderInt, List<Point> commanderPoint, List<String> commanderPath,
            List<int> bugInt, List<Point> bugPoint, List<String> bugPath)
        {
            this.shipInt = shipInt;
            this.commanderInt = commanderInt;
            this.bugInt = bugInt;

            this.shipPoint = shipPoint;
            this.commanderPoint = commanderPoint;
            this.bugPoint = bugPoint;

            this.shipPath = shipPath;
            this.commanderPath = commanderPath;
            this.bugPath = bugPath;

        }




    }
}
