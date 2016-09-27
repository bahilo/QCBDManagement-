using Microsoft.Win32;
using QCBDManagementBusiness;
using QCBDManagementCommon.Classes;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QCBDManagementCommon.Entities;
using System.Collections.ObjectModel;
using System.Globalization;
using QCBDManagementWPF.Models;
using System.Threading;
using System.IO;
using System.Xml.Serialization;

namespace QCBDManagementWPF.ViewModel
{
    public class OptionDataAndDisplayViewModel : BindBase
    {
        private ObservableCollection<DisplayAndData.Display.Image> _imageList;
        private List<int> _imageWidthSizeList;
        private List<int> _imageHeightSizeList;
        private Func<DisplayAndData.Display.Image, string, DisplayAndData.Display.Image> _imageManagement;        
        private List<DisplayAndData.Data> _dataList;
        private CultureInfo[] _cultureInfoArray;
        //private bool _isUserAdmin;
        private string _title;

        //----------------------------[ Models ]------------------

        //----------------------------[ Commands ]------------------

        public ButtonCommand<DisplayAndData.Display.Image> OpenFileExplorerCommand { get; set; }
        public ButtonCommand<DisplayAndData.Display.Image> DeleteImageCommand { get; set; }
        public ButtonCommand<string> UpdateLanguageCommand { get; set; }
        public ButtonCommand<string> AddNewRowLanguageCommand { get; set; }

        public OptionDataAndDisplayViewModel() : base()
        {
            instances();
            instancesPoco();
            instancesModel();
            instancesCommand();
            initEvents();
        }

        private void initEvents()
        {

        }

        private void instances()
        {
            _title = "Data/Display Management";
            _imageList = new ObservableCollection<DisplayAndData.Display.Image>();
            _imageWidthSizeList = new List<int>();
            _imageHeightSizeList = new List<int>();
            _cultureInfoArray = CultureInfo.GetCultures(CultureTypes.AllCultures & CultureTypes.NeutralCultures);
            
        }

        private void instancesPoco()
        {

        }

        private void instancesModel()
        {

        }

        private void instancesCommand()
        {
            OpenFileExplorerCommand = new ButtonCommand<DisplayAndData.Display.Image>(getFileFromLocal, canGetFileFromLocal);
            DeleteImageCommand = new ButtonCommand<DisplayAndData.Display.Image>(deleteImage, canDeleteImage);
        }

        //----------------------------[ Properties ]------------------


        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange( "Bl"); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public List<int> ImageWidthSizeList
        {
            get { return _imageWidthSizeList; }
            set { setProperty(ref _imageWidthSizeList, value, "ImageWidthSizeList"); }
        }

        public List<int> ImageHeightSizeList
        {
            get { return _imageHeightSizeList; }
            set { setProperty(ref _imageHeightSizeList, value, "ImageHeightSizeList"); }
        }

        public ObservableCollection<DisplayAndData.Display.Image> ImageList
        {
            get { return _imageList; }
            set { setProperty(ref _imageList, value, "ImageList"); }
        }

        public CultureInfo[] CultureInfoArray
        {
            get { return _cultureInfoArray; }
            set { setProperty(ref _cultureInfoArray, value, "CultureInfoArray"); }
        }

        //----------------------------[ Display by LanguageModel ]------------------

        public List<DisplayAndData.Data> DataList
        {
            get { return _dataList; }
            set { setProperty(ref _dataList, value, "DataList"); }
        }

        
        public object PdfInvoiceLanguage { get; private set; }
        public object PdfQuoteLanguage { get; private set; }
        public object PdfDeliveryLanguage { get; private set; }


        //----------------------------[ Actions ]------------------

        public List<LanguageModel> LanguageListToLanguageModelList(List<Language> languageList)
        {
            List<LanguageModel> output = new List<LanguageModel>();
            
            foreach (Language language in languageList)
            {
                LanguageModel languageModel = new LanguageModel();
                languageModel.Language = language;

                output.Add(languageModel);
            }

            return output;
        }
        public void loadData()
        {
            loadImages();
        }
              
       
        private void loadImages()
        {
            Dialog.showSearch("Loading...");
            Dispose();
            ImageList.Clear();

            //----[ Bill Image ]
            DisplayAndData.Display.Image displayBillImage = _imageManagement(null, "bill"); // get Logo image created by MainWindowViewModel for updating
            displayBillImage.PropertyChanged += onFilePathChange_updateUIImage;
            displayBillImage.PropertyChanged += onWidthChange_saveImageWidth;
            displayBillImage.PropertyChanged += onHeightChange_saveImageHeight;
            ImageList.Add(displayBillImage);

            //----[ Logo Image ]
            DisplayAndData.Display.Image displayLogoImage = _imageManagement(null, "logo"); // get Logo image created by MainWindowViewModel for displaying in the UI Header
            displayLogoImage.PropertyChanged += onFilePathChange_updateUIImage;
            displayLogoImage.PropertyChanged += onWidthChange_saveImageWidth;
            displayLogoImage.PropertyChanged += onHeightChange_saveImageHeight;
            ImageList.Add(displayLogoImage);

            //----[ Header Image ] 
            DisplayAndData.Display.Image displayHeaderImage = _imageManagement(null, "header"); // get Header image created by MainWindowViewModel for displaying in the UI Header
            displayHeaderImage.PropertyChanged += onFilePathChange_updateUIImage;
            displayHeaderImage.PropertyChanged += onWidthChange_saveImageWidth;
            displayHeaderImage.PropertyChanged += onHeightChange_saveImageHeight;
            ImageList.Add(displayHeaderImage);

            var widthSizeList = new List<int>();
            ImageWidthSizeList.Clear();
            for (int i = 25; i <= 800; i = i + 25)
            {
                widthSizeList.Add(i);
            }
            ImageWidthSizeList = widthSizeList;

            var heightSizeList = new List<int>();
            ImageHeightSizeList.Clear();
            for (int i = 5; i <= 300; i = i + 5)
            {
                heightSizeList.Add(i);
            }
            ImageHeightSizeList = heightSizeList;
            Dialog.IsDialogOpen = false;
        }

        public string ExecuteOpenFileDialog()
        {
            string outputFile = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
                outputFile = openFileDialog.FileName;

            return outputFile;
        }

        internal void setImageManagement(Func<DisplayAndData.Display.Image, string, DisplayAndData.Display.Image> imageManagement)
        {
            _imageManagement = imageManagement;
        }

        public override void Dispose()
        {
            foreach (var image in ImageList)
            {
                image.PropertyChanged -= onFilePathChange_updateUIImage;
                image.PropertyChanged -= onWidthChange_saveImageWidth;
                image.PropertyChanged -= onHeightChange_saveImageHeight;
            }
        }

        //----------------------------[ Event Handler ]------------------
        
        private async void onHeightChange_saveImageHeight(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtHeight"))
            {
                var infosHeightSavedList = await Bl.BlReferential.UpdateInfos(new List<Infos> { ((DisplayAndData.Display.Image)sender).ImageHeight });
            }
        }

        private async void onWidthChange_saveImageWidth(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtWidth"))
            {
                var infosHeightSavedList = await Bl.BlReferential.UpdateInfos(new List<Infos> { ((DisplayAndData.Display.Image)sender).ImageWidth });
            }
        }

        private void onFilePathChange_updateUIImage(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtFileFullPath") && !string.IsNullOrEmpty(((DisplayAndData.Display.Image)sender).TxtFileFullPath) )
            {
                if (((DisplayAndData.Display.Image)sender).TxtFileNameWithoutExtension.Equals("header_image"))
                {
                    _imageManagement((DisplayAndData.Display.Image)sender, "header");
                }
                else if(((DisplayAndData.Display.Image)sender).TxtFileNameWithoutExtension.Equals("logo_image"))
                {
                    _imageManagement((DisplayAndData.Display.Image)sender, "logo");
                }
                else if (((DisplayAndData.Display.Image)sender).TxtFileNameWithoutExtension.Equals("bill_image"))
                {
                    _imageManagement((DisplayAndData.Display.Image)sender, "bill");
                }                   
            }
            
        }

        //----------------------------[ Action Commands ]------------------



        private async void getFileFromLocal(DisplayAndData.Display.Image obj)
        {
            obj.TxtChosenFile = ExecuteOpenFileDialog();
            Dialog.showSearch("File saving...");
            obj.save();
            List<Infos> savedInfosList = new List<Infos>();
            //if (obj.ImageDataList.Count > 0)
            //{
                var infosFoundList = await Bl.BlReferential.searchInfos(new Infos { Name = obj.TxtFileNameWithoutExtension }, "AND");
                if (infosFoundList.Count > 0)
                {
                    savedInfosList = await Bl.BlReferential.UpdateInfos(obj.ImageDataList);
                if (savedInfosList.Count > 0)
                    await Dialog.show("Image updated successfully!");
                }
                else
                {
                    savedInfosList = await Bl.BlReferential.InsertInfos(obj.ImageDataList);
                    if (savedInfosList.Count > 0)
                        await Dialog.show("Image saved successfully!");
                }
            //}   
            Dialog.IsDialogOpen = false;         

        }

        private bool canGetFileFromLocal(DisplayAndData.Display.Image arg)
        {
            return true;
        }

        private async void deleteImage(DisplayAndData.Display.Image obj)
        {
            Dialog.showSearch("Image deleting...");
            var notDeletedInfosList = await Bl.BlReferential.DeleteInfos(obj.ImageDataList);
            var whereImageInfosIDIsZeroList = obj.ImageDataList.Where(x=>x.ID == 0 && x.Name.Equals(obj.TxtFileNameWithoutExtension)).ToList();
            if ((notDeletedInfosList.Count == 0 || whereImageInfosIDIsZeroList.Count > 0) && obj.deleteFiles())
            {
                await Dialog.show(obj.TxtFileName + " has been successfully deteleted!");
                obj.TxtFileFullPath = "";
            }
            Dialog.IsDialogOpen = false;
        }

        private bool canDeleteImage(DisplayAndData.Display.Image arg)
        {
            return true;
        }    


    }
}
