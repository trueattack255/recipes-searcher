using FreshMvvm;
using Mobile.Models;

namespace Mobile.PageModels
{
    class RecipePageModel : FreshBasePageModel
    {
        public Recipe Recipe { get; set; }

        public Ingredient[] Ingredients { get; private set; }
        public Step[] Steps { get; private set; }
        public string Image { get; private set; }

        public override void Init(object initData)
        {
            base.Init(initData);

            Recipe = initData as Recipe;

            Ingredients = Recipe.Ingredients;
            Steps = Recipe.RecipeSteps;
            Image = Recipe.Photo;
        }
    }
}
