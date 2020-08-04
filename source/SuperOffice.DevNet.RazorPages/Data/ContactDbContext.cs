using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Data
{
    public class ContactDbContext : DbContext
    {
        private readonly IHttpRestClient _restClient;

        public ContactDbContext(DbContextOptions<ContactDbContext> options, IHttpRestClient restClient)
            : base(options)
        {
            _restClient = restClient;
        }

        public DbSet<Contact> Contacts { get; set; }

        public async Task Delete(int? contactId)
        {
            var foundContact = await Contacts.FindAsync(contactId);

            if (foundContact != null) // Contact.Delete(_client))
            {
                //var contactCopy = new Contact(foundContact);

                //204 ContactEntity deleted.
                //412 Delete aborted because ContactEntity has changed since the requested If-Unmodified - Since timestamp.

                // must delete the local copy first, otherwise exception is DisposedException is thrown.

                Contacts.Remove(foundContact);
                await SaveChangesAsync();

                var deletedContact = await _restClient.Delete($"/v1/Contact/{contactId}");
            }
        }

        public async Task<Contact> Save(Contact contact)
        {
            // save contact to SuperOffice 
            var obj = JObject.FromObject(contact);
            var jsonContact = new StringContent(obj.ToString(), System.Text.Encoding.UTF8, "application/json");
            var contactResponse = await _restClient.Post("/v1/Contact", jsonContact);
            var newContact = JsonConvert.DeserializeObject<Contact>(contactResponse);

            // add saved contact to local dbcontext
            Contacts.Add(newContact);
            await SaveChangesAsync();

            

            return newContact;
        }

        public async Task<IList<Contact>> GetAllContacts()
        {
            // get all contacts from SuperOffice
            var contacts = await _restClient.Get("/v1/Contact?$select=contactId,name,department"); //$top=611
            var json = JObject.Parse(contacts);
            var contactList = JsonConvert.DeserializeObject<List<Contact>>(json["value"].ToString());

            // save all contacts to local dbcontext
            Contacts.AddRange(contactList);
            await SaveChangesAsync();

            return contactList;
        }

        public async Task<Contact> Update(int contactId, JArray patch)
        {
            // update contact in SuperOffice
            var jsonContent = new StringContent(patch.ToString(), System.Text.Encoding.UTF8, "application/json");
            var patchResult = await _restClient.Patch($"/v1/Contact/{contactId}", content: jsonContent);
            var contact = JsonConvert.DeserializeObject<Contact>(patchResult);


            // update dbcontext with updated contact
            Attach(contact).State = EntityState.Modified;

            try
            {
                await SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(contact.ContactId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return contact;
        }

        public async Task<Contact> CreateDefault()
        {
            // create a new contact to present in the Create View.
            var contactString = await _restClient.Get("/v1/Contact/default");
            return JsonConvert.DeserializeObject<Contact>(contactString);
        }


        public bool TryGetChanges(Contact oldContact, Contact newContact, out JArray patch)
        {
            // pull out only the fields needed to update and add those changes as a PATCH structure
            patch = JArray.Parse("[]");
            
            var nameUpdated = patch.CompareAndReplace(oldContact.Name, newContact.Name, "/Name");
            var deptUpdated = patch.CompareAndReplace(oldContact.Department, newContact.Department, "/Department");
            return nameUpdated || deptUpdated;
        }

        /// <summary>
        /// Check if a contact exists by ID.
        /// </summary>
        /// <param name="id">Contact Id value.</param>
        /// <returns></returns>
        private bool ContactExists(int id)
        {
            return Contacts.Any(e => e.ContactId == id);
        }
    }
}
