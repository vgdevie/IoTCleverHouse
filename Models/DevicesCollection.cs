using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICH
{
    public class DevicesCollection<T> : ObservableCollection<T>
    {
        public T this[string name]
        {
            get
            {
                return base.Items.Where(e => (e as IDevice).Name == name).Single();
            }
            set
            {
                base.Items.Where(e => (e as IDevice).Name == name).All(g => { g = value; return true; });
            }
        }
    }
}
