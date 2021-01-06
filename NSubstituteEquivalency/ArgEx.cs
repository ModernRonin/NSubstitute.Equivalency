using System;
using FluentAssertions.Equivalency;
using NSubstitute.Core.Arguments;

namespace NSubstitute.Equivalency
{
    public static class ArgEx
    {
        public static ref T IsEquivalentTo<T>(T value) => ref IsEquivalentTo(value, _ => _);

        public static ref T IsEquivalentTo<T>(T value,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> configure) =>
            ref ArgumentMatcher.Enqueue(new EquivalencyArgumentMatcher<T>(value, configure));
    }
}