using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickCollections
{
   // Java Program to insert in a sorted list
class QuickPopCollection<T>
{
    Func<T, T, int> comp;
    public QuickPopCollection(Func<T,T,int> comp)
    {
        this.comp = comp;
    }
    Node<T> max;  // max of list
 
    /* Linked list Node*/
    
 
    /* function to insert a new_node in a list. */

    public T Pop() //O(1)
    {
        if (max !=null)
        {
            T ret = max.data;
            max = max.next;
            return ret;
        }else
            throw new Exception ("no more data");
    }
    
    
    public void Push(T data) // O(n)
    {
         Node<T> new_node = new Node<T> { data = data };
        
 
         /* Special case for max node */
         if (max == null || 
             comp( max.data ,data)<=0)
         {
            new_node.next = max;
            max = new_node;
         }
         else {
 
            /* Locate the node before point of insertion. */
             Node<T> current = max;
 
            while (current.next != null &&
                   comp(current.next.data , new_node.data)>0)
                  current = current.next;
 
            new_node.next = current.next;
            current.next = new_node;
         }
     }
 
                  /*Utility functions*/
 
   

    StringBuilder sb = new StringBuilder();
    public override string ToString()
    {
        sb.Clear();
        if (max == null)
            sb.AppendFormat("EMPTY");
        else
        {
            sb.AppendFormat("MAX:{0}", max.data);
            Node<T> t = max.next;
            while (t != null)
            {
                sb.AppendFormat("=>{0}", t.data);
                t = t.next;

            }
        }
        return sb.ToString();

    }
     
 
     /*  function to test above methods */
     public static void test()
     {
         QuickPopCollection<int> llist = new QuickPopCollection<int>((y,x) =>
         {
             if (x == y) return 0;
             if (x < y) return 1;
             return -1;
         });


         llist .Push (1000);
         llist.Push (200);
         llist.Push(30);
         llist.Push(400);
         var ret = llist.Pop();

         llist.Push (100);

     }
}


}
