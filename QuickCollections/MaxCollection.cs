using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickCollections
{
    public abstract class MaxCollection<T>
    {
        protected Node<T> head;  // head of list
        StringBuilder sb = new StringBuilder();
        public override string ToString()
        {
            sb.Clear();
            if (head == null)
                sb.AppendFormat("EMPTY");
            else
            {
                sb.AppendFormat("MAX:{0}", head.data);
                var t = head.next;
                while (t != null)
                {
                    sb.AppendFormat("=>{0}", t.data);
                    t = t.next;

                }
            }
            return sb.ToString();

        }
    }
}
