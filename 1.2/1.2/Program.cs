using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Задание 2");
        Console.WriteLine("1 - Synchronous");
        Console.WriteLine("2 - Async");
        Console.Write("Введите номер версии: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                RunSynchronous();
                break;
            case "2":
                await RunAsync();
                break;
            default:
                Console.WriteLine("Неверный выбор.");
                break;
        }
    }

    static void RunSynchronous()
    {
        Stopwatch sw = Stopwatch.StartNew();

        string[] urls = new string[]
        {
            "https://jsonplaceholder.typicode.com/posts/1",
            "https://jsonplaceholder.typicode.com/users/1",
            "https://jsonplaceholder.typicode.com/todos/1"
        };

        using (HttpClient client = new HttpClient())
        {
            for (int i = 0; i < urls.Length; i++)
            {
                Console.WriteLine($"\nЗапрос {i + 1} к: {urls[i]}");

                try
                {
                    HttpResponseMessage response = client.GetAsync(urls[i]).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Ошибка! Статус: {(int)response.StatusCode} {response.ReasonPhrase}");
                        continue;
                    }

                    string json = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("Ответ от сервера (JSON):");
                    Console.WriteLine(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка при запросе: {ex.Message}");
                }
            }
        }

        sw.Stop();
        Console.WriteLine($"\nОбщее время выполнения: {sw.ElapsedMilliseconds} мс");
    }

    static async Task RunAsync()
    {
        Stopwatch sw = Stopwatch.StartNew();

        string[] urls = new string[]
        {
            "https://jsonplaceholder.typicode.com/posts/1",
            "https://jsonplaceholder.typicode.com/users/1",
            "https://jsonplaceholder.typicode.com/todos/1"
        };

        using (HttpClient client = new HttpClient())
        {
            Task[] tasks = new Task[urls.Length];

            for (int i = 0; i < urls.Length; i++)
            {
                int index = i;
                tasks[i] = FetchAndPrintAsync(client, urls[index], index + 1);
            }

            await Task.WhenAll(tasks);
        }

        sw.Stop();
        Console.WriteLine($"\nОбщее время выполнения: {sw.ElapsedMilliseconds} мс");
    }

    static async Task FetchAndPrintAsync(HttpClient client, string url, int requestNumber)
    {
        Console.WriteLine($"\nЗапрос {requestNumber} к: {url}");

        try
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Ошибка! Статус: {(int)response.StatusCode} {response.ReasonPhrase}");
                return;
            }

            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Ответ от сервера (JSON):");
            Console.WriteLine(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка при запросе: {ex.Message}");
        }
    }
}