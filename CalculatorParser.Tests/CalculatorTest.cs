using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace CalculatorParser.Tests
{
	/// <summary>
	/// 字句解析Lexerが正しく動いているか検証
	/// </summary>
	public class TokenLexerTests
	{
		/// <summary>
		/// Tokenへの解析テストで利用する共通メソッド
		/// 数式をLexer(字句解析)経由でTokenリストを取得し、期待したTokenリストと等しいことを検証
		/// </summary>
		/// <param name="formula">数式テキスト</param>
		/// <param name="token_type_array">期待するTokenリスト</param>
		private static void TokenCheck(string formula, TokenType[] token_type_array)
		{
			var lexer = new Lexer(formula);
			var token = lexer.GetToken();
			var i = 0;
			foreach (var t in token)
			{
				Assert.Equal(t.Type, token_type_array[i++]);
			}
		}

		/// <summary>
		/// 演算子のTokenが正しく取得されているか検証
		/// </summary>
		[Fact(DisplayName = "演算子")]
		public void GetTokenOperatorTest()
		{
			const string formula = "+-*/().";
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
		/// 数字のTokenが正しく取得されているか検証
		/// </summary>
		[Fact(DisplayName = "数値")]
		public void GetTokenNumbertest()
		{
			const string formula = "0123456789";
			var token_type_array = new TokenType[]
			{
				TokenType.NUBER,
				TokenType.EOF,
			};

			TokenCheck(formula, token_type_array);
		}

		/// <summary>
		/// 単純な加算 2+3 のTokenが正しく取得されているか検証
		/// </summary>
		[Fact(DisplayName = "2+3")]
		public void GetTokenTest1()
		{
			const string formula = "2+3";

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
		/// 少し複雑な数式でも、Tokenが正しく取得されているか検証
		/// </summary>
		[Fact(DisplayName = "2+((1+2)*3+(4/8))")]
		public void GetTokenTest2()
		{
			const string formula = "2+((1+2)*3+(4/8))";

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
		/// 小数点込みの数式でも、小数点をTokenType.DOTとして、Tokenが正しく取得されているか検証
		/// </summary>
		[Fact(DisplayName = "22314+((1066+2256)*3.948+(42658/8587))")]
		public void GetTokenTest3()
		{
			const string formula = "22314+((1066+2256)*3.948+(42658/8587))";

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
				// *3.948+ ここが肝
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

		/// <summary>
		/// 空白混じりの数式でも、Tokenが正しく取得されているか検証
		/// </summary>
		[Fact(DisplayName="231 +(9832*6232/ (1230-777))-21001")]
		public void GetTokenTest4()
		{
			const string  formula = "231 +(9832*6232/ (1230-777))-21001";

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

	/// <summary>
	/// 取得したTokenリストの構文妥当性を検証する
	/// </summary>
	public class ValidityCheckerTests
	{
		/// <summary>
		/// 記号、数値からTokenを返す共通メソッド
		/// </summary>
		/// <param name="oper">Tokenを取得したい値</param>
		/// <returns>Token</returns>
		private static Token MakeToken(string oper)
		{
			return oper switch
			{
				"+" => new Token(TokenType.PLUS, oper),
				"-" => new Token(TokenType.MINUS, oper),
				"*" => new Token(TokenType.MULITPLY, oper),
				"/" => new Token(TokenType.DIVIDE, oper),
				"(" => new Token(TokenType.LPARAM, oper),
				")" => new Token(TokenType.RPARAM, oper),
				"." => new Token(TokenType.DOT, oper),
				"" => new Token(TokenType.EOF, oper),
				_ => new Token(TokenType.NUBER, oper),
			};
		}

		/// <summary>
		/// 括弧が正しく閉じているか検証
		/// </summary>
		[Fact(DisplayName = "妥当性：()")]
		public void ValidityParamTest()
		{
			var token_list = new List<Token>(10)
			{
				MakeToken("("),
				MakeToken(")")
			};

			var validity_checker = new ValidityChecker();
			// 括弧は適切に閉じているので True 期待
			Assert.True(validity_checker.ParamCheck(token_list));
			// エラーが起きているフラグが立ってないことを検証
			Assert.False(validity_checker.ErrorOccurred);
		}

		/// <summary>
		/// 括弧が過剰に閉じている時の検証
		/// </summary>
		[Fact(DisplayName = "妥当性：()))")]
		public void RightParamOverTest()
		{
			var token_list = new List<Token>(10)
			{
				MakeToken("("),
				MakeToken(")"),
				MakeToken(")"),
				MakeToken(")"),
			};

			var validity_checker = new ValidityChecker();
			// 括弧が適切に閉じてないので、False期待
			Assert.False(validity_checker.ParamCheck(token_list));
			// エラーが起きているフラグが立っているか検証
			Assert.True(validity_checker.ErrorOccurred);
			// 発生したエラーが適切か検証。ここでは '(' が足りない LPARAM_SHOTAGE、が含まれることを期待
			Assert.Contains(ValidityError.LPARAM_SHORTAGE, validity_checker.ErrorList);
		}

		/// <summary>
		/// 括弧が適切に閉じてない時の検証
		/// </summary>
		[Fact(DisplayName = "妥当性：((()")]
		public void LeftParamOverTest()
		{
			var token_list = new List<Token>(10)
			{
				MakeToken("("),
				MakeToken("("),
				MakeToken("("),
				MakeToken(")"),
			};

			var validity_checker = new ValidityChecker();
			// 括弧が適切に閉じてないので、False期待
			Assert.False(validity_checker.ParamCheck(token_list));
			// エラーが起きているフラグが立っているか検証
			Assert.True(validity_checker.ErrorOccurred);
			// 発生したエラーが適切か検証。ここでは ')' が足りない RPARAM_SHOTAGE、が含まれることを期待
			Assert.Contains(ValidityError.RPARAM_SHORTAGE, validity_checker.ErrorList);
		}

		/// <summary>
		/// 演算子が不正に連続している時の検証
		/// </summary>
		[Fact(DisplayName = "妥当性：++")]
		public void PlusPlusTest()
		{
			var token_list = new List<Token>(10)
			{
				MakeToken("+"),
				MakeToken("+"),
			};
			var validity_checker = new ValidityChecker();
			// 演算子連続なので、False期待
			Assert.False(validity_checker.OperatorCheck(token_list));
			// エラーが起きているフラグが立っているか検証
			Assert.True(validity_checker.ErrorOccurred);
			// 発生したエラーが適切か検証。ここでは演算子 RPARAM_SHOTAGE、が含まれることを期待
			Assert.Contains(ValidityError.OPERATOR_INVALID, validity_checker.ErrorList);
		}

		/// <summary>
		/// 演算子が不正に連続している時の検証
		/// </summary>
		[Fact(DisplayName = "妥当性：/*")]
		public void DuvMulTest()
		{
			var token_list = new List<Token>(10)
			{
				MakeToken("/"),
				MakeToken("*"),
			};
			var validity_checker = new ValidityChecker();
			// 演算子連続なので、False期待
			Assert.False(validity_checker.OperatorCheck(token_list));
			// エラーが起きているフラグが立っているか検証
			Assert.True(validity_checker.ErrorOccurred);
			// 発生したエラーが適切か検証。ここでは演算子 RPARAM_SHOTAGE、が含まれることを期待
			Assert.Contains(ValidityError.OPERATOR_INVALID, validity_checker.ErrorList);
		}

		/// <summary>
		/// 演算子が正しく連続している時(二つ目の演算子が-なので、負の値として不正ではない)の検証
		/// </summary>
		[Fact(DisplayName = "妥当性：*-20")]
		public void MulMunusTwentyTest()
		{
			var token_list = new List<Token>(10)
			{
				MakeToken("*"),
				MakeToken("-"),
				MakeToken("20"),
			};
			var validity_checker = new ValidityChecker();
			// 演算子連続だが2つめ '-' なので、True期待
			Assert.True(validity_checker.OperatorCheck(token_list));
		}

		/// <summary>
		/// 正しい括弧あり数式の時の検証
		/// </summary>
		[Fact(DisplayName = "妥当性：20*20/(10+10)")]
		public void MinusCalcTwentyTest()
		{
			var token_list = new List<Token>(30)
			{
				MakeToken("20"),
				MakeToken("*"),
				MakeToken("20"),
				MakeToken("/"),
				MakeToken("("),
				MakeToken("10"),
				MakeToken("+"),
				MakeToken("10"),
				MakeToken(")"),
				MakeToken(""),
			};
			var validity_checker = new ValidityChecker();
			// 正しい括弧つき数式なので、True期待
			Assert.True(validity_checker.OperatorCheck(token_list));
		}

		/// <summary>
		/// 正しい数式の検証
		/// </summary>
		[Fact(DisplayName = "妥当性：20*20/20")]
		public void MulDivTwentyTest()
		{
			var token_list = new List<Token>(10)
			{
				MakeToken("20"),
				MakeToken("*"),
				MakeToken("20"),
				MakeToken("/"),
				MakeToken("20"),
			};
			var validity_checker = new ValidityChecker();
			// 正しい数式なので、True期待
			Assert.True(validity_checker.OperatorCheck(token_list));
		}

		/// <summary>
		/// 正しい小数点表記の検証
		/// </summary>
		[Fact(DisplayName = "妥当性：20.34")]
		public void DotTrueCheck()
		{
			var token_list = new List<Token>(10)
			{
				MakeToken("20"),
				MakeToken("."),
				MakeToken("34"),
			};

			var validity_checker = new ValidityChecker();
			// 正しい数値なので、True期待
			Assert.True(validity_checker.DotCheck(token_list));
		}

		/// <summary>
		/// 正しくない小数点表記の検証
		/// </summary>
		[Fact(DisplayName = "妥当性：20.*34")]
		public void DotFalseCheck()
		{
			var token_list = new List<Token>(10)
			{
				MakeToken("20"),
				MakeToken("."),
				MakeToken("*"),
				MakeToken("34"),
			};

			var validity_checker = new ValidityChecker();
			// '-' に続くTokenが演算子なので、False期待
			Assert.False(validity_checker.DotCheck(token_list));
			// エラーが起きているフラグが立っているか検証
			Assert.True(validity_checker.ErrorOccurred);
			// 発生したエラーが適切か検証。ここでは '.' の後がおかしい DOT_INVALID、が含まれることを期待
			Assert.Contains(ValidityError.DOT_INVALID, validity_checker.ErrorList);
		}
	}

	public class ParseTests
	{
		// 演算子や数値からノード取得
		private static FormulaNode GetFormulaNode(string oper)
		{
			return oper switch
			{
				"+" => new AdditionNode(new Token(TokenType.PLUS, oper)),
				"-" => new AdditionNode(new Token(TokenType.MINUS, oper)),
				"*" => new MultiplicatoinNode(new Token(TokenType.MULITPLY, oper)),
				"/" => new MultiplicatoinNode(new Token(TokenType.DIVIDE, oper)),
				"." => new DotNode(new Token(TokenType.DOT, oper)),
				_ => new NumberNode(new Token(TokenType.NUBER, oper)),
			};
		}

		/// <summary>
		/// 数式から構文木を取得する共通メソッド
		/// </summary>
		/// <param name="formula">数式</param>
		/// <returns>構文木</returns>
		private static List<FormulaNode> GetFormlaNode(string formula)
			{
			// 数式を字句解析にかけて正当性チェック
			var lexer = new Lexer(formula);
			var token_list = lexer.GetToken();
			var validity_checker = new ValidityChecker();
			Assert.True(validity_checker.ValidityCheck(token_list));

			// 構文木取得
			var parser = new Parser();
			return parser.Parsing(token_list);

		}

		/// <summary>
		/// 構文木が正しく展開されているか比較検証
		/// </summary>
		/// <param name="formula_node">取得した構文木</param>
		/// <param name="result_node">期待する構文木</param>
		private static void CheckParse(List<FormulaNode> formula_node, List<FormulaNode> result_node)
		{
			var i = 0;
			foreach (var f in formula_node)
			{
				if (f.Type == NodeType.FORMULA)
				{
					// FormulaNodeが子ノードの場合、再帰的に呼び出し
					Assert.Equal(f.Node.Count, result_node[i].Node.Count);
					CheckParse(f.Node, result_node[i++].Node);
				}
				else
				{
					// 子ノードでなければ、同じか検証
					Assert.Equal(f.Type.ToString(), result_node[i++].Type.ToString());
				}
			}
		}

		[Fact(DisplayName = "20*20-6/2")]
		public void SimpleParsingTest()
		{
			// 期待する構文木
			var result_node = new List<FormulaNode>(30)
			{
				GetFormulaNode("20"),
				GetFormulaNode("*"),
				GetFormulaNode("20"),
				GetFormulaNode("-"),
				GetFormulaNode("6"),
				GetFormulaNode("/"),
				GetFormulaNode("2")
			};

			// 数式から構文木を取得
			var formula_node = GetFormlaNode("20*20-6/2");
			// 取得した構文木が期待通りか検証
			CheckParse(formula_node, result_node);
		}

		[Fact(DisplayName = "20*(10+5)/2")]
		public void ParamParsingTest()
		{
			// 括弧 (10+5) の中の構文木
			var child_node = new List<FormulaNode>(10)
			{
				GetFormulaNode("10"),
				GetFormulaNode("+"),
				GetFormulaNode("5"),
			};

			// 期待する構文木
			var result_node = new List<FormulaNode>(10)
			{
				GetFormulaNode("20"),
				GetFormulaNode("*"),
				new PriorisedFormulaNode(child_node),
				GetFormulaNode("/"),
				GetFormulaNode("2"),
			};

			// 数式から構文木を取得
			var formula_node = GetFormlaNode("20*(10+5)/2");
			// 取得した構文木が期待通りか検証
			CheckParse(formula_node, result_node);
		}

		[Fact(DisplayName = "(24-9)+((10+5)/2)")]
		public void DoubleParsingTest()
		{
			// 1つめの括弧 (24-9) の中の構文木
			var first_childnode = new List<FormulaNode>(5)
			{
				GetFormulaNode("24"),
				GetFormulaNode("-"),
				GetFormulaNode("9"),
			};

			// 2つめの括弧 ((10+5)/2) の内側 (10+5) の構文木
			var second_grand_childnode = new List<FormulaNode>(5)
			{
				GetFormulaNode("10"),
				GetFormulaNode("+"),
				GetFormulaNode("5"),
			};

			// 2つめの括弧 ((10+5)/2) の構文木
			var second_childnode = new List<FormulaNode>(5)
			{
				new PriorisedFormulaNode(second_grand_childnode),
				GetFormulaNode("/"),
				GetFormulaNode("2"),
			};

			// 期待する構文木
			var result_node = new List<FormulaNode>(5){
				new PriorisedFormulaNode(first_childnode),
				GetFormulaNode("+"),
				new PriorisedFormulaNode(second_childnode),
			};

			// 数式から構文木を取得
			var formula_node = GetFormlaNode("(24-9)+((10+5)/2)");
			// 取得した構文木が期待通りか検証
			CheckParse(formula_node, result_node);
		}


	}
}
