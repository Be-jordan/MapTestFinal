using MvvmCross.Core.ViewModels;
using TrackMap.Core.ViewModels;

namespace TrackMap.Core.ViewModels
{
	public class FirstViewModel
		: MvxViewModel
	{
		private readonly IAppTranslation _appTranslation;
		public FirstViewModel(IAppTranslation appTranslation)
		{
			_appTranslation = appTranslation;
		}
		public override void Start()
		{
			_subTotal = 100;
			_generosity = 10;
			Recalcuate();
			base.Start();
		}
		double _subTotal;

		public double SubTotal
		{
			get { return _subTotal; }
			set
			{
				_subTotal = value;
				RaisePropertyChanged(() => SubTotal);
				Recalcuate();
			}
		}
		int _generosity;

		public int Generosity
		{
			get { return _generosity; }
			set
			{
				_generosity = value;
				RaisePropertyChanged(() => Generosity);
				Recalcuate();
			}
		}
		double _tip;

		public double Tip
		{
			get { return _tip; }
			set
			{
				_tip = value;
				RaisePropertyChanged(() => Tip);
			}
		}

		private double _total;
		public double Total
		{
			get { return _total; }
			set { _total = value; RaisePropertyChanged(() => Total); }
		}
		void Recalcuate()
		{
			Tip = _appTranslation.TipAmount(SubTotal, Generosity);
			Total = SubTotal + Tip;
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

