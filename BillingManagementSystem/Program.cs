using System;
using System.IO;
//using System.Security.Cryptography.X509Certificates;

class Program
{
    static string currentUser = "";

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("===============");
            Console.WriteLine("1. Admin Login");
            Console.WriteLine("2. Cashier Login");
            Console.WriteLine("3. Exit");
            Console.WriteLine("===============");
            Console.Write("Select an option: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AdminLogin();
                    break;
                case 2:
                    CashierLogin();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("===============");
                    Console.WriteLine("Invalid option. Please select again.");
                    Console.WriteLine("===============");
                    break;
            }
        }
    }

    static void AdminLogin()
    {
        // Default admin credentials
        string defaultAdminUsername = "admin";
        string defaultAdminPassword = "admin123";

        Console.Write("Enter admin username: ");
        string username = Console.ReadLine();
        Console.WriteLine();
        Console.Write("Enter admin password: ");
        string password = Console.ReadLine();
        Console.WriteLine("===============");

        if (username == defaultAdminUsername && password == defaultAdminPassword)
        {
            currentUser = "admin";
            AdminMenu();
        }
        else
        {
            Console.WriteLine("===============");
            Console.WriteLine("Invalid credentials.");
            Console.WriteLine("===============");
        }
    }

    static void CashierLogin()
    {
        Console.Write("Enter cashier username: ");
        string username = Console.ReadLine();
        Console.WriteLine();
        Console.Write("Enter cashier password: ");
        string password = Console.ReadLine();
        Console.WriteLine("===============");

        if (ValidateUser(username, password, "cashier"))
        {
            currentUser = "cashier";
            CashierMenu();
        }
        else
        {
            Console.WriteLine("===============");
            Console.WriteLine("Invalid credentials.");
            Console.WriteLine("===============");
        }
    }

    static bool ValidateUser(string username, string password, string userType)
    {
        string userFileName = userType == "admin" ? "AdminData.txt" : "CashierData.txt";

        if (File.Exists(userFileName))
        {
            string[] lines = File.ReadAllLines(userFileName);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2 && parts[0] == username && parts[1] == password)
                {
                    return true;
                }
            }
        }

        return false;
    }

    static void AdminMenu()
    {
        while (true)
        {
            Console.WriteLine("Admin Menu");
            Console.WriteLine("===============");
            Console.WriteLine("1. Add Inventory Item");
            Console.WriteLine("2. Add Cashier");
            Console.WriteLine("3. Display All Inventory Items");
            Console.WriteLine("4. Logout");
            Console.WriteLine("===============");
            Console.Write("Select an option: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddInventoryItem();
                    break;
                case 2:
                    AddCashier();
                    break;
                case 3:
                    Console.Clear();
                    DisplayAllInventoryItems();
                    break;
                case 4:
                    currentUser = "";
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("===============");
                    Console.WriteLine("Invalid option. Please select again.");
                    Console.WriteLine("===============");
                    break;
            }
        }
    }

    static void AddCashier()
    {
        Console.Write("Enter new cashier username: ");
        string newUsername = Console.ReadLine();
        Console.WriteLine();
        Console.Write("Enter new cashier password: ");
        string newPassword = Console.ReadLine();
        Console.WriteLine("===============");

        string fileName = "CashierData.txt";
        using (StreamWriter sw = File.AppendText(fileName))
        {
            sw.WriteLine($"{newUsername},{newPassword}");
        }

        Console.WriteLine("===============");
        Console.WriteLine("Cashier account added.");
        Console.WriteLine("===============");
    }

    static void AddInventoryItem()
    {
        Console.Write("Enter item name: ");
        string itemName = Console.ReadLine();
        Console.WriteLine();
        Console.Write("Enter item price: ");
        double itemPrice = double.Parse(Console.ReadLine());
        Console.WriteLine("===============");

        string fileName = "InventoryData.txt";
        using (StreamWriter sw = File.AppendText(fileName))
        {
            sw.WriteLine($"{itemName},{itemPrice}");
        }

        Console.WriteLine("===============");
        Console.WriteLine("Item added to inventory.");
        Console.WriteLine("===============");
    }

    static void DisplayAllInventoryItems()
    {
        string fileName = "InventoryData.txt";
        if (File.Exists(fileName))
        {
            string[] lines = File.ReadAllLines(fileName);

            Console.WriteLine("Inventory Items");
            Console.WriteLine("=============================");
            Console.WriteLine("|    Item     |   Price ($)  |");
            Console.WriteLine("=============================");

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2)
                {
                    Console.WriteLine($"| {parts[0],-11} | {parts[1],12:F2} |");
                }
            }

            Console.WriteLine("=============================");
        }
        else
        {
            Console.WriteLine("Inventory is empty.");
        }
    }

    static void CashierMenu()
    {
        while (true)
        {
            Console.WriteLine("Cashier Menu");
            Console.WriteLine("===============");
            Console.WriteLine("1. Add Item to Cart");
            Console.WriteLine("2. Print Bill");
            Console.WriteLine("3. Display All Inventory Items");
            Console.WriteLine("4. Logout");
            Console.WriteLine("===============");
            Console.Write("Select an option: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddItemToCart();
                    break;
                case 2:
                    PrintBill();
                    break;
                case 3:
                    Console.Clear();
                    DisplayAllInventoryItems();
                    break;
                case 4:
                    currentUser = "";
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("===============");
                    Console.WriteLine("Invalid option. Please select again.");
                    Console.WriteLine("===============");
                    break;
            }
        }
    }

    static string[] cartItems = new string[100]; // Adjust the array size based on your needs
    static int cartItemCount = 0;

    static void AddItemToCart()
    {
        Console.Write("Enter item name to add to cart: ");
        string itemName = Console.ReadLine();

        // Validate if the item exists in the inventory
        if (IsItemInInventory(itemName))
        {
            if (cartItemCount < cartItems.Length)
            {
                cartItems[cartItemCount] = itemName;
                cartItemCount++;
                Console.WriteLine($"Item '{itemName}' added to cart.");
                Console.WriteLine("===============");
            }
            else
            {
                Console.WriteLine("===============");
                Console.WriteLine("Cart is full. Cannot add more items.");
                Console.WriteLine("===============");
            }
        }
        else
        {
            Console.WriteLine("===============");
            Console.WriteLine($"Item '{itemName}' not found in inventory.");
            Console.WriteLine("===============");
        }
    }
    static bool IsItemInInventory(string itemName)
    {
        string inventoryFileName = "InventoryData.txt";
        if (File.Exists(inventoryFileName))
        {
            string[] lines = File.ReadAllLines(inventoryFileName);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2 && parts[0] == itemName)
                {
                    return true; // Item found in inventory
                }
            }
        }

        return false; // Item not found in inventory
    }



    static void PrintBill()
    {
        Console.WriteLine("===============");
        Console.WriteLine("Receipt");
        Console.WriteLine("===============");

        double totalAmount = 0;

        Console.WriteLine("Items in Cart:");
        Console.WriteLine("=============================");
        Console.WriteLine("|    Item     |   Price ($)  |");
        Console.WriteLine("=============================");

        for (int i = 0; i < cartItemCount; i++)
        {
            string itemName = cartItems[i];
            double itemPrice = GetItemPrice(itemName);
            totalAmount += itemPrice;

            Console.WriteLine($"| {itemName,-11} | {itemPrice,12:F2} |");
        }

        Console.WriteLine("=============================");
        Console.WriteLine($"Total Amount: {totalAmount,15:F2}");
        Console.WriteLine("===============");

        // Clear the cart
        Array.Clear(cartItems, 0, cartItems.Length);
        cartItemCount = 0;

        Console.WriteLine("Cart cleared.");
        Console.WriteLine("===============");
    }

    static double GetItemPrice(string itemName)
    {
        string inventoryFileName = "InventoryData.txt";
        if (File.Exists(inventoryFileName))
        {
            string[] lines = File.ReadAllLines(inventoryFileName);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2 && parts[0] == itemName)
                {
                    return double.Parse(parts[1]);
                }
            }
        }

        return 0; // Return 0 if item price is not found
    }

}