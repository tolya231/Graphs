using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Graphs
{
    public class Graph
    {

        List<Point> L;
        Color color;
        string drawString;
        public Matrix M;
        //Конструктор пустого графа
        public Graph()
        {
            L = new List<Point>();
            color = Color.Black;
        }

        //Конструктор по матрице смежности
        public Graph(string s)
        {
            L = new List<Point>();
            color = Color.Black;
            M = new Matrix(s);
            Point pt = new Point();
            for(int i = 0; i < M.n; i++)
            {
                pt.X = 350 + (int)(150 * Math.Cos(2 * Math.PI * i / M.n));
                pt.Y = 273 + (int)(150 * Math.Sin(2 * Math.PI * i / M.n));
                L.Add(pt);
            }
        }

        //запись графа в файл
        public void WriteToFile(string s)
        {
            M.WriteToFile(s);
        }
        //Количество вершин
        public int Size
        { get { return L.Count; } }

        //Цвет 
        public Color Color
        { get { return color; } set { color = value; } }

        //Название вершины
        public string DrawString
        { get { return drawString; } set { drawString = value; } }

        //Смена цвета графа
        public void ChangeColor(Color color)
        { this.color = color; }

        //Индексатор
        public Point this[int i]
        { get { return L[i]; } set { L[i] = value; } }

        //Добавление вершины в конец
        public void Add(Point P)
        {
            L.Add(P);
            if(Size != 1)
            {
                Matrix B = new Matrix(Size - 1);
                B = M;
                M = new Matrix(Size);
                for(int i = 0; i < Size - 1; i++)
                    for(int j = 0; j < Size - 1; j++)
                        M[i, j] = B[i, j];
                for(int i = 0; i < Size; i++)
                {
                    M[i, Size - 1] = 0;
                    M[Size - 1, i] = 0;
                }
            }
            else
            {
                M = new Matrix(1);
                M[0, 0] = 1;
            }
        }

        //Проверка на связность (если не пустой)
        public bool Connected()
        {
            if(Size == 1)
                return true;
            else
            {
                int sum;
                for(int i = 0; i < Size; i++)
                {
                    sum = 0;
                    for(int j = 0; j < Size; j++)
                        if(M[i, j] == 0 && j != i)
                            sum++;
                    if(sum == Size - 1)
                        return false;
                }
            }
            return true;
        }

        //проверка на полноту
        public bool IsFull()
        {
            if(Size == 1)
                return true;
            else
                for(int i = 0; i < Size; i++)
                    for(int j = 0; j < Size; j++)
                        if(M[i, j] == 0)
                            return false;
            return true;
        }

        //Удаление вершины
        public void Remove(int q)
        {
            q--;
            if(Size != 1)
            {
                Matrix B = new Matrix(Size - 1);
                if(q != 0)
                {
                    //левая верхняя часть
                    for(int i = 0; i < q; i++)
                        for(int j = 0; j < q; j++)
                            B[i, j] = M[i, j];
                    //левая нижняя часть
                    for(int i = q + 1; i < Size; i++)
                        for(int j = 0; j < q; j++)
                            B[i - 1, j] = M[i, j];
                    //правая верхняя часть
                    for(int i = 0; i < q; i++)
                        for(int j = q + 1; j < Size; j++)
                            B[i, j - 1] = M[i, j];
                    //правая нижняя часть
                    for(int i = q + 1; i < Size; i++)
                        for(int j = q + 1; j < Size; j++)
                            B[i - 1, j - 1] = M[i, j];
                }
                else
                    for(int i = 1; i < Size; i++)
                        for(int j = 1; j < Size; j++)
                            B[i - 1, j - 1] = M[i, j];
                M = new Matrix(Size - 1);
                M = B;
            }
            /*else
            {
                //M[0, 0] = 0;
                //M.n = 0;
            }*/
            L.RemoveAt(q);
        }

        //Удаление ребра
        public void DeleteEdge(int i, int j)
        {
            if(Size > 0)
            {
                M[i - 1, j - 1] = 0;
                M[j - 1, i - 1] = 0;
            }
        }

        //Поиск остовного дерева d
        public void Prima(Graphics g)
        {
            if(Connected() == true)
            {
                int[] used = new int[Size];
                int[,] a = new int[Size, Size];
                for(int i = 0; i < Size; i++)
                {
                    used[i] = 0;
                    for(int j = 0; j < Size; j++)
                        a[i, j] = 0;
                }
                used[0] = 1;
                int count = 0;
                while(count < Size - 1)
                {
                    int min = int.MaxValue;
                    for(int i = 0; i < Size; i++)
                        if(used[i] != 0)
                        {
                            for(int j = 0; j < Size; j++)
                            {
                                if(M[i, j] < min && M[i, j] != 0 && used[j] == 0 && i != j)
                                    min = M[i, j];
                            }
                        }
                    for(int i = 0; i < Size; i++)
                        for(int j = 0; j < Size; j++)
                            if(M[i, j] == min && used[j] == 0)
                            {
                                used[j] = 1;
                                a[i, j] = min;
                                a[j, i] = min;
                                count++;
                                i = Size;
                                break;
                            }
                }
                //a - матрица смежности остовного дерева
                // отрисовка остовного дерева
                Pen myPen = new Pen(Color.Yellow, 2);
                if(Size > 0)
                {
                    for(int i = 0; i < Size; i++)
                        for(int j = 0; j < Size; j++)
                            if(a[i, j] != 0)
                                g.DrawLine(myPen, this[i], this[j]);
                }
            }
            else
                MessageBox.Show("Граф не связен");
        }

        //Поиск кратчайшего расстояния (Алгоритм Дейкстры)
        public int Dijkstra(int st, int q)
        {
            int[] distance = new int[Size];
            int count, index = 0, i, u, m = st + 1;
            bool[] visited = new bool[Size];
            for(i = 0; i < Size; i++)
            {
                distance[i] = int.MaxValue;
                visited[i] = false;
            }
            distance[st] = 0;
            for(count = 0; count < Size - 1; count++)
            {
                int min = int.MaxValue;
                for(i = 0; i < Size; i++)
                    if(!visited[i] && distance[i] <= min)
                    {
                        min = distance[i];
                        index = i;
                    }
                u = index;
                visited[u] = true;
                for(i = 0; i < Size; i++)
                    if(!visited[i] && M[u, i] > 0 && distance[u] != int.MaxValue && distance[u] + M[u, i] < distance[i])
                        distance[i] = distance[u] + M[u, i];
            }
            if(distance[q] == int.MaxValue)
                return -1;
            else
                return distance[q];
        }

        //Поиск в глубину с изменением цвета
        public bool dfs(int v, int[] col)
        {
            for(int i = 0; i < Size; i++)
            {
                if(M[v, i] > 0 && v != i)
                    if(col[i] == 0)
                    {
                        col[i] = 3 - col[v];
                        dfs(i, col);
                    }
                    else if(col[i] == col[v])
                        return false;
            }
            return true;
        }
        //Проверка на двудольность
        public bool Dipartition()
        {
            bool ok = true;
            int[] col = new int[Size];
            for(int i = 0; i < Size; i++)
                col[i] = 0;
            for(int i = 0; i < Size; i++)
                if(col[i] == 0)
                {
                    col[i] = 1;
                    Console.WriteLine(ok);
                    Console.WriteLine(i);
                    ok = dfs(i, col);
                    Console.WriteLine(ok);
                    /*if(!ok)
                        return false;*/
                }
            return ok;
        }

        //Квадрат расстояния между двумя точками
        int dist(Point P1, Point P2)
        { return (P1.X - P2.X) * (P1.X - P2.X) + (P1.Y - P2.Y) * (P1.Y - P2.Y); }

        //нахождение ближайшей к данной точке вершины в заданном круге
        public int Closest(Point P, int range)
        {
            if(Size <= 0)
                return -1;

            int e = -1;
            int d = range;

            for(int i = 0; i < Size; ++i)
                if(dist(this[i], P) < d)
                { d = dist(this[i], P); e = i; }

            return e;
        }

        //Добавить ребро
        public void AddEdge(int i, int j, int k)
        {
            M[i, j] = k;
            M[j, i] = k;
        }

        //Индикатор пустоты списка
        public bool Empty
        { get { return Size == 0; } }

        //Отрисовка ломаной
        public void Draw(Graphics g)
        {
            Pen myPen = new Pen(color, 2);
            SolidBrush myBrush = new SolidBrush(color);
            Font myFont = new System.Drawing.Font("Arial", 17);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            SolidBrush drawBrush2 = new SolidBrush(Color.Red);
            Point pt = new Point();
            //отрисовка звеньев
            if(Size > 1)
            {
                for(int i = 0; i < Size; i++)
                    for(int j = 0; j < Size; j++)
                        if(M[i, j] != 0)
                        {
                            g.DrawLine(myPen, this[i], this[j]);
                            pt.X = (L[i].X + L[j].X) / 2;
                            pt.Y = (L[i].Y + L[j].Y) / 2 - 5;
                            if(i != j) g.DrawString(M[i, j].ToString(), myFont, drawBrush2, pt);
                        }
            }
            //отрисовка вершин
            for(int i = 0; i < Size; i++)
            {
                pt.X = this[i].X + 3;
                pt.Y = this[i].Y + 3;
                g.FillPie(myBrush, new Rectangle(this[i].X - 3, this[i].Y - 4, 8, 8), 0, 360);
                g.DrawString((i + 1).ToString(), myFont, drawBrush, pt);
            }
        }

    }
}
