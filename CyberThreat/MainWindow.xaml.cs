using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
using ExcelDataReader;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

namespace CyberThreat
{
    public partial class MainWindow : Window
    {
        public Dictionary<double, Threat> newThreatsBase = new Dictionary<double, Threat>();
        public Dictionary<double, Threat> oldThreatsBase = new Dictionary<double, Threat>();
        public ObservableCollection<Change> allChanges = new ObservableCollection<Change>();

        const char EnterSymbol = '鵝';
        const char newPropertyThread = '豬';



        public int Page { get; set; } = 1;
        public int MaxPage { get; set; } = 1;
        public ObservableCollection<Threat> pageThreats = new ObservableCollection<Threat>();
        public void TurnPage(int page)
        {
            if (page > 0 && page <= MaxPage)
            {
                Page = page;
            }
            else if (page > MaxPage)
            {
                Page = MaxPage;
            }
            else
            {
                Page = 1;
            }
            int itemStartCount = (Page - 1) * 15;
            int itemEndCount = itemStartCount + 15;
            int i = 1;
            pageThreats.Clear();
            foreach (var item in newThreatsBase)
            {
                if (i > itemStartCount && i <= itemEndCount)
                {
                    pageThreats.Add(item.Value);
                }
                i++;
            }
            Page_Text.Text = Page.ToString();
            Main_List.ItemsSource = pageThreats;
        }

        public void ReadFromXlsx()
        {
            using (var stream = File.Open("thrlist.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    var result = reader.AsDataSet();
                    var dataTable = result.Tables[0];

                    for (var i = 2; i < dataTable.Rows.Count; i++)
                    {
                        bool con;
                        bool intg;
                        bool avl;
                        if ((double)dataTable.Rows[i][5] == 1) con = true;
                        else con = false;

                        if ((double)dataTable.Rows[i][6] == 1) intg = true;
                        else intg = false;

                        if ((double)dataTable.Rows[i][7] == 1) avl = true;
                        else avl = false;
                        newThreatsBase.Add((double)dataTable.Rows[i][0], new Threat()
                        {
                            Id = (double)dataTable.Rows[i][0],
                            Name = (string)dataTable.Rows[i][1],
                            Discription = (string)dataTable.Rows[i][2],
                            Source = (string)dataTable.Rows[i][3],
                            Subject = (string)dataTable.Rows[i][4],
                            Confidential = con,
                            Integrity = intg,
                            Availability = avl,
                            DateIn = ((DateTime)dataTable.Rows[i][8]).ToString("dd.MM.yyyy"),
                            LastDate = ((DateTime)dataTable.Rows[i][9]).ToString("dd.MM.yyyy"),
                        });
                        newThreatsBase[(double)dataTable.Rows[i][0]].BoolToString();
                    }
                }
            }
        }
        public async void ReadFromLocalBase()
        {
            string[] stringThread;
            string threadLine;
            try
            {
                using (StreamReader stream = new StreamReader("local_thrlist.txt"))
                {
                    while ((threadLine = stream.ReadLine()) != null)
                    {
                        stringThread = threadLine.Split(newPropertyThread);
                        bool con;
                        bool intg;
                        bool avl;
                        if (stringThread[5] == "True") con = true;
                        else con = false;
                        if (stringThread[6] == "True") intg = true;
                        else intg = false;
                        if (stringThread[7] == "True") avl = true;
                        else avl = false;
                        newThreatsBase.Add(double.Parse(stringThread[0]), new Threat()
                        {
                            Id = double.Parse(stringThread[0]),
                            Name = stringThread[1].Replace(EnterSymbol.ToString(), "\r\n"),
                            Discription = stringThread[2].Replace(EnterSymbol.ToString(), "\r\n"),
                            Source = stringThread[3].Replace(EnterSymbol.ToString(), "\r\n"),
                            Subject = stringThread[4].Replace(EnterSymbol.ToString(), "\r\n"),
                            Confidential = con,
                            Integrity = intg,
                            Availability = avl,
                            DateIn = stringThread[8].Replace(EnterSymbol.ToString(), "\r\n"),
                            LastDate = stringThread[9].Replace(EnterSymbol.ToString(), "\r\n"),
                        });
                        newThreatsBase[double.Parse(stringThread[0])].BoolToString();
                        stringThread = default;
                    }
                }
            }
            catch (Exception)
            {
                Load_Text.Text = "База данных повреждена, сейчас загрузим новую...";
                Loading_Layer.Tag = "Downloading";
                Warning.Visibility = Visibility.Visible;
                Done.Visibility = Visibility.Collapsed;
                Clock.Visibility = Visibility.Collapsed;
                Loading_Layer.Visibility = Visibility.Visible;
                try
                {
                    await new WebClient().DownloadFileTaskAsync("https://bdu.fstec.ru/files/documents/thrlist.xlsx", "thrlist.xlsx");
                }
                catch (Exception e)
                {
                    Warning.Visibility = Visibility.Visible;
                    Load_Text.Text = e.Message;
                }
                ReadFromXlsx();
                
                Loading_Layer.Tag = "Done";
                Done.Visibility = Visibility.Visible;
                Warning.Visibility = Visibility.Collapsed;
                Clock.Visibility = Visibility.Collapsed;
                Load_Text.Text = "Загрузка успешно завершена!";

                Page = 1;
                MaxPage = newThreatsBase.Count / 15 + 1;
                Page_Text.Text = Page.ToString();
                Max_Page_Text.Text = $"/{MaxPage}";
                TurnPage(1);
            }

        }

        public async Task<bool> DownloadFile()
        {
            Load_Text.Text = "Downloading...";
            Warning.Visibility = Visibility.Collapsed;
            Done.Visibility = Visibility.Collapsed;
            Clock.Visibility = Visibility.Visible;
            Loading_Layer.Visibility = Visibility.Visible;
            Loading_Layer.Tag = "Downloading";
            try
            {
                await new WebClient().DownloadFileTaskAsync("https://bdu.fstec.ru/files/documents/thrlist.xlsx", "thrlist.xlsx");
            }
            catch (Exception e)
            {
                Warning.Visibility = Visibility.Visible;
                Load_Text.Text = e.Message;
                return false;
            }
            Loading_Layer.Tag = "Done";
            Done.Visibility = Visibility.Visible;
            Clock.Visibility = Visibility.Collapsed;
            Load_Text.Text = "Загрузка успешно завершена!";
            return true;
        }

        public async void OpenFile()
        {
            pageThreats.Clear();
            newThreatsBase.Clear();

            if (File.Exists("local_thrlist.txt"))
            {
                ReadFromLocalBase();
            }
            else
            {
                bool check = await DownloadFile();
                if (!check) return;

                ReadFromXlsx();
            }
            Page = 1;
            MaxPage = newThreatsBase.Count / 15 + 1;
            Page_Text.Text = Page.ToString();
            Max_Page_Text.Text = $"/{MaxPage}";
            TurnPage(1);
        }
        public MainWindow()
        {
            InitializeComponent();
        }



        private void Page_Text_Key_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(Page_Text.Text, out int tempPage))
                {
                    TurnPage(tempPage);
                }
                else
                {
                    Page_Text.Text = Page.ToString();
                }
            }
        }


        private void Main_List_Double_Click(object sender, MouseButtonEventArgs e)
        {
            if (Main_List.SelectedItem != null)
            {
                ShowDescription();
                Name_Text.Text = $"УБИ.{(Main_List.SelectedItem as Threat).Id} - {(Main_List.SelectedItem as Threat).Name} ";
                Description_Text.Text = $"{(Main_List.SelectedItem as Threat).Discription}";
                Source_Text.Text = $"{(Main_List.SelectedItem as Threat).Source}";
                Subject_Text.Text = $"{(Main_List.SelectedItem as Threat).Subject}";
                Confidential_Text.Text = $"{(Main_List.SelectedItem as Threat).ConfidentialString}";
                Integ_Text.Text = $"{(Main_List.SelectedItem as Threat).IntegrityString}";
                Availability_Text.Text = $"{(Main_List.SelectedItem as Threat).AvailabilityString}";
            }
        }


        private void Save_Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (newThreatsBase.Count == 0)
            {
                Loading_Layer.Visibility = Visibility.Visible;
                Warning.Visibility = Visibility.Visible;
                Done.Visibility = Visibility.Collapsed;
                Clock.Visibility = Visibility.Collapsed;
                Load_Text.Text = "Нечего сохранять!";
            }
            else
            {
                string threatLine;
                using (StreamWriter outputFile = new StreamWriter("local_thrlist.txt"))
                {
                    foreach (var line in newThreatsBase)
                    {
                        threatLine = line.Value.Id.ToString() + newPropertyThread +
                                     line.Value.Name.Replace("\r\n", EnterSymbol.ToString()) + newPropertyThread +
                                     line.Value.Discription.Replace("\r\n", EnterSymbol.ToString()) + newPropertyThread +
                                     line.Value.Source.Replace("\r\n", EnterSymbol.ToString()) + newPropertyThread +
                                     line.Value.Subject.Replace("\r\n", EnterSymbol.ToString()) + newPropertyThread +
                                     line.Value.Confidential + newPropertyThread +
                                     line.Value.Integrity + newPropertyThread +
                                     line.Value.Availability + newPropertyThread +
                                     line.Value.DateIn.Replace("\r\n", EnterSymbol.ToString()) + newPropertyThread +
                                     line.Value.LastDate.Replace("\r\n", EnterSymbol.ToString());
                        outputFile.WriteLine(threatLine);
                    }
                }
            }
        }

        private async void Refresh_Button_Click(object sender, MouseButtonEventArgs e)
        {
            if (newThreatsBase.Count == 0)
            {
                Loading_Layer.Visibility = Visibility.Visible;
                Warning.Visibility = Visibility.Visible;
                Done.Visibility = Visibility.Collapsed;
                Clock.Visibility = Visibility.Collapsed;
                Load_Text.Text = "Нечего обновлять, сначала откройте базу!";
            }
            else
            {
                oldThreatsBase.Clear();
                allChanges.Clear();

                bool check = await DownloadFile();
                if (!check) return;
                foreach (var item in newThreatsBase)
                {
                    oldThreatsBase.Add(item.Key, item.Value);
                }
                newThreatsBase.Clear();
                ReadFromXlsx();
                List<Change> sortetChanges = new List<Change>();


                Page = 1;
                MaxPage = newThreatsBase.Count / 15 + 1;
                Page_Text.Text = Page.ToString();
                Max_Page_Text.Text = $"/{MaxPage}";
                TurnPage(1);

                foreach (var item in oldThreatsBase)
                {
                    if (!newThreatsBase.ContainsKey(item.Key))
                    {
                        sortetChanges.Add(new Change("Удалено", item.Value));
                    }

                }
                foreach (var item in newThreatsBase)
                {
                    if (!oldThreatsBase.ContainsKey(item.Key))
                    {
                        sortetChanges.Add(new Change("Добавлено", item.Value));
                    }
                    if (oldThreatsBase.ContainsKey(item.Key))
                    {
                        if (newThreatsBase[item.Key].LastDate != oldThreatsBase[item.Key].LastDate)
                        {
                            sortetChanges.Add(new Change("Изменено", item.Value) { oldThreat = oldThreatsBase[item.Key], newThreat = newThreatsBase[item.Key] });
                        }
                    }
                }
                sortetChanges = sortetChanges.OrderBy(o => o.ChangedThreat.Id).ToList();
                foreach (var item in sortetChanges)
                {
                    allChanges.Add(item);
                }
                if (allChanges.Count == 0)
                {
                    MessageBox.Show("Изменений нет!");
                }
                else
                {
                    Changes changes_window = new Changes();
                    changes_window.Show();
                    changes_window.Changes_List.ItemsSource = allChanges;
                }
            }
        }

        private void Open_File_Button(object sender, MouseButtonEventArgs e)
        {
            OpenFile();
            Main_List.ItemsSource = pageThreats;
        }

        private void Next_Page_Button_CLick(object sender, MouseButtonEventArgs e)
        {
            if ((Page + 1) <= MaxPage)
            {
                Page++;
                TurnPage(Page);
            }
        }

        private void Previos_Page_Button_Click(object sender, MouseButtonEventArgs e)
        {
            if ((Page - 1) > 0)
            {
                Page--;
                TurnPage(Page);
            }
        }

        private void Hide_Load_Screen(object sender, MouseButtonEventArgs e)
        {
            if (Loading_Layer.Tag.ToString() == "Done")
            {
                Loading_Layer.Visibility = Visibility.Collapsed;
            }
        }

        private void Hide_Description_Button(object sender, MouseButtonEventArgs e)
        {
            HideDescription();
        }

        public void ShowDescription()
        {
            Storyboard positionStoryboard = new Storyboard();

            DoubleAnimation positionAnimation = new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                FillBehavior = FillBehavior.HoldEnd,
                EasingFunction = new QuadraticEase()
                {
                    EasingMode = EasingMode.EaseOut
                }
            };
            Storyboard.SetTargetProperty(positionAnimation, new PropertyPath("RenderTransform.(TranslateTransform.X)"));
            positionStoryboard.Children.Add(positionAnimation);
            positionStoryboard.Begin(Description_Grid);
        }

        public void HideDescription()
        {
            Storyboard positionStoryboard = new Storyboard();

            DoubleAnimation positionAnimation = new DoubleAnimation()
            {
                To = 600,
                Duration = TimeSpan.FromSeconds(0.5),
                FillBehavior = FillBehavior.HoldEnd,
                EasingFunction = new QuadraticEase()
                {
                    EasingMode = EasingMode.EaseIn
                }
            };
            Storyboard.SetTargetProperty(positionAnimation, new PropertyPath("RenderTransform.(TranslateTransform.X)"));
            positionStoryboard.Children.Add(positionAnimation);
            positionStoryboard.Begin(Description_Grid);

        }
    }
}
