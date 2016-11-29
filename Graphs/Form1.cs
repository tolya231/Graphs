using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Graphs
{
    public partial class Form1 : Form
    {
        Graphics g;
        Graph graph;
        bool MouseHold;
        int HeldPoint;

        //Инициализация
        public Form1()
        {
            InitializeComponent();
            MouseHold = false;
            HeldPoint = -1;
            g = Graphics.FromHwnd(pictureBox.Handle);
            graph = new Graph();
        }

        //Смена цвета
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = сd1.ShowDialog();
            if(result == DialogResult.OK)
                graph.ChangeColor(сd1.Color);
            if(!(graph.Empty))
                pictureBox.Refresh();
        }

        //Обработчик перерисовки графического окна
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Aquamarine);
            numericUpDown1.Maximum = graph.Size;
            numericUpDown2.Maximum = graph.Size;
            numericUpDown3.Maximum = graph.Size;
            numericUpDown4.Maximum = graph.Size;
            numericUpDown1.Minimum = 1;
            numericUpDown2.Minimum = 1;
            numericUpDown3.Minimum = 1;
            numericUpDown4.Minimum = 1;
            if(graph.Size == 0)
            {
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;

            }
            else
            {
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button8.Enabled = true;
                button9.Enabled = true;
                button10.Enabled = true;
                button11.Enabled = true;
                button12.Enabled = true;
            }
            graph.Draw(e.Graphics);
        }

        //Обработчик нажатия мыши в графическом окне
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
                    if(e.Button == MouseButtons.Left)
                    {
                        int r = graph.Closest(e.Location, 30);
                        if(r == -1)
                        { graph.Add(e.Location); HeldPoint = graph.Size - 1; MouseHold = true; }
                        else
                        { HeldPoint = r; MouseHold = true; }
                    }
                    else if(e.Button == MouseButtons.Right)
                    {
                        int r = graph.Closest(e.Location, 10);
                        if(r != -1)
                            graph.Remove(r + 1);
                    }
                    pictureBox.Refresh();

        }

        //Обработчик движения мыши в графическом окне
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            
                if (!(graph.Empty))
                {
                    if (MouseHold)
                    {
                        try
                        { graph[HeldPoint] = e.Location; }
                        catch (System.InvalidCastException) { }
                    }
                }
            pictureBox.Refresh();

        }

        //Обработчик отпускания кнопки мыши в графическом окне
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if(!(graph.Empty))
            {
                HeldPoint = -1;
                MouseHold = false;
                pictureBox.Refresh();
            }
        }

        //Нарисовать граф из файла
        private void button2_Click(object sender, EventArgs e)
        {
            graph = new Graph(textBox1.Text);
            pictureBox.Refresh();
        }

        //удалить граф
        private void button3_Click(object sender, EventArgs e)
        {
            //g.Clear(Color.Aquamarine);
            int t = graph.Size - 1;
            while(t > -1)
            {
                graph.Remove(t + 1);
                t--;
            }
            g.Clear(Color.Aquamarine);
            pictureBox.Refresh();
        }

        //Проверка на связность
        private void button4_Click(object sender, EventArgs e)
        {
                MessageBox.Show(graph.Connected().ToString());
        }

        //Проверка на полноту
        private void button5_Click(object sender, EventArgs e)
        {
                MessageBox.Show(graph.IsFull().ToString());
        }

        //запись графа в файл
        private void button6_Click(object sender, EventArgs e)
        {
            graph.WriteToFile(textBox2.Text);
        }

        //Открыть матрицу смежности
        private void button7_Click(object sender, EventArgs e)
        {
            Matrix_sm NewForm = new Matrix_sm(graph);
            NewForm.Show();
            Refresh();
        }


        //Открыть файл
        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                MessageBox.Show(sr.ReadToEnd());
                sr.Close();
                pictureBox.Refresh();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show(graph.Dipartition().ToString());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int q1 = (int)numericUpDown1.Value - 1;
            int q2 = (int)numericUpDown2.Value - 1;
            if(graph.Dijkstra(q1, q2) != -1)
                MessageBox.Show(graph.Dijkstra(q1, q2).ToString());
            else
                MessageBox.Show("Нет пути");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            graph.Prima(g);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            graph.AddEdge((int)numericUpDown3.Value - 1, (int)numericUpDown4.Value - 1, (int)numericUpDown5.Value);
            pictureBox.Refresh();
        }
    }
}
