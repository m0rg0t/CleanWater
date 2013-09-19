using GalaSoft.MvvmLight;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
            set { _currentItem = value; }
        }
        

        private MobileServiceCollection<WaterItem, WaterItem> parkItems;
        private IMobileServiceTable<WaterItem> WaterTable = App.MobileService.GetTable<WaterItem>();

        public async Task<bool> LoadData()
        {
            this.Loading = true;
            Items = await WaterTable.ToCollectionAsync();
            RaisePropertyChanged("Items");
            this.Loading = false;

            return true;
        }        
        
    }
}