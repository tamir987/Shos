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
        public List<Item> ItemsList { get; set; }

        private void connect()
        {
            Service = new ConnectToEbay
            {
                //Url = "http://svcs.ebay.com/services/search/FindingService/v1"
                Url = ServiceURL
            };
        }

        private void filter()
        {
            //ItemFilter itemFilteredAuthorizedSellerOnly = new ItemFilter
            //{
            //    name = ItemFilterType.AuthorizedSellerOnly,
            //    value = new string[] { "true" }
            //};
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

            ItemFiltering = new ItemFilter[2];
            ItemFiltering[0] = itemFilteredByShipping;
            ItemFiltering[1] = itemFilteredByFreeShipping;
            //ItemFiltering[2] = itemFilteredByWorldWideLocation;
        }

        private void managePaging()
        {
            PagingManager = new PaginationInput
            {
                entriesPerPageSpecified = true,
                entriesPerPage = 25,
                pageNumberSpecified = true,
                pageNumber = 1
            };
        }

        private void buildRequest(string i_SearchKeyword)
        {
            Request = new FindItemsByKeywordsRequest
            {
                //affiliate attribute
                //affiliate = new Affiliate { networkId = "9", trackingId = "5338260688" },
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

        //private void test()
        //{
        //    // create a new service
        //    Shopping svc = new Shopping();
        //    // set the URL and it's parameters
        //    // Note: Since this is a demo appid, it is very critical to replace the appid with yours to ensure the proper servicing of your application.
        //    svc.Url = "http://open.api.ebay.com/shopping?appid=eBayAPID-73f4-45f2-b9a3-c8f6388b38d8&version=523&siteid=0&callname=GetSingleItem&responseencoding=SOAP&requestencoding=SOAP";
        //    // create a new request type
        //    GetSingleItemRequestType request = new GetSingleItemRequestType();
        //    // put in your own item number
        //    request.ItemID = "280140869222";
        //    // we will request Details
        //    // for IncludeSelector reference see
        //    // http://developer.ebay.com/DevZone/shopping/docs/CallRef/GetSingleItem.html#detailControls
        //    request.IncludeSelector = "Details";
        //    // create a new response type
        //    GetSingleItemResponseType response = new GetSingleItemResponseType();
        //    try
        //    {
        //        // make the call
        //        response = svc.GetSingleItem(request);
        //    }
        //    catch (Exception ex)
        //    {
        //        // catch generic exception
        //        Console.WriteLine(ex.Message);
        //    }
        //    // output some of the data
        //    Console.WriteLine(response.Item.ItemID);
        //}
    }
}
