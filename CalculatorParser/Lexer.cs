using System.Collections.Generic;

namespace CalculatorParser
{
    /*
     * Lexer.cs
     * 字句解析クラス、文法についてはチェックしない
     */
    public class Lexer
    {
        public string LexInput { get; private set; }

        public Lexer(string _lex_input)
        {
            LexInput = _lex_input;
        }

        private static string GetNumber(string number_start)
        {
            var i = 0;
            while (i < number_start.Length && char.IsDigit(number_start[i]))
            {
                i++;
            }
            return number_start.Substring(0, i);
        }

        public List<Token> GetToken()
        {
            var token_list = new List<Token>(LexInput.Length * 2);

            for (var i = 0; i < LexInput.Length; i++)
            {
                // 空白(系)なら読み飛ばし
                if (char.IsWhiteSpace(LexInput[i]))
                {
                    continue;
                }
                // 括弧か演算子なら
                TokenType token_type = Token.LookupOperators(LexInput[i]);
                if (token_type != TokenType.UNKNOWN)
                {
                    token_list.Add(new Token(token_type, LexInput[i].ToString()));
                    continue;
                }
                // 数値なら続く数字をチェック
                if (char.IsDigit(LexInput[i]))
                {
                    var current_number = GetNumber(LexInput.Substring(i));
                    token_list.Add(new Token(TokenType.NUBER, current_number));
                    i += current_number.Length - 1;
                    continue;

                }
                // 英字なら予約語チェック,予約語でないなら変数と見做す(後日、必要なら)
            }
            token_list.Add(new Token(TokenType.EOF, ""));

            return token_list;
        }
    }
}
