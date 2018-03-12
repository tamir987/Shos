using Models;
using SearchEngine.com.ebay.developer;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace SearchEngine
{
    public class Engine
    {
        public ConnectToEbay Service { get; set; }
        public string ServiceURL { get; set; } = "http://svcs.ebay.com/services/search/FindingService/v1";
        public ItemFilter[] ItemFiltering { get; set; }
        public PaginationInput PagingManager { get; set; }
        public FindItemsByKeywordsRequest Request;
        public List<Item> ItemsList { get; set; }

        private void connect()
        {
            Service = new ConnectToEbay
            {
                Url = ServiceURL
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
                value = new string[] { "false" }
            };

            ItemFiltering = new ItemFilter[2];
            ItemFiltering[0] = itemFilteredByShipping;
            ItemFiltering[1] = itemFilteredByFreeShipping;
        }

        private void managePaging()
        {
            PagingManager = new PaginationInput
            {
                entriesPerPageSpecified = true,
                entriesPerPage = 100,
                pageNumberSpecified = true,
                pageNumber = 10
            };
        }

        private void buildRequest(string i_SearchKeyword)
        {
            Request = new FindItemsByKeywordsRequest
            {
                // Sorting properties
                sortOrderSpecified = true,
                sortOrder = SortOrderType.BestMatch,
                // Setting the required property values
                itemFilter = ItemFiltering,
                keywords = i_SearchKeyword
            };

            Request.paginationInput = PagingManager;
        }

        private void setList(SearchResult i_Result)
        {
            ItemsList = new List<Item>();

            foreach (SearchItem item in i_Result.item)
            {
                Item itemToAddToList = new Item
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

                ItemsList.Add(itemToAddToList);
            }
        }

        public List<Item> Search(string i_SearchKeyword)
        {
            try
            {
                connect();
                filter();
                managePaging();
                buildRequest(i_SearchKeyword);
                // Creating response object
                FindItemsByKeywordsResponse response = Service.findItemsByKeywords(Request);
                SearchResult result = response.searchResult;
                // Looping through response object for result
                setList(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("* Error: " + ex.Message);
            }

            return ItemsList;
        }

        public List<Item> Search2(string i_SearchKeyword)
        {
            try
            {
                connect();
                filter();
                managePaging();

                FindCompletedItemsRequest Request = new FindCompletedItemsRequest
                {
                    // Sorting properties
                    sortOrderSpecified = true,
                    sortOrder = SortOrderType.BestMatch,
                    // Setting the required property values
                    itemFilter = ItemFiltering,
                    keywords = i_SearchKeyword,
                };

                Request.paginationInput = PagingManager;

                FindCompletedItemsResponse response = Service.findCompletedItems(Request);
                SearchResult result = response.searchResult;
                // Looping through response object for result
                setList(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("* Error: " + ex.Message);
            }

            return ItemsList;
        }

        public List<Item> Search3(string i_SearchKeyword)
        {
            try
            {
                ConnectToEbay service = new ConnectToEbay
                {
                    Url = "http://svcs.sandbox.ebay.com/services/search/FindingService/v1"
                };

                filter();
                managePaging();
                FindItemsAdvancedRequest request = new FindItemsAdvancedRequest
                {
                    // Sorting properties
                    sortOrderSpecified = true,
                    sortOrder = SortOrderType.BestMatch,
                    // Setting the required property values
                    itemFilter = ItemFiltering,
                    keywords = i_SearchKeyword,
                    paginationInput = PagingManager
                };

                FindItemsAdvancedResponse response = service.findItemsAdvanced(request);
                setList(response.searchResult);
            }
            catch(Exception e)
            {
                Debug.Print(e.StackTrace);
            }

            return ItemsList;
        }     
    }
}
