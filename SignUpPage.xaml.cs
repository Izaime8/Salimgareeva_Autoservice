using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Salimgareeva_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    /// 


    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if(SelectedService != null)
                this._currentService = SelectedService;

            DataContext = _currentService;

            var _currentClient = Salimgareeva_AutoserviceEntities.GetContext().Client.ToList();

            ComboClient.ItemsSource = _currentClient;
         }

        private ClientService _currentClientService = new ClientService();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");
            if (TBEnd.Text == "")
                errors.AppendLine("Время начала услуги указано неверно");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);
            if (_currentClientService.ID == 0)
                Salimgareeva_AutoserviceEntities.GetContext().ClientService.Add(_currentClientService);
            try
            {
                Salimgareeva_AutoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;

            if (s.Length <= 3 || !s.Contains(":"))
            {
                TBEnd.Text = "";
            }
            else
            {
                string[] start = s.Split(new char[] { ':' });
                int startHour = Convert.ToInt32(start[0].ToString());
                
                int startMin = Convert.ToInt32(start[1].ToString());
                ////////////////
                if (startHour >= 0 && startHour <= 23 && startMin >= 0 && startMin <= 59)
                {
                    startHour *= 60;

                    int sum = startHour + startMin + _currentService.Duration;

                    int EndHour = (sum / 60) % 24;
                    int EndMin = sum % 60;
                    //
                    if (EndMin < 10)
                        s = EndHour.ToString() + ":" + "0" + EndMin.ToString();
                    else
                        s = EndHour.ToString() + ":" + EndMin.ToString();
                    TBEnd.Text = s;
                }
                else
                    TBEnd.Text = "";
            }
        }
    }
}
