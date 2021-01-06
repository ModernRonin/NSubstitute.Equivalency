using System;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Core.Arguments;
using NSubstitute.Exceptions;
using NUnit.Framework;

namespace UsageTests
{
    public class EquivalencyArgumentMatcher<T> : IArgumentMatcher<T>, IDescribeNonMatches
    {
        readonly Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> _configure;
        readonly T _expectation;
        string _failedExpectations = string.Empty;

        public EquivalencyArgumentMatcher(T expectation,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> configure)
        {
            _expectation = expectation;
            _configure = configure;
        }

        public string DescribeFor(object argument) => _failedExpectations;

        public bool IsSatisfiedBy(T argument)
        {
            try
            {
                argument.Should().BeEquivalentTo(_expectation, _configure);
                return true;
            }
            catch (Exception x)
            {
                _failedExpectations = x.Message;
                return false;
            }
        }
    }

    public static class Arg
    {
        public static ref T IsEquivalentTo<T>(T value) => ref IsEquivalentTo(value, _ => _);

        public static ref T IsEquivalentTo<T>(T value,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> configure) =>
            ref ArgumentMatcher.Enqueue(new EquivalencyArgumentMatcher<T>(value, configure));
    }

    public class Tests

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
            UseService(service);

            service.Received()
                .Use(Arg.IsEquivalentTo(new Person
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
            UseService(service);

            var receivedMsg = string.Empty;
            try
            {
                service.Received()
                    .Use(Arg.IsEquivalentTo(new Person
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
	Use(NSubstitute.Core.Arguments.ArgumentMatcher+GenericToNonGenericMatcherProxyWithDescribe`1[UsageTests.Tests+Person])
Actually received no matching calls.
Received 1 non-matching call (non-matching arguments indicated with '*' characters):
	Use(*Tests+Person*)
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
            UseService(service);

            service.Received()
                .Use(Arg.IsEquivalentTo(new Person
                {
                    FirstName = "John",
                    LastName = "Doe"
                }, cfg => cfg.Excluding(p => p.Birthday)));
        }

        static void UseService(ISomeInterface service)
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