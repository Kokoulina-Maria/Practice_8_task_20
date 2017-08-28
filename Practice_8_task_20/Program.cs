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
            edges = edges - 1;//количество ребер сократилось
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
                if (mas[point1, i] == 1)//находим ребро, исходящее из данной вершины
                {
                    if (IsFirstOneInEdge(mas, tops, i, point1))//если оно первое в своем столбце матрицы 
                    {
                        if (SecondOneInEdge(mas, tops, i) == point2) return true;//если данное ребро соединяет вершину с концом (с вершиной, к которой нужно найти путь), то возвращаем true
                        else
                        {                                                        
                            int[,] masNew = new int[tops, edges];//новый массив, чтоб не удалить ребра в старом
                            masNew = DeleteEdge(mas, i, ref edges, tops);//удаляем ребро между точками, чтобы не пройти по нему второй раз
                            if (FindWay(ref masNew, tops, edges, SecondOneInEdge(mas, tops, i), point2)) return true;//Запускаем рекурсивную функцию посика пути до point2 от конца данной грани, если путь от данной точки до конца (point2) был найден, то возвращаем true
                            else return false;//иначе возвращаем false
                        }
                    }
                    else//если оно втрое в своем столбц матрицы
                    {
                        if (FirstOneInEdge(mas, tops, i) == point2) return true;//если данное ребро соединяет вершину с концом (с вершиной, к которой нужно найти путь), то возвращаем true
                        else
                        {                           
                            int[,] masNew = new int[tops, edges];//новый массив, чтоб не удалить ребра в старом
                            masNew = DeleteEdge(mas, i, ref edges, tops);//удаляем ребро между точками, чтобы не пройти по нему второй раз
                            if (FindWay(ref masNew, tops, edges, FirstOneInEdge(mas, tops, i), point2)) return true;//Запускаем рекурсивную функцию посика пути до point2 от конца данной грани, если путь от данной точки до конца (point2) был найден, то возвращаем true
                            else return false;//иначе возвращаем false
                        }
                    }
                }
            }
            return false;//если при переборе всех граней, исходящих из вершины, не было найдено пути до point2, то возвращаем false
        }

        static bool IsBridge(int[,] mas, int edge, int tops, int edges)
        {//функция, определяющая, является ли данное ребро мостом
            int point1 = FirstOneInEdge(mas, tops, edge);//первая вершина ребра
            int point2 = SecondOneInEdge(mas, tops, edge);//вторая вершина ребра
            mas = DeleteEdge(mas, edge, ref edges, tops);//удаляем данное ребро

            int[,] masNew = new int[tops, edges];//новый массив для того, чтобы не поудалять ребра в старом
            masNew = MakeMas(mas, masNew, tops, edges);
            if (FindWay(ref masNew, tops, edges, point1, point2))//если можно найти путь от первой вершины до втрой без удаленного ребра, то данное ребро не мост
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
                if ((mas[begin, i]==1)&&((ones==1)||(!IsBridge(mas, i, tops, edges))))//если из данной вершины исходит ребро и оно единственное или не является мостом
                {
                    Console.Write(begin + " --> ");//выводим на экран вершину
                    if (IsFirstOneInEdge(mas, tops, i, begin))//теперь вторая вершина ребра становится началом (в зависимости от того, первая она в стлобце матрицы или вторая)
                        begin = SecondOneInEdge(mas, tops, i);
                    else begin = FirstOneInEdge(mas, tops, i);
                    mas = DeleteEdge(mas, i, ref edges, tops);//удаляем ребро
                    EulerСycle(mas, ref edges, tops, begin);//вызывваем функцию вывода на экран эйлерова цикла из самой себя, меняем только начало
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
                int[,] masNew = new int[tops, edges];//создаем новый массив, чтобы не удалить грани в старом
                masNew = MakeMas(mas, masNew, tops, edges);
                if (!FindWay(ref masNew, tops, edges, 0, i)) return false;//если нельзя найти путь от нулевой вершины хотя бы до одной другой, то граф не Эйлеровый
            }

            //проверка на то, нет ли в графе вершин с четной степенью или ни с чем не связанных вершин
            for (int i = 0; i < tops; i++)
            {
                int g = 0;//количество единиц в строке (граней, исходящих из вершины)
                for (int j = 0; j < edges; j++)
                    if (mas[i, j] == 1) g++;//если встретили единицу, значит, есть грань
                if ((g == 0) || (g % 2 == 1)) return false;//число единиц в каждой строке должно быть больше 0 и быть четным
            }
            return true;
        }

        static int[,] Generator(ref int tops, ref int edges)
        {//генератор эйлеровых графов
            int[,] mas;
            bool ok;//переменная для проверки, является ли сгенерированный граф матрицей
            do
            {
                Random rnd = new Random();
                tops = rnd.Next(4, 30);//количество вершин
                edges = rnd.Next(tops, tops * (tops - 1) / 2);//количество граней не должно быть меньше количества вершин и не должно быть больше количества граней в полном графе с данным количеством вершин
                mas = new int[tops, edges];//матрица
                for (int i = 0; i < edges; i++)//заполняем матрицу случайными числами
                {
                    int oneFirst;//первая вершина для данной грани
                    int oneSecond;//вторая вершина для данной грани
                    do//выбираем две вершины для единиц
                    {
                        oneFirst = rnd.Next(0, tops);
                        oneSecond = rnd.Next(0, tops);
                    } while (oneFirst == oneSecond);//вершины не должны совпадать
                    for (int j = 0; j < tops; j++)//заполняем столбец (грань) матрицы
                    {
                        if ((j == oneFirst) || (j == oneSecond)) mas[j, i] = 1;
                        else mas[j, i] = 0;
                    }
                }
                //делаем проверку на то, нет ли однаковых ребер, удаляем одно, если есть
                for (int i = 0; i < edges; i++)
                {
                    int oneFirst = FirstOneInEdge(mas, tops, i);//находим первую вершину
                    int oneSecond = SecondOneInEdge(mas, tops, i);//находим вторую вершину
                    for (int j = i + 1; j < edges; j++)//перебираем последующие ребра
                    {
                        if ((oneFirst == FirstOneInEdge(mas, tops, j) && (oneSecond == SecondOneInEdge(mas, tops, j))))//если ребра одинаковые
                        {
                            mas=DeleteEdge(mas, j, ref edges, tops);//удаляем ребро
                            j--;//уменьшаем j, чтобы не пропустить следующее ребро
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
                int tops=0;//переменная для хранения количества вершин
                int edges=0;//переменная для хранения количества ребер
                int[,] mas=Generator(ref tops, ref edges);//генерируем матрицу инциденций эйлерова графа
                Console.WriteLine("МАТРИЦА:");
                Console.WriteLine();
                WriteMas(mas, tops, edges);//выводим матрицу на экран
                Console.WriteLine();
                Console.WriteLine("ЭЙЛЕРОВ ЦИКЛ:");
                EulerСycle(mas, ref edges, tops, 0);//ищем и выводим на экран эйлеров цикл с помощью матрицы инциденций
                Console.Write("0");//выводим последнюю и первую вершину
                Console.WriteLine();
                Console.ReadLine();
            } while (true);
        }
    }
}
