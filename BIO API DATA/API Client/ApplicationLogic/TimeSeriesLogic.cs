using BIO_API_DATA.API_Client.Database;
using BIO_API_DATA.API_Client.Interfaces;
using BIO_API_DATA.API_Client.Interfaces.Database;
using BIO_API_DATA.Data;
using BIO_API_DATA.Model.CompositObjectModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.API_Client.ApplicationLogic
{
    public class TimeSeriesLogic : ITimeSeriesLogic
    {
        private readonly ITimeSeriesClient _timeSeriesClient;
        private readonly ITopLevelCustomersClientList _topLevelCustomers;
        private readonly IGasMeteringPointCustomerClientList _gasMeteringPointCustomerClientList;
        private readonly IGasMeteringPointCustomerClient _gasMeteringPointCustomer;
        private readonly ICustomerRepository _customerRepository;
        private readonly IGasMeterCustomerRelationRepository _gasMeterCustomerRelationRepository;
        private readonly IGasMeterMeasurementRepository _gasMeterMeasurementRepository;
        private readonly IObservationRepository _observationRepository;
        private readonly IGasMeteringPointRepository _gasMeteringPointRepository;
        private readonly IUnitOfWork _unitOfWork;


        public TimeSeriesLogic(ITimeSeriesClient timeSeriesClient, 
            ITopLevelCustomersClientList topLevelCustomers, 
            IGasMeteringPointCustomerClientList gasMeteringPointCustomerClient,
            IGasMeteringPointCustomerClient gasMeteringPointCustomer,
            ICustomerRepository customerRepository,
            IGasMeterCustomerRelationRepository gasMeterCustomerRelationRepository,
            IGasMeterMeasurementRepository gasMeterMeasurementRepository,
            IObservationRepository observationRepository,
            IGasMeteringPointRepository gasMeteringPointRepository,
            IUnitOfWork unitOfWork
            )
        {
            _timeSeriesClient = timeSeriesClient;
            _topLevelCustomers = topLevelCustomers;
            _gasMeteringPointCustomerClientList = gasMeteringPointCustomerClient;
            _gasMeteringPointCustomer = gasMeteringPointCustomer;
            _customerRepository = customerRepository;
            _gasMeterCustomerRelationRepository = gasMeterCustomerRelationRepository;
            _gasMeterMeasurementRepository = gasMeterMeasurementRepository;
            _observationRepository = observationRepository;
            _gasMeteringPointRepository = gasMeteringPointRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddTimeSeriesAsync() 
        {
            var customerIds = await _topLevelCustomers.GetAllCustomers();
            var gastMeteringPoint = await _gasMeteringPointCustomerClientList.GetGasmeteringPointCustomerassociation(customerIds);
            var customerGasRelations = await _gasMeteringPointCustomer.GetGasCustomer(gastMeteringPoint);
            var compositModel = await _timeSeriesClient.GetTimeSeries(customerIds, customerGasRelations);

            var mapCompositToEntity = MapData(compositModel, _gasMeterMeasurementRepository);

            foreach (var tuple in mapCompositToEntity) {
                _gasMeteringPointRepository.Add(tuple.Item1);
                _gasMeterCustomerRelationRepository.DeactivateLastRelation((int)tuple.Item2.CustomerId);
                _gasMeterCustomerRelationRepository.Add(tuple.Item2);
                _customerRepository.Add(tuple.Item3);
                _gasMeterMeasurementRepository.AddOrUpdate(tuple.Item4, e =>
                            e.MeteringPointIdentification == tuple.Item4.MeteringPointIdentification &&
                            e.Resolution == tuple.Item4.Resolution &&
                            e.Unit == tuple.Item4.Unit
                        );
                _observationRepository.Add(tuple.Item5);
            }

            
            await _unitOfWork.SaveChangesAsync();
        }

        public List<(GasMeteringPoint, GasMeterCustomerRelation, Customer, GasMeterMeasurement, Observation)> MapData(List<CompositModel> compositModel, IGasMeterMeasurementRepository gasMeterRepository)
        {
            var tupleList = new List<(GasMeteringPoint, GasMeterCustomerRelation, Customer, GasMeterMeasurement, Observation)>();
            var gasMeteringPointEntity = new Data.GasMeteringPoint();
            var gasMeterCustomerRelationEntity = new GasMeterCustomerRelation();
            var customerEntity = new Data.Customer();
            var gasMeterMeasurementEntity = new GasMeterMeasurement();
            var observationEntity = new Data.Observation();

            foreach (var item in compositModel)
            {
                // Create a customer
                var customer = new Data.Customer()
                {
                    Id = int.Parse(item.Customer.id),
                    CustomerNumber = item.Customer.identifiers.customerNumber,
                    EffectiveStartTimeUtc = item.Customer.parent.start,
                    EffectiveEndTimeUtc = item.Customer.parent.end,
                    VatIdentification = item.Customer.identifiers.vatIdentification,
                    CustomerName = item.Customer.contactInformation.customerName.fullName,
                    Country = item.Customer.contactInformation.address.countryCode,
                    MunicipalityCode = item.Customer.contactInformation.address.municipalityCode,
                    PostalCode = item.Customer.contactInformation.address.postalCode,
                    City = item.Customer.contactInformation.address.city,
                    StreetName = item.Customer.contactInformation.address.streetName,
                    BuildingNumber = item.Customer.contactInformation.address.buildingNumber,
                    BuildingFloor = item.Customer.contactInformation.address.buildingFloor,
                    RoomIdentification = item.Customer.contactInformation.address.roomIdentification,
                    Source = "Web"
                };

                // Create a gas metering point
                var gasMeteringPoint = new Data.GasMeteringPoint
                {
                    Id = int.Parse(item.GasMeteringCustomerObjectModel.Id),
                    MeterId = int.Parse(item.GasMeteringCustomerObjectModel.Identifiers.MeteringPointIdentification),
                    EffectiveStartTimeUtc = item.GasMeteringCustomerObjectModel.DeliveryStatus.Start,
                    EffectiveEndTimeUtc = item.GasMeteringCustomerObjectModel.DeliveryStatus.End,
                    InDelivery = item.GasMeteringCustomerObjectModel.DeliveryStatus.InDelivery,
                    PriceAreaCode = "",
                    InstallationCountry = item.GasMeteringCustomerObjectModel.InstallationAddress.CountryCode,
                    InstallationMunicipalityCode = item.GasMeteringCustomerObjectModel.InstallationAddress.MunicipalityCode,
                    InstalationPostalCode = item.GasMeteringCustomerObjectModel.InstallationAddress.PostalCode,
                    InstallationCity = item.GasMeteringCustomerObjectModel.InstallationAddress.City,
                    InstallationStreetName = item.GasMeteringCustomerObjectModel.InstallationAddress.StreetName,
                    InstallationBuildingNumber = item.GasMeteringCustomerObjectModel.InstallationAddress.BuildingNumber,
                    InstallationBuildingFloor = item.GasMeteringCustomerObjectModel.InstallationAddress.BuildingFloor,
                    InstallationRoomIdentification = item.GasMeteringCustomerObjectModel.InstallationAddress.RoomIdentification,
                    Source = "Web"
                };


                var effectiveStartTimeFromRelation = GetEffectiveStartDateFromRelation(customer.Id, gasMeteringPoint.Id);
                // Create a gas meter customer relation
                var gasMeterCustomerRelation = new GasMeterCustomerRelation
                {
                    EffectiveStartTimeUtc = effectiveStartTimeFromRelation != null? effectiveStartTimeFromRelation : item.GasMeteringCustomerObjectModel.Customer.Start,
                    EffectiveEndTimeUtc = null,
                    Customer = customer,
                    GasMeteringPoint = gasMeteringPoint,
                    Source = "Web"
                };

                GasMeterMeasurement gasMeterMeasurement = new GasMeterMeasurement();

                foreach (var measurement in item.TimeSeries.Readings)
                {
                    // Create a gas meter measurement
                    gasMeterMeasurement = new GasMeterMeasurement
                    {
                        StartUtc = measurement.Start,
                        EndUtc = measurement.End,
                        Resolution = measurement.Resolution,
                        Unit = measurement.Unit,
                        MeteringPointIdentificationNavigation = gasMeteringPoint
                    };
                }


                //Check if correction
                var isCorrection = item.TimeSeries.Readings.FirstOrDefault(measurement => {
                    return gasMeterRepository.GetAll()
                        ?.FirstOrDefault(e =>
                            e.StartUtc == measurement.Start &&
                            e.EndUtc == measurement.End &&
                            e.Resolution == measurement.Resolution &&
                            e.Unit == measurement.Unit &&
                            e.MeteringPointIdentification == gasMeteringPoint.Id
                        ) != null;
                }) != null;

                var observation = new Data.Observation();

                foreach (var read in item.TimeSeries.Readings)
                {
                    foreach (var obs in read.Observations)
                    {
                        observation = new Data.Observation
                        {
                            Quality = obs.Quality,
                            Value = obs.Value,
                            Position = obs.Position,
                            Correction = isCorrection,
                            GasMeterMeasurement = gasMeterMeasurement,
                            LastChanged = isCorrection ? DateTime.UtcNow : null
                        };
                    }
                }
                tupleList.Add((gasMeteringPointEntity, gasMeterCustomerRelation, customer,gasMeterMeasurement, observation));
            }

            return tupleList;
        }

        public DateTime? GetEffectiveStartDateFromRelation(long customerId,long gasMeteringId)
        {
            var gasMeteringRelation =  _gasMeterCustomerRelationRepository.GetAll()
                                        .FirstOrDefault(x => 
                                            x.CustomerId == customerId && 
                                            x.GasMeteringPointId == gasMeteringId
                                        );

            if (gasMeteringRelation != null)
            {
                return DateTime.UtcNow;
            }
            return null;
        }
    }  
}
