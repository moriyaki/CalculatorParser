/*  CalculatorParser
 *
 *  算術パーサー習作(臭作?)
 *  C#で作ってもどうせ遅いので実用性は多分ほとんどない 
 *  気が向いたところまでやってみる
 *  
 *  コード参考はこの辺
 *  http://www.ss.cs.meiji.ac.jp/CCP035.html ← 括弧無し四則演算
 *  https://qiita.com/r-ngtm/items/490c5ce6d7f43f9677f1 ←考え方の参考になる
 */
using System;


namespace CalculatorParser
{
	static class Program
	{
		static void Main()//string[] args)
		{
			Console.WriteLine("CalculatorParser");

			string formula = NewMethod();
			var lexer = new Lexer(formula);
			var token = lexer.GetToken();
			var parser = new Parser();
			var syntax_tree = parser.Parsing(token);

			var result = Calculator.Caluculate(syntax_tree);
			Console.WriteLine($"{NewMethod()} = {result}");

		}

		private static string NewMethod()
		{
			return "1+1+2";
		}
	}

	/*
	// 数式ノード
	public class FormulaNode
	{
		public TokenType Type;
		public int Number;
		public List<FormulaNode> formula_list;

		public FormulaNode()
		{
			Type = TokenType.EXPRESSION;
			formula_list = null;
		}

		// 数式の文字列化
		public override string ToString()
		{
			if (formula_list == null && Type == TokenType.NUBER)
			{
				return Number.ToString();
			}
			else
			{
				var expr = new StringBuilder();

				foreach (var fl in formula_list)
				{
					expr.Append(" ");
					if (fl.Type == TokenType.NUBER)
					{
						expr.Append(fl.Number);
					}
					else if (fl.Type == TokenType.EXPRESSION)
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
		public static Dictionary<TokenType, string> operator_dict = new Dictionary<TokenType, string>() {
			[TokenType.OPERATOR_PLUS] = "+",
			[TokenType.OPERATOR_MINUS] = "-",
			[TokenType.OPERATOR_MULITPLY] = "*",
			[TokenType.OPERATOR_DIVIDE] = "/",
		};

		// 演算子文字列→演算子種類の取得
		public static TokenType GetDictKey(string es)
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
						Type = TokenType.NUBER,
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
							Type = TokenType.EXPRESSION,
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
	*/
}
