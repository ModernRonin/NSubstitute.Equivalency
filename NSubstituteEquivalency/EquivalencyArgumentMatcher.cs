using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using NSubstitute.Core;
using NSubstitute.Core.Arguments;

namespace NSubstitute.Equivalency
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
            using (var scope = new AssertionScope())
            {
                argument.Should().BeEquivalentTo(_expectation, _configure);
                _failedExpectations = string.Join(Environment.NewLine, scope.Discard());
                return !_failedExpectations.Any();
            }
        }
    }
}