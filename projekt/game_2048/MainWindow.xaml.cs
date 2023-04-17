using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace game_2048
{
    public partial class MainWindow : Window
    {
        game gm = new game();
        PlayerScore leaderboard;
        public MainWindow()
        {
            InitializeComponent();
            update();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            gm.Shift(e.Key);
            update();
        }
        private void update()
        {
            bord00.Background = new SolidColorBrush(gm.GetColor(0, 0));
            bord01.Background = new SolidColorBrush(gm.GetColor(0, 1));
            bord02.Background = new SolidColorBrush(gm.GetColor(0, 2));
            bord03.Background = new SolidColorBrush(gm.GetColor(0, 3));

            bord10.Background = new SolidColorBrush(gm.GetColor(1, 0));
            bord11.Background = new SolidColorBrush(gm.GetColor(1, 1));
            bord12.Background = new SolidColorBrush(gm.GetColor(1, 2));
            bord13.Background = new SolidColorBrush(gm.GetColor(1, 3));

            bord20.Background = new SolidColorBrush(gm.GetColor(2, 0));
            bord21.Background = new SolidColorBrush(gm.GetColor(2, 1));
            bord22.Background = new SolidColorBrush(gm.GetColor(2, 2));
            bord23.Background = new SolidColorBrush(gm.GetColor(2, 3));

            bord30.Background = new SolidColorBrush(gm.GetColor(3, 0));
            bord31.Background = new SolidColorBrush(gm.GetColor(3, 1));
            bord32.Background = new SolidColorBrush(gm.GetColor(3, 2));
            bord33.Background = new SolidColorBrush(gm.GetColor(3, 3));

            lab00.Content = gm.getContent(0, 0);
            lab01.Content = gm.getContent(0, 1);
            lab02.Content = gm.getContent(0, 2);
            lab03.Content = gm.getContent(0, 3);

            lab10.Content = gm.getContent(1, 0);
            lab11.Content = gm.getContent(1, 1);
            lab12.Content = gm.getContent(1, 2);
            lab13.Content = gm.getContent(1, 3);

            lab20.Content = gm.getContent(2, 0);
            lab21.Content = gm.getContent(2, 1);
            lab22.Content = gm.getContent(2, 2);
            lab23.Content = gm.getContent(2, 3);

            lab30.Content = gm.getContent(3, 0);
            lab31.Content = gm.getContent(3, 1);
            lab32.Content = gm.getContent(3, 2);
            lab33.Content = gm.getContent(3, 3);

            labScore.Content = "Score: " + gm.getScore();
            labBestScore.Content = "Best score: " + gm.getBestScore();
            labNickname.Content = "Nickname: " + gm.getNickname();
            labBestPlayer.Content = "Best player: " + gm.getBestPlayer();

        }
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            grid2.Visibility = Visibility.Collapsed;
            grid1.Visibility = Visibility.Visible;           
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Collapsed;
            grid2.Visibility = Visibility.Visible;
            continueButton.Visibility = Visibility.Visible;
            gm.setScoreToZero();
            gm.newGame();
            update();
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Collapsed;
            grid2.Visibility = Visibility.Visible;
            update();
        }

        private void nameButton_Click(object sender, RoutedEventArgs e)
        {
            if (insertName.Text == "")
            {
                MessageBox.Show("Your name is empty!");
            }
            else if(insertName.Text.Contains(" "))
            {
                MessageBox.Show("Insert your name without gaps");
            }
            else
            {               
                grid0.Visibility = Visibility.Collapsed;
                grid1.Visibility = Visibility.Visible;
                gm.nickname = insertName.Text;
            }
        }

        private void exitButton2_Click(object sender, RoutedEventArgs e)
        {
            grid3.Visibility = Visibility.Collapsed;
            grid1.Visibility = Visibility.Visible;
        }

        private void statisticsButton_Click(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Collapsed;
            grid3.Visibility = Visibility.Visible;
            leaderboard = new PlayerScore { Name = gm.nickname, Score = gm.score };
            gm.SaveScore(leaderboard);
            if (File.Exists("leaderboard.txt"))
            {
                using (StreamReader sr = new StreamReader("leaderboard.txt"))
                {
                    string line;
                    labLeaderboard.Content = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        labLeaderboard.Content += line + Environment.NewLine;
                    }
                }
            }
        }
    }
    public class PlayerScore
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
    }

    class game
    {
        int[,] pole;
        Random rnd;
        int size = 4;
        public int score = 0;
        private int bestScore = 0;
        private string bestPlayer = "";
        public string nickname;


        public game()
        {
            rnd = new Random();
            pole = new int[size, size];
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    pole[x, y] = 0;
            RandomAdd();
            RandomAdd();
        }

        public Color GetColor(int x, int y)
        {
            switch (pole[x, y])
            {
                case 0:
                    return Colors.White;
                case 2:
                    return Colors.Aqua;
                case 4:
                    return Colors.SkyBlue;
                case 8:
                    return Colors.CornflowerBlue; 
                case 16:
                    return Colors.DeepSkyBlue;
                case 32:
                    return Colors.SteelBlue;
                case 64:
                    return Colors.Blue; 
                case 128:
                    return Colors.MediumBlue;
                case 256:
                    return Colors.DarkBlue;
                case 512:
                    return Colors.LightGreen;
                case 1024:
                    return Colors.SeaGreen;
                case 2048:
                    return Colors.Green;
                default:
                    return Colors.DarkGreen;
            }
        }

        public string getContent(int x, int y)
        {
            if (pole[x, y] == 0)
                return " ";
            return pole[x, y].ToString();
        }
        public int getScore()
        {
            return score;
        }
        public void setScoreToZero()
        {
            score = 0;
        }
        public int getBestScore()
        {
            bestPlayer = nickname;
            if (score > bestScore)
            {
                bestScore = score;
                bestPlayer = nickname;
                using (StreamWriter writer = new StreamWriter("bestscore.txt"))
                {
                    writer.WriteLine(bestPlayer);
                    writer.WriteLine(bestScore);
                }
            }
            if (File.Exists("bestscore.txt"))
            {
                using (StreamReader reader = new StreamReader("bestscore.txt"))
                {
                    bestPlayer = reader.ReadLine();
                    bestScore = int.Parse(reader.ReadLine());
                }
            }
            return bestScore;
        }
        public void loadLeaderboard()
        {
            
        }
        public List<PlayerScore> LoadLeaderboard()
        {
            List<PlayerScore> leaderboard = new List<PlayerScore>();
            if (File.Exists("leaderboard.txt"))
            {
                using (StreamReader sr = new StreamReader("leaderboard.txt"))
                {
                    int rank = 1;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split('.', ':', ';',' ');
                        leaderboard.Add(new PlayerScore { Rank = rank, Name = parts[4], Score = int.Parse(parts[8]) });
                        rank++;
                    }
                }
            }
            return leaderboard;
        }
        public void SaveScore(PlayerScore score)
        {
            List<PlayerScore> leaderboard = LoadLeaderboard();
            bool playerExists = false;
            foreach (PlayerScore existingScore in leaderboard)
            {
                if (existingScore.Name == score.Name)
                {
                    playerExists = true;
                    if (score.Score > existingScore.Score)
                    {
                        existingScore.Score = score.Score;
                    }
                    break;
                }
            }
            if (!playerExists)
            {
                leaderboard.Add(score);
            }
            leaderboard = leaderboard.OrderByDescending(s => s.Score).ToList();
            for (int i = 0; i < leaderboard.Count; i++)
            {
                leaderboard[i].Rank = i + 1;
            }
            leaderboard = leaderboard.Take(10).ToList();
            using (StreamWriter sw = new StreamWriter("leaderboard.txt"))
            {
                foreach (PlayerScore s in leaderboard)
                {
                    sw.WriteLine($"{s.Rank}. PLAYER: {s.Name}; SCORE: {s.Score}");
                }
            }
        }
        public string getBestPlayer()
        {
            return bestPlayer;
        }
        public string getNickname()
        {
            return nickname;
        }
       
        public void newGame()
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    pole[x, y] = 0;
            RandomAdd();
            RandomAdd();
        }

        public void Shift(Key k)
        {
            int[,] arr = new int[size, size];
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    arr[x, y] = pole[x, y];
            switch (k)
            {
                case Key.Left:
                    ShiftLeft();
                    break;
                case Key.Right:
                    ShiftRight();
                    break;
                case Key.Up:
                    ShiftUp();
                    break;
                case Key.Down:
                    ShiftDown();
                    break;
            }
            if (!compare(pole, arr))
                RandomAdd();
        }

        private bool compare(int[,] A, int[,] B)
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    if (A[x, y] != B[x, y])
                        return false;
            return true;
        }

        private void ShiftRight()
        {
            for (int x = size - 1; x >= 0; x--)
                for (int y = size - 1; y > 0; y--)
                {
                    if (pole[x, y] == 0)
                    {
                        for (int i = y - 1; i >= 0; i--)
                            if (pole[x, i] != 0)
                            {
                                pole[x, y] = pole[x, i];
                                pole[x, i] = 0;
                                break;
                            }
                    }
                    else
                    {
                        for (int i = y - 1; i >= 0; i--)
                            if (pole[x, i] == pole[x, y])
                            {
                                pole[x, y] += pole[x, i];
                                score += pole[x, y];
                                pole[x, i] = 0;
                                break;
                            }
                    }

                }
        }

        private void ShiftDown()
        {
            for (int x = size - 1; x >= 0; x--)
                for (int y = size - 1; y > 0; y--)
                {
                    if (pole[y, x] == 0)
                    {
                        for (int i = y - 1; i >= 0; i--)
                            if (pole[i, x] != 0)
                            {
                                pole[y, x] = pole[i, x];                              
                                pole[i, x] = 0;
                                break;
                            }
                    }
                    else
                    {
                        for (int i = y - 1; i >= 0; i--)
                            if (pole[i, x] == pole[y, x])
                            {
                                pole[y, x] += pole[i, x];
                                score += pole[x, y];
                                pole[i, x] = 0;
                                break;
                            }
                    }

                }
        }

        private void ShiftUp()
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size - 1; y++)
                {
                    if (pole[y, x] == 0)
                    {
                        for (int i = y + 1; i < size; i++)
                            if (pole[i, x] != 0)
                            {
                                pole[y, x] = pole[i, x];
                                pole[i, x] = 0;
                                break;
                            }
                    }
                    else
                    {
                        for (int i = y + 1; i < size; i++)
                            if (pole[i, x] == pole[y, x])
                            {
                                pole[y, x] += pole[i, x];
                                score += pole[x, y];
                                pole[i, x] = 0;
                                break;
                            }
                    }

                }
        }

        private void ShiftLeft()
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size - 1; y++)
                {
                    if (pole[x, y] == 0)
                    {
                        for (int i = y + 1; i < size; i++)
                            if (pole[x, i] != 0)
                            {
                                pole[x, y] = pole[x, i];
                                pole[x, i] = 0;
                                break;
                            }
                    }
                    else
                    {
                        for (int i = y + 1; i < size; i++)
                            if (pole[x, i] == pole[x, y])
                            {
                                pole[x, y] += pole[x, i];
                                score += pole[x, y];
                                pole[x, i] = 0;
                                break;
                            }
                    }

                }
        }

        private bool Finish()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (pole[x, y] == 0)
                    {
                        return false;
                    }                
                }
            }
            return true;
        }

        public void RandomAdd()
        {
            if (Finish())
            {
                MessageBox.Show("GAME OVER!");
            }

            int x, y;
            do
            {
                x = rnd.Next(size);
                y = rnd.Next(size);
            } 
            while (pole[x, y] != 0);
            {
                pole[x, y] = 2;
            }
        }
    }
}
