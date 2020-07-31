using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages
{
    public static class JsonExtension
    {
        public static bool CompareAndReplace<T>(this JArray patch, T origField, T newField, string fieldPath)
        {
            bool updated = false;

            if (!origField.Equals(newField))
            {
                string json;
                if (newField is string || newField is DateTime)
                {
                    json = $"{{ 'op': 'replace', 'path': '{fieldPath}', 'value': '{newField}' }}";
                }
                else
                {
                    json = $"{{ 'op': 'replace', 'path': '{fieldPath}', 'value': {newField} }}";
                }
                patch.Add(JToken.Parse(json));
                updated = true;
            }

            return updated;
        }
    }
}
