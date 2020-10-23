using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingPairsGame
{
    public partial class matchingPairsForm : Form
    {
        //When using the webdings font, these letters are nice patterns, so this is a list to store them
        //I also created a List to hold the patterns I will use in that specific game
        List<string> goodIconPatterns = new List<string>() { "i", "j", ",", "w", "e", "t", "o",
            "p", "f", "h", "j", "k", "l", "!", "%", "T", "Y", "H", "J", "N", "M", "¨", "S",
            "¡", "©", "°", "²", "µ", "·", "º", "¾", "Ä", "Í", "Ñ", "ü" };
        List<string> iconPatternsBeingUsed = new List<string>();

        Label firstIconClicked = null;
        Label secondIconClicked = null;

        int matchedPairs = 0;
        Random random = new Random();
        float timeFromTheStart = 0;
        int pairsTriedInThisGame = 0;

        public matchingPairsForm()
        {
            InitializeComponent();
            ChoosePatterns();
            AssignPatternsToSquares();

        }

        /// <summary>
        /// Assigns the pattern from the patterns being used in this game into every square of the Panel
        /// </summary>
        private void AssignPatternsToSquares()
        {
            foreach (Control control in squaresPanel.Controls)
            {
                Label iconsLabel = control as Label;
                if (iconsLabel != null)
                {
                    int randomNumber = random.Next(0, iconPatternsBeingUsed.Count);
                    iconsLabel.Text = iconPatternsBeingUsed[randomNumber];

                    iconsLabel.ForeColor = iconsLabel.BackColor;

                    iconPatternsBeingUsed.RemoveAt(randomNumber);
                }
            }
        }

        /// <summary>
        /// This method chooses some patterns from the list of good patterns to be used on that game
        /// </summary>
        private void ChoosePatterns()
        {
            int ammountOfPatterns = 0;
            while (ammountOfPatterns < 16)
            {
                var randomPattern = goodIconPatterns[random.Next(0, goodIconPatterns.Count)];
                var foundPattern = false;
                foreach (string pattern in iconPatternsBeingUsed)
                {
                    if (pattern == randomPattern)
                    {
                        foundPattern = true;
                        break;
                    }
                }
                if (!foundPattern)
                {
                    //Adds the random pattern into the list that will be used in this game. Adds two to show how many pairs have entered
                    iconPatternsBeingUsed.Add(randomPattern);
                    iconPatternsBeingUsed.Add(randomPattern);
                    ammountOfPatterns += 2;
                }
            }
        }

        private void icon_Click(object sender, EventArgs e)
        {
            Label clickedIcon = sender as Label;
            if (clickedIcon != null)
            {
                if (gameTime.Enabled) return;
                if (clickedIcon.ForeColor == Color.Black || clickedIcon.ForeColor == Color.Indigo) return;

                if (firstIconClicked == null)
                {
                    firstIconClicked = clickedIcon;
                    firstIconClicked.ForeColor = Color.Black;

                    return;
                }
                else
                {
                    pairsTriedInThisGame++;
                    secondIconClicked = clickedIcon;
                    secondIconClicked.ForeColor = Color.Black;

                    if(secondIconClicked.Text == firstIconClicked.Text)
                    {
                        matchedPairs++;
                        firstIconClicked.ForeColor = Color.Indigo;
                        secondIconClicked.ForeColor = Color.Indigo;
                        firstIconClicked = null;
                        secondIconClicked = null;
                        if(matchedPairs == 8)
                        {
                            MessageBox.Show("Congratulations, you won!!\n" +
                                "You took " + timeFromTheStart/100f + " seconds!\n" +
                                "You tried " + pairsTriedInThisGame + " times");
                            RestartGame();
                        }
                        return;
                    } 
                    gameTime.Start();
                }
                        
            }
        }

        /// <summary>
        /// Sets all the variables to default and assigns different patterns to the game
        /// </summary>
        private void RestartGame()
        {
            ChoosePatterns();
            AssignPatternsToSquares();
            timeFromTheStart = 0;
            pairsTriedInThisGame = 0;
            matchedPairs = 0;
        }

        private void gameTime_Tick(object sender, EventArgs e)
        {
            gameTime.Stop();
            firstIconClicked.ForeColor = firstIconClicked.BackColor;
            secondIconClicked.ForeColor = secondIconClicked.BackColor;
            firstIconClicked = null;
            secondIconClicked = null;
        }

        private void recordTime_Tick(object sender, EventArgs e)
        {
            timeFromTheStart++;
        }
    }
}
