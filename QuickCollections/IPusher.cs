using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickCollections
{
    public interface  IPusher<T>
    {
         event EventHandler<Node<T>> Pushed;

    }
}
