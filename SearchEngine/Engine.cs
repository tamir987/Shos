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

                // Filtering the items by Available to Israel
                ItemFilter itemFilteredByShipping = new ItemFilter { name = ItemFilterType.AvailableTo,
                    value = new string[] { "IL" } };
                // Filtering the items by Free shipping only
                ItemFilter itemFilteredByFreeShipping = new ItemFilter
                {
                    name = ItemFilterType.FreeShippingOnly,
                    value = new string[] {"true"}
                };
                ItemFilter itemFilteredByWorldWideLocation = new ItemFilter
                {
                    name = ItemFilterType.LocatedIn,
                    value = new string[] { "WorldWide" }
                };
                ItemFilter [] currentItemFilter = new ItemFilter[3];
                currentItemFilter[0] = itemFilteredByShipping;
                currentItemFilter[1] = itemFilteredByFreeShipping;
                currentItemFilter[2] = itemFilteredByWorldWideLocation;
                // Creating request object for FindBestMatchItemDetailsByKeywords API
                FindItemsByKeywordsRequest request = new FindItemsByKeywordsRequest
                {
                    sortOrderSpecified = true,
                    sortOrder = SortOrderType.BestMatch,
                    // Setting the required property values
                    itemFilter = currentItemFilter,
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
                        ItemShipingPrice = item.shippingInfo.shippingServiceCost.Value.ToString(),
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
