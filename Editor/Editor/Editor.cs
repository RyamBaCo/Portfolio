using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.IO;

namespace Editor
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();
        }

        private void buttonOpenLetterGraphics_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogLevels.ShowDialog() == DialogResult.OK)
            {
                textBoxLetterGraphicsPath.Text = folderBrowserDialogLevels.SelectedPath;

                string[] files = { "back.png", "letters.png", "front.png", "frontMask.png", "dot.png" };
                PictureBox[] boxes = { pictureBoxBackGraphics, pictureBoxLetterGraphics, pictureBoxFrontGraphics, pictureBoxFrontMaskGraphics, pictureBoxDot };

                for (int i = 0; i < 5; i++)
                {
                    string path = Path.Combine(folderBrowserDialogLevels.SelectedPath, files[i]);
                    if (!File.Exists(path))
                    {
                        MessageBox.Show("Kein " + path + "!", "Ungültige Pfadstruktur");
                        continue;
                    }
                    boxes[i].Image = new Bitmap(path);
                }
                /*
                if (!File.Exists(Path.Combine(folderBrowserDialogLevels.SelectedPath, "misc.xml")))
                    saveMisc();
                else
                    loadMisc();
                */
                buttonSave.Enabled = true;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
                if (c is Button)
                    c.Enabled = false;
            backgroundWorkerWriteXML.RunWorkerAsync();
        }

        private void saveLetterPositions()
        {
            List<Point> points = new List<Point>();
            Bitmap bmp = pictureBoxLetterGraphics.Image as Bitmap;

            for (int x = 0; x < bmp.Width; x += Convert.ToInt32(numericUpDownAccuracy.Value))
                for (int y = 0; y < bmp.Height; y += Convert.ToInt32(numericUpDownAccuracy.Value))
                {
                    Color pixel = bmp.GetPixel(x, y);
                    if ((pixel.A > 120))// && (pixel.GetBrightness() > 0.9f))
                        //&& (pixel.R + pixel.G + pixel.B < 600))
                        points.Add(new Point(x, y));
                }
            /*
             * is working but a little bit too time consuming ;)
             * 
            Point maxFailure = new Point(1, 1); // for possible "gaps" (x- and y-direction) in bad gfx
            List<Point> additionalPoints = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                for (int x = -maxFailure.X; x <= maxFailure.X; x++)
                    for (int y = -maxFailure.Y; y <= maxFailure.Y; y++)
                    {
                        Point newPoint = new Point(points[i].X + x, points[i].Y + y);
                        if (!points.Contains(newPoint))
                            additionalPoints.Add(newPoint);
                    }
                backgroundWorkerWriteXML.ReportProgress(Convert.ToInt32(Convert.ToDouble(i + 1) / points.Count));
            }
            foreach (Point p in additionalPoints)
                points.Add(p);
            */

/*            XmlTextWriter textWriter = new XmlTextWriter(Path.Combine(folderBrowserDialogLevels.SelectedPath, "letters.xml"), null);
            textWriter.Formatting = Formatting.Indented;
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("Letters");*/
            int maximum = points.Count;

            Graphics gfx = Graphics.FromImage(bmp);
            Pen red = new Pen(Color.Red);
            List<Rectangle> letterRectangles = new List<Rectangle>();

            while (points.Count != 0)
            {
                Point p = points[0];
                Point leftUp = new Point(int.MaxValue, int.MaxValue);
                Point rightDown = new Point(int.MinValue, int.MinValue);
                seekNeigbors(ref p, ref points, ref leftUp, ref rightDown, maximum);

                int width = rightDown.X - leftUp.X;
                int height = rightDown.Y - leftUp.Y;
                if ((width > 0) ||
                    (height > 0))
                {/*
                    textWriter.WriteStartElement("Letter");
                    textWriter.WriteAttributeString("X", XmlConvert.ToString(leftUp.X));
                    textWriter.WriteAttributeString("Y", XmlConvert.ToString(leftUp.Y));
                    textWriter.WriteAttributeString("Width", XmlConvert.ToString(width + 1));
                    textWriter.WriteAttributeString("Height", XmlConvert.ToString(height + 1));
                    textWriter.WriteEndElement();*/
                    Rectangle newRectangle = new Rectangle(leftUp.X, leftUp.Y, width + 1, height + 1);
                    bool toAdd = true;
                    for(int i = 0; i < letterRectangles.Count; i++)
                        if (letterRectangles[i].Contains(newRectangle))
                            toAdd = false;
                        else if(newRectangle.Contains(letterRectangles[i]))
                        {
                            letterRectangles.RemoveAt(i);
                            i--;
                        }
                    if (toAdd)
                    {
                        letterRectangles.Add(newRectangle);
                        gfx.DrawRectangle(red, newRectangle);
                    }
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{\"0\":{\"x\":0,\"y\":0,\"r\":10},");
            int index = 1;
            foreach (Rectangle rectangle in letterRectangles)
            {
                int width = rectangle.Width;
                int height = rectangle.Height;

                if (width % 2 == 1)
                    ++width;
                if (height % 2 == 1)
                    ++height;
                stringBuilder.Append("\"" + index + "\":{\"x\":" + (rectangle.X + width / 2) + ",\"y\":" + (rectangle.Y + height / 2) + ",\"w\":" + width + ",\"h\":" + height + "},");
                ++index;
/*                textWriter.WriteStartElement("Letter");
                textWriter.WriteAttributeString("X", XmlConvert.ToString(rectangle.X));
                textWriter.WriteAttributeString("Y", XmlConvert.ToString(rectangle.Y));
                textWriter.WriteAttributeString("Width", XmlConvert.ToString(rectangle.Width));
                textWriter.WriteAttributeString("Height", XmlConvert.ToString(rectangle.Height));
                textWriter.WriteEndElement();*/
            }

            stringBuilder.Length -= 1;
            stringBuilder.Append("}");
            gfx.Dispose();
            red.Dispose();

            using (StreamWriter outfile = new StreamWriter(Path.Combine(folderBrowserDialogLevels.SelectedPath, "bodies.json")))
            {
                outfile.Write(stringBuilder.ToString());
            }

          /*  textWriter.WriteFullEndElement();
            textWriter.Close();*/
        }

        private struct ColorPoint
        {
            public Color m_color;
            public Point m_point;
            public ColorPoint(Color color, Point point)
            {
                m_color = Color.FromArgb(255, color); // only save rgb values
                m_point = point;
            }
        }

        private void saveElementPositions()
        {
            List<ColorPoint> points = new List<ColorPoint>();
            Bitmap bmp = pictureBoxFrontMaskGraphics.Image as Bitmap;

            for (int x = 0; x < bmp.Width; x += Convert.ToInt32(numericUpDownAccuracy.Value))
                for (int y = 0; y < bmp.Height; y += Convert.ToInt32(numericUpDownAccuracy.Value))
                {
                    Color pixel = bmp.GetPixel(x, y);
                    if (pixel.A == 255)
                        points.Add(new ColorPoint(pixel, new Point(x, y)));
                }

            XmlTextWriter textWriter = new XmlTextWriter(Path.Combine(folderBrowserDialogLevels.SelectedPath, "ground.xml"), null);
            textWriter.Formatting = Formatting.Indented;
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("Elements");

            Graphics gfx = Graphics.FromImage(bmp);
            Pen red = new Pen(Color.Blue);
            int maximum = points.Count;

            while (points.Count > 0)
            {
                Color currentColor = points[0].m_color;
                bool isFinishingElement = (currentColor.R == 0) && (currentColor.G == 0) && (currentColor.B == 0);
//                red.Color = Color.FromArgb(255, 255 - currentColor.R, 255 - currentColor.G, 255 - currentColor.B);

                List<Point> upPoints = new List<Point>();
                List<Point> downPoints = new List<Point>();
                List<Point> leftPoints = new List<Point>();
                List<Point> rightPoints = new List<Point>();
// fixed hook points will stay there forever while "dynamic" hook points will be destroyed on player's contact
                Point lastPosition = points[0].m_point;
                List<Point> fixedHookPoints = new List<Point>();
                List<Point> dynamicHookPoints = new List<Point>();

                int leftest = int.MaxValue;
                int rightest = int.MinValue;
                int uppest = int.MaxValue;
                int downest = int.MinValue;

                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].m_color == currentColor)
                    {
                        Point currentPoint = points[i].m_point;

                        // search for hook-points
                        Point firstHookPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                        for (int j = i + 1; j < points.Count; j++)
                            if ((points[j].m_point == firstHookPoint)
                                 && (points[j].m_color != points[i].m_color))
                            {
                                // sort of white --> fixed, otherwise dynamic!
                                if (points[j].m_color.GetBrightness() >= 0.5f)
                                    fixedHookPoints.Add(points[j].m_point);
                                else
                                    dynamicHookPoints.Add(points[j].m_point);
                                points.Remove(points[j]);
                                break;
                            }

                        lastPosition = currentPoint;
                        if (currentPoint.X <= leftest)
                        {
                            if(currentPoint.X < leftest)
                                leftPoints.Clear();
                            leftest = currentPoint.X;
                            leftPoints.Add(currentPoint);
                        }

                        if (currentPoint.X >= rightest)
                        {
                            if (currentPoint.X > rightest)
                                rightPoints.Clear();
                            rightest = currentPoint.X;
                            rightPoints.Add(currentPoint);
                        }

                        if (currentPoint.Y <= uppest)
                        {
                            if (currentPoint.Y < uppest)
                                upPoints.Clear();
                            uppest = currentPoint.Y;
                            upPoints.Add(currentPoint);
                        }

                        if (currentPoint.Y >= downest)
                        {
                            if (currentPoint.Y > downest)
                                downPoints.Clear();
                            downest = currentPoint.Y;
                            downPoints.Add(currentPoint);
                        }

                        points.Remove(points[i]);
                        i--;
                    }

                    backgroundWorkerWriteXML.ReportProgress(Convert.ToInt32((maximum - points.Count) / Convert.ToDouble(maximum) * 100));
                }

                Point leftUp = leftPoints[0];
                Point rightUp = upPoints[0];
                Point leftDown = downPoints[0];
                Point rightDown = rightPoints[0];

                for (int i = 1; i < leftPoints.Count; i++)
                    if (leftPoints[i].Y < leftUp.Y)
                        leftUp.Y = leftPoints[i].Y;
                for (int i = 1; i < upPoints.Count; i++)
                    if (upPoints[i].X > rightUp.X)
                        rightUp.X = upPoints[i].X;
                for (int i = 1; i < rightPoints.Count; i++)
                    if (rightPoints[i].Y > rightDown.Y)
                        rightDown.Y = rightPoints[i].Y;
                for (int i = 1; i < downPoints.Count; i++)
                    if (downPoints[i].X < leftDown.X)
                        leftDown.X = downPoints[i].X;

                if(leftUp == leftDown)
                gfx.DrawPolygon(red, new Point[] { leftUp, leftDown, rightDown, rightUp }); 

                // calculate the points into box2d-values for vertices
                // would have turned shorter with the xna vector 2 class, but i didn't want any urgent dependencies to the xna-framework
                Point position = new Point((leftUp.X + rightUp.X + leftDown.X + rightDown.X) / 4, (leftUp.Y + rightUp.Y + leftDown.Y + rightDown.Y) / 4);
                leftUp.X -= position.X;
                leftUp.Y -= position.Y;
                rightUp.X -= position.X;
                rightUp.Y -= position.Y;
                leftDown.X -= position.X;
                leftDown.Y -= position.Y;
                rightDown.X -= position.X;
                rightDown.Y -= position.Y;
                textWriter.WriteStartElement("Element");
                textWriter.WriteStartElement("Position");
                textWriter.WriteAttributeString("X", XmlConvert.ToString(position.X));
                textWriter.WriteAttributeString("Y", XmlConvert.ToString(position.Y));
                textWriter.WriteEndElement();

                if (isFinishingElement)
                {
                    textWriter.WriteStartElement("IsFinishingElement");
                    textWriter.WriteEndElement();
                }

                foreach (Point p in fixedHookPoints)
                {
                    textWriter.WriteStartElement("FixedHook");
                    textWriter.WriteAttributeString("X", XmlConvert.ToString(p.X));
                    textWriter.WriteAttributeString("Y", XmlConvert.ToString(p.Y));
                    textWriter.WriteEndElement();
                }

                foreach (Point p in dynamicHookPoints)
                {
                    textWriter.WriteStartElement("DynamicHook");
                    textWriter.WriteAttributeString("X", XmlConvert.ToString(p.X));
                    textWriter.WriteAttributeString("Y", XmlConvert.ToString(p.Y));
                    textWriter.WriteEndElement();
                }

                // store the vertex points
                textWriter.WriteStartElement("Vertices");
                textWriter.WriteStartElement("Point");
                textWriter.WriteAttributeString("X", XmlConvert.ToString(rightUp.X));
                textWriter.WriteAttributeString("Y", XmlConvert.ToString(rightUp.Y));
                textWriter.WriteEndElement();
                textWriter.WriteStartElement("Point");
                textWriter.WriteAttributeString("X", XmlConvert.ToString(rightDown.X));
                textWriter.WriteAttributeString("Y", XmlConvert.ToString(rightDown.Y));
                textWriter.WriteEndElement();
                textWriter.WriteStartElement("Point");
                textWriter.WriteAttributeString("X", XmlConvert.ToString(leftDown.X));
                textWriter.WriteAttributeString("Y", XmlConvert.ToString(leftDown.Y));
                textWriter.WriteEndElement();
                textWriter.WriteStartElement("Point");
                textWriter.WriteAttributeString("X", XmlConvert.ToString(leftUp.X));
                textWriter.WriteAttributeString("Y", XmlConvert.ToString(leftUp.Y));
                textWriter.WriteEndElement();
                textWriter.WriteEndElement();

                textWriter.WriteEndElement();
            }

            gfx.Dispose();
            red.Dispose();

            textWriter.WriteFullEndElement();
            textWriter.Close();
        }

        private void saveMisc()
        {
            Bitmap bmp = pictureBoxDot.Image as Bitmap;

            // dot is a circle with centerpoint in the middle of the gfx so we need only the radius as size
            int radius = bmp.Width;
            for (int x = 0; x < bmp.Width; x++)
                if (bmp.GetPixel(x, bmp.Height / 2).A == 0)
                    radius--;
            radius /= 2;

            XmlTextWriter textWriter = new XmlTextWriter(Path.Combine(folderBrowserDialogLevels.SelectedPath, "misc.xml"), null);
            textWriter.Formatting = Formatting.Indented;
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("Parameters");
            textWriter.WriteStartElement("Dot");
            textWriter.WriteAttributeString("X", XmlConvert.ToString(pictureBoxDot.Location.X + pictureBoxDot.Size.Width / 2 + panelGraphics.HorizontalScroll.Value));
            textWriter.WriteAttributeString("Y", XmlConvert.ToString(pictureBoxDot.Location.Y + pictureBoxDot.Size.Height / 2 + panelGraphics.VerticalScroll.Value));
            textWriter.WriteAttributeString("Radius", XmlConvert.ToString(radius));
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("Goal");
            textWriter.WriteAttributeString("LettersPercentage", textBoxLettersNeeded.Text);
            textWriter.WriteAttributeString("TimeSeconds", textBoxTime.Text);
            textWriter.WriteEndElement();
            textWriter.WriteFullEndElement();
            textWriter.Close();
        }

        private void loadMisc()
        {
            XmlTextReader reader = new XmlTextReader(Path.Combine(folderBrowserDialogLevels.SelectedPath, "misc.xml"));
            Point position = new Point();

            while (reader.Read())
                if (reader.IsStartElement())
                {
                    if (reader.Name == "Dot")
                        while (reader.MoveToNextAttribute())
                        {
                            if (reader.Name == "X")
                                position.X = XmlConvert.ToInt32(reader.Value);
                            else if (reader.Name == "Y")
                                position.Y = XmlConvert.ToInt32(reader.Value);
                        }
                    else if (reader.Name == "Goal")
                        while (reader.MoveToNextAttribute())
                            if (reader.Name == "LettersPercentage")
                                trackBarLettersNeeded.Value = XmlConvert.ToInt32(reader.Value);
                            else if (reader.Name == "TimeSeconds")
                                textBoxTime.Text = reader.Value;
                }

            position.X -= pictureBoxDot.Size.Width / 2;
            position.Y -= pictureBoxDot.Size.Height / 2;
            pictureBoxDot.Location = position;
        }

        private void seekNeigbors(ref Point p, ref List<Point> neighborPoints, ref Point leftUp, ref Point rightDown, int maximum)
        {
            if(p.X < leftUp.X)
                leftUp.X = p.X;
            if(p.Y < leftUp.Y)
                leftUp.Y = p.Y;

            if(p.X > rightDown.X)
                rightDown.X = p.X;
            if(p.Y > rightDown.Y)
                rightDown.Y = p.Y;

            neighborPoints.Remove(p);
            backgroundWorkerWriteXML.ReportProgress(Convert.ToInt32(100 * Convert.ToDouble(maximum - neighborPoints.Count) / maximum));

            int accuracy = Convert.ToInt32(numericUpDownAccuracy.Value);
            for (int x = -accuracy; x <= accuracy; x += accuracy)
                for (int y = -accuracy; y <= accuracy; y += accuracy)
                {
                    Point neighborPoint = new Point(p.X + x, p.Y + y);

                    if (neighborPoints.Contains(neighborPoint))
                        seekNeigbors(ref neighborPoint, ref neighborPoints, ref leftUp, ref rightDown, maximum);
                }
        }

        private void backgroundWorkerWriteXML_DoWork(object sender, DoWorkEventArgs e)
        {
            if (radioButtonShowLetters.Checked)
                saveLetterPositions();
            else if (radioButtonShowFrontMask.Checked)
                saveElementPositions();
            else if (radioButtonShowBack.Checked)
                saveMisc();
        }

        private void backgroundWorkerWriteXML_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (Control c in this.Controls)
                if (c is Button)
                    c.Enabled = true;
            progressBarSave.Value = 0;
        }

        private void backgroundWorkerWriteXML_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarSave.Value = e.ProgressPercentage;
        }

        private void radioButtonShowBack_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxBackGraphics.Visible = true;
            pictureBoxLetterGraphics.Visible = false;
            pictureBoxFrontMaskGraphics.Visible = false;
            pictureBoxFrontGraphics.Visible = false;
        }

        private void radioButtonShowLetters_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxBackGraphics.Visible = false;
            pictureBoxLetterGraphics.Visible = true;
            pictureBoxFrontMaskGraphics.Visible = false;
            pictureBoxFrontGraphics.Visible = false;
        }

        private void radioButtonShowFrontMask_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxBackGraphics.Visible = false;
            pictureBoxLetterGraphics.Visible = false;
            pictureBoxFrontMaskGraphics.Visible = true;
            pictureBoxFrontGraphics.Visible = false;    
        }

        private void radioButtonShowFront_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxBackGraphics.Visible = false;
            pictureBoxLetterGraphics.Visible = false;
            pictureBoxFrontMaskGraphics.Visible = false;
            pictureBoxFrontGraphics.Visible = true;
        }

        private void pictureBoxLetterGraphics_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBoxDot.Location = new Point(e.X - panelGraphics.HorizontalScroll.Value, e.Y - panelGraphics.VerticalScroll.Value);
        }

        private void trackBarLettersNeeded_ValueChanged(object sender, EventArgs e)
        {
            textBoxLettersNeeded.Text = trackBarLettersNeeded.Value.ToString();
        }

        private void textBoxTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar))
                e.Handled = true;
        }
    }
}
