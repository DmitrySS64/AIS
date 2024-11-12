using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_6
{
    class ProductVM
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("Продукт")]
        public string Name { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        [DisplayName("Цена")]
        public string Price { get; set; }
        [DisplayName("Ссылка на товар")]
        public string Url { get; set; }
        [DisplayName("Дата создания записи")]
        public System.DateTime CreatedDate { get; set; } = DateTime.Now;
        [DisplayName("Дата последнего изменения записи")]
        public System.DateTime UpdatedDate { get; set; } = DateTime.Now;

        public ProductVM() { }
        public ProductVM(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price.ToString();
            Url = product.Url;
            CreatedDate = product.CreatedDate;
            UpdatedDate = product.UpdatedDate;
        }


    }
}
