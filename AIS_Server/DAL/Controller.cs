using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;


namespace AIS_Server.DAL
{
    public static class Controller<T> where T : class
    {
        public static string ListObjects()
        {
            List<T> values;
            using (var db = new AISEntities())
            {
                values = db.Set<T>().ToList();
            }
            return JsonConvert.SerializeObject(values);
        }
        public static string GetObjectById(int id) {
            T value;
            using (var db = new AISEntities())
            {
                value = db.Set<T>().Find(id);
            }
            if (value == null) return "NotFound";
            return JsonConvert.SerializeObject(value);
        }
        public static string AddObject(T obj)
        {
            using (var db = new AISEntities())
            {
                db.Set<T>().Add(obj);
                db.SaveChanges();
                
            }
            return "OK";
        }
        public static string UpdateObject(int id, T obj)
        {
            using (var db = new AISEntities())
            {
                var existing = db.Set<T>().Find(id);
                if (existing == null)
                {
                    return "NotFound";
                }
                db.Entry(existing).CurrentValues.SetValues(obj);
                db.SaveChanges();
            }
            return "OK";
        }
        public static string DeleteObject(int id)
        {
            using (var db = new AISEntities())
            {
                var obj = db.Set<T>().Find(id);
                if (obj == null)
                {
                    return "NotFound";
                }
                db.Set<T>().Remove(obj);
                db.SaveChanges();
            }
            return "OK";
        }
    }
}
