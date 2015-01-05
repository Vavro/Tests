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
            //var lines = SourceLinks.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            var lines = new List<string>();
            lines.Add("test1.txt");
            lines.Add("test2.txt");

            var client = new WebClient();


            foreach (var line in lines)
            {
                CurrentItem = 0;
                CurrentLink = line;
                var doc = new HtmlDocument();
                doc.Load(line);

                var imageLinks = doc.DocumentNode.SelectNodes("//a[@href][@class='photoImage']").Select(a => a.Attributes["href"].Value).ToList();

                CurrentLinkTotalItems = imageLinks.Count;

                foreach (var imageLink in imageLinks)
                {
                    client.DownloadFile(imageLink, Path.Combine(TargetDirectory, Path.GetFileName(imageLink)));

                    CurrentItem++;
                }
            }

            CurrentLink = "Complete";
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
