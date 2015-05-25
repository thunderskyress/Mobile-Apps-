using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ToDoList.Resources;
// Directive for the ViewModel.
using LocalDatabaseSample.Model;
using Windows.Devices.Geolocation;
using System.Device.Location;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Maps.Controls;


namespace ToDoList
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinate drawCoordinate;
        MapRoute MyMapRoute = null;
        Geolocator MyGeolocator = new Geolocator();
        Geoposition MyGeoPosition = null;
        RouteQuery MyQuery = null;
        GeocodeQuery MygeocodeQuery = null;
        List<GeoCoordinate> Mycoordinates = new List<GeoCoordinate>();
        private Geolocator locator = null;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.GetCoordinates();
            if (locator == null)
            {
                locator = new Geolocator();
                locator.DesiredAccuracy = PositionAccuracy.High;
                locator.MovementThreshold = 20;
                locator.PositionChanged += locator_PositionChanged;
                locator.StatusChanged += locator_StatusChanged;
            }
            // Set the page DataContext property to the ViewModel.
            this.DataContext = App.ViewModel;

           
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private async void GetCoordinates()
        {
            
            MyGeolocator.DesiredAccuracyInMeters = 5;
            
            try
            {
                MyGeoPosition = await MyGeolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1),TimeSpan.FromSeconds(10));
                Mycoordinates.Add(new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude));
               
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("locatie niet toegestaan in settings");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
     
        void MygeocodeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Error == null)
            {
                MyQuery = new RouteQuery();
                Mycoordinates.Add(e.Result[0].GeoCoordinate);
                MyQuery.Waypoints = Mycoordinates;
                MyQuery.QueryCompleted += MyQuery_QueryCompleted;
                MyQuery.QueryAsync();
                MygeocodeQuery.Dispose();
            }
        }

        void MyQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (e.Error == null)
            {
               
                Route myRoute = e.Result;
                MyMapRoute = new MapRoute(myRoute);
                myLocationMap.AddRoute(MyMapRoute);
                MyQuery.Dispose();
            }
        }

        void locator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    updateStatus(args.Status);
                });
        }



        void locator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    updateDisplay(args.Position);
                });
        }

        private void updateDisplay(Geoposition position)
        {
            drawCoordinate = new GeoCoordinate(position.Coordinate.Point.Position.Latitude, position.Coordinate.Point.Position.Longitude);
            myLocationMap.Center = drawCoordinate;
            myLocationMap.ZoomLevel = 12;
        }
        private void updateStatus(PositionStatus status)
        {

        }

        private void newTaskAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewTaskPage.xaml", UriKind.Relative));
        }


        private void deleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                ToDoItem toDoForDelete = button.DataContext as ToDoItem;

                App.ViewModel.DeleteToDoItem(toDoForDelete);
            }

            this.Focus();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            App.ViewModel.SaveChangesToDB();
        }

        private void ChangeRouteAll(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (MyMapRoute != null) 
                {
                    myLocationMap.RemoveRoute(MyMapRoute);
                    Mycoordinates.RemoveAt(1);
                    myLocationMap.Center = drawCoordinate;
                }
                
                string locatie;
                
                var selected = allToDoItemsListBox.SelectedValue as ToDoItem;
                locatie = selected.ItemName;
                
                MygeocodeQuery = new GeocodeQuery();
                MygeocodeQuery.SearchTerm = locatie;
                MygeocodeQuery.GeoCoordinate = new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude);
                MygeocodeQuery.QueryCompleted += MygeocodeQuery_QueryCompleted;
                MygeocodeQuery.QueryAsync();
                
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void ChangeRouteGenk(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (MyMapRoute != null)
                {
                    myLocationMap.RemoveRoute(MyMapRoute);
                    Mycoordinates.RemoveAt(1);
                    myLocationMap.Center = drawCoordinate;
                }

                string locatie;

               
                var selected = GenkToDoItemsListBox.SelectedValue as ToDoItem;
                locatie = selected.ItemName;
                MygeocodeQuery = new GeocodeQuery();
                MygeocodeQuery.SearchTerm = locatie;
                MygeocodeQuery.GeoCoordinate = new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude);
                MygeocodeQuery.QueryCompleted += MygeocodeQuery_QueryCompleted;
                MygeocodeQuery.QueryAsync();

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void ChangeRouteHasselt(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (MyMapRoute != null)
                {
                    myLocationMap.RemoveRoute(MyMapRoute);
                    Mycoordinates.RemoveAt(1);
                    myLocationMap.Center = drawCoordinate;
                }

                string locatie;

                var selected = HasseltToDoItemsListBox.SelectedValue as ToDoItem;
                locatie = selected.ItemName;
                MygeocodeQuery = new GeocodeQuery();
                MygeocodeQuery.SearchTerm = locatie;
                MygeocodeQuery.GeoCoordinate = new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude);
                MygeocodeQuery.QueryCompleted += MygeocodeQuery_QueryCompleted;
                MygeocodeQuery.QueryAsync();

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void ChangeRouteStTruiden(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (MyMapRoute != null)
                {
                    myLocationMap.RemoveRoute(MyMapRoute);
                    Mycoordinates.RemoveAt(1);
                    myLocationMap.Center = drawCoordinate;
                }

                string locatie;

                var selected = SttruidenToDoItemsListBox.SelectedValue as ToDoItem;
                locatie = selected.ItemName;
                MygeocodeQuery = new GeocodeQuery();
                MygeocodeQuery.SearchTerm = locatie;
                MygeocodeQuery.GeoCoordinate = new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude);
                MygeocodeQuery.QueryCompleted += MygeocodeQuery_QueryCompleted;
                MygeocodeQuery.QueryAsync();

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void ChangeRouteOverpelt(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (MyMapRoute != null)
                {
                    myLocationMap.RemoveRoute(MyMapRoute);
                    Mycoordinates.RemoveAt(1);
                    myLocationMap.Center = drawCoordinate;
                }

                string locatie;

                var selected = OverpeltToDoItemsListBox.SelectedValue as ToDoItem;
                locatie = selected.ItemName;
                MygeocodeQuery = new GeocodeQuery();
                MygeocodeQuery.SearchTerm = locatie;
                MygeocodeQuery.GeoCoordinate = new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude);
                MygeocodeQuery.QueryCompleted += MygeocodeQuery_QueryCompleted;
                MygeocodeQuery.QueryAsync();

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void ChangeRouteTongeren(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (MyMapRoute != null)
                {
                    myLocationMap.RemoveRoute(MyMapRoute);
                    Mycoordinates.RemoveAt(1);
                    myLocationMap.Center = drawCoordinate;
                }
                
                string locatie;

                var selected = TongerenToDoItemsListBox.SelectedValue as ToDoItem;
                locatie = selected.ItemName;
                MygeocodeQuery = new GeocodeQuery();
                MygeocodeQuery.SearchTerm = locatie;
                MygeocodeQuery.GeoCoordinate = new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude);
                MygeocodeQuery.QueryCompleted += MygeocodeQuery_QueryCompleted;
                MygeocodeQuery.QueryAsync();

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}