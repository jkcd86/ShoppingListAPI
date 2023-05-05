﻿using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Models;

namespace ShoppingListAPI.Controllers
{
    [Route("[controller]/api/")] // https://.../shoppinglist/api
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private List<Article> shoppingArticles = new List<Article>();

        /// <summary>
        /// ctor.
        /// </summary>
        public ShoppingListController()
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
        public string Get()
        {
            return "*** ShoppingList REST API is running… ***";
        }

        #endregion

        #region // https://.../shoppinglist/api/additem

        /// <summary>
        /// Method to add a new article.
        /// </summary>
        /// <param name="article">Specifies an object of type 'Article'.</param>
        /// <returns>Resultcode.</returns>
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [HttpPost("additem", Name = "PostAddItem")]
        public IActionResult PostAddItem([FromBody] Article article)
        {
            try
            {
                if (article == null)
                {
                    return BadRequest(ModelState); // Equals Statuscode 400
                }
                if (shoppingArticles.Exists(x => x.Name == article.Name))
                {
                    // ModelState.AddModelError("", "The specified article still exists");
                    // return StatusCode(409, ModelState); // 409 equals 'ClientConflict'
                    return Conflict("The specified article still exists");
                }
                shoppingArticles.Add(article);

                //return Ok();                        // Equals Statuscode 200
                return StatusCode(201, article);    // 201 equals 'Created'
            }
            catch (Exception exception)
            {
                return StatusCode(500, "Internal error occured:" + exception.Message + " InnerException:" + exception.InnerException);
            }
        }

        #endregion

        #region // https://.../shoppinglist/api/additem/{name}/{amount}/{remark}

        /// <summary>
        /// Method to add a new article by url parameters.
        /// </summary>
        /// <param name="name">Specifies the name of an articel.</param>
        /// <param name="amount">Specifies the amount of an articel.</param>
        /// <param name="remark">Specifies the remark of an articel.</param>
        /// <returns>Resultcode.</returns>
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [HttpPost("additem/{name}/{amount}/{remark}", Name = "PostAddItemUrl")]
        public IActionResult PostAddItemUrl(string name, string amount, string remark)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest(ModelState);   // Equals Statuscode 400
                }
                if (shoppingArticles.Exists(x => x.Name == name))
                {
                    return Conflict("The specified article still exists");   // Equals Statuscode 409
                }
                if (string.IsNullOrEmpty(amount))
                {
                    amount = "-";
                }
                if (string.IsNullOrEmpty(remark))
                {
                    remark = "-";
                }

                // Create article object
                Article myArticle = new Article();
                myArticle.Name = name;
                myArticle.Amount = amount;
                myArticle.Remark = remark;
                myArticle.IsBought = false;
                myArticle.ID = shoppingArticles.Count + 1;

                shoppingArticles.Add(myArticle);
                return StatusCode(201, myArticle);    // 201 equals 'Created'
            }
            catch (Exception exception)
            {
                return StatusCode(500, "Internal error occured:" + exception.Message + " InnerException:" + exception.InnerException);
            }
        }

        #endregion

        #region // https://.../shoppinglist/api/getarticles

        /// <summary>
        /// Get a list of all articles.
        /// </summary>
        /// <returns>Return resultcode and a list of type 'Article'.</returns>
        //[ProducesResponseType(typeof(string), 200)]
        [HttpGet("getarticles", Name = "GetArticles")]
        public IActionResult GetArticles()
        {
            return Ok(shoppingArticles);
        }

        #endregion

        #region // https://.../shoppinglist/api/getarticles/{name}

        /// <summary>
        /// Get a specific article by name.
        /// </summary>
        /// <param name="name">Specifies the name of the required article.</param>
        /// <returns>Return resultcode and an item of type 'Article'.</returns>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [HttpGet("getarticles/{name}", Name = "GetArticle")]
        public IActionResult GetArticle(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest(ModelState);
                }
                // Search for specific item in collection/database
                var articleResult = shoppingArticles.Find(x => x.Name == name);
                if (articleResult == null)
                {
                    return NotFound(name);
                }
                return Ok(articleResult);
            }
            catch (Exception exception)
            {
                return StatusCode(500, "Internal error occured:" + exception.Message + " InnerException:" + exception.InnerException);
            }
        }

        #endregion
    }
}