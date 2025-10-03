
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        public List<BoughtProducts> Get(int? productId)
        {
            List<BoughtProducts> boughtProductsList = new List<BoughtProducts>();

            //In BoughtProductsService werk je de Get(productid) functie uit,
            //zodat alle Clients die product met productid hebben gekocht met client,
            //boodschappenlijst en product in de lijst staan die wordt geretourneerd.

            //Men gaat met het productID bekijken in welke boodschappen lijst het
            //product zich bevindt. Vervolgens word er gekeken van welke client deze
            //boodschappen lijst is. Wanneer de lijst van een user is die niet de rol
            //Admin heeft, dan wordt de boodschappenlijst aan de boughtProductList toegevoegd
            //samen met het Client object en het product.

            //voordat men door de lijst met gekochte producten gaat kijken gaat men eerst
            //bepalen welke lijsten van Admins zijn, want deze gegevens willen we niet opslaan.
            var allViableClients = _clientRepository.GetAll().Where(client => client.UserRole == Client.Role.None).ToList();

            

            var allViableGroceryListsIds = _groceryListRepository.GetAll().Where(groceryList => allViableClients.Select(vC => vC.Id).Contains(groceryList.ClientId)).Select(groceryList => groceryList.Id).ToList();


            foreach(int id in allViableGroceryListsIds)
            {
                var currentGroceryList = _groceryListRepository.Get(id);
                var allViableProducts = _groceryListItemsRepository.GetAllOnGroceryListId(id);
                foreach (var viableProduct in allViableProducts) 
                {
                    if (viableProduct.ProductId != productId) { 
                        continue;
                    }
                    BoughtProducts boughtProduct = new BoughtProducts(_clientRepository.Get(currentGroceryList.ClientId), currentGroceryList, viableProduct.Product);
                    boughtProductsList.Add(boughtProduct);
                }
            }
            return boughtProductsList;
        }
    }
}
