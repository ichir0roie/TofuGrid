using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TofuGridWPF
{
    /// <summary>
    /// Interaction logic for DeepGrid.xaml
    /// </summary>
    public partial class DeepGrid : Window
    {
        public DeepGrid()
        {
            InitializeComponent();

            GetOC();
            SetColumns();
        }

        public int depth = 2;
        public int value = 0;
        public int width = 2;
        public int height = 2;

        public ObservableCollection<PropDict> oc;

        public class PropDict
        {
            public Dictionary<int, PropDict> d { get; set; }
            //public string value { get; set; }
            public object value { get; set; }

            public PropDict()
            {
                d = new Dictionary<int, PropDict>();
                value = "-";
            }
            public PropDict(object value)
            {
                this.value = value;
            }
        }

        public class testProp
        {
            public Dictionary<int,String> d { get; set; }
        }


        public void GetOC()
        {

            oc = new ObservableCollection<PropDict>();
            var oc2 = new ObservableCollection<testProp>();
            for (var w = 0; w < width; w++)
            {
                oc.Add(deepDict(depth));

                var tp = new testProp();
                tp.d = new Dictionary<int, string>();
                tp.d[0] = "value0";
                tp.d[1] = "value1";
                tp.d[2] = "value2";
                tp.d[3] = "value3";
                tp.d[4] = "value4";
                oc2.Add(tp);
            }



            dg.ItemsSource = oc;
            dg.AutoGenerateColumns = false;
            //dg.ItemsSource=oc2;

        }

        public string testV => "aaa";

        public void SetColumns()
        {
            var head = 0;
            foreach(var path in bindPaths)
            {
                var col = new DataGridTextColumn();
                //var b = new Binding(path);
                //b.Source = oc;
                //col.Binding = b;
                col.Binding = new Binding(path);
                col.Header = head.ToString();
                head++;
                dg.Columns.Add(col);
            }
        }

        public List<String> bindPaths = new List<string> {
            //"d[0]",
            //"d[1][1]",
            //"d[2][3]",
            //"d[3]",
            //"d[4]",
            "value.ToString()",
            "d[1].value.ToString()",
            "d[1].d[1].value.ToString()",
            "d[1].d[1].d[1].value.ToString()",
            "d[1].d[1].d[1].d[1].value.ToString()",
            "d[1].d[1].d[1].d[1].d[1].value.ToString()",
            "value",
            "d[1].value",
            "d[1].d[1].value",
            "d[1].d[1].d[1].value",
            "d[1].d[1].d[1].d[1].value",
            "d[1].d[1].d[1].d[1].d[1].value",
        };

        public PropDict deepDict(int depth)
        {
            var retDict=new PropDict();

            for(var c = 0; c < width; c++)
            {
                if (depth == 0)
                {
                    value++;
                    //retDict.Add(c, value);
                    retDict.d[c] = new PropDict(value.ToString());
                }
                else
                {
                    retDict.d[c]= deepDict(depth - 1);
                }
            }
            return retDict;
        }

    }
}
