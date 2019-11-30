using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Triangle.Models;

namespace WPF_Triangle.Methods
{
    public class Methods
    {
        public List<Triangle> GetListOfTriangle(List<int> listOfEdge)
        {
            List<Triangle> listOfEdgesToTriangle = new List<Triangle>();
            if (listOfEdge.Count > 2)
            {
                for (int i = 0; i < listOfEdge.Count(); i++)
                    for (int j = i + 1; j < listOfEdge.Count(); j++)
                        for (int k = j + 1; k < listOfEdge.Count(); k++)
                        {
                            if (listOfEdge[i] + listOfEdge[j] > listOfEdge[k] && listOfEdge[i] + listOfEdge[k] > listOfEdge[j] && listOfEdge[k] + listOfEdge[j] > listOfEdge[i])
                            {
                                Triangle triangle = new Triangle();
                                triangle.sideA = listOfEdge[i];
                                triangle.sideB = listOfEdge[j];
                                triangle.sideC = listOfEdge[k];
                                listOfEdgesToTriangle.Add(triangle);
                            }
                        }
            }

            return listOfEdgesToTriangle;
        }
    }
}
