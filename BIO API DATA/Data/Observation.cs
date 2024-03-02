using System;
using System.Collections.Generic;

namespace BIO_API_DATA.Data;

public partial class Observation
{
    public long Id { get; set; }

    public long? GasMeterMeasurementId { get; set; }

    public string? Quality { get; set; }

    public decimal? Value { get; set; }

    public int? Position { get; set; }

    public bool? Correction { get; set; }

    public virtual GasMeterMeasurement? GasMeterMeasurement { get; set; }
}
