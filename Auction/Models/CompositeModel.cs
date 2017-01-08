using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auction.Models
{
    public class CompositeModel
    {
       public Item it { get; set; }
       public IEnumerable<Category>cat { get; set; }
    }
}