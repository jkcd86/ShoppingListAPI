using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Models;

namespace ShoppingListAPI.Controllers
{
    [Route("[controller]/api/")] // https://...//shoppinglist/api/
    [ApiController]
    public class ShoppingListController: ControllerBase
    {
        private List<Article> shoppingArticles = new List<Article>();

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
    }
}