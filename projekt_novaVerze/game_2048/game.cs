using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Reflection.Emit;

namespace game_2048
{
    public class game
    {
        int[,] array;
        int size = 4;
        Random rnd;
        public int score = 0;
        private int bestScore = 0;
        private string bestPlayer = "";
        public string nickname;

        public game()
        {
            array = new int[size, size];
            rnd = new Random();
        }

        // Nastaveni barvy podle hodnoty pole
        public Color GetColor(int x, int y)
        {
            switch (array[x, y])
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

        // Vraci hodnotu policka a priradi do labelu
        public string getContent(int x, int y)
        {
            if (array[x, y] == 0)
                return " ";
            return array[x, y].ToString();
        }

        public int getScore()
        {
            return score;
        }

        public void setScoreToZero()
        {
            score = 0;
        }

        // Zapise nejlepsi skore do souboru a pote vrati
        public int getBestScore()
        {
            bestPlayer = nickname;
            if (score > bestScore)
            {
                bestScore = score;
                bestPlayer = nickname;
                using (StreamWriter sw = new StreamWriter("bestscore.txt"))
                {
                    sw.WriteLine(bestPlayer);
                    sw.WriteLine(bestScore);
                }
            }
            if (File.Exists("bestscore.txt"))
            {
                using (StreamReader sr = new StreamReader("bestscore.txt"))
                {
                    bestPlayer = sr.ReadLine();
                    bestScore = int.Parse(sr.ReadLine());
                }
            }
            return bestScore;
        }

        // Best player se prepisuje ve funkci getBestScore
        public string getBestPlayer()
        {
            return bestPlayer;
        }

        public string getNickname()
        {
            return nickname;
        }

        // Zavola se pri kliku na tlacitko new game a vynuluje cele pole a vygeneruje 2 nove random policka
        public void newGame()
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    array[x, y] = 0;
            RandomAdd();
            RandomAdd();
        }

        // Reaguje na stisk klavesy a porovnava jestli doslo ke zmene pole po stisku klavesy, pokud ano, vygeneruje nove random policko
        public void Shift(Key k)
        {
            int[,] arr = new int[size, size];
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    arr[x, y] = array[x, y];
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
            if (!compare(array, arr))
                RandomAdd();
        }

        // Funkce ktera porovnava jestli po stisku klavesy doslo ke zmene pole
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
                    if (array[x, y] == 0)
                    {
                        for (int i = y - 1; i >= 0; i--)
                            if (array[x, i] != 0)
                            {
                                array[x, y] = array[x, i];
                                array[x, i] = 0;
                                break;
                            }
                    }
                    else
                    {
                        for (int i = y - 1; i >= 0; i--)
                            if (array[x, i] == array[x, y])
                            {
                                array[x, y] += array[x, i];
                                score += array[x, i];
                                array[x, i] = 0;
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
                    if (array[y, x] == 0)
                    {
                        for (int i = y - 1; i >= 0; i--)
                            if (array[i, x] != 0)
                            {
                                array[y, x] = array[i, x];
                                array[i, x] = 0;
                                break;
                            }
                    }
                    else
                    {
                        for (int i = y - 1; i >= 0; i--)
                            if (array[i, x] == array[y, x])
                            {
                                array[y, x] += array[i, x];
                                score += array[i, x];
                                array[i, x] = 0;
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
                    if (array[y, x] == 0)
                    {
                        for (int i = y + 1; i < size; i++)
                            if (array[i, x] != 0)
                            {
                                array[y, x] = array[i, x];
                                array[i, x] = 0;
                                break;
                            }
                    }
                    else
                    {
                        for (int i = y + 1; i < size; i++)
                            if (array[i, x] == array[y, x])
                            {
                                array[y, x] += array[i, x];
                                score += array[i, x];
                                array[i, x] = 0;
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
                    if (array[x, y] == 0)
                    {
                        for (int i = y + 1; i < size; i++)
                            if (array[x, i] != 0)
                            {
                                array[x, y] = array[x, i];
                                array[x, i] = 0;
                                break;
                            }
                    }
                    else
                    {
                        for (int i = y + 1; i < size; i++)
                            if (array[x, i] == array[x, y])
                            {
                                array[x, y] += array[x, i];
                                score += array[x, i];
                                array[x, i] = 0;
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
                    if (array[x, y] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //Prida nove policko na volne misto
        public void RandomAdd()
        {
            int x, y;
            if (Finish())
            {
                MessageBox.Show("GAME OVER!");
            }
            do
            {
                x = rnd.Next(size);
                y = rnd.Next(size);
            }
            while (array[x, y] != 0);
            {
                array[x, y] = 2;
            }
        }

        // Ulozi aktualni hraci uspech do souboru pri ukonceni aplikace
        public void saveActualScore()
        {
            // Nacteni aktualniho hraciho uspechu do listu, pouziva 2 listy, jeden na jmena a druhy na cely radek
            List<string> players = new List<string>();
            List<string> scores = new List<string>();
            if (File.Exists("actualScore.txt"))
            {
                using (StreamReader sr = new StreamReader("actualScore.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length > 2) 
                        {
                            players.Add(parts[0]);
                            scores.Add(line);
                        }
                    }
                }
            }

            // Nacteni noveho hraciho uspechu
            string scoreLine = nickname;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    scoreLine += "," + array[x, y];
                }
            }

            // Zapis noveho hraciho uspechu do souboru, pokud tam byl predtim hrac se stejnym jmenem, tak ho prepise
            bool scoreWritten = false;
            using (StreamWriter sw = new StreamWriter("actualScore.txt"))
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i] == nickname)
                    {
                        scores[i] = scoreLine;
                        scoreWritten = true;
                        break;
                    }
                }

                if (!scoreWritten)
                {
                    players.Add(nickname);
                    scores.Add(scoreLine);
                }

                for (int i = 0; i < players.Count; i++)
                {
                    sw.WriteLine(scores[i]);
                }
            }
        }

        // Funkce pro nacteni hraciho uspechu z minule hry, zavola se stiskem na tlacitko continue in game
        public void loadActualScore()
        {
            if (File.Exists("actualScore.txt"))
            {
                List<string> players = new List<string>();
                using (StreamReader sr = new StreamReader("actualScore.txt"))
                {
                    string line;
                    // Nacte do listu vsechny hrace ulozene v souboru
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        players.Add(parts[0]);
                    }
                    // Pokud najde shodu s nejakym hracem v listu, tak nacte jeho hraci uspech tak, jak skoncil
                    foreach (string s in players)
                    {
                        if (s == nickname)
                        {
                            sr.BaseStream.Position = 0;
                            int i = 1;
                            int index = players.IndexOf(nickname);
                            for (int z = 0; z <= index; z++)
                            {
                                line = sr.ReadLine();
                            }
                            string[] parts = line.Split(',');
                            for (int x = 0; x < size; x++)
                            {
                                for (int y = 0; y < size; y++)
                                {
                                    int j = int.Parse(parts[i]);
                                    array[x, y] = j;
                                    i++;
                                }
                            }

                        }
                    }
                }
            }
        }

        // Funkce ktera zjisti, jestli uz je v actualScore, dany hrac. Pouziva se pro zakazani nebo povoleni tlacitka continue in game button
        public bool isPlayerInList()
        {
            if (File.Exists("actualScore.txt"))
            {
                List<string> players = new List<string>();
                using (StreamReader sr = new StreamReader("actualScore.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        players.Add(parts[0]);
                    }
                    foreach (string s in players)
                    {
                        if (s == nickname)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        // Nacte leaderboard ze souboru
        public List<playerScore> loadLeaderboard()
        {
            List<playerScore> leaderboard = new List<playerScore>();
            if (File.Exists("leaderboard.txt"))
            {
                using (StreamReader sr = new StreamReader("leaderboard.txt"))
                {
                    int rank = 1;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split('.', ':', ';', ' ');
                        leaderboard.Add(new playerScore { Rank = rank, Name = parts[4], Score = int.Parse(parts[8]) });
                        rank++;
                    }
                }
            }
            return leaderboard;
        }

        // Ulozi do souboru 10 nejlepsich hracu podle skore 
        public void saveScore(playerScore score)
        {
            List<playerScore> leaderboard = loadLeaderboard();
            bool playerExists = false;
            // Zkontroluje jestli neni v leaderboardu hrac se stejnym jmenem, pokud ano a ma mensi skore, prepise ho
            foreach (playerScore existingScore in leaderboard)
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
            // Zapis do souboru
            using (StreamWriter sw = new StreamWriter("leaderboard.txt"))
            {
                foreach (playerScore s in leaderboard)
                {
                    sw.WriteLine($"{s.Rank}. PLAYER: {s.Name}; SCORE: {s.Score}");
                }
            }
        } 

        // vypise do labelu leaderboard ze souboru
        public void writeLeadeboardToLabel(System.Windows.Controls.Label labLeaderboard)
        {
            playerScore leaderboard;
            leaderboard = new playerScore { Name = nickname, Score = score };
            saveScore(leaderboard);
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
}
