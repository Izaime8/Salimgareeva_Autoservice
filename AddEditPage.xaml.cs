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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentService = new Service();

        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
                _currentService = SelectedService;

            DataContext = _currentService;
            //_currentService.DiscountInput = Convert.ToInt32(_currentService.Discount * 100);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentService.Title))
                errors.AppendLine("Укажите название услуги");
            if (_currentService.Cost <= 0) //стоимость меньше нуля?
                errors.AppendLine("Укажите стоимость услуги");

            //////////////////////////
            if (_currentService.DiscountInt < 0 || _currentService.DiscountInt > 100)
                errors.AppendLine("Скидка введена некорректно");


            if (_currentService.Duration == 0) // || _currentService.Duration > ???)
                errors.AppendLine("Укажите длительность усуги");

            if (_currentService.Duration > 240)
                errors.AppendLine("Длительность услуги не может быть больше 240 минут");
            if (_currentService.Duration < 0)
                errors.AppendLine("Длительность услуги не может быть меньше 0 минут");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var allServises = Salimgareeva_AutoserviceEntities.GetContext().Service.ToList();
            allServises = allServises.Where(p => p.Title == _currentService.Title).ToList();
            if (allServises.Count == 0)
            {
                if (_currentService.ID == 0)
                    Salimgareeva_AutoserviceEntities.GetContext().Service.Add(_currentService);
                try
                {
                    Salimgareeva_AutoserviceEntities.GetContext().SaveChanges();
                    MessageBox.Show("информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("уже существует такая услуга"); 
            }
            ////////////////////////////////
            //_currentService.Discount = _currentService.DiscountInput / 100.0;
            if (_currentService.ID == 0)
                Salimgareeva_AutoserviceEntities.GetContext().Service.Add(_currentService);
            try
            {
                Salimgareeva_AutoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
