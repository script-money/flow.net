﻿using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Models;
using Google.Protobuf;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Extensions
{
    public static class CadenceExtensions
    {
        public static string Encode(this ICadence cadence)
        {
            return JsonConvert.SerializeObject(cadence);
        }

        public static ICadence Decode(this string cadenceJson, CadenceConverter cadenceConverter = null)
        {
            return JsonConvert.DeserializeObject<ICadence>(cadenceJson, cadenceConverter ?? new CadenceConverter());
        }

        public static T AsCadenceType<T>(this ICadence cadence) where T : ICadence
        {
            return (T)cadence;
        }

        public static IList<ByteString> ToTransactionArguments(this IEnumerable<ICadence> cadenceValues)
        {
            var arguments = new List<ByteString>();

            if (cadenceValues != null && cadenceValues.Count() > 0)
            {
                foreach (var value in cadenceValues)
                {
                    var serialized = value.Encode();
                    arguments.Add(serialized.FromStringToByteString());
                }
            }

            return arguments;
        }

        public static string AccountCreatedAddress(this IList<FlowEvent> flowEvents)
        {
            return flowEvents.Where(w => w.Type == "flow.AccountCreated")
                    .FirstOrDefault().Payload.AsCadenceType<CadenceComposite>()
                    .Value.Fields.FirstOrDefault().Value.AsCadenceType<CadenceAddress>()
                    .Value.Remove0x();
        }
    }
}