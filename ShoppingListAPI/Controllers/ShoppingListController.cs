using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Models;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace ShoppingListAPI.Controllers
{
    [Route("[controller]/api/")] // https://...//shoppinglist/api/
    [ApiController]
    public class ShoppingListController: ControllerBase
    {
        private List<Article> shoppingArticles = new List<Article>();


        /// <summary>
        /// ctor
        /// </summary>
        public ShoppingListController()
        {
            Article articleOne = new Article { ID = 1, Name = "Milch", Amount = "2 Liter", Remark = "3,5% Fett", IsBought = false };
            Article articleTwo = new Article { ID = 1, Name = "Cola", Amount = "2 Kisten", Remark = "", IsBought = false };
            Article articleThree = new Article { ID = 1, Name = "Fanta", Amount = "1 Kiste", Remark = "gesünder als Cola... vielleicht? :-)", IsBought = false };
            Article articleFour = new Article { ID = 4, Name = "Bananen", Amount = "6 Stück", Remark = "", IsBought = false };
            shoppingArticles.Add(articleFour);
        }

        #region // https://.../shoppinglist/api
        /// <summary>
        ///  Method to check if webservice is running
        /// </summary>
        /// <returns>return an information</returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public string Get()
        {
            return "*** ShoppingList REST API is running...";
        }

        #endregion

        #region // https://...//shoppinglist/api/additem
        /// <summary>
        /// Method to add a new article
        /// </summary>
        /// <param name="article">Specified an object of type 'Article'.</param>
        /// <returns>ResultCode</returns>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [HttpPost("additem",Name = "PostAddItem")]
        public IActionResult PostAddItem([FromBody]Article article)
        {
            try
            {
                if (article == null)
                {
                    return BadRequest(ModelState); // equals statuscode 400
                }

                if (shoppingArticles.Exists(x=> x.Name == article.Name))
                {
                    ModelState.AddModelError("", "The specified article still exists!");
                    return StatusCode(409, ModelState);// 409 equals 'ClientConflict'
                }
                shoppingArticles.Add(article);

                //return Ok(); // equals statuscode 200
                return StatusCode(201, article); // 201 equals 'Created'
            }
            catch (Exception exception)
            {
                return StatusCode(500, "Internal error occured:" + exception.Message + " InnerException:" + exception.InnerException);
            }
        }

        #endregion

        #region // https://...//shoppinglist/api/additem/{name}/{amount}/{remark}

        /// <summary>
        /// Method to add a new articleby url parameters
        /// </summary>
        /// <param name="name">Specifies the name of an article/param>
        /// <param name="amount">Specifies the amount of an article</param>
        /// <param name="remark">Specifies the remark of an article</param>
        /// <returns>ResultCode</returns>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [HttpPost("additem,/{name}/{amount}/{remark}", Name = "PostAddItemUrl")]
        public IActionResult PostAddItemUrl(string name, string amount, string remark)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest(ModelState); // equals statuscode 400
                }

                if (shoppingArticles.Exists(x => x.Name == name))
                {
                    ModelState.AddModelError("", "The specified article still exists!"); // equals statuscode 400
                }

                if (string.IsNullOrEmpty(amount))
                {
                    amount = "-";
                }

                if (string.IsNullOrEmpty(remark))
                {
                    remark = "-";
                }

                // create article object
                Article myArticle = new Article();
                myArticle.Name = name;
                myArticle.Amount = amount;
                myArticle.Remark = remark;
                myArticle.IsBought = false;
                myArticle.ID = shoppingArticles.Count + 1;

                shoppingArticles.Add(myArticle);

                return StatusCode(201, myArticle); // 201 equals 'Created'
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
        /// <returns>Return resultcode and a list of type 'Article'</returns>
        [ProducesResponseType(typeof(string), 200)]
        [HttpGet("getarticles", Name = "GetArticles")]
        public IActionResult GetArticles()
        {
            return Ok(shoppingArticles);
        }
        #endregion

        #region // https://.../shoppinglist/api/getarticles/{name}

        /// <summary>
        /// Get a specific article by name
        /// </summary>
        /// <param name="name">Specifies the name of the required article</param>
        /// <returns>Return resultcode and an item of type 'Article'</returns>
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
                return StatusCode(500, "Internal error oocured:" + exception.Message + " InnerException:" + exception.InnerException);
            }
            
        }
        #endregion
    }
}