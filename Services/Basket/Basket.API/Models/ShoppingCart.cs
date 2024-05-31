namespace Basket.API.Models;

public class ShoppingCart
{
  public string UserName { get; set; } = string.Empty;
  public List<ShoppingCartItem> Items { get; set; } = [];
  public decimal TotalePrice
    => Items.Sum(x => x.Price * x.Quantity);

  public ShoppingCart(string UserName)
    => this.UserName = UserName;

  public ShoppingCart()
  {
  }
}
