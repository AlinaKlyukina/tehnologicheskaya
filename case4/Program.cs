using System;
using System.Threading;

namespace TetrisGame
{
    public class Figure
    {
        public int[,] Shape { get; private set; }

        public Figure(int[,] shape)
        {
            Shape = shape;
        }

        // Вращение фигуры
        public void Rotate()
        {
            int rows = Shape.GetLength(0);
            int cols = Shape.GetLength(1);
            int[,] newShape = new int[cols, rows];

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    newShape[x, rows - y - 1] = Shape[y, x];
                }
            }

            Shape = newShape;
        }
    }

    public class GameField
    {
        private int[,] field;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public GameField(int width, int height)
        {
            Width = width;
            Height = height;
            field = new int[Height, Width];
        }

        // Отображение игрового поля с учетом текущей фигуры
        public void Draw(Figure figure, int offsetX, int offsetY)
        {
            Console.Clear();

            // Верхняя граница
            Console.WriteLine("TETRIS  GAME");
            Console.WriteLine(new string('-', Width + 2));

            for (int y = 0; y < Height; y++)
            {
                Console.Write("|"); // Левая граница
                for (int x = 0; x < Width; x++)
                {
                    if (IsPartOfFigure(figure, x, y, offsetX, offsetY))
                    {
                        Console.Write("#"); // Рисуем часть фигуры
                    }
                    else
                    {
                        Console.Write(field[y, x] == 1 ? "#" : " "); // Рисуем поле
                    }
                }
                Console.Write("|"); // Правая граница
                Console.WriteLine();
            }

            // Нижняя граница
            Console.WriteLine(new string('-', Width + 2));
        }

        // Проверяем, является ли координата частью фигуры
        private bool IsPartOfFigure(Figure figure, int x, int y, int offsetX, int offsetY)
        {
            int figureWidth = figure.Shape.GetLength(1);
            int figureHeight = figure.Shape.GetLength(0);

            if (x >= offsetX && x < offsetX + figureWidth && y >= offsetY && y < offsetY + figureHeight)
            {
                return figure.Shape[y - offsetY, x - offsetX] == 1;
            }
            return false;
        }

        // Размещение фигуры на поле
        public void PlaceFigure(Figure figure, int offsetX, int offsetY)
        {
            for (int y = 0; y < figure.Shape.GetLength(0); y++)
            {
                for (int x = 0; x < figure.Shape.GetLength(1); x++)
                {
                    if (figure.Shape[y, x] == 1)
                    {
                        if (offsetY + y >= 0 && offsetY + y < Height && offsetX + x >= 0 && offsetX + x < Width)
                        {
                            field[offsetY + y, offsetX + x] = 1;
                        }
                    }
                }
            }
        }

        // Проверка на допустимость позиции фигуры
        public bool IsValidPosition(Figure figure, int offsetX, int offsetY)
        {
            for (int y = 0; y < figure.Shape.GetLength(0); y++)
            {
                for (int x = 0; x < figure.Shape.GetLength(1); x++)
                {
                    if (figure.Shape[y, x] == 1)
                    {
                        int newX = offsetX + x;
                        int newY = offsetY + y;

                        if (newX < 0 || newX >= Width || newY < 0 || newY >= Height || field[newY, newX] == 1)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int fieldWidth = 10;
            int fieldHeight = 11;
            GameField gameField = new GameField(fieldWidth, fieldHeight);

            Figure currentFigure = new Figure(new int[,] { { 1, 1 } });
            int posX = 4;
            int posY = 0;

            while (true)
            {
                gameField.Draw(currentFigure, posX, posY);

                // Логика управления
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.LeftArrow && gameField.IsValidPosition(currentFigure, posX - 1, posY))
                    {
                        posX--;
                    }
                    else if (key == ConsoleKey.RightArrow && gameField.IsValidPosition(currentFigure, posX + 1, posY))
                    {
                        posX++;
                    }
                    else if (key == ConsoleKey.UpArrow)
                    {
                        currentFigure.Rotate();
                        if (!gameField.IsValidPosition(currentFigure, posX, posY))
                        {
                            // Отменить поворот, если не хватает места
                            currentFigure.Rotate();
                            currentFigure.Rotate();
                            currentFigure.Rotate();
                        }
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        // Пока фигура может падать, сдвигаем её вниз
                        while (gameField.IsValidPosition(currentFigure, posX, posY + 1))
                        {
                            posY++;
                        }
                    }
                }

                // Проверка падения фигуры
                if (!gameField.IsValidPosition(currentFigure, posX, posY + 1))
                {
                    gameField.PlaceFigure(currentFigure, posX, posY);
                    posX = 4;
                    posY = 0;

                    // Проверка конца игры
                    if (!gameField.IsValidPosition(currentFigure, posX, posY))
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Game Over!");
                        Console.WriteLine("Press R to restart...");
                        while (Console.ReadKey(true).Key != ConsoleKey.R) ;

                        gameField = new GameField(fieldWidth, fieldHeight);
                        currentFigure = new Figure(new int[,] { { 1, 1 } });
                        posX = 4;
                        posY = 0;
                    }
                }
                else
                {
                    posY++;
                }

                Thread.Sleep(500); // Уменьшаем скорость падения фигуры
            }
        }
    }
}
