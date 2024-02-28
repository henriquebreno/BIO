using System;
using System.Collections.Generic;

namespace BIO_API_DATA.Data;

public partial class GasMeterMeasurement
{
    public long Id { get; set; }

    public DateOnly? Start { get; set; }

    public DateOnly? End { get; set; }

    public string? Resolution { get; set; }

    public string? Unit { get; set; }

    public virtual ICollection<GasMeteringPoint> GasMeteringPoints { get; set; } = new List<GasMeteringPoint>();

    public virtual ICollection<Observation> Observations { get; set; } = new List<Observation>();
}
