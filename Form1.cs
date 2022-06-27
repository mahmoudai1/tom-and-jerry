using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{

    public partial class Form1 : Form
    {
        Bitmap off;
        class VLine
        {
            public int X;
            public int Y;
            public int W;
            public int H;
            public List<HLine> LHLines = new List<HLine>();
        }
        class HLine
        {
            public int X;
            public int Y;
            public int W;
            public int H;
            public bool BlueTaken;
            public List<Dot> LDots = new List<Dot>();
        }
        class Tom
        {
            public Bitmap im;
            public int X;
            public int Y;
        }
        class Jerry
        {
            public Bitmap im;
            public int X;
            public int Y;
        }

        class Dot
        {
            public int X;
            public int Y;
            public int W;
            public int H;
        }

        class SingleLine
        {
            public int X1;
            public int Y1;
            public int X2;
            public int Y2;
        }

        List <VLine> L1 = new List<VLine>();
        List <HLine> L2 = new List<HLine>();
        List<Tom> L3 = new List<Tom>();
        List<Jerry> L4 = new List<Jerry>();
        List<SingleLine> L5 = new List<SingleLine>();

        Timer t = new Timer();
        Random RR;
        Random RR2;
        Random RR3;
        Random RR4;
        Random RR5;
        int TomLine, JerryLine;
        bool StartMoving = false;
        bool Finish = false;
        bool win = false;
        int ctF = 0;
        bool iDrag = false;
        int iCatch = -1;
        int prevX, prevY;


        public Form1()
        {
            InitializeComponent();
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            this.MouseUp += new MouseEventHandler(Form1_MouseUp);
            this.WindowState = FormWindowState.Maximized;
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.KeyUp += Form1_KeyUp;
            this.Text = " Assignment 13";
            this.KeyDown += Form1_KeyDown;
            t.Tick += T_Tick;
            t.Start();
        }

        void MoveTom()
        {
            int f = 1;
            for (int k = 0; k < 50; k++)
            {
                f = 1;
                L3[0].Y++;
                for (int i = 0; i < L1[TomLine].LHLines.Count; i++)
                {
                    if (L3[0].Y + (L3[0].im.Height / 2) == L1[TomLine].LHLines[i].Y && L3[0].X + (L3[0].im.Width / 2) == L1[TomLine].LHLines[i].X + L1[TomLine].LHLines[i].W)
                    {
                        for (int j = L1[TomLine].LHLines[i].X + L1[TomLine].LHLines[i].W; j > L1[TomLine].LHLines[i].X; j--)
                        {
                            for (int t = 0; t < 30; t++)
                            {
                                L3[0].X--;
                                if(L3[0].X + (L3[0].im.Width / 2) == L1[TomLine - 1].X)
                                {
                                    f = 0;
                                    TomLine--;
                                    break;
                                }
                            }
                            if(f == 0)
                            {
                                break;
                            }
                        }
                    }
                    else if (L3[0].Y + (L3[0].im.Height / 2) == L1[TomLine].LHLines[i].Y && L3[0].X + (L3[0].im.Width / 2) + L1[TomLine].W == L1[TomLine].LHLines[i].X)
                    {
                        for (int j = L1[TomLine].LHLines[i].X; j < L1[TomLine].LHLines[i].X + L1[TomLine].LHLines[i].W + L1[TomLine].W; j++)
                        {
                            for (int t = 0; t < 30; t++)
                            {
                                L3[0].X++;
                                if (L3[0].X + (L3[0].im.Width / 2) == L1[TomLine + 1].X)
                                {
                                    f = 0;
                                    TomLine++;
                                    break;
                                }
                            }
                            if (f == 0)
                            {
                                break;
                            }
                        }
                    }
                    if (f == 0)
                    {
                        f = 1;
                        break;
                    }
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            

            DrawDouble(this.CreateGraphics());
        }

        private void T_Tick(object sender, EventArgs e)
        {
            if (StartMoving && !Finish)
            {
                if (L3[0].Y < L4[0].Y)
                {
                    MoveTom();
                }
                else
                {
                    if(TomLine == JerryLine)
                    {
                        win = true;
                        Finish = true;
                    }
                    else
                    {
                        win = false;
                        Finish = true;
                    }
                }
            }

            if(Finish && win && ctF++ == 0)
            {
                MessageBox.Show("Good, you won!");
                StartMoving = false;
            }
            else if(Finish && !win && ctF++ == 0)
            {
                MessageBox.Show("Bad, you lost!");
                StartMoving = false;
            }
            DrawDouble(this.CreateGraphics());
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !StartMoving)
            {
                L1.Clear();
                L3.Clear();
                L4.Clear();
                L5.Clear();
                for (int i = 0; i < L1.Count - 1; i++)
                {
                    L1[i].LHLines.Clear();
                    for(int j = 0; j < L1[i].LHLines.Count; j++)
                    {
                        L1[i].LHLines[j].LDots.Clear();
                    }
                }
                Finish = false;
                win = false;
                ctF = 0;
                CreateActors();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDouble(e.Graphics);
        }

        void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            
            
            if (!Finish && StartMoving)
            {
               MoveTom();
            }

            if(e.X >= L1[iCatch + 1].X  && e.Y >= L1[iCatch + 1].Y + 50 && e.Y <= L1[iCatch + 1].Y + L1[iCatch + 1].H && iDrag)
            {
                StartMoving = true;
                ReplaceByLine();
                iCatch = -1;
                L5.Clear();
            }
            else
            {
                if (iDrag)
                {
                    L5.Clear();
                }
            }
            iDrag = false;
        }

        void ReplaceByLine()
        {
            HLine pnn = new HLine();
            pnn.X = L1[iCatch].X + L1[iCatch].W;
            pnn.Y = L5[0].Y1 - 7;
            pnn.W = L1[iCatch + 1].X - L1[iCatch].X - L1[iCatch].W;
            pnn.H = this.ClientSize.Height / (1975 / 15);

            L1[iCatch].LHLines.Add(pnn);
            L1[iCatch + 1].LHLines.Add(pnn);
            L1[iCatch].LHLines[L1[iCatch].LHLines.Count - 1].BlueTaken = false;

            Dot pnn2 = new Dot();
            pnn2.X = L1[iCatch].LHLines[L1[iCatch].LHLines.Count - 1].X + L1[iCatch].LHLines[L1[iCatch].LHLines.Count - 1].W - this.ClientSize.Width / (3360 / 15);
            pnn2.Y = L1[iCatch].LHLines[L1[iCatch].LHLines.Count - 1].Y - this.ClientSize.Height / (1975 / 8); ;
            pnn2.W = this.ClientSize.Width / (3360 / 30);
            pnn2.H = this.ClientSize.Height / (1975 / 30);
            L1[iCatch].LHLines[L1[iCatch].LHLines.Count - 1].LDots.Add(pnn2);

            Dot pnn3 = new Dot();
            pnn3.X = L1[iCatch].LHLines[L1[iCatch].LHLines.Count - 1].X - this.ClientSize.Width / (3360 / 15);
            pnn3.Y = L1[iCatch].LHLines[L1[iCatch].LHLines.Count - 1].Y - this.ClientSize.Height / (1975 / 8);
            pnn3.W = this.ClientSize.Width / (3360 / 30);
            pnn3.H = this.ClientSize.Height / (1975 / 30);
            L1[iCatch].LHLines[L1[iCatch].LHLines.Count - 1].LDots.Add(pnn3);

            L1[iCatch].LHLines[L1[iCatch].LHLines.Count - 1].BlueTaken = true;
        }

        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < L1.Count - 1; i++)
            {
                int fT = 1;
                if (e.X >= L1[i].X && e.X <= L1[i].X + L1[i].W && e.Y >= L1[i].Y + 10 && e.Y <= L1[i].Y + L1[i].H - 10)
                {
                    for(int j = 0; j < L1[i].LHLines.Count; j++)
                    {
                        if(e.Y >= L1[i].LHLines[j].Y && e.Y <= L1[i].LHLines[j].Y + L1[i].LHLines[j].H && e.X >= L1[i].LHLines[j].X - L1[i].W && e.X <= L1[i].LHLines[j].X + 5)
                        {
                            fT = 0;
                            break;
                        }
                    }
                    if (fT == 1 && !Finish)
                    {
                        iDrag = true;
                        prevX = e.X;
                        prevY = e.Y;
                        SingleLine pnn = new SingleLine();
                        pnn.X1 = e.X;
                        pnn.Y1 = e.Y;
                        pnn.X2 = e.X;
                        pnn.Y2 = e.Y;
                        L5.Add(pnn);
                        iCatch = i;
                        break;
                    }
                }
            }
        }

        void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            label4.Text = "X = " + e.X + "  Y = " + e.Y;

            if(iDrag)
            {
                int dx = e.X - prevX;
                int dy = e.Y - prevY;
                if(dx < 0)
                {
                    dx *= -1;
                }
                if(dy < 0)
                {
                    dy *= -1;
                }

                if(e.X > prevX)
                {
                    L5[0].X2 += dx;
                    prevX = e.X;
                }
                if (e.X < prevX)
                {
                    L5[0].X2 -= dx;
                    prevX = e.X;
                }
                if(e.Y > prevY)
                {
                    L5[0].Y2 += dy;
                    prevY = e.Y;
                }
                if(e.Y < prevY)
                {
                    L5[0].Y2 -= dy;
                    prevY = e.Y;
                }
               
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        }



        void CreateActors()
        {
            RR = new Random();
            int ax = 50;
            for (int i = 0; i < RR.Next(2, 10); i++)
            {
                VLine pnn = new VLine();
                pnn.X = ax += this.ClientSize.Width / (3360 / 300);
                pnn.Y = this.ClientSize.Height / (1975 / 300); ;
                pnn.W = this.ClientSize.Width / (3360 / 15);
                pnn.H = this.ClientSize.Height / (1975 / 700);
                L1.Add(pnn);
            }

            for (int i = 0; i < L1.Count - 1; i++)
            {
                for (int t = 0; t < 9999; t++)
                {
                    RR2 = new Random();
                }
                for (int j = 0; j < RR2.Next(1, 3); j++)
                {
                    for (int t = 0; t < 9999; t++)
                    {
                        RR3 = new Random();
                    }
                    HLine pnn = new HLine();
                    pnn.X = L1[i].X + L1[i].W;
                    pnn.Y = RR3.Next(L1[i].Y + 50, L1[i].Y + L1[i].H - 50);
                    pnn.W = L1[i + 1].X - L1[i].X - L1[i].W;
                    pnn.H = this.ClientSize.Height / (1975 / 15);
                   
                    L1[i].LHLines.Add(pnn);
                    L1[i + 1].LHLines.Add(pnn);
                    L1[i].LHLines[j].BlueTaken = false;
                }
            }

            for(int i = 0; i < L1.Count; i++)
            {
                for(int j = 0; j < L1[i].LHLines.Count; j++)
                {
                    if (!L1[i].LHLines[j].BlueTaken)
                    {
                        Dot pnn = new Dot();
                        pnn.X = L1[i].LHLines[j].X + L1[i].LHLines[j].W - this.ClientSize.Width / (3360 / 15);
                        pnn.Y = L1[i].LHLines[j].Y - this.ClientSize.Height / (1975 / 8); ;
                        pnn.W = this.ClientSize.Width / (3360 / 30);
                        pnn.H = this.ClientSize.Height / (1975 / 30);
                        L1[i].LHLines[j].LDots.Add(pnn);

                        pnn = new Dot();
                        pnn.X = L1[i].LHLines[j].X - this.ClientSize.Width / (3360 / 15);
                        pnn.Y = L1[i].LHLines[j].Y - this.ClientSize.Height / (1975 / 8);
                        pnn.W = this.ClientSize.Width / (3360 / 30);
                        pnn.H = this.ClientSize.Height / (1975 / 30);
                        L1[i].LHLines[j].LDots.Add(pnn);

                        L1[i].LHLines[j].BlueTaken = true;
                    }
                }
            }

            for(int i = 0; i < 1; i++)
            {
                Tom pnn = new Tom();
                RR4 = new Random();
                pnn.im = new Bitmap("tom.bmp");
                pnn.im.MakeTransparent(Color.Red);
                pnn.X = RR4.Next(L1[0].X, L1[L1.Count - 1].X);
                int Tx = 0;
                for(int j = 0; j < L1.Count; j++)
                {
                    if(pnn.X < L1[j].X + 50)
                    {
                        Tx = L1[j].X;
                        TomLine = j;
                        break;
                    }
                }
                pnn.X = Tx - pnn.im.Width / 2;
                pnn.Y = L1[0].Y - pnn.im.Height;
                L3.Add(pnn);
            }

            for (int i = 0; i < 1; i++)
            {
                Jerry pnn = new Jerry();
                RR5 = new Random();
                pnn.im = new Bitmap("jerry.bmp");
                pnn.im.MakeTransparent(Color.Red);
                for (int k = 0; k < 999; k++)
                {
                    pnn.X = RR5.Next(L1[0].X + 51, L1[L1.Count - 1].X + 300);
                }
                
                int Tx = 0;
                for (int j = 0; j < L1.Count; j++)
                {
                    if (pnn.X > L1[j].X + 50)
                    {
                        Tx = L1[j].X;
                        JerryLine = j;
                    }
                }
                pnn.X = Tx - pnn.im.Width / 2;
                pnn.Y = L1[0].Y + L1[0].H;
                L4.Add(pnn);
            }


        }

        void DrawScene(Graphics g)
        {
            g.Clear(Color.YellowGreen);

            for(int i = 0; i < L1.Count; i++)
            {
                g.FillRectangle(Brushes.Black, L1[i].X, L1[i].Y, L1[i].W, L1[i].H);

                for(int j = 0; j < L1[i].LHLines.Count; j++)
                {
                    g.FillRectangle(Brushes.Red, L1[i].LHLines[j].X, L1[i].LHLines[j].Y, L1[i].LHLines[j].W, L1[i].LHLines[j].H);

                    for (int k = 0; k < L1[i].LHLines[j].LDots.Count; k++)
                    {
                        g.FillEllipse(Brushes.Blue, L1[i].LHLines[j].LDots[k].X, L1[i].LHLines[j].LDots[k].Y, L1[i].LHLines[j].LDots[k].W, L1[i].LHLines[j].LDots[k].H);
                    }
                }
            }
            
            for (int i = 0; i < L3.Count; i++)
            {
                g.DrawImage(L3[0].im, L3[0].X, L3[0].Y);
            }

            for(int i = 0; i < L4.Count; i++)
            {
                g.DrawImage(L4[0].im, L4[0].X, L4[0].Y);
            }

            Pen p = new Pen(Color.DarkRed, this.ClientSize.Width / (3360 / 15));

            for(int i = 0; i < L5.Count; i++)
            {
                g.DrawLine(p, L5[0].X1, L5[0].Y1, L5[0].X2, L5[0].Y2);
            }
        }

        void DrawDouble(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }



    }
}


