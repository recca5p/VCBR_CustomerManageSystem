using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCBRDemo.ImportRequests.DTOs;

namespace VCBRDemo.ImportRequests
{
    public class ImportRequestHub : Hub
    {
        public async Task SendCustomerAdded(ImportCRUDDTO response)
        {
            await Clients.All.SendAsync("New import status", response);
        }
    }
}
