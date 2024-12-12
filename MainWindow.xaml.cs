using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using GymSovellus;

namespace GymGraafinen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Gym gymi = new Gym();
        Customer? selected;
        Trainer? selectedtrnr;
        Class? selectedclas;
        Schedule? selectedsch;
        public MainWindow()
        {
            InitializeComponent();            
        }

        private void CustLBtn_Click(object sender, RoutedEventArgs e)
        {
            TabContr.SelectedIndex = 0;
            ListList.ItemsSource = null;
            ListList.ItemsSource = gymi.GetCustList();
        }

        private void TrnrLBtn_Click(object sender, RoutedEventArgs e)
        {
            TabContr.SelectedIndex = 1;
            TrnrList.ItemsSource = null;
            TrnrList.ItemsSource = gymi.GetTrnrList();
        }

        private void ClasLBtn_Click(object sender, RoutedEventArgs e)
        {
            TabContr.SelectedIndex = 2;
            ClasList.ItemsSource = null;
            ClasList.ItemsSource = gymi.GetClassList();
        }

        private void SchLBtn_Click(object sender, RoutedEventArgs e)
        {
            TabContr.SelectedIndex = 3;
        }

        private void ListList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected = ListList.SelectedItem as Customer;
            Membership m;
            Trainer t;

            

            if (selected != null)
            {
                if (selected.Membership_ref > 0)
                {
                    RecBtn.IsEnabled = true;
                }
                else { RecBtn.IsEnabled = false; }
                SelCustEdit.IsEnabled = true;
                SelCustRmv.IsEnabled = true;
                Membships.Items.Clear();
                Trainers.ItemsSource = gymi.GetTrnrList();
                if (selected.Membership_ref > 0 && selected.PersonalTrainer == 0)
                {
                    m = gymi.GetMembShip("MembershipID", selected.Membership_ref.ToString());
                    SelCustID.Text = selected.ID.ToString();
                    SelCustFN.Text = selected.FirstName;
                    SelCustLN.Text = selected.LastName;
                    SelCustEM.Text = selected.Email;
                    Membships.Items.Add(m);
                    Membships.SelectedIndex = 0;
                }
                else if (selected.Membership_ref > 0 && selected.PersonalTrainer > 0)
                {
                    m = gymi.GetMembShip("MembershipID", selected.Membership_ref.ToString());
                    t = gymi.GetTrnr("TrainerID", selected.PersonalTrainer.ToString());
                    SelCustID.Text = selected.ID.ToString();
                    SelCustFN.Text = selected.FirstName;
                    SelCustLN.Text = selected.LastName;
                    SelCustEM.Text = selected.Email;
                    Membships.Items.Add(m);
                    Membships.SelectedIndex = 0;
                    Trainers.Text = t.ToString();
                }
                else
                {
                    Membships.Items.Add("Select");
                    SelCustID.Text = selected.ID.ToString();
                    SelCustFN.Text = selected.FirstName;
                    SelCustLN.Text = selected.LastName;
                    SelCustEM.Text = selected.Email;
                    SelSchRmv.IsEnabled = false;
                    SelSchEdit.IsEnabled = false;
                    
                }

                Membships.Items.Add("Monthly");
                Membships.Items.Add("Half a year");
                Membships.Items.Add("Yearly");
            }



        }

        private void TrnrList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedtrnr = TrnrList.SelectedItem as Trainer;

            if (selectedtrnr != null)
            {
                SelTrnrEdit.IsEnabled = true;
                SelTrnrRmv.IsEnabled = true;
                SelTrnrID.Text = selectedtrnr.ID.ToString();
                SelTrnrFN.Text = selectedtrnr.FirstName;
                SelTrnrLN.Text = selectedtrnr.LastName;
                SelTrnrEM.Text = selectedtrnr.Email;
            }
            else
            {
                SelSchRmv.IsEnabled = false;
                SelSchEdit.IsEnabled = false;
            }
        }

        private void SelCustEdit_Click(object sender, RoutedEventArgs e)
        {
            ListList.IsEnabled = false;
            AddCustBtn.IsEnabled = false;
            SelCustCancel.IsEnabled = true;
            SelCustEdit.IsEnabled = false;
            SelCustSave.IsEnabled = true;
            SelCustFN.IsEnabled = true;
            SelCustLN.IsEnabled = true;
            SelCustEM.IsEnabled = true;
            Membships.IsEnabled = true;
            Trainers.IsEnabled = true;

        }

        private void SelCustCancel_Click(object sender, RoutedEventArgs e)
        {
            ListList.IsEnabled = true;
            AddCustBtn.IsEnabled = true;
            SelCustCancel.IsEnabled = false;
            SelCustEdit.IsEnabled = true;
            SelCustSave.IsEnabled = false;
            SelCustFN.IsEnabled = false;
            SelCustLN.IsEnabled = false;
            SelCustEM.IsEnabled = false;
            Membships.IsEnabled = false;
            Trainers.IsEnabled = false;
        }

        private void SelCustSave_Click(object sender, RoutedEventArgs e)
        {
            selected = ListList.SelectedItem as Customer;
            if (Membships.SelectedValue != null && Membships.SelectedIndex > 0)
            {
                Membership uusm = new Membership(selected.ID, Membships.SelectedIndex);
                gymi.AddMemb(uusm);
                selected = gymi.GetCust("CustomerID", selected.ID.ToString());
            }

            if (Trainers.SelectedValue != null)
            {
                Trainer t = Trainers.SelectedItem as Trainer;
                selected.PersonalTrainer = t.ID;
            }

            selected.FirstName = SelCustFN.Text;
            selected.LastName = SelCustLN.Text;
            selected.Email = SelCustEM.Text;

            gymi.UpdateCust(selected);
            ListList.ItemsSource = null;
            ListList.ItemsSource = gymi.GetCustList();

            ListList.IsEnabled = true;
            SelCustCancel.IsEnabled = false;
            SelCustEdit.IsEnabled = true;
            SelCustSave.IsEnabled = false;
            SelCustFN.IsEnabled = false;
            SelCustLN.IsEnabled = false;
            SelCustEM.IsEnabled = false;
            AddCustBtn.IsEnabled = true;
            Membships.IsEnabled = false;
            Trainers.IsEnabled = false;
        }

        private void AddCustBtn_Click(object sender, RoutedEventArgs e)
        {
            AddsWindow AddCust = new AddsWindow(0);
            AddCust.ShowDialog();
            ListList.ItemsSource = null;
            ListList.ItemsSource = gymi.GetCustList();
        }

        private void AddTrnrBtn_Click(object sender, RoutedEventArgs e)
        {
            AddsWindow AddTrnr = new AddsWindow(1);
            AddTrnr.ShowDialog();
            TrnrList.ItemsSource = null;
            TrnrList.ItemsSource = gymi.GetTrnrList();
        }

        private void SelTrnrEdit_Click(object sender, RoutedEventArgs e)
        {
            TrnrList.IsEnabled = false;
            AddTrnrBtn.IsEnabled = false;
            SelTrnrCancel.IsEnabled = true;
            SelTrnrEdit.IsEnabled = false;
            SelTrnrSave.IsEnabled = true;
            SelTrnrFN.IsEnabled = true;
            SelTrnrLN.IsEnabled = true;
            SelTrnrEM.IsEnabled = true;
        }

        private void SelTrnrCancel_Click(object sender, RoutedEventArgs e)
        {
            TrnrList.IsEnabled = true;
            SelTrnrCancel.IsEnabled = false;
            SelTrnrEdit.IsEnabled = true;
            SelTrnrSave.IsEnabled = false;
            SelTrnrFN.IsEnabled = false;
            SelTrnrLN.IsEnabled = false;
            SelTrnrEM.IsEnabled = false;
        }

        private void SelTrnrSave_Click(object sender, RoutedEventArgs e)
        {
            selectedtrnr = TrnrList.SelectedItem as Trainer;

            selectedtrnr.FirstName = SelCustFN.Text;
            selectedtrnr.LastName = SelCustLN.Text;
            selectedtrnr.Email = SelCustEM.Text;

            gymi.UpdateTrnr(selectedtrnr);
            TrnrList.ItemsSource = null;
            TrnrList.ItemsSource = gymi.GetTrnrList();

            TrnrList.IsEnabled = true;
            SelTrnrCancel.IsEnabled = false;
            SelTrnrEdit.IsEnabled = true;
            SelTrnrSave.IsEnabled = false;
            SelTrnrFN.IsEnabled = false;
            SelTrnrLN.IsEnabled = false;
            SelTrnrEM.IsEnabled = false;
        }

        private void SelCustRmv_Click(object sender, RoutedEventArgs e)
        {
            gymi.RemovePerson(selected);
            MessageBox.Show("Customer deleted");
            ListList.ItemsSource = null;
            ListList.ItemsSource = gymi.GetCustList();
        }

        private void SelTrnrRmv_Click(object sender, RoutedEventArgs e)
        {
            gymi.RemovePerson(selectedtrnr);
            MessageBox.Show("Trainer deleted");
            TrnrList.ItemsSource = null;
            TrnrList.ItemsSource = gymi.GetTrnrList();
        }

        private void ClasList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedclas = ClasList.SelectedItem as Class;

            if (selectedclas != null)
            {
                SelClasEdit.IsEnabled = true;
                SelClasRmv.IsEnabled = true;
                SelClasID.Text = selectedclas.ID.ToString();
                SelClasNM.Text = selectedclas.Name;
            }
            else
            {
                SelClasRmv.IsEnabled = false;
                SelClasEdit.IsEnabled = false;
            }
        }

        private void AddClasBtn_Click(object sender, RoutedEventArgs e)
        {
            AddsWindow AddClas = new AddsWindow(2);
            AddClas.ShowDialog();
            ClasList.ItemsSource = null;
            ClasList.ItemsSource = gymi.GetClassList();
        }

        private void SelClasEdit_Click(object sender, RoutedEventArgs e)
        {
            ClasList.IsEnabled = false;
            AddClasBtn.IsEnabled = false;
            SelClasCancel.IsEnabled = true;
            SelClasEdit.IsEnabled = false;
            SelClasSave.IsEnabled = true;
            SelClasNM.IsEnabled = true;
        }

        private void SelClasCancel_Click(object sender, RoutedEventArgs e)
        {
            ClasList.IsEnabled = true;
            AddClasBtn.IsEnabled = true;
            SelClasCancel.IsEnabled = false;
            SelClasEdit.IsEnabled = true;
            SelClasSave.IsEnabled = false;
            SelClasNM.IsEnabled = false;
        }

        private void SelClasSave_Click(object sender, RoutedEventArgs e)
        {
            selectedclas = ClasList.SelectedItem as Class;

            selectedclas.Name = SelClasNM.Text;

            gymi.UpdateClass(selectedclas);
            ClasList.ItemsSource = null;
            ClasList.ItemsSource = gymi.GetClassList();

            ClasList.IsEnabled = true;
            SelClasCancel.IsEnabled = false;
            SelClasEdit.IsEnabled = true;
            SelClasSave.IsEnabled = false;
            SelClasNM.IsEnabled = false;
        }

        private void SelClasRmv_Click(object sender, RoutedEventArgs e)
        {
            gymi.RemoveClass(selectedclas);
            MessageBox.Show("Class deleted");
            ClasList.ItemsSource = null;
            ClasList.ItemsSource = gymi.GetClassList();
        }

        private void SchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedsch = SchList.SelectedItem as Schedule;
            

            SelSchCN.ItemsSource = gymi.GetClassList();


            SelSchTrnr.ItemsSource = gymi.GetTrnrList();

            if (selectedsch != null)
            {
                SelSchEdit.IsEnabled = true;
                SelSchRmv.IsEnabled = true;
                Class c = gymi.GetClass("ClassID", selectedsch.Class_ref.ToString());
                SelSchCN.SelectedItem = c;
                Trainer t = gymi.GetTrnr("TrainerID", selectedsch.Trainer_ref.ToString());
                SelSchTrnr.SelectedItem = t;

                Attcount.Text = $"Attendance:\n{gymi.AttendPerClass(selectedsch)}";
                SelSchID.Text = selectedsch.ScheduleID.ToString();
                SelSchCN.Text = c.Name;
                SelSt.Text = selectedsch.Time.ToString("g");
                SelEn.Text = selectedsch.EndTime.ToString("g");
                SelSchTrnr.Text = t.ToString();
            }
            else
            {
                SelSchRmv.IsEnabled = false;
                SelSchEdit.IsEnabled = false;
            }
        }

        private void AddSchBtn_Click(object sender, RoutedEventArgs e)
        {
            AddsWindow AddSch = new AddsWindow(3);
            AddSch.ShowDialog();
            SchList.ItemsSource = null;
            SchList.ItemsSource = gymi.GetScheduleList();
        }

        private void SelSchEdit_Click(object sender, RoutedEventArgs e)
        {
            SchList.IsEnabled = false;
            AddSchBtn.IsEnabled = false;
            SelSchCancel.IsEnabled = true;
            SelSchEdit.IsEnabled = false;
            SelSchSave.IsEnabled = true;
            SelSchCN.IsEnabled = true;
            SelSt.IsEnabled = true;
            SelSchTrnr.IsEnabled = true;
        }

        private void SelSchCancel_Click(object sender, RoutedEventArgs e)
        {
            SchList.IsEnabled = true;
            AddSchBtn.IsEnabled = true;
            SelSchCancel.IsEnabled = false;
            SelSchEdit.IsEnabled = true;
            SelSchSave.IsEnabled = false;
            SelSchCN.IsEnabled = false;
            SelSt.IsEnabled = false;
            SelSchTrnr.IsEnabled = false;
        }

        private void SelSchSave_Click(object sender, RoutedEventArgs e)
        {
            selectedsch = SchList.SelectedItem as Schedule;

            Class c = SelSchCN.SelectedItem as Class;
            Trainer t = SelSchTrnr.SelectedItem as Trainer;

            selectedsch.Class_ref = c.ID;

            selectedsch.Trainer_ref = t.ID;

            selectedsch.Time = Convert.ToDateTime(SelSt.Text);
            selectedsch.EndTime = selectedsch.Time.AddHours(1);

            gymi.UpdateSchedule(selectedsch);
            SchList.ItemsSource = null;
            SchList.ItemsSource = gymi.GetScheduleList();

            SchList.IsEnabled = true;
            AddSchBtn.IsEnabled = true;
            SelSchCancel.IsEnabled = false;
            SelSchEdit.IsEnabled = true;
            SelSchSave.IsEnabled = false;
            SelSchCN.IsEnabled = false;
            SelSt.IsEnabled = false;
            SelSchTrnr.IsEnabled = false;
        }

        private void SelSchRmv_Click(object sender, RoutedEventArgs e)
        {
            gymi.RemoveSchedule(selectedsch);
            MessageBox.Show("Schedule deleted");
            SchList.ItemsSource = null;
            SchList.ItemsSource = gymi.GetScheduleList();
        }

        private void RecBtn_Click(object sender, RoutedEventArgs e)
        {
            Records rec = new Records(selected);
            rec.ShowDialog();
        }

        private void St_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Schedule> scL = gymi.GetScheduleList();
            SchList.Items.Clear();
            if (En.SelectedDate != null)
            {
                if (St.SelectedDate < En.SelectedDate)
                {
                    foreach (var item in scL)
                    {
                        if (item.Time > St.SelectedDate && item.Time < En.SelectedDate)
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
                    if (item.Time > St.SelectedDate)
                    {
                        SchList.Items.Add(item);
                    }
                }
            }
        }

        private void En_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Schedule> scL = gymi.GetScheduleList();
            SchList.Items.Clear();
            if (St.SelectedDate != null)
            {
                if (St.SelectedDate < En.SelectedDate)
                {
                    foreach (var item in scL)
                    {
                        if (item.Time > St.SelectedDate && item.Time < En.SelectedDate)
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
                    if (item.Time < En.SelectedDate)
                    {
                        SchList.Items.Add(item);
                    }
                }
            }
        }
    }
}