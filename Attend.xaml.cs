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
    /// Interaction logic for Attend.xaml
    /// </summary>
    public partial class Attend : Window
    {
        List<Schedule> scL;
        Customer cust;
        Gym gymi;
        public Attend()
        {
            InitializeComponent();
        }

        public Attend(Customer c)
        {
            InitializeComponent ();
            gymi = new Gym();
            scL = gymi.GetScheduleList();
            cust = c;           
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if(SchList.SelectedItem != null)
            {
                Schedule s = SchList.SelectedItem as Schedule;

                Membership m = gymi.GetMembShip("MembershipID", cust.Membership_ref.ToString());
                if (s.Time.Ticks > m.Start.Ticks && s.Time.Ticks < m.End.Ticks)
                {
                    Attendance a = new Attendance(cust.Membership_ref, s.ScheduleID);
                    if (gymi.AttendClass(a))
                    {
                        MessageBox.Show("Attendance recorded");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Can't record attendance");
                        Close();
                    }
                }
                else { MessageBox.Show("Class out of membership"); }
            }
            else
            {
                MessageBox.Show("Select a class to attend");
            }
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void st_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SchList.Items.Clear();
            if(en.SelectedDate !=  null)
            {
                if(st.SelectedDate < en.SelectedDate)
                {
                    foreach(var item in scL)
                    {
                        if (item.Time > st.SelectedDate && item.Time < en.SelectedDate )
                        {
                            SchList.Items.Add(item);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Start date has to be before end date");
                }
            }
            else
            {
                foreach (var item in scL)
                {
                    if (item.Time > st.SelectedDate)
                    {
                        SchList.Items.Add(item);
                    }
                }
            }
        }

        private void en_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SchList.Items.Clear();
            if (st.SelectedDate != null)
            {
                if (st.SelectedDate < en.SelectedDate)
                {
                    foreach (var item in scL)
                    {
                        if (item.Time > st.SelectedDate && item.Time < en.SelectedDate)
                        {
                            SchList.Items.Add(item);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Start date has to be before end date");
                }
            }
            else
            {
                foreach (var item in scL)
                {
                    if (item.Time < en.SelectedDate)
                    {
                        SchList.Items.Add(item);
                    }
                }
            }
        }
    }
}
