using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.Models.Requests.ThirdParties.Pasargad
{
    public class PasargadBankGetTokenRq
    {
        public IEnumerable<string> Scopes { get; set; }

        public string Name { get; set; }

    }
}
