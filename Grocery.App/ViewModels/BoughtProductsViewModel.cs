using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class BoughtProductsViewModel : BaseViewModel
    {
        private readonly IBoughtProductsService _boughtProductsService;

        [ObservableProperty]
        Product selectedProduct;

        public ObservableCollection<BoughtProducts> BoughtProductsList { get; set; } = [];
        public ObservableCollection<Product> Products { get; set; }

        public BoughtProductsViewModel(IBoughtProductsService boughtProductsService, IProductService productService)
        {
            _boughtProductsService = boughtProductsService;
            Products = new(productService.GetAll());
        }

        partial void OnSelectedProductChanged(Product? oldValue, Product newValue)
        {
            if (newValue == null) return;

            BoughtProductsList.Clear();

            var boughtProducts = _boughtProductsService.Get(newValue.Id);

            // Debug: kijk hoeveel producten er worden gevonden
            System.Diagnostics.Debug.WriteLine($"Product selected: {newValue.Name} (ID: {newValue.Id})");
            System.Diagnostics.Debug.WriteLine($"Found {boughtProducts.Count} bought products");

            foreach (var bp in boughtProducts)
            {
                System.Diagnostics.Debug.WriteLine($"Adding: Client={bp.Client?.Name}, GroceryList={bp.GroceryList?.Name}");
                BoughtProductsList.Add(bp);
            }

            System.Diagnostics.Debug.WriteLine($"BoughtProductsList now has {BoughtProductsList.Count} items");
        }

        [RelayCommand]
        public void NewSelectedProduct(Product product)
        {
            SelectedProduct = product;
        }
    }
}