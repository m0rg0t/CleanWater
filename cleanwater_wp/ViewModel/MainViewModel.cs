using GalaSoft.MvvmLight;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Windows;
using cleanwater_wp.Common;
using System.Device.Location;

namespace cleanwater_wp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private bool _loading;
        /// <summary>
        /// 
        /// </summary>
        public bool Loading
        {
            get { return _loading; }
            set { 
                _loading = value;
                RaisePropertyChanged("Loading");
                RaisePropertyChanged("IsLoaded");
                RaisePropertyChanged("LoadingVisibility");
            }
        }

        private bool _geolocationStatus = true;
        public bool GeolocationStatus
        {
            get
            {
                return _geolocationStatus;
            }
            set
            {
                if (_geolocationStatus != value)
                {
                    _geolocationStatus = value;
                    //AppSettings.StoreSetting<bool>("GeolocationStatus", value);
                    RaisePropertyChanged("GeolocationStatus");
                };
            }
        }

        private bool _location = true;
        public bool Location
        {
            get
            {
                return _location;
            }
            set
            {
                UpdateCoordinatesWatcher();
                _location = value;
            }
        }

        public void UpdateCoordinatesWatcher()
        {
            try
            {
                if (ViewModelLocator.MainStatic.GeolocationStatus)
                {
                    myCoordinateWatcher.Stop();
                    myCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                    myCoordinateWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(myCoordinateWatcher_PositionChanged);
                    myCoordinateWatcher.Start();
                };
            }
            catch { };
        }
        public GeoCoordinateWatcher myCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
        private bool _getCoordinates = false;

        void myCoordinateWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (ViewModelLocator.MainStatic.GeolocationStatus)
            {
                if (ViewModelLocator.MainStatic.Location == true)
                {
                if (((!e.Position.Location.IsUnknown) && (_getCoordinates == false)))
                {
                    Latitued = e.Position.Location.Latitude;
                    Longitude = e.Position.Location.Longitude;

                    _getCoordinates = true;
                    GetPlaceInfo(Latitued, Longitude);
                };
                }
                else
                {
                    Latitued = 55.45;
                    Longitude = 37.36;
                };
            };
        }



        public bool IsLoaded
        {
            private set
            {
            }
            get
            {
                return !Loading;
            }
        }

        public Visibility LoadingVisibility
        {
            get
            {
                if (IsLoaded)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                };
            }
            private set
            {
            }
        }

        private ObservableCollection<WaterItem> _items = new ObservableCollection<WaterItem>();
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<WaterItem> Items
        {
            get { return _items; }
            set { 
                _items = value;
                RaisePropertyChanged("Loading");
            }
        }

        private WaterItem _currentItem;
        /// <summary>
        /// Текущий элемент
        /// </summary>
        public WaterItem CurrentItem
        {
            get { return _currentItem; }
            set { 
                _currentItem = value;
                RaisePropertyChanged("Items");
            }
        }

        private RegionWaterItem _currentRegionItem;
        /// <summary>
        /// Текущий элемент
        /// </summary>
        public RegionWaterItem CurrentRegionItem
        {
            get { return _currentRegionItem; }
            set
            {
                _currentRegionItem = value;
                RaisePropertyChanged("CurrentRegionItem");
            }
        }

        private RegionWaterItem _geoCurrentRegionItem;
        /// <summary>
        /// Текущий элемент (по координатам)
        /// </summary>
        public RegionWaterItem GeoCurrentRegionItem
        {
            get { return _geoCurrentRegionItem; }
            set
            {
                _geoCurrentRegionItem = value;
                RaisePropertyChanged("GeoCurrentRegionItem");
            }
        }

        private ObservableCollection<RegionWaterItem> _regionItems;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<RegionWaterItem> RegionItems
        {
            get { return _regionItems; }
            set { 
                _regionItems = value;
                RaisePropertyChanged("RegionItems");
            }
        }
                

        private MobileServiceCollection<WaterItem, WaterItem> parkItems;
        private IMobileServiceTable<WaterItem> WaterTable = App.MobileService.GetTable<WaterItem>();

        public async Task<string> MakeWebRequest(string url = "")
        {
            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async void GetPlaceInfo(double lat, double lon)
        {
            var responseText = await MakeWebRequest("http://geocode-maps.yandex.ru/1.x/?geocode=" + lon.ToString().Replace(",", ".") + "," + lat.ToString().Replace(",", ".") + "&kind=district&format=json");
            try
            {
                JObject o = JObject.Parse(responseText.ToString());
                string district_name = o["response"]["GeoObjectCollection"]["featureMember"][0]["GeoObject"]["metaDataProperty"]["GeocoderMetaData"]["AddressDetails"]["Country"]["Locality"]["DependentLocality"]["DependentLocalityName"].ToString();
                CurrentDistrict = district_name;

                foreach (var item in this.RegionItems)
                {
                    if (item.Title.Trim() == CurrentDistrict.Trim())
                    {
                        this.GeoCurrentRegionItem = item;
                    };
                };
            }
            catch
            {
            };
        }
        public double Latitued = 55.758;
        public double Longitude = 37.611;

        private string _currentDistrict;
        /// <summary>
        /// 
        /// </summary>
        public string CurrentDistrict
        {
            get { return _currentDistrict; }
            set { 
                _currentDistrict = value;
                RaisePropertyChanged("CurrentDistrict");
            }
        }
        

        public async Task<bool> LoadData()
        {
            this.Loading = true;

            ObservableCollection<WaterItem> initItems = new ObservableCollection<WaterItem>(await WaterTable.GetAllAsync());
            //Items = await WaterTable.IncludeTotalCount().ToCollectionAsync(1300); //.ToCollectionAsync(900);
            
            this.Items = new ObservableCollection<WaterItem>();
            this.CurrentRegionItem = null;
            foreach (var item in initItems)
            {
                if (item.Ind_name.Split(' ')[0].ToString()!="Неизвестно")
                {
                    this.Items.Add(item);
                };
            }; 
            RaisePropertyChanged("Items");

            /*try
            {
                var geolocator = new Geolocator();
                Geoposition position = await geolocator.GetGeopositionAsync();
                var str = position.ToString();
                Latitued = position.Coordinate.Latitude;
                Longitude = position.Coordinate.Longitude;
                GetPlaceInfo(Latitued, Longitude);
            }
            catch {
                GetPlaceInfo(Latitued, Longitude);
            };*/

            this.RegionItems = new ObservableCollection<RegionWaterItem>();
            var ItemsInGroup = from b in this.Items
                                    group b by b.Regname into g
                                    select g;
            foreach (var reg in ItemsInGroup)
            {
                try {
                var i = reg;
                var regItem = new RegionWaterItem();
                regItem.Title = reg.Key.ToString();
                try
                {
                    regItem.Code = reg.ToList<WaterItem>().FirstOrDefault().Code.ToString();
                }
                catch { };
                regItem.Items = new ObservableCollection<WaterItem>(reg.ToList<WaterItem>());
                this.RegionItems.Add(regItem);

                    /*if (regItem.Title.Trim()==this.CurrentDistrict.Trim()) {
                        this.CurrentRegionItem = regItem;
                    };*/

                } catch {};
            };

            UpdateCoordinatesWatcher();

            if (this.GeoCurrentRegionItem==null) {
                this.GeoCurrentRegionItem = this.RegionItems.FirstOrDefault();
            };

            this.Loading = false;

            return true;
        }

        private ObservableCollection<RegionWaterItem> _resultItems = new ObservableCollection<RegionWaterItem>();
        /// <summary>
        /// Результаты поиска
        /// </summary>
        public ObservableCollection<RegionWaterItem> ResultItems
        {
            get { return _resultItems; }
            set
            {
                _resultItems = value;
                RaisePropertyChanged("ResultItems");
            }
        }

        private string _searchQuery;

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;

                var items = from item in RegionItems
                            where item.Title.ToLower().Contains(_searchQuery.ToLower())
                            select item;
                ResultItems = new ObservableCollection<RegionWaterItem>();
                foreach (var item in items)
                {
                    ResultItems.Add(item);
                };
                RaisePropertyChanged("ResultItems");
                RaisePropertyChanged("SearchQuery");
            }
        }

    }
}