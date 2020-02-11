using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace CalculatorParser.Tests
{
    public class TokenLexerTest
    {
        private void TokenCheck(string formula, TokenType[] token_type_array, string number_check = "")
        {
            var lexer = new Lexer(formula);
            var token = lexer.GetToken();
            var i = 0;
            foreach (var t in token)
            {
                Assert.Equal(t.Type, token_type_array[i++]);
                if (t.Type == TokenType.NUBER && number_check != "")
                {
                    Assert.Equal(number_check, t.Literal);
                }
            }
        }


        [Fact(DisplayName = "演算子")]
        public void GetTokenOperatorTest()
        {
            var formula = "+-*/().";
            var token_type_array = new TokenType[]
            {
                TokenType.PLUS,
                TokenType.MINUS,
                TokenType.MULITPLY,
                TokenType.DIVIDE,
                TokenType.LPARAM,
                TokenType.RPARAM,
                TokenType.DOT,
                TokenType.EOF,
            };

            TokenCheck(formula, token_type_array);
        }

        [Fact(DisplayName = "数値")]
        public void GetTokenNumbertest()
        {
            var formula = "0123456789";
            var token_type_array = new TokenType[]
            {
                TokenType.NUBER,
                TokenType.EOF,
            };

            TokenCheck(formula, token_type_array, formula);
        }

        [Fact(DisplayName = "2+3")]
        public void GetTokenTest1()
        {
            var formula = "2+3";

            var token_type_array = new TokenType[]
            {
                // 2+3
                TokenType.NUBER,
                TokenType.PLUS,
                TokenType.NUBER,
                TokenType.EOF
            };

            TokenCheck(formula, token_type_array);
        }

        [Fact(DisplayName = "2+((1+2)*3+(4/8))")]
        public void GetTokenTest2()
        {
            var formula = "2+((1+2)*3+(4/8))";

            var token_type_array = new TokenType[]
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

            TokenCheck(formula, token_type_array);
        }

        [Fact(DisplayName = "22314+((1066+2256)*3.948+(42658/8587))")]
        public void GetTokenTest3()
        {
            var formula = "22314+((1066+2256)*3.948+(42658/8587))";

            var token_type_array = new TokenType[]
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
                // *3.948+
                TokenType.MULITPLY,
                TokenType.NUBER,
                TokenType.DOT,
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

            TokenCheck(formula, token_type_array);
        }

        [Fact(DisplayName="231 +(9832*6232/ (1230-777))-21001")]
        public void GetTokenTest4()
        {
            var formula = "231 +(9832*6232/ (1230-777))-21001";

            var token_type_array = new TokenType[]
            {
                // 231 +
                TokenType.NUBER,
                TokenType.PLUS,
                // (
                TokenType.LPARAM,
                // 9832*6232/
                TokenType.NUBER,
                TokenType.MULITPLY,
                TokenType.NUBER,
                TokenType.DIVIDE,
                //  (1230-777)
                TokenType.LPARAM,
                TokenType.NUBER,
                TokenType.MINUS,
                TokenType.NUBER,
                TokenType.RPARAM,
                // )-21001
                TokenType.RPARAM,
                TokenType.MINUS,
                TokenType.NUBER,
                TokenType.EOF,
            };

            TokenCheck(formula, token_type_array);
        }
    }

    public class ValidityCheckerTest
    {
        private IEnumerable<Token> LeftParam(int number)
        {
            return (from i in Enumerable.Range(0, number)
                    select new Token(TokenType.LPARAM, "("));
        }

        private IEnumerable<Token> RightParam(int number)
        {
            return (from i in Enumerable.Range(0, number)
                    select new Token(TokenType.RPARAM, ")"));
        }


        [Fact(DisplayName = "妥当性：()")]
        public void ValidityParamTest()
        {
            var token_list = new List<Token>();
            token_list.AddRange(LeftParam(2));
            token_list.AddRange(RightParam(1));
            token_list.AddRange(LeftParam(1));
            token_list.AddRange(RightParam(2));

            Assert.True(ValidityChecker.ParamCheck(token_list));
        }

        [Fact(DisplayName = "妥当性：()))")]
        public void RightParamOverTest()
        {
            var token_list = new List<Token>();
            token_list.AddRange(LeftParam(1));
            token_list.AddRange(RightParam(3));

            Assert.False(ValidityChecker.ParamCheck(token_list));
        }

        [Fact(DisplayName = "妥当性：((()")]
        public void LeftParamOverTest()
        {
            var token_list = new List<Token>();
            token_list.AddRange(LeftParam(3));
            token_list.AddRange(RightParam(1));

            Assert.False(ValidityChecker.ParamCheck(token_list));
        }

        [Fact(DisplayName = "妥当性：++")]
        public void PlusPlusTest()
        {
            var token_list = new List<Token>{
                new Token(TokenType.PLUS, "+"),
                new Token(TokenType.PLUS, "+"),
            };
            Assert.False(ValidityChecker.OperatorCheck(token_list));
        }

        [Fact(DisplayName = "妥当性：/*")]
        public void DuvMulTest()
        {
            var token_list = new List<Token>{
                new Token(TokenType.DIVIDE, "/"),
                new Token(TokenType.MULITPLY, "*"),
            };
            Assert.False(ValidityChecker.OperatorCheck(token_list));
        }

        [Fact(DisplayName = "妥当性：*-20")]
        public void MulMunusTwentyTest()
        {
            var token_list = new List<Token>{
                new Token(TokenType.MULITPLY, "*"),
                new Token(TokenType.MINUS, "-"),
                new Token(TokenType.NUBER, "20"),
            };
            Assert.True(ValidityChecker.OperatorCheck(token_list));
        }

        [Fact(DisplayName = "妥当性：20*20--20")]
        public void MinusCalcTwentyTest()
        {
            var token_list = new List<Token>{
                new Token(TokenType.NUBER, "20"),
                new Token(TokenType.MULITPLY, "*"),
                new Token(TokenType.NUBER, "20"),
                new Token(TokenType.MINUS, "-"),
                new Token(TokenType.MINUS, "-"),
                new Token(TokenType.NUBER, "20"),
            };
            Assert.True(ValidityChecker.OperatorCheck(token_list));
        }


        [Fact(DisplayName = "妥当性：20*20/20")]
        public void MulDivTwentyTest()
        {
            var token_list = new List<Token>{
                new Token(TokenType.NUBER, "20"),
                new Token(TokenType.MULITPLY, "*"),
                new Token(TokenType.NUBER, "20"),
                new Token(TokenType.DIVIDE, "/"),
                new Token(TokenType.NUBER, "20"),
            };
            Assert.True(ValidityChecker.OperatorCheck(token_list));
        }

        [Fact(DisplayName = "妥当性：20.34")]
        public void DotTrueCheck()
        {
            var token_list = new List<Token>{
                new Token(TokenType.NUBER, "20"),
                new Token(TokenType.DOT, "."),
                new Token(TokenType.NUBER, "34"),
            };

            Assert.True(ValidityChecker.DotCheck(token_list));
        }

        [Fact(DisplayName = "妥当性：20.*34")]
        public void DotFalseCheck()
        {
            var token_list = new List<Token>{
                new Token(TokenType.NUBER, "20"),
                new Token(TokenType.DOT, "."),
                new Token(TokenType.MULITPLY, "*"),
                new Token(TokenType.NUBER, "34"),
            };

            Assert.False(ValidityChecker.DotCheck(token_list));
        }
    }
}
