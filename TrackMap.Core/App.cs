using TrackMap.Core.ViewModels;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;

namespace TrackMap.Core
{
    public class App : MvxApplication
	{
		public App()
		{
			var dr = new DependenciesRegister();
			dr.RegisterCommon();

			Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<FirstViewModel>());
		}
    
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<FirstViewModel>();
        }
    }
}