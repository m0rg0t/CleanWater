using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Coding4Fun.Toolkit.Controls;

namespace cleanwater_wp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void RateAppMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var marketplaceReviewTask = new MarketplaceReviewTask();
                marketplaceReviewTask.Show();
            }
            catch
            {
            };
        }

        /// <summary>
        /// Политика конфиденциальности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrivacyMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var messagePrompt = new MessagePrompt
                {
                    Title = "Политика конфиденциальности",
                    Body = new TextBlock
                    {
                        Text = "Приложение не собирает никаких данных без вашего ведома.\nПриложение не собирает и не хранит информацию, которая связана с определенным именем. Мы также делаем все возможное, чтобы обезопасить хранимые данные.\nПринимая условия, которые включают эту политику вы соглашаетесь с данной политикой конфиденциальности.\nКонтакты m0rg0t.Anton@gmail.com",
                        MaxHeight = 500,
                        TextWrapping = TextWrapping.Wrap
                    },
                    IsAppBarVisible = false,
                    IsCancelVisible = false
                };
                messagePrompt.Show();
            }
            catch { };
        }

    }
}