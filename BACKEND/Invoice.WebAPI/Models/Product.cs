using System;
using System.Collections.Generic;

namespace Invoice.WebAPI.Models
{
    public partial class Product
    {
        public Product()
        {
            InvoiceDetail = new HashSet<InvoiceDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<InvoiceDetail> InvoiceDetail { get; set; }
    }
}
