using System;
using System.Collections.Generic;

namespace Reusables.Storage
{
    /// <summary>
    /// Stores (including store contacts) that sell Adventure Works Cycles products to consumers.
    /// </summary>
    public partial class VStoreWithContact
    {
        public int BusinessEntityId { get; set; }
        public string Name { get; set; }
        public string ContactType { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberType { get; set; }
        public string EmailAddress { get; set; }
        public int EmailPromotion { get; set; }
    }
}
