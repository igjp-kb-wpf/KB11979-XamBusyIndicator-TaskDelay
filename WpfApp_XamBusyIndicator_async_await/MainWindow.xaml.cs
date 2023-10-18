using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace WpfApp_XamBusyIndicator_async_await
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // ボタンがクリックされたら、xamBusyIndicator を表示させ、データ生成処理を非同期で実行する。
        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            //this.xamBusyIndicator1.DisplayAfter = new TimeSpan(0, 0, 0, 0); // 表示遅延を0秒に変更

            // XamBusyIndicator の IsBusy を true にして表示する
            this.xamBusyIndicator1.IsBusy = true;

            // xamDataGridのデータ初期化を行う
            this.xamDataGrid1.DataSource = new List<Item>();

            // データ生成処理を非同期で実行する
            var resultData = await Task.Run(() => this.CreateData());

            this.xamDataGrid1.DataSource = resultData;

            // 階層データを持つ xamDataGrid を全展開する処理
            //this.xamDataGrid1.Records.ExpandAll(true);

            // 階層データを持つ xamDataGrid をレコード毎に展開し、都度 Task.Delay を実行する。
            foreach (var record in this.xamDataGrid1.Records.Cast<Record>())
            {
                record.IsExpanded = true;
                await Task.Delay(1);
            }

            // XamBusyIndicator の IsBusy を false にして非表示にする
            this.xamBusyIndicator1.IsBusy = false;
        }

        // データ生成処理
        private List<Item> CreateData()
        {
            // Thread.Sleep(3000);
            var listItems = new List<Item>();

            for (int i = 0; i < 100; i++)
            {
                var parentItem = new Item();
                parentItem.Item1 = $"parent_1_{i}";
                parentItem.Item2 = $"parent_2_{i}";
                parentItem.Item3 = $"parent_3_{i}";
                parentItem.Item4 = $"parent_4_{i}";
                parentItem.Item5 = $"parent_5_{i}";
                for (int j = 0; j < 1000; j++)
                {
                    var child = new Item();
                    child.Item1 = $"child_1_{i}-{j}";
                    child.Item2 = $"child_2_{i}-{j}";
                    child.Item3 = $"child_3_{i}-{j}";
                    child.Item4 = $"child_4_{i}-{j}";
                    child.Item5 = $"child_5_{i}-{j}";
                    parentItem.Items.Add(child);
                }
                listItems.Add(parentItem);
            }
            return listItems;
        }
    }

    public class Item
    {
        public string Item1 { get; set; }
        public string Item2 { get; set; }
        public string Item3 { get; set; }
        public string Item4 { get; set; }
        public string Item5 { get; set; }

        public List<Item> Items { get; set; }

        public Item()
        {
            this.Item1 = "";
            this.Item2 = "";
            this.Item3 = "";
            this.Item4 = "";
            this.Item5 = "";
            this.Items = new List<Item>();
        }
    }
}
