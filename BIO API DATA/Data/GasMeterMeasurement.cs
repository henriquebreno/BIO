using System;
using System.Collections.Generic;

namespace BIO_API_DATA.Data;

public partial class GasMeterMeasurement
{
    public long Id { get; set; }

    public DateTime? StartUtc { get; set; }

    public DateTime? EndUtc { get; set; }

    public string? Resolution { get; set; }

    public string? Unit { get; set; }

    public long? MeteringPointIdentification { get; set; }

    public virtual GasMeteringPoint? MeteringPointIdentificationNavigation { get; set; }

    public virtual ICollection<Observation> Observations { get; set; } = new List<Observation>();

    public DateTime? LastChangedUtc { get; set; }
}
