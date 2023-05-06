using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Models;
using System;
using System.Reflection.Metadata.Ecma335;

namespace ShoppingListAPI.Controllers
{
    [Route("shoppinglist/v2/api/")] // https://.../shoppinglist/v1/api
    [ApiController]
    public class ShoppingListV2Controller : ControllerBase
    {
        private List<Article> shoppingArticles = new List<Article>();

        /// <summary>
        /// ctor.
        /// </summary>
        public ShoppingListV2Controller()
        {
            Article articleOne = new Article { ID = 1, Name = "Milch", Amount = "2 Liter", Remark = "3,5% Fett", IsBought = false };
            shoppingArticles.Add(articleOne);
            Article articleTwo = new Article { ID = 2, Name = "Cola", Amount = "2 Kisten", Remark = "", IsBought = false };
            shoppingArticles.Add(articleTwo);
            Article articleThree = new Article { ID = 3, Name = "Fanta", Amount = "1 Kiste", Remark = "gesünder als Cola... vielleicht? :-)", IsBought = false };
            shoppingArticles.Add(articleThree);
            Article articleFour = new Article { ID = 4, Name = "Bananen", Amount = "6 Stück", Remark = "", IsBought = false };
            shoppingArticles.Add(articleFour);
        }

        #region // https://.../shoppinglist/api

        /// <summary>
        /// Method to check if webservice is running.
        /// </summary>
        /// <returns>Return an information.</returns>
        [HttpGet]
        //[ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(typeof(string), 404)]
        //[ProducesResponseType(typeof(string), 500)]
        public string Get(string name)
        {
            return "*** ShoppingList REST API is running… ***" + name;
        }

        #endregion


    }
}