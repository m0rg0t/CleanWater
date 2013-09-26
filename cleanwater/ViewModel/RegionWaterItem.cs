using GalaSoft.MvvmLight;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cleanwater.ViewModel
{
    public class RegionWaterItem : ViewModelBase
    {
        public RegionWaterItem()
        {
        }

        private string _title;
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { 
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _code;
        /// <summary>
        /// 
        /// </summary>
        public string Code
        {
            get { return _code; }
            set { 
                _code = value;

                this.Image = "/Assets/regions/"+_code +".jpg";

                RaisePropertyChanged("Code");
            }
        }

        private string _image;
        /// <summary>
        /// image for region
        /// </summary>
        public string Image
        {
            get { return _image; }
            set { _image = value; }
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
                RaisePropertyChanged("Items");
            }
        }

        private ObservableCollection<CommentItem> _commentItems = new ObservableCollection<CommentItem>();
        /// <summary>
        /// Список комментариев
        /// </summary>
        public ObservableCollection<CommentItem> CommentItems
        {
            get { return _commentItems; }
            set { 
                _commentItems = value;
                RaisePropertyChanged("CommentItems");
            }
        }

        private MobileServiceCollection<CommentItem, CommentItem> commentItems;
        private IMobileServiceTable<CommentItem> CommentTable = App.MobileService.GetTable<CommentItem>();

        public async Task<bool> LoadComments()
        {
            this.CommentItems = await CommentTable.Where(c => c.RegionCode == this.Code).ToCollectionAsync();
            return true;
        }
        
        
    }
}
