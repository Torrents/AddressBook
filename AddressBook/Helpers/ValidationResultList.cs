using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Helpers
{
	public class ValidationResultList : IEnumerable<ValidationResult>
	{
		private List<ValidationResult> _validationResults = new List<ValidationResult>();

		public IEnumerator<ValidationResult> GetEnumerator()
		{
			return this._validationResults.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(ValidationResult validationResult)
		{
			this._validationResults.Add(validationResult);
		}

		public void Add(string errorMessage)
		{
			this._validationResults.Add(new ValidationResult(errorMessage));
		}

		public bool HasErrors()
		{
			return this._validationResults.Any();
		}

	}
}
