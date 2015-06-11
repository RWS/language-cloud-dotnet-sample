using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocalTravelInfo.Model
{
    public class ProjectQuotesResult
    {
        public PricesDetails PricesDetails { get; set; }
    }

    public class PricesDetails
    {
        public PriceDetails Q3 { get; set; }
        public PriceDetails Q4 { get; set; }
        public PriceDetails Q5 { get; set; }
    }

    public class PriceDetails
    {
        public double? Price { get; set; }
        public int? Days { get; set; }
        public string ErrorCode { get; set; }
    }

}