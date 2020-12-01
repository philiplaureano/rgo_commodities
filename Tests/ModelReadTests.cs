using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RGO.Models;
using Xunit;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Tests
{
    public class ModelReadTests
    {
        [Fact]
        public void ShouldReadSpaceStationInfo()
        {
            var station = new SpaceStation()
            {
                Name = "MyStation",
                ManufacturedItems = new List<CommodityType>(),
                ItemsInDemand = new List<CommodityType>()
            };

            var otherSystem = new SolarSystem() {Name = "Foo"};
            
            var system = new SolarSystem();
            system.Name = "MySystem";
            system.Stations.Add(station);
            system.Neighbors.Add(otherSystem);

            SolarSystem[] expectedModel = new[] {system};

            var json = JsonConvert.SerializeObject(expectedModel);

            var rgoModelReader = new RgoModelReader();
            SolarSystem[] actualModel = rgoModelReader.LoadFrom(json);
            Assert.Contains(actualModel, currentSystem => currentSystem.Stations.Any(s => s.Name == station.Name));
            Assert.Contains(actualModel, currentSystem => currentSystem.Neighbors.Any(n => n.Name == "Foo"));
        }
    }
}