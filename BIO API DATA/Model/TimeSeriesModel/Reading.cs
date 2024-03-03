using BIO_API_DATA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.Model.TimeSeriesModel
{
	public class Reading
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public string Resolution { get; set; }
		public string Unit { get; set; }
		public List<Observation> Observations { get; set; }
	}
}
