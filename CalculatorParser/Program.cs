/*  CalculatorParser
 *
 *  算術パーサー習作(臭作?)
 *  C#で作ってもどうせ遅いので実用性は多分ほとんどない 
 *  気が向いたところまでやってみる
 *  
 *  BNF参考(というかほぼパクり)は https://qiita.com/toru0408/items/1ff86eccbed4ee0f6f95
 *  
 *  {α} :αの0回以上の繰り返し
 *  [a] :αまたは空
 *  (a) :αのグループ化
 
	〈式〉::= 〈加算式〉
	〈加算式〉::= 〈乗算式〉{〈加算演算子〉〈乗算式〉}
	〈乗算式〉::= 〈単項式〉{〈乗算演算子〉〈乗算式〉}
	〈単項式〉::=〈開括弧〉〈式〉〈閉括弧〉|〈数〉

	〈数〉::=〈整数〉
	〈加算演算子〉::= “+” | “-”
	〈乗算演算子〉::= “*” | “/” | “%”
	〈開括弧〉 ::= “(“
	〈閉括弧〉::= “)”
	〈正の数〉::= “1” | “2” | “3” | “4” | “5” | “6” | “7” | “8” | “9”
	〈数字〉 ::= “0” | 〈正の数〉
	〈符号無し正の整数〉::= 〈正の数〉{〈数字〉}
	〈整数〉::= “0” | [“+”|”-”]〈符号無し整数〉

    蛇足
	〈数〉::= 〈整数〉|〈小数〉|〈有理数〉
	〈小数〉 ::= 〈整数〉[”.”〈符号無し整数〉]
	〈分数〉::=〈整数〉”/”〈符号無し正の整数〉
   
 *  
 *  コード参考はこの辺
 *  http://www.ss.cs.meiji.ac.jp/CCP035.html ← 括弧無し四則演算
 *  https://qiita.com/r-ngtm/items/490c5ce6d7f43f9677f1 ←考え方の参考になる
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace CalculatorParser
{
    public enum PieceType
    {
        UNKNOWN,
        NUBER,
        OPERATOR_PLUS,
        OPERATOR_MINUS,
        OPERATOR_MULITPLY,
        OPERATOR_DIVIDE,
        OPERATOR_PRIORIZE_START,
        OPERATOR_PRIORIZE_END
    }

    public struct ExprPiece
    {
        public PieceType Type;
        public int Number;
        public int Priority;

        public override string ToString()
        {
            string to_str = "";

            for (int i = 0; i < Priority; i++) to_str += "\t";

            if (Type == PieceType.NUBER)
            {
                to_str += Number.ToString();
            }
            else
            {
                to_str += CalculatorParser.operator_dict[Type];
            }
            return to_str;
        }
    }

    static class CalculatorParser
    {
        public static Dictionary<PieceType, string> operator_dict = new Dictionary<PieceType, string>() {
            { PieceType.OPERATOR_PLUS, "+" },
            { PieceType.OPERATOR_MINUS, "-" },
            { PieceType.OPERATOR_MULITPLY, "*" },
            { PieceType.OPERATOR_DIVIDE, "/" },
            { PieceType.OPERATOR_PRIORIZE_START, "(" },
            { PieceType.OPERATOR_PRIORIZE_END, ")" }
        };

       
        public static (bool, List<ExprPiece>) Parse(string s)
        {
            var eplist = new List<ExprPiece>();

            int num = 0;
            int priority = 0;

            foreach (char c in s.ToCharArray())
            {
                if (char.IsDigit(c))
                {
                    num = num * 10 + c - '0';
                }
                else
                {
                    var ep = new ExprPiece
                    {
                        Type = PieceType.NUBER,
                        Number = num,
                        Priority = priority
                    };
                    eplist.Add(ep);
                    num = 0;

                    if (c == ')') priority--;
                    ep = new ExprPiece
                    {
                        Type = operator_dict.First(x => x.Value == c.ToString()).Key,
                        Priority = priority
                    };
                    if (c == '(') priority++;
                    eplist.Add(ep);
                }
            }



            return (true, eplist);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CalculatorParser Test");

            string expr = "1+2*(3+4)+5*(6*(7+8)+(9+10))";
            //string expr = "231+9832*6232/(1230-777)";
            Console.WriteLine($"{expr} =  ...");

            bool result;
            List<ExprPiece> error;
            (result, error) = CalculatorParser.Parse(expr);
        }
    }
}
