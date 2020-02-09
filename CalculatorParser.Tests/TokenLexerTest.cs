using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CalculatorParser;

namespace CalculatorParser.Tests
{
    public class TokenLexerTest
    {
        [Fact(DisplayName = "演算子Tokenを正しく取れているか")]
        public void GetTokenOperatorTest()
        {
            var formula = "+-*/()";
            var tokentype_array = new TokenType[]
            {
                TokenType.PLUS,
                TokenType.MINUS,
                TokenType.MULITPLY,
                TokenType.DIVIDE,
                TokenType.LPARAM,
                TokenType.RPARAM,
                TokenType.EOF,
            };
               

            var lexer = new Lexer(formula);
            var token = lexer.GetToken();
            var i=0;
            foreach (var t in token)
            {
                Assert.Equal(t.Type, tokentype_array[i++]);
            }

        }

        [Fact(DisplayName = "数値Tokenを正しく取れているか")]
        public void GetTokenNumbertest()
        {
            var formula = "0123456789";
            var tokentype_array = new TokenType[]
            {
                TokenType.NUBER,
                TokenType.EOF,
            };

            var lexer = new Lexer(formula);
            var token = lexer.GetToken();
            var i=0;
            foreach (var t in token)
            {
                Assert.Equal(t.Type, tokentype_array[i++]);
                if (t.Type == TokenType.NUBER)
                {
                    Assert.Equal(formula, t.Literal);
                }
            }

        }

        [Fact(DisplayName = "演算子と数値Token混合で上手く取れるか1")]
        public void GetTokenTest1()
        {
            var formula = "2+3";

            var tokentype_array = new TokenType[]
            {
                // 2+3
                TokenType.NUBER,
                TokenType.PLUS,
                TokenType.NUBER,
                TokenType.EOF
            };

            var lexer = new Lexer(formula);
            var token = lexer.GetToken();
            var i=0;
            foreach (var t in token)
            {
                Assert.Equal(t.Type, tokentype_array[i++]);
            }
        }

        [Fact(DisplayName = "演算子と数値Token混合で上手く取れるか2")]
        public void GetTokenTest2()
        {
            var formula = "2+((1+2)*3+(4/8))";

            var tokentype_array = new TokenType[]
            {
                // 2+
                TokenType.NUBER,
                TokenType.PLUS,
                // (
                TokenType.LPARAM,
                // (1+2)
                TokenType.LPARAM,
                TokenType.NUBER,
                TokenType.PLUS,
                TokenType.NUBER,
                TokenType.RPARAM,
                // *3+
                TokenType.MULITPLY,
                TokenType.NUBER,
                TokenType.PLUS,
                // (4*8)
                TokenType.LPARAM,
                TokenType.NUBER,
                TokenType.DIVIDE,
                TokenType.NUBER,
                TokenType.RPARAM,
                // )
                TokenType.RPARAM,
                TokenType.EOF,
            };

            var lexer = new Lexer(formula);
            var token = lexer.GetToken();
            var i=0;
            foreach (var t in token)
            {
                Assert.Equal(t.Type, tokentype_array[i++]);
            }
        }

        [Fact(DisplayName = "演算子と数値Token混合で上手く取れるか3")]
        public void GetTokenTest3()
        {
            var formula = "22314+((1066+2256)*3948+(42658/8587))";

            var tokentype_array = new TokenType[]
            {
                // 22314+
                TokenType.NUBER,
                TokenType.PLUS,
                // (
                TokenType.LPARAM,
                // (1066+2256)
                TokenType.LPARAM,
                TokenType.NUBER,
                TokenType.PLUS,
                TokenType.NUBER,
                TokenType.RPARAM,
                // *3948+
                TokenType.MULITPLY,
                TokenType.NUBER,
                TokenType.PLUS,
                // (42658*8587)
                TokenType.LPARAM,
                TokenType.NUBER,
                TokenType.DIVIDE,
                TokenType.NUBER,
                TokenType.RPARAM,
                // )
                TokenType.RPARAM,
                TokenType.EOF,
            };

            var lexer = new Lexer(formula);
            var token = lexer.GetToken();
            var i=0;
            foreach (var t in token)
            {
                Assert.Equal(t.Type, tokentype_array[i++]);
            }
        }

    }
}
