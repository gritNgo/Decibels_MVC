using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Decibels.DataAccess.Repository.IRepository
{
    // T is a Generic class until implementation of this interface (A Model class ex: Category)
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(); // IEnumerable is like a 'collection'
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        // Update is not here but in ICategoryRepository instead because the logic for updating Category for example is different than updating a Product
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        
    }
}
