using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_8_task_20
{
    class Program
    {
        static int[,] DeleteEdge(int[,] mas, int edge, ref int edges, int tops)
        {//удаление столбца (удаление ребра)
            int[,] masNew = new int[tops, edges - 1];
            int k = 0;
            for (int i=0; i<edges; i++)
            {
                if (i!=edge)
                {
                    for (int j = 0; j < tops; j++)
                        masNew[j, k] = mas[j, i];
                    k++;
                }
            }
            edges = edges - 1;
            return masNew;
        }

        static int FirstOneInEdge(int[,] mas, int tops, int edge)
        {//находит первую единицу в столбце (первую вершину при ребре)
            for (int i=1; i<tops; i++)
            {
                if (mas[i, edge] == 1) return i;
            }
            return 0;
        }

        static int SecondOneInEdge(int[,] mas, int tops, int edge)
        {//находит вторую единицу в столбце (вторую вершину при ребре)
            int k = 0;
            for (int i = 0; i < tops; i++)
            {
                if (mas[i, edge] == 1) k++;
                if (k == 2) return i;
            }
            return 0;
        }

        static bool IsFirstOneInEdge(int[,] mas, int tops, int edge, int top)
        {//проверяет, является ли данная вершина первой в матрице в данном ребре
            for (int i = 0; i < tops; i++)
            {
                if ((mas[i, edge] == 1) && (i == top)) return true;
                else if (mas[i, edge] == 1) return false;
            }
            return false;
        }

        static int[,] MakeMas(int[,] mas, int[,] masNew, int tops, int edges)
        {//функция, переписывающая элементы одного массива в новый другой
            for (int i = 0; i < tops; i++)
            {
                for (int j = 0; j < edges; j++)
                    masNew[i, j] = mas[i, j];
            }
            return masNew;
        }

        static bool FindWay(ref int[,] mas, int tops, int edges, int point1, int point2)
        {//рекурсивная функция, выясняющая, можно ли добраться от вершины point1 до вершины point2
            for (int i=0; i<edges; i++)
            {
                if (mas[point1, i] == 1)
                {
                    if (IsFirstOneInEdge(mas, tops, i, point1))
                    {
                        if (SecondOneInEdge(mas, tops, i) == point2) return true;
                        else
                        {                                                        
                            int[,] masNew = new int[tops, edges];
                            masNew = DeleteEdge(mas, i, ref edges, tops);
                            if (FindWay(ref masNew, tops, edges, SecondOneInEdge(mas, tops, i), point2)) return true;
                            else return false;
                        }
                    }
                    else
                    {
                        if (FirstOneInEdge(mas, tops, i) == point2) return true;
                        else
                        {                           
                            int[,] masNew = new int[tops, edges];
                            masNew = DeleteEdge(mas, i, ref edges, tops);
                            if (FindWay(ref masNew, tops, edges, FirstOneInEdge(mas, tops, i), point2)) return true;
                            else return false;
                        }
                    }
                }
            }
            return false;
        }

        static bool IsBridge(int[,] mas, int edge, int tops, int edges)
        {//функция, определяющая, является ли данное ребро мостом
            int point1 = FirstOneInEdge(mas, tops, edge);
            int point2 = SecondOneInEdge(mas, tops, edge);
            mas = DeleteEdge(mas, edge, ref edges, tops);

            int pointNow = point1;

            int[,] masNew = new int[tops, edges];
            masNew = MakeMas(mas, masNew, tops, edges);
            if (FindWay(ref masNew, tops, edges, point1, point2))
                return true;
            else return false;
        }

        static void EulerСycle(int[,] mas, ref int edges, int tops, int begin)
        {//рекурсивная функция вывода на экран эйлерова цикла
            for (int i=0; i<edges; i++)
            {
                if ((mas[begin, i]==1)&&(!IsBridge(mas, i, tops, edges)))
                {
                    Console.Write(begin + " --> ");
                    if (IsFirstOneInEdge(mas, tops, i, begin))
                        begin = SecondOneInEdge(mas, tops, i);
                    else begin = FirstOneInEdge(mas, tops, i);
                    mas = DeleteEdge(mas, i, ref edges, tops);
                    EulerСycle(mas, ref edges, tops, begin);
                } 
            }
        }

        static void Main(string[] args)
        {

        }
    }
}
