using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HtmlAgilityPack;

namespace SpoluzaciPhotoGrabber
{
    public class MainWindowViewModel : ObservableObject
    {
        private string _currentLink;
        private string _status;
        private int _currentLinkTotalItems;
        private int _currentItem;

        public MainWindowViewModel()
        {
#if DEBUG
            TargetDirectory = @"d:\SpoluzaciObrazky\";
#endif

            CurrentItem = 0;
            CurrentLinkTotalItems = 0;
            CurrentLink = "Not running";

            GrabImagesCommand = new RelayCommand(GrabImageCommandExecute);
        }

        protected override void RaisePropertyChanged(string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);

            var handler = this.PropertyChangedHandler;
            switch (propertyName)
            {
                case "CurrentLink":
                case "CurrentLinkTotalItems":
                case "CurrentItem":
                    if (handler != null) handler(this, new PropertyChangedEventArgs("Status"));
                    break;
            }
        }


        //todo: move to background thread
        private void GrabImageCommandExecute()
        {
            Task.Run(() =>
            {
                //var lines = SourceLinks.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

                var lines = new List<Tuple<string, string>>();
                lines.Add(new Tuple<string, string>("test1.txt", "Konryho narozky 07"));
                lines.Add(new Tuple<string, string>("test2.txt", "Maturak 07"));
                lines.Add(new Tuple<string, string>("test3.txt", "Voda se Sýsou 06"));
                lines.Add(new Tuple<string, string>("test4.txt", "Voda se třídou 06"));
                lines.Add(new Tuple<string, string>("test5.txt", "Vochyho 19 07"));
                lines.Add(new Tuple<string, string>("test6.txt", "Weidenberg 11 06"));


                var client = new WebClient();

                foreach (var line in lines)
                {
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        CurrentItem = 0;
                        CurrentLink = line.Item1;
                    });

                    var doc = new HtmlDocument();
                    doc.Load(CurrentLink);

                    var imageLinks = doc.DocumentNode.SelectNodes("//a[@href][@class='photoImage']").Select(a => a.Attributes["href"].Value).ToList();

                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        CurrentLinkTotalItems = imageLinks.Count;
                    });

                    var path = Path.Combine(TargetDirectory, line.Item2);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    foreach (var imageLink in imageLinks)
                    {
                        client.DownloadFile(imageLink, Path.Combine(path, Path.GetFileName(imageLink)));

                        Dispatcher.CurrentDispatcher.Invoke(() => { CurrentItem++; });
                    }
                }

                CurrentLink = "Complete";
            });
        }

        public string TargetDirectory { get; set; }

        public string SourceLinks { get; set; }

        public RelayCommand GrabImagesCommand { get; private set; }

        public string CurrentLink
        {
            get { return _currentLink; }
            set { _currentLink = value; RaisePropertyChanged(() => CurrentLink); }
        }

        public int CurrentLinkTotalItems
        {
            get { return _currentLinkTotalItems; }
            set { _currentLinkTotalItems = value; RaisePropertyChanged(() => CurrentLinkTotalItems); }
        }

        public int CurrentItem
        {
            get { return _currentItem; }
            set { _currentItem = value; RaisePropertyChanged(() => CurrentItem); }
        }

        public string Status
        {
            get { return String.Format("{0} {1}/{2}", CurrentLink, CurrentItem, CurrentLinkTotalItems); }
        }
    }

}
