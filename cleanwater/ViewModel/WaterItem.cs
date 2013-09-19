using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cleanwater.ViewModel
{
    public class WaterItem: ViewModelBase
    {
        public WaterItem()
        {
        }

        private int _id;
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        

        /*
        dtfrom - дата начала сбора данных
        dtto - дата окончания
        */

        private string _zones;
        /// <summary>
        /// zones - зоны водоснабжения к которым относится данный район
        /// </summary>
        public string Zones
        {
            get { return _zones; }
            set { _zones = value; }
        }       

        private string _code;
        /// <summary>
        /// code - код района
        /// </summary>
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }        

        private string _value;
        /// <summary>
        /// value - значение индикатора
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        

        private string _ind_id;
        /// <summary>
        /// ind_id - уникальный код идентификатора
        /// </summary>
        public string Ind_id
        {
            get { return _ind_id; }
            set { 
                _ind_id = value;
                RaisePropertyChanged("Ind_id");
            }
        }

        private string _ind_name;
        /// <summary>
        /// ind_name - название идентификатора
        /// </summary>
        public string Ind_name
        {
            get { return _ind_name; }
            set { 
                _ind_name = value;
                RaisePropertyChanged("Ind_name");
            }
        }

        private string _regname;
        /// <summary>
        /// regname - название района
        /// </summary>
        public string Regname
        {
            get { return _regname; }
            set { _regname = value; }
        }
        
        
        
    }
}
