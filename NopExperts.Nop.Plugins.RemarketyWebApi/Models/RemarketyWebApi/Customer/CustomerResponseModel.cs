using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Customer
{
    public class CustomerResponseModel
    {
        public CustomerResponseModel()
        {
            Groups = new List<GroupModel>();
        }

        [JsonProperty("accepts_marketing")]
        public bool AcceptsMarketing { get; set; }

        [JsonProperty("birthdate")]
        public string BirthDate { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("verified_email")]
        public bool? VerifiedEmail { get; set; }


        [JsonProperty("default_address")]
        public AddressModel DefaultAddress { get; set; }

        [JsonProperty("groups")]
        public IList<GroupModel> Groups { get; set; }
        [JsonProperty("info")]
        public object Info { get; set; }


        public class GroupModel
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
        }


    }

}