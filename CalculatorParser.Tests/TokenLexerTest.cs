using System;
using Xunit;
using CalculatorParser;

namespace CalculatorParser.Tests
{
    public class TokenLexerTest
    {
        /*
        [Fact]
        public void LParamNotFoundTest()
        {
            var formula = "1+2*3";
            var lexer = new Lexer(formula);
            Assert.Empty(lexer.GetOperatorPoint('('));
        }

        [Fact]
        public void LParamFindTest()
        {
            var formula = "2+(1+2)*3+(4/8)";
            var lparam = new int[] { 2, 10 };
            var index = 0;

            var lexer = new Lexer(formula);
            foreach (var i in lexer.GetOperatorPoint('('))
            {
                Assert.Equal(lparam[index++], i);
            }

        }
        */
        [Theory]
        [InlineData('+', TokenType.PLUS)]
        [InlineData('-', TokenType.MINUS)]
        [InlineData('*', TokenType.MULITPLY)]
        [InlineData('/', TokenType.DIVIDE)]
        [InlineData('(', TokenType.LPARAM)]
        [InlineData(')', TokenType.RPARAM)]
        public void GetOperatorTokenTest(char c, TokenType type)
        {
            Assert.Equal(Token.LookupOperators(c), type);
        }

        [Fact]
        public void GetTokenTest()
        {
            var formula = "2+(1+2)*3+(4/8)";

            var lexer = new Lexer(formula);
            Assert.True(lexer.GetToken());
        }
    }
}
