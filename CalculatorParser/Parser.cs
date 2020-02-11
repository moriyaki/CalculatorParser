using System;
using System.Collections.Generic;

namespace CalculatorParser
{

    public enum NodeType
    {
        FORMULA,            // 括弧演算
        MULTIPLICATION,     // 乗除算
        ADDITION,           // 加減算
        NUMBER,             // 数値
    }

    // 式ノードクラス
   abstract public class FormulaNode 
    {
        public NodeType Type { get; protected set; }
        public bool Calced { get; protected set; } = false;
    }

    // 括弧演算クラス
    public class PriorisedFormulaNode : FormulaNode
    {
        public List<FormulaNode> Node { get; private set; }

        public PriorisedFormulaNode(List<FormulaNode> _node)
        {
            Node = _node;
            Type = NodeType.FORMULA;
        }

        public override string ToString()
        {
            return "";
        }
    }

    // 乗除算ノードクラス
    public class MultiplicatoinNode : FormulaNode
    {
        public Token Operator { get; private set; }

        public MultiplicatoinNode(Token _operator) 
        {
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
            Operator = _operator;
            Type = NodeType.MULTIPLICATION;
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

        public NumberNode(int _number) 
        {
            Number = _number;
            Type = NodeType.NUMBER;
        }

        public override string ToString()
        {
            return Number.ToString();
        }

    }

    public class ValidityChecker
    {
        /// <summary>
        /// // '(' で +1、')' で -1、マイナス値になったらエラー、最終的に 0 ならOK
        /// </summary>
        /// <param name="token">チェックするTokenリスト</param>
        /// <returns>括弧の整合性が取れているか</returns>
        public static bool ParamCheck(List<Token> token)
        {
            
            var param = 0;

            foreach (var t in token)
            {
                if (t.Type == TokenType.LPARAM) param++;
                if (t.Type == TokenType.RPARAM) param--;
                if (param < 0) return false;
            }
            return param == 0;
        }


        /// <summary>
        /// 演算子が続いていないか、続いていても2つ目がマイナス、かつ続くのが数値ならOK
        /// </summary>
        /// <param name="token">チェックするTokenリスト</param>
        /// <returns>演算子の整合性が取れているか</returns>
        public static bool OperatorCheck(List<Token> token)
        {
            for (var i = 0 ; i < token.Count; i++)
            {
                switch (token[i].Type)
                {
                    case TokenType.PLUS:
                    case TokenType.MINUS:
                    case TokenType.MULITPLY:
                    case TokenType.DIVIDE:
                        i++;
                        if (i == token.Count) return false;
                        // 次のTokenが演算子(非MINUS)か
                        switch (token[i].Type)
                        {
                            case TokenType.PLUS:
                            case TokenType.MULITPLY:
                            case TokenType.DIVIDE:
                                return false;
                            case TokenType.MINUS:
                                i++;
                                if (i == token.Count) return false;
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
        public static bool DotCheck(List<Token> token)
        {
            for (var i = 1; i < token.Count-1; i++)
            {
                if (token[i].Type != TokenType.DOT)
                {
                    continue;
                }
                if (!(token[i - 1].Type == TokenType.NUBER && token[i + 1].Type == TokenType.NUBER))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ValidityCheck(List<Token> token)
        {
            // 括弧の数が合っているか
            if (!ParamCheck(token))
            {
                return false;
            }

            // 演算子が続いてないか
            if (!OperatorCheck(token))
            {
                return false;
            }

            // . の前後が数値かどうか
            if (!DotCheck(token)){
                return false;
            }

            return true;
        }

    }

    public class Parser
    {
        public TokenType Type;
        public List<FormulaNode> nodes = null;



        public bool Parse(List<Token> token) 
        {
            // tokenListすべてを検索
                // 数値なら数値ノードの追加
                // LPARAM'('なら Parse を再帰読み出し
                // RPARAM')'なら 再帰呼び出しから抜ける
                // 演算子なら演算子ノードを追加

            return false;
        }
    }
}
