using System;
using DistribuidoraDoChines.Commons.Helpers;
using GoogleMaps.LocationServices;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DistribuidoraDoChines.Commons.Models
{
    public class Delivery
    {
        private const string ApiKey = "AIzaSyC4ns7q2uewD7IEeleld1d8vfQ2A9zGcqU";

        private static readonly AddressData OriginAddress = new AddressData
        {
            Address = "Av. Ministro Albuquerque Lima, 1550",
            City = "Fortaleza",
            State = "Ceará",
            Country = "Brazil",
            Zip = "60533697"
        };

        private readonly GoogleLocationService _locationService;

        public Delivery(Enderecos endereco)
        {
            _locationService = new GoogleLocationService(ApiKey);

            GetDeliveryDistance(endereco);

            if (IsDeliveryAvailable())
                GetDeliveryPrice();
        }

        private AddressData DestinationAddress { get; set; }

        public bool Availability { get; set; }

        public double Distance { get; set; }

        public decimal Price { get; set; }

        private void GetDeliveryDistance(Enderecos endereco)
        {
            DestinationAddress = Mappers.MapEnderecoToDestinationAddress(endereco);

            var directions = _locationService.GetDirections(OriginAddress, DestinationAddress);

            Distance = Convert.ToDouble(directions.Distance
                .Replace("k", "")
                .Replace("m", "")
            );
        }

        private bool IsDeliveryAvailable()
        {
            return !(Distance > 6);
        }

        private bool IsDeliveryFree()
        {
            return Distance < 3;
        }

        private void GetDeliveryPrice()
        {
            Availability = true;
            Price = IsDeliveryFree() ? 0 : (decimal) (Distance * 1.5);
        }
    }
}