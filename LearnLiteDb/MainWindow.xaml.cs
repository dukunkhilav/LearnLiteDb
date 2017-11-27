using LiteDB;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace LearnLiteDb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {
        int _id;
        string _name = "";
        string _phone = "";

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                var col = db.GetCollection<Model.Customer>("customer");
                var customer = new Model.Customer
                {
                    Name = txtName.Text,
                    Phones = txtPhone.Text,
                    IsActive = cbActive.IsChecked.Value
                };

                col.Insert(customer);
                MessageBox.Show("Success");
                Window_Loaded(sender, e);
            }    
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                var col = db.GetCollection<Model.Customer>("customer");
                dg.DataContext = col.FindAll();
                cbId.DataContext = col.Find(Query.All("Id", Query.Descending)).Select(x => x.Id);
            }
           
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                var col = db.GetCollection<Model.Customer>("customer");
                var customer = new Model.Customer
                {
                    
                    Name = txtName.Text,
                    Phones = txtPhone.Text,
                    IsActive = cbActive.IsChecked.Value
                };
                _id = Int32.Parse(cbId.SelectedItem.ToString());
                col.Update(_id,customer);
                MessageBox.Show("Update Success");
                Window_Loaded(sender, e);
            }
        }

        private void DataGridCell_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dGrid = (DataGrid)sender;
            DataRowView selectedRow = dGrid.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                _id = Int32.Parse(selectedRow[0].ToString());
                _name = selectedRow[1].ToString();
                _phone = selectedRow[2].ToString();
                MessageBox.Show(selectedRow[1].ToString());
            }
            
            txtName.Text = _name;
            txtPhone.Text = _phone;

            //gagalla
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                var col = db.GetCollection<Model.Customer>("customer");
                _id = Int32.Parse(cbId.SelectedItem.ToString());
                col.Delete(_id);
                MessageBox.Show("Delete Success");
                Window_Loaded(sender, e);
            }
        }
    }
}
