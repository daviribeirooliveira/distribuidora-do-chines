namespace DistribuidoraDoChines.Commons.Models
{
    public class Coordinates
    {
        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        internal double Latitude { get; }
        internal double Longitude { get; }
    }
}