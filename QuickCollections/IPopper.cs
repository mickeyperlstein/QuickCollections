using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickCollections
{
    public interface  IPopper<T>
    {
         event EventHandler<Node<T>> Popped;

    }
}
