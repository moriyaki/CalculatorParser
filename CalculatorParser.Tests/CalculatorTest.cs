using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace CalculatorParser.Tests
{
	public class TokenLexerTests
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


		[Fact(DisplayName = "演算子")]
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

		[Fact(DisplayName = "数値")]
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

	public class ValidityCheckerTests
	{

		private Token MakeToken(string oper)
		{
			switch (oper)
			{
				case "+":
					return new Token(TokenType.PLUS, oper);
				case "-":
					return new Token(TokenType.MINUS, oper);
				case "*":
					return new Token(TokenType.MULITPLY, oper);
				case "/":
					return new Token(TokenType.DIVIDE, oper);
				case "(":
					return new Token(TokenType.LPARAM, oper);
				case ")":
					return new Token(TokenType.RPARAM, oper);
				case ".":
					return new Token(TokenType.DIVIDE, oper);
				case "":
					return new Token(TokenType.EOF, oper);
				default:
					return new Token(TokenType.NUBER, oper);

			}
		}


		[Fact(DisplayName = "妥当性：()")]
		public void ValidityParamTest()
		{
			var token_list = new List<Token>(){
				MakeToken("("),
				MakeToken(")"),
			};
			var validity_checker = new ValidityChecker();
			Assert.True(validity_checker.ParamCheck(token_list));
		}

		[Fact(DisplayName = "妥当性：()))")]
		public void RightParamOverTest()
		{
			var token_list = new List<Token>(){
				MakeToken("("),
				MakeToken(")"),
				MakeToken(")"),
				MakeToken(")"),
			};

			var validity_checker = new ValidityChecker();
			Assert.False(validity_checker.ParamCheck(token_list));
			Assert.True(validity_checker.ErrorOccurred);
			Assert.Contains(ValidityError.LPARAM_SHORTAGE, validity_checker.ErrorList);
		}

		[Fact(DisplayName = "妥当性：((()")]
		public void LeftParamOverTest()
		{
			var token_list = new List<Token>(){
				MakeToken("("),
				MakeToken("("),
				MakeToken("("),
				MakeToken(")"),
			};

			var validity_checker = new ValidityChecker();
			Assert.False(validity_checker.ParamCheck(token_list));
			Assert.True(validity_checker.ErrorOccurred);
			Assert.Contains(ValidityError.RPARAM_SHORTAGE, validity_checker.ErrorList);
		}

		[Fact(DisplayName = "妥当性：++")]
		public void PlusPlusTest()
		{
			var token_list = new List<Token>{
				MakeToken("+"),
				MakeToken("+"),
			};
			var validity_checker = new ValidityChecker();
			Assert.False(validity_checker.OperatorCheck(token_list));
		}

		[Fact(DisplayName = "妥当性：/*")]
		public void DuvMulTest()
		{
			var token_list = new List<Token>{
				MakeToken("/"),
				MakeToken("*"),
			};
			var validity_checker = new ValidityChecker();
			Assert.False(validity_checker.OperatorCheck(token_list));
			Assert.True(validity_checker.ErrorOccurred);
			Assert.Contains(ValidityError.OPERATOR_INVALID, validity_checker.ErrorList);
		}

		[Fact(DisplayName = "妥当性：*-20")]
		public void MulMunusTwentyTest()
		{
			var token_list = new List<Token>{
				MakeToken("*"),
				MakeToken("-"),
				MakeToken("20"),
			};
			var validity_checker = new ValidityChecker();
			Assert.True(validity_checker.OperatorCheck(token_list));
		}

		[Fact(DisplayName = "妥当性：20*20--20")]
		public void MinusCalcTwentyTest()
		{
			var token_list = new List<Token>{
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
			Assert.True(validity_checker.OperatorCheck(token_list));
		}


		[Fact(DisplayName = "妥当性：20*20/20")]
		public void MulDivTwentyTest()
		{
			var token_list = new List<Token>{
				MakeToken("20"),
				MakeToken("*"),
				MakeToken("20"),
				MakeToken("/"),
				MakeToken("20"),
			};
			var validity_checker = new ValidityChecker();
			Assert.True(validity_checker.OperatorCheck(token_list));
		}

		[Fact(DisplayName = "妥当性：20.34")]
		public void DotTrueCheck()
		{
			var token_list = new List<Token>{
				MakeToken("20"),
				MakeToken("."),
				MakeToken("34"),
			};

			var validity_checker = new ValidityChecker();
			Assert.True(validity_checker.DotCheck(token_list));
		}

		[Fact(DisplayName = "妥当性：20.*34")]
		public void DotFalseCheck()
		{
			var token_list = new List<Token>{
				MakeToken("20"),
				MakeToken("."),
				MakeToken("*"),
				MakeToken("34"),
			};

			var validity_checker = new ValidityChecker();
			Assert.False(validity_checker.DotCheck(token_list));
			Assert.True(validity_checker.ErrorOccurred);
			Assert.Contains(ValidityError.DOT_INVALID, validity_checker.ErrorList);
		}

		private List<FormulaNode> CheckToen(List<Token> token_list)
		{
			var validity_checker = new ValidityChecker();
			Assert.True(validity_checker.ValidityCheck(token_list));
			var parser = new Parser();
			return parser.Parsing(token_list);
		}

		[Fact(DisplayName = "20*20-6/2")]
		public void SimpleParsingTest()
		{
			var token_list = new List<Token>{
				MakeToken("20"),
				MakeToken("*"),
				MakeToken("10"),
				MakeToken("-"),
				MakeToken("6"),
				MakeToken("/"),
				MakeToken("2"),
				MakeToken(""),
			};

			var result_node = new List<FormulaNode>{
				new NumberNode(MakeToken("20")),
				new MultiplicatoinNode(MakeToken("*")),
				new NumberNode(MakeToken("10")),
				new AdditionNode(MakeToken("-")),
				new NumberNode(MakeToken("6")),
				new MultiplicatoinNode(MakeToken("/")),
				new NumberNode(MakeToken("2")),
			};

			var formula_node = CheckToen(token_list);
			Assert.Equal(formula_node[0].Type.ToString(), result_node[0].Type.ToString());	// 20
			Assert.Equal(formula_node[1].Type.ToString(), result_node[1].Type.ToString());	// *
			Assert.Equal(formula_node[2].Type.ToString(), result_node[2].Type.ToString());	// (
			Assert.Equal(formula_node[3].Type.ToString(), result_node[3].Type.ToString());	// /
			Assert.Equal(formula_node[4].Type.ToString(), result_node[4].Type.ToString());	// 2
			Assert.Equal(formula_node[5].Type.ToString(), result_node[5].Type.ToString());	// 2
			Assert.Equal(formula_node[6].Type.ToString(), result_node[6].Type.ToString());	// 2
		}

		[Fact(DisplayName = "20*(10+5)/2")]
		public void ParamParsingTest()
		{
			var token_list = new List<Token>{
				MakeToken("20"),
				MakeToken("*"),
				MakeToken("("),
				MakeToken("10"),
				MakeToken("+"),
				MakeToken("5"),
				MakeToken(")"),
				MakeToken("/"),
				MakeToken("2"),
				MakeToken(""),
			};
			var sub_node = new List<FormulaNode>{
				new NumberNode(MakeToken("10")),
				new AdditionNode(MakeToken("+")),
				new NumberNode(MakeToken("5")),
			};

			var result_node = new List<FormulaNode>{
				new NumberNode(MakeToken("20")),
				new MultiplicatoinNode(MakeToken("*")),
				new PriorisedFormulaNode(sub_node),
				new MultiplicatoinNode(MakeToken("/")),
				new NumberNode(MakeToken("2")),
			};
			var formula_node = CheckToen(token_list);

			Assert.Equal(formula_node[0].Type.ToString(), result_node[0].Type.ToString());	// 20
			Assert.Equal(formula_node[1].Type.ToString(), result_node[1].Type.ToString());	// *
			Assert.Equal(formula_node[2].Type.ToString(), result_node[2].Type.ToString());	// (
			Assert.Equal(formula_node[2].Node.Count, sub_node.Count);	// ノード数

			Assert.Equal(formula_node[2].Node[0].Type.ToString(), sub_node[0].Type.ToString());	// 10
			Assert.Equal(formula_node[2].Node[1].Type.ToString(), sub_node[1].Type.ToString());	// +
			Assert.Equal(formula_node[2].Node[2].Type.ToString(), sub_node[2].Type.ToString());	// 5

			Assert.Equal(formula_node[3].Type.ToString(), result_node[3].Type.ToString());	// /
			Assert.Equal(formula_node[4].Type.ToString(), result_node[4].Type.ToString());	// 2
		}
	}
}
