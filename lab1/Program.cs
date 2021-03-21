using System;
using System.Diagnostics;
using System.Threading;

namespace lab1
{
    class Program
    {
        static void Work(int n) {//функция для последовательного вычисления
            double x = 1.0;
            for(int i=0;i<5000;++i)
                for(int j = 0; j < 5000; ++j)
                {
                    x = Math.Sqrt(x);
                    x += 0.000000001;
                    x = Math.Pow(x, 2);
                }
            Console.WriteLine($"{(char)n}: x = {x}");
        }
        static void Work1(object item){//функция для потоков
            double x = 1.0;
            for (int i = 0; i < 5000; ++i)
                for (int j = 0; j < 5000; ++j)
                {
                    x = Math.Sqrt(x);
                    x += 0.000000001;
                    x = Math.Pow(x, 2);
                }
            Console.Write($"{Convert.ToChar(item)} ");
        }

        static void parallelCalc(int n) {
            Thread[] threads = new Thread[n];
            for (int j = 0; j < n; ++j)//назначение функции потокам
                threads[j] = new Thread(Work1);
            for (int j = 0; j < threads.Length; ++j){
                threads[j].Start(65 + j);//65 код символа 
                threads[j].Join();
            }
            //foreach (var thread in threads)
            //    thread.Join();
        }

        static void parallelCalcPriority(int n){//функция для вычисления с приоритетом
            Stopwatch sw;
            Thread[] threads = new Thread[n];
            int priority = 0;
            for (int i = 0; i <= 4/n; ++i){//цикл приоритетов Priority enum {0-4}

                Console.WriteLine($"Parall n={i}");
                for (int j = 0; j < threads.Length; ++j){//по всем элементам массива потоков 
                    sw = Stopwatch.StartNew();
                    threads[j] = new Thread(Work1);
                    threads[j].Priority = (ThreadPriority)priority;
                    threads[j].Start(65 + j);//65 код символа для имени потока
                    threads[j].Join();
                    sw.Stop();
                    Console.WriteLine($"{(ThreadPriority)priority++}: {sw.ElapsedMilliseconds} ms");//вывод с изменением приоритета
                    priority %= 5;//значение приоритета не больше 4
                }
            }
        }
        static void Main(string[] args){
            Stopwatch sw;
            const int N = 3;//N+1 где N количество ядер
            //последовательно вычисление
            for (int i = 1; i <= N; ++i){
                Console.WriteLine($" вывод для {i} блоков последовательно");
                sw = Stopwatch.StartNew();
                for (int j = 0; j < i; ++j)
                    Work(65 + j);
                sw.Stop();
                Console.WriteLine($"Serial n={i}: {sw.ElapsedMilliseconds} ms -> avg {sw.ElapsedMilliseconds / i} ms");
            }
            //параллельное вычисление
            for (int i = 1; i <= N; ++i){
                Console.WriteLine($"последовательность вывода для {i} блоков параллельно");
                sw = Stopwatch.StartNew();
                parallelCalc(i);
                sw.Stop();
                Console.WriteLine($"Parall n={i}: {sw.ElapsedMilliseconds} ms -> avg {sw.ElapsedMilliseconds / i} ms");
            }
            ////параллельное вычисление с приоритетами
            for (int i = 1; i <= N; ++i){
                Console.WriteLine($"последовательность вывода для {i} блоков с приоритетом");
                parallelCalcPriority(i);
            }
        }
    }
}