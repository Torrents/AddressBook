﻿using AddressBook.Dtos;
using AddressBook.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AddressBook.Web.Controllers
{
	public class ContactsController : ApiController
	{
        private string _connectionString = ConfigurationManager.ConnectionStrings["AddressBook"].ConnectionString;

        public AddressBookService AddressBookService { get; }

        // Not working with ApiController
		//public ContactController(AddressBookService addressBookService)
		//{
		//	this.AddressBookService = addressBookService;
		//}

		// ToDo: Update to use injected service
		// see http://blog.ploeh.dk/2012/09/28/DependencyInjectionandLifetimeManagementwithASP.NETWebAPI/
		public ContactsController()
		{
			AddressBookService = new AddressBookService(new AddressBookDbContext(_connectionString));
		}

		// GET: api/Contacts
		public string Get()
		{
            var contacts = AddressBookService.ContactManager.GetAll();

            return JsonConvert.SerializeObject(contacts);
		}

		// GET: api/Contact/5
		public string Get(int id)
		{
			return "value";
		}

		// POST: api/Contacts
		public string Post(ContactDto contactDto)
		{
            var result = AddressBookService.ContactManager.Add(contactDto);
            return JsonConvert.SerializeObject(result);
		}

		// PUT: api/Contact/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE: api/Contact/5
		public void Delete(int id)
		{
		}
	}
}
