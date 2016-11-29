using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Graphs
{
    public partial class Matrix_sm : Form
    {
        Graph G;
        public Matrix_sm(Graph G)
        {
            InitializeComponent();
            this.G = G;
            ReDraw();
        }
        //перерисовка таблицы
        public void ReDraw()
        {
            tableLayoutPanel1.Controls.Clear();
            numericUpDown4.Maximum = G.Size;
            numericUpDown5.Maximum = G.Size;
            numericUpDown6.Maximum = G.Size;
            tableLayoutPanel1.ColumnCount = G.Size;
            tableLayoutPanel1.RowCount = G.Size;
            for(int i = 0; i < G.Size; i++)
                for(int j = 0; j < G.Size; j++)
                {
                    Size s = new Size();
                    s.Width = 40;
                    s.Height = 10;
                    NumericUpDown nud = new NumericUpDown();
                    nud.Size = s;
                    nud.Maximum = 1000;
                    nud.Minimum = -1000;
                    nud.Text = G.M.A[i, j].ToString();
                    tableLayoutPanel1.Controls.Add(nud, i, j);
                    tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.AutoSize;
                    tableLayoutPanel1.RowStyles[0].SizeType = SizeType.AutoSize;
                }
            numericUpDown1.Maximum = G.Size;
            numericUpDown2.Maximum = G.Size;
            numericUpDown3.Maximum = G.Size;
            
        }

        //Добавляем вершину
        private void button2_Click(object sender, EventArgs e)
        {
            Point pt = new Point(200 + G.Size * 40, 200 + G.Size * 30);
            G.Add(pt);
            ReDraw();
        }

        //Удаляем вершину
        private void button3_Click(object sender, EventArgs e)
        {
            G.Remove((int)numericUpDown1.Value);
            ReDraw();
        }

        //удаляем ребро
        private void button4_Click(object sender, EventArgs e)
        {
            G.DeleteEdge((int)numericUpDown2.Value, (int)numericUpDown3.Value);
            ReDraw();
        }

        //Закрыть матрицу смежности
        private void button1_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < G.Size; i++)
                for(int j = 0; j < G.Size; j++)
                    G.M[i, j] = (int)((NumericUpDown)tableLayoutPanel1.GetControlFromPosition(i, j)).Value;
                    Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            G.AddEdge((int)numericUpDown4.Value - 1, (int)numericUpDown5.Value - 1, (int)numericUpDown6.Value);
            ReDraw();
        }
    }
}
