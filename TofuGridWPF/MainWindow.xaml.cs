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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using Newtonsoft.Json;
using System.Data;
using System.Collections.ObjectModel;

namespace TofuGridWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();



            loadData();
            createDataGrid();
        }


        private ObservableCollection<TofuData> data { get; set; }

        private otherData otherData { get; set; }

        public void loadData()
        {
            JsonSerializer js = new JsonSerializer();

            data=new ObservableCollection<TofuData>();

            string fileName = "data.json";
            Newtonsoft.Json.Linq.JArray jArray = null;
            using(TextReader tr=new StreamReader(fileName))
            using(JsonReader jr=new JsonTextReader(tr))
            {
                var temp=js.Deserialize<List<Dictionary<string, string>>>(jr);
                foreach(Dictionary<string, string> item in temp)
                {
                    TofuData d = new TofuData();
                    d.data = item;
                    data.Add(d);
                }
            }

            Console.WriteLine("debug");


            var items = new Dictionary<string, string>();
            items.Add("item0", "アイテム0");
            items.Add("item1", "アイテム1");
            items.Add("item2", "アイテム2");
            items.Add("item3", "アイテム3");
            items.Add("item4", "アイテム4");
            items.Add("item5", "アイテム5");
            items.Add("item6", "アイテム6");
            otherData =new otherData();
            otherData.items = items;
        }

        //public string bindTest { get; set; } = "aa";//clear

        public void createDataGrid()
        {
            tofuGrid.Columns.Clear();
            tofuGrid.AutoGenerateColumns = false;

            var temp = data[0];

            foreach(var k in temp.data.Keys)
            {
                setColumn(k);
            }

            tofuGrid.ItemsSource = data;

        }

        private DataTemplate getCellTemp(string k)
        {
            if (k == "CheckBox")
            {
                return createCheckBox(k);
            }
            else if (k == "ComboBox")
            {
                return createComboBox(k);
            }
            else if (k == "Integer")
            {
                return createTextBox(k);
            }
            else if (k == "String")
            {
                return createTextBox(k);
            }
            else if (k == "Row1")
            {
                return createRow();
            }
            return null;
        }

        private void setColumn(string k)
        {
            var col = new DataGridTemplateColumn();
            col.Header = k;
            var celltemp=getCellTemp(k);
            if (celltemp == null) return;
            col.CellTemplate = getCellTemp(k);
            tofuGrid.Columns.Add(col);
        }
        private DataTemplate createCheckBox(string k)
        {
            var temp = new DataTemplate();
            var root = new FrameworkElementFactory(typeof(CheckBox));
            var bind = new Binding("data[" + k + "]");
            root.SetBinding(CheckBox.IsCheckedProperty, bind);
            temp.VisualTree = root;
            return temp;
        }
        private DataTemplate createTextBox(string k)
        {
            var temp = new DataTemplate();
            var root = new FrameworkElementFactory(typeof(TextBox));
            var bind = new Binding("data[" + k + "]");
            root.SetBinding(TextBox.TextProperty, bind);
            temp.VisualTree = root;
            return temp;
        }
        private DataTemplate createComboBox(string k)
        {
            var temp = new DataTemplate();
            var root = new FrameworkElementFactory(typeof(ComboBox));
            var bind = new Binding("data[" + k + "]");
            root.SetBinding(ComboBox.SelectedValueProperty, bind);
            bind = new Binding("items");
            bind.Source = otherData;
            root.SetBinding(ComboBox.ItemsSourceProperty, bind);
            root.SetValue(ComboBox.SelectedValuePathProperty, "Key");
            root.SetValue(ComboBox.DisplayMemberPathProperty,"Value");
            temp.VisualTree = root;
            return temp;
        }

        private DataTemplate createRow()
        {
            var temp = new DataTemplate();
            var root = new FrameworkElementFactory(typeof(StackPanel));
            root.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
            var row = new FrameworkElementFactory(typeof(TextBox));
            var bind = new Binding("data[Row1]");
            row.SetBinding(TextBox.TextProperty, bind);
            root.AppendChild(row);
            row = new FrameworkElementFactory(typeof(TextBox));
            bind = new Binding("data[Row2]");
            row.SetBinding(TextBox.TextProperty, bind);
            root.AppendChild(row);

            temp.VisualTree = root;
            return temp;
        }
    }
}

public class TofuData
{
    public Dictionary<string, string> data { get; set; }
 
}

public class otherData
{
    public Dictionary<string, string> items { get; set; }
}
