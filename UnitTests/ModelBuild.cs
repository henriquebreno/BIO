using BIO_API_DATA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class ModelBuild
    {
        public ICollection<GasMeterMeasurement> GetGasMeterCustomerRelation() {

            var list = new List<GasMeterMeasurement> {
                {
                    new GasMeterMeasurement
                    {
                        Id = 1, 
                        StartUtc = DateTime.UtcNow.AddHours(-1),
                        EndUtc = DateTime.UtcNow, 
                        Resolution = "Some resolution", 
                        Unit = "Some unit", 
                        MeteringPointIdentification = 1, 
                        MeteringPointIdentificationNavigation = new GasMeteringPoint(), 
                        Observations = new List<Observation>()
                    }
        }
            };
            return list;           
        }

        public ICollection<Customer> GetCustomers()
        {

            var list = new List<Customer> {
                {
                     new Customer
                        {
                            Id = 1, 
                            CustomerNumber = "12345", 
                            EffectiveStartTimeUtc = DateTime.UtcNow.AddDays(-1), 
                            EffectiveEndTimeUtc = DateTime.UtcNow, 
                            VatIdentification = "123456789", 
                            CustomerName = "Nome do Cliente", 
                            MunicipalityCode = "Código Municipal", 
                            PostalCode = "Código Postal",
                            City = "Cidade", 
                            StreetName = "Nome da Rua",
                            BuildingNumber = "Número do Prédio", 
                            BuildingFloor = "Andar",
                            RoomIdentification = "Identificação do Quarto", 
                            Source = "Fonte de Dados",
                            GasMeterCustomerRelations = new List<GasMeterCustomerRelation>()  
                     }
                }
            };
            return list;
        }

        public ICollection<GasMeterCustomerRelation> GetGasMeterCustomerRelations()
        {

            var list = new List<GasMeterCustomerRelation> {
                {
                     new GasMeterCustomerRelation
                    {
                        Id = 1,
                        EffectiveStartTimeUtc = DateTime.UtcNow,
                        EffectiveEndTimeUtc = DateTime.UtcNow.AddDays(1),
                        CustomerId = 1,
                        GasMeteringPointId = 1,
                        Source = "Some Source",
                        Customer = new Customer(),
                        GasMeteringPoint = new GasMeteringPoint()
                    }
                }
            };
            return list;
        }

        public ICollection<GasMeteringPoint> GetGasMeteringPoints()
        {

            var list = new List<GasMeteringPoint> {
                {
                    new GasMeteringPoint
                    {
                        Id = 1,
                        MeterId = 123456789,
                        EffectiveStartTimeUtc = DateTime.UtcNow,
                        EffectiveEndTimeUtc = DateTime.UtcNow.AddDays(1),
                        InDelivery = true,
                        PriceAreaCode = "123",
                        InstallationCountry = "Country",
                        InstallationMunicipalityCode = "MunicipalityCode",
                        InstalationPostalCode = "PostalCode",
                        InstallationCity = "City",
                        InstallationStreetName = "StreetName",
                        InstallationBuildingNumber = "BuildingNumber",
                        InstallationBuildingFloor = "BuildingFloor",
                        InstallationRoomIdentification = "RoomIdentification",
                        CalorificValueAreaId = 1,
                        Source = "Source",
                        GasMeterCustomerRelations = new List<GasMeterCustomerRelation>(),
                        GasMeterMeasurement = new GasMeterMeasurement()
                    }
                }
            };
            return list;
        }


        public ICollection<Observation> GetObservations()
        {

            var list = new List<Observation> {
                {
                     new Observation
                    {
                        Id = 1,
                        GasMeterMeasurementId = 1,
                        Quality = "Some Quality",
                        Value = 10.5m,
                        Position = 1,
                        Correction = true,
                        GasMeterMeasurement = new GasMeterMeasurement()
                    }
                }
            };
            return list;
        }
    }
}
