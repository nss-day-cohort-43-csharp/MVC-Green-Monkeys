using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICategoryRepository
    {
        void AddCategory(Category category);
        List<Category> GetAll();
        
    }
}