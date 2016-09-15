using System;
using System.Collections.Generic;
using System.Linq;
using QCBDManagementBusiness;
using LiveCharts;
using QCBDManagementWPF.Classes;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Windows.Media;
using System.Windows;
using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Command;
using System.ComponentModel;
using System.Windows.Threading;
using QCBDManagementWPF.Models;
using System.IO;
using System.Xml.Serialization;

namespace QCBDManagementWPF.ViewModel
{
    public class HomeViewModel : BindBase
    {
        private SeriesCollection _purchaseAndSalePriceseriesCollection;
        private SeriesCollection _payReceivedSeries;
        private ChartValues<DateTimePoint> _chartValueList;
        private List<StatisticModel> _statisticList;
        private List<ToDo> _toDoList;
        private string _newTask;

        //private Func<double, string> _xFormatter;
        //private Func<double, string> _yFormatter;

        //----------------------------[ Models ]------------------

        private ItemModel _firstBestSeller;
        private ItemModel _secondBestSeller;
        private ItemModel _ThirdBestSeller;
        private ItemModel _fourthBestSeller;
        private SeriesCollection _creditSeries;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<ToDo> DeleteToDoTaskCommand { get; set; }

        public HomeViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onNewTaskChange_SaveToToDoList;
        }
        private void instances()
        {
            _toDoList = new List<ToDo>();
            _firstBestSeller = new ItemModel();
            _secondBestSeller = new ItemModel();
            _ThirdBestSeller = new ItemModel();
            _fourthBestSeller = new ItemModel();
            _purchaseAndSalePriceseriesCollection = new SeriesCollection();
            _payReceivedSeries = new SeriesCollection();            
        }

        private void instancesModel()
        {

        }

        private void instancesCommand()
        {
            DeleteToDoTaskCommand = new ButtonCommand<ToDo>(deleteTask, canDeleteTask);
        }

        //----------------------------[ Properties ]------------------

        public BusinessLogic Bl
        {
            get { return Startup.Bl; }
            set { Startup.Bl = value; onPropertyChange("Bl"); }
        }

        public List<StatisticModel> StatisticDataList
        {
            get { return _statisticList; }
            set { _statisticList = value; onPropertyChange("StatisticDataList"); }
        }

        public ItemModel FirstBestItemModelSeller
        {
            get { return _firstBestSeller; }
            set { _firstBestSeller  = value; onPropertyChange("FirstBestItemModelSeller"); }
        }

        public ItemModel SecondBestItemModelSeller
        {
            get { return _secondBestSeller; }
            set { _secondBestSeller = value; onPropertyChange("SecondBestItemModelSeller"); }
        }

        public ItemModel ThirdBestItemModelSeller
        {
            get { return _ThirdBestSeller; }
            set { _ThirdBestSeller = value; onPropertyChange("ThirdBestItemModelSeller"); }
        }

        public ItemModel FourthBestItemModelSeller
        {
            get { return _fourthBestSeller; }
            set { _fourthBestSeller = value; onPropertyChange("FourthBestItemModelSeller"); }
        }

        public List<ToDo> ToDoList
        {
            get { return _toDoList; }
            set { _toDoList = value; onPropertyChange("ToDoList"); }
        }

        public string TxtNewTask
        {
            get { return _newTask; }
            set { _newTask = value; onPropertyChange("TxtNewTask"); }
        }

        public SeriesCollection PurchaseAndIncomeSeriesCollection
        {
            get { return _purchaseAndSalePriceseriesCollection; }
            set { _purchaseAndSalePriceseriesCollection = value; onPropertyChange("PurchaseAndIncomeSeriesCollection"); }
        }

        public SeriesCollection PayReceivedSeriesCollection
        {
            get { return _payReceivedSeries; }
            set { setProperty(ref _payReceivedSeries, value, "PayReceivedSeriesCollection"); }
        }

        public SeriesCollection CreditSeriesCollection
        {
            get { return _creditSeries; }
            set { setProperty(ref _creditSeries, value, "CreditSeriesCollection"); }
        }

        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public string[] PurchaseAndIncomeLabels { get; set; }
        public string[] PayReceivedAndBillLabels { get; set; }

        private string readTxtNewTask()
        {
            return _newTask;
        }

        private void setTxtNewTask(string task)
        {
            _newTask = task;
        }
        //----------------------------[ Actions ]------------------
        
        public async void loadData()
        {
            Dialog.showSearch("Loading...");
            StatisticDataList = (await Bl.DALStatisitc.searchStatisticFromWebService(new Statistic { Option = 1 }, "AND")).Select(x=>new StatisticModel { Statistic = x }).ToList();
            ToDoList = getToDoTasks();
            loadUIData();
            Dialog.IsDialogOpen = false;
        }


        private void loadUIData()
        {
            loadDataGauge();
            //loadChartPayreceivedData();
            loadPurchaseAndIncomeChart();
            loadPayReceivedAndBillChart();
        }

        private void loadPayReceivedAndBillChart()
        {
            var payReceivedChartValue = new ChartValues<decimal>();
            var billAmountChartValue = new ChartValues<decimal>();

            payReceivedChartValue.AddRange(StatisticDataList.OrderBy(x => x.Statistic.ID).Select(x => x.Statistic.Pay_received).ToList());
            billAmountChartValue.AddRange(StatisticDataList.OrderBy(x => x.Statistic.ID).Select(x => x.Statistic.Total_tax_included).ToList());

            CreditSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Pay received",
                    Values = payReceivedChartValue
                },
                new LineSeries
                {
                    Title = "Bill",
                    Values = billAmountChartValue
                }
            };

            PayReceivedAndBillLabels = StatisticDataList.OrderBy(x => x.Statistic.ID).Select(x => x.Statistic.Bill_date.ToString("MMM")).ToArray();

        }

        private void loadPurchaseAndIncomeChart()
        {
            var purchaseChartValue = new ChartValues<decimal>();
            var IncomeChartValue = new ChartValues<decimal>();
            var BillAmountChartValue = new ChartValues<decimal>();

            purchaseChartValue.AddRange(StatisticDataList.OrderBy(x=>x.Statistic.ID).Select(x=>x.Statistic.Price_purchase_total).ToList());
            IncomeChartValue.AddRange(StatisticDataList.OrderBy(x => x.Statistic.ID).Select(x => x.Statistic.Income).ToList());
            BillAmountChartValue.AddRange(StatisticDataList.OrderBy(x => x.Statistic.ID).Select(x => x.Statistic.Total_tax_included).ToList());

            PurchaseAndIncomeSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Sale",
                    Values = BillAmountChartValue
                },
                new LineSeries
                {
                    Title = "Purchase",
                    Values = purchaseChartValue
                },
                new LineSeries
                {
                    Title = "Income",
                    Values = IncomeChartValue
                }
            };

            PurchaseAndIncomeLabels = StatisticDataList.OrderBy(x => x.Statistic.ID).Select(x=>x.Statistic.Bill_date.ToString("MMMM") ).ToArray();
            //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
        }

        private void loadDataGauge()
        {
            getBestSellers();
        }

        public async void getBestSellers()
        {
            var itemBestSsellerList = await Bl.BlItem.searchItemFromWebService(new Item { Option =1  }, "AND");
            if(itemBestSsellerList.Count > 4)
            {
                FirstBestItemModelSeller = itemBestSsellerList.OrderByDescending(x => x.Number_of_sale).Select(x => new ItemModel { Item = x }).FirstOrDefault();
                SecondBestItemModelSeller = itemBestSsellerList.OrderByDescending(x => x.Number_of_sale).Where(x => x.Number_of_sale < FirstBestItemModelSeller.Item.Number_of_sale).Select(x => new ItemModel { Item = x }).FirstOrDefault();
                ThirdBestItemModelSeller = itemBestSsellerList.OrderByDescending(x => x.Number_of_sale).Where(x => x.Number_of_sale < SecondBestItemModelSeller.Item.Number_of_sale).Select(x => new ItemModel { Item = x }).FirstOrDefault();
                FourthBestItemModelSeller = itemBestSsellerList.OrderByDescending(x => x.Number_of_sale).Where(x => x.Number_of_sale < ThirdBestItemModelSeller.Item.Number_of_sale).Select(x => new ItemModel { Item = x }).FirstOrDefault();
            }             
        }

        private void loadChartPayreceivedData()
        {
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };

            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(33, 148, 241), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));

            PayReceivedSeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = GetDataPayreceived(),
                        Fill = gradientBrush,
                        StrokeThickness = 1,
                        PointGeometrySize = 0
                    }
                };


            XFormatter = val => new DateTime((long)val).ToString("dd MMM");
            YFormatter = val => val.ToString("C");
        }


        private ChartValues<DateTimePoint> GetDataPayreceived()
        {
            loadStatisticPayReceived();
            return _chartValueList;
        }        

        private void loadStatisticPayReceived()
        {            
            _chartValueList = new ChartValues<DateTimePoint>();
            var statistics = StatisticDataList.OrderBy(x => x.Statistic.Date_limit).ToList();
            foreach (var statisticModel in statistics)
            {
                _chartValueList.Add(new DateTimePoint(statisticModel.Statistic.Pay_date, (double)statisticModel.Statistic.Pay_received));
            }
        }

        private void setNewTask()
        {
            var toDo = new ToDo();
            toDo.PropertyChanged += onToDoListIsDoneChange;
            toDo.TxtTask = TxtNewTask;
            ToDoList.Add(toDo);
            ToDoList = new List<ToDo>(ToDoList);
        }

        private void saveToDoTasks(List<ToDo> taskList)
        {
            string path = Directory.GetCurrentDirectory();
            string fileName = "tasks.xml";
            string fullFileName = string.Format(@"{0}\Docs\Files\{1}", path, fileName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //if (!File.Exists(fullFileName))
            //    File.Create(fullFileName);
            if (taskList.Count == 0)
                return;

            using (StreamWriter sw = new StreamWriter(fullFileName))
            {
                XmlSerializer xs = new XmlSerializer(taskList.GetType());
                xs.Serialize(sw, taskList);
            }
        }

        private List<ToDo> getToDoTasks()
        {
            string path = Directory.GetCurrentDirectory();
            string fileName = "tasks.xml";
            string fullFileName = string.Format(@"{0}\Docs\Files\{1}", path, fileName);

            List<ToDo> results = new List<ToDo>();
            if (File.Exists(fullFileName))
            {
                using (StreamReader sr = new StreamReader(fullFileName))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(List<ToDo>));
                    results = (List<ToDo>)xs.Deserialize(sr);
                }
                foreach (var todo in results)
                    todo.PropertyChanged += onToDoListIsDoneChange;
            }
            return results;
        }

        //----------------------------[ Event Handler ]------------------

        private void onNewTaskChange_SaveToToDoList(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtNewTask"))
            {
                setNewTask();
                saveToDoTasks(ToDoList);
            }
        }

        private void onToDoListIsDoneChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsDone"))
            {
                saveToDoTasks(ToDoList);
            }
        }

        //----------------------------[ Action Commands ]------------------


        private void deleteTask(ToDo obj)
        {
            ToDoList.Remove(obj);
            ToDoList = new List<ToDo>(ToDoList);
            saveToDoTasks(ToDoList);
        }

        private bool canDeleteTask(ToDo arg)
        {
            return true;
        }
    }
}
