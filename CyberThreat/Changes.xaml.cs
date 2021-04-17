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

namespace CyberThreat
{
    /// <summary>
    /// Interaction logic for Changes.xaml
    /// </summary>
    public partial class Changes : Window
    {
        public Changes()
        {
            InitializeComponent();
            ClearNew();
            ClearWas();
        }

        private void Changes_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Changes_List.SelectedItem != null)
            {
                RestoreHight();
                if ((Changes_List.SelectedItem as Change).Name == "Удалено")
                {
                    SetWas((Changes_List.SelectedItem as Change).ChangedThreat as Threat);
                    ClearNew();
                }
                if ((Changes_List.SelectedItem as Change).Name == "Добавлено")
                {
                    SetNew((Changes_List.SelectedItem as Change).ChangedThreat as Threat);
                    ClearWas();
                }
                if ((Changes_List.SelectedItem as Change).Name == "Изменено")
                {
                    ClearNew();
                    ClearWas();
                    Name_Text_Was.Text = $"УБИ.{(Changes_List.SelectedItem as Change).ChangedThreat.Id} - {(Changes_List.SelectedItem as Change).ChangedThreat.Name} ";
                    Name_Text_New.Text = $"УБИ.{(Changes_List.SelectedItem as Change).ChangedThreat.Id} - {(Changes_List.SelectedItem as Change).ChangedThreat.Name} ";
                    if ((Changes_List.SelectedItem as Change).oldThreat.Discription != (Changes_List.SelectedItem as Change).newThreat.Discription)
                    {
                        Description_Text_Was.Text = $"{(Changes_List.SelectedItem as Change).oldThreat.Discription}";
                        Description_Text_New.Text = $"{(Changes_List.SelectedItem as Change).newThreat.Discription}";
                    }
                    else
                    {
                        Description_Text_Was.Height = 0;
                        Description_Text_New.Height = 0;
                    }
                    if ((Changes_List.SelectedItem as Change).oldThreat.Source != (Changes_List.SelectedItem as Change).newThreat.Source)
                    {
                        Source_Text_Was_Tag.Text = "Источник угрозы:";
                        Source_Text_New_Tag.Text = "Источник угрозы:";
                        Source_Text_Was.Text = $"{(Changes_List.SelectedItem as Change).oldThreat.Source}";
                        Source_Text_New.Text = $"{(Changes_List.SelectedItem as Change).newThreat.Source}";
                    }
                    else
                    {
                        Source_Text_Was_Tag.Height = 0;
                        Source_Text_New_Tag.Height = 0;
                        Source_Text_Was.Height = 0;
                        Source_Text_New.Height = 0;
                    }
                    if ((Changes_List.SelectedItem as Change).oldThreat.Subject != (Changes_List.SelectedItem as Change).newThreat.Subject)
                    {
                        Subject_Text_Was_Tag.Text = "Объект воздействия:";
                        Subject_Text_New_Tag.Text = "Объект воздействия:";
                        Subject_Text_Was.Text = $"{(Changes_List.SelectedItem as Change).oldThreat.Subject}";
                        Subject_Text_New.Text = $"{(Changes_List.SelectedItem as Change).newThreat.Subject}";
                    }
                    else
                    {
                        Subject_Text_Was_Tag.Height = 0;
                        Subject_Text_New_Tag.Height = 0;
                        Subject_Text_Was.Height = 0;
                        Subject_Text_New.Height = 0;
                    }
                    if ((Changes_List.SelectedItem as Change).oldThreat.Confidential != (Changes_List.SelectedItem as Change).newThreat.Confidential)
                    {
                        Confidential_Text_Was_Tag.Text = "Нарушение конфиденциальности:";
                        Confidential_Text_New_Tag.Text = "Нарушение конфиденциальности:";
                        Confidential_Text_Was.Text = $"{(Changes_List.SelectedItem as Change).oldThreat.ConfidentialString}";
                        Confidential_Text_New.Text = $"{(Changes_List.SelectedItem as Change).newThreat.ConfidentialString}";
                    }
                    else
                    {
                        Confidential_Text_Was_Tag.Height = 0;
                        Confidential_Text_New_Tag.Height = 0;
                        Confidential_Text_Was.Height = 0;
                        Confidential_Text_New.Height = 0;
                    }
                    if ((Changes_List.SelectedItem as Change).oldThreat.Integrity != (Changes_List.SelectedItem as Change).newThreat.Integrity)
                    {
                        Integ_Text_Was_Tag.Text = "Нарушение целостности:";
                        Integ_Text_New_Tag.Text = "Нарушение целостности:";
                        Integ_Text_Was.Text = $"{(Changes_List.SelectedItem as Change).oldThreat.IntegrityString}";
                        Integ_Text_New.Text = $"{(Changes_List.SelectedItem as Change).newThreat.IntegrityString}";
                    }
                    else
                    {
                        Integ_Text_Was_Tag.Height = 0;
                        Integ_Text_New_Tag.Height = 0;
                        Integ_Text_Was.Height = 0;
                        Integ_Text_New.Height = 0;
                    }
                    if ((Changes_List.SelectedItem as Change).oldThreat.Availability != (Changes_List.SelectedItem as Change).newThreat.Availability)
                    {
                        Availability_Text_Was_Tag.Text = "Нарушение доступности:";
                        Availability_Text_New_Tag.Text = "Нарушение доступности:";
                        Availability_Text_Was.Text = $"{(Changes_List.SelectedItem as Change).oldThreat.AvailabilityString}";
                        Availability_Text_New.Text = $"{(Changes_List.SelectedItem as Change).newThreat.AvailabilityString}";
                    }
                    else
                    {
                        Availability_Text_Was_Tag.Height = 0;
                        Availability_Text_New_Tag.Height = 0;
                        Availability_Text_Was.Height = 0;
                        Availability_Text_New.Height = 0;
                    }
                }

            }
        }
        private void RestoreHight()
        {
            Description_Text_Was.Height = Double.NaN;
            Description_Text_New.Height = Double.NaN;
            Source_Text_Was_Tag.Height = Double.NaN;
            Source_Text_New_Tag.Height = Double.NaN;
            Source_Text_Was.Height = Double.NaN;
            Source_Text_New.Height = Double.NaN;
            Subject_Text_Was_Tag.Height = Double.NaN;
            Subject_Text_New_Tag.Height = Double.NaN;
            Subject_Text_Was.Height = Double.NaN;
            Subject_Text_New.Height = Double.NaN;
            Confidential_Text_Was_Tag.Height = Double.NaN;
            Confidential_Text_New_Tag.Height = Double.NaN;
            Confidential_Text_Was.Height = Double.NaN;
            Confidential_Text_New.Height = Double.NaN;
            Integ_Text_Was_Tag.Height = Double.NaN;
            Integ_Text_New_Tag.Height = Double.NaN;
            Integ_Text_Was.Height = Double.NaN;
            Integ_Text_New.Height = Double.NaN;
            Availability_Text_Was_Tag.Height = Double.NaN;
            Availability_Text_New_Tag.Height = Double.NaN;
            Availability_Text_Was.Height = Double.NaN;
            Availability_Text_New.Height = Double.NaN;
        }
        private void SetWas(Threat threat)
        {
            Name_Text_Was.Text = $"УБИ.{threat.Id} - {(Changes_List.SelectedItem as Change).ChangedThreat.Name} ";
            Description_Text_Was.Text = $"{threat.Discription}";
            Source_Text_Was.Text = $"{threat.Source}";
            Subject_Text_Was.Text = $"{threat.Subject}";
            Confidential_Text_Was.Text = $"{threat.ConfidentialString}";
            Integ_Text_Was.Text = $"{threat.IntegrityString}";
            Availability_Text_Was.Text = $"{threat.AvailabilityString}";

            Source_Text_Was_Tag.Text = "Источник угрозы:";
            Subject_Text_Was_Tag.Text = "Объект воздействия:";
            Confidential_Text_Was_Tag.Text = "Нарушение конфиденциальности:";
            Integ_Text_Was_Tag.Text = "Нарушение целостности:";
            Availability_Text_Was_Tag.Text = "Нарушение доступности:";
        }
        private void SetNew(Threat threat)
        {
            Name_Text_New.Text = $"УБИ.{threat.Id} - {(Changes_List.SelectedItem as Change).ChangedThreat.Name} ";
            Description_Text_New.Text = $"{threat.Discription}";
            Source_Text_New.Text = $"{threat.Source}";
            Subject_Text_New.Text = $"{threat.Subject}";
            Confidential_Text_New.Text = $"{threat.ConfidentialString}";
            Integ_Text_New.Text = $"{threat.IntegrityString}";
            Availability_Text_New.Text = $"{threat.AvailabilityString}";

            Source_Text_New_Tag.Text = "Источник угрозы:";
            Subject_Text_New_Tag.Text = "Объект воздействия:";
            Confidential_Text_New_Tag.Text = "Нарушение конфиденциальности:";
            Integ_Text_New_Tag.Text = "Нарушение целостности:";
            Availability_Text_New_Tag.Text = "Нарушение доступности:";
        }
        private void ClearWas()
        {
            Name_Text_Was.Text = "";
            Description_Text_Was.Text = "";
            Source_Text_Was.Text = "";
            Subject_Text_Was.Text = "";
            Confidential_Text_Was.Text = "";
            Integ_Text_Was.Text = "";
            Availability_Text_Was.Text = "";

            Source_Text_Was_Tag.Text = "";
            Subject_Text_Was_Tag.Text = "";
            Confidential_Text_Was_Tag.Text = "";
            Integ_Text_Was_Tag.Text = "";
            Availability_Text_Was_Tag.Text = "";
        }
        private void ClearNew()
        {
            Name_Text_New.Text = "";
            Description_Text_New.Text = "";
            Source_Text_New.Text = "";
            Subject_Text_New.Text = "";
            Confidential_Text_New.Text = "";
            Integ_Text_New.Text = "";
            Availability_Text_New.Text = "";

            Source_Text_New_Tag.Text = "";
            Subject_Text_New_Tag.Text = "";
            Confidential_Text_New_Tag.Text = "";
            Integ_Text_New_Tag.Text = "";
            Availability_Text_New_Tag.Text = "";
        }
    }
}
