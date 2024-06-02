using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM05
{
    public partial class UserControl1: UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private List<PointF> points = new List<PointF>();
        private List<PointF> convexHull = new List<PointF>();
        private const int PointRadius = 3;


        private void GenerateButton_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            points.Clear();
            convexHull.Clear();
            int n = 20;

            for (int i = 0; i < n; i++)
            {
                points.Add(new PointF(rand.Next(50, 700), rand.Next(50, 450)));
            }

            convexHull = CalculateConvexHull(points);
            this.Invalidate();
        }
        ///////////////////////////////////////////// Graham
        private List<PointF> CalculateConvexHull(List<PointF> points)
        {
            if (points.Count < 3)
                return new List<PointF>(points);

            List<PointF> hull = new List<PointF>();

            PointF start = points.OrderBy(p => p.X).First();
            PointF current = start;
            PointF endpoint;

            do
            {
                hull.Add(current);
                endpoint = points[0];

                for (int i = 1; i < points.Count; i++)
                {
                    if ((endpoint == current) || (Orientation(current, endpoint, points[i]) == -1))
                    {
                        endpoint = points[i];
                    }
                }

                current = endpoint;
            } while (endpoint != start);

            return hull;
        }

        private int Orientation(PointF p, PointF q, PointF r)
        {
            float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0;
            return (val > 0) ? 1 : -1;
        }
        //////////////////////////
        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen hullPen = new Pen(Color.Red, 2);
            Brush pointBrush = Brushes.Blue;

            foreach (var point in points)
            {
                g.FillEllipse(pointBrush, point.X - PointRadius, point.Y - PointRadius, PointRadius * 2, PointRadius * 2);
            }

            if (convexHull.Count > 1)
            {
                for (int i = 0; i < convexHull.Count; i++)
                {
                    PointF p1 = convexHull[i];
                    PointF p2 = convexHull[(i + 1) % convexHull.Count];
                    g.DrawLine(hullPen, p1, p2);
                }
            }
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            this.Width = 800;
            this.Height = 600;

            Button generateButton = new Button
            {
                Text = "Generate Points",
                Location = new Point(10, 10)
            };
            generateButton.Click += GenerateButton_Click;
            this.Controls.Add(generateButton);

            this.Paint += DrawingPanel_Paint;
        }
    }
}
