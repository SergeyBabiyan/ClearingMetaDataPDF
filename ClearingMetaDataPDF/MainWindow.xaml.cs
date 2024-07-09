using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
using System.Xml.Serialization;


namespace ClearingMetaDataPDF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MainWindow window;

        string PathPDFFile = null;

        List<string> Paths = new List<string>();
        List<Button> Buttons = new List<Button>();

        MetaDataCleaner Cleaner = new MetaDataCleaner();

        Canvas Canvas = null;
        Button ClearButton;

        public MainWindow()
        {
            InitializeComponent();
            window = this;
            
        }


        //действия при наведении курсороа с файлом на панель
        private void StackPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        


        //действия после дропа файла
        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                if (CheckPDF(file))//проверка на то, что файл является pdf докуметном 
                {
                    ClearButton = new Button();
                    ClearButton.Width = 15;
                    ClearButton.Height = 15;
                    ClearButton.FontSize = 1;
                    ClearButton.Margin = new Thickness(280, 8, 0, 0);
                    ClearButton.Click += DeleteCanvas;
                    ClearButton.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("C:\\Users\\serge\\source\\repos\\ClearingMetaDataPDF\\ClearingMetaDataPDF\\krest.png", UriKind.RelativeOrAbsolute)) };

                    PathPDFFile = file;
                    Paths.Add(PathPDFFile);
                    Canvas = new Canvas();
                    Canvas.Height = 30;
                    ClearButton.Content = Paths.Count.ToString();
                    Buttons.Add(ClearButton);
                    Canvas.Children.Add(new Label() { Content = CharacterReduction(PathPDFFile,47), Foreground = System.Windows.Media.Brushes.White, FontSize = 11 });
                    Canvas.Children.Add(ClearButton);
                    PathsPanels.Children.Add(Canvas);
                }
            }


            
        }


        //функция проверки на то, что файл является pdf докуметном
        private bool CheckPDF(string filePath)
        {
            char[] chars = filePath.ToCharArray();
            if (chars[chars.Length - 1] == 'f' && chars[chars.Length - 2] == 'd' && chars[chars.Length - 3] == 'p')
            {
                return true;
            }
            else 
            { 
                MessageBox.Show("Not PDF File", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
                return false;  
            }
        }

        //обработка нажатия кнопки очистки
        private void ClearMetaDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (PathPDFFile != null)
            {
                foreach (string Path in Paths)
                {
                    Cleaner.RemoveMetadata(Path);
                }
                
            }
            DeleteAllCanvas();
        }


        //удаление всех файлов
        void DeleteAllCanvas()
        {


            for (int i = 0; i < Buttons.Count;)
            {
                Buttons.RemoveAt(i);
            }

            for (int i = 0; i < Paths.Count;)
            {
                Paths.RemoveAt(i);
            }

            for (int i = 0; i < PathsPanels.Children.Count;)
            {
                PathsPanels.Children.RemoveAt(i);
            }

        }

        //удаление всей файлов по нажатию кнопки
        void DeleteAllCanvas_Click(object sender, RoutedEventArgs e)
        {
            DeleteAllCanvas();
        }


        //удаление файла при нажатии на кнопку
        void DeleteCanvas(object sender, RoutedEventArgs e)
        {
            int a = Convert.ToInt32((sender as Button).Content) - 1;

            Paths.Remove(Paths[a]);
            PathsPanels.Children.Remove(PathsPanels.Children[a]);
            Buttons.Remove(Buttons[a]);

            for (int i = 0; i < Buttons.Count; i ++)
            {
                Buttons[i].Content = i + 1;
            }
        }


        //функиция сокращения символов
        string CharacterReduction(string text, int MaxSymbol)
        {
            char[] chars = text.ToCharArray();
            string Redacttext = "";
            if (chars.Length <= MaxSymbol)
            {
                return text;
            }
            else
            {
                for (int i = 0; i < MaxSymbol; i++)
                {
                    Redacttext += chars[i];
                }
                Redacttext += " . . .";
                return Redacttext;
            }
        }

    }
}
    

