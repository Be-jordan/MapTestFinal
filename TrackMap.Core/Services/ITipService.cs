using System;
namespace TrackMap.Core
{
	public interface ITipService
	{
		double Calc(double subTotal, int generosity);
	}

}