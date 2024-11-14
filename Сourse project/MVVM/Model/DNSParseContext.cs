using OpenQA.Selenium.DevTools.V128.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static OpenQA.Selenium.BiDi.Modules.Script.EvaluateResult;
using static Сourse_project.MVVM.Model.CasingMaterial;

namespace Сourse_project.MVVM.Model
{
    internal class DNSParseContext
    {
        public class ParseProductsContext : DbContext
        {
            public ParseProductsContext() : base("DNSDBConnection") { }

            //public DbSet<Catalog> Catalogs { get; set; }
            //public DbSet<Subsection> Subsections { get; set; }
            public DbSet<Smartphone> Smartphones { get; set; }
            public DbSet<Brand> Brands { get; set; }
            public DbSet<Line> Lines { get; set; }
            public DbSet<Model> Models { get; set; }
            public DbSet<ColorType> ColorTypes { get; set; }
            public DbSet<VersionType> Versions { get; set; }
            public DbSet<TypeOfMatrix> TypeOfMatrices { get; set; }
            public DbSet<RAM> RAMSizes { get; set; }
            public DbSet<BuiltInMemory> BuildInMemories { get; set; }
            public DbSet<ScreenResolutionFormat> ScreenResolutionFormats { get; set; }
            public DbSet<ScreenRefreshRate> ScreenRefreshRates { get; set; }
            public DbSet<OS> OSes { get; set; }
            public DbSet<Processor> Processors { get; set; }
            public DbSet<ScreenResolution> ScreenResolutions { get; set; }
            public DbSet<CasingMaterial> CasingMaterials { get; set; }
        }
    }

    public class Smartphone
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ArticleNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Warranty { get; set; } = string.Empty;
        public int Price { get; set; }

        public int ModelId { get; set; }
        public virtual Model Model { get; set; }

        public int ColorTypeId {  get; set; }
        public virtual ColorType ColorType { get; set; } // Тип цвета продукта

        public int VersionTypeId { get; set; }
        public virtual VersionType VersionType { get; set; } // Версия продукта

        public int RAMSizeId { get; set; }
        public virtual RAM RAMSize { get; set; } // Размер оперативной памяти продукта

        public int BuiltInMemoryId { get; set; }
        public virtual BuiltInMemory BuiltInMemory { get; set; } // Встроенная память продукта
    }

    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public List<Line> Lines { get; set; }

        public Brand()
        {
            Lines = new List<Line>();
        }
    }

    public class Line 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; } // Бренд в линии
        public List<Model> Models { get; set; } // Модели в линии

        public Line()
        {
            Models = new List<Model>();
        }
    }

    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LineId { get; set; }
        public virtual Line Line { get; set; }

        public int MatrixTypeId { get; set; }
        public virtual TypeOfMatrix TypeOfMatrix { get; set; } // Тип матрицы

        public int ScreenResolutionFormatId { get; set; }
        public virtual ScreenResolutionFormat ScreenResolutionFormat { get; set; }

        public int ScreenRefreshRateId { get; set; }
        public virtual ScreenRefreshRate ScreenRefreshRate { get; set; } // частота экрана продукта

        public int OSId { get; set; }
        public virtual OS OS { get; set; }

        public int ProcessorId { get; set; }
        public virtual Processor Processor { get; set; }

        public string Lifespan { get; set; }

        public string ScreenSize { get; set; }

        public int ScreenResolutionId { get; set; }
        public virtual ScreenResolution ScreenResolution { get; set; }

        public int SIMCount { get; set; }
        public string BatteryCapacity { get; set; }
        public string ChargingTime { get; set; }
        public string HasHeadphoneJack { get; set; }
        public string ReleaseDate { get; set; }
        public int CasingMaterialId { get; set; }
        public virtual CasingMaterial CasingMaterial { get; set; } // Материал корпуса продукта

        public decimal Weight { get; set; }
        public string Features { get; set; }

        public List<Smartphone> Smartphones { get; set; }
        public Model()
        {
            Smartphones = new List<Smartphone>();
        }
    }

    public class CasingMaterial
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Smartphone> Smartphones { get; set; }
    }

    public class ScreenResolution
    {
        public int Id { get; set; }
        public string Resolution { get; set; }
        public List<Model> Models { get; set; }
    }

    public class TypeOfMatrix
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public List<Model> Models { get; set; }
    }

    public class RAM
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public List<Smartphone> Smartphones { get; set; }
    }

    public class BuiltInMemory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Smartphone> Smartphones { get; set; }
    }

    public class ColorType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Smartphone> Smartphones { get; set; }
    }

    public class VersionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Model> Models { get; set; }
    }

    public class ScreenResolutionFormat
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<Model> Models { get; set; }
    }

    public class ScreenRefreshRate
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<Model> Models { get; set; }
    }

    public class  OS
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<Model> Models { get; set; }
    }

    public class Processor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Model> Models { get; set; }
    }

    //public class Catalog
    //{
    //    public int Id { get; set; } // Идентификатор каталога
    //    public string Name { get; set; } // Название каталога
    //    public string Url { get; set; } 
    //    public List<Subsection> Subsections { get; set; } // Разделы в каталоге

    //    public Catalog()
    //    {
    //        Subsections = new List<Subsection>();
    //    }
    //}

    //public class Subsection
    //{
    //    public int Id { get; set; } // Идентификатор подраздела
    //    public string Name { get; set; } // Название подраздела
    //    public string Url { get; set; }
    //    public int CatalogId { get; set; } // Идентификатор раздела, к которому относится подраздел
    //    public Catalog Catalog { get; set; } // Навигационное свойство для раздела

    //    public int? ParentSubsectionId { get; set; } // Идентификатор родительского подраздела (если есть)
    //    public virtual Subsection ParentSubsection { get; set; } // Навигационное свойство для родительского подраздела

    //    public List<Subsection> Subsections { get; set; } // Подразделы подраздела (для вложенности)
    //    public List<Product> Products { get; set; } //Товар подраздела

    //    public Subsection()
    //    {
    //        Subsections = new List<Subsection>();
    //        Products = new List<Product>();
    //    }
    //}
}
