using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using TrackMap.Core.ViewModels;
using TrackMap.Touch;

namespace TrackMap.Core
{
	public class DependenciesRegister : IDependenciesRegister
	{
		public virtual void RegisterCommon()
		{
			Mvx.RegisterType<IAppTranslation, AppTranslation>();
		}
	}
}