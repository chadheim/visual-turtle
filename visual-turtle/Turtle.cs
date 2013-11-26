using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace visual_turtle
{
    class Turtle
    {
        Canvas canvas;
        Point pos;
        Point dir;

        public Turtle(Canvas canvas)
        {
            this.canvas = canvas;
            pos.X = 200;
            pos.Y = 200;
            dir.X = 0;
            dir.Y = -1;
        }

        private void DrawLine(double x1, double y1, double x2, double y2, Color color)
        {
            Line line = new Line()
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = new SolidColorBrush(color)
            };
            canvas.Children.Add(line);
        }

        public void DrawTurtle()
        {
            Point tip = pos;
            tip.X += dir.X * 5;
            tip.Y += dir.Y * 5;

            Point tail = pos;
            tail.X -= dir.X * 5;
            tail.Y -= dir.Y * 5;

            DrawLine(tip.X, tip.Y, tail.X + (dir.Y * 5), tail.Y + (dir.X * 5), Colors.Yellow);
            DrawLine(tip.X, tip.Y, tail.X - (dir.Y * 5), tail.Y - (dir.X * 5), Colors.Yellow);
        }

        public void MoveForward()
        {
            Point dst = pos;
            dst.X += dir.X * 25;
            dst.Y += dir.Y * 25;

            DrawLine(pos.X, pos.Y, dst.X, dst.Y, Colors.Blue);

            pos = dst;
        }

        private void Turn(int d)
        {
            if (dir.X != 0)
            {
                dir.Y = dir.X * d;
                dir.X = 0;
            }
            else
            {
                dir.X = dir.Y * -d;
                dir.Y = 0;
            }
        }

        public void TurnLeft()
        {
            Turn(-1);
        }

        public void TurnRight()
        {
            Turn(1);
        }
    }
}
