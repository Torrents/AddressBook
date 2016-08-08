using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Dtos
{
	public class ContactDto
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string NickName { get; set; }

		public string Email { get; set; }

		public IList<string> PhoneNumbers { get; set; }
	}
}
