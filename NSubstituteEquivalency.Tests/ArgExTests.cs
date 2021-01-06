using System;
using FluentAssertions;
using NSubstitute.Exceptions;
using NUnit.Framework;

namespace NSubstitute.Equivalency.Tests
{
    public class ArgExTests
    {
        public class Person
        {
            public DateTime Birthday { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public interface ISomeInterface
        {
            void Use(Person person);
        }

        [Test]
        public void If_equivalency_is_given()
        {
            var service = Substitute.For<ISomeInterface>();
            DoSomethingWith(service);

            service.Received()
                .Use(ArgEx.IsEquivalentTo(new Person
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Birthday = new DateTime(1968, 6, 1)
                }));
        }

        [Test]
        public void If_equivalency_is_not_given()
        {
            var service = Substitute.For<ISomeInterface>();
            DoSomethingWith(service);

            var receivedMsg = string.Empty;
            try
            {
                service.Received()
                    .Use(ArgEx.IsEquivalentTo(new Person
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Birthday = new DateTime(1968, 7, 1)
                    }));
            }
            catch (ReceivedCallsException x)
            {
                receivedMsg = x.Message;
            }

            var expected = split(@"Expected to receive a call matching:
	Use(NSubstitute.Core.Arguments.ArgumentMatcher+GenericToNonGenericMatcherProxyWithDescribe`1[NSubstitute.Equivalency.Tests.ArgExTests+Person])
Actually received no matching calls.
Received 1 non-matching call (non-matching arguments indicated with '*' characters):
	Use(*ArgExTests+Person*)
		arg[0]: Expected member Birthday to be <1968-07-01>, but found <1968-06-01>.");

            split(receivedMsg).Should().StartWith(expected);

            string[] split(string what) =>
                what.Split(new[]
                {
                    '\n',
                    '\r'
                }, StringSplitOptions.RemoveEmptyEntries);
        }

        [Test]
        public void With_custom_equivalency_configuration()
        {
            var service = Substitute.For<ISomeInterface>();
            DoSomethingWith(service);

            service.Received()
                .Use(ArgEx.IsEquivalentTo(new Person
                {
                    FirstName = "John",
                    LastName = "Doe"
                }, cfg => cfg.Excluding(p => p.Birthday)));
        }

        [Test]
        public void Without_ArgEx()
        {
            var service = Substitute.For<ISomeInterface>();
            Person received = null;
            service.WhenForAnyArgs(s => s.Use(null)).Do(ci => received = ci.Arg<Person>());

            DoSomethingWith(service);

            received.Should()
            .BeEquivalentTo(new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Birthday = new DateTime(1968, 6, 1)
            });
        }

        static void DoSomethingWith(ISomeInterface service)
        {
            service.Use(new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Birthday = new DateTime(1968, 6, 1)
            });
        }
    }
}