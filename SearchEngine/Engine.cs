using Models;
using SearchEngine.com.ebay.developer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SearchEngine
{
    public class Engine
    {
        public static List<Item> Search(string i_SearchKeyword)
        {
            List<Item> itemsList = new List<Item>();
            try
            {
                // Creating an object to the BestMatchService class
                ConnectToEbay service = new ConnectToEbay();
                service.Url = "http://svcs.ebay.com/services/search/FindingService/v1";
                // Creating request object for FindBestMatchItemDetailsByKeywords API
                FindItemsByKeywordsRequest request = new FindItemsByKeywordsRequest();
                // Setting the required property values
                request.keywords = i_SearchKeyword;
                // Setting the pagination
                PaginationInput pagination = new PaginationInput();
                pagination.entriesPerPageSpecified = true;
                pagination.entriesPerPage = 25;
                pagination.pageNumberSpecified = true;
                pagination.pageNumber = 1;
                request.paginationInput = pagination;
                // Creating response object
                FindItemsByKeywordsResponse response = service.findItemsByKeywords(request);
                SearchResult result = response.searchResult;
                Console.WriteLine("Find items results");
                // Looping through response object for result
                foreach (SearchItem item in result.item)
                {
                    Item itemToAddToList = new Item();
                    itemToAddToList.ItemID = item.itemId;
                    itemToAddToList.ItemPictureURL = item.galleryURL;
                    itemToAddToList.ItemPrice = item.sellingStatus.convertedCurrentPrice.Value.ToString();
                    itemToAddToList.ItemTitle = item.title;
                    itemToAddToList.ItemCondition = item.condition.ToString();
                    itemToAddToList.ItemLink = item.viewItemURL;
                    itemToAddToList.ItemProfitableLink = item.viewItemURL;
                    itemToAddToList.ItemShipingPrice = item.shippingInfo.shippingType;
                    itemToAddToList.ItemShipingToCountry = item.shippingInfo.shipToLocations;
                    itemToAddToList.ItemSourceCountry = item.location;
                    itemToAddToList.ItemComments = item.subtitle;
                    itemToAddToList.ItemPriceCorency = item.shippingInfo.shippingServiceCost.currencyId; ;

                    itemsList.Add(itemToAddToList);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("* Error: " + ex.Message);
            }

            return itemsList;
        }
    }
}
