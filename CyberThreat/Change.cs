using System.Windows.Media;

namespace CyberThreat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class Change
    {
        public Threat ChangedThreat { get; set; }
        public string Name { get; set; }
        public Brush Color { get; set; }
        public Brush FillColor { get; set; }
        public Threat oldThreat { get; set; }
        public Threat newThreat { get; set; }
        public Change(string name, Threat threat)

        {
            Name = name;
            ChangedThreat = threat;
            var converter = new System.Windows.Media.BrushConverter();
            switch (name)
            {
                case "Удалено":
                    Color = (Brush)converter.ConvertFromString("#C72C41");
                    FillColor = (Brush)converter.ConvertFromString("#FF7673");
                    break;
                case "Изменено":
                    Color = (Brush)converter.ConvertFromString("#FFD500");
                    FillColor = (Brush)converter.ConvertFromString("#FFF3A7");
                    break;
                case "Добавлено":
                    Color = (Brush)converter.ConvertFromString("#00BB3F");
                    FillColor = (Brush)converter.ConvertFromString("#97E293");
                    break;
            }
        }
    }

}
