namespace DistribuidoraDoChines.Commons.Models
{
    public class UnitOfLength
    {
        public static readonly UnitOfLength Kilometers = new UnitOfLength(1.609344);
        // public static readonly UnitOfLength NauticalMiles = new UnitOfLength(0.8684);
        // public static readonly UnitOfLength Miles = new UnitOfLength(1);

        private readonly double _fromMilesFactor;

        private UnitOfLength(double fromMilesFactor)
        {
            _fromMilesFactor = fromMilesFactor;
        }

        public double ConvertFromMiles(double input)
        {
            return input * _fromMilesFactor;
        }
    }
}