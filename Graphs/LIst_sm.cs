using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Graphs
{
    class List_sm
    {
        public List<int>[] L;
        int size = 0;
        //конструктор по матрице смежности
        public List_sm(Matrix M)
        {
            L = new List<int>[100];
            size = M.n;
            for(int i = 0; i < M.n; i++)
            {
                L[i] = new List<int>();
                for(int j = 0; j < M.n; j++)
                    if(M[i, j] != 0)
                        L[i].Add(j);
            }
        }

        //добавление вершины
        public void AddVertex()
        {
            size++;
            L[size] = new List<int>();
        }

        //удаление вершины
        public void DeleteVertex(int i)
        {
            for(int j = i; j < size - 1; j++)
                L[j] = L[j - 1];
            for(int j = 0; j < L[size - 1].Count - 1; j++)
                L[i].RemoveAt(j);
            size--;
        }

    }
}
