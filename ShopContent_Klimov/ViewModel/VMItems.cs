using ShopContent_Klimov.Classes;
using ShopContent_Klimov.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShopContent_Klimov.ViewModel
{
    public class VMItems : INotifyPropertyChanged
    {
        public ObservableCollection<ItemsContext> Items { get; set; }

        public RelayCommand NewItem
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ItemsContext newModel = new ItemsContext(true);
                    Items.Add(newModel);
                    MainWindow.init.frame.Navigate(new View.Add(newModel));
                });
            }
        }

        public VMItems()
        {
            Items = ItemsContext.AllItems();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
