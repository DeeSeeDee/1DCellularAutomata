using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1D_Cellular_Automata
{
    class Automaton
    {

        public char[] Cells
        {
            get { return cells; }
        }
        private char[] cells;

        private char[] ruleset;

        public int Size
        {
            get { return size; }
        }
        private int size = 5;

        public int Generation
        {
            get { return generation; }
        }
        private int generation = 0;

        public Automaton(int width, int rule)
        {
            cells = new char[width / size];
            string ruleString = ToBin(rule, 8);
            ruleset = ruleString.ToCharArray();
            for (int i = 0; i < ruleset.Length; i++)
            {
                if (ruleset[i] != '1')
                {
                    ruleset[i] = '\0';
                }
            }
            cells[cells.Length / 2] = '1';
        }

        public void Generate()
        {
            char[] nextGen = new char[cells.Length];
            for (int i = 1; i < cells.Length - 1; i++)
            {
                char left = cells[i - 1];
                char me = cells[i];
                char right = cells[i + 1];
                nextGen[i] = Rules(left, me, right);
            }
            cells = nextGen;
            generation++;
        }

        public static string ToBin(int value, int len)
        {
            return (len > 1 ? ToBin(value >> 1, len - 1) : null) + "01"[value & 1];
        }

        private char Rules(char a, char b, char c)
        {
            if (a == '1' && b == '1' && c == '1')
                return ruleset[0];
            else if (a == '1' && b == '1' && c == '\0')
                return ruleset[1];
            else if (a == '1' && b == '\0' && c == '1')
                return ruleset[2];
            else if (a == '1' && b == '\0' && c == '\0')
                return ruleset[3];
            else if (a == '\0' && b == '1' && c == '1')
                return ruleset[4];
            else if (a == '\0' && b == '1' && c == '\0')
                return ruleset[5];
            else if (a == '\0' && b == '\0' && c == '1')
                return ruleset[6];
            else if (a == '\0' && b == '\0' && c == '\0')
                return ruleset[7];
            return '1';
        }
    }
}
