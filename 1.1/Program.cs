using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    private const int MAX_NUMBER = 10000;
    private const int THREAD_COUNT = 4;

    static void Main()
    {
        Console.WriteLine("Задание 1.1");
        Console.WriteLine("1 - Monitor");
        Console.WriteLine("2 - Mutex");
        Console.WriteLine("3 - Semaphore");
        Console.Write("Введите номер версии: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                RunMonitor();
                break;
            case "2":
                RunMutex();
                break;
            case "3":
                RunSemaphore();
                break;
            default:
                Console.WriteLine("Неверный выбор.");
                break;
        }
    }

    static bool IsPrime(int n)
    {
        if (n < 2) return false;
        if (n == 2) return true;
        if (n % 2 == 0) return false;

        int limit = (int)Math.Sqrt(n);
        for (int i = 3; i <= limit; i += 2)
        {
            if (n % i == 0) return false;
        }
        return true;
    }

    static void RunMonitor()
    {
        Console.WriteLine("=== Задание 1.1 Версия 1: Monitor (lock) ===");

        int primeCount = 0;
        object locker = new object();

        Stopwatch sw = Stopwatch.StartNew();

        Thread[] threads = new Thread[THREAD_COUNT];
        int chunkSize = MAX_NUMBER / THREAD_COUNT;

        for (int i = 0; i < THREAD_COUNT; i++)
        {
            int threadId = i + 1;
            int start = i * chunkSize + 1;
            int end = (i == THREAD_COUNT - 1) ? MAX_NUMBER : (i + 1) * chunkSize;

            threads[i] = new Thread(() =>
            {
                for (int num = start; num <= end; num++)
                {
                    Console.WriteLine($"[Поток {threadId}] Проверяем число: {num}");

                    if (IsPrime(num))
                    {
                        Console.WriteLine($"[Поток {threadId}] Найдено простое число: {num}");

                        lock (locker)
                        {
                            primeCount++;
                        }
                    }
                }
            });
            threads[i].Start();
        }

        foreach (var t in threads)
        {
            t.Join();
        }

        sw.Stop();

        Console.WriteLine();
        Console.WriteLine($"Общее количество простых чисел: {primeCount}");
        Console.WriteLine($"Время выполнения: {sw.ElapsedMilliseconds} мс");
    }

    static void RunMutex()
    {
        Console.WriteLine("=== Задание 1.1 Версия 2: Mutex ===");

        int primeCount = 0;
        Mutex mutex = new Mutex();

        Stopwatch sw = Stopwatch.StartNew();

        Thread[] threads = new Thread[THREAD_COUNT];
        int chunkSize = MAX_NUMBER / THREAD_COUNT;

        for (int i = 0; i < THREAD_COUNT; i++)
        {
            int threadId = i + 1;
            int start = i * chunkSize + 1;
            int end = (i == THREAD_COUNT - 1) ? MAX_NUMBER : (i + 1) * chunkSize;

            threads[i] = new Thread(() =>
            {
                for (int num = start; num <= end; num++)
                {
                    Console.WriteLine($"[Поток {threadId}] Проверяем число: {num}");

                    if (IsPrime(num))
                    {
                        Console.WriteLine($"[Поток {threadId}] Найдено простое число: {num}");

                        mutex.WaitOne();
                        try
                        {
                            primeCount++;
                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }
                    }
                }
            });
            threads[i].Start();
        }

        foreach (var t in threads)
        {
            t.Join();
        }

        sw.Stop();

        Console.WriteLine();
        Console.WriteLine($"Общее количество простых чисел: {primeCount}");
        Console.WriteLine($"Время выполнения: {sw.ElapsedMilliseconds} мс");
    }

    static void RunSemaphore()
    {
        Console.WriteLine("=== Задание 1.1 Версия 3: Semaphore ===");

        int primeCount = 0;
        Semaphore semaphore = new Semaphore(1, 1);

        Stopwatch sw = Stopwatch.StartNew();

        Thread[] threads = new Thread[THREAD_COUNT];
        int chunkSize = MAX_NUMBER / THREAD_COUNT;

        for (int i = 0; i < THREAD_COUNT; i++)
        {
            int threadId = i + 1;
            int start = i * chunkSize + 1;
            int end = (i == THREAD_COUNT - 1) ? MAX_NUMBER : (i + 1) * chunkSize;

            threads[i] = new Thread(() =>
            {
                for (int num = start; num <= end; num++)
                {
                    Console.WriteLine($"[Поток {threadId}] Проверяем число: {num}");

                    if (IsPrime(num))
                    {
                        Console.WriteLine($"[Поток {threadId}] Найдено простое число: {num}");

                        semaphore.WaitOne();
                        try
                        {
                            primeCount++;
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }
                }
            });
            threads[i].Start();
        }

        foreach (var t in threads)
        {
            t.Join();
        }

        sw.Stop();

        Console.WriteLine();
        Console.WriteLine($"Общее количество простых чисел: {primeCount}");
        Console.WriteLine($"Время выполнения: {sw.ElapsedMilliseconds} мс");
    }
}