using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorParser
{
	public enum ValidityError
	{
		// '(' 不足
		LPARAM_SHORTAGE,
		// ')' 不足
		RPARAM_SHORTAGE,
		// 演算子不正
		OPERATOR_INVALID,
		// '.' 前後不正
		DOT_INVALID,
	}


	public class ValidityChecker
	{
		public bool ErrorOccurred { get; private set; } = false;

		public IList<ValidityError> ErrorList = new List<ValidityError>();

		public IDictionary<ValidityError, string> ErrorMessage = new Dictionary<ValidityError, string>
		{
			[ValidityError.LPARAM_SHORTAGE] = "'('が不足しています",
			[ValidityError.RPARAM_SHORTAGE] = "')'が不足しています",
			[ValidityError.OPERATOR_INVALID] = "演算子が不正に続いています",
			[ValidityError.DOT_INVALID] = "'.'の前後が数値ではありません",
		};

		/// <summary>
		/// すべての構文テストを通す
		/// </summary>
		/// <param name="token">チェックするTokenリスト</param>
		/// <returns>構文テスト成功可否</returns>
		public bool ValidityCheck(IList<Token> token)
		{
			// 括弧の数が合っているか
			return ParamCheck(token) && OperatorCheck(token) && DotCheck(token);
		}

		/// <summary>
		/// エラー発生フラグを立て、エラー種別をリスト登録
		/// </summary>
		/// <param name="error">エラー種別</param>
		private void ErrorRegist(ValidityError error)
		{
			ErrorList.Add(error);
			ErrorOccurred = true;
		}

		/// <summary>
		/// // '(' で +1、')' で -1、マイナス値になったらエラー、最終的に 0 ならOK
		/// </summary>
		/// <param name="token">チェックするTokenリスト</param>
		/// <returns>括弧の整合性が取れているか</returns>
		public bool ParamCheck(IList<Token> token)
		{

			var param = 0;

			foreach (var t in token)
			{
				if (t.Type == TokenType.LPARAM) param++;
				if (t.Type == TokenType.RPARAM) param--;
				if (param < 0)
				{
					ErrorRegist(error: ValidityError.LPARAM_SHORTAGE);
					return false;
				}
			}
			if (param != 0)
			{
				ErrorRegist(error: ValidityError.RPARAM_SHORTAGE);
				return false;
			}
			return true;

		}


		/// <summary>
		/// 演算子が続いていないか、続いていても2つ目がマイナス、かつ続くのが数値ならOK
		/// </summary>
		/// <param name="token">チェックするTokenリスト</param>
		/// <returns>演算子の整合性が取れているか</returns>
		public bool OperatorCheck(IList<Token> token)
		{
			for (var i = 0; i < token.Count; i++)
			{
				switch (token[i].Type)
				{
					case TokenType.PLUS:
					case TokenType.MINUS:
					case TokenType.MULITPLY:
					case TokenType.DIVIDE:
						i++;
						if (i == token.Count)
						{
							ErrorRegist(error: ValidityError.OPERATOR_INVALID);
							return false;
						}
						// 次のTokenが演算子(非MINUS)か
						switch (token[i].Type)
						{
							case TokenType.PLUS:
							case TokenType.MULITPLY:
							case TokenType.DIVIDE:
								ErrorRegist(error: ValidityError.OPERATOR_INVALID);
								return false;
							case TokenType.MINUS:
								i++;
								if (i == token.Count)
								{
									ErrorRegist(error: ValidityError.OPERATOR_INVALID);
									return false;
								}
								// 演算子 + '-' + 数値なら負の数
								if (token[i].Type != TokenType.NUBER) return false;
								break;
						}
						break;
				}
			}
			return true;
		}

		/// <summary>
		/// '.' の前後が数字かどうか
		/// </summary>
		/// <param name="token">チェックするTokenリスト</param>
		/// <returns>小数点の整合性が取れているか</returns>        
		public bool DotCheck(IList<Token> token)
		{
			for (var i = 1; i < token.Count - 1; i++)
			{
				if (token[i].Type != TokenType.DOT)
				{
					continue;
				}
				if (token[i - 1].Type == TokenType.NUBER && token[i + 1].Type == TokenType.NUBER)
				{
					i++;
					continue;
				}
				ErrorRegist(error: ValidityError.DOT_INVALID);
				return false;
			}
			return true;
		}

	}

}
