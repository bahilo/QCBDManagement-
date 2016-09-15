using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using QCBDManagementWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Models;
using QCBDManagementWPF.Interfaces;
using System.Windows.Threading;
using System.IO;
using System.Threading;
using QCBDManagementCommon.Enum; 

namespace QCBDManagementWPF
{

    public class MainWindowViewModel : BindBase, IDisposable
    {
        public MainWindow MainWindow { get; set; }
        public bool isNewAgentAuthentication { get; set; }
        private Object _currentViewModel;
        private Object _currentSideBarViewModel;
        private Object _currentHomeViewModel;
        private string _blockGridCentralVisibility;
        private string _blockSideBarVisibility;
        private string _blockHomeVisibility;
        private Cart _cart;
        private double _progressBarPercentValue;
        private string _searchProgressVisibolity;
        private Context _context;
        private DisplayAndData.Display.Image _headerImageDisplay;
        private DisplayAndData.Display.Image _logoImageDisplay;
        private DisplayAndData.Display.Image _billImageDisplay;
        private bool _isThroughContext;
        
        //----------------------------[ Models ]------------------

        public ClientViewModel ClientViewModel { get; set; }
        public ItemViewModel ItemViewModel { get; set; }
        public CommandViewModel CommandViewModel { get; set; }
        public AgentViewModel AgentViewModel { get; set; }
        public NotificationViewModel NotificationViewModel { get; set; }
        public HomeViewModel HomeViewModel { get; set; }
        public ReferentialViewModel ReferentialViewModel { get; set; }
        public StatisticViewModel StatisticViewModel { get; set; }
        public QuoteViewModel QuoteViewModel { get; set; }
        public SecurityLoginViewModel SecurityLoginViewModel { get; set; }
        private CommandSideBarViewModel _commonCommandQuoteSideBarViewModel;
        //private string _navigButtonStyle;


        //----------------------------[ Commands ]------------------        

        public ButtonCommand<string> CommandNavig { get; set; }



        public MainWindowViewModel() : base()
        {
            init();
            instancesCommand();
            setInitEvents();
            setObjectsManagement();
            //SecurityLoginViewModel.showView();
            SecurityLoginViewModel.startAuthentication();

        }

        //----------------------------[ Initialization ]------------------

        private void init()
        {
            _searchProgressVisibolity = "Visible";
            _startup = new Startup();
            _context = new Context(navigation);
            _currentViewModel = null;
            _dialog = new ConfirmationViewModel();
            //_navigButtonStyle = "";// "DarkGray";

            //------[ Image ]
            _headerImageDisplay = new DisplayAndData.Display.Image();
            _headerImageDisplay.TxtFileNameWithoutExtension = "header_image";
            _headerImageDisplay.TxtName = "Header Image";
            _logoImageDisplay = new DisplayAndData.Display.Image();
            _logoImageDisplay.TxtFileNameWithoutExtension = "logo_image";
            _logoImageDisplay.TxtName = "Logo Image";
            _billImageDisplay = new DisplayAndData.Display.Image();
            _billImageDisplay.TxtFileNameWithoutExtension = "bill_image";
            _billImageDisplay.TxtName = "BIll Image";

            //------[ ViewModel ]
            _commonCommandQuoteSideBarViewModel = new CommandSideBarViewModel();
            ClientViewModel = new ClientViewModel();
            ItemViewModel = new ItemViewModel();
            CommandViewModel = new CommandViewModel();
            AgentViewModel = new AgentViewModel();
            HomeViewModel = new HomeViewModel();
            NotificationViewModel = new NotificationViewModel();
            ReferentialViewModel = new ReferentialViewModel();
            StatisticViewModel = new StatisticViewModel();
            QuoteViewModel = new QuoteViewModel();
            SecurityLoginViewModel = new SecurityLoginViewModel();

        }

        private void instancesCommand()
        {
            CommandNavig = new ButtonCommand<string>(appNavig, canAppNavig);
        }

        private void setInitEvents()
        {
            SecurityLoginViewModel.AgentModel.PropertyChanged += onAuthenticatedAgentChange;
        }

        private void setObjectsManagement()
        {
            //------[ Business Logic ]
            ClientViewModel.Startup = _startup;
            ItemViewModel.Startup = _startup;
            CommandViewModel.Startup = _startup;
            CommandQuoteSideBar.Startup = _startup;
            AgentViewModel.Startup = _startup;
            SecurityLoginViewModel.Startup = _startup;
            //NotificationViewModel.loadClients();
            HomeViewModel.Startup = _startup;
            ReferentialViewModel.Startup = _startup;
            QuoteViewModel.Startup = _startup;

            //------[ Dialog component ]
            ClientViewModel.Dialog = _dialog;
            ItemViewModel.Dialog = _dialog;
            CommandViewModel.Dialog = _dialog;
            CommandQuoteSideBar.Dialog = _dialog;
            AgentViewModel.Dialog = _dialog;
            SecurityLoginViewModel.Dialog = _dialog;
            HomeViewModel.Dialog = _dialog;
            ReferentialViewModel.Dialog = _dialog;
            QuoteViewModel.Dialog = _dialog;

            //------[ Side bar ]
            ItemViewModel.sideBarContentManagement(sideBarManagement);
            ClientViewModel.sideBarContentManagement(sideBarManagement);
            CommandViewModel.setSideBar(ref _commonCommandQuoteSideBarViewModel);
            CommandViewModel.sideBarContentManagement(sideBarManagement);
            AgentViewModel.sideBarContentManagement(sideBarManagement);
            QuoteViewModel.sideBarContentManagement(sideBarManagement);
            QuoteViewModel.setSideBar(ref _commonCommandQuoteSideBarViewModel);
            ReferentialViewModel.sideBarContentManagement(sideBarManagement);

            //------[ Navigation ]
            ClientViewModel.mainNavigObject(navigation);
            ItemViewModel.mainNavigObject(navigation);
            CommandViewModel.mainNavigObject(navigation);
            AgentViewModel.mainNavigObject(navigation);
            QuoteViewModel.mainNavigObject(navigation);
            SecurityLoginViewModel.mainNavigObject(homeNavigation);
            _commonCommandQuoteSideBarViewModel.mainNavigObject(navigation);
            ReferentialViewModel.mainNavigObject(navigation);

            //------[ Cart ]
            _cart = new Cart();
            ItemViewModel.setInitCart(ref _cart);
            ClientViewModel.setInitCart(ref _cart);
            QuoteViewModel.setInitCart(ref _cart);
            CommandViewModel.CommandSideBarViewModel.setInitCart(ref _cart);

            //------[ Accessor ]
            ClientViewModel.setObjectAccessorFromMainWindowViewModel(getObject);
            CommandViewModel.setObjectAccessorFromMainWindowViewModel(getObject);
            QuoteViewModel.setObjectAccessorFromMainWindowViewModel(getObject);
            ItemViewModel.setObjectAccessorFromMainWindowViewModel(getObject);
            AgentViewModel.setObjectAccessorFromMainWindowViewModel(getObject);
            ReferentialViewModel.setObjectAccessorFromMainWindowViewModel(getObject);
            SecurityLoginViewModel.setObjectAccessorFromMainWindowViewModel(getObject);

            //------[ Header image ]
            //ReferentialViewModel.setHeaderImageManagement(headerImageManagement);
            ReferentialViewModel.setImageManagement(ImageManagement);
        }



        //----------------------------[ Properties ]------------------

        public string TxtUserName
        {
            get { return (AuthenticatedUser != null) ? AuthenticatedUser.FirstName + " " + AuthenticatedUser.LastName : ""; }
        }

        public Agent AuthenticatedUser
        {
            get { return _startup.Bl.BlSecurity.GetAuthenticatedUser(); }
        }

        public bool IsThroughContext
        {
            get { return _isThroughContext; }
            set { setProperty(ref _isThroughContext, value, "IsThroughContext"); }
        }

        /*public string NavigButtonStyle
        {
            get { return _navigButtonStyle; }
            set { setProperty(ref _navigButtonStyle, value, "NavigButtonStyle"); }
        }*/
        

        public Context Context
        {
            get { return _context; }
            set { setProperty(ref _context, value, "Context"); }
        }

        public DisplayAndData.Display.Image HeaderImageDisplay
        {
            get { return _headerImageDisplay; }
            set { setProperty(ref _headerImageDisplay, value, "HeaderImageDisplay"); }
        }

        public DisplayAndData.Display.Image LogoImageDisplay
        {
            get { return _logoImageDisplay; }
            set { setProperty(ref _logoImageDisplay, value, "LogoImageDisplay"); }
        }

        public DisplayAndData.Display.Image BillImageDisplay
        {
            get { return _billImageDisplay; }
            set { setProperty(ref _billImageDisplay, value, "BillImageDisplay"); }
        }

        public string SearchProgressVisibility
        {
            get { return _searchProgressVisibolity; }
            set { setProperty(ref _searchProgressVisibolity, value, "SearchProgressVisibility"); }
        }

        public Cart Cart
        {
            get { return _cart; }
        }

        public CommandSideBarViewModel CommandQuoteSideBar
        {
            get { return _commonCommandQuoteSideBarViewModel; }
            set { setProperty(ref _commonCommandQuoteSideBarViewModel, value, "_commonCommandQuoteSideBarViewModel"); }
        }

        public Object CurrentViewModel
        {
            get { return _currentViewModel; }
            set { setProperty(ref _currentViewModel, value, "CurrentViewModel"); }
        }

        public Object CurrentHomeViewModel
        {
            get { return _currentHomeViewModel; }
            set { setProperty(ref _currentHomeViewModel, value, "CurrentHomeViewModel"); }
        }

        public Object CurrentSideBarViewModel
        {
            get { return _currentSideBarViewModel; }
            set { setProperty(ref _currentSideBarViewModel, value, "CurrentSideBarViewModel"); }
        }

        public double ProgressBarPercentValue
        {
            get { return _progressBarPercentValue; }
            set { setProperty(ref _progressBarPercentValue, value, "ProgressBarPercentValue"); }
        }

        public string BlockSideBarVisibility
        {
            get { return _blockSideBarVisibility; }
            set { setProperty(ref _blockSideBarVisibility, value, "BlockSideBarVisibility"); }
        }

        public string BlockGridCentralVisibility
        {
            get { return _blockGridCentralVisibility; }
            set { setProperty(ref _blockGridCentralVisibility, value, "BlockGridCentralVisibility"); }
        }

        public string BlockHomeVisibility
        {
            get { return _blockHomeVisibility; }
            set { setProperty(ref _blockHomeVisibility, value, "BlockHomeVisibility"); }
        }
        
        //----------------------------[ Actions ]------------------
        
        private async void downloadHeaderImages()
        {
            //var allImages = await Startup.Bl.BlReferential.searchInfosFromWebService(new Infos { Option = 1 }, "AND"); // option = 1 ( = retrieving all images)
            var headerImageFoundDisplay = await loadImage(HeaderImageDisplay.TxtFileNameWithoutExtension, HeaderImageDisplay.TxtName);
            if (!string.IsNullOrEmpty(headerImageFoundDisplay.TxtFileFullPath) && File.Exists(headerImageFoundDisplay.TxtFileFullPath))
                HeaderImageDisplay = headerImageFoundDisplay;

            var logoImageFoundDisplay = await loadImage(LogoImageDisplay.TxtFileNameWithoutExtension, LogoImageDisplay.TxtName);
            if (!string.IsNullOrEmpty(logoImageFoundDisplay.TxtFileFullPath) && File.Exists(logoImageFoundDisplay.TxtFileFullPath))
                LogoImageDisplay = logoImageFoundDisplay;

            var billImageFoundDisplay = await loadImage(BillImageDisplay.TxtFileNameWithoutExtension, BillImageDisplay.TxtName);
            if (!string.IsNullOrEmpty(billImageFoundDisplay.TxtFileFullPath) && File.Exists(billImageFoundDisplay.TxtFileFullPath))
                BillImageDisplay = billImageFoundDisplay;

            //LogoImageDisplay = await loadImage("logo_image");
            //LogoImageDisplay.ImageSource = new System.Windows.Media.Imaging.BitmapImage();
        }

        private async Task<DisplayAndData.Display.Image> loadImage(string fileName, string imageName)
        {
            var imageDataList = new List<Infos>();
            var infosFoundImage = (await _startup.Bl.BlReferential.searchInfos(new QCBDManagementCommon.Entities.Infos { Name = fileName }, "AND")).FirstOrDefault();
            //var infosFoundImage = imageList.Where(x => x.Name.Equals(fileName)).FirstOrDefault();
            DisplayAndData.Display.Image imageObject = new DisplayAndData.Display.Image();

            if (infosFoundImage != null)
            {
                imageObject.TxtFileNameWithoutExtension = fileName;
                imageObject.TxtName = imageName;
                var infosWidthFound = (await _startup.Bl.BlReferential.searchInfos(new QCBDManagementCommon.Entities.Infos { Name = fileName + "_width" }, "AND")).FirstOrDefault();
                //var infosWidthFound = imageList.Where(x => x.Name.Equals(fileName + "_width")).FirstOrDefault();
                var infosHeightFound = (await _startup.Bl.BlReferential.searchInfos(new QCBDManagementCommon.Entities.Infos { Name = fileName + "_height" }, "AND")).FirstOrDefault();
                //var infosHeightFound = imageList.Where(x => x.Name.Equals(fileName + "_height")).FirstOrDefault();

                if (infosWidthFound != null)
                    imageDataList.Add(infosWidthFound);
                if (infosHeightFound != null)
                    imageDataList.Add(infosHeightFound);
                if (infosFoundImage != null)
                    imageDataList.Add(infosFoundImage);

                imageObject.ImageDataList = imageDataList;
            }
            return imageObject;
        }

        public Object navigation(Object centralPageContent = null)
        {            
            if (centralPageContent != null)
            {
                MainWindow.onUIThreadSync(() => {
                    Context.PreviousState = CurrentViewModel as IState;
                    CurrentViewModel = centralPageContent;
                    Context.NextState = centralPageContent as IState;
                });                
            }

            return CurrentViewModel;
        }

        public Object homeNavigation(Object homeContent = null)
        {
            if (homeContent != null)
            {
                MainWindow.onUIThreadSync(() => {
                    Context.PreviousState = CurrentViewModel as IState;
                    CurrentHomeViewModel = homeContent;
                    Context.NextState = homeContent as IState;
                });                
            }

            return CurrentViewModel;
        }

        public Object sideBarManagement(Object sideBarContent = null)
        {
            if (sideBarContent != null)
            {
                //MainWindow.onUIThreadSync(() => {
                    CurrentSideBarViewModel = sideBarContent;
                //});
                
            }
            return CurrentSideBarViewModel;
        }

        public double progressBarManagement(double status = 0)
        {
            if (status != 0)
            {
                //MainWindow.onUIThreadSync(() => {
                    ProgressBarPercentValue = status;
                    if (status != -1)
                        SearchProgressVisibility = "Hidden";
                //});                
            }
            return ProgressBarPercentValue;
        }

        public DisplayAndData.Display.Image ImageManagement(DisplayAndData.Display.Image newImage = null, string fileType = null)
        {
            if (fileType.ToUpper().Equals("HEADER"))
            {
                if (newImage != null)
                    MainWindow.onUIThreadSync(() => {
                        HeaderImageDisplay = newImage;
                    });
                
                return HeaderImageDisplay;
            }

            if (fileType.ToUpper().Equals("LOGO"))
            {
                if (newImage != null)
                    MainWindow.onUIThreadSync(() => {
                        LogoImageDisplay = newImage;
                    });                
                return LogoImageDisplay;
            }

            if (fileType.ToUpper().Equals("BILL"))
            {
                if (newImage != null)
                    MainWindow.onUIThreadSync(() => {
                        BillImageDisplay = newImage;
                    });                
                return BillImageDisplay;
            }

            return new DisplayAndData.Display.Image();
        }

        public object getObject(string objectName)
        {
            object ObjectToReturn = new object();
            switch (objectName.ToUpper())
            {
                case "CLIENT":
                    ObjectToReturn = ClientViewModel;
                    break;
                case "ITEM":
                    ObjectToReturn = ItemViewModel;
                    break;
                case "COMMAND":
                    ObjectToReturn = CommandViewModel;
                    break;
                case "QUOTE":
                    ObjectToReturn = QuoteViewModel;
                    break;
                case "AGENT":
                    ObjectToReturn = AgentViewModel;
                    break;
                case "REFERENTIAL":
                    ObjectToReturn = ReferentialViewModel;
                    break;
                case "SECURITY":
                    ObjectToReturn = SecurityLoginViewModel;
                    break;
                case "NOTIFICATION":
                    ObjectToReturn = NotificationViewModel;
                    break;
                case "HOME":
                    ObjectToReturn = HomeViewModel;
                    break;
                case "STATISTIC":
                    ObjectToReturn = StatisticViewModel;
                    break;
                case "MAIN":
                    ObjectToReturn = this;
                    break;
                case "CART":
                    ObjectToReturn = Cart;
                    break;
                case "CONTEXT":
                    ObjectToReturn = Context;
                break;
                case "WINDOW":
                    ObjectToReturn = MainWindow;
                    break;
            }

            return ObjectToReturn;
        }

        private void loadUIData()
        {
            //await MainWindow.onUIThreadAsync(() => {
                if (isNewAgentAuthentication)
                {
                    _startup.Dal.SetUserCredential(SecurityLoginViewModel.Bl.BlSecurity.GetAuthenticatedUser(), isNewAgentAuthentication);
                    isNewAgentAuthentication = false;
                }
                else
                {
                    _startup.Dal.ProgressBarFunc = progressBarManagement;
                    _startup.Dal.SetUserCredential(SecurityLoginViewModel.Bl.BlSecurity.GetAuthenticatedUser());
                    _startup.Dal.DALReferential.PropertyChanged += onLodingGeneralInfosDataFromWebServiceToLocalChange_loadHeaderImage;
                    //downloadHeaderImages();                    
                }
            CommandNavig.raiseCanExecuteActionChanged();
            onPropertyChange("TxtUserName");
            //}); 
        }
        
        /*private void setPageActive(object page)
        {
            string defaultStyle = "DynamicResource MaterialDesignFlatButton";
        }*/

        public void Dispose()
        {
            _startup.Dal.Dispose();
            GC.Collect();
        }

        //----------------------------[ Event Handler ]------------------

        private async void onAuthenticatedAgentChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Agent"))
            {
                await MainWindow.onUIThreadAsync(()=> {
                    loadUIData();
                });
                
            }
        }

        private async  void onLodingGeneralInfosDataFromWebServiceToLocalChange_loadHeaderImage(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsLodingDataFromWebServiceToLocal"))
            {
                await MainWindow.onUIThreadAsync(() => {
                    downloadHeaderImages();
                });               
            }
        }
        
        
        //----------------------------[ Action Commands ]------------------

        private async void appNavig(string propertyName)
        {
            await MainWindow.onUIThreadAsync(() => {
                IsThroughContext = false;
                BlockGridCentralVisibility = BlockSideBarVisibility = "Visible";
                BlockHomeVisibility = "Hidden";
                switch (propertyName)
                {
                    case "home":
                        //CurrentSideBarViewModel = null;
                        _currentViewModel = null;
                        BlockGridCentralVisibility = BlockSideBarVisibility = "Hidden";
                        BlockHomeVisibility = "Visible";
                        CurrentHomeViewModel = HomeViewModel;
                        break;
                    case "client":
                        ClientViewModel.executeNavig(propertyName);
                        break;
                    case "item":
                        ItemViewModel.executeNavig(propertyName);
                        break;
                    case "command":
                        CommandViewModel.executeNavig(propertyName);
                        break;
                    case "quote":
                        //CurrentSideBarViewModel = CommandViewModel.CommandSideBarViewModel;
                        QuoteViewModel.executeNavig(propertyName);
                        break;
                    case "agent":
                        AgentViewModel.executeNavig(propertyName);
                        break;
                    case "notification":
                        CurrentSideBarViewModel = NotificationViewModel.NotificationSideBarViewModel;
                        CurrentViewModel = NotificationViewModel;
                        break;
                    case "notification-new":
                        CurrentSideBarViewModel = NotificationViewModel.NotificationSideBarViewModel;
                        CurrentViewModel = NotificationViewModel;
                        break;
                    case "notification-detail":
                        CurrentSideBarViewModel = NotificationViewModel.NotificationSideBarViewModel;
                        CurrentViewModel = NotificationViewModel;
                        break;
                    case "option":
                        ReferentialViewModel.executeNavig(propertyName);
                        break;
                    case "statistic":
                        CurrentSideBarViewModel = null;
                        CurrentViewModel = StatisticViewModel;
                        break;
                    case "back":
                        Context.Request();
                        IsThroughContext = true;
                        //InputDialog.show("Going back!");
                        break;
                }
            });            
        }

        private bool canAppNavig(string arg)
        {
            if (_startup == null)
                return false;
            if (AuthenticatedUser == null || AuthenticatedUser.Status == EStatus.Deactivated.ToString())
                return false;
            if (arg.Equals("client"))
                return securityCheck(QCBDManagementCommon.Enum.EAction.Client, QCBDManagementCommon.Enum.ESecurity._Read);
            if (arg.Equals("item"))
                return securityCheck(QCBDManagementCommon.Enum.EAction.Item, QCBDManagementCommon.Enum.ESecurity._Read);
            if (arg.Equals("agent"))
                return securityCheck(QCBDManagementCommon.Enum.EAction.Agent, QCBDManagementCommon.Enum.ESecurity._Read);
            if (arg.Equals("notification"))
                return false;// securityCheck(QCBDManagementCommon.Enum.EAction.Notification, QCBDManagementCommon.Enum.ESecurity._Read);
            if (arg.Equals("quote"))
                return securityCheck(QCBDManagementCommon.Enum.EAction.Quote, QCBDManagementCommon.Enum.ESecurity._Read);
            if (arg.Equals("command"))
                return securityCheck(QCBDManagementCommon.Enum.EAction.Command, QCBDManagementCommon.Enum.ESecurity._Read);
            if (arg.Equals("statistic"))
                return false;// securityCheck(QCBDManagementCommon.Enum.EAction.Statistic, QCBDManagementCommon.Enum.ESecurity._Read);
            if (arg.Equals("option"))
                return securityCheck(QCBDManagementCommon.Enum.EAction.Option, QCBDManagementCommon.Enum.ESecurity._Read);
            if (arg.Equals("home") || arg.Equals("back"))
                return true;

            return false;
        }
    }
}
