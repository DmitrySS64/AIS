using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Сourse_project.MVVM.Model;

namespace Сourse_project.MVVM.ModelView
{
    internal class CatalogVM : BaseViewModel
    {
        //private ObservableCollection<Catalog> _categories;
        //private ObservableCollection<Subsection> _subcategories;
        //private ObservableCollection<Smartphone> _products;
        //private string _navigationPath;

        //public ObservableCollection<Catalog> Categories
        //{
        //    get { return _categories; }
        //    set { SetProperty(ref _categories, value); }
        //}

        //public ObservableCollection<Subsection> Subcategories
        //{
        //    get { return _subcategories; }
        //    set { SetProperty(ref _subcategories, value); }
        //}

        //public ObservableCollection<Smartphone> Products
        //{
        //    get { return _products; }
        //    set { SetProperty(ref _products, value); }
        //}

        //public string NavigationPath
        //{
        //    get { return _navigationPath; }
        //    set { SetProperty(ref _navigationPath, value); }
        //}

        //public ICommand LoadCategoriesCommand { get; }
        //public ICommand LoadSubcategoriesCommand { get; }
        //public ICommand LoadProductsCommand { get; }

        public CatalogVM()
        {
            //LoadCategoriesCommand = new RelayCommand(async () => await LoadCategories());
            //LoadSubcategoriesCommand = new RelayCommand<int>(async (categoryId) => await LoadSubcategories(categoryId));
            //LoadProductsCommand = new RelayCommand<int>(async (subcategoryId) => await LoadProducts(subcategoryId));
        }

        //private async Task LoadCategories()
        //{
        //    // Загрузка категорий из базы данных
        //    // Если базы данных нет или она пуста, запустим парсинг
        //    Categories = await CategoryService.GetCategoriesAsync();
        //    NavigationPath = "Каталог";
        //}

        //private async Task LoadSubcategories(int categoryId)
        //{
        //    // Загрузка подкатегорий для выбранной категории
        //    // Если в БД нет подкатегорий, нужно выполнить парсинг
        //    Subcategories = await CategoryService.GetSubcategoriesAsync(categoryId);
        //    NavigationPath += " > Подкатегория"; // Обновляем навигацию
        //}

        //private async Task LoadProducts(int subcategoryId)
        //{
        //    // Загрузка продуктов для выбранной подкатегории
        //    Products = await CategoryService.GetProductsAsync(subcategoryId);
        //    NavigationPath += " > Продукты"; // Обновляем навигацию
        //}
    }
}
