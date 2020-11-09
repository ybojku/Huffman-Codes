using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Priority
{
    // Used by class PriorityQueue<T>
    // Implements IComparable and overrides ToString (from Object)

    public class PriorityClass : IComparable
    {
        private int priorityValue;       // Ordered on priorityValue
        private String name;

        // Constructor

        public PriorityClass(int priority, String name)
        {
            this.name = name;
            priorityValue = priority;
        }

        // CompareTo (inherited from IComparable)
        // Returns >0 if the current item is greater than obj (null or otherwise)
        // Returns 0  if the current item is equal to obj (of PriorityClass)
        // Returns <0 if the current item is less than obj (of PriorityClass)

        public int CompareTo(Object obj)
        {
            if (obj != null)
            {
                PriorityClass other = (PriorityClass)obj;   // Explicit cast
                if (other != null)
                    return priorityValue - other.priorityValue;
                else
                    return 1;
            }
            else
                return 1;
        }

        // ToString (overridden from Object class)
        // Returns a string represent of an object of PriorityClass

        public override string ToString()
        {
            return name + " with priority " + priorityValue;
        }
    }
}

//-----------------------------------------------------------------------------

namespace PriorityQueue
{
    using Priority;

     public interface IContainer<T>
    {
        void MakeEmpty();  // Reset an instance to empty
        bool Empty();      // Test if an instance is empty
        int Size();        // Return the number of items in an instance
    }

    //-----------------------------------------------------------------------------

    public interface IPriorityQueue<T> : IContainer<T> where T : IComparable
    {
        void Add(T item);  // Add an item to a priority queue
        void Remove();     // Remove the item with the highest priority
        T Front();         // Return the item with the highest priority
    }

    //-------------------------------------------------------------------------

    // Priority Queue
    // Implementation:  Binary heap

    public class PriorityQueue<T> : IPriorityQueue<T> where T : IComparable
    {
        private int capacity;  // Maximum number of items in a priority queue
        private T[] A;         // Array of items
        private int count;     // Number of items in a priority queue

        // Constructor 1
        // Create a priority with maximum capacity of size
        // Time complexity:  O(1)

        public PriorityQueue(int size)
        {
            capacity = size;
            A = new T[size + 1];  // Indexing begins at 1
            count = 0;
        }

        // Constructor 2
        // Create a priority from an array of items
        // Time complexity:  O(n)

        public PriorityQueue(T[] inputArray)
        {
            int i;

            count = capacity = inputArray.Length;
            A = new T[capacity + 1];

            for (i = 0; i < capacity; i++)
            {
                A[i+1] = inputArray[i];
            }

            BuildHeap();
        }

        // PercolateUp
        // Percolate up an item from position i in a priority queue
        // Time complexity:  O(log n)

        private void PercolateUp(int i)
        {
            int child = i, parent;

            // As long as child is not the root (i.e. has a parent)
            while (child > 1)
            {
                parent = child / 2;
                if (A[child].CompareTo(A[parent]) > 0)
                // If child has a higher priority than parent
                {
                    // Swap parent and child
                    T item = A[child];
                    A[child] = A[parent];
                    A[parent] = item;
                    child = parent;  // Move up child index to parent index
                }
                else
                    // Item is in its proper position
                    return;
            }
        }

        // Add
        // Add an item into the priority queue
        // Time complexity:  O(log n)

        public void Add(T item)
        {
            if (count < capacity)
            {
                A[++count] = item;  // Place item at the next available position
                PercolateUp(count);
            }
        }

        // PercolateDown
        // Percolate down from position i in a priority queue
        // Time complexity:  O(log n)

        private void PercolateDown(int i)
        {
            int parent = i, child;

            // while parent has at least one child
            while (2 * parent <= count)
            {
                // Select the child with the highest priority
                child = 2 * parent;    // Left child index
                if (child < count)  // Right child also exists
                    if (A[child + 1].CompareTo(A[child]) > 0)
                        // Right child has a higher priority than left child
                        child++;

                // If child has a higher priority than parent
                if (A[child].CompareTo(A[parent]) > 0)
                {
                    // Swap parent and child
                    T item = A[child];
                    A[child] = A[parent];
                    A[parent] = item;
                    parent = child;  // Move down parent index to child index
                }
                else
                    // Item is in its proper place
                    return;
            }
        }

        // Remove
        // Remove an item from the priority queue
        // Time complexity:  O(log n)

        public void Remove()
        {
            if (!Empty())
            {
                // Remove item with highest priority (root) and
                // replace it with the last item
                A[1] = A[count--];

                // Percolate down the new root item
                PercolateDown(1);
            }
        }

        // Front
        // Return the item with the highest priority
        // Time complexity:  O(1)

        public T Front()
        {
            if (!Empty())
            {
                return A[1];  // Return the root item (highest priority)
            }
            else
                return default(T);
        }

        // BuildHeap
        // Create a binary heap from a given list
        // Time complexity:  O(n)

        private void BuildHeap()
        {
            int i;

            // Percolate down from the last parent to the root (first parent)
            for (i = count / 2; i >= 1; i--)
            {
                PercolateDown(i);
            }
        }

        // HeapSort
        // Sort and return inputArray
        // Time complexity:  O(n log n)

        public void HeapSort(T[] inputArray)
        {
            int i;

            capacity = count = inputArray.Length;

            // Copy input array to A (indexed from 1)
            for (i = capacity - 1; i >= 0; i--)
            {
                A[i + 1] = inputArray[i];
            }

            // Create a binary heap
            BuildHeap();

            // Remove the next item and place it into the input (output) array
            for (i = 0; i < capacity; i++)
            {
                inputArray[i] = Front();
                Remove();
            }
        }

        // MakeEmpty
        // Reset a priority queue to empty
        // Time complexity:  O(1)

        public void MakeEmpty()
        {
            count = 0;
        }

        // Empty
        // Return true if the priority is empty; false otherwise
        // Time complexity:  O(1)

        public bool Empty()
        {
            return count == 0;
        }

        // Size
        // Return the number of items in the priority queue
        // Time complexity:  O(1)

        public int Size()
        {
            return count;
        }
    }

}
