using BIO_API_DATA.API_Client.Interfaces.Database;
using BIO_API_DATA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.API_Client.Database
{

    public class ObservationRepository : BaseRepository<Observation, BioDataContext> , IObservationRepository
    {
        public ObservationRepository(BioDataContext context) : base(context)
        {

        }

    }
}
