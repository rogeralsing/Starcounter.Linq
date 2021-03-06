﻿using System;
using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    public class LinqTests : IClassFixture<BaseTestsFixture>
    {
        private readonly BaseTestsFixture _fixture;

        public LinqTests(BaseTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Any_Predicate(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                bool any = mode == Mode.CompiledQuery
                    ? CompileQuery((int age) => Objects<Person>().Any(x => x.Age > age))(0)
                    : Objects<Person>().Any(x => x.Age > 0);
                Assert.True(any);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Any(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                bool any = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Any())()
                    : Objects<Person>().Any();
                Assert.True(any);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Where_Any_Predicate(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                bool any = mode == Mode.CompiledQuery
                    ? CompileQuery((Gender g, int age) => Objects<Person>().Where(x => x.Gender == g).Any(x => x.Age > age))(Gender.Male, 0)
                    : Objects<Person>().Where(x => x.Gender == Gender.Male).Any(x => x.Age > 0);
                Assert.True(any);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Where_Any(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                bool any = mode == Mode.CompiledQuery
                    ? CompileQuery((Gender g) => Objects<Person>().Where(x => x.Gender == g).Any())(Gender.Male)
                    : Objects<Person>().Where(x => x.Gender == Gender.Male).Any();
                Assert.True(any);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Select_Any_Predicate(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                bool any = mode == Mode.CompiledQuery
                    ? CompileQuery((string city) => Objects<Person>().Select(x => x.Office).Any(x => x.City == city))("Stockholm")
                    : Objects<Person>().Select(x => x.Office).Any(x => x.City == "Stockholm");
                Assert.True(any);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Select_Any(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                bool any = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Select(x => x.Office).Any())()
                    : Objects<Person>().Select(x => x.Office).Any();
                Assert.True(any);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void First(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().First())()
                    : Objects<Person>().First();
                Assert.NotNull(person);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void First_SequenceEmpty(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var person = mode == Mode.CompiledQuery
                        ? CompileQuery((int age) => Objects<Person>().First(x => x.Age == age))(100)
                        : Objects<Person>().First(x => x.Age == 100);
                });
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Where_First_SequenceEmpty(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var person = mode == Mode.CompiledQuery
                        ? CompileQuery((int age) => Objects<Person>().Where(x => x.Age == age).First())(100)
                        : Objects<Person>().Where(x => x.Age == 100).First();
                });
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Single(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((int age) => Objects<Person>().Single(x => x.Age > age))(40)
                    : Objects<Person>().Single(x => x.Age > 40);
                Assert.NotNull(person);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SingleOrDefault(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((int age) => Objects<Person>().SingleOrDefault(x => x.Age > age))(40)
                    : Objects<Person>().SingleOrDefault(x => x.Age > 40);
                Assert.NotNull(person);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Single_SequenceEmpty(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var person = mode == Mode.CompiledQuery
                        ? CompileQuery((int age) => Objects<Person>().Single(x => x.Age == age))(100)
                        : Objects<Person>().Single(x => x.Age == 100);
                });
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SingleOrDefault_SequenceEmpty(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((int age) => Objects<Person>().SingleOrDefault(x => x.Age == age))(100)
                    : Objects<Person>().SingleOrDefault(x => x.Age == 100);
                Assert.Null(person);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Single_TooMuchSequence(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var person = mode == Mode.CompiledQuery
                        ? CompileQuery((int age) => Objects<Person>().Single(x => x.Age > age))(0)
                        : Objects<Person>().Single(x => x.Age > 0);
                });
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SingleOrDefault_TooMuchSequence(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var person = mode == Mode.CompiledQuery
                        ? CompileQuery((int age) => Objects<Person>().Single(x => x.Age > age))(0)
                        : Objects<Person>().Single(x => x.Age > 0);
                });
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Where_Single_SequenceEmpty(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var person = mode == Mode.CompiledQuery
                        ? CompileQuery((int age) => Objects<Person>().Where(x => x.Age == age).Single())(100)
                        : Objects<Person>().Where(x => x.Age == 100).Single();
                });
            }).Wait();
        }
    }
}
