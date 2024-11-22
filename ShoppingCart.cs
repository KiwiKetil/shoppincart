namespace shoppingcart;
public readonly record struct ShoppingCartId(Guid shoppingCartId) 
{
    public static ShoppingCartId NewId => new(Guid.NewGuid());
    public static ShoppingCartId Empty => new(Guid.Empty);  
};

internal class ShoppingCart
{
    private readonly ShoppingCartId _shoppingCartId;
    private readonly List<Item> _items = [];
    private static readonly object _inventoryLock = new();

    public ShoppingCart()
    {
        _shoppingCartId = ShoppingCartId.NewId;
    }

    public ShoppingCartId ShoppingCartId { get => _shoppingCartId; }
    public List<Item> Items => new(_items);   
    public decimal TotalPrice => _items.Sum(item => item.Price);
   
    public void AddItem(Item item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Item cannot be null.");

        lock (_inventoryLock)
        {
            if (item.QuantityInStock < 1)
            {
                Console.WriteLine($"Cannot add {item.ItemName} to cart. Item is out of stock!");
            }
            else
            {
                _items.Add(item);
                item.QuantityInStock--;
                Console.WriteLine($"{item.ItemName} X 1 added to cart.");
            }
        }
    }

    public void RemoveItem(Item item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Item cannot be null.");

        if (_items.Remove(item))
        {
            lock (_inventoryLock)
            {
                item.QuantityInStock++;
                Console.WriteLine($"{item.ItemName} removed from cart.");
            }
        }
        else
        {
            Console.WriteLine($"{item.ItemName} not found in the cart.");
        }
    }

    public void Checkout()
    {
        if (_items.Count == 0)
        {
            Console.WriteLine("\nCart is empty!");
        }
        else
        {
            Console.WriteLine($"\nThank you for your purchase of {TotalPrice:C} and not paying");
            _items.Clear();
        }
    }

    public void DisplayCartItems()
    {        
        Console.WriteLine("\nCart contents:\n");
        foreach (var item in Items)
        {
            Console.WriteLine($"{item.ItemName} {item.Price:C}");
        }
        Console.WriteLine($"\nTotal Price: {TotalPrice:C}");
    }
}

