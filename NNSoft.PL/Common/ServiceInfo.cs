using System.ComponentModel;

namespace NNSoft.PL.Common
{
    public class ServiceInfo : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Group { get; set; }

        int id;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                NotifyPropertyChanged(nameof(Id));
            }
        }

        ServiceState state;
        public ServiceState State
        {
            get => state;
            set
            {
                state = value;
                NotifyPropertyChanged(nameof(State));
            }
        }

        public string Path { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
