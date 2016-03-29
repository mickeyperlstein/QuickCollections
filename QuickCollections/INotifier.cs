using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickCollections
{
    public interface  INotifier<T> : IPusher<T>, IPopper<T>
    {
    }
}
