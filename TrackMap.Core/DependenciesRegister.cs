using System;
using TrackMap.Touch;

namespace TrackMap.Core
{
	public class DependenciesRegister : IDependenciesRegister
	{
		protected DependenciesRegister(AppTranslation appTranslation)
		{
			AppTranslation = appTranslation;
		}

		protected AppTranslation AppTranslation { get; }
		public virtual void RegisterCommon()
		{
		}
	}
}