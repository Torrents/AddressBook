using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

		[Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public string Email { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }

    }
}
