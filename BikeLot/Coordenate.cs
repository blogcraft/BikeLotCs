using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeLot
{

    class Coordenate
    {
        private double _Lat;
        public double latitude
        {
            get 
            { 
                return _Lat;
            }
            set 
            { 
                _Lat = value; 
            }
        }
        private double _Lon;
        public double longitude
         {
            get
            { 
                return _Lon; 
            }
            set 
            { 
                _Lon = value; 
            }
        }
        private double _Spots;
        public double spots
        {
            get { return _Spots; }
            set { _Lon = value; }
        }
        
    }
}
