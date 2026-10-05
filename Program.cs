using System.Collections.Generic;
using System.Threading;

namespace Snaker;

struct Location
{
    public int X;
    public int Y;

    public Location(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
};

enum Direction
{
    Left,
    Right,
    Up,
    Down
};

class Program
{
    private static int score = 0;
    
    static string[] grid = new string[]
    {

    };
    
    private static Direction direction;
    static List<Location> snake = new List<Location>();
    static Location fruit = new Location(60, 15);

    static bool quit = false;
    const int FieldTop = 8;
    static void Main(string[] args)
    {
        Console.Clear();
        string text = @"  ________   _____  ___         __       __   ___    _______  
 /""       ) (\""   \|""  \       /""""\     |/""| /  "")  /""     ""| 
(:   \___/  |.\\   \    |     /    \    (: |/   /  (: ______) 
 \___  \    |: \.   \\  |    /' /\  \   |    __/    \/    |   
  __/  \\   |.  \    \. |   //  __'  \  (// _  \    // ___)_  
 /"" \   :)  |    \    \ |  /   /  \\  \ |: | \  \  (:      ""| 
(_______/    \___|\____\) (___/    \___)(__|  \__)  \_______) ";
        
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.WriteLine("_______________________________created by mohalbyt______________________________________________");
        
        Console.CursorVisible = false;

        grid = CreateGrid();
        
        Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight);
        Location head = new Location(50, 12);
        snake.Add(head);
        
        Location next;
        
        direction = Direction.Right;
        
        //Thread
        Thread thread = new Thread(Move);
        thread.IsBackground = true;
        thread.Start();
       
        while (!quit)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    return;
                case ConsoleKey.UpArrow:
                    if(direction != Direction.Down)
                        direction = Direction.Up;
                    break;
                case ConsoleKey.DownArrow:
                    if(direction != Direction.Up)
                        direction = Direction.Down;
                    break;
                case ConsoleKey.LeftArrow:
                    if(direction != Direction.Right)
                        direction = Direction.Left;
                    break;
                case ConsoleKey.RightArrow:
                    if(direction != Direction.Left)
                        direction = Direction.Right;
                    break;
            }
        }
    }
    static int width = 80;
    static int height = 25;
    static Location scoreLoc =  new Location(81, 2);


    public static string[] CreateGrid()
    {
    
        string[] grid = new string[height];
        for (int y = 0; y < height; y++)
        {
            if (y == 0 || y == height - 1)
            {
                grid[y] = new string('X', width);
            }
            else
            {
                grid[y] = "X" + new string(' ', width - 2) + "X";
            }
        }
        
        char[] row = grid[12].ToCharArray();

        for (int x = 35; x < 45; x++)
        {
            row[x] = 'X';
        }

        grid[12] = new string(row);
        
        row = grid[7].ToCharArray();

        for (int x = 20; x < 25; x++)
        {
            row[x] = 'X';
        }

        grid[7] = new string(row);

        row = grid[18].ToCharArray();

        for (int x = 55; x < 62; x++)
        {
            row[x] = 'X';
        }

        grid[18] = new string(row);

        return grid;
    }
    
    static Random random = new Random();
   
  
    public static void ChooseFruits()
    {
        int choice = random.Next(4);
        if (choice < 3)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write('*');
        }

        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('*');  
        }
    }
    
    
    public static void Move()
    {
        var gridChecker = false;
        while (true)
        {
            {
                var next = snake[0];
                var tail = snake[snake.Count - 1];

                switch (direction)
                {
                    case Direction.Left:
                        if (next.X > 1)
                            next.X--;
                        else
                            next.X = width - 2;
                        break;

                    case Direction.Right:
                        if (next.X < width - 2)
                            next.X++;
                        else
                            next.X = 1;
                        break;
                    case Direction.Up:
                        if (next.Y > 1)
                            next.Y--;
                        else
                            next.Y =  height - 2;
                        break;

                    case Direction.Down:
                        if (next.Y < height - 2)
                            next.Y++;
                        else
                            next.Y = 1;
                        break;
                }
                
                if (grid[next.Y][next.X] == 'X')
                {
                    quit = true;
                    return;
                }
                
                bool hitHimself = false;
                for (int i = 0; i < snake.Count; i++)
                {
                    if (snake[i].X == next.X && snake[i].Y == next.Y)
                    {
                        hitHimself = true;
                        quit = true;
                    }
                    else
                    {
                        hitHimself = false;
                    }
                }
                
                //Snake
                Console.SetCursorPosition(tail.X, FieldTop + tail.Y);
                Console.Write(' ');
                snake.Insert(0, next);
                Console.SetCursorPosition(next.X, FieldTop + next.Y);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write((char)178);
                Console.ResetColor();


                //wall
                if (!gridChecker)
                {
                    gridChecker = true;
                    
                    for (int i = 0; i < grid.Length; i++)
                    {
                        Console.SetCursorPosition(0, FieldTop + i);
                        Console.Write(grid[i]);
                    }
                }
                
                Console.SetCursorPosition(fruit.X, FieldTop + fruit.Y);
                
                ChooseFruits();
                Console.ResetColor();
                
                
                
                if (next.X == fruit.X && next.Y == fruit.Y)
                {
                    Random random = new Random();
                    fruit.X = random.Next(1, 65);
                    fruit.Y = random.Next(8, 24);
                }
                else
                {
                    Console.SetCursorPosition(tail.X, FieldTop + tail.Y);
                    Console.Write(' ');

                    snake.RemoveAt(snake.Count - 1);
                }
                // Delaying
                int delay = 120;
                if (direction == Direction.Down || direction == Direction.Up)
                {
                   Thread.Sleep(delay);
                }
                else if (direction == Direction.Left || direction == Direction.Right)
                {
                    Thread.Sleep(delay / 2);
                }

                for (int i = 0; i < snake.Count; i++)
                {
                    score = i;
                    Console.SetCursorPosition(scoreLoc.X, FieldTop + scoreLoc.Y);
                    Console.Write(score);
                }
            }
        }
    }
}