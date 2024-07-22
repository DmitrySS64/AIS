using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Models
{
    public interface IModel<T> where T : class, new()
    {
        List<T> GetValues();

        void AddEntry(IEnumerable<string> entryFields, bool NeedSave = true);
        void EditEntry(int key, IEnumerable<string> entryFields);
        void RemoveEntry(int key);
    }
}
