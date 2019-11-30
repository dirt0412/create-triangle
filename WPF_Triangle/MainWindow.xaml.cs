using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Triangle.Models;

namespace WPF_Triangle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<int> listOfEdge = new List<int>();
        List<Triangle> listOfEdgesToTriangle = new List<Triangle>();

        Methods.Methods methods = new Methods.Methods();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "D:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            canvasToTriangles.Children.Clear();

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            using (StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default))
                            {
                                listOfEdge.Clear();
                                while (sr.Peek() >= 0)
                                {
                                    string text = sr.ReadLine();
                                    int temp = 0;
                                    int.TryParse(text, out temp);
                                    listOfEdge.Add(temp);                                  
                                }
                            }
                        }
                    }
                    listOfEdgesToTriangle = methods.GetListOfTriangle(listOfEdge);

                    listBox.Items.Clear();
                    if (listOfEdgesToTriangle.Count == 0)
                        MessageBox.Show("Brak rozwiązań");
                    else
                    {
                        List<Triangle> listOfEdgesToTriangleAfterResize = new List<Triangle>();
                        double resultMaxTemp = 0, resultMax = 0;
                        foreach (var item in listOfEdgesToTriangle)
                        {
                            listBox.Items.Add(item.sideA+",   "+item.sideB+",   "+item.sideC);
                            resultMaxTemp = Math.Max(item.sideA, Math.Max(item.sideB, item.sideC)) / canvasToTriangles.Height*2;
                            if (resultMaxTemp > resultMax)
                                resultMax = resultMaxTemp;
                        }
                        if (resultMax > 1)//resize all edges of triangle
                        {
                            double resultToDivide = Math.Ceiling(resultMax);
                            foreach (var item in listOfEdgesToTriangle)
                            {
                                Triangle triangle = new Triangle();
                                triangle.sideA = item.sideA / resultToDivide;
                                triangle.sideB = item.sideB / resultToDivide;
                                triangle.sideC = item.sideC / resultToDivide;
                                listOfEdgesToTriangleAfterResize.Add(triangle);
                            }
                            listOfEdgesToTriangle = listOfEdgesToTriangleAfterResize;
                        }
                        
                        double x1 = 0, y1 = 70, y2 = 70;

                        foreach (var item in listOfEdgesToTriangle)
                        {
                            double cos_C = (Math.Pow(item.sideA, 2) + Math.Pow(item.sideB, 2) - Math.Pow(item.sideC, 2)) / (2 * item.sideA * item.sideB);
                            double angleAB = Math.Acos(cos_C)*(180/Math.PI);
                            double cos_B = (Math.Pow(item.sideA, 2) + Math.Pow(item.sideC, 2) - Math.Pow(item.sideB, 2)) / (2 * item.sideA * item.sideC);
                            double angleAC = Math.Acos(cos_B) * (180 / Math.PI);
                            double cos_A = (Math.Pow(item.sideC, 2) + Math.Pow(item.sideB, 2) - Math.Pow(item.sideA, 2)) / (2 * item.sideC * item.sideB);
                            double angleBC = Math.Acos(cos_A) * (180 / Math.PI);
                            if (item.sideA == item.sideB && item.sideB == item.sideC)
                            {
                                Line lineA = new Line();
                                lineA.Stroke = Brushes.Black;
                                lineA.X1 = x1;
                                lineA.Y1 = y1;
                                lineA.X2 = item.sideA;
                                lineA.Y2 = y2;
                                lineA.StrokeThickness = 2;
                                canvasToTriangles.Children.Add(lineA);

                                Line lineB = new Line();
                                lineB.Stroke = Brushes.Green;
                                lineB.X1 = x1;
                                lineB.Y1 = y1;
                                lineB.X2 = (item.sideA/2);
                                lineB.Y2 = y1+Math.Sqrt(Math.Pow(item.sideA/2,2) + Math.Pow(item.sideC,2)  );
                                lineB.StrokeThickness = 2;
                                canvasToTriangles.Children.Add(lineB);

                                Line lineC = new Line();
                                lineC.Stroke = Brushes.Red;
                                lineC.X1 = item.sideA;
                                lineC.Y1 = y2;
                                lineC.X2 =  (item.sideA / 2); 
                                lineC.Y2 = y1+Math.Sqrt(Math.Pow(item.sideA / 2, 2) + Math.Pow(item.sideC, 2)); ;
                                canvasToTriangles.Children.Add(lineC);
                            }
                            else
                            {


                                Line lineA = new Line();
                                lineA.Stroke = Brushes.Black;
                                lineA.X1 = x1;
                                lineA.Y1 = y1;
                                lineA.X2 = item.sideA;
                                lineA.Y2 = y2;
                                lineA.StrokeThickness = 2;
                                canvasToTriangles.Children.Add(lineA);

                                Line lineB = new Line();
                                lineB.Stroke = Brushes.Green;
                                lineB.X1 = x1;
                                lineB.Y1 = y1;
                                //lineB.X2 = x1 + (Math.Cos(angleAB) * item.sideC) - (Math.Sin(angleAB) * item.sideB);
                                //lineB.Y2 = y1 + (Math.Sin(angleAB) * item.sideC) + (Math.Cos(angleAB) * item.sideB);
                                lineB.X2 = lineA.X2 + (Math.Cos(angleAC) * item.sideC);
                                lineB.Y2 = lineA.Y2 + (Math.Sin(angleAC) * item.sideC);
                                lineB.StrokeThickness = 2;
                                canvasToTriangles.Children.Add(lineB);

                                Line lineC = new Line();
                                lineC.Stroke = Brushes.Red;
                                lineC.X1 = item.sideA;
                                lineC.Y1 = y2;
                                //lineC.X2 = x1 + (Math.Cos(angleAB) * item.sideC) - (Math.Sin(angleAB) * item.sideB);
                                //lineC.Y2 = y1 + (Math.Sin(angleAB) * item.sideC) + (Math.Cos(angleAB) * item.sideB);
                                lineC.X2 = lineA.X2 + (Math.Cos(angleAC) * item.sideC);
                                lineC.Y2 = lineA.Y2 + (Math.Sin(angleAC) * item.sideC);
                                canvasToTriangles.Children.Add(lineC);
                            }

                            //x1 += 15;
                            //x2 += 15;
                            //y1 += 15;
                            //y2 += 15;
                        }
                        
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

       


    }
}
