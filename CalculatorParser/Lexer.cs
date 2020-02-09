using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            LexInput =  _lex_input;
        }

        /// <summary>
        /// 引数 c の場所を列挙する
        /// </summary>
        /// <param name="c">列挙する文字</param>
        /// <returns>見つかった場所</returns>
        public IEnumerable<int> GetOperatorPoint(char c)
        {
            // '(' と ')' が含まれていなければ終了
            if (!LexInput.Contains(c))
            {
                yield break;
            }

            var pos = LexInput.IndexOf(c);
            while (pos != -1)
            {
                yield return pos;
                pos = LexInput.IndexOf(c, ++pos);
            }
        }

        public bool GetToken()
        {
            var token_list = new List<TokenType>();

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
                    token_list.Add(token_type);
                    continue;
                }
                // 数値なら続く数字をチェック
                if (char.IsDigit(LexInput[i]))
                {

                }


                // 英字なら予約語チェック
                // 予約語でないなら変数と見做す
            }

            return false;
        }
    }
}
