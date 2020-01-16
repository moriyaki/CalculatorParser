/*  CalculatorParser
 *
 *  算術パーサー習作(臭作?)
 *  C#で作ってもどうせ遅いので実用性は多分ほとんどない 
 *  気が向いたところまでやってみる
 *  
 *  BNF参考(というかほぼパクり) https://qiita.com/toru0408/items/1ff86eccbed4ee0f6f95
 *  でもBNFのまま書き起こしてないという…
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


namespace CalculatorParser
{
    // 数式内の要素タイプ
    public enum ExprType
    {
        UNKNOWN,
        NUBER,
        OPERATOR_PLUS,
        OPERATOR_MINUS,
        OPERATOR_MULITPLY,
        OPERATOR_DIVIDE,
        OPERATOR_PRIORIZED,
    }

    // TODO : BNR表記に合わせる

    // 数式内の要素
    // Visitorパターン使うの？
    public struct ExprPiece
    {
        public ExprType Type;
        public int Number;
        public List<ExprPiece> exprPieces;

        public override string ToString()
        {
            string to_str = "";

            if (Type == ExprType.NUBER)
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

    // 数式パーサ
    static class CalculatorParser
    {
        public static Dictionary<ExprType, string> operator_dict = new Dictionary<ExprType, string>() {
            [ExprType.OPERATOR_PLUS] = "+",
            [ExprType.OPERATOR_MINUS] = "-",
            [ExprType.OPERATOR_MULITPLY] = "*",
            [ExprType.OPERATOR_DIVIDE] = "/",
            [ExprType.OPERATOR_PRIORIZED] = "()"
        };

        public static ExprType GetDictKey(string es)
        {
            return operator_dict.First(x => x.Value == es.ToString()).Key;
        }

       
        private static List<ExprPiece> Parsing(string s, ref int i)
        {
            var eplist = new List<ExprPiece>();

            int num = 0;

            while(i < s.Length)
            {
                if (char.IsDigit(s[i]))
                {
                    do
                    {
                        num = num * 10 + s[i++] - '0';
                    }
                    while (i < s.Length && char.IsDigit(s[i]));

                    // 数値をリストに追加
                    var ep = new ExprPiece
                    {
                        Type = ExprType.NUBER,
                        Number = num
                    };
                    eplist.Add(ep);

                    // 数値初期化
                    num = 0;
                }
                else
                {
                    // TODO: プライオリティ数値を廃止し、ExprPiece内にList<ExprPirce>を持たせる
                    if (s[i] == '(')
                    {
                        i++;
                        var ep = new ExprPiece
                        {
                            Type = ExprType.OPERATOR_PRIORIZED,
                            exprPieces = Parsing(s, ref i),
                        };
                        eplist.Add(ep);
                    }
                    else if (s[i] == ')')
                    {
                        i++;
                        return eplist;
                    }
                    else
                    {
                        // 通常の四則演算
                        var ep = new ExprPiece
                        {
                            Type = GetDictKey(s[i++].ToString())
                        };
                        eplist.Add(ep);
                    }
                }
            }

            return eplist;
        }

        public static void Print(List<ExprPiece> eplist)
        {
            foreach (var ep in eplist)
            {
                if (ep.Type == ExprType.OPERATOR_PRIORIZED)
                {
                    Console.Write("\n( ");
                    Print(ep.exprPieces);
                    Console.Write(")\n");
                }
                else
                {
                    Console.Write(ep.ToString() + " ");
                }
            }
        }

        public static List<ExprPiece> Parse(string s)
        {
            int i = 0;
            var ep = Parsing(s, ref i);

            Print(ep);


            return ep;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CalculatorParser Test");

            string expr = "1+2*(3+4)+5*(6*(7+8)+(9+10))";
            //string expr = "231+(9832*6232/(1230-777))-20";
            Console.WriteLine($"{expr} =  ...");

            List<ExprPiece> result;
            result = CalculatorParser.Parse(expr);
        }
    }
}
