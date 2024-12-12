using GymSovellus;
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

namespace GymGraafinen
{
    /// <summary>
    /// Interaction logic for Records.xaml
    /// </summary>
    public partial class Records : Window
    {
        Gym gymi;
        Customer? cust;
        public Records()
        {
            InitializeComponent();
        }

        public Records(Customer c)
        {
            InitializeComponent ();
            gymi = new Gym();
            cust = c;
            Custreport.Text = gymi.FullReport(cust);
            CustOts.Text = cust.FirstName + " " + cust.LastName;
        }

        private void AddVis_Click(object sender, RoutedEventArgs e)
        {
            if (gymi.CustVisit(cust, DateTime.Now))
            {
                MessageBox.Show("Visit recorded");
                Custreport.Text = gymi.FullReport(cust);
            }
            else
            {
                MessageBox.Show("Can't record visit");
            }
        }

        private void AddAtt_Click(object sender, RoutedEventArgs e)
        {
            Attend attwind = new Attend(cust);
            attwind.ShowDialog();
            Custreport.Text = gymi.FullReport(cust);
        }

        private void AddTrUse_Click(object sender, RoutedEventArgs e)
        {
            bool ok = gymi.UseTrainer(cust, DateTime.Now);
            if (ok)
            {
                MessageBox.Show("Appointment recorded");
                Custreport.Text = gymi.FullReport(cust);
            }
            else
            {
                MessageBox.Show("Can't record appointment");
            }
        }
    }
}
