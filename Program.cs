using shoppingcart;

Item item = new("greenCheese", 15.25m, 18);
Item item2 = new("SoftDrink", 2.99m, 7);

ShoppingCart cart = new();
ShoppingCart cart2 = new();

cart.AddItem(item);
cart.AddItem(item2);
cart.AddItem(item2);
cart.AddItem(item2);

cart.DisplayCartItems();

cart.Checkout();
cart2.Checkout();