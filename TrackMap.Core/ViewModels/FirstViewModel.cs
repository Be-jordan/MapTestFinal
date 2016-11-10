using MvvmCross.Core.ViewModels;
using TrackMap.Core.ViewModels;

namespace TrackMap.Core.ViewModels
{
	public class FirstViewModel
		: MvxViewModel
	{
		private readonly ITipService _tipService;

		private readonly IAppTranslation _appTranslation;
		public FirstViewModel(IAppTranslation appTranslation)
		{
			_appTranslation = appTranslation;
		}

		public FirstViewModel(ITipService tipSerive)
		{
			_tipService = tipSerive;
		}

		private double _subtotal;
		public double Subtotal
		{
			get { return _subtotal; }
			set { _subtotal = value; RaisePropertyChanged(() => Subtotal); Update(); }
		}
		private int _generosity;
		public int Generosity
		{
			get { return _generosity; }
			set { _generosity = value; RaisePropertyChanged(() => Generosity); Update(); }
		}
		void Update()
		{
			Tip = _tipService.Calc(Subtotal, Generosity);
		}

		private double _tip;
		public double Tip
		{
			get { return _tip; }
			set { _tip = value; RaisePropertyChanged(() => Tip); }
		}
		public override void Start()
		{
			Recalculate();
			base.Start();
		}

		string _request;

		public string Request
		{
			get { return _request; }
			set
			{
				_request = value;
				RaisePropertyChanged(() => Request);
			}
		}

		void Recalculate()
		{
			Request = _appTranslation.Request();

		}
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
