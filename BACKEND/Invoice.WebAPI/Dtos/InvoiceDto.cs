using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.WebAPI.Dtos
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string ClientName { get; set; }
        public List<InvoiceDetailDto> InvoiceDetail { get; set; }

    }
}
