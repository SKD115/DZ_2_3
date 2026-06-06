using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    private const int MAX_CONCURRENT = 3;

    private static readonly Semaphore semaphore = new Semaphore(MAX_CONCURRENT, MAX_CONCURRENT);
    private static readonly object resultsLocker = new object();
    private static readonly Mutex totalMutex = new Mutex();

    private static List<string> results = new List<string>();
    private static long grandTotal = 0;

    static void Main()
    {
        string[] lines = File.ReadAllLines("numbers_sets.txt");
        if (lines.Length != 15)
        {
            Console.WriteLine("Ошибка: ожидалось 15 наборов");
            return;
        }

        Stopwatch sw = Stopwatch.StartNew();

        Thread[] threads = new Thread[15];

        for (int i = 0; i < 15; i++)
        {
            int setNumber = i + 1;
            string numbersLine = lines[i];

            threads[i] = new Thread(() => ProcessSet(setNumber, numbersLine));
            threads[i].Start();
        }

        foreach (var t in threads)
        {
            t.Join();
        }

        sw.Stop();

        Console.WriteLine();
        foreach (var res in results)
        {
            Console.WriteLine(res);
        }

        Console.WriteLine();
        Console.WriteLine($"Общий итог всех сумм: {grandTotal}");
        Console.WriteLine($"Время выполнения: {sw.ElapsedMilliseconds} мс");
    }

    static void ProcessSet(int setNumber, string numbersLine)
    {
        semaphore.WaitOne();
        try
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            int[] numbers = numbersLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(int.Parse)
                                       .ToArray();

            long sum = numbers.Sum();

            lock (resultsLocker)
            {
                results.Add($"Набор {setNumber}: сумма = {sum}, поток = {threadId}");
            }

            totalMutex.WaitOne();
            try
            {
                grandTotal += sum;
            }
            finally
            {
                totalMutex.ReleaseMutex();
            }

            Console.WriteLine($"Набор {setNumber}: завершён в потоке {threadId}, сумма = {sum}");
        }
        finally
        {
            semaphore.Release();
        }
    }
}