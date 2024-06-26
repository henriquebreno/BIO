﻿using System;
using System.Collections.Generic;

namespace BIO_API_DATA.Data;

public partial class GasMeterCustomerRelation
{
    public long Id { get; set; }

    public DateTime? EffectiveStartTimeUtc { get; set; }

    public DateTime? EffectiveEndTimeUtc { get; set; }

    public long? CustomerId { get; set; }

    public long? GasMeteringPointId { get; set; }

    public string? Source { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual GasMeteringPoint? GasMeteringPoint { get; set; }
}
