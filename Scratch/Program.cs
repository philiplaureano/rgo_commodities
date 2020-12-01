using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using RGO.Models;

namespace Scratch
{
    public class TradeRoute
    {
        public TradeRoute(CommodityType commodityType, (SolarSystem, SpaceStation) source,
            (SolarSystem, SpaceStation) destination)
        {
            CommodityType = commodityType;
            Source = source;
            Destination = destination;
        }

        public CommodityType CommodityType { get; set; }
        public (SolarSystem, SpaceStation) Source { get; set; }
        public (SolarSystem, SpaceStation) Destination { get; set; }

        public override string ToString()
        {
            string ToString((SolarSystem, SpaceStation) kvp)
            {
                return $"{kvp.Item1.Name} ({kvp.Item2.Name})";
            }

            var commodityName = Enum.GetName(typeof(CommodityType), CommodityType);
            return $"{commodityName}: {ToString(Source)} -> {ToString(Destination)}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../");
            var targetDirectory = Path.Combine(basePath, "SolarSystems");

            var targetFiles = Directory.GetFiles(targetDirectory, "*.json").ToArray();

            var systems = new List<SolarSystem>();
            foreach (var targetFile in targetFiles)
            {
                var json = File.ReadAllText(targetFile);
                var readModel = JsonConvert.DeserializeObject<SolarSystem[]>(json);

                systems.AddRange(readModel);
                continue;
            }

            var supplyBoard = new Dictionary<CommodityType, IList<(SolarSystem, SpaceStation)>>();
            var demandBoard = new Dictionary<CommodityType, IList<(SolarSystem, SpaceStation)>>();
            foreach (var system in systems)
            {
                foreach (var station in system.Stations)
                {
                    // Map the supply
                    foreach (var manufacturedCommodityType in station.ManufacturedItems)
                    {
                        if (!supplyBoard.ContainsKey(manufacturedCommodityType))
                            supplyBoard[manufacturedCommodityType] = new List<(SolarSystem, SpaceStation)>();

                        supplyBoard[manufacturedCommodityType].Add((system, station));
                    }

                    // Map the demand
                    foreach (var demandedCommodityType in station.ItemsInDemand)
                    {
                        if (!demandBoard.ContainsKey(demandedCommodityType))
                            demandBoard[demandedCommodityType] = new List<(SolarSystem, SpaceStation)>();

                        demandBoard[demandedCommodityType].Add((system, station));
                    }
                }
            }

            var tradeRoutes = new List<TradeRoute>();
            foreach (var value in Enum.GetValues(typeof(CommodityType)))
            {
                var commodityType = (CommodityType) value;
                if (!supplyBoard.ContainsKey(commodityType) && demandBoard.ContainsKey(commodityType))
                    continue;

                var suppliers = supplyBoard[commodityType];

                if (!demandBoard.ContainsKey(commodityType))
                    continue;

                var buyers = demandBoard[commodityType];
                foreach (var supplier in suppliers)
                {
                    foreach (var buyer in buyers)
                    {
                        tradeRoutes.Add(new TradeRoute(commodityType, supplier, buyer));
                    }
                }
            }

            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }
    }
}