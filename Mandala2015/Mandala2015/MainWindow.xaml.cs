using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mandala2015
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IEnumerable<Point> drawing;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public IEnumerable<Point> Preview
        {
            set
            {
                if (value != null)
                {
                    var start = value.First();

                    if (LikeRound(start))
                    {
                        var list = value.Select(p => new Point(Math.Round(p.X), Math.Round(p.Y))).ToList();
                        list.AddRange(value.Select(p => new Point(Math.Round(p.Y), Math.Round(p.X))));
                        Drawing = list;
                    }
                    else
                    {
                        Drawing1 = value;
                    }
                }
                else
                {
                    Drawing = null;
                    Drawing1 = null;
                }
            }
        }

        public IEnumerable<Point> Preview1
        {
            set
            {
                if (value != null)
                {
                    var start = value.First();
                    if (LikeRound(start))
                    {
                        return;
                    }
                }
                Drawing1 = value;
            }
        }

        private bool LikeRound(Point start)
        {
            return Math.Abs(start.X - Math.Round(start.X)) < 0.2 && Math.Abs(start.Y - Math.Round(start.Y)) < 0.2;
        }

        public IEnumerable<Point> Drawing
        {
            get
            {
                return drawing;
            }

            private set
            {
                drawing = value;
                OnPropertyChanged("Drawing");
            }
        }
        public IEnumerable<Point> Drawing1
        {
            get
            {
                return drawing;
            }

            private set
            {
                drawing = value;
                OnPropertyChanged("Drawing1");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
