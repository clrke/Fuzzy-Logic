using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        ArrayList trapezoids;
        public static float x;

        public Form1()
        {
            InitializeComponent();
            trapezoids = new ArrayList();
            txtMax.Focus();
            txtMax.Text = "1";
            txtMax_Leave(null, null);
            lbTrapezoids.SelectedIndex = 0;
        }

        private void txtMax_Leave(object sender, EventArgs e)
        {
            try
            {
                int x = int.Parse(txtMax.Text);

                while (trapezoids.Count < x)
                    trapezoids.Add(new Trapezoid(trapezoids.Count+1, 0, 0, 0, 0));

                while (trapezoids.Count > x)
                    trapezoids.RemoveAt(trapezoids.Count - 1);

                RefreshGrid();
            }
            catch { }
        }

        private void txtMax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == '\r')
            {
                groupBox1.Focus();
            }
        }

        private void lbTrapezoids_SelectedIndexChanged(object sender, EventArgs e)
        {
            Trapezoid trapezoid = lbTrapezoids.SelectedItem as Trapezoid;
            groupBox1.Text = string.Format("Trapezoid # {0}", trapezoid.id.ToString("000"));
            textBox1.Text = trapezoid.a.ToString();
            textBox2.Text = trapezoid.b.ToString();
            textBox3.Text = trapezoid.c.ToString();
            textBox4.Text = trapezoid.d.ToString();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Trapezoid trapezoid = lbTrapezoids.SelectedItem as Trapezoid;
            trapezoid.a = float.Parse(textBox1.Text);
            RefreshGrid();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Trapezoid trapezoid = lbTrapezoids.SelectedItem as Trapezoid;
            trapezoid.b = float.Parse(textBox2.Text);
            RefreshGrid();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Trapezoid trapezoid = lbTrapezoids.SelectedItem as Trapezoid;
            trapezoid.c = float.Parse(textBox3.Text);
            RefreshGrid();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Trapezoid trapezoid = lbTrapezoids.SelectedItem as Trapezoid;
            trapezoid.d = float.Parse(textBox4.Text);
            RefreshGrid();
        }

        static Bitmap bitmap = new Bitmap(400, 190);
        Graphics graphics = Graphics.FromImage(bitmap);
        Pen pen = new Pen(Brushes.Black, 4);

        public void RefreshGrid()
        {
            Form1.x = float.Parse(textBox6.Text);

            int selected = lbTrapezoids.SelectedIndex;
            lbTrapezoids.Items.Clear();
            foreach (Trapezoid trapezoid in trapezoids)
                lbTrapezoids.Items.Add(trapezoid);

            lbTrapezoids.SelectedIndex = selected;

            textBox5.Text = (lbTrapezoids.SelectedItem as Trapezoid).CrispOutput(float.Parse(textBox6.Text)).ToString();//(float.Parse(textBox6.Text));
            graphics.Clear(Color.White);

            graphics.DrawLine(new Pen(Brushes.Black, 10), 0, 180, 490, 180);
            graphics.DrawLine(new Pen(Brushes.Black, 10), 180-Form1.x, 190, 180-Form1.x, 0);

            foreach (Trapezoid trapezoid in lbTrapezoids.Items)
            {
                graphics.DrawLine(pen, 180 + trapezoid.a - x, 180, 180 + trapezoid.b - x, 90);
                graphics.DrawLine(pen, 180 + trapezoid.b - x, 90, 180 + trapezoid.c - x, 90);
                graphics.DrawLine(pen, 180 + trapezoid.c - x, 90, 180 + trapezoid.d - x, 180);

                graphics.DrawString(trapezoid.a.ToString(), new Font(FontFamily.GenericSansSerif, 8), Brushes.Cyan, 160 + trapezoid.a-x, 173);
                graphics.DrawString(trapezoid.b.ToString(), new Font(FontFamily.GenericSansSerif, 8), Brushes.Blue, 160 + trapezoid.b-x, 80);
                graphics.DrawString(trapezoid.c.ToString(), new Font(FontFamily.GenericSansSerif, 8), Brushes.Blue, 180 + trapezoid.c - x, 80);
                graphics.DrawString(trapezoid.d.ToString(), new Font(FontFamily.GenericSansSerif, 8), Brushes.Cyan, 180 + trapezoid.d - x, 173);
            }

            try
            {
                float x = float.Parse(textBox6.Text);
                graphics.DrawLine(pen, 180, 0, 180, 190);
                graphics.DrawString(x.ToString(), new Font(FontFamily.GenericSansSerif, 8), Brushes.Black, 180, 0);
            }
            catch {}
            pictureBox1.Refresh();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = bitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox6.Text = (float.Parse(textBox6.Text) + 1).ToString();
            RefreshGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox6.Text = (float.Parse(textBox6.Text) - 1).ToString();
            RefreshGrid();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            RefreshGrid();
        }
    }

    class Trapezoid
    {
        public int id;
        public float a;
        public float b;
        public float c;
        public float d;

        public Trapezoid(int id, float a, float b, float c, float d)
        {
            this.id = id;
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public float CrispOutput(float x)
        {
            return Max(Min((x-a)/(b-a), 1, (d-x)/(d-c)), 0);
        }
        public override string ToString()
        {
            return string.Format("{0}   {1,3}, {2,3}, {3,3}, {4,3}   {5}", id.ToString("000"), a, b, c, d, (CrispOutput(Form1.x)).ToString("0.00"));
        }

        public float Min(params float[] values)
        {
            float min = values[0];

            for (int i = 1; i < values.Length; i++)
                if (values[i] < min)
                    min = values[i];

            return min;
        }

        public float Max(params float[] values)
        {
            float max = values[0];

            for (int i = 1; i < values.Length; i++)
                if (values[i] > max)
                    max = values[i];

            return max;
        }
    }
}
