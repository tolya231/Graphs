using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Graphs
{
    public class Matrix
    {
        public int n;
        public int[,] A;

        //конструктор 1 (матрица n*n)
        public Matrix(int n)
        {
            this.n = n;
            A = new int[n, n];
        }
        //конструтктор 2 (матрица из файла)
        public Matrix(string s)
        {
            string[] s2;
            string s1;
            if(!File.Exists(s))
            { MessageBox.Show("Файл не найден"); }
            else
            {
                StreamReader reader = File.OpenText(s);
                s1 = reader.ReadLine();
                s2 = s1.Split(' ');
                n = Convert.ToInt32(s2[0]);
                A = new int[n, n];
                for(int i = 0; i < n; i++)
                {
                    s1 = reader.ReadLine();
                    s2 = s1.Split(' ');
                    for(int j = 0; j < n; j++)
                        A[i, j] = Convert.ToInt32(s2[j]);
                }
                reader.Close();
            }
        }

        //запись матрицы в файл
        public void WriteToFile(string s)
        {
            StreamWriter writer = new StreamWriter(s);
            writer.WriteLine(n.ToString());
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    writer.Write(A[i, j].ToString());
                    writer.Write(" ");
                }
                writer.WriteLine();
            }
            writer.Close();
        }

        //индексатор
        public int this[int index1, int index2]
        {
            get
            {
                return A[index1, index2];
            }
            set
            {
                A[index1, index2] = value;
            }
        }
    }
}
