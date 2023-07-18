using System;
using System.Collections.Generic;
using System.Text;

namespace VCBRDemo.ImportRequests.DTOs
{
    public class ImportDataIntoDatabaseDTO
    {
        public byte[] fileBytes { get; set; }
        public List<Guid> userIds { get; set; }
    }
}
