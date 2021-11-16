using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TapShoesCanada.Models
{
    public class ConfigParams
    {
        internal static int COOKIE_EXPIRY_TIME = 60 * 24 * 2; //For 2 Days
        internal static string CART_ITEM_COOKIE_NAME = "cartItems";
        internal static string CUSTOM_CART_ITEM_COOKIE_NAME = "customCartItems";
        internal static int CUSTOM_SHOE_FIXED_PRICE = 500;
        internal static float GST_RATE = 13.0f;
    }
}
