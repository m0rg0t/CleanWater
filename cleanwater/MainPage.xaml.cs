using cleanwater.Common;
using cleanwater.ViewModel;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace cleanwater
{
    public class TodoItem
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }
    }

    public sealed partial class MainPage : Page
    {
        private MobileServiceCollection<TodoItem, TodoItem> items;
        private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();

        private MobileServiceCollection<WaterItem, WaterItem> waterItems;
        private IMobileServiceTable<WaterItem> WaterTable = App.MobileService.GetTable<WaterItem>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void InsertTodoItem(TodoItem todoItem)
        {
            // This code inserts a new TodoItem into the database. When the operation completes
            // and Mobile Services has assigned an Id, the item is added to the CollectionView
            await todoTable.InsertAsync(todoItem);
            items.Add(todoItem);                        
        }

        private async void RefreshTodoItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems
                items = await todoTable
                    .Where(todoItem => todoItem.Complete == false)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                ListItems.ItemsSource = items;
            }
        }

        private async void UpdateCheckedTodoItem(TodoItem item)
        {
            // This code takes a freshly completed TodoItem and updates the database. When the MobileService 
            // responds, the item is removed from the list 
            await todoTable.UpdateAsync(item);
            items.Remove(item);
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTodoItems();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var todoItem = new TodoItem { Text = TextInput.Text };
            InsertTodoItem(todoItem);
        }

        private void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            TodoItem item = cb.DataContext as TodoItem;
            UpdateCheckedTodoItem(item);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RefreshTodoItems();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            string earthQuakeData = await client.GetStringAsync("http://m0rg0t.com/all.csv");

            // Parse the file 
            CsvParser csvParser = new CsvParser();
            csvParser.RawText = earthQuakeData;
            csvParser.HasHeaderRow = true;
            csvParser.Delimiter = '\t';

            var results = await csvParser.Parse();

            foreach (var item in results)
            {
                WaterItem water = new WaterItem();
                //park.Id = Int32.Parse(item["1_Номер"]);
                water.Ind_name = item["ind_name"].ToString();
                water.Ind_id = item["ind_id"].ToString();
                water.Code = item["code"].ToString();
                water.Zones = item["zones"].ToString();
                water.Regname = item["regname"].ToString();
                water.Value = item["value"].ToString();

                await WaterTable.InsertAsync(water);
                    //Water.InsertAsync(park);
            };
        }
    }
}
