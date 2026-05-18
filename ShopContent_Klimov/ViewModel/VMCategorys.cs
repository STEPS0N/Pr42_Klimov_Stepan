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
    public class VMCategorys : INotifyPropertyChanged
    {
        public ObservableCollection<CategorysContext> Categorys { get; set; }

        public VMCategorys()
        {
            Categorys = CategorysContext.AllCategorys();
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
