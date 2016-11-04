using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;
using Foundation;
using System.Drawing;
using MvvmCross.Core.Views;
using TrackMap.Core;

namespace TrackMap.Touch.Views
{

	public partial class FirstView : MvxViewController
	{

		public override void ViewDidLoad()
		{
			this.Title = "Main Menu";

			base.ViewDidLoad();
			View = new UIView() { BackgroundColor = UIColor.White };
			var button2 = new UIButton(UIButtonType.RoundedRect);
			button2.SetTitle("Map", UIControlState.Normal);
			button2.Frame = new RectangleF(-120, 150, 300, 40);
			Add(button2);

			var set = this.CreateBindingSet<FirstView, TrackMap.Core.ViewModels.FirstViewModel>();
			set.Bind(button2).To(vm => vm.GoSecondCommand);
			set.Apply();
		}
	}

}