using ChatApplication.DB;
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
using System.Windows.Shapes;

namespace ChatApplication.View
{
    /// <summary>
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        List<Employee> existEmployees;
        int idChatRoom = 0;
        public AddUserWindow(List<Employee> lst, int idChatRoom)
        {
            InitializeComponent();
            this.idChatRoom = idChatRoom;
            existEmployees = lst;

            List<Employee> employees = App.db.Employee.ToList();
            List<Employee> resEmployees = new List<Employee>();
            if (existEmployees != null)
            {
                foreach (Employee employee in employees)
                {
                    bool state = true;
                    foreach (var item in existEmployees)
                    {
                        if (employee.Id == item.Id)
                            state = false;
                    }
                    if (state)
                        resEmployees.Add(employee);
                }
            }
            else
            {
                resEmployees = employees;
            }

            UserCmbBox.ItemsSource = resEmployees;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if(UserCmbBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите юзера!");
                return;
            }
            try
            {
                var selectEmployee = (Employee)UserCmbBox.SelectedItem;
                Employee_ChatRoom addItem = new Employee_ChatRoom();
                addItem.Id_ChatRoom = idChatRoom;
                addItem.Employee = selectEmployee;
                App.db.Employee_ChatRoom.Add(addItem);
                App.db.SaveChanges();
                MessageBox.Show("Юзер успешно добавлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Close();
        }
    }
}
