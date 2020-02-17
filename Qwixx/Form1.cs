using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qwixx
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Array of all 4 rows with buttons
            Buttons = new[] { new List<Button>(), new List<Button>(), new List<Button>(), new List<Button>() };

            // Add row with buttons and add events
            AddRow(0, numbersRow1, Color.FromArgb(255, 192, 192));
            AddRow(1, numbersRow2, Color.FromArgb(255, 255, 192));
            AddRow(2, numbersRow3, Color.FromArgb(192, 255, 192), true);
            AddRow(3, numbersRow4, Color.FromArgb(192, 192, 255), true);

            // Lock Icon displayed at the end of a row.
            LockIcons = new[] { Row0LockIcon, Row1LockIcon, Row2LockIcon, Row3LockIcon };

            // Set default states
            TotalScoreColor = new[] { 0, 0, 0, 0 };
        }

        List<Button>[] Buttons;
        PictureBox[] LockIcons;
        int[] TotalScoreColor;
        int RowsDisabled, BadThrows;

        // Makes a new row with buttons, adds events to those buttons and adds them to the Buttons list.
        private void AddRow(int row, TableLayoutPanel RowControl, Color buttonBackColor, bool reversed = false)
        {
            // Makes button and add properties + additional operations.
            for (int j = 2; j < 13; j++)
            {
                Button button = new Button();
                button.Text = (reversed ? 14 - j : j).ToString();
                button.Dock = DockStyle.Fill;
                button.Click += UpdateTotalScoreColor;
                button.Click += UpdateLockIcon;
                button.Click += CheckIfGameIsOver;
                button.Tag = row;
                button.BackColor = buttonBackColor;
                // Add button to the button list of the current row.
                Buttons[row].Add(button);
                // Add button to the parameter of TableLayoutPanel
                RowControl.Controls.Add(button, j - 2, 0);
            }
        }

        // Disable all numbers left of the pressed button.
        private void DisableLowerNumbers(object sender, EventArgs e)
        {
            // If sender is button, initiliaze button instance.
            if (!(sender is Button button)) return;

            // Get current row.
            int btnRowIndex = (int)((Button)sender).Tag;
 
            // Loops over each previous button till it hits current button, then stops.
            foreach (Button singleButton in Buttons[btnRowIndex])
            {
                singleButton.Enabled = false;
                if (singleButton == button)
                {
                    break;
                }
            }
        }

        // Check if most far right button is disabled, and if so update the lock to display a locked status.
        private void UpdateLockIcon(object sender, EventArgs e)
        {
            // Get current row.
            int btnRowIndex = (int)((Button)sender).Tag;
            if (Buttons[btnRowIndex][10].Enabled == false)
            {
                LockIcons[btnRowIndex].Image = Properties.Resources.lockClosed;
            }
        }

        private void UpdateTotalScoreColor(object sender, EventArgs e)
        {
            if (!(sender is Button button)) return;
            // Get current row.
            int btnRowIndex = (int)((Button)sender).Tag;
            
            // If the button pressed is the last button in the row and the buttons crossed is not yet 5, show error.
            if (button == Buttons[btnRowIndex][10] && TotalScoreColor[btnRowIndex] < 5)    
            {
                MessageBox.Show("A minimum of 5 previous buttons have to be checked in order for the row to be closed.");
            } else
            {
                // Continue normal operations.
                DisableLowerNumbers(sender, e);
                // Add extra point for current row if the last button is disabled and thus the row is locked.
                if (Buttons[btnRowIndex][10].Enabled == false)
                {
                    TotalScoreColor[btnRowIndex] += 2;
                } else
                {
                    TotalScoreColor[btnRowIndex]++;
                }
            }
        }

        // Checks if the game is over and if true it changes certain properties.
        private void CheckIfGameIsOver(object sender, EventArgs e)
        {
            UpdateRowsDisabled();
            UpdateBadThrows();
            if (RowsDisabled >= 2 || BadThrows >= 4)
            {
                // Loop over final amount of "crosses" set per row and convert them to their point value.
                for (int i = 0; i < TotalScoreColor.Count(); i++)
                {
                    TotalScoreColor[i] = GetScore(TotalScoreColor[i]);
                }

                // Display total score for each color.
                TotalScoreRedTxt.Text = TotalScoreColor[0].ToString();
                TotalScoreYellowTxt.Text = TotalScoreColor[1].ToString();
                TotalScoreGreenTxt.Text = TotalScoreColor[2].ToString();
                TotalScoreBlueTxt.Text = TotalScoreColor[3].ToString();

                // Display points which will be subtracted because of BadThrows.
                MinusBadThrowsTxt.Text = (BadThrows * 5).ToString();

                // Calculate and display total sum.
                int totalScore = TotalScoreColor.Sum() - BadThrows * 5;
                TotalScoreTxt.Text = totalScore.ToString();

                // Disable all buttons.
                for (int i = 0; i < 4; i++)
                {
                    foreach (Button singleButton in Buttons[i])
                    {
                        singleButton.Enabled = false;
                    }
                }

            }
        }

        // Updates the RowsDisabled variable
        private void UpdateRowsDisabled()
        {
            // Reset bad throws to avoid concatenation of previous set values.
            RowsDisabled = 0;
            // Loop over last button in each row and check if it's Enabled. If not, add 1 to RowsDisabled variable.
            for (int i = 0; i < 4; i++)
            {
                if (Buttons[i][10].Enabled == false)
                {
                    RowsDisabled++;
                }
            }
        }

        // Updates the BadThrows variable
        private void UpdateBadThrows()
        {
            // Select all checboxes and store the boolean of whether it's checked.
            bool[] isBadThrowChecked = { BadThrowCheckbox1.Checked, BadThrowCheckbox2.Checked, BadThrowCheckbox3.Checked, BadThrowCheckbox4.Checked };

            // Reset bad throws to avoid concatenation of previous set values.
            BadThrows = 0;
            // Loop over each checkbox and check it's state, if checked add 1 to BadThrows variable.
            foreach (bool isChecked in isBadThrowChecked)
            {
                if (isChecked)
                {
                    BadThrows++;
                }
            }
        }

        // Return score based on the table values in the GUI.
        private int GetScore(int crossesChecked)
        {
            int score = 0;
            switch (crossesChecked)
            {
                case 1:
                    score = 1;
                    break;
                case 2:
                    score = 3;
                    break;
                case 3:
                    score = 6;
                    break;
                case 4:
                    score = 10;
                    break;
                case 5:
                    score = 15;
                    break;
                case 6:
                    score = 21;
                    break;
                case 7:
                    score = 28;
                    break;
                case 8:
                    score = 36;
                    break;
                case 9:
                    score = 45;
                    break;
                case 10:
                    score = 55;
                    break;
                case 11:
                    score = 66;
                    break;
                case 12:
                    score = 78;
                    break;
            }
            return score;
        }
    }
}

