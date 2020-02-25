using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext)
        {
            this.basketContext = BasketContext;

            this.productContext = ProductContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketId = cookie.Value; // vo Create mu davame Value = ID
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }

                }

            }
            else // if is null
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }
            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket(); // kreirame bakst
            basketContext.Insert(basket); // go stavame vo DB basketContext
            basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id; // Koga Kreirame ke ima vrednost ID
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie); //adding it to the http responce, Back to the User

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true); //ako ima vrakja Vrednost ako nema Kreira Novo
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId); // naogam item so dadeno Id

            if (item == null) // ako nema idem so toa ID 
            {
                item = new BasketItem() // pravime novo
                {
                    BasketId = basket.Id,// ova od basket
                    ProductId = productId, //zimame vredosti od fun
                    Quanity = 1
                };
                basket.BasketItems.Add(item);  //go stavame noviov Item u Basket
            }
            else
            {
                item.Quanity = item.Quanity + 1; // ako postoi sepak zgolemuvame Quanity
            }
            basketContext.Commit(); // na kraj Update/Commit. 

        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }
    }
}

