using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCollections
{
    public class QuickPushCollection<T> : MaxCollection<T>
    {
        Func<T, T, int> comp;
        public QuickPushCollection(Func<T, T, int> comp)
        {
            this.comp = comp;
        }


        public void Push(T item) //O(1)
        {
            var new_node = new Node<T>{ data = item};
            if (head == null)
                head = new_node;
            else
            {
                new_node.next = head;
                head = new_node;
            }
                
        }

        public T Pop() //O(n)
        {
            Node<T> before = null;
            

            //special case first item
            Node<T> beforeMax = null;
            var max = head;
            if (head == null)
                throw new Exception("no more data");

            var temp = head.next;
           
            while(temp != null)
            {
                
                if (comp(max.data,temp.data)<0)
                {
                    max = temp;
                    beforeMax = before;
                }
                before = temp;
                temp = temp.next;
            }

            //found max
            var temp1 = max;
            if (beforeMax ==null)
            {
                head = max.next;
            }
            else
            {
                beforeMax.next = max.next;
            }
            return temp1.data;

        }

        private Node<T> remove(Node<T> max)
        {
            var ret = max;
           if (max.next !=null)
           {//in the middle
               max.data = max.next.data;
               max.next = max.next.next;
           }else
           {
               
           }
           return ret;
        }


        
            public static void test()
        {
            var llist = new QuickPushCollection<int>((y, x) =>
            {
                if (x == y) return 0;
                if (x < y) return 1;
                return -1;
            });


            llist.Push(1000);
            llist.Push(200);
            llist.Push(30);
            llist.Push(400);
            var ret = llist.Pop();
            Debug.Assert(ret == 1000);
            ret = llist.Pop();
            Debug.Assert(ret == 400);
            llist.Push(100);

            ret = llist.Pop();
            Debug.Assert(ret == 200);
            ret = llist.Pop();
            Debug.Assert(ret == 100);
            ret = llist.Pop();
            Debug.Assert(ret == 30);
            try
            {
                ret = llist.Pop();
            }
            catch (Exception e)
            {
                Debug.Assert(e.Message == "no more data");
            }

            
            
            
            
        }
        
    }
}
