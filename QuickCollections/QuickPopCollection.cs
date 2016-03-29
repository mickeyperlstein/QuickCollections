using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace QuickCollections
{
    /// <summary>
    /// threadsafe Push O(n) // Pop O(1)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class QuickPopCollection<T> :  CollectionNotifier<T>, INotifier<T>
    {

        ReaderWriterLock sync = new ReaderWriterLock();
        public readonly TimeSpan TEN_SECS = new TimeSpan(0, 0, 10);

       
        Func<T, T, int> comp;
        public QuickPopCollection(Func<T, T, int> comp):base()
        {
            this.comp = comp;
        }
        
       
      
        public T Pop() //O(1)
        {
            sync.AcquireReaderLock(TEN_SECS);
            
                if (head != null)
                {

                    var ret = head;
                    sync.UpgradeToWriterLock(TEN_SECS);
                    head = head.next;
                    sync.ReleaseLock();
                    base.OnPopped(ret);
                    return ret.data;
                }
                else
                    throw new Exception("no more data");
           
        }


        public void Push(T item) // O(n)
        {
            Node<T> new_node = new Node<T> { data = item };

            sync.AcquireReaderLock(TEN_SECS);
            
                /* Special case for head node */
                if (head == null ||
                    comp(head.data, item) <= 0)
                {
                    var cookie = sync.UpgradeToWriterLock(TEN_SECS);
                        new_node.next = head;
                        head = new_node;
                    sync.DowngradeFromWriterLock(ref cookie);
                }
                else
                {

                    
                    /* Locate the node before point of insertion. */
                    Node<T> current = head;        

                    while (current.next != null &&
                           comp(current.next.data, item) > 0)
                        current = current.next;
                    var cookie = sync.UpgradeToWriterLock(TEN_SECS);
                        new_node.next = current.next;
                        current.next = new_node;
                    sync.DowngradeFromWriterLock(ref cookie);

                }
                sync.ReleaseLock();
                base.OnPushed(new_node);
            
        }

      

       

        /*  function to test above methods */
        public static void test()
        {

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("      Quick pop  collection test");

            Console.WriteLine("-----------------------------------------------");

            QuickPopCollection<int> llist = new QuickPopCollection<int>((y, x) =>
            {
                if (x == y) return 0;
                if (x < y) return 1;
                return -1;
            });

            llist.Popped += (source,node) => {
                Console.WriteLine("Node Popped O(1):{0}" , node.data);
                Console.WriteLine("Current state {0}", llist.head);
            };

            llist.Pushed += (source, node) =>
            {
                Console.WriteLine("Node Pushed O(n):{0}" , node.data);
                Console.WriteLine("Current state {0}", llist.head);
            };
            try
            {
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
            catch (ApplicationException)
            {
                Console.WriteLine("starved lock issues");
            }
        }
    }


}
