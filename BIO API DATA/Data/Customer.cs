using System;
using System.Collections.Generic;

namespace BIO_API_DATA.Data;

public partial class Customer
{
    public long Id { get; set; }

    public string? CustomerNumber { get; set; }

    public DateTime? EffectiveStartTimeUtc { get; set; }

    public DateTime? EffectiveEndTimeUtc { get; set; }

    public string? VatIdentification { get; set; }

    public string? CustomerName { get; set; }

    public string? Country { get; set; }

    public string? MunicipalityCode { get; set; }

    public string? PostalCode { get; set; }

    public string? City { get; set; }

    public string? StreetName { get; set; }

    public string? BuildingNumber { get; set; }

    public string? BuildingFloor { get; set; }

    public string? RoomIdentification { get; set; }

    public string? Source { get; set; }

    public virtual ICollection<GasMeterCustomerRelation> GasMeterCustomerRelations { get; set; } = new List<GasMeterCustomerRelation>();
}
