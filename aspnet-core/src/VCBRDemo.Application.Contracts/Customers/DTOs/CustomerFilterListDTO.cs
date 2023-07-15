using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace VCBRDemo.Customers.DTOs
{
    public class CustomerFilterListDTO : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
    }
}
