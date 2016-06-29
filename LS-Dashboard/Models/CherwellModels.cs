using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Cherwell.Models
{
    public class BusinessObjectModel
    {
        [JsonProperty("busObId")]
        public Guid Id { get; set; }
        [JsonProperty("busObPublicId")]
        public int PublicId { get; set; }
        [JsonProperty("busObRecId")]
        public Guid RecId { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("hasError")]
        public bool HasError { get; set; }
        [JsonProperty("fields")]
        public List<FieldModel> Fields { get; set; }
        [JsonProperty("links")]
        public List<LinkModel> Links { get; set; }

        public string GetField(string FieldName)
        {
            var field = Fields.FirstOrDefault(f => f.Name == FieldName);
            if (field != null)
            {
                return field.Value;
            }

            return "";
        }

        public string GetField(Guid FieldId)
        {
            var field = Fields.FirstOrDefault(f => f.Id == FieldId);
            if (field != null)
            {
                return field.Value;
            }

            return "";
        }
    }

    public class LinkModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class FieldModel
    {
        [JsonProperty("fieldId")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("dirty")]
        public bool IsDirty { get; set; }
    }

    public class SearchResultsModel
    {
        [JsonProperty("totalRows")]
        public int TotalRows { get; set; }
        [JsonProperty("links")]
        public List<LinkModel> Links { get; set; }
        [JsonProperty("businessObjects")]
        public List<BusinessObjectModel> BusinessObject { get; set; }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("as:client_id")]
        public string AsClientId { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty(".issued")]
        public DateTime Issued { get; set; }
        [JsonProperty(".expires")]
        public DateTime Expires { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}