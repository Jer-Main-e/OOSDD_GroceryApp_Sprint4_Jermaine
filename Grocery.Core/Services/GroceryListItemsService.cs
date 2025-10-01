using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            List<BestSellingProducts> bestSellingProductsList = new List<BestSellingProducts>();

            var allGroceries = _groceriesRepository.GetAll().GroupBy(g => g.ProductId);
            int hoeveelheid;
            foreach ( var g in allGroceries)
            {
                //We willen niet meer dan 5 producten in de best verkochte productenlijst zien, 
                //dus daarom breekt de loop wanneer er 5 of meer producten in de BestSellingProducts lijst staan.
                if (bestSellingProductsList.Count > topX)
                {
                    break;
                }
                var firstGrocery = g.First();
                hoeveelheid = 0;
                foreach (var value in g)
                {
                    hoeveelheid += value.Amount;
                }
                //Men haalt de standaard gegevens van een bepaald type product op met behulp van de _productRepository, 
                //omdat deze gegevens niet op te halen zijn met de _groceriesRepository.
                bestSellingProductsList.Add(new BestSellingProducts(firstGrocery.ProductId, 
                    _productRepository.Get(firstGrocery.ProductId).Name, 
                    _productRepository.Get(firstGrocery.ProductId).Stock, 
                    hoeveelheid, 
                    0));
            }

            bestSellingProductsList = bestSellingProductsList.OrderByDescending(p => p.NrOfSells).ToList();

            foreach (var product in bestSellingProductsList)
            {
                product.Ranking = bestSellingProductsList.IndexOf(product) + 1;
            }

            return bestSellingProductsList;
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}
