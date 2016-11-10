using System;
namespace TrackMap.Core
{
	public class TipService : ITipService
	{
		public double Calc(double subTotal, int generosity)
		{
			return subTotal * generosity / 100.0;
		}

	}
}