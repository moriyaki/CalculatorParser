using System.Collections.Generic;

namespace CalculatorParser
{
    public class ParseNode
    {
        public TokenType Type;
        public int Number { get; set; }
        public List<ParseNode> parse_list = null;

        
        

    }


    public class Parser
    {
    }
}
