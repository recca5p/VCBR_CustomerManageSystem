using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.Customers.DTOs;

namespace VCBRDemo.Customers
{
    public class CustomerHub : Hub
    {
        public async Task SendCustomerAdded(CustomerDTO customer)
        {
            await Clients.All.SendAsync("CustomerAdded", customer);
        }
    }
}
