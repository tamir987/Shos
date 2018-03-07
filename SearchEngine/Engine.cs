using Models;
using SearchEngine.com.ebay.developer;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SearchEngine
{
    public class Engine
    {
        public ConnectToEbay Service { get; set; }
        public string ServiceURL { get; set; } = "http://svcs.ebay.com/services/search/FindingService/v1";
        public ItemFilter[] ItemFiltering { get; set; }
        public PaginationInput PagingManager { get; set; }
        public FindItemsByKeywordsRequest Request;

        private void connect()
        {
            Service = new ConnectToEbay
            {
                Url = "http://svcs.ebay.com/services/search/FindingService/v1"
            };
        }

        private void filter()
        {
            // Filtering the items by Available to Israel
            ItemFilter itemFilteredByShipping = new ItemFilter
            {
                name = ItemFilterType.AvailableTo,
                value = new string[] { "IL" }
            };
            // Filtering the items by Free shipping only
            ItemFilter itemFilteredByFreeShipping = new ItemFilter
            {
                name = ItemFilterType.FreeShippingOnly,
                value = new string[] { "true" }
            };
            // Filtering the items by WorldWide location
            ItemFilter itemFilteredByWorldWideLocation = new ItemFilter
            {
                name = ItemFilterType.LocatedIn,
                value = new string[] { "WorldWide" }
            };

            ItemFiltering = new ItemFilter[3];
            ItemFiltering[0] = itemFilteredByShipping;
            ItemFiltering[1] = itemFilteredByFreeShipping;
            ItemFiltering[2] = itemFilteredByWorldWideLocation;
        }

        private void managePaging()
        {
            PagingManager = new PaginationInput
            {
                entriesPerPageSpecified = true,
                entriesPerPage = 250,
                pageNumberSpecified = true,
                pageNumber = 1
            };
        }

        private void buildRequest(string i_SearchKeyword)
        {
            Request = new FindItemsByKeywordsRequest
            {
                sortOrderSpecified = true,
                affiliate = new Affiliate { networkId = "9", trackingId = "5338260688" },
                sortOrder = SortOrderType.BestMatch,
                // Setting the required property values
                itemFilter = ItemFiltering,
                keywords = i_SearchKeyword
            };

            Request.paginationInput = PagingManager;
        }

        public List<Item> Search(string i_SearchKeyword)
        {
            List<Item> itemsList = new List<Item>();

            try
            {
                connect();
                filter();
                managePaging();
                buildRequest(i_SearchKeyword);
                
                // Creating response object
                FindItemsByKeywordsResponse response = Service.findItemsByKeywords(Request);
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
                        ItemPriceCorency = item.sellingStatus.currentPrice.currencyId,
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
