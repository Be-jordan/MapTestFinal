using System;
using CoreGraphics;
using CoreLocation;
using Foundation;
using MapKit;
using MvvmCross.iOS.Views;
using UIKit;
using MvvmCross.Core.ViewModels;
using TrackMap.Core.ViewModels;

namespace TrackMap.Core.Touch.Views
{
	public class SecondView : MvxViewController
	{
		private MKMapViewDelegate mapDelegate;
		private MKMapView map;
		private UISegmentedControl mapTypes;
		private CLLocationManager locationManager = new CLLocationManager();
		public override void ViewDidLoad()
		{
			map = new MKMapView(View.Bounds)
			{
				ShowsUserLocation = true,
				ZoomEnabled = true,
				ScrollEnabled = true
			};
			View = map;
			base.ViewDidLoad();
			map.Delegate = mapDelegate;

			var secondViewModel = (SecondViewModel)ViewModel;

			CreateRoute();
		}

		private void CreateRoute()
		{
			NSDictionary marker = new NSDictionary();

			var orignPlaceMark = new MKPlacemark(new CLLocationCoordinate2D(37.797530, -122.402590), marker);
			var sourceItem = new MKMapItem(orignPlaceMark);

			var destPlaceMark = new MKPlacemark(new CLLocationCoordinate2D(42.374172, -71.120639), marker);
			var destItem = new MKMapItem(destPlaceMark);

			var request = new MKDirectionsRequest
			{
				Source = sourceItem,
				Destination = destItem,
				RequestsAlternateRoutes = true
			};

			var directions = new MKDirections(request);

			directions.CalculateDirections((response, error) =>
			{
				if (error != null)
				{
					Console.WriteLine(error.LocalizedDescription);
				}
				else
				{
					foreach (var route in response.Routes)
					{
						map.AddOverlay(route.Polyline);
					}
				}
			});

			map.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			map.MapType = MKMapType.Standard;

			int typesWidth = 260, typesHeight = 30, distanceFromBottom = 60;
			mapTypes = new UISegmentedControl(new CGRect((View.Bounds.Width - typesWidth) / 2, View.Bounds.Height - distanceFromBottom, typesWidth, typesHeight));
			mapTypes.InsertSegment("Road", 0, false);
			mapTypes.InsertSegment("Satellite", 1, false);
			mapTypes.InsertSegment("Hybrid", 2, false);
			mapTypes.SelectedSegment = 0; // Road is the default
			mapTypes.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin;
			mapTypes.ValueChanged += (s, e) =>
			{
				switch (mapTypes.SelectedSegment)
				{
					case 0:
						map.MapType = MKMapType.Standard;
						break;
					case 1:
						map.MapType = MKMapType.Satellite;
						break;
					case 2:
						map.MapType = MKMapType.Hybrid;
						break;
				}
			};
			if (map.UserLocation != null)
			{
				CLLocationCoordinate2D coords = map.UserLocation.Coordinate;
				MKCoordinateSpan span = new MKCoordinateSpan(MilesToLatitudeDegrees(2), MilesToLongitudeDegrees(2, coords.Latitude));
				map.Region = new MKCoordinateRegion(coords, span);
			}
			if (map.UserLocationVisible)
			{
				//user denied permission, default to cupertino.
				CLLocationCoordinate2D coords = new CLLocationCoordinate2D(37.33233141, -122.0312186);
				MKCoordinateSpan span = new MKCoordinateSpan(MilesToLatitudeDegrees(20), MilesToLongitudeDegrees(20, coords.Latitude));
				map.Region = new MKCoordinateRegion(coords, span);
			}
			map.AddSubview(mapTypes);
			map.UserTrackingMode = MKUserTrackingMode.Follow;
			map.Delegate = new MapDelegate();

			locationManager.RequestWhenInUseAuthorization();
			map.ShowsUserLocation = true;

			Add(mapTypes);
		}
		public double MilesToLatitudeDegrees(double miles)
		{
			double earthRadius = 3960.0; //in miles
			double radiansToDegrees = 180.0 / Math.PI;
			return (miles / earthRadius) * radiansToDegrees;
		}
		public double MilesToLongitudeDegrees(double miles, double atLatitude)
		{
			double earthRadius = 3960.0; //in miles
			double degreesToRadians = Math.PI / 180.0;
			double radiansToDegrees = 180.0 / Math.PI;
			//derive the earth's radius at theat point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
			return (miles / radiusAtLatitude) * radiansToDegrees;
		}
		class MapDelegate : MKMapViewDelegate
		{
			//Override OverLayRenderer to draw Polyline returned from directions
			public override MKOverlayRenderer OverlayRenderer(MKMapView mapView, IMKOverlay overlay)
			{
				if (overlay is MKPolyline)
				{
					var route = (MKPolyline)overlay;
					var renderer = new MKPolylineRenderer(route) { StrokeColor = UIColor.Blue };
					return renderer;
				}
				return null;
			}
		}
	}
}