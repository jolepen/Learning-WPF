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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfExample.Views
{
    /// <summary>
    /// HierarchyControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HierarchyControl : UserControl
    {
        public List<HierarchyInfo> HierarchyList { get; init; }

        public HierarchyControl()
        {
            InitializeComponent();

            HierarchyList =
            [
                CreateHirarchyInfo("성춘향", "사원", "온라인", "AAA부"),
                CreateHirarchyInfo("홍길동", "대리", "오프라인", "AAA부"),
                CreateHirarchyInfo("심청이", "과장", "온라인", "BBB부"),
                CreateHirarchyInfo("이도령", "차장", "오프라인", "BBB부"),
                CreateHirarchyInfo("이몽룡", "부장", "온라인", "CCC부"),
                CreateHirarchyInfo("최두목", "이사", "오프라인", "CCC부")
            ];

            this.rootTreeViewItem.Selected += Item_Selected;
            this.FetchTreeView();
        }

        private void FetchTreeView()
        {
            this.rootTreeViewItem.Items.Clear();
            this.listBox.ItemsSource = null;

            foreach (HierarchyInfo info in this.HierarchyList)
            {
                bool isHeaderExists = false;
                foreach (TreeViewItem item in this.rootTreeViewItem.Items)
                {
                    if (item is TreeViewItem childItem && childItem.Header.ToString() == info.Location)
                    {
                        childItem.Items.Add(info);
                        isHeaderExists = true;
                        break;
                    }
                }

                if (!isHeaderExists)
                {
                    TreeViewItem newItem = new()
                    {
                        Style = (Style)this.Resources["CustomTreeViewItem"],
                        Header = info.Location
                    };
                    newItem.Items.Add(info);

                    this.rootTreeViewItem.Items.Add(newItem);
                }
            }

            foreach (TreeViewItem item in this.rootTreeViewItem.Items)
            {
                this.BindList(item);
            }
        }

        private void BindList(TreeViewItem treeViewItem)
        {
            //ItemSource를 활용한 데이터 바인딩
            if (treeViewItem.Parent is TreeViewItem parentItem)
            {
                this.hierarchyTextBlock.Text = $"{parentItem.Header} > {treeViewItem.Header}";
                this.listBox.ItemsSource = treeViewItem.Items;
            }
            else
            {
                this.hierarchyTextBlock.Text = $"{treeViewItem.Header}";
                this.listBox.ItemsSource = this.HierarchyList;
            }
        }

        private HierarchyInfo CreateHirarchyInfo(string name, string position, string status, string location)
        {
            return new()
            {
                Location = location,
                Name = name,
                Position = position,
                Status = status
            };
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            if (e.Source is TreeViewItem treeViewItem)
            {
                this.BindList(treeViewItem);
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is Grid mainGrid)
            {
                double gridWidth = mainGrid.ActualWidth;

                double minColumnWidth = gridWidth * 0.2;

                mainGrid.ColumnDefinitions[2].MinWidth = minColumnWidth;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            this.hierarchyTextBlock.Text = string.Empty;

            if (e.Key == Key.Return)
            {
                List<HierarchyInfo> list = [];
                foreach (TreeViewItem item in this.rootTreeViewItem.Items)
                {
                    foreach (HierarchyInfo hierarchyInfo in item.Items)
                    {
                        var textBox = (TextBox)sender;

                        if (hierarchyInfo.Name.Contains(textBox.Text))
                        {
                            list.Add(hierarchyInfo);
                        }
                    }
                }

                this.listBox.ItemsSource = list;
            }
        }

        private int locCount = 1;
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            this.HierarchyList.Add(CreateHirarchyInfo("AddTest", "Test", "오프라인", $"{locCount}부"));
            this.FetchTreeView();
            locCount++;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            int lastIndex = this.HierarchyList.Count - 1;

            if (lastIndex > -1)
            {
                this.HierarchyList.RemoveAt(lastIndex);
            }
            this.FetchTreeView();
        }
    }

    public class HierarchyInfo
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
    }
}
