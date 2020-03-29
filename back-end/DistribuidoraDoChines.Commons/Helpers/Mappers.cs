using DistribuidoraDoChines.Commons.Models;
using GoogleMaps.LocationServices;

namespace DistribuidoraDoChines.Commons.Helpers
{
    public static class Mappers
    {
        internal static AddressData MapEnderecoToDestinationAddress(Enderecos endereco)
        {
            return new AddressData
            {
                Address = $"{endereco.Rua}, {endereco.Numero}",
                City = "Fortaleza",
                State = "Ceará",
                Country = "Brazil",
                Zip = endereco.Cep
            };
        }
    }
}