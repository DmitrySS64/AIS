using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Сourse_project.MVVM.Model.DNSParseContext;

namespace Сourse_project.MVVM.Model
{
    public static class CategoryService
    {

        public static async Task<int> GetOrCreateBrandIdAsync(string brandName)
        {
            using (var context = new ParseProductsContext())
            {
                var brand = await context.Brands.FirstOrDefaultAsync(b => b.Name == brandName);

                if (brand == null)
                {
                    brand = new Brand { Name = brandName };
                    context.Brands.Add(brand);
                    await context.SaveChangesAsync(); // Сохраняем новую запись
                }
                return brand.Id; // Возвращаем ID бренда
            }
        }

        public static async Task<int> GetOrCreateLineIdAsync(string lineName, int brandId)
        {
            using (var context = new ParseProductsContext())
            {
                var line = await context.Lines
                    .FirstOrDefaultAsync(l => l.Name == lineName && l.BrandId == brandId);

                if (line == null)
                {
                    line = new Line { Name = lineName, BrandId = brandId };
                    context.Lines.Add(line);
                    await context.SaveChangesAsync(); // Сохраняем новую запись
                }

                return line.Id; // Возвращаем ID линии
            }
        }

        public static async Task<int> GetOrCreateModelIdAsync(string modelName, int lineId)
        {
            using (var context = new ParseProductsContext())
            {
                var model = await context.Models
                    .FirstOrDefaultAsync(m => m.Name == modelName && m.LineId == lineId);

                if (model == null)
                {
                    model = new Model { Name = modelName, LineId = lineId };
                    context.Models.Add(model);
                    await context.SaveChangesAsync(); // Сохраняем новую запись
                }

                return model.Id; // Возвращаем ID модели
            }
        }

        public static async Task<int> GetOrCreateColorTypeIdAsync(string colorTypeName)
        {
            using (var context = new ParseProductsContext())
            {
                var colorType = await context.ColorTypes
                    .FirstOrDefaultAsync(c => c.Name == colorTypeName);

                if (colorType == null)
                {
                    colorType = new ColorType { Name = colorTypeName };
                    context.ColorTypes.Add(colorType);
                    await context.SaveChangesAsync(); // Сохраняем новую запись
                }

                return colorType.Id; // Возвращаем ID типа цвета
            }
        }

        public static async Task<int> GetOrCreateVersionIdAsync(string versionName)
        {
            using (var context = new ParseProductsContext())
            {
                var version = await context.Versions
                    .FirstOrDefaultAsync(v => v.Name == versionName);

                if (version == null)
                {
                    version = new VersionType { Name = versionName };
                    context.Versions.Add(version);
                    await context.SaveChangesAsync(); // Сохраняем новую запись
                }

                return version.Id; // Возвращаем ID версии
            }
        }

        public static async Task<int> GetOrCreateMatrixTypeIdAsync(string typeName)
        {
            using (var context = new ParseProductsContext())
            {
                var matrixType = await context.TypeOfMatrices
                    .FirstOrDefaultAsync(t => t.TypeName == typeName);

                if (matrixType == null)
                {
                    matrixType = new TypeOfMatrix { TypeName = typeName };
                    context.TypeOfMatrices.Add(matrixType);
                    await context.SaveChangesAsync(); // Сохраняем новую запись
                }

                return matrixType.Id; // Возвращаем ID типа матрицы
            }
        }

        public static async Task<int> GetOrCreateRAMSizeIdAsync(string ramSize)
        {
            using (var context = new ParseProductsContext())
            {
                var ram = await context.RAMSizes
                    .FirstOrDefaultAsync(r => r.Size == ramSize);

                if (ram == null)
                {
                    ram = new RAM { Size = ramSize };
                    context.RAMSizes.Add(ram);
                    await context.SaveChangesAsync(); // Сохраняем новую запись
                }

                return ram.Id; // Возвращаем ID RAM
            }
        }

        public static async Task<int> GetOrCreateBuiltInMemoryIdAsync(string memoryName)
        {
            using (var context = new ParseProductsContext())
            {
                var memory = await context.BuildInMemories
                    .FirstOrDefaultAsync(m => m.Name == memoryName);

                if (memory == null)
                {
                    memory = new BuiltInMemory { Name = memoryName };
                    context.BuildInMemories.Add(memory);
                    await context.SaveChangesAsync(); // Сохраняем новую запись
                }

                return memory.Id; // Возвращаем ID встроенной памяти
            }
        }

        public static async Task<int> GetOrCreateScreenResolutionFormatIdAsync(string resolutionFormat)
        {
            using (var context = new ParseProductsContext())
            {
                var resolution = await context.ScreenResolutionFormats
                    .FirstOrDefaultAsync(r => r.Type == resolutionFormat);

                if (resolution == null)
                {
                    resolution = new ScreenResolutionFormat { Type = resolutionFormat };
                    context.ScreenResolutionFormats.Add(resolution);
                    await context.SaveChangesAsync(); // Сохраняем новую запись
                }

                return resolution.Id; // Возвращаем ID формата разрешения экрана
            }
        }

        public static async Task<int> GetOrCreateOSIdAsync(string osType)
        {
            using (var context = new ParseProductsContext())
            {
                var os = await context.OSes
                    .FirstOrDefaultAsync(o => o.Type == osType);

                if (os == null)
                {
                    os = new OS { Type = osType };
                    context.OSes.Add(os);
                    await context.SaveChangesAsync(); // Сохраняем новую запись
                }

                return os.Id; // Возвращаем ID операционной системы
            }
        }

        public static async Task<int> GetOrCreateProcessorIdAsync(string processorName)
        {
            using (var context = new ParseProductsContext())
            {
                var processor = await context.Processors
                    .FirstOrDefaultAsync(p => p.Name == processorName);

                if (processor == null)
                {
                    processor = new Processor { Name = processorName };
                    context.Processors.Add(processor);
                    await context.SaveChangesAsync(); // Сохраняем новую запись
                }

                return processor.Id; // Возвращаем ID процессора
            }
        }
    }

}
