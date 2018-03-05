using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Item
    {
        public string ItemID { get; set; } = string.Empty;

        public string ItemTitle { get; set; } = string.Empty;

        public string ItemLink { get; set; } = string.Empty;

        public string ItemProfitableLink { get; set; } = string.Empty;

        public string ItemPictureURL { get; set; } = string.Empty;

        public string ItemPrice { get; set; } = string.Empty;

        public string ItemShipingPrice { get; set; } = string.Empty;

        public string ItemSoldAmount { get; set; } = string.Empty;

        public string ItemCondition { get; set; } = string.Empty;

        public string ItemDeliveryAstimatedDate { get; set; } = string.Empty;

        public string ItemComments { get; set; } = string.Empty;

        public string ItemSourceCountry { get; set; } = string.Empty;

        public string[] ItemShipingToCountry { get; set; }

        public string ItemPriceCorency { get; set; } = string.Empty;

        public eSourceSites ItemSourceWebSite { get; set; } = eSourceSites.Default;

        public override string ToString()
        {
            string shipingToCountry = string.Empty;

            foreach (string str in this.ItemShipingToCountry)
            {
                shipingToCountry += str + " ";
            }

            return string.Format(
@"Item id: {0} 
item titel: {1} 
item price: {2} 
item link: {3} 
item 2 link: {4} 
item shipping price: {5} 
item comments: {6} 
item source country: {7} 
item shiping to country: {8} 
item price corency: {9} 
item condition: {10} 
item picture: {11}"
, this.ItemID, this.ItemTitle, this.ItemPrice, this.ItemLink, this.ItemProfitableLink, this.ItemShipingPrice, this.ItemComments, this.ItemSourceCountry, shipingToCountry, this.ItemPriceCorency, this.ItemCondition, this.ItemPictureURL);
        }
    }
}
