﻿using BIO_API_DATA.API_Client.Interfaces;
using BIO_API_DATA.API_Client.Interfaces.Database;
using BIO_API_DATA.Data;
using BIO_API_DATA.Model.CompositObjectModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

            var mapCompositToEntity = MapData(compositModel);
            _gasMeteringPointRepository.Add(mapCompositToEntity.Item1);
            _gasMeterCustomerRelationRepository.Add(mapCompositToEntity.Item2);
            _customerRepository.Add(mapCompositToEntity.Item3);
            _gasMeterMeasurementRepository.Add(mapCompositToEntity.Item4);
            _observationRepository.Add(mapCompositToEntity.Item5);
            await _unitOfWork.SaveChangesAsync();
        }

        public (GasMeteringPoint, GasMeterCustomerRelation, Customer, GasMeterMeasurement, Observation) MapData(List<CompositModel> compositModel)
        {

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

                // Create a gas meter customer relation
                var gasMeterCustomerRelation = new GasMeterCustomerRelation
                {
                    EffectiveStartTimeUtc = item.GasMeteringCustomerObjectModel.Customer.Start,
                    EffectiveEndTimeUtc = item.GasMeteringCustomerObjectModel.Customer.End,
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
                        Start = measurement.Start,
                        End = measurement.End,
                        Resolution = measurement.Resolution,
                        Unit = measurement.Unit,
                        MeteringPointIdentificationNavigation = gasMeteringPoint
                    };
                }


                //Check if correction
                bool exist = false;
                GasMeterMeasurement check;
                foreach (var measurement in item.TimeSeries.Readings)
                {
                    //check = _dbContext.GasMeterMeasurements.FirstOrDefault(e => e.Start == measurement.Start && e.End == measurement.End);
                    //if (check != null)
                    //{
                    //    exist = true;
                    //}
                }
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
                            Correction = exist,
                            GasMeterMeasurement = gasMeterMeasurement
                        };
                    }
                }

               
            }

            return (gasMeteringPointEntity, gasMeterCustomerRelationEntity, customerEntity, gasMeterMeasurementEntity, observationEntity);
        }
    }
}