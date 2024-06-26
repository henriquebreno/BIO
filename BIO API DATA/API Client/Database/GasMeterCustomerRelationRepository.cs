﻿using BIO_API_DATA.API_Client.Interfaces.Database;
using BIO_API_DATA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.API_Client.Database
{

    public class GasMeterCustomerRelationRepository : BaseRepository<GasMeterCustomerRelation, BioDataContext>, IGasMeterCustomerRelationRepository
    {
        public GasMeterCustomerRelationRepository(BioDataContext context) : base(context)
        {

        }
        public void DeactivateLastRelation(long customerId) 
        {
            var relation = this.GetAll().LastOrDefault(x => x.CustomerId == customerId);
            if (relation != null) {
                relation.EffectiveEndTimeUtc = DateTime.UtcNow;
                this.Update(relation);
            }
        }

    }
}
