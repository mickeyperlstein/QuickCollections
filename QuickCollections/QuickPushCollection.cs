using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuickCollections
{
    public class QuickPushCollection<T> : CollectionNotifier<T>
    {
        ReaderWriterLock sync = new ReaderWriterLock();
        public readonly TimeSpan TEN_SECS = new TimeSpan(0, 0, 10);

        
        Func<T, T, int> comp;
        public QuickPushCollection(Func<T, T, int> comp)
        {
            this.comp = comp;
        }

       
        public void Push(T item) //O(1)
        {
            var new_node = new Node<T>{ data = item};

            sync.AcquireWriterLock(TEN_SECS);
                if (head == null)
                    head = new_node;
                else
                {
                    new_node.next = head;
                    head = new_node;
                }
           sync.ReleaseLock();
                base.OnPushed(new_node);
              
        }

        public T Pop() //O(n)
        {
            Node<T> before = null;
            

            //special case first item
            Node<T> beforeMax = null;

            sync.AcquireReaderLock(TEN_SECS);
                var max = head;
                if (head == null)
                    throw new Exception("no more data");

                var temp = head.next;

                while (temp != null)
                {

                    if (comp(max.data, temp.data) < 0)
                    {
                        max = temp;
                        beforeMax = before;
                    }
                    before = temp;
                    temp = temp.next;
                }

                //found max
                var temp1 = max;
            var cookie = sync.UpgradeToWriterLock(TEN_SECS);

                if (beforeMax == null)
                {
                    head = max.next;
                }
                else
                {
                    beforeMax.next = max.next;
                }
                sync.DowngradeFromWriterLock(ref cookie);
            base.OnPopped(temp1);
            sync.ReleaseLock();
                return temp1.data;
            }

        

       


        
            public static void test()
        {

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("      Quick push collection test");

                Console.WriteLine("-----------------------------------------------");

            var llist = new QuickPushCollection<int>((y, x) =>
            {
                if (x == y) return 0;
                if (x < y) return 1;
                return -1;
            });
            llist.Popped += (source, node) =>
            {
                Console.WriteLine("Node Popped O(n):{0}", node.data);
                Console.WriteLine("Current state {0}", llist.head);
            };

            llist.Pushed += (source, node) =>
            {
                Console.WriteLine("Node Pushed O(1):{0}", node.data);
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
                Console.WriteLine("Starved lock issues");
            }
            
            
            
            
        }


    }
}
