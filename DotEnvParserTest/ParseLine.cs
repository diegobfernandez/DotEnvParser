using DotEnvParser;
using Xunit;

namespace DotEnvParserTest
{
    public class ParseLine
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void MustFailWhenLineIsEmpty(string line)
        {
            var result = Parser.ParseLine(line);
            Assert.False(result.IsSuccess);
            Assert.Equal(Parser.EMPTY_LINE, result.Failure);
        }

        [Theory]
        [InlineData("RANDON TEXT")]
        public void MustFailIfHasNotSeparator(string line)
        {
            var result = Parser.ParseLine(line);
            Assert.False(result.IsSuccess);
            Assert.Equal(Parser.E_COULD_NOT_PARSE_LINE, result.Failure);
        }

        [Theory]
        [InlineData(@"1=")]
        [InlineData(@"12=")]
        [InlineData(@"1K=")]
        public void MustFailIfKeyStartsWithNumeric(string line)
        {
            var result = Parser.ParseLine(line);
            Assert.False(result.IsSuccess);
            Assert.Equal(Parser.E_COULD_NOT_PARSE_LINE, result.Failure);
        }

        [Theory]
        [InlineData(@"#ENV=123")]
        public void MustFailWhenIsComment(string line)
        {
            var result = Parser.ParseLine(line);
            Assert.False(result.IsSuccess);
            Assert.Equal(Parser.COMMENT_LINE, result.Failure);
        }

        [Theory]
        [InlineData(@"K=", "K", "")]
        [InlineData(@"KEY=", "KEY", "")]
        [InlineData(@"K_E_Y=", "K_E_Y", "")]
        [InlineData(@"K__=", "K__", "")]
        [InlineData(@"K1=", "K1", "")]
        [InlineData(@"K111=", "K111", "")]
        [InlineData(@"K=VALUE", "K", "VALUE")]
        [InlineData(@"K=My Value", "K", "My Value")]
        [InlineData(@"K=""Complex"" Value. Ok?=", "K", @"""Complex"" Value. Ok?=")]
        public void MustPass(string line, string key, string value)
        {
            var result = Parser.ParseLine(line);
            Assert.True(result.IsSuccess);
            Assert.Equal(key, result.Success.Name);
            Assert.Equal(value, result.Success.Value);
        }
    }
}
