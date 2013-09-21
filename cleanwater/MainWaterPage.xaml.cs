using Callisto.Controls;
using cleanwater.Controls;
using cleanwater.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента страницы элементов задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234233

namespace cleanwater
{
    /// <summary>
    /// Страница, на которой отображается коллекция эскизов элементов.  В приложении с разделением эта страница
    /// служит для отображения и выбора одной из доступных групп.
    /// </summary>
    public sealed partial class MainWaterPage : cleanwater.Common.LayoutAwarePage
    {
        public MainWaterPage()
        {
            this.InitializeComponent();

            if (ViewModelLocator.MainStatic.Items.Count() < 1)
            {
                ViewModelLocator.MainStatic.LoadData();
            };
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации. Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="navigationParameter">Значение параметра, передаваемое
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы.
        /// </param>
        /// <param name="pageState">Словарь состояния, сохраненного данной страницей в ходе предыдущего
        /// сеанса. Это значение будет равно NULL при первом посещении страницы.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Назначение привязываемой коллекции элементов объекту this.DefaultViewModel["Items"]
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested -= Settings_CommandsRequested;
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += Settings_CommandsRequested;
            base.OnNavigatedTo(e);
        }

        void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            try
            {
                var viewAboutPage = new SettingsCommand("", "Об авторе", cmd =>
                {
                    //(Window.Current.Content as Frame).Navigate(typeof(AboutPage));
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.Content = new About();
                    settingsFlyout.HeaderText = "Об авторе";

                    settingsFlyout.IsOpen = true;
                });
                args.Request.ApplicationCommands.Add(viewAboutPage);

                var viewAboutMalukahPage = new SettingsCommand("", "Политика конфиденциальности", cmd =>
                {
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.Content = new Privacy();
                    settingsFlyout.HeaderText = "Политика конфиденциальности";

                    settingsFlyout.IsOpen = true;
                });
                args.Request.ApplicationCommands.Add(viewAboutMalukahPage);
            }
            catch { };
        }

        private void itemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var item = ((RegionWaterItem)e.ClickedItem);
                this.Frame.Navigate(typeof(WaterDetailPage), item.Code.ToString());
            }
            catch { };
        }

        private void CurrentParamsDataGrid_DataBindingComplete(object sender, Telerik.UI.Xaml.Controls.Grid.DataBindingCompleteEventArgs e)
        {
            //this.CurrentParamsDataGrid.Columns.Remove(this.CurrentParamsDataGrid.Columns.FirstOrDefault(c => c.Header == "Id"));
            //this.CurrentParamsDataGrid.Columns.Remove(this.CurrentParamsDataGrid.Columns.FirstOrDefault(c => c.Header == "Code"));
        }
    }
}
