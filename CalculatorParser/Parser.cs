using System;
using System.Collections.Generic;

namespace CalculatorParser
{
	public enum NodeType
	{
		FORMULA,            // 括弧演算
		MULTIPLICATION,     // 乗除算
		DOT,				// 小数点
		ADDITION,           // 加減算
		NUMBER,             // 数値
	}

	// 式ノードクラス
   abstract public class FormulaNode 
	{
		public NodeType Type { get; protected set; }
		public bool Calced { get; protected set; } = false;
		public List<FormulaNode> Node { get; protected set; }
	}

	// 括弧演算クラス
	public class PriorisedFormulaNode : FormulaNode
	{

		public PriorisedFormulaNode(List<FormulaNode> _node)
		{
			Node = _node;
			Type = NodeType.FORMULA;
		}

		public override string ToString()
		{
			return "(";
		}
	}

	// 乗除算ノードクラス
	public class MultiplicatoinNode : FormulaNode
	{
		public Token Operator { get; private set; }

		public MultiplicatoinNode(Token _operator) 
		{
			Node = null;
			Operator = _operator;
			Type = NodeType.MULTIPLICATION;
		}

		public override string ToString()
		{
			return Operator.Literal;
		}
	}

	// 加減算クラス
	public class AdditionNode : FormulaNode 
	{
		public Token Operator { get; private set; }

		public AdditionNode(Token _operator)
		{
			Node = null;
			Operator = _operator;
			Type = NodeType.ADDITION;
		}

		public override string ToString()
		{
			return Operator.Literal;
		}


	}

	// 小数点
	public class DotNode : FormulaNode
	{
		public Token Operator { get; private set; }

		public DotNode(Token _operator)
		{
			Node = null;
			Operator = _operator;
			Type = NodeType.DOT;
		}

		public override string ToString()
		{
			return Operator.Literal;
		}
	}

	// 数値クラス
	public class NumberNode : FormulaNode
	{
		public int Number { get; private set; }
		public int DecimalNumber { get; private set; }

		public NumberNode(Token _number, Token _decimal = null) 
		{
			Node = null;
			Number = int.Parse(_number.Literal);

			if (_decimal == null)
			{
				DecimalNumber = 0;
			}
			else
			{
				DecimalNumber = int.Parse(_decimal.Literal);
			}
			Type = NodeType.NUMBER;
		}

		public override string ToString()
		{
			return Number.ToString();
		}

		public static NumberNode operator+(NumberNode a, NumberNode b)
		{
			if (a.DecimalNumber == 0 && b.DecimalNumber == 0)
			{
				return new NumberNode(
					new Token(
						TokenType.NUBER,
						(a.Number + b.Number).ToString()));
			}
			else
			{
				// TODO:小数点の実装
				return null;
			}
		}

		public static NumberNode operator-(NumberNode a, NumberNode b)
		{
			if (a.DecimalNumber == 0 && b.DecimalNumber == 0)
			{
				return new NumberNode(
					new Token(
						TokenType.NUBER,
						(a.Number - b.Number).ToString()));
			}
			else
			{
				// TODO:小数点の実装
				return null;
			}
		}

		public static NumberNode operator*(NumberNode a, NumberNode b)
		{
			if (a.DecimalNumber == 0 && b.DecimalNumber == 0)
			{
				return new NumberNode(
					new Token(
						TokenType.NUBER,
						(a.Number * b.Number).ToString()));
			}
			else
			{
				// TODO:小数点の実装
				return null;
			}
		}

		public static NumberNode operator/(NumberNode a, NumberNode b)
		{
			if (a.DecimalNumber == 0 && b.DecimalNumber == 0)
			{
				return new NumberNode(
					new Token(
						TokenType.NUBER,
						(a.Number / b.Number).ToString()));
			}
			else
			{
				// TODO:小数点の実装
				return null;
			}
		}

	}

	public class Parser
	{
		private int index = 0;

		/// <summary>
		/// 構文解析して構文木を生成する
		/// 前提条件：ValidityCheckerを通してエラーが無いこと
		/// </summary>
		/// <param name="token">解析するToken</param>
		/// <returns></returns>
		public List<FormulaNode> Parsing(List<Token> token) 
		{
			var validity_checker = new ValidityChecker();
			if (!validity_checker.ValidityCheck(token))
			{
				throw new Exception("Validity Error");
			}

			var formula_node = new List<FormulaNode>(token.Count * 2);

			// tokenListすべてを検索
			for (var p = index; p < token.Count; p++)
			{
				switch (token[p].Type)
				{
					// 左括弧なら
					case TokenType.LPARAM:
						index = p + 1;	// '(' の次場所を保存、再帰で利用
						formula_node.Add(new PriorisedFormulaNode(Parsing(token)));
						p = index;	// ')' の後に移動
						break;
					// 右括弧なら
					case TokenType.RPARAM:
						index = p;	// 現在位置を保存
						return formula_node;
					// 乗除演算子なら
					case TokenType.MULITPLY:
					case TokenType.DIVIDE:
						formula_node.Add(new MultiplicatoinNode(token[p]));
						break;
					// 加減演算子なら
					case TokenType.PLUS:
					case TokenType.MINUS:
						formula_node.Add(new AdditionNode(token[p]));
						break;
					// 数値なら
					case TokenType.NUBER:
						formula_node.Add(new NumberNode(token[p]));
						break;
					case TokenType.EOF:
						return formula_node;
					// ILLEGAL,UNKNOWNは不正
					default:
						return null;
				}
			}
			// TokenType.EOF で終わってなかったら不正
			return null;
		}
	}
}
