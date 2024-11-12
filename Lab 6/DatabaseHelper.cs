using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lab_6
{
    class DatabaseHelper
    {
        //public static List<TViewModel> GetObjectsFromDatabase<TEntity, TViewModel>(
        //    Expression<Func<Database1Entities, DbSet<TEntity>>> tableSelector,
        //    Func<TEntity, TViewModel> viewModelSelector,
        //    int currentPage = 1,
        //    int pageSize = 5)
        //where TEntity : class
        //where TViewModel : class
        //{
        //    var result = new List<TViewModel>();
        //    using (var db = new Database1Entities())
        //    {
        //        var table = tableSelector.Compile()(db);
        //        result.Select(e => new viewModelSelector)
        //            .AddRange(
        //            table.OrderBy(e => EF.Property<object>(e, "Id"))
        //                 .Skip((currentPage - 1) * pageSize)
        //                 .Take(pageSize)
        //                 .Select(viewModelSelector)
        //                 .ToList()
        //        );
        //    }
        //    return result;
        
    }
}
