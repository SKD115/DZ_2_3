using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }

    public override string ToString()
    {
        return "Заказ #" + Id + ": " + CustomerName + ", " + TotalAmount + " руб, статус: " + Status;
    }
}

public interface IOrderRepository
{
    void Save(Order order);
    Order GetById(int id);
    List<Order> GetAll();
    void Update(Order order);
    void Delete(int id);
}

public class InMemoryOrderRepository : IOrderRepository
{
    private List<Order> orders = new List<Order>();
    private int nextId = 1;

    public void Save(Order order)
    {
        order.Id = nextId;
        nextId++;
        orders.Add(order);
        Console.WriteLine("Сохранён заказ #" + order.Id);
    }

    public Order GetById(int id)
    {
        return orders.FirstOrDefault(o => o.Id == id);
    }

    public List<Order> GetAll()
    {
        return new List<Order>(orders);
    }

    public void Update(Order order)
    {
        Order existing = GetById(order.Id);
        if (existing != null)
        {
            existing.CustomerName = order.CustomerName;
            existing.TotalAmount = order.TotalAmount;
            existing.Status = order.Status;
            Console.WriteLine("Обновлён заказ #" + order.Id);
        }
    }

    public void Delete(int id)
    {
        orders.RemoveAll(o => o.Id == id);
        Console.WriteLine("Удалён заказ #" + id);
    }
}

public class CsvOrderRepository : IOrderRepository
{
    private string filePath;
    private int nextId = 1;

    public CsvOrderRepository(string filePath = "orders.csv")
    {
        this.filePath = filePath;

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Id,CustomerName,TotalAmount,Status\n");
        }
        else
        {
            var lines = File.ReadAllLines(filePath).Skip(1);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                int id = int.Parse(parts[0]);
                if (id >= nextId) nextId = id + 1;
            }
        }
    }

    private List<Order> LoadAll()
    {
        List<Order> result = new List<Order>();
        if (!File.Exists(filePath)) return result;

        var lines = File.ReadAllLines(filePath).Skip(1);
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(',');
            result.Add(new Order
            {
                Id = int.Parse(parts[0]),
                CustomerName = parts[1],
                TotalAmount = decimal.Parse(parts[2]),
                Status = parts[3]
            });
        }
        return result;
    }

    private void SaveAll(List<Order> orders)
    {
        List<string> lines = new List<string> { "Id,CustomerName,TotalAmount,Status" };
        foreach (var o in orders)
        {
            lines.Add(o.Id + "," + o.CustomerName + "," + o.TotalAmount + "," + o.Status);
        }
        File.WriteAllLines(filePath, lines);
    }

    public void Save(Order order)
    {
        order.Id = nextId;
        nextId++;
        var list = LoadAll();
        list.Add(order);
        SaveAll(list);
        Console.WriteLine("Сохранён заказ #" + order.Id + " в файл " + filePath);
    }

    public Order GetById(int id)
    {
        return LoadAll().FirstOrDefault(o => o.Id == id);
    }

    public List<Order> GetAll()
    {
        return LoadAll();
    }

    public void Update(Order order)
    {
        var list = LoadAll();
        var existing = list.FirstOrDefault(o => o.Id == order.Id);
        if (existing != null)
        {
            existing.CustomerName = order.CustomerName;
            existing.TotalAmount = order.TotalAmount;
            existing.Status = order.Status;
            SaveAll(list);
            Console.WriteLine("Обновлён заказ #" + order.Id);
        }
    }

    public void Delete(int id)
    {
        var list = LoadAll();
        list.RemoveAll(o => o.Id == id);
        SaveAll(list);
        Console.WriteLine("Удалён заказ #" + id);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("PortAdapter\n");

        Console.WriteLine("MemoryOrder");
        IOrderRepository memoryRepo = new InMemoryOrderRepository();

        memoryRepo.Save(new Order { CustomerName = "Олег Монгол", TotalAmount = 1500.50m, Status = "Новый" });
        memoryRepo.Save(new Order { CustomerName = "Антон Чигур", TotalAmount = 2750.00m, Status = "Новый" });

        var allMemory = memoryRepo.GetAll();
        foreach (var o in allMemory)
        {
            Console.WriteLine(o);
        }
        Console.WriteLine();

        Console.WriteLine("CsvOrder");
        IOrderRepository csvRepo = new CsvOrderRepository("orders.csv");

        csvRepo.Save(new Order { CustomerName = "Иван Золон", TotalAmount = 890.00m, Status = "Новый" });
        csvRepo.Save(new Order { CustomerName = "Шаман", TotalAmount = 3200.75m, Status = "Новый" });

        var allCsv = csvRepo.GetAll();
        foreach (var o in allCsv)
        {
            Console.WriteLine(o);
        }
    }
}