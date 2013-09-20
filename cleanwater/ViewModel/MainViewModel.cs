using GalaSoft.MvvmLight;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using cleanwater.Common;

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
            }
        }

        private ObservableCollection<WaterItem> _items;
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

        private WaterItem _currentRegionItem;
        /// <summary>
        /// Текущий элемент
        /// </summary>
        public WaterItem CurrentRegionItem
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

        public async Task<bool> LoadData()
        {
            this.Loading = true;

            Items = new ObservableCollection<WaterItem>(await WaterTable.GetAllAsync());
            //Items = await WaterTable.IncludeTotalCount().ToCollectionAsync(1300); //.ToCollectionAsync(900);
            RaisePropertyChanged("Items");

            this.RegionItems = new ObservableCollection<RegionWaterItem>();
            var ItemsInGroup = from b in this.Items
                                    group b by b.Regname into g
                                    select g;
            foreach (var reg in ItemsInGroup)
            {
                var i = reg;
                var regItem = new RegionWaterItem();
                regItem.Title = reg.Key.ToString();
                regItem.Items = new ObservableCollection<WaterItem>(reg.ToList<WaterItem>());
                this.RegionItems.Add(regItem);
            };

            this.Loading = false;

            return true;
        }        
        
    }
}