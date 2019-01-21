using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest.Fhir
{
    class FhirTest
    {
        public static void TestJson()
        {
            string body = "{'resourceType': 'Bundle' }";
            IElementNavigator navigator = JsonDomFhirNavigator.Create(body);
            foreach (var item in typeof(IElementNavigator).GetProperties())
            {
                Console.WriteLine($"{item.Name} = {item.GetValue(navigator)}");
            }
            JObject jObject = JObject.Parse(body);
            jObject.Add("birthdate", "1985-01-01");
            Console.WriteLine(jObject.ToString());
        }

        public static void TestPatient()
        {
            Patient patient = new Patient();
        }

        private FhirClient GetClientAsync(string accessToken)
        {
            var client = new FhirClient("");
            client.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) =>
            {
                e.RawRequest.Headers.Add("Authorization", $"Bearer {accessToken}");
            };
            client.PreferredFormat = ResourceFormat.Json;
            return client;
        }
    }
}
