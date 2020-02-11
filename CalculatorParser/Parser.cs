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
        public static bool ParamCheck(List<Token> token)
        {
            // '(' で +1、')' で -1、マイナス値になったらエラー、最終的に 0 ならOK
            var param = 0;

            foreach (var t in token)
            {
                switch (t.Type)
                {
                    case TokenType.LPARAM:
                        param++;
                        break;
                    case TokenType.RPARAM:
                        param--;
                        break;
                }
                if (param < 0)
                {
                    return false;
                }
            }
            
            return param == 0;
        }


        public static bool ValidityCheck(List<Token> token)
        {
            // 括弧の数が合っているか
            if (!ParamCheck(token))
            {
                return false;
            }

            // 演算子が続いてないか
            // 続いていても 2つ目がマイナス、続いて数値ならOK

            // . の前後が数値かどうか


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
