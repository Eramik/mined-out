using System;

namespace Mined_Out {
    public class GameController {
        private Field field;
        private void Generate() {
            this.field = FieldGenerator.Generate();
        }
        public void Run() {
            while(true) {
                this.Generate();
                field.PrintToConsole();
                this.Walking();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
        }

        public void Walking() {
            while(true) {
                var consoleKey = Console.ReadKey(true);
                ConsoleKey key = consoleKey.Key;
                Direction d;
                if(key == ConsoleKey.DownArrow ||
                    key == ConsoleKey.S) {
                    d = Direction.Down;
                } else if(key == ConsoleKey.UpArrow || 
                    key == ConsoleKey.W) {
                    d = Direction.Up;
                } else if(key == ConsoleKey.LeftArrow || 
                    key == ConsoleKey.A) {
                    d = Direction.Left;
                } else if(key == ConsoleKey.RightArrow || 
                    key == ConsoleKey.D) {
                    d = Direction.Right;
                } else {
                    continue;
                }
                
                Event result = field.Move(d);
                if (result == Event.Boom) {
                    Console.Clear();
                    Console.WriteLine("YOU DIED!");
                    return;
                }
                if (result == Event.Finished) {
                    Console.Clear();
                    Console.WriteLine("$$$ YOU WON $$$");
                    return;
                }
                if(result != Event.Nothing) {
                    field.PrintToConsole();
                }
                
            }
        }
    }
}