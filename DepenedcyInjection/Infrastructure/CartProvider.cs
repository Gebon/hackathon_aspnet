using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;

namespace DepenedcyInjection.Infrastructure
{
    public class CartProvider : ICartProvider
    {
        public Controller Controller { get; set; }
        public IEnumerable<int> Cart { get { return Controller.Session["votes"] as HashSet<int>; } }

        public CartProvider(Controller controller)
        {
            Controller = controller;
        }
    }
}