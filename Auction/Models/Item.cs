using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;

namespace Auction.Models
{
    public class Item
    {
        public int ID { get; set; }
        [Display(Name="Name ")]
        [StringLength(60,MinimumLength=3)]
        public string Name { get; set; }
        
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [StringLength(30)]
        [Display(Name="Category")]
        public string Category { get; set; }
        [Display(Name="Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Display(Name="Rating")]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [StringLength(5)]
        public string Rating { get; set; }
        public byte[] imageData{get;set;}
        public bool Approved { get; set; }
        public string User { get; set; }
    }

    public class ItemDBContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ItemDBContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<Auction.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<Auction.Models.Auct> Aucts { get; set; }

        public System.Data.Entity.DbSet<Auction.Models.SuggestedItem> SuggestedItems { get; set; }

        public override int SaveChanges()
          {
            try
            {
              return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
              // Retrieve the error messages as a list of strings.
              var errorMessages = ex.EntityValidationErrors
                      .SelectMany(x => x.ValidationErrors)
                      .Select(x => x.ErrorMessage);

              // Join the list to a single string.
              var fullErrorMessage = string.Join("; ", errorMessages);

              // Combine the original exception message with the new one.
              var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

              // Throw a new DbEntityValidationException with the improved exception message.
              throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
          }
    }
  }
}