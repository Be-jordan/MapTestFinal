using System;
namespace TrackMap.Core
{
	public class AppTranslation : IAppTranslation
	{
		public double TipAmount(double subTotal, double generosity)
		{
			return subTotal * generosity / 100.0;
		}
	}
}
