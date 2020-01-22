using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Arcanoid
{
    public partial class MainWindow : Window
    {
        Random rand = new Random();
        DispatcherTimer timer;
        Rectangle platform;
        Ellipse ball;
        List<Rectangle> list = new List<Rectangle>();
        int dirX = 1, dirY = -1;

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();
            dirX = 1;
            dirY = -1;
            list.Clear();

            for(int i = 0; i < 40; i++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                MainGrid.RowDefinitions.Add(new RowDefinition());
            }

            for(int i = 0; i < 100; i++)
            {
                int row = rand.Next(0, 20);
                int col = rand.Next(0, 39);
                int children = MainGrid.Children.Cast<Rectangle>().Count(r => Grid.GetRow(r) == row && 
                (Grid.GetColumn(r) == col || Grid.GetColumn(r) == col - 1 || Grid.GetColumn(r) == col + 1));
                if (children == 0)
                {
                    Rectangle block = new Rectangle();
                    block.Fill = Brushes.Green;
                    block.Margin = new Thickness(1);
                    block.Tag = 3;
                    MainGrid.Children.Add(block);
                    Grid.SetColumnSpan(block, 2);
                    Grid.SetColumn(block, col);
                    Grid.SetRow(block, row);
                    list.Add(block);
                }
            }

            platform = new Rectangle();
            platform.Fill = Brushes.Orange;
            Grid.SetColumnSpan(platform, 8);
            MainGrid.Children.Add(platform);
            Grid.SetColumn(platform, 16);
            Grid.SetRow(platform, 39);

            ball = new Ellipse();
            ball.Fill = Brushes.Purple;
            MainGrid.Children.Add(ball);
            Grid.SetColumn(ball, new Random().Next(5, 35));
            Grid.SetRow(ball, 30);

            if (timer != null)
                timer.Stop();
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            CheckBlocksCollision();

            try
            {
                Grid.SetColumn(ball, Grid.GetColumn(ball) + dirX);
            }
            catch
            {
                dirX *= -1;
                Grid.SetColumn(ball, Grid.GetColumn(ball) + dirX);
            }
            try
            {
                Grid.SetRow(ball, Grid.GetRow(ball) + dirY);
            }
            catch
            {
                dirY *= -1;
                Grid.SetRow(ball, Grid.GetRow(ball) + dirY);
            }

            if (Grid.GetRow(ball) == 0)
                dirY *= -1;
            if (Grid.GetRow(ball) == 39)
            {
                StartGame();
                return;
            }
            if (Grid.GetColumn(ball) == 0)
                dirX *= -1;
            if (Grid.GetColumn(ball) == 39)
                dirX *= -1;
            if (Grid.GetRow(ball) == 38 && Grid.GetColumn(ball) > Grid.GetColumn(platform) && Grid.GetColumn(ball) < Grid.GetColumn(platform) + 8)
                dirY *= -1;
        }

        private void CheckBlocksCollision()
        {
            bool xChanged = true;
            foreach (var block in list)
            {
                int row = Grid.GetRow(block);
                int c1 = Grid.GetColumn(block);
                int c2 = c1 + 1;

                int ballRow = Grid.GetRow(ball);
                int ballCol = Grid.GetColumn(ball);

                if (dirX == -1 && dirY == -1)
                {
                    if (ballRow == row + 1 && ballCol == c2 + 1)
                    {
                        int k = rand.Next(0, 2);
                        if (k == 1)
                        {
                            dirX *= -1;
                            xChanged = true;
                        }
                        else
                        {
                            dirY *= -1;
                            xChanged = false;
                        }
                        HitBlock(block);
                        break;
                    }
                    else if (ballRow == row + 1 && ballCol == c2)
                    {
                        dirY *= -1;
                        xChanged = false;
                        HitBlock(block);
                        break;
                    }
                    else if (ballRow == row + 1 && ballCol == c1)
                    {
                        dirY *= -1;
                        xChanged = false;
                        HitBlock(block);
                        break;
                    }
                    else if (ballRow == row && ballCol == c2 + 1)
                    {
                        dirX *= -1;
                        xChanged = true;
                        HitBlock(block);
                        break;
                    }
                }
                else if (dirX == -1 && dirY == 1)
                {
                    if (ballRow == row - 1 && ballCol == c2 + 1)
                    {
                        int k = rand.Next(0, 2);
                        if (k == 1)
                        {
                            dirX *= -1;
                            xChanged = true;
                        }
                        else
                        {
                            dirY *= -1;
                            xChanged = false;
                        }
                        HitBlock(block);
                        break;
                    }
                    else if (ballRow == row - 1 && ballCol == c1)
                    {
                        dirY *= -1;
                        xChanged = false;
                        HitBlock(block);
                        break;
                    }
                    else if (ballRow == row - 1 && ballCol == c2)
                    {
                        dirY *= -1;
                        xChanged = false;
                        HitBlock(block);
                        break;
                    }
                    else if (ballRow == row && ballCol == c2 + 1)
                    {
                        dirX *= -1;
                        xChanged = true;
                        HitBlock(block);
                        break;
                    }
                }
                else if (dirX == 1 && dirY == -1)
                {
                    if (ballRow == row + 1 && ballCol == c1 - 1)
                    {
                        int k = rand.Next(0, 2);
                        if (k == 1)
                        {
                            dirX *= -1;
                            xChanged = true;
                        }
                        else
                        {
                            dirY *= -1;
                            xChanged = false;
                        }
                        HitBlock(block);
                        break;
                    }
                    else if (ballRow == row + 1 && ballCol == c1)
                    {
                        dirY *= -1;
                        xChanged = false;
                        HitBlock(block);
                        break;
                    }
                    else if (ballRow == row + 1 && ballCol == c2)
                    {
                        dirY *= -1;
                        xChanged = false;
                        HitBlock(block);
                        break;
                    }
                    else if (ballRow == row && ballCol == c1 - 1)
                    {
                        dirX *= -1;
                        xChanged = true;
                        HitBlock(block);
                        break;
                    }
                }
                else
                {
                    if (ballRow == row - 1 && ballCol == c1 - 1)
                    {
                        int k = rand.Next(0, 2);
                        if (k == 1)
                        {
                            dirX *= -1;
                            xChanged = true;
                        }
                        else
                        {
                            dirY *= -1;
                            xChanged = false;
                        }
                        HitBlock(block);
                        break;
                    }
                    else if (ballRow == row - 1 && ballCol == c1)
                    {
                        dirY *= -1;
                        xChanged = false;
                        HitBlock(block);
                        break;
                    }
                    else if (ballRow == row - 1 && ballCol == c2)
                    {
                        dirY *= -1;
                        xChanged = false;
                        HitBlock(block);
                        break;
                    }
                    else if (ballRow == row && ballCol == c1 - 1)
                    {
                        dirX *= -1;
                        xChanged = true;
                        HitBlock(block);
                        break;
                    }
                }
            }
            foreach(var block in list)
            {
                int nextCol = Grid.GetColumn(ball) + dirX;
                int nextRow = Grid.GetRow(ball) + dirY;
                var collision = MainGrid.Children.Cast<UIElement>().Where(c => c is Rectangle && Grid.GetRow(c) == nextRow && Grid.GetColumn(c) == nextCol).ToList();
                collision.AddRange(MainGrid.Children.Cast<UIElement>().Where(c => c is Rectangle && Grid.GetRow(c) == nextRow && Grid.GetColumn(c) + 1 == nextCol).ToList());
                if (collision.Count() != 0)
                {
                    if (xChanged)
                    {
                        dirY *= -1;
                    }
                    else
                    {
                        dirX *= -1;
                    }
                }
            }
        }

        private void HitBlock(Rectangle block)
        {
            switch (Convert.ToInt32(block.Tag))
            {
                case 1:
                    MainGrid.Children.Remove(block);
                    list.Remove(block);
                    break;
                case 2:
                    block.Fill = Brushes.Red;
                    block.Tag = 1;
                    break;
                case 3:
                    block.Fill = Brushes.Orange;
                    block.Tag = 2;
                    break;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                if (Grid.GetColumn(platform) > 1)
                    Grid.SetColumn(platform, Grid.GetColumn(platform) - 2);
            }
            else if (e.Key == Key.D)
            {
                if (Grid.GetColumn(platform) < 31)
                    Grid.SetColumn(platform, Grid.GetColumn(platform) + 2);
            }
            else if (e.Key == Key.Space)
            {
                if (timer.IsEnabled) timer.Stop();
                else timer.Start();
            }
        }
    }
}