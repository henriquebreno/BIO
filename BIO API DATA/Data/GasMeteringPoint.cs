using System;
using System.Collections.Generic;

namespace BIO_API_DATA.Data;

public partial class GasMeteringPoint
{
    public long Id { get; set; }

    public long MeterId { get; set; }

    public DateTime? EffectiveStartTimeUtc { get; set; }

    public DateTime? EffectiveEndTimeUtc { get; set; }

    public bool? InDelivery { get; set; }

    public string? PriceAreaCode { get; set; }

    public string? InstallationCountry { get; set; }

    public string? InstallationMunicipalityCode { get; set; }

    public string? InstalationPostalCode { get; set; }

    public string? InstallationCity { get; set; }

    public string? InstallationStreetName { get; set; }

    public string? InstallationBuildingNumber { get; set; }

    public string? InstallationBuildingFloor { get; set; }

    public string? InstallationRoomIdentification { get; set; }

    public long? CalorificValueAreaId { get; set; }

    public string? Source { get; set; }

    public virtual ICollection<GasMeterCustomerRelation> GasMeterCustomerRelations { get; set; } = new List<GasMeterCustomerRelation>();

    public virtual GasMeterMeasurement? GasMeterMeasurement { get; set; }
}
