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
            addRow(0, numbersRow1, Color.FromArgb(255, 192, 192));
            addRow(1, numbersRow2, Color.FromArgb(255, 255, 192));
            addRow(2, numbersRow3, Color.FromArgb(192, 255, 192), true);
            addRow(3, numbersRow4, Color.FromArgb(192, 192, 255), true);

            // Lock Icon displayed at the end of a row.
            LockIcons = new[] { row0LockIcon, row1LockIcon, row2LockIcon, row3LockIcon };

            rowsDisabled = 0;
            totalScoreColor = new[] { 0, 0, 0, 0 };
        }

        List<Button>[] Buttons;
        PictureBox[] LockIcons;
        int[] totalScoreColor;
        int rowsDisabled, badThrows;

        // Makes a new row with buttons, adds events to those buttons and adds them to the Buttons list.
        private void addRow(int row, TableLayoutPanel RowControl, Color buttonBackColor, bool reversed = false)
        {
            for (int j = 2; j < 13; j++)
            {
                Button button = new Button();
                button.Text = (reversed ? 14 - j : j).ToString();
                button.Dock = DockStyle.Fill;
                button.Click += DisableLowerNumbers;
                button.Click += UpdateLockIcon;
                button.Click += updateTotalScoreColor;
                button.Click += checkIfGameIsOver;
                button.Tag = row;
                button.BackColor = buttonBackColor;
                Buttons[row].Add(button);
                RowControl.Controls.Add(button, j - 2, 0);
            }
        }

        // Disable all numbers left of the pressed button.
        private void DisableLowerNumbers(object sender, EventArgs e)
        {
            if (!(sender is Button button)) return;

            // Get current row.
            int btnRowIndex = (int)((Button)sender).Tag;
 
            foreach (Button SingleButton in Buttons[btnRowIndex])
            {
                SingleButton.Enabled = false;
                if (SingleButton == button)
                {
                    break;
                }
            }
        }

        // Check if most far right button is disabled, and if so update the lock to display a locked status.
        private void UpdateLockIcon(object sender, EventArgs e)
        {
            // Get current row.
            int BtnRowIndex = (int)((Button)sender).Tag;
            if (Buttons[BtnRowIndex][10].Enabled == false)
            {
                LockIcons[BtnRowIndex].Image = Properties.Resources.lockClosed;
            }
        }

        private void updateTotalScoreColor(object sender, EventArgs e)
        {
            if (!(sender is Button button)) return;
            // Get current row.
            int BtnRowIndex = (int)((Button)sender).Tag;
            totalScoreColor[BtnRowIndex]++;
        }

        private void checkIfGameIsOver(object sender, EventArgs e)
        {
            updateRowsDisabled();
            updateBadThrows();
            if (rowsDisabled >= 2 || badThrows >= 4)
            {
                totalScoreRedTxt.Text = getScore(totalScoreColor[0]).ToString();
                totalScoreYellowTxt.Text = getScore(totalScoreColor[1]).ToString();
                totalScoreGreenTxt.Text = getScore(totalScoreColor[2]).ToString();
                totalScoreBlueTxt.Text = getScore(totalScoreColor[3]).ToString();

                minusBadThrowsTxt.Text = (badThrows * 5).ToString();

                int totalScore = totalScoreColor.Sum() - badThrows * 5;
                totalScoreTxt.Text = totalScore.ToString();


                for (int i = 0; i < 4; i++)
                {
                    foreach (Button singleButton in Buttons[i])
                    {
                        singleButton.Enabled = false;
                    }
                }

            }
        }

        private void updateRowsDisabled()
        {
            rowsDisabled = 0;
            for (int i = 0; i < 4; i++)
            {
                if (Buttons[i][10].Enabled == false)
                {
                    rowsDisabled++;
                }
            }
        }

        private void updateBadThrows()
        {
            bool[] badThrowCheckboxes = { badThrowCheckbox1.Checked, badThrowCheckbox2.Checked, badThrowCheckbox3.Checked, badThrowCheckbox4.Checked };

            badThrows = 0;
            foreach (bool checkbox in badThrowCheckboxes)
            {
                if (checkbox)
                {
                    badThrows++;
                }
            }
        }

        // Return score based on the table values in the GUI.
        private int getScore(int crossesChecked)
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

