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
                ConnectToEbay service = new ConnectToEbay
                {
                    Url = "http://svcs.ebay.com/services/search/FindingService/v1"
                };
                // Creating request object for FindBestMatchItemDetailsByKeywords API
                FindItemsByKeywordsRequest request = new FindItemsByKeywordsRequest
                {
                    // Setting the required property values
                    keywords = i_SearchKeyword
                };
                // Setting the pagination
                PaginationInput pagination = new PaginationInput
                {
                    entriesPerPageSpecified = true,
                    entriesPerPage = 25,
                    pageNumberSpecified = true,
                    pageNumber = 1
                };
                request.paginationInput = pagination;
                // Creating response object
                FindItemsByKeywordsResponse response = service.findItemsByKeywords(request);
                SearchResult result = response.searchResult;
                Console.WriteLine("Find items results");
                // Looping through response object for result
                foreach (SearchItem item in result.item)
                {
                    Item itemToAddToList = new Models.Item
                    {
                        ItemID = item.itemId,
                        ItemPictureURL = item.galleryURL,
                        ItemPrice = item.sellingStatus.convertedCurrentPrice.Value.ToString(),
                        ItemTitle = item.title,
                        ItemCondition = item.condition.ToString(),
                        ItemLink = item.viewItemURL,
                        ItemProfitableLink = item.viewItemURL,
                        ItemShipingPrice = item.shippingInfo.shippingType,
                        ItemShipingToCountry = item.shippingInfo.shipToLocations,
                        ItemSourceCountry = item.location,
                        ItemComments = item.subtitle,
                        ItemPriceCorency = item.shippingInfo.shippingServiceCost.currencyId,
                        ItemSourceWebSite = eSourceSites.eBay
                    };


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
