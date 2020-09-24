using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpressionCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.GetFullPath("expression.txt"); //each line have numbers spaced out by a "+", example: 2 + 5 + 3
            List<int> expressions = File.ReadLines(path).First().Split(new[] { " + " }, StringSplitOptions.None).Select(int.Parse).ToList();

            List<string> instructions = new List<string>();
            instructions.Add("MOVW R13, 0");
            instructions.Add("MOVT R13, 0");

            instructions.Add($"MOVW R1, {expressions[0]}");
            instructions.Add("MOVT R1, 0x0000");

            if (expressions.Count > 1)
            {
                foreach (var num in expressions.Skip(1))
                {
                    instructions.Add($"MOVW R6, {num}");
                    instructions.Add("MOVT R6, 0x0000");

                    instructions.Add("ADD R1, R6, R1");
                }
            }

            instructions.Add("STR {u,w} R1, (R13), 0x04");
            instructions.Add("LDR {p,w} R1, (R13), 0x04");
            instructions.Add("MOVW R4, 0");
            instructions.Add("MOVT R4, 0x3F20");
            instructions.Add("ADD R2, R4, 0x08");
            instructions.Add("LDR R3, (R2)");
            instructions.Add("ORR R3, R3, 0x00000008");
            instructions.Add("STR R3, (R2)");
            instructions.Add("ADD R3, R4, 0x1c");
            instructions.Add("MOVW R2, 0x0000");
            instructions.Add("MOVT R2, 0x0020");
            instructions.Add("STR R2, (R3)");
            instructions.Add("B {l} 13");
            instructions.Add("MOVW R4, 0");
            instructions.Add("MOVT R4, 0x3F20");
            instructions.Add("ADD R2, R4, 0x08");
            instructions.Add("LDR R3, (R2)");
            instructions.Add("ORR R3, R3, 0x00000008");
            instructions.Add("STR R3, (R2)");
            instructions.Add("ADD R3, R4, 0x28");
            instructions.Add("MOVW R2, 0x0000");
            instructions.Add("MOVT R2, 0x0020");
            instructions.Add("STR R2, (R3)");
            instructions.Add("B {l} 2");
            instructions.Add("SUB {s} R1, R1, 1");
            instructions.Add("BNE -25");
            instructions.Add("B 4");
            instructions.Add("MOVW R5, 0x4240");
            instructions.Add("MOVT R5, 0x000f");
            instructions.Add("SUB {s} R5, R5, 1");
            instructions.Add("BNE -3");
            instructions.Add("BX R14");

            File.WriteAllLines(Path.GetFullPath("expression.asm"), instructions);
        }
    }
}
