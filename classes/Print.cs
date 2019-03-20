using System;

namespace Mined_Out {
    public static class Print {
        public static void WithStyle(Cell c) {
            if(c is Wall) {
                Wall();
                return;
            }
            Path p = (Path)c;
            if(p.IsSelected) {
                CustomBackground(p.Icon, ConsoleColor.DarkGray);
                return;
            }
            if(p.ContainsItem) {
                Item(p.Icon);
                return;
            }
            if(p.IsPlayerHere) {
                Player(p.Icon);
                return;
            }
            if(p.IsVisited) {
                VisitedPath();
                return;
            }
            if(p.IsExposed && p.IsMined) {
                Mine();
                return;
            }
            Console.Write(' ');
        }
        public static void Mine() {
            Custom('✘', ConsoleColor.Red);
        }
        public static void Wall() {
            Custom('#', ConsoleColor.Gray);
        }
        public static void VisitedPath() {
            Custom('·', ConsoleColor.Green);
        }
        public static void Player(char icon) {
            Custom(icon, ConsoleColor.DarkGreen);
        }
        public static  void Item(char icon) {
            Custom(icon, ConsoleColor.Yellow);
        }

        public static void Custom(char c, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.Write(c);
            Console.ResetColor();
        }
        public static void CustomBackground(char c, ConsoleColor color) {
            Console.BackgroundColor = color;
            Console.Write(c);
            Console.ResetColor();
        }

        public static void CustomLine(string s, ConsoleColor c) {
            Console.ForegroundColor = c;
            Console.WriteLine(s);
            Console.ResetColor();
        }
    }
}