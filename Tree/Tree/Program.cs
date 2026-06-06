using System;
using System.Collections.Generic;

public class TreeNode
{
    public string Value { get; set; }
    public List<TreeNode> Children { get; set; }

    public TreeNode(string value)
    {
        Value = value;
        Children = new List<TreeNode>();
    }

    public void PrintAllDescendants(int level = 0)
    {
        for (int i = 0; i < level; i++)
        {
            Console.Write("  ");
        }
        Console.WriteLine(Value);

        if (Children.Count > 0)
        {
            foreach (TreeNode child in Children)
            {
                child.PrintAllDescendants(level + 1);
            }
        }
        else
        {
            return;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Tree\n");

        TreeNode root = new TreeNode("Корень");

        TreeNode child1 = new TreeNode("Дочерний 1");
        TreeNode child2 = new TreeNode("Дочерний 2");

        TreeNode grandchild1 = new TreeNode("Внук 1.1");
        TreeNode grandchild2 = new TreeNode("Внук 1.2");

        child1.Children.Add(grandchild1);
        child1.Children.Add(grandchild2);

        TreeNode grandchild3 = new TreeNode("Внук 2.1");

        child2.Children.Add(grandchild3);

        root.Children.Add(child1);
        root.Children.Add(child2);

        TreeNode child3 = new TreeNode("Дочерний 3 (без внуков)");
        root.Children.Add(child3);

        Console.WriteLine("Структура дерева:");
        root.PrintAllDescendants();
        Console.WriteLine("Дерево выведено");
    }
}