using Mandala2015.Controls;
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
    internal partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IEnumerable<Edge> drawing;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public Edge? Preview
        {
            set
            {
                if (value.HasValue)
                {
                    if (LikeRound(value.Value.Start))
                    {
						var list = new[] {
							new Edge(Math.Round(value.Value.Start.X), Math.Round(value.Value.Start.Y), Math.Round(value.Value.End.X), Math.Round(value.Value.End.Y)),
							new Edge(Math.Round(value.Value.Start.Y), Math.Round(value.Value.Start.X), Math.Round(value.Value.End.Y), Math.Round(value.Value.End.X)),
                            };
                        Drawing = list;
                    }
                    else
                    {
                        Drawing1 = new[] { value.Value };
                    }
                }
                else
                {
                    Drawing = null;
                    Drawing1 = null;
                }
            }
        }

        private bool LikeRound(Point start)
        {
            return Math.Abs(start.X - Math.Round(start.X)) < 0.2 && Math.Abs(start.Y - Math.Round(start.Y)) < 0.2;
        }

        public IEnumerable<Edge> Drawing
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
        public IEnumerable<Edge> Drawing1
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
