namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models
{
    public class Contact
    {
        public Contact() {}

        public Contact(Contact contact)
        {
            Name = contact.Name;
            Department = contact.Department;
        }

        public int ContactId { get; set; }

        public string Name { get; set; }

        public string Department { get; set; }

    }
}
