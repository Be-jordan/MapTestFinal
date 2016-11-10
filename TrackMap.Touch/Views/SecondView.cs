using System;
using System.Drawing;
using CoreGraphics;
using CoreLocation;
using Foundation;
using MapKit;
using MvvmCross.iOS.Views;
using UIKit;
using MvvmCross.Core.Views;
using TrackMap.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.iOS.Views;
namespace TrackMap.Core.Touch.Views
{
	public class SecondView : MvxViewController
	{
		public string UserName { get; set; }
		private MKMapViewDelegate mapDelegate;
		private MKMapView map;
		private UISegmentedControl mapTypes;
		private CLLocationManager locationManager = new CLLocationManager();

		UISearchController searchController;

		public SecondView()
		{
			UserName = string.Empty;
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			map = new MKMapView(new RectangleF(0, 0, 320, 620))
			{
				ShowsUserLocation = true,
				ZoomEnabled = true,
				ScrollEnabled = true
			};

			Add(map);

			var searchResultsController = new SearchResultsViewController(map);

			var searchUpdater = new SearchResultsUpdator();
			searchUpdater.UpdateSearchResults += searchResultsController.Search;

			searchController = new UISearchController(searchResultsController)
			{
				SearchResultsUpdater = searchUpdater
			};

			searchController.SearchBar.SizeToFit();
			searchController.SearchBar.SearchBarStyle = UISearchBarStyle.Minimal;
			searchController.SearchBar.Placeholder = "Enter a search query";
			searchController.HidesNavigationBarDuringPresentation = false;

			DefinesPresentationContext = true;
			NavigationItem.TitleView = searchController.SearchBar;

			map.Delegate = mapDelegate;

			var secondViewModel = (SecondViewModel)ViewModel;

			CreateRoute();
		}

		private void CreateRoute()
		{
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
		public class SearchResultsViewController : UITableViewController
		{
			static readonly string mapItemCellId = "mapItemCellId";
			MKMapView map;

			public List<MKMapItem> MapItems { get; set; }

			public SearchResultsViewController(MKMapView map)
			{
				this.map = map;

				MapItems = new List<MKMapItem>();
			}

			public override nint RowsInSection(UITableView tableView, nint section)
			{
				return MapItems.Count;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell(mapItemCellId);

				if (cell == null)
					cell = new UITableViewCell();

				cell.TextLabel.Text = MapItems[indexPath.Row].Name;
				return cell;
			}
			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				// add item to map
				CLLocationCoordinate2D coord = MapItems[indexPath.Row].Placemark.Location.Coordinate;
				map.AddAnnotations(new MKPointAnnotation()
				{
					Title = MapItems[indexPath.Row].Name,
					Coordinate = coord
				});
				map.SetCenterCoordinate(coord, true);

				CLLocationCoordinate2D coords = map.UserLocation.Coordinate;
				NSDictionary marker1 = new NSDictionary();
				var orignPlaceMark = new MKPlacemark((coords), marker1);
				var sourceItem = new MKMapItem(orignPlaceMark);


				var destPlaceMark = new MKPlacemark((coord), marker1);
				var destItem = new MKMapItem(destPlaceMark);

				var go = new MKDirectionsRequest
				{

					Source = sourceItem,
					Destination = destItem,
					RequestsAlternateRoutes = true
				};

				var line = new MKDirections(go);

				line.CalculateDirections((response, error) =>
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
				var save = new NSString();

			});

				DismissViewController(false, null);
			}
			public void Search(string forSearchString)
			{
				// create search request
				var searchRequest = new MKLocalSearchRequest();
				searchRequest.NaturalLanguageQuery = forSearchString;
				searchRequest.Region = new MKCoordinateRegion(map.UserLocation.Coordinate, new MKCoordinateSpan(0.25, 0.25));

				// perform search
				var localSearch = new MKLocalSearch(searchRequest);

				localSearch.Start(delegate (MKLocalSearchResponse response, NSError error)
				{
					if (response != null && error == null)
					{
						this.MapItems = response.MapItems.ToList();
						this.TableView.ReloadData();
					}
					else {
						Console.WriteLine("local search error: {0}", error);
					}
				});

			}
		}
		public class SearchResultsUpdator : UISearchResultsUpdating
		{
			public event Action<string> UpdateSearchResults = delegate { };

			public override void UpdateSearchResultsForSearchController(UISearchController searchController)
			{
				this.UpdateSearchResults(searchController.SearchBar.Text);
			}
		}
	}
}
