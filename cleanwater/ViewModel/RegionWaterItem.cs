﻿using GalaSoft.MvvmLight;
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
                RaisePropertyChanged("Items");
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
                RaisePropertyChanged("Items");
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
                RaisePropertyChanged("Items");
            }
        }
        
    }
}