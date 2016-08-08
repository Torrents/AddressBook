using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AddressBook.Managers
{
	public class PhoneNumberManager
	{
		internal PhoneNumber CreatePhoneNumber(string phoneNumberString, Contact contact)
		{
			if (string.IsNullOrWhiteSpace(phoneNumberString.Trim()))
				return null;

			return new PhoneNumber { Contact = contact, Number = this.GetNumbers(phoneNumberString) };
		}

		private string GetNumbers(string numberString)
		{
			return new string(numberString.Where(c => char.IsDigit(c)).ToArray());
		}
	}
}
