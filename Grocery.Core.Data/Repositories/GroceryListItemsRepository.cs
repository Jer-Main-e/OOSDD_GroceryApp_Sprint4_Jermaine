using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using System.Collections.Generic;

namespace Grocery.Core.Data.Repositories
{
    public class GroceryListItemsRepository : IGroceryListItemsRepository
    {
        private readonly List<GroceryListItem> groceryListItems;

        public GroceryListItemsRepository()
        {
            groceryListItems = [
                new GroceryListItem(1, 1, 1, 3),
                new GroceryListItem(2, 1, 2, 1),
                new GroceryListItem(3, 1, 3, 4),
                new GroceryListItem(4, 2, 1, 2),
                new GroceryListItem(5, 2, 2, 5),
            ];
        }

        public List<GroceryListItem> GetAll()
        {
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int id)
        {
            return groceryListItems.Where(g => g.GroceryListId == id).ToList();
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            int newId = groceryListItems.Max(g => g.Id) + 1;
            item.Id = newId;
            groceryListItems.Add(item);
            return Get(item.Id);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return groceryListItems.FirstOrDefault(g => g.Id == id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            GroceryListItem? listItem = groceryListItems.FirstOrDefault(i => i.Id == item.Id);
            listItem = item;
            return listItem;
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {

            List<BestSellingProducts> bestSellingProductsList = new List<BestSellingProducts>();

            //groceryListItems.OrderByDescending(p => p.Amount).Take(topX).ToList();
            var tmpList = groceryListItems.GroupBy(item => item.ProductId);

            foreach (var group in tmpList)
            {
                var distinctId = group.Key;
                var count = group.Count();
                //gO is in dit geval group Object, oftwel een representatief object van de groepering van objecten.
                var gO = group.First();
                BestSellingProducts product = new BestSellingProducts(gO.ProductId, gO.Name, gO.Amount, count, 0);
                bestSellingProductsList.Add(product);
            }

            bestSellingProductsList.OrderByDescending(bSP => bSP.NrOfSells);

            foreach (var product in bestSellingProductsList)
            {
                product.Ranking = bestSellingProductsList.IndexOf(product) + 1;
            }


            return bestSellingProductsList;

        //    public BestSellingProducts(int productId, string name, int stock, int nrOfSells, int ranking) : base(productId, name)
        //{
        //    Stock = stock;
        //    NrOfSells = nrOfSells;
        //    Ranking = ranking;
        //}

    }
    }
}
