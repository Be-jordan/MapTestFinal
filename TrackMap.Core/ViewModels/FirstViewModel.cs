using MvvmCross.Core.ViewModels;
using TrackMap.Core.ViewModels;

namespace TrackMap.Core.ViewModels
{
	public class FirstViewModel
		: MvxViewModel
	{

		private MvvmCross.Core.ViewModels.MvxCommand _goSecondCommand;
		public System.Windows.Input.ICommand GoSecondCommand
		{

			get
			{
				_goSecondCommand = _goSecondCommand ?? new MvvmCross.Core.ViewModels.MvxCommand(DoGoSecond);
				return _goSecondCommand;
			}
		}

		private void DoGoSecond()
		{
			base.ShowViewModel<SecondViewModel>();
		}

	}
}

