using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace QuickCollections
{
    /// <summary>
    /// threadsafe Push O(n) // Pop O(1)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class QuickPopCollection<T> :  MaxCollection<T>
    {
        protected object sync = new object();
        Func<T, T, int> comp;
        public QuickPopCollection(Func<T, T, int> comp):base()
        {
            this.comp = comp;
        }
        

      
        public T Pop() //O(1)
        {
            lock (sync)
            {
                if (head != null)
                {
                    T ret = head.data;
                    head = head.next;
                    return ret;
                }
                else
                    throw new Exception("no more data");
            }
        }


        public void Push(T item) // O(n)
        {
            Node<T> new_node = new Node<T> { data = item };
            
            lock (sync)
            {
                /* Special case for head node */
                if (head == null ||
                    comp(head.data, item) <= 0)
                {
                    new_node.next = head;
                    head = new_node;
                }
                else
                {

                    
                    /* Locate the node before point of insertion. */
                    Node<T> current = head;        

                    while (current.next != null &&
                           comp(current.next.data, item) > 0)
                        current = current.next;

                    new_node.next = current.next;
                    current.next = new_node;

                }
            }
        }

      

       

        /*  function to test above methods */
        public static void test()
        {
            QuickPopCollection<int> llist = new QuickPopCollection<int>((y, x) =>
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
