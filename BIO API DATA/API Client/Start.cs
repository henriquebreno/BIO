using BIO_API_DATA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.API_Client
{
	public class Start
	{

		public void Run()
		{
			var _context = new BioDataContext();

			// Create a customer
			var customer = new Customer
			{
				Id = 1,
				CustomerNumber = "12345",
				EffectiveStartTimeUtc = DateTime.UtcNow,
				EffectiveEndTimeUtc = DateTime.UtcNow.AddDays(365),
				VatIdentification = "VAT123",
				CustomerName = "Test Customer",
				Country = "USA",
				MunicipalityCode = "MUN123",
				PostalCode = "12345",
				City = "New York",
				StreetName = "Broadway",
				BuildingNumber = "123",
				BuildingFloor = "5",
				RoomIdentification = "Room 501",
				Source = "Web"
			};

			// Create a gas metering point
			var gasMeteringPoint = new GasMeteringPoint
			{
				Id = 1,
				MeterId = 1,
				EffectiveStartTimeUtc = DateTime.UtcNow,
				EffectiveEndTimeUtc = DateTime.UtcNow.AddDays(365),
				InDelivery = true,
				PriceAreaCode = "PRICE123",
				InstallationCountry = "USA",
				InstallationMunicipalityCode = "MUN123",
				InstalationPostalCode = "12345",
				InstallationCity = "New York",
				InstallationStreetName = "Broadway",
				InstallationBuildingNumber = "123",
				InstallationBuildingFloor = "5",
				InstallationRoomIdentification = "Room 501",
				Source = "Web"
			};

			// Create a gas meter customer relation
			var gasMeterCustomerRelation = new GasMeterCustomerRelation
			{
				EffectiveStartTimeUtc = DateTime.UtcNow,
				EffectiveEndTimeUtc = DateTime.UtcNow.AddDays(365),
				Customer = customer,
				GasMeteringPoint = gasMeteringPoint,
				Source = "Web"
			};

			// Create a gas meter measurement
			var gasMeterMeasurement = new GasMeterMeasurement
			{
				Start = DateTime.UtcNow,
				End = DateTime.UtcNow.AddHours(1),
				Resolution = "Hourly",
				Unit = "Cubic meters",
				MeteringPointIdentificationNavigation = gasMeteringPoint
			};

			// Create an observation
			var observation = new Observation
			{
				Quality = "Good",
				Value = 100,
				Position = 1,
				Correction = false,
				GasMeterMeasurement = gasMeterMeasurement
			};

			// Add entities to context
			_context.Customers.Add(customer);
			_context.GasMeteringPoints.Add(gasMeteringPoint);
			_context.GasMeterCustomerRelations.Add(gasMeterCustomerRelation);
			_context.GasMeterMeasurements.Add(gasMeterMeasurement);
			_context.Observations.Add(observation);

			// Save changes to the database
			_context.SaveChanges();
		}

	}
}

