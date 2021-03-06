﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickCollections
{
    public class Node<T> 
    {

        internal Node<T> next;
        internal T data;
        StringBuilder sb = new StringBuilder();
        public override string ToString()
        {
            this.sb.Clear();
           
            {
                sb.AppendFormat("[{0}]", this.data);
                var t = this.next;
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
