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
        playerScore leaderboard;
        public MainWindow()
        {
            InitializeComponent();
            updateArray();
            Closing += MainWindow_Closing;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            gm.Shift(e.Key);
            updateArray();
        }

        private void updateArray()
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
            gameGrid.Visibility = Visibility.Collapsed;
            menuGrid.Visibility = Visibility.Visible;
            gm.saveActualScore();
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            menuGrid.Visibility = Visibility.Collapsed;
            gameGrid.Visibility = Visibility.Visible;
            continueButton.IsEnabled = true;
            gm.setScoreToZero();
            gm.newGame();
            updateArray();
        }
        
        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            menuGrid.Visibility = Visibility.Collapsed;
            gameGrid.Visibility = Visibility.Visible;
            gm.loadActualScore();
            updateArray();
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
                nameGrid.Visibility = Visibility.Collapsed;
                menuGrid.Visibility = Visibility.Visible;
                gm.nickname = insertName.Text;
            }
            continueButton.IsEnabled = gm.isPlayerInList();
        }

        private void exitStatsButton_Click(object sender, RoutedEventArgs e)
        {
            statisticsGrid.Visibility = Visibility.Collapsed;
            menuGrid.Visibility = Visibility.Visible;
        }

        private void statisticsButton_Click(object sender, RoutedEventArgs e)
        {
            menuGrid.Visibility = Visibility.Collapsed;
            statisticsGrid.Visibility = Visibility.Visible;
            gm.writeLeadeboardToLabel(labLeaderboard);
           
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            leaderboard = new playerScore { Name = gm.nickname, Score = gm.score };
            gm.saveScore(leaderboard);
            gm.saveActualScore();
        }

        private void endButton_Click(object sender, RoutedEventArgs e)
        {
            leaderboard = new playerScore { Name = gm.nickname, Score = gm.score };
            gm.saveScore(leaderboard);
            gm.saveActualScore();
            Application.Current.Shutdown();
        }
    }      
}
