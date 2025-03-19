using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace CocktelesAPI.Controllers
{
    public class CoctelesController : Controller
    {
        // GET: Cockteles
        public ActionResult GetAll()
        {
            ML.Cockteles cockteles = new ML.Cockteles();

            using (HttpClient client = new HttpClient())
            {
                string URLCockteles = ConfigurationManager.AppSettings["URLCockteles"].ToString();
               
                client.BaseAddress = new Uri(URLCockteles);

                var responseTask = client.GetAsync("");

                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Cockteles>();
                    readTask.Wait();

                    var cocktelesList = readTask.Result;//API
                    cockteles = cocktelesList;

                    // Pasar los ingredientes y medidas a la vista
                    foreach (var cocktail in cockteles.drinks)
                    {
                        for (int i = 1; i <= 15; i++)
                        {
                            var ingredientProperty = typeof(ML.Drink).GetProperty($"strIngredient{i}");
                            var measureProperty = typeof(ML.Drink).GetProperty($"strMeasure{i}");

                            string ingredient = (string)ingredientProperty.GetValue(cocktail);
                            string measure = (string)measureProperty.GetValue(cocktail);

                            
                            ViewData[$"strIngredient{i}"] = ingredient;
                            ViewData[$"strMeasure{i}"] = measure;
                        }
                    }

                }
            }

            return View(cockteles);




        }
        

    }



}
