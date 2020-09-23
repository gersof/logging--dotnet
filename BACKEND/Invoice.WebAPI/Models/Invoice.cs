using System;
using System.Collections.Generic;

namespace Invoice.WebAPI.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceDetail = new HashSet<InvoiceDetail>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int EmployeeId { get; set; }
        public string ClientName { get; set; }

        public virtual Employee IdNavigation { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetail { get; set; }
    }
}
