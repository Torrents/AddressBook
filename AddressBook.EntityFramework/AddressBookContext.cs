using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.EntityFramework
{
    public class AddressBookDbContext : DbContext
    {
        public AddressBookDbContext(string connectionString)
            : base(connectionString)
        { }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhoneNumber>()
                .HasRequired(p => p.Contact)
                .WithMany(c => c.PhoneNumbers)
                .WillCascadeOnDelete(false);
                
        }
    }
}
