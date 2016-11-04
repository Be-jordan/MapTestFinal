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
			var label = new UILabel(new RectangleF(10, 0, 300, 40));
			label.Text = "SubtTotal";
			Add(label);
			var label2 = new UILabel(new RectangleF(10, 80, 300, 40));
			label2.Text = "Generosity?";
			Add(label2);
			var slider = new UISlider(new RectangleF(10, 120, 300, 40));
			slider.MinValue = 0;
			slider.MaxValue = 100;
			Add(slider);
			var label3 = new UILabel(new RectangleF(10, 0, 300, 40));
			label3.Text = "Tip";
			Add(label3);
			var tipLabel = new UILabel(new RectangleF(10, 200, 300, 40));
			Add(tipLabel);
			var label4 = new UILabel(new RectangleF(10, 240, 300, 40));
			label3.Text = "Tip";
			Add(label4);
			var totalLabel = new UILabel(new RectangleF(10, 160, 300, 40));
			Add(totalLabel);
			var subTotalTextField = new UITextField(new RectangleF(10, 40, 300, 40));
			Add(subTotalTextField);
			var button2 = new UIButton(UIButtonType.RoundedRect);
			button2.SetTitle("Map", UIControlState.Normal);
			button2.Frame = new RectangleF(-120, 150, 300, 40);
			Add(button2);

			var set = this.CreateBindingSet<FirstView, TrackMap.Core.ViewModels.FirstViewModel>();
			set.Bind(subTotalTextField).To(vm => vm.SubTotal);
			set.Bind(slider).To(vm => vm.Generosity);
			set.Bind(tipLabel).To(vm => vm.Tip);
			set.Bind(totalLabel).To(vm => vm.Total);
			set.Bind(button2).To(vm => vm.GoSecondCommand);
			set.Apply();
		}
	}

}