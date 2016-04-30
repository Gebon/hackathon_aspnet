using System.Web.Mvc;

namespace DepenedcyInjection.Infrastructure
{
    public interface ICartProvider
    {
        Cart GetCart(Controller c);
        void SetCart(Controller c, Cart cart);
    }
    public class CartProvider : ICartProvider
    {
        private const string Cart = "cart";

        public Cart GetCart(Controller c)
        {
            if (c.Session[Cart] == null)
                SetCart(c, new Cart());

            return c.Session[Cart] as Cart;
        }

        public void SetCart(Controller c, Cart cart)
        {
            c.Session[Cart] = cart;
        }
    }
}