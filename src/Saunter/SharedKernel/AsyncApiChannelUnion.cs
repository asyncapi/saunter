using System;
using System.Collections.Generic;
using System.Linq;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using Saunter.SharedKernel.Interfaces;

namespace Saunter.SharedKernel
{
    internal class AsyncApiChannelUnion : IAsyncApiChannelUnion
    {
        public AsyncApiChannel Union(AsyncApiChannel source, AsyncApiChannel additionaly)
        {
            if (source.Publish is not null && additionaly.Publish is not null)
            {
                throw new InvalidOperationException("Publish operation conflict");
            }

            if (source.Subscribe is not null && additionaly.Subscribe is not null)
            {
                throw new InvalidOperationException("Subscribe operation conflict");
            }

            if (source.Reference is not null && additionaly.Reference is not null)
            {
                throw new InvalidOperationException("Reference operation conflict");
            }

            var publishOperation = source.Publish ?? additionaly.Publish;
            var subscribeOperation = source.Subscribe ?? additionaly.Subscribe;
            var reference = source.Reference ?? additionaly.Reference;

            if (reference is not null && (publishOperation is not null || subscribeOperation is not null))
            {
                throw new InvalidOperationException("Reference operation conflict");
            }

            var servers = source.Servers?.Any() == true
                ? source.Servers
                : additionaly.Servers
                ?? new List<string>();

            var bindings = source.Bindings?.Any() == true
                ? source.Bindings
                : additionaly.Bindings
                ?? new();

            var parameters = source.Parameters?.Any() == true
                ? source.Parameters
                : additionaly.Parameters
                ?? new Dictionary<string, AsyncApiParameter>();

            var extensions = source.Extensions?.Any() == true
                ? source.Extensions
                : additionaly.Extensions
                ?? new Dictionary<string, IAsyncApiExtension>();

            return new()
            {
                Publish = publishOperation,
                Subscribe = subscribeOperation,

                Servers = servers,
                Bindings = bindings,
                Parameters = parameters,
                Extensions = extensions,

                Reference = reference,
                Description = source.Description ?? additionaly.Description,
            };
        }
    }
}
