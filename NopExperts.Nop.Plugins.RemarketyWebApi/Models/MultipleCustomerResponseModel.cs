﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models
{
    public class MultipleCustomerResponseModel
    {
        [JsonProperty("customers")]
        public List<CustomerResponseModel> Customers { get; set; }
    }
}