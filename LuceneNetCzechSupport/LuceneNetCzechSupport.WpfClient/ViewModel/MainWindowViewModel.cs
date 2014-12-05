using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using IFilterReader;
using Lucene.Net.Analysis;
using LuceneNetCzechSupport.Analyzers;
using LuceneNetCzechSupport.Lucene;
using LuceneNetCzechSupport.WpfClient.Helpers;
using LuceneNetCzechSupport.WpfClient.IFilter;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace LuceneNetCzechSupport.WpfClient.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _fileName;
        private AnalyzerViewModel _selectedAnalyzer;
        private string _searchResult;

        public MainWindowViewModel()
        {
            var supportedAnalyzers = new Analyzer[] { 
                SupportedAnalyzers.CzechAnalyzer, 
                SupportedAnalyzers.CzechLightSnowballAnalyzer, 
                SupportedAnalyzers.CzechAggresiveSnowballAnalyzer, 
                SupportedAnalyzers.CzechHunspellAnalyzer };

            AwailibleAnalyzers = supportedAnalyzers.Select(a => new AnalyzerViewModel(a)).ToList();

            OpenFileCommand = new RelayCommand(OpenFileExecute);
            OpenDirectoryCommand = new RelayCommand(OpenDirectoryExecute);
            IndexSelectedDocumentsCommand = new RelayCommand(IndexSelectedDocumentsExecute, IndexSelectedDocumentsCanExecute);

            SearchCommand = new RelayCommand(SearchExecute);
        }

        private void SearchExecute()
        {
            var results = Fulltext.SearchIndex(SearchText);

            var searchResult = new StringBuilder();
            foreach (var result in results)
            {
                searchResult.Append("id: ");
                searchResult.Append(result);
                searchResult.AppendLine();
            }
            SearchResult = searchResult.ToString();
        }

        private bool IndexSelectedDocumentsCanExecute()
        {
            return SelectedAnalyzer != null;
        }

        private void IndexSelectedDocumentsExecute()
        {
            if (SelectedAnalyzer == null)
                return;

            if (FileHelpers.IsDirectory(FileName))
            {
                var directory = new DirectoryInfo(FileName);
                var files = directory.GetFiles();
                foreach (var fileInfo in files)
                {
                    AddToIndex(fileInfo);
                }
            }
            else
            {
                var file = new FileInfo(FileName);
                AddToIndex(file);
            }
        }

        private void AddToIndex(FileInfo file)
        {
            using (var fileStream = new FileStream(file.FullName, FileMode.Open))
            {
                if (false)
                {
                    var dialogService = SimpleIoc.Default.GetInstance<IDialogService>();
                    dialogService.ShowMessage(
                        String.Format("Could find a suitable IFilter for file {0}", file.FullName),
                        "Could not open file");
                    return;
                }

                var fileText = ParseHelper.ParseIFilter(fileStream, file.FullName);

                Fulltext.AddDocToFulltext(new FullTextDocument()
                                          {
                                              FileFulltextInfo =
                                              {
                                                  FileName = file.FullName, FileText = fileText
                                              }, 
                                              Id = file.FullName
                                          });
            }
        }

        private void OpenDirectoryExecute()
        {
            var openDirectoryDialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = openDirectoryDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                FileName = openDirectoryDialog.SelectedPath;
            }
        }

        private void OpenFileExecute()
        {
            var openFileDialog = new OpenFileDialog()
                                 {
                                     Multiselect = false,
                                 };

            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                FileName = openFileDialog.FileName;
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; RaisePropertyChanged(() => FileName); }
        }

        public ICommand OpenFileCommand
        {
            get;
            set;
        }

        public ICommand OpenDirectoryCommand
        {
            get;
            set;
        }

        public ICommand ClearIndexedDocumentsCommand
        {
            get;
            set;
        }

        public string SearchText
        {
            get;
            set;
        }

        public ICommand SearchCommand
        {
            get;
            set;
        }

        public string SearchResult
        {
            get { return _searchResult; }
            set
            {
                _searchResult = value; 
                RaisePropertyChanged(() => SearchResult);
            }
        }

        public RelayCommand IndexSelectedDocumentsCommand { get; set; }

        public AnalyzerViewModel SelectedAnalyzer
        {
            get { return _selectedAnalyzer; }
            set
            {
                //todo: add notification that current fulltext index get discarded
                _selectedAnalyzer = value;
                RaisePropertyChanged(() => SelectedAnalyzer);
                InitFulltext();
                IndexSelectedDocumentsCommand.RaiseCanExecuteChanged();
            }
        }

        private void InitFulltext()
        {
            if (SelectedAnalyzer != null)
            {
                Fulltext = new Fulltext(SelectedAnalyzer.Analyzer);
            }
        }

        public Fulltext Fulltext { get; set; }

        public List<AnalyzerViewModel> AwailibleAnalyzers { get; set; }
    }

    public class AnalyzerViewModel
    {
        public AnalyzerViewModel(Analyzer analyzer)
        {
            Analyzer = analyzer;
        }

        public Analyzer Analyzer { get; set; }

        public string Name
        {
            get
            {
                return Analyzer != null ? Analyzer.ToString() : null;
            }
        }
    }
}
