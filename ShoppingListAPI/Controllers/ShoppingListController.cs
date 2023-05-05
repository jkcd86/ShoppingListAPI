using Microsoft.AspNetCore.Mvc;

namespace ShoppingListAPI.Controllers
{
    [Route("[controller]/api/")] // https://...//shoppinglist/api/
    [ApiController]
    public class ShoppingListController: ControllerBase
    {
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
    }
}