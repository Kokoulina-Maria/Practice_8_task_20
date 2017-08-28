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
            for (int i=0; i<tops; i++)
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

            int[,] masNew = new int[tops, edges];
            masNew = MakeMas(mas, masNew, tops, edges);
            if (FindWay(ref masNew, tops, edges, point1, point2))
                return false;
            else return true;
        }

        static void EulerСycle(int[,] mas, ref int edges, int tops, int begin)
        {//рекурсивная функция вывода на экран эйлерова цикла
            int ones = 0;//переменная для подсчета количетсва ребер, исходящих из данной ввершины
            for (int i = 0; i < edges; i++)//подсчитываем количество ребер, исходящих из данного ребра
                if (mas[begin, i] == 1) ones++;
            for (int i=0; i<edges; i++)
            {
                if ((mas[begin, i]==1)&&((ones==1)||(!IsBridge(mas, i, tops, edges))))
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

        static void WriteMas(int[,] mas, int tops, int edges)
        {//вывод матрицы интиденций на экран
            for (int i=0; i<tops; i++)
            {
                Console.Write(i + " ");
                for (int j = 0; j < edges; j++)
                    Console.Write(mas[i, j] + " ");
                Console.WriteLine();
            }
        }
        static bool IsEulerGraph(int[,] mas, int edges, int tops)
        {//функция, определяющая, является ли данный граф Эйлеровым

            //проверка на то, является ли граф связанным (от нулевой вершины можно добраться до всех остальных)
            for (int i = 1; i < tops; i++)
            {
                int[,] masNew = new int[tops, edges];
                masNew = MakeMas(mas, masNew, tops, edges);
                if (!FindWay(ref masNew, tops, edges, 0, i)) return false;
            }

            //проверка на то, нет ли в графе вершин с четной степенью или ни с чем не связанных вершин
            for (int i = 0; i < tops; i++)
            {
                int g = 0;
                for (int j = 0; j < edges; j++)
                    if (mas[i, j] == 1) g++;
                if ((g == 0) || (g % 2 == 1)) return false;
            }
            return true;
        }

        static int[,] Generator(ref int tops, ref int edges)
        {//генератор эйлеровых графов
            int[,] mas;
            bool ok;
            do
            {
                Random rnd = new Random();
                tops = rnd.Next(4, 20);
                edges = rnd.Next(tops, tops * (tops - 1) / 2);
                mas = new int[tops, edges];
                for (int i = 0; i < edges; i++)//заполняем матрицу случайными числами
                {
                    int oneFirst;
                    int oneSecond;
                    do//выбираем две вершины для единиц
                    {
                        oneFirst = rnd.Next(0, tops);
                        oneSecond = rnd.Next(0, tops);
                    } while (oneFirst == oneSecond);
                    for (int j = 0; j < tops; j++)
                    {
                        if ((j == oneFirst) || (j == oneSecond)) mas[j, i] = 1;
                        else mas[j, i] = 0;
                    }
                }
                //делаем проверку на то, нет ли однаковых ребер, удаляем одно, если есть
                for (int i = 0; i < edges; i++)
                {
                    int oneFirst = FirstOneInEdge(mas, tops, i);
                    int oneSecond = SecondOneInEdge(mas, tops, i);
                    for (int j = i + 1; j < edges; j++)
                    {
                        if ((oneFirst == FirstOneInEdge(mas, tops, j) && (oneSecond == SecondOneInEdge(mas, tops, j))))
                        {
                            mas=DeleteEdge(mas, j, ref edges, tops);
                            j--;
                        }
                    }
                }                
                ok = IsEulerGraph(mas, edges, tops);//проверяем, является ли граф эйлеровым
            } while (!ok);
            return mas;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Нахождение эйлерова цикла в графе, заданном матрицей инциденций");           
            do
            {
                int tops=0;
                int edges=0;
                int[,] mas=Generator(ref tops, ref edges);
                Console.WriteLine("МАТРИЦА:");
                Console.WriteLine();
                WriteMas(mas, tops, edges);
                Console.WriteLine();
                Console.WriteLine("ЭЙЛЕРОВ ЦИКЛ:");
                EulerСycle(mas, ref edges, tops, 0);
                Console.Write("0");
                Console.WriteLine();
                Console.ReadLine();
            } while (true);
        }
    }
}
