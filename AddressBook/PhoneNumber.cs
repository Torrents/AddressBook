using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook
{
    public class PhoneNumber
    {
        [Key]
        public int Id { get; set; }

        public int ContactId { get; set; }

        [JsonIgnore]
        public virtual Contact Contact { get; set; }

        public string Number { get; set; }

    }
}
