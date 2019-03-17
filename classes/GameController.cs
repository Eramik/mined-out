using System;

namespace Mined_Out {
    public class GameController {
        private Field field;
        public GameController() {
            this.field = FieldGenerator.Generate();
        }
        public void Run() {
            field.PrintToConsole();
            this.Walking();
        }

        public void Walking() {
            while(true) {
                var consoleKey = Console.ReadKey(true);
                ConsoleKey key = consoleKey.Key;
                Direction d;
                if(key == ConsoleKey.DownArrow) {
                    d = Direction.Down;
                } else if(key == ConsoleKey.UpArrow) {
                    d = Direction.Up;
                } else if(key == ConsoleKey.LeftArrow) {
                    d = Direction.Left;
                } else if(key == ConsoleKey.RightArrow) {
                    d = Direction.Right;
                } else {
                    continue;
                }
                
                Event result = field.Move(d);
                if(result != Event.Nothing) {
                    field.PrintToConsole();
                }
            }
        }
    }
}