using GymSovellus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GymGraafinen
{
    /// <summary>
    /// Interaction logic for AddsWindow.xaml
    /// </summary>
    public partial class AddsWindow : Window
    {
        Gym gymi;


        public AddsWindow()
        {
            InitializeComponent();
        }

        public AddsWindow(int i)
        {
            InitializeComponent();

            gymi = new Gym();

            switch(i)
            {
                case 0:

                    AddsTabs.SelectedItem = Cust;
                    Ots.Text = "Add new customer";
                    break;
                case 1:
                    AddsTabs.SelectedItem = Trnr;
                    Ots.Text = "Add new trainer";
                    break;
                case 2:
                    AddsTabs.SelectedItem = Clas;
                    Ots.Text = "Add new class";
                    break;
                case 3:
                    AddsTabs.SelectedItem = Sch;
                    SelSchClas.ItemsSource = gymi.GetClassList();
                    SelSchTrnr.ItemsSource = gymi.GetTrnrList();
                    Ots.Text = "Add new schedule";
                    break;
            }
        }

        private void SelCustSave_Click(object sender, RoutedEventArgs e)
        {
            gymi.AddCust(SelCustFN.Text, SelCustLN.Text, SelCustEM.Text);
            MessageBox.Show("Customer added!");
            Close();
        }

        private void SelCustCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SelTrnrCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SelTrnrSave_Click(object sender, RoutedEventArgs e)
        {
            gymi.AddTrainer(SelTrnrFN.Text, SelTrnrLN.Text, SelTrnrEM.Text);
            MessageBox.Show("Trainer added!");
            Close();
        }

        private void SelClasCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SelClasSave_Click(object sender, RoutedEventArgs e)
        {
            Class c = new Class(SelClasNM.Text);
            gymi.AddClass(c);
            MessageBox.Show("Class added!");
            Close();
        }

        private void SelSchCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SelSchSave_Click(object sender, RoutedEventArgs e)
        {
            Class c = SelSchClas.SelectedItem as Class;
            Trainer t = SelSchTrnr.SelectedItem as Trainer;
            Schedule sch = new Schedule(c.ID, t.ID, Convert.ToDateTime(SelSt.Text));
            gymi.AddSchedule(sch);
            MessageBox.Show("Schedule added!");
            Close();
        }

        private void SelSt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DateTime st = Convert.ToDateTime(SelSt.Text);
                SelEnd.Text = st.AddHours(1).ToString("g");
            }
            catch { }
            
        }
    }
}
