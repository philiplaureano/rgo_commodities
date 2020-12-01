using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RGO.Models
{
    public class SpaceStation
    {
        public string Name { get; set; }
        public IList<CommodityType> ManufacturedItems { get; set; }
        public IList<CommodityType> ItemsInDemand { get; set; } 
    }
}