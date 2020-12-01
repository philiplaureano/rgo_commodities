using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Optional;

namespace RGO.Models
{
    public class SolarSystem : SolarSystemReference
    {
        public IList<SpaceStation> Stations { get; } = new Collection<SpaceStation>();
        public IList<SolarSystemReference> Neighbors { get; } = new Collection<SolarSystemReference>();
        public override Option<SolarSystem> Resolve()
        {
            return Option.Some(this);
        }
    }
}