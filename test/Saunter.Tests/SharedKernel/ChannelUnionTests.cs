using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using Saunter.SharedKernel;
using Shouldly;
using Xunit;

namespace Saunter.Tests.SharedKernel
{
    public class ChannelUnionTests
    {
        public static IEnumerable<object[]> GetOnUnionConflictData()
        {
            yield return new AsyncApiChannel[]
            {
                new() { Publish = new() },
                new() { Publish = new() },
            };

            yield return new AsyncApiChannel[]
            {
                new() { Subscribe = new() },
                new() { Subscribe = new() },
            };

            yield return new AsyncApiChannel[]
            {
                new() { Reference = new() },
                new() { Reference = new() },
            };

            yield return new AsyncApiChannel[]
            {
                new() { Reference = new() },
                new() { Subscribe = new() },
            };

            yield return new AsyncApiChannel[]
            {
                new() { Reference = new() },
                new() { Publish = new() },
            };
        }

        [Theory]
        [MemberData(nameof(GetOnUnionConflictData))]
        public void AsyncApiChannelUnion_OnUnion_Conflict(AsyncApiChannel source, AsyncApiChannel additionaly)
        {
            // Arrange
            AsyncApiChannelUnion channelUnion = new();

            // Act
            var actual = () => channelUnion.Union(source, additionaly);

            // Assert
            Should.Throw<InvalidOperationException>(actual);
        }

        public static IEnumerable<object[]> GetOnUnionSuccessMerge()
        {
            yield return new AsyncApiChannel[]
            {
                new() { },
                new() { Publish = new(), Subscribe = new() },
                new() { Publish = new(), Subscribe = new() },
            };
            yield return new AsyncApiChannel[]
            {
                new() { Publish = new(), Subscribe = new() },
                new() { },
                new() { Publish = new(), Subscribe = new() },
            };
            yield return new AsyncApiChannel[]
            {
                new() { Publish = new() },
                new() { Subscribe = new() },
                new() { Publish = new(), Subscribe = new() },
            };
            yield return new AsyncApiChannel[]
            {
                new() { Subscribe = new() },
                new() { Publish = new() },
                new() { Publish = new(), Subscribe = new() },
            };
            yield return new AsyncApiChannel[]
            {
                new() { Reference = new() },
                new() { },
                new() { Reference = new() },
            };
            yield return new AsyncApiChannel[]
            {
                new() { },
                new() { Reference = new() },
                new() { Reference = new() },
            };
            yield return new AsyncApiChannel[]
            {
                new()
                {
                    Description = "description",
                    Servers = new List<string>() { "server1", "server2", },
                    Parameters = new Dictionary<string, AsyncApiParameter>()
                    {
                        { "test", new() { Description = "description" } }
                    },
                },
                new() { },
                new()
                {
                    Description = "description",
                    Servers = new List<string>() { "server1", "server2", },
                    Parameters = new Dictionary<string, AsyncApiParameter>()
                    {
                        { "test", new() { Description = "description" } }
                    },
                },
            };
        }

        [Theory]
        [MemberData(nameof(GetOnUnionSuccessMerge))]
        public void AsyncApiChannelUnion_OnUnion_SuccessMerge(AsyncApiChannel source, AsyncApiChannel additionaly, AsyncApiChannel expected)
        {
            // Arrange
            AsyncApiChannelUnion channelUnion = new();

            // Act
            var actual = channelUnion.Union(source, additionaly);

            // Assert
            actual.ShouldNotBeNull();

            actual.Description.ShouldBe(expected.Description);
            actual.Servers.ShouldBe(expected.Servers);

            actual.Parameters.Count.ShouldBe(expected.Parameters.Count);

            foreach (var item in expected.Parameters)
            {
                actual.Parameters.ShouldContainKey(item.Key);
            }

            if (actual.Publish is null)
            {
                expected.Publish.ShouldBeNull();
            }
            else
            {
                expected.Publish.ShouldNotBeNull();
                actual.Publish.OperationId.ShouldBe(expected.Publish.OperationId);
            }

            if (actual.Subscribe is null)
            {
                expected.Subscribe.ShouldBeNull();
            }
            else
            {
                expected.Subscribe.ShouldNotBeNull();
                actual.Subscribe.OperationId.ShouldBe(expected.Subscribe.OperationId);
            }

            if (actual.Reference is null)
            {
                expected.Reference.ShouldBeNull();
            }
            else
            {
                expected.Reference.ShouldNotBeNull();
                actual.Reference.Id.ShouldBe(expected.Reference.Id);
                actual.Reference.Type.ShouldBe(expected.Reference.Type);
            }
        }
    }
}
