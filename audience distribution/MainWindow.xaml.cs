using Microsoft.Win32;
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

namespace audience_distribution
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataTable table = new DataTable();
 
        string path = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                path = openFileDialog.FileName;
            FilePathBlock.Text = path;

        }

        private void UpdateDataBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateTable();
       
        }

        private void ResetFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            Week.Text = "";
            Day.Text = "";

            UpdateTable();
        }

        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            DataTable filterTable = table.Clone();
            filterTable.Rows.Clear();
            
            foreach (DataRow row in table.Rows)
            {
                if(row["Дата"] != "")
                if(Convert.ToDateTime(row["Дата"]).DayOfYear == Convert.ToInt32(Week.Text) * 7 + Convert.ToInt32(Day.Text))
                    {
                       
                        filterTable.Rows.Add(row.ItemArray);
                    }
                    
            }

            Grid.ItemsSource = filterTable.DefaultView;
        }
        private void UpdateTable()
        {
            table.Rows.Clear();
            table.Columns.Clear();


            table = ConvertTxtToDataTable(path);
            Grid.ItemsSource = table.DefaultView;
        }
        private DataTable ConvertTxtToDataTable(string filePath)
        {
            DataTable tbl = new DataTable();//Пустая таблица
            string[] lines = System.IO.File.ReadAllLines(filePath,Encoding.Default);//загрузка файла построчно

            //Заполнение названий столбцов
            string[] headerNames = lines[0].Split('\t'); 
            for(int i = 0; i < headerNames.Length; i++)
                tbl.Columns.Add(headerNames[i]);
            //Заполнение строк
            for (int i = 1; i< lines.Length;i++)
            {
                var cols = lines[i].Split('\t');

                DataRow dr = tbl.NewRow();
                for (int j = 0; j < headerNames.Length; j++)
                    dr[j] = cols[j];

                tbl.Rows.Add(dr);
            }


            return tbl;
        }

        private void startTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void endTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
