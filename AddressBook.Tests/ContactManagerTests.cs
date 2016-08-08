using AddressBook.Dtos;
using AddressBook.EntityFramework;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Xunit;

namespace AddressBook.Tests
{
	public class ContactManagerTests 
	{
        private string _connectionString = ConfigurationManager.ConnectionStrings["AddressBook"].ConnectionString;
        private AddressBookService MyAddressBookService { get; set; }

		public ContactManagerTests()
		{
			this.MyAddressBookService = new AddressBookService(new AddressBookDbContext(this._connectionString));
            MyAddressBookService.DbContext.Database.ExecuteSqlCommand("Truncate Table [dbo].[PhoneNumbers]");
            MyAddressBookService.DbContext.Database.ExecuteSqlCommand("Delete [dbo].[Contacts]");
        }

		[Fact]
		public void AddContact_Success()
		{
			var contactDto = new ContactDto
			{
				FirstName = "Champ",
				LastName = "Bailey",
				NickName = "Shutdown Corner",
				Email = "cbailey24@dovevalley.com",
				PhoneNumbers = new List<string>() { "999-321-1234", "999.456.7898" }
			};

			var results = MyAddressBookService.ContactManager.Add(contactDto);

			Assert.True(!results.HasErrors());
			var contacts = MyAddressBookService.ContactManager.GetAll();
			var foundChamp = contacts.Any(c => c.FirstName == "Champ" && c.LastName == "Bailey");
			Assert.True(foundChamp);
		}

		[Fact]
		public void AddContact_Failure()
		{
			var contactDto = new ContactDto
			{
				LastName = "Atwater",
				NickName = "Shutdown Linebacker",
				Email = "sethAtwater@dovevalley.com"
			};

			var results = MyAddressBookService.ContactManager.Add(contactDto);

			Assert.True(results.HasErrors());
			var contacts = MyAddressBookService.ContactManager.GetAll();
			var foundSeth = contacts.Any(c => c.LastName == "Atwater");
			Assert.True(!foundSeth);
		}
	}
}
