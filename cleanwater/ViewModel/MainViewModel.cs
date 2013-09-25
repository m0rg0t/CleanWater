using GalaSoft.MvvmLight;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using cleanwater.Common;
using System.Net;
using Bing;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Callisto.Controls;

namespace cleanwater.ViewModel
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

        public async Task<bool> GetPlaceInfo(double lat, double lon)
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            //if (roamingSettings.Values["street"].ToString() == "")
            //{
            var responseText = await MakeWebRequest("http://geocode-maps.yandex.ru/1.x/?geocode=" + lon.ToString().Replace(",", ".") + "," + lat.ToString().Replace(",", ".") + "&kind=district&format=json");
            try
            {
                JObject o = JObject.Parse(responseText.ToString());
                string district_name = o["response"]["GeoObjectCollection"]["featureMember"][0]["GeoObject"]["metaDataProperty"]["GeocoderMetaData"]["AddressDetails"]["Country"]["Locality"]["DependentLocality"]["DependentLocalityName"].ToString();
                CurrentDistrict = district_name;
                //ViewModelLocator.MainStatic.Street = road;
                //ViewModelLocator.MainStatic.Town = town;
            }
            catch
            {
            };
            return true;
            //};
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

        /// <summary>
        /// 
        /// </summary>
        public Flyout AddBox { get; set; }

        public async Task<bool> GetCurrentPosition()
        {
            try
            {
                var geolocator = new Geolocator();
                Geoposition position = await geolocator.GetGeopositionAsync();
                var str = position.ToString();
                Latitued = position.Coordinate.Latitude;
                Longitude = position.Coordinate.Longitude;
                await GetPlaceInfo(Latitued, Longitude);
            }
            catch
            {
                GetPlaceInfo(Latitued, Longitude);
            };
            return true;
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

            //await GetCurrentPosition();

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

            await GetCurrentPosition();

            if (this.CurrentRegionItem==null) {
                this.CurrentRegionItem = this.RegionItems.FirstOrDefault();
            };

            this.Loading = false;

            return true;
        }        
        
    }
}