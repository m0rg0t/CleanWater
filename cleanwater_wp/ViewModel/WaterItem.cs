using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cleanwater_wp.ViewModel
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
            set { 
                _code = value;
                RaisePropertyChanged("Code");
            }
        }        

        private string _value;
        /// <summary>
        /// value - значение индикатора
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { 
                _value = value;
                RaisePropertyChanged("Value");
                RaisePropertyChanged("MesureUnit");
                RaisePropertyChanged("ValueMesureUnit");
            }
        }

        public string ValueMesureUnit
        {
            private set
            {
            }
            get
            {
                return Value + " (" + MesureUnit + ")";
            }
        }

        /// <summary>
        /// Единицы измерения
        /// </summary>
        public string MesureUnit
        {
            private set
            {
            }
            get
            {
                string outValue = "";
                switch (this.Ind_name.Trim())
                {
                    case "Общие колиформные бактерии (ОКБ)":
                        outValue = "КОЕ/100мл";
                        break;
                    case "Запах при 60C":
                        outValue = "баллы";
                        break;
                    case "Остаточный хлор":
                        outValue = "мг/дм³";
                        break;
                    case "Общее микробное число (ОМЧ)":
                        outValue = "кол. в 1 мл";
                        break;
                    case "Термотолерантные колиформные бактерии (ТКБ)":
                        outValue = "КОЕ/100мл";
                        break;
                    case "Железо общее":
                        outValue = "мг/дм³";
                        break;                        
                    case "Водородный показатель (pH)":
                        outValue = "ед. pH";
                        break;
                    case "Мутность":
                        outValue = "мг/дм³";
                        break;
                    case "Цветность":
                        outValue = "градус";
                        break;
                    case "Запах при 20C":
                        outValue = "баллы";
                        break;
                    default:
                        outValue = "";
                        break;
                };
                return outValue;
            }
        }

        public string NormalValue
        {
            private set
            {
            }
            get
            {
                string outValue = "";
                switch (this.Ind_name.Trim())
                {
                    case "Общие колиформные бактерии (ОКБ)":
                        outValue = "отсутствие";
                        break;
                    case "Запах при 60C":
                        outValue = "не более 2";
                        break;
                    case "Остаточный хлор":
                        outValue = "не нормируется";
                        break;
                    case "Общее микробное число (ОМЧ)":
                        outValue = "не более 50";
                        break;
                    case "Термотолерантные колиформные бактерии (ТКБ)":
                        outValue = "отсутствие";
                        break;
                    case "Железо общее":
                        outValue = "не более 0,3";
                        break;
                    case "Водородный показатель (pH)":
                        outValue = "в пределах 6,0-9,0";
                        break;
                    case "Мутность":
                        outValue = "не более 1,5";
                        break;
                    case "Цветность":
                        outValue = "не более 20";
                        break;
                    case "Запах при 20C":
                        outValue = "не более 2";
                        break;
                    default:
                        outValue = "";
                        break;
                };
                return outValue;
            }
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
            set { 
                _regname = value;
                RaisePropertyChanged("Regname");
            }
        }
        
        
        
    }
}
