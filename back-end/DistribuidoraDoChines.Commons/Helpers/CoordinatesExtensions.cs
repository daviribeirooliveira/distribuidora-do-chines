using System;
using DistribuidoraDoChines.Commons.Models;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace DistribuidoraDoChines.Commons.Helpers
{
    public static class CoordinatesExtensions
    {
        private static readonly Coordinates BaseCoordinates = new Coordinates(-3.7561765, -38.6063537);

        public static double DistanceTo(Coordinates targetCoordinates)
        {
            return DistanceTo(BaseCoordinates, targetCoordinates, UnitOfLength.Kilometers);
        }

        private static double DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates,
            UnitOfLength unitOfLength)
        {
            var baseRad = Math.PI * baseCoordinates.Latitude / 180;
            var targetRad = Math.PI * targetCoordinates.Latitude / 180;
            var theta = baseCoordinates.Longitude - targetCoordinates.Longitude;
            var thetaRad = Math.PI * theta / 180;

            var distance =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);

            distance = Math.Acos(distance);
            distance = distance * 180 / Math.PI;
            distance = distance * 60 * 1.1515;

            return unitOfLength.ConvertFromMiles(distance);
        }
    }
}