using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.Model.TimeSeriesModel
{
	public class Observation
	{
		public string Quality { get; set; }
		public int Value { get; set; }
		public int Position { get; set; }
	}
}
