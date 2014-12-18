using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Lucene.Net.Analysis;
using LuceneNetCzechSupport.Analyzers;
using LuceneNetCzechSupport.Lucene;
using LuceneNetCzechSupport.WpfClient.Helpers;
using LuceneNetCzechSupport.WpfClient.IFilter;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace LuceneNetCzechSupport.WpfClient.ViewModel
{
    //todo: add indexed terms
    //todo: add compare between all analyzers
    //todo: add search in all analyzers
    public class MainWindowViewModel : ViewModelBase
    {
        private string _fileName;
        private string _searchResultCzech;
        private string _searchResultCzechAggressiveSnowball;
        private string _searchResultCzechLightSnowball;
        private string _searchResultCzechHunspell;

        public MainWindowViewModel()
        {
            SupportedCzechIndexes = new SupportedCzechIndexes();

            OpenFileCommand = new RelayCommand(OpenFileExecute);
            OpenDirectoryCommand = new RelayCommand(OpenDirectoryExecute);
            IndexSelectedDocumentsCommand = new RelayCommand(IndexSelectedDocumentsExecute, IndexSelectedDocumentsCanExecute);

            SearchCommand = new RelayCommand(SearchExecute);

            ClearIndexedDocumentsCommand = new RelayCommand(ClearIndexedDocumentsExecute);
        }

        private void ClearIndexedDocumentsExecute()
        {
            SupportedCzechIndexes.ClearIndexes();
        }

        private void SearchExecute()
        {
            SearchOn(SupportedCzechIndexes.CzechFulltext, result => SearchResultCzech = result);
            SearchOn(SupportedCzechIndexes.CzechAggressiveSnowballFulltext, result => SearchResultCzechAggressiveSnowball = result);
            SearchOn(SupportedCzechIndexes.CzechLightSnowballFulltext, result => SearchResultCzechLightSnowball = result);
            SearchOn(SupportedCzechIndexes.CzechHunspellFulltext, result => SearchResultCzechHunspell = result);
        }

        private void SearchOn(FulltextViewModel fulltext, Action<string> writeOutputFunc)
        {
            var results = fulltext.Fulltext.SearchIndex(SearchText);
         
            fulltext.Statistics.LastSearchTime = fulltext.Fulltext.Stats.LastSearchTime;

            var searchResult = new StringBuilder();
            foreach (var result in results)
            {
                searchResult.Append("id: ");
                searchResult.Append(result);
                searchResult.AppendLine();
            }

            writeOutputFunc(searchResult.ToString());
        }

        private bool IndexSelectedDocumentsCanExecute()
        {
            return true;
        }

        private void IndexSelectedDocumentsExecute()
        {
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
            try
            {
                using (var fileStream = new FileStream(file.FullName, FileMode.Open))
                {
                    var fileText = ParseHelper.ParseIFilter(fileStream, file.FullName);

                    SupportedCzechIndexes.AddDocToFulltext(new FullTextDocument()
                                              {
                                                  FileFulltextInfo =
                                                  {
                                                      FileName = file.FullName,
                                                      FileText = fileText
                                                  },
                                                  Id = file.FullName
                                              });
                }
            }
            catch (Exception e)
            {
                //var dialogService = SimpleIoc.Default.GetInstance<IDialogService>();
                //dialogService.ShowError(e, String.Format("Couldn't index file {0}", file.FullName), "OK", null);
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
        
        public RelayCommand IndexSelectedDocumentsCommand { get; set; }

        public SupportedCzechIndexes SupportedCzechIndexes { get; private set; }

        public string SearchResultCzech
        {
            get { return _searchResultCzech; }
            set
            {
                _searchResultCzech = value;
                RaisePropertyChanged(() => SearchResultCzech);
            }
        }

        public string SearchResultCzechAggressiveSnowball
        {
            get { return _searchResultCzechAggressiveSnowball; }
            set
            {
                _searchResultCzechAggressiveSnowball = value;
                RaisePropertyChanged(() => SearchResultCzechAggressiveSnowball);
            }
        }

        public string SearchResultCzechLightSnowball
        {
            get { return _searchResultCzechLightSnowball; }
            set
            {
                _searchResultCzechLightSnowball = value;
                RaisePropertyChanged(() => SearchResultCzechLightSnowball);
            }
        }

        public string SearchResultCzechHunspell
        {
            get { return _searchResultCzechHunspell; }
            set
            {
                _searchResultCzechHunspell = value;
                RaisePropertyChanged(() => SearchResultCzechHunspell);
            }
        }
    }

    public class Statistics : ObservableObject
    {
        private TimeSpan _lastIndexingTime;
        private TimeSpan _lastSearchTime;

        public TimeSpan LastIndexingTime
        {
            get { return _lastIndexingTime; }
            set { _lastIndexingTime = value; RaisePropertyChanged(() => LastIndexingTime);}
        }

        public TimeSpan LastSearchTime
        {
            get { return _lastSearchTime; }
            set { _lastSearchTime = value; RaisePropertyChanged(() => LastSearchTime);}
        }
    }

    public class SupportedCzechIndexes
    {
        public SupportedCzechIndexes()
        {
            var czechFulltext = new FulltextViewModel(new Fulltext(SupportedAnalyzers.CzechAnalyzer));
            var czechAggressiveSnowballFulltext = new FulltextViewModel(new Fulltext(SupportedAnalyzers.CzechAggresiveSnowballAnalyzer));
            var czechLightSnowballFulltext = new FulltextViewModel(new Fulltext(SupportedAnalyzers.CzechLightSnowballAnalyzer));
            var czechHunspellFulltext = new FulltextViewModel(new Fulltext(SupportedAnalyzers.CzechHunspellAnalyzer));

            _supportedFulltexts = new List<FulltextViewModel>() { czechFulltext, czechAggressiveSnowballFulltext, czechLightSnowballFulltext, czechHunspellFulltext };
        }

        private readonly List<FulltextViewModel> _supportedFulltexts;

        public FulltextViewModel CzechFulltext { get { return _supportedFulltexts[0]; } }
        public FulltextViewModel CzechAggressiveSnowballFulltext { get { return _supportedFulltexts[1]; } }
        public FulltextViewModel CzechLightSnowballFulltext { get { return _supportedFulltexts[2]; } }
        public FulltextViewModel CzechHunspellFulltext { get { return _supportedFulltexts[3]; } }
        public IReadOnlyList<FulltextViewModel> SupportedFulltexts { get { return _supportedFulltexts.AsReadOnly(); } }

        public void AddDocToFulltext(FullTextDocument insertedDocument)
        {
            _supportedFulltexts.ForEach(fulltext =>
                                        {
                                            try
                                            {
                                                fulltext.Fulltext.AddDocToFulltext(insertedDocument);
                                                fulltext.Statistics.LastIndexingTime = fulltext.Fulltext.Stats.LastIndexingTime;
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e);
                                            }
                                        });
        }

        public void ClearIndexes()
        {
            _supportedFulltexts.ForEach(fulltext => fulltext.Fulltext.Clear());
        }
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

    public class FulltextViewModel
    {
        public FulltextViewModel(Fulltext fulltext)
        {
            Fulltext = fulltext;
            Statistics = new Statistics();
        }

        public Fulltext Fulltext { get; private set; }
        public Statistics Statistics
        {
            get; private set; }
    }
}
