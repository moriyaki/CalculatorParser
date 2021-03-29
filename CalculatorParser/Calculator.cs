using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorParser
{
	/// <summary>
	/// 計算器クラス
	/// </summary>
	public class Calculator
	{
		private static List<FormulaNode> Multiplication(List<FormulaNode> syntax_tree)
		{
			// 加減算
			for (var i = 0; i < syntax_tree.Count; i++)
			{
				if (syntax_tree[i].Type == NodeType.MULTIPLICATION)
				{
					var right = syntax_tree[i - 1] as NumberNode;
					var left = syntax_tree[i + 1] as NumberNode;
					var oper = syntax_tree[i] as MultiplicatoinNode;

					if (oper.ToString() == "*")
					{
						syntax_tree[i] = right * left;
					}
					else
					{
						syntax_tree[i] = right / left;
					}

					syntax_tree.RemoveAt(i + 1);
					syntax_tree.RemoveAt(i - 1);
					i--;
				}
			}
			return syntax_tree;
		}

		private static List<FormulaNode> Additional(List<FormulaNode> syntax_tree)
		{
			// 加減算
			for (var i = 0; i < syntax_tree.Count; i++)
			{
				if (syntax_tree[i].Type == NodeType.ADDITION)
				{
					var right = syntax_tree[i - 1] as NumberNode;
					var left = syntax_tree[i + 1] as NumberNode;
					var oper = syntax_tree[i] as AdditionNode;

					if (oper.ToString() == "+")
					{
						syntax_tree[i] = right + left;
					}
					else
					{
						syntax_tree[i] = right - left;
					}

					syntax_tree.RemoveAt(i + 1);
					syntax_tree.RemoveAt(i - 1);
					i--;
				}
			}
			return syntax_tree;
		}

		public static string Caluculate(List<FormulaNode> syntax_tree)
		{ 
			syntax_tree = Multiplication(syntax_tree);
			syntax_tree = Additional(syntax_tree);
			return syntax_tree[0].ToString();
		}
	}
}
