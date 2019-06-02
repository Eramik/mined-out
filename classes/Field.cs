using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Mined_Out
{

    public struct Coords {
        public Coords(int i, int j) {
            this.i = i;
            this.j = j;
        }
        public int i {set; get;}
        public int j {set; get;}
    }
    public enum Direction {
        Up,
        Right,
        Left,
        Down
    }
    public enum Event {
        Boom,
        Step,
        Nothing,
        Finished
    }
    public class Field {
        protected Cell[,] field;
        protected Inventory inventory;

        public Cell this[int i, int j] {
            get {
                return field[i,j];
            }
            set {
                field[i,j] = value;
            }
        }
        protected Path playerCell {get {
            return field[player.coords.i, player.coords.j] as Path;
        }}

        protected Path player2Cell {get {
            return field[player2.coords.i, player2.coords.j] as Path;
        }}

        public int SuitableCellsAmount {get {
            int a = 0;
            for(int i = 1; i < Height - 1; i++) {
                for(int j = 1; j < Width - 1; j++) {
                    if(IsSuitable(i, j)) {
                        a++;
                    }
                }
            }
            return a;
        }}

        protected Player player;
        protected Player player2 = null;
        bool twoPlayersMode = false;
        public bool HasPlayer1 {get{
            return this.player != null;
        }}
        public Coords PlayerCoords {get {
            if(player == null) {
                return player2.coords;
            }
            return player.coords;
        }}
        public Coords Player2Coords {get {
            if(player2 == null) {
                return player.coords;
            } else {
                return player2.coords;
            }
        }}
        
        public Field(int height, int width, Inventory i) {
            this.field = new Cell[height, width];
            this.initEmptyField();
            this.inventory = i;
        }

        public Field(int height, int width, Inventory i, bool twoPlayers) {
            this.field = new Cell[height, width];
            this.initEmptyField();
            this.inventory = i;
            this.twoPlayersMode = twoPlayers;
        }

        public void SetPlayer(int i, int j, int playerNumber = 1) {
            if(playerNumber == 1) {
                this.player = new Player(i, j);
                this.field[i, j] = new Path(player: true);
            } else {
                this.player2 = new Player(i, j);
                this.field[i, j] = new Path(player: true, playerNumber: 2);
            }
            
        }
        protected int CalculateMines (Coords c) {
            return this.CalculateMines(c.i, c.j);
        }

        public void SetProtected(int i, int j) {
            ((Path)field[i,j]).IsProtected = true;
        }
        public void SetProtected(Coords c) {
            SetProtected(c.i, c.j);
        }

        public bool IsSuitable(int i, int j, bool acceptProtected = false, bool ignorePlayer = true) {
            return IsSuitable(new Coords(i, j), acceptProtected, ignorePlayer);
        }
        // Used for generating levels
        public bool IsSuitable(Coords c, bool acceptProtected = false, bool ignorePlayer = true) {
            if(c.i < 0 || c.j < 0 || c.i >= Height || c.j >= Width) {
                return false;
            }
            if(field[c.i, c.j] is Wall) {
                return false;
            }
            if(isMined(c.i, c.j) || (!acceptProtected && IsProtected(c))) {
                return false;
            }
            if(!ignorePlayer && ((Path)field[c.i, c.j]).IsPlayerHere) {
                return false;
            }
            return true;
        }

        public bool IsPath(int i, int j) {
            if(i < 0 || j < 0 || i >= Height || j >= Width) {
                return false;
            }
            if(field[i, j] is Wall) {
                return false;
            }
            return true;
        }
        public bool IsProtected(Coords c) {
            return ((Path)field[c.i,c.j]).IsProtected;
        }
        public void PlantMine(int i, int j) {
            Cell c = field[i,j];
            Path p = c as Path;
            if(p == null) {
                p = new Path();
                field[i, j] = p;
            }
            if(p.IsPlayerHere) {
                p.PlayerLeft();
                player = null;
            }
            p.IsMined = true;
        }

        public void PlaceWall(Coords c) {
            Path p = field[c.i, c.j] as Path;
            if(p != null && p.IsPlayerHere) {
                this.player = null;
            }
            field[c.i, c.j] = new Wall();
        }
        
        public void PlaceScanner(Coords c) {
            Path p = field[c.i, c.j] as Path;
            if(p != null && p.IsPlayerHere) {
                this.player = null;
            }
            field[c.i, c.j] = new Path(new Scanner());
        }

        public bool IsFinish(int i, int j) {
            if(!IsSuitable(i, j, false, true)) {
                return false;
            }
            if(i == 0) {
                return true;
            }
            return false;
        }
        public bool IsFinish(Coords c) {
            return IsFinish(c.i, c.j);
        }

        public void SaveToFile(string path) {
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, field);
            }
        } 

        public void LoadFromFile(string path)
        {
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                this.field = (Cell[,])serializer.Deserialize(file, typeof(Path[,]));
            }

            for(int i = 0; i < field.GetLength(0); i++)
            {
                for(int j = 0; j < field.GetLength(1); j++)
                {
                    if (((Path)field[i, j]).IsWall)
                    {
                        field[i, j] = new Wall();
                        continue;
                    }
                    if (((Path)field[i, j]).item != null)
                    {
                        ((Path)field[i, j]).PutItem(new Scanner());
                    }
                    if (((Path)field[i, j]).IsVisited && !((Path)field[i, j]).IsPlayerHere)
                    {
                        ((Path)field[i, j]).Icon = '.';
                    }
                    if (((Path)field[i, j]).IsPlayerHere)
                    {
                        if(((Path)field[i, j]).PlayerNumber == 1)
                        {
                            if (this.player == null)
                            {
                                this.player = new Player(i, j);
                            }
                            else
                            {
                                this.player.coords = new Coords(i, j);
                            }
                        } else
                        {
                            if(this.player2 == null)
                            {
                                this.player2 = new Player(i, j);
                            } else
                            {
                                this.player2.coords = new Coords(i, j);
                            }
                        }
                    }
                }
            }
        }

        protected int CalculateMines(int i, int j) {
            int mines = 0;

            if(i - 1 >= 0 && isMined(i - 1, j)) {
                mines++;
            }

            if(j - 1 >= 0 && isMined(i, j - 1)) {
                mines++;
            }

            if(i + 1 < field.GetLength(0) && isMined(i+1, j)) {
                mines++;
            }

            if(j + 1 < field.GetLength(1) && isMined(i, j+1)) {
                mines++;
            }
            
            return mines;
        }
        protected bool isMined(int i, int j) {
            Path p = field[i, j] as Path;
            return p != null && p.IsMined;
        }
        public int Width {get {
            return field.GetLength(1);
        }}
        public int Height {get {
            return field.GetLength(0);
        }}
        protected void initEmptyField() {
            for(int i = 0; i < this.field.GetLength(0); i++) {
                for(int j = 0; j < this.field.GetLength(1); j++) {
                    this.field[i, j] = new Path();
                }
            }
        }

        private void ResetChecks() {
            foreach(Cell c in field) {
                Path p = c as Path;
                if(p != null) p.Checked = false;
            }
        }
        private void ResetCheckNumbers() {
            foreach(Cell c in field) {
                Path p = c as Path;
                if(p != null) p.CheckNumber = -1;
            }
        }

        public Coords GetHint() {
            Queue<Coords> queue = new Queue<Coords>();
            queue.Enqueue(PlayerCoords);
            Coords c;
            ResetChecks();
            ResetCheckNumbers();
            int checkNumber;
            playerCell.CheckNumber = 0;
            while(true) {
                c = queue.Dequeue();
                Path p = field[c.i, c.j] as Path;
                p.Checked = true;
                checkNumber = p.CheckNumber;
                if(IsFinish(c)) {
                    break;
                }
                
                // Add adjacent elements
                if(IsSuitable(c.i + 1, c.j, true) &&
                    !((Path)field[c.i + 1, c.j]).Checked) {
                    queue.Enqueue(new Coords(c.i + 1, c.j));
                    ((Path)field[c.i + 1, c.j]).CheckNumber = checkNumber + 1;
                    ((Path)field[c.i + 1, c.j]).Checked = true;
                }
                if(IsSuitable(c.i - 1, c.j, true) &&
                    !((Path)field[c.i - 1, c.j]).Checked) {
                    queue.Enqueue(new Coords(c.i - 1, c.j));
                    ((Path)field[c.i - 1, c.j]).Checked = true;
                    ((Path)field[c.i - 1, c.j]).CheckNumber = checkNumber + 1;
                }
                if(IsSuitable(c.i, c.j + 1, true) &&
                    !((Path)field[c.i, c.j + 1]).Checked) {
                    queue.Enqueue(new Coords(c.i, c.j + 1));
                    ((Path)field[c.i, c.j + 1]).Checked = true;
                    ((Path)field[c.i, c.j + 1]).CheckNumber = checkNumber + 1;
                }
                if(IsSuitable(c.i, c.j - 1, true)  &&
                    !((Path)field[c.i, c.j - 1]).Checked) {
                    queue.Enqueue(new Coords(c.i, c.j - 1));
                    ((Path)field[c.i, c.j - 1]).Checked = true;
                    ((Path)field[c.i, c.j - 1]).CheckNumber = checkNumber + 1;
                }
                //Console.ReadKey(true);
            }
            while(true) {
                //((Path)field[c.i, c.j]).Select();
                if(IsSuitable(c.i + 1, c.j, true) &&
                    ((Path)field[c.i + 1, c.j]).CheckNumber == checkNumber - 1) {
                    if(((Path)field[c.i + 1, c.j]).IsPlayerHere) {
                        return c;
                    }
                    c = new Coords(c.i + 1, c.j);
                    checkNumber--;
                }
                if(IsSuitable(c.i - 1, c.j, true) &&
                    ((Path)field[c.i - 1, c.j]).CheckNumber == checkNumber - 1) {
                    if(((Path)field[c.i - 1, c.j]).IsPlayerHere) {
                        return c;
                    }
                    c = new Coords(c.i - 1, c.j);
                    checkNumber--;
                }
                if(IsSuitable(c.i, c.j + 1, true) &&
                    ((Path)field[c.i, c.j + 1]).CheckNumber == checkNumber - 1) {
                    if(((Path)field[c.i, c.j + 1]).IsPlayerHere) {
                        return c;
                    }
                    c = new Coords(c.i, c.j + 1);
                    checkNumber--;
                }
                if(IsSuitable(c.i, c.j - 1, true) &&
                    ((Path)field[c.i, c.j - 1]).CheckNumber == checkNumber - 1) {
                    if(((Path)field[c.i, c.j - 1]).IsPlayerHere) {
                        return c;
                    }
                    c = new Coords(c.i, c.j - 1);
                    checkNumber--;
                }
                //Console.ReadKey(true);
            }
            throw new Exception("Hint not found");
        }

        public Event Emulate() {
            Random rnd = new Random();
            Direction[] directions = new Direction[4];
            directions[0] = Direction.Down;
            directions[1] = Direction.Left;
            directions[2] = Direction.Right;
            directions[3] = Direction.Up;
            int playerNumber;
            while(true) {
                if(this.player == null) {
                    playerNumber = 2;
                } else {
                    playerNumber = 1;
                }
                this.PrintToConsole();
                System.Threading.Thread.Sleep(500);
                if(player == null && player2 == null) {
                    return Event.Boom;
                }
                if(rnd.Next(100) < 40) {
                    var e = this.Move(directions[rnd.Next(4)], playerNumber);
                    if(e == Event.Boom || e == Event.Finished) {
                        return e;
                    }
                    continue;
                }
                Direction d = GetDirectionTo(GetHint());
                var e2 = this.Move(d, playerNumber);
                if(e2 == Event.Boom || e2 == Event.Finished) {
                    return e2;
                }
            }
        }

        private Direction GetDirectionTo(Coords c) {
            var cc = PlayerCoords;
            if(cc.i + 1 == c.i) {
                return Direction.Down;
            }
            if(cc.i - 1 == c.i) {
                return Direction.Up;
            }
            if(cc.j + 1 == c.j) {
                return Direction.Right;
            }
            return Direction.Left;
        }
        public bool CheckIsWinnable() {
            Queue<Coords> queue = new Queue<Coords>();
            queue.Enqueue(PlayerCoords);
            Coords c;
            ResetChecks();
            while(queue.Count > 0) {
                c = queue.Dequeue();
                if(IsFinish(c)) return true;
                Path p = field[c.i, c.j] as Path;
                p.Checked = true;
                // Add adjacent elements
                if(IsSuitable(c.i + 1, c.j, true) &&
                    !((Path)field[c.i + 1, c.j]).Checked) {
                    queue.Enqueue(new Coords(c.i + 1, c.j));
                }
                if(IsSuitable(c.i - 1, c.j, true) &&
                    !((Path)field[c.i - 1, c.j]).Checked) {
                    queue.Enqueue(new Coords(c.i - 1, c.j));
                }
                if(IsSuitable(c.i, c.j + 1, true) &&
                    !((Path)field[c.i, c.j + 1]).Checked) {
                    queue.Enqueue(new Coords(c.i, c.j + 1));
                }
                if(IsSuitable(c.i, c.j - 1, true)  &&
                    !((Path)field[c.i, c.j - 1]).Checked) {
                    queue.Enqueue(new Coords(c.i, c.j - 1));
                }
            }
            return false;
        }

        public void PrintToConsole(string[] inventoryOutput = null) {
            if(inventoryOutput == null) {
                inventoryOutput = inventory.ToArrayOfStrings();
            }
            Console.Clear();
            for(int i = 0; i < this.field.GetLength(0); i++) {
                for(int j = 0; j < this.field.GetLength(1); j++) {
                    Print.WithStyle(this.field[i, j]);
                    Console.Write(' ');
                }
                if(i < inventoryOutput.Length) {
                    Console.Write(inventoryOutput[i]);
                }
                Console.WriteLine();
            }
        }

        public void ReplacePlayer(Coords c) {
            if(this.player != null)
                playerCell.PlayerLeft();
            this.player = new Player(c.i, c.j);
            if(field[c.i, c.j] is Wall) {
                field[c.i, c.j] = new Path();
            }
            ((Path)field[c.i, c.j]).PlayerEntered();
        }

        public Event Move(Direction direction, int playerNumber = 1) {
            Player player;
            Path playerCell;
            if(this.player == null && this.player2 == null) {
                return Event.Boom;
            }
            if(playerNumber == 1) {
                if(this.player == null) {
                    return Event.Nothing;
                }
                player = this.player;
                playerCell = this.playerCell;
            } else {
                if(this.player2 == null) {
                    return Event.Nothing;
                }
                player = this.player2;
                playerCell = this.player2Cell;
            }
            switch(direction) {
                case Direction.Down:
                    if(player.coords.i + 1 < field.GetLength(0) &&
                        !(field[player.coords.i + 1, player.coords.j] is Wall) &&
                        !((Path)field[player.coords.i + 1, player.coords.j]).IsPlayerHere) {
                        playerCell.PlayerLeft();
                        player.coords.i += 1;
                    } else {
                        return Event.Nothing;
                    }
                    break;
                case Direction.Up:
                    if(player.coords.i - 1 >= 0 &&
                        !(field[player.coords.i - 1, player.coords.j] is Wall) &&
                        !((Path)field[player.coords.i - 1, player.coords.j]).IsPlayerHere) {
                        playerCell.PlayerLeft();
                        player.coords.i--;
                    } else {
                        return Event.Nothing;
                    }
                    break;
                case Direction.Left:
                    if(player.coords.j - 1 >= 0 &&
                        !(field[player.coords.i, player.coords.j - 1] is Wall) &&
                        !((Path)field[player.coords.i, player.coords.j - 1]).IsPlayerHere) {
                        playerCell.PlayerLeft();
                        player.coords.j--;
                    } else {
                        return Event.Nothing;
                    }
                    break;
                case Direction.Right:
                    if(player.coords.j + 1 < field.GetLength(1) &&
                        !(field[player.coords.i, player.coords.j + 1] is Wall) &&
                        !((Path)field[player.coords.i, player.coords.j + 1]).IsPlayerHere) {
                        playerCell.PlayerLeft();
                        player.coords.j++;
                    } else {
                        return Event.Nothing;
                    }
                    break;
            }
            return PlayerDidMove(playerNumber);
        }

        public void Expose() {
            foreach(Cell c in field) {
                Path p = c as Path;
                if(p != null) {
                    p.Expose();
                }
            }
        }

        public void Hide() {
            foreach(Cell c in field) {
                Path p = c as Path;
                if(p != null) {
                    p.Hide();
                }
            }
        }

        public void ClearCell(Coords c) {
            Path p = field[c.i, c.j] as Path;
            if(p != null && p.IsPlayerHere) {
                this.player = null;
            }
            field[c.i, c.j] = new Path();
        }

        public bool IsInField(int i, int j) {
            if(i < 1 || j < 0) return false;
            if(i >= field.GetLength(0) || j >= field.GetLength(1))
                return false;
            return true;
        }

        protected Event PlayerDidMove(int playerNumber = 1) {
            Player player;
            Path playerCell;
            if(playerNumber == 1) {
                if(this.player == null) {
                    return Event.Nothing;
                }
                player = this.player;
                playerCell = this.playerCell;
            } else {
                if(this.player2 == null) {
                    return Event.Nothing;
                }
                player = this.player2;
                playerCell = this.player2Cell;
            }

            if(playerCell.IsMined) {
                if(player == null && player2 == null) {
                    return Event.Boom;
                }
                playerCell.Expose();
                if(playerNumber == 1) {
                    this.player = null;
                } else {
                    this.player2 = null;
                }
                return Event.Step;
            }
            char playerIcon = CalculateMines(player.coords).ToString()[0];
            InventoryItem item = playerCell.PlayerEntered(playerIcon, playerNumber);
            if(item != null) {
                inventory.Add(item);
            }
            if(IsFinish(player.coords)) {
                return Event.Finished;
            }
            return Event.Step;
        }
    }
}