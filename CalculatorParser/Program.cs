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
	〈加算式 expr〉::= 〈乗算式〉{〈加算演算子〉〈乗算式〉}
	〈乗算式 term〉::= 〈単項式〉{〈乗算演算子〉〈乗算式〉}
	〈単項式 factor〉::=〈開括弧〉〈式〉〈閉括弧〉|〈数〉

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
using System.Text;
using System.Collections.Generic;
using System.Linq;


namespace CalculatorParser
{
    // 数式内の要素タイプ
    public enum ExprType
    {
        NUBER,
        OPERATOR_PLUS,
        OPERATOR_MINUS,
        OPERATOR_MULITPLY,
        OPERATOR_DIVIDE,
        OPERATOR_PRIORIZED,
    }

    // TODO : BNR表記に合わせる

    // 数式ノード
    public class FormulaNode
    {
        public ExprType Type;
        public int Number;
        public List<FormulaNode> formula_list;

        public FormulaNode()
        {
            formula_list = null;
        }

        // 数式の文字列化
        public override string ToString()
        {
            if (formula_list == null && Type == ExprType.NUBER)
            {
                return Number.ToString();
            }
            else
            {
                StringBuilder expr = new StringBuilder();

                foreach (var fl in formula_list)
                {
                    expr.Append(" ");
                    if (fl.Type == ExprType.NUBER)
                    {
                        expr.Append(fl.Number);
                    }
                    else if (fl.Type == ExprType.OPERATOR_PRIORIZED)
                    {
                        expr.Append("(");
                        expr.Append(fl.ToString());
                        expr.Append(")");
                    }
                    else
                    {
                        expr.Append(CalculatorParser.operator_dict[fl.Type]);
                    }
                }

                return expr.ToString();
            }

        }

        // パーサ呼び出し
        public void Parse(string expr)
        {
            int i = 0;
            formula_list = CalculatorParser.Parse(expr, ref i);
        }
    }

    // 数式パーサ
    static class CalculatorParser
    {
        // 演算子種類→演算子文字列の辞書
        public static Dictionary<ExprType, string> operator_dict = new Dictionary<ExprType, string>() {
            [ExprType.OPERATOR_PLUS] = "+",
            [ExprType.OPERATOR_MINUS] = "-",
            [ExprType.OPERATOR_MULITPLY] = "*",
            [ExprType.OPERATOR_DIVIDE] = "/",
            [ExprType.OPERATOR_PRIORIZED] = "()"
        };

        // 演算子文字列→演算子種類の取得
        public static ExprType GetDictKey(string es)
        {
            return operator_dict.First(x => x.Value == es.ToString()).Key;
        }


        // 数の評価
        private static int Number(string s, ref int i)
        {
            int num = 0;

            do
            {
                num = num * 10 + s[i++] - '0';
            }
            while (i < s.Length && char.IsDigit(s[i]));
            return num;
        }

        public static List<FormulaNode> Parse(string s, ref int i)
        {
            var fl_list = new List<FormulaNode>();
            int num;

            while(i < s.Length)
            {
                if (char.IsDigit(s[i]))
                {
                    num = Number(s, ref i);

                    // 数値をリストに追加
                    fl_list.Add(new FormulaNode
                    {
                        Type = ExprType.NUBER,
                        Number = num,
                    });
                }
                else
                {
                    if (s[i] == '(')
                    {
                        i++;
                        fl_list.Add(new FormulaNode
                        {
                            Type = ExprType.OPERATOR_PRIORIZED,
                            formula_list = Parse(s, ref i),
                        });
                    }
                    else if (s[i] == ')')
                    {
                        i++;
                        return fl_list;
                    }
                    else
                    {
                        if (operator_dict.ContainsValue(s[i].ToString()))
                        {
                            // 演算子が見つかった
                            fl_list.Add(new FormulaNode {
                                Type = GetDictKey(s[i].ToString()),
                            });
                        }
                        i++;
                    }
                }
            }

            return fl_list;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CalculatorParser Test");

            //string expr = "1+2*(3+4)+5*(6*(7+8)+(9+10))";
            string expr = "231 +(9832*6232/(1230-777))-21001";
            Console.WriteLine($"{expr} =  114490.538 ...");

            var formula = new FormulaNode();
            formula.Parse(expr);
            Console.WriteLine(formula);
        }
    }
}
