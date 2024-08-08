using LEGO.AsyncAPI.Models;
using Microsoft.Extensions.Logging.Testing;
using Saunter.SharedKernel;
using Xunit;

namespace Saunter.Tests.SharedKernel
{
    public class DocumentSerializeClonerTests
    {
        private readonly AsyncApiDocumentSerializeCloner _cloner;

        public DocumentSerializeClonerTests()
        {
            _cloner = new AsyncApiDocumentSerializeCloner(new FakeLogger<AsyncApiDocumentSerializeCloner>());
        }

        [Fact]
        public void CloneProtype_ShouldCloneDocumentSuccessfully()
        {
            // Arrange
            var prototype = new AsyncApiDocument()
            {
                Id = "id document",
                Asyncapi = "2.6.0",
                Info = new()
                {
                    Version = "1.0.0",
                    Title = "title",
                    Description = "description",
                    License = new()
                    {
                        Url = new("http://localhost:9200"),
                        Name = "test",
                    },
                    Contact = new()
                    {
                        Url = new("http://localhost:9201"),
                        Name = "contact",
                        Email = "gmail.ru",
                    },
                    TermsOfService = new("http://localhost:9202"),
                },
                Tags =
                {
                    new()
                    {
                        Name = "test",
                        Description = "descriptions",
                        ExternalDocs = new()
                        {
                            Url = new("http://localhost:9203"),
                            Description = "ExternalDocs",
                        },
                    }
                },
                DefaultContentType = "default/type",
                ExternalDocs = new()
                {
                    Url = new("http://localhost:9204"),
                    Description = "tester",
                },
                Servers =
                {
                    {
                        "one",
                        new()
                        {
                            Url = "hellowa",
                            Description = "server desc",
                            Protocol = "kaffka",
                            ProtocolVersion = "0.0.1",
                            Tags = { new() { Name = "kaffka tag" } },
                            Variables =
                            {
                                {
                                    "var",
                                    new()
                                    {
                                        Default = "default",
                                        Description = "default var",
                                        Enum = { "q", "w", "e" },
                                        Examples = { "example one" },
                                    }
                                }
                            }
                        }
                    }
                },
                Channels =
                {
                    {
                        "channel",
                        new()
                        {
                            Description = "description channel",
                            Servers = { "one" },
                            Parameters =
                            {
                                {
                                    "params" ,
                                    new()
                                    {
                                        Location = "default",
                                        Description = "default var",
                                        Schema = new(){ Type = SchemaType.String },
                                    }
                                }
                            },
                            Publish = new()
                            {
                                Description = "operation descr",
                                ExternalDocs=new()
                                {
                                    Url = new("http://localhost:9205"),
                                    Description = "tester",
                                },
                                OperationId = "1",
                                Tags = { new() { Name = "tag" } },
                                Summary = "my summary",
                                Message =
                                {
                                    new()
                                    {
                                        Summary = "message summary",
                                        Description = "message description",
                                        ContentType = "application/json",
                                        ExternalDocs = new()
                                        {
                                            Url = new("http://localhost:9206"),
                                            Description = "message tester",
                                        },
                                        MessageId = "message 1",
                                        Name = "message name 1",
                                        Title = "message title 1",
                                        Payload = new()
                                        {
                                            Deprecated = true,
                                            Description = "payload",
                                            Type = SchemaType.String,
                                            Default = new("empty"),
                                            ReadOnly = false,
                                            WriteOnly = true,
                                            Title = "title test",
                                        },
                                    },
                                },
                            },
                            Subscribe = new()
                            {
                                Description = "subscribe operation descr",
                                ExternalDocs=new()
                                {
                                    Url = new("http://localhost:9207"),
                                    Description = "subscribe tester",
                                },
                                OperationId = "subscribe 1",
                                Tags = { new() { Name = "subscribe tag" } },
                                Summary = "subscribe my summary",
                                Message =
                                {
                                    new()
                                    {
                                        MessageId = "subscribe message 1",
                                        Summary = "subscribe message summary",
                                        Description = "subscribe message description",
                                        ContentType = "application/json",
                                        ExternalDocs = new()
                                        {
                                            Url = new("http://localhost:9208"),
                                            Description = "subscribe message tester",
                                        },
                                        Name = "subscribe message name 1",
                                        Title = "subscribe message title 1",
                                        Payload = new()
                                        {
                                            Deprecated = true,
                                            Description = "subscribe payload",
                                            Type = SchemaType.String,
                                            Default = new("empty"),
                                            ReadOnly = true,
                                            WriteOnly = false,
                                            Title = "title test",
                                        },
                                    },
                                },
                            },
                        }
                    }
                }
            };

            // Act
            var result = _cloner.CloneProtype(prototype);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(prototype.Id, result.Id);
            Assert.Equal(prototype.Asyncapi, result.Asyncapi);

            // Verify Info
            Assert.Equal(prototype.Info.Version, result.Info.Version);
            Assert.Equal(prototype.Info.Title, result.Info.Title);
            Assert.Equal(prototype.Info.Description, result.Info.Description);
            Assert.Equal(prototype.Info.License.Url, result.Info.License.Url);
            Assert.Equal(prototype.Info.License.Name, result.Info.License.Name);
            Assert.Equal(prototype.Info.Contact.Url, result.Info.Contact.Url);
            Assert.Equal(prototype.Info.Contact.Name, result.Info.Contact.Name);
            Assert.Equal(prototype.Info.Contact.Email, result.Info.Contact.Email);
            Assert.Equal(prototype.Info.TermsOfService, result.Info.TermsOfService);

            // Verify DefaultContentType
            Assert.Equal(prototype.DefaultContentType, result.DefaultContentType);

            // Verify ExternalDocs
            Assert.Equal(prototype.ExternalDocs.Url, result.ExternalDocs.Url);
            Assert.Equal(prototype.ExternalDocs.Description, result.ExternalDocs.Description);

            // Verify Tags
            Assert.Equal(prototype.Tags.Count, result.Tags.Count);
            for (var i = 0; i < prototype.Tags.Count; i++)
            {
                Assert.Equal(prototype.Tags[i].Name, result.Tags[i].Name);
                Assert.Equal(prototype.Tags[i].Description, result.Tags[i].Description);
                Assert.Equal(prototype.Tags[i].ExternalDocs.Url, result.Tags[i].ExternalDocs.Url);
                Assert.Equal(prototype.Tags[i].ExternalDocs.Description, result.Tags[i].ExternalDocs.Description);
            }

            // Verify Servers
            Assert.Equal(prototype.Servers.Count, result.Servers.Count);

            foreach (var serverKey in prototype.Servers.Keys)
            {
                var prototypeServer = prototype.Servers[serverKey];
                var resultServer = result.Servers[serverKey];

                Assert.Equal(prototypeServer.Url, resultServer.Url);
                Assert.Equal(prototypeServer.Description, resultServer.Description);
                Assert.Equal(prototypeServer.Protocol, resultServer.Protocol);
                Assert.Equal(prototypeServer.ProtocolVersion, resultServer.ProtocolVersion);
                Assert.Equal(prototypeServer.Tags.Count, resultServer.Tags.Count);

                for (var i = 0; i < prototypeServer.Tags.Count; i++)
                {
                    Assert.Equal(prototypeServer.Tags[i].Name, resultServer.Tags[i].Name);
                }

                Assert.Equal(prototypeServer.Variables.Count, resultServer.Variables.Count);

                foreach (var variableKey in prototypeServer.Variables.Keys)
                {
                    var prototypeVariable = prototypeServer.Variables[variableKey];
                    var resultVariable = resultServer.Variables[variableKey];

                    Assert.Equal(prototypeVariable.Default, resultVariable.Default);
                    Assert.Equal(prototypeVariable.Description, resultVariable.Description);
                    Assert.Equal(prototypeVariable.Enum, resultVariable.Enum);
                    Assert.Equal(prototypeVariable.Examples, resultVariable.Examples);
                }
            }

            // Verify Channels
            Assert.Equal(prototype.Channels.Count, result.Channels.Count);
            foreach (var channelKey in prototype.Channels.Keys)
            {
                var prototypeChannel = prototype.Channels[channelKey];
                var resultChannel = result.Channels[channelKey];

                Assert.Equal(prototypeChannel.Description, resultChannel.Description);
                Assert.Equal(prototypeChannel.Servers, resultChannel.Servers);
                Assert.Equal(prototypeChannel.Parameters.Count, resultChannel.Parameters.Count);

                foreach (var parameterKey in prototypeChannel.Parameters.Keys)
                {
                    var prototypeParameter = prototypeChannel.Parameters[parameterKey];
                    var resultParameter = resultChannel.Parameters[parameterKey];

                    Assert.Equal(prototypeParameter.Description, resultParameter.Description);
                    Assert.Equal(prototypeParameter.Schema.Type, resultParameter.Schema.Type);
                    Assert.Equal(prototypeParameter.Location, resultParameter.Location);
                }

                // Verify Publish
                Assert.Equal(prototypeChannel.Publish.Description, resultChannel.Publish.Description);
                Assert.Equal(prototypeChannel.Publish.ExternalDocs.Url, resultChannel.Publish.ExternalDocs.Url);
                Assert.Equal(prototypeChannel.Publish.ExternalDocs.Description, resultChannel.Publish.ExternalDocs.Description);
                Assert.Equal(prototypeChannel.Publish.OperationId, resultChannel.Publish.OperationId);
                Assert.Equal(prototypeChannel.Publish.Tags.Count, resultChannel.Publish.Tags.Count);

                for (var i = 0; i < prototypeChannel.Publish.Tags.Count; i++)
                {
                    Assert.Equal(prototypeChannel.Publish.Tags[i].Name, resultChannel.Publish.Tags[i].Name);
                }

                Assert.Equal(prototypeChannel.Publish.Summary, resultChannel.Publish.Summary);
                Assert.Equal(prototypeChannel.Publish.Message.Count, resultChannel.Publish.Message.Count);

                for (var i = 0; i < prototypeChannel.Publish.Message.Count; i++)
                {
                    var prototypeMessage = prototypeChannel.Publish.Message[i];
                    var resultMessage = resultChannel.Publish.Message[i];

                    // TODO: bug?
                    // Assert.Equal(prototypeMessage.MessageId, resultMessage.MessageId);

                    Assert.Equal(prototypeMessage.Summary, resultMessage.Summary);
                    Assert.Equal(prototypeMessage.Description, resultMessage.Description);
                    Assert.Equal(prototypeMessage.SchemaFormat, resultMessage.SchemaFormat);
                    Assert.Equal(prototypeMessage.ContentType, resultMessage.ContentType);
                    Assert.Equal(prototypeMessage.ExternalDocs.Url, resultMessage.ExternalDocs.Url);
                    Assert.Equal(prototypeMessage.ExternalDocs.Description, resultMessage.ExternalDocs.Description);
                    Assert.Equal(prototypeMessage.Name, resultMessage.Name);
                    Assert.Equal(prototypeMessage.Title, resultMessage.Title);
                    Assert.Equal(prototypeMessage.Payload.Deprecated, resultMessage.Payload.Deprecated);
                    Assert.Equal(prototypeMessage.Payload.Description, resultMessage.Payload.Description);
                    Assert.Equal(prototypeMessage.Payload.Type, resultMessage.Payload.Type);
                    Assert.Equal(prototypeMessage.Payload.Default.GetValueOrDefault<string>(), resultMessage.Payload.Default.GetValueOrDefault<string>());
                    Assert.Equal(prototypeMessage.Payload.ReadOnly, resultMessage.Payload.ReadOnly);
                    Assert.Equal(prototypeMessage.Payload.WriteOnly, resultMessage.Payload.WriteOnly);
                    Assert.Equal(prototypeMessage.Payload.Title, resultMessage.Payload.Title);
                }

                // Verify Subscribe
                Assert.Equal(prototypeChannel.Subscribe.Description, resultChannel.Subscribe.Description);
                Assert.Equal(prototypeChannel.Subscribe.ExternalDocs.Url, resultChannel.Subscribe.ExternalDocs.Url);
                Assert.Equal(prototypeChannel.Subscribe.ExternalDocs.Description, resultChannel.Subscribe.ExternalDocs.Description);
                Assert.Equal(prototypeChannel.Subscribe.OperationId, resultChannel.Subscribe.OperationId);
                Assert.Equal(prototypeChannel.Subscribe.Tags.Count, resultChannel.Subscribe.Tags.Count);

                for (var i = 0; i < prototypeChannel.Subscribe.Tags.Count; i++)
                {
                    Assert.Equal(prototypeChannel.Subscribe.Tags[i].Name, resultChannel.Subscribe.Tags[i].Name);
                }

                Assert.Equal(prototypeChannel.Subscribe.Summary, resultChannel.Subscribe.Summary);
                Assert.Equal(prototypeChannel.Subscribe.Message.Count, resultChannel.Subscribe.Message.Count);

                for (var i = 0; i < prototypeChannel.Subscribe.Message.Count; i++)
                {
                    var prototypeMessage = prototypeChannel.Subscribe.Message[i];
                    var resultMessage = resultChannel.Subscribe.Message[i];

                    // TODO: bug?
                    // Assert.Equal(prototypeMessage.MessageId, resultMessage.MessageId);

                    Assert.Equal(prototypeMessage.Summary, resultMessage.Summary);
                    Assert.Equal(prototypeMessage.Description, resultMessage.Description);
                    Assert.Equal(prototypeMessage.SchemaFormat, resultMessage.SchemaFormat);
                    Assert.Equal(prototypeMessage.ContentType, resultMessage.ContentType);
                    Assert.Equal(prototypeMessage.ExternalDocs.Url, resultMessage.ExternalDocs.Url);
                    Assert.Equal(prototypeMessage.ExternalDocs.Description, resultMessage.ExternalDocs.Description);
                    Assert.Equal(prototypeMessage.Name, resultMessage.Name);
                    Assert.Equal(prototypeMessage.Title, resultMessage.Title);
                    Assert.Equal(prototypeMessage.Payload.Deprecated, resultMessage.Payload.Deprecated);
                    Assert.Equal(prototypeMessage.Payload.Description, resultMessage.Payload.Description);
                    Assert.Equal(prototypeMessage.Payload.Type, resultMessage.Payload.Type);
                    Assert.Equal(prototypeMessage.Payload.Default.GetValueOrDefault<string>(), resultMessage.Payload.Default.GetValueOrDefault<string>());
                    Assert.Equal(prototypeMessage.Payload.ReadOnly, resultMessage.Payload.ReadOnly);
                    Assert.Equal(prototypeMessage.Payload.WriteOnly, resultMessage.Payload.WriteOnly);
                    Assert.Equal(prototypeMessage.Payload.Title, resultMessage.Payload.Title);
                }
            }
        }
    }
}
