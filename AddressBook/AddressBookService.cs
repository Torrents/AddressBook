using AddressBook.Managers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook
{
	public class AddressBookService : IDisposable
	{

		internal DbContext DbContext { get; }

		public ContactManager ContactManager { get; }

		public PhoneNumberManager PhoneNumberManager { get; }


		public AddressBookService(DbContext dbContext)
		{
			this.DbContext = dbContext;

			this.ContactManager = new ContactManager(this);
			this.PhoneNumberManager = new PhoneNumberManager();

		}

		public void Dispose()
		{
			DbContext.Dispose();
		}
	}
}
