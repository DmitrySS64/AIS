using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureOfInformationSystems.MVC.Model
{
    public interface IModel<T> where T : class, new()
    {
        public List<T> GetValues();

        public void AddEntry(IEnumerable<string> entryFields, bool NeedSave = true);
        public void EditEntry(int key, IEnumerable<string> entryFields);
        public void RemoveEntry(int key);
    }
}
