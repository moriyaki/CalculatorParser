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


        /// <summary>
        /// 記号のトークン取れてるかテスト
        /// </summary>
        [Fact(DisplayName = "+-*/().")]
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

        /// <summary>
        /// 数値のトークン取れてるかテスト
        /// </summary>
        [Fact(DisplayName = "0123456789")]
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

        /// <summary>
        /// 簡単な数式ののトークン取れてるかテスト
        /// </summary>
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

        /// <summary>
        /// 比較的複雑な数式のトークン取れてるかテスト
        /// </summary>
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

        /// <summary>
        /// 小数点込み数式のトークン取れてるかテスト
        /// </summary>
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
}
