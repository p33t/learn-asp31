using System;
using api.binder;
using Xunit;
using Xunit.Sdk;

namespace test
{
    public class ParseUtcDateTimeTests
    {
        private static readonly DateTime SomeUtcDateTime = new DateTime(2022, 1, 2, 13, 4, 5, DateTimeKind.Utc);
        private const string AbbreviatedIsoFormat = "yyyy'-'MM'-'dd'T'HH':'mmK";
        private const string SansTimezoneFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
        
        public static TheoryData<string, DateTime?> ParseDateTimeFixtures() => new TheoryData<string, DateTime?>
        {
            {SomeUtcDateTime.ToString("O"), SomeUtcDateTime},
            {SomeUtcDateTime.ToLocalTime().ToString("O"), SomeUtcDateTime},
            {SomeUtcDateTime.ToString(AbbreviatedIsoFormat), SomeUtcDateTime.AddSeconds(-5)},
            {SomeUtcDateTime.ToLocalTime().ToString(AbbreviatedIsoFormat), SomeUtcDateTime.AddSeconds(-5)},
            {SomeUtcDateTime.ToString(SansTimezoneFormat + "'+10:00'"), SomeUtcDateTime.AddHours(-10)},
            {SomeUtcDateTime.ToString(SansTimezoneFormat), null},
            {" ", null},
            {" ", null},
            {"", null},
            {null, null}
        };

        [Theory]
        [MemberData(nameof(ParseDateTimeFixtures))]
        public void RunFixtures(string dateTimeString, DateTime? expected)
        {
            var actual = DateTimeModelBinder.ParseUtcDateTime(dateTimeString);
            Assert.Equal(expected, actual);
        }
    }
}