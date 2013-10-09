using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace v1._0
{
    class CountryData
    {
        //all the properties of a country
        public string Name;
        public string Tag;
        public int[] Color;
        public string Gfx;
        public string[] Ideas;
        public List<string> Units;
        public string Nationalideagroup;
        public string Culture;
        public string Religion;
        public string PreferedReligion;
        public string Techgroup;
        public string Government;
        public int Capital;
        public decimal Mercantilism;

        //extra properties wich are normally in the controls
        public string Culturegroup;
        public string Religiongroup;
        public string Capitalregion;

        public CountryData()
        { 
            
        }
    }
}
