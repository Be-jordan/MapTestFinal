using NUnit.Framework;

using TrackMap.Core;

namespace TrackMap.Test
{
	[TestFixture()]
	public class Test
	{
		[Test]
		public void TestThatZeroGenerosityReturnsZero()
		{
			//Arrange
			var tip = new TipService();

			//Act
			var result = tip.Calc(42.35, 0);

			//Assert
			Assert.AreEqual(0, result);
		}
	}
}
