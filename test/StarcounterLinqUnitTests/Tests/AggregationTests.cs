﻿using System.Linq;
using Starcounter.Nova;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace StarcounterLinqUnitTests.Tests
{
    public class AggregationTests : IClassFixture<BaseTestsFixture>
    {
        public AggregationTests(BaseTestsFixture fixture)
        {
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void AverageInteger(Mode mode)
        {
            Db.Transact(() =>
            {
                var avg = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Average(x => x.Age))()
                    : Objects<Person>().Average(x => x.Age);

                Assert.Equal(36, avg);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MinInteger(Mode mode)
        {
            var min = mode == Mode.CompiledQuery
                ? CompileQuery(() => Objects<Person>().Min(x => x.Age))()
                : Objects<Person>().Min(x => x.Age);
            Assert.Equal(31, min);
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void MaxInteger(Mode mode)
        {
            var max = mode == Mode.CompiledQuery
                ? CompileQuery(() => Objects<Person>().Max(x => x.Age))()
                : Objects<Person>().Max(x => x.Age);
            Assert.Equal(41, max);
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SumInteger(Mode mode)
        {
            var sum = mode == Mode.CompiledQuery
                ? CompileQuery(() => Objects<Person>().Sum(x => x.Age))()
                : Objects<Person>().Sum(x => x.Age);
            Assert.Equal(72, sum);
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Count(Mode mode)
        {
            var cnt = mode == Mode.CompiledQuery
                ? CompileQuery(() => Objects<Person>().Count())()
                : Objects<Person>().Count();
            Assert.Equal(2, cnt);
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void CountWithPredicate(Mode mode)
        {
            var cnt = mode == Mode.CompiledQuery
                ? CompileQuery(() => Objects<Person>().Count(x => x is Employee))()
                : Objects<Person>().Count(x => x is Employee);
            Assert.Equal(2, cnt);
        }
    }
}