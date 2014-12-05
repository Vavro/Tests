using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Lucene.Net.Analysis;
using LuceneNetCzechSupport.Analyzers;
using LuceneNetCzechSupport.WpfClient.Helpers;

namespace LuceneNetCzechSupport.WpfClient
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            var supportedAnalyzers = new Analyzer[] { 
                SupportedAnalyzers.CzechAnalyzer, 
                SupportedAnalyzers.CzechLightSnowballAnalyzer, 
                SupportedAnalyzers.CzechAggresiveSnowballAnalyzer, 
                SupportedAnalyzers.CzechHunspellAnalyzer };

            AwailibleAnalyzers = supportedAnalyzers.Select(a => new AnalyzerViewModel(a)).ToList();

            OpenFileCommand = new DelegateCommand
        }

        public string FileName
        {
            get;
            set;
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
            get;
            set;
        }

        public ICommand IndexSelectedDocumentsCommand { get; set; }

        public AnalyzerViewModel SelectedAnalyzer { get; set; }

        public List<AnalyzerViewModel> AwailibleAnalyzers { get; set; }
    }

    public class AnalyzerViewModel
    {
        public AnalyzerViewModel(Analyzer analyzer)
        {
            Analyzer = analyzer;
        }

        public Analyzer Analyzer { get; set; }
        public string Name { get { return Analyzer != null ? Analyzer.ToString() : null; } }
    }
}
