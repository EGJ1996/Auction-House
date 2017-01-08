using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auction.Models
{
    public class Auct
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Rating { get; set; }
        public DateTime startTime { get; set; }
        public decimal highestBid { get; set; }
        public string highestUser { get; set; }
        public int itemId { get; set; }
    }
}