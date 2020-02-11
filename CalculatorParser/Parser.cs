using System;
using System.Collections.Generic;

namespace CalculatorParser
{

    public enum NodeType
    {
        PRIORITY_FORMULA,   // 括弧演算
        MULTIPLICATION,     // 乗除算
        ADDITION,           // 加減算
        NUMBER,             // 
    }

    public abstract class FormulaNode 
    {
        public NodeType Node { get; private set; }
    }

    // 括弧演算クラス
    public class PriorityFormulaNode : FormulaNode
    {
        public List<FormulaNode> Nodes { get; private set; }

        public PriorityFormulaNode(List<FormulaNode> _nodes) => Nodes = _nodes;
    }

    // 乗除算ノードクラス
    public class MultiplicatoinNode : FormulaNode
    {
        public Token Operator { get; private set; }

        public MultiplicatoinNode(Token _operator) => Operator = _operator;
    }

    public class AdditionNode : FormulaNode 
    {
        public Token Operator { get; private set; }

        public AdditionNode(Token _operator) => Operator = _operator;

    }

    public class NumberNode : FormulaNode
    {
        public int Number { get; private set; }

        public NumberNode(int _number) => Number = _number;
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
