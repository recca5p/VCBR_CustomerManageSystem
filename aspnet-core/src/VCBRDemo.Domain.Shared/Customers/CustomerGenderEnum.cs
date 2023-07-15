using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace VCBRDemo.Customers
{
    public enum CustomerGenderEnum
    {
        [EnumMember(Value = "0")]
        Male = 0,
        [EnumMember(Value = "1")]
        Female = 1,
    }
}
