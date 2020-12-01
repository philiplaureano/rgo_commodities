using Optional;

namespace RGO.Models
{
    public class SolarSystemReference
    {
        public string Name { get; set; }

        public virtual Option<SolarSystem> Resolve()
        {
            return Option.None<SolarSystem>();
        }
    }
}