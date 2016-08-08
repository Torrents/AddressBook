using AddressBook.Dtos;
using AddressBook.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Managers
{
	public class ContactManager
	{
		private AddressBookService Service { get; set; }

		public ContactManager(AddressBookService service)
		{
			Service = service;
		}

		public IList<Contact> GetAll()
		{
            var contacts = Service.DbContext.Set<Contact>().Include(c => c.PhoneNumbers);
            return contacts.ToList();
		}

		public ValidationResultList Add(ContactDto contactDto)
		{
			var validationResults = new ValidationResultList();

			if (string.IsNullOrWhiteSpace(contactDto.FirstName))
				validationResults.Add(new ValidationResult($"{nameof(Contact.FirstName)} cannot be blank", new List<string>() { nameof(Contact.FirstName) }));

			if (validationResults.HasErrors())
				return validationResults;
			
			var contact = new Contact
			{
				FirstName = contactDto.FirstName,
				LastName = contactDto.LastName,
				NickName = contactDto.NickName,
				Email = contactDto.Email
			};

			var phoneNumbers = new List<PhoneNumber>();
			foreach (var phoneString in contactDto.PhoneNumbers)
			{
				var phoneNumber = Service.PhoneNumberManager.CreatePhoneNumber(phoneString, contact);
				if (phoneNumber != null)
					phoneNumbers.Add(phoneNumber);
			}

			if (phoneNumbers.Count > 0)
				contact.PhoneNumbers = phoneNumbers;

			Service.DbContext.Set<Contact>().Add(contact);
			if (Service.DbContext.SaveChanges() == 0)
				validationResults.Add($"An error occurred while trying to save Contact \"{contactDto.FirstName} {contactDto.LastName}\"");

			return validationResults;
		}

	}
}
