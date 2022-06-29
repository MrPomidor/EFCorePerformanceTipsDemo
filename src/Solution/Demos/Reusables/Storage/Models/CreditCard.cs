using System;
using System.Collections.Generic;

namespace Reusables.Storage
{
    /// <summary>
    /// Customer credit card information.
    /// </summary>
    public partial class CreditCard
    {
        public CreditCard()
        {
            PersonCreditCards = new HashSet<PersonCreditCard>();
            SalesOrderHeaders = new HashSet<SalesOrderHeader>();
        }

        /// <summary>
        /// Primary key for CreditCard records.
        /// </summary>
        public int CreditCardId { get; set; }
        /// <summary>
        /// Credit card name.
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// Credit card number.
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// Credit card expiration month.
        /// </summary>
        public byte ExpMonth { get; set; }
        /// <summary>
        /// Credit card expiration year.
        /// </summary>
        public short ExpYear { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<PersonCreditCard> PersonCreditCards { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}
