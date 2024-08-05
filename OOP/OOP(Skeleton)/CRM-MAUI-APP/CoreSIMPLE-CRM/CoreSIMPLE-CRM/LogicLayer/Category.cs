using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreSIMPLECRM.LogicLayer
{
    public class Category
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public static List<Category> categories = new List<Category>();
        public Category CreateCategory(string name)
        {
            var newCategory = new Category { Name = name, Id = Guid.NewGuid().ToString() };
            categories.Add(newCategory);
            return newCategory;
        }

        public bool EditCategory(string Id, string newName)
        {
            // Assuming 'categories' is a list of Category objects
            var category = categories.Find(c => c.Id == Id);
            if (category != null)
            {
                category.Name = newName;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteCategory(string nameForDelete)
        {
            // Assuming 'categories' is a list of Category objects
            var category = categories.Find(c => c.Name == nameForDelete);
            if (category != null)
            {
                categories.Remove(category);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static List<Category> GetAllCategories()
        {
            return categories;
        }

        public Category GetCategoryById(string Id)
        {
            Category searchCat = categories.Find(c => c.Id == Id);
            if (searchCat != null)
            {
                return searchCat;
            }
            else
                return null;
        }
    }
}
