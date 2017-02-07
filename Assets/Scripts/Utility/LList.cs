using UnityEngine;
using System.Collections;

public class LList<T> where T : new()
{
    public int first = -1, last = -1;
    public int SIZE;
    public Element[] es, ageEs, orderEs; //ageEs => elements referenced by age
    public int numElements;

    public LList(int size)
    {
        numElements = 0;
        SIZE = size;
        es = new Element[SIZE];
        ageEs = new Element[SIZE];
        orderEs = new Element[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            es[i] = new Element(i, -1, -1);
        }

        es[SIZE - 1].next = 0;
        es[0].prev = SIZE - 1;

    }

    public Element getElementAt(int index)
    {
        if (index < 0 || index >= es.Length)
        {
            return null;
        }
        return es[index];

    }
    public Element getFirstElement()
    {
        if (numElements <= 0)
        {
            return null;
        }
        return es[first];

    }
    public Element getLastElement()
    {
        if (numElements <= 0)
        {
            return null;
        }
        return es[last];

    }

    public Element getPrevElement(int index)
    {
        if (index < 0 || index >= es.Length)
        {
            return null;
        }

        if (es[index].prev < 0 || es[index].prev >= es.Length)
        {
            return null;
        }
        return es[es[index].prev];
    }
    public Element getNextElement(int index)
    {
        if (index < 0 || index >= es.Length)
        {
            return null;
        }
        if (es[index].next < 0 || es[index].next >= es.Length)
        {
            return null;
        }
        return es[es[index].next];
    }

    public Element getElementAtAge(int age)
    {
        return ageEs[age];
    }
    public Element getElementAtOrder(int order)
    {
        return orderEs[order];
    }

    public void clearAll()
    {
        numElements = 0;
        first = -1;
        last = -1;
        for (int i = 0; i < SIZE; i++)
        {
            es[i].clear();

        }
    }

   /* public void addNext(T element)
    {
        if (last == -1)
        {
            last = 0;
            first = 0;
        }
        else
        {
            //last += 1;
            if (last + 1 > SIZE - 1)
            {
                es[last].next = 0;
                es[0].prev = last;
                if (es[0].next != -1)
                {
                    getElementAt(es[0].next).prev = -1;
                }
                es[0].next = -1;

                last = 0;
                first = 1;

            }
            else
            {


                if (last + 1 == first)
                {
                    first = last + 2;
                    if (first > SIZE - 1)
                    {
                        first = 0;
                    }
                    es[last].next = last + 1;
                    es[last + 1].prev = last;
                    if (es[last + 1].next != -1)
                    {
                        getElementAt(es[last + 1].next).prev = -1;
                    }
                    last++;
                    es[last].next = -1;
                }
                else
                {
                    es[last].next = last + 1;
                    es[last + 1].prev = last;
                    last++;
                    es[last].next = -1;
                }
            }
        }


        es[last].e = element;
        es[last].age = 0;
        if (numElements < SIZE)
        {
            numElements++;
        }
        ageEs[0] = es[last];
        orderEs[numElements - 1] = es[last];
        es[last].order = numElements - 1;
        int iter = es[last].prev;
        int count = 1;
        while (iter != -1)
        {
            count++;

            es[iter].age = getElementAt(es[iter].next).age + 1;
            ageEs[es[iter].age] = es[iter];

            orderEs[numElements - count] = es[iter];
            es[iter].order = numElements - count;

            iter = es[iter].prev;
        }





        //last = last + 1;

    }*/
    /// <summary>
    /// No garbage generated. Returns the next swipeNod holder. Use getNext().set() to set the values.
    /// </summary>
    /// <returns></returns>
    public T getNext()
    {
        if (last == -1)
        {
            last = 0;
            first = 0;
        }
        else
        {
            //last += 1;
            if (last + 1 > SIZE - 1)
            {
                es[last].next = 0;
                es[0].prev = last;
                if (es[0].next != -1)
                {
                    getElementAt(es[0].next).prev = -1;
                }
                es[0].next = -1;

                last = 0;
                first = 1;

            }
            else
            {


                if (last + 1 == first)
                {
                    first = last + 2;
                    if (first > SIZE - 1)
                    {
                        first = 0;
                    }
                    es[last].next = last + 1;
                    es[last + 1].prev = last;
                    if (es[last + 1].next != -1)
                    {
                        getElementAt(es[last + 1].next).prev = -1;
                    }
                    last++;
                    es[last].next = -1;
                }
                else
                {
                    es[last].next = last + 1;
                    es[last + 1].prev = last;
                    last++;
                    es[last].next = -1;
                }
            }
        }

        es[last].age = 0;
        if (numElements < SIZE)
        {
            numElements++;
        }
        ageEs[0] = es[last];
        orderEs[numElements - 1] = es[last];
        es[last].order = numElements - 1;
        int iter = es[last].prev;
        int count = 1;
       /* while (iter != -1)
        {
            count++;

            es[iter].age = getElementAt(es[iter].next).age + 1;
            ageEs[es[iter].age] = es[iter];
            Debug.LogError("numel-c>>" + numElements +"<>"+count+"<>"+ (numElements - count));
            orderEs[numElements - count] = es[iter];
            es[iter].order = numElements - count;

            iter = es[iter].prev;
        }*/
        return es[last].e;


        //last = last + 1;

    }

    public void AddNext(T item)
    {
        if (last == -1)
        {
            last = 0;
            first = 0;
        }
        else
        {
            //last += 1;
            if (last + 1 > SIZE - 1)
            {
                Debug.LogError("condition entered");
                es[last].next = 0;
                es[0].prev = last;
                if (es[0].next != -1)
                {
                    getElementAt(es[0].next).prev = -1;
                }
                es[0].next = -1;

                last = 0;
                first = 1;

            }
            else
            {


                if (last + 1 == first)
                {
                    first = last + 2;
                    if (first > SIZE - 1)
                    {
                        first = 0;
                    }
                    es[last].next = last + 1;
                    es[last + 1].prev = last;
                    if (es[last + 1].next != -1)
                    {
                        getElementAt(es[last + 1].next).prev = -1;
                    }
                    last++;
                    es[last].next = -1;
                }
                else
                {
                    es[last].next = last + 1;
                    es[last + 1].prev = last;
                    last++;
                    es[last].next = -1;
                }
            }
        }

        es[last].age = 0;
        if (numElements < SIZE)
        {
            numElements++;
        }
        ageEs[0] = es[last];
        orderEs[numElements - 1] = es[last];
        es[last].order = numElements - 1;
        int iter = es[last].prev;
        int count = 1;
     /*   while (iter != -1)
        {
            count++;

            es[iter].age = getElementAt(es[iter].next).age + 1;
            ageEs[es[iter].age] = es[iter];

            orderEs[numElements - count] = es[iter];
            es[iter].order = numElements - count;

            iter = es[iter].prev;
        }*/
      
        es[last].e = item;

        es[SIZE - 1].next = 0;
        es[0].prev = SIZE - 1;
        // Debug.LogError("last>>" + last);
        //last = last + 1;

    }



    public class Element
    {

        public int index, next = -1, prev = -1;
        public int age, order;
        //static int first = 0, last = 0;
        public T e;
        public Element(int index, int prev, int next)
        {
            age = -1;
            order = -1;
            this.index = index;
            this.prev = prev;
            this.next = next;

           // e = new T();
        }
        public void clear()
        {
            age = -1;
            order = -1;
            prev = -1;
            next = -1;
        }
    }
}

public class SwipeNod
{
    public Vector2 pos;
    public float time;
    public SwipeNod()
    {
        pos = new Vector2();
        time = -1;
    }

    public void set(Vector3 pos, float time)
    {
        this.pos.Set(pos.x, pos.y);
        this.time = time;
    }
}
