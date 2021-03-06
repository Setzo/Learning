﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

/************************************
 *									*
 * Autorzy:							*
 *		Aleksandra Matuszewska		*
 *		Wojciech Pruszak			*
 *									*
 ************************************/

namespace Checkers
{
	/*
	 * Klasa odpowiadająca za całą interakcje użytkownika
	 * z menu.
	 */
	public partial class Form1 : Form
	{
		/*
		 * Ścieżki do obrazków pionków / damek.
		*/
		public const string BLACK_PAWN = "C:\\Users\\Setzo\\Documents\\Visual Studio 2013\\Projects\\Checkers\\blackPawn.png";
		public const string BLACK_QUEEN = "C:\\Users\\Setzo\\Documents\\Visual Studio 2013\\Projects\\Checkers\\blackQueen.png";
		public const string RED_PAWN = "C:\\Users\\Setzo\\Documents\\Visual Studio 2013\\Projects\\Checkers\\redPawn.png";
		public const string RED_QUEEN = "C:\\Users\\Setzo\\Documents\\Visual Studio 2013\\Projects\\Checkers\\redQueen.png";

		/*
		 * Tablica zawierająca wszystkie nasze guziki.
		 */
		private Button[,] bTab;

		private bool xCheat = false;
		private bool oCheat = false;

		/*
		 * Plansza.
		 */
		private Board board;
		
		/*
		 * Konstruktor, inicjalizujący planszę.
		 */
		public Form1()
		{
			InitializeComponent();

			this.bTab = new Button[8, 8];

			List<Control> list = new List<Control>();

			listControls(this, list);

			/*
			 * Wczytywanie wszystkich guzików do tablicy.
			 */
			foreach (Control control in list)
			{
				if (control.GetType() == typeof(Button))
				{
					Button b = (Button)control;

					if (b.Name.StartsWith("button"))
					{
						int y = (int)char.GetNumericValue(b.Name[b.Name.Length - 1]);
						int x = (int)char.GetNumericValue(b.Name[b.Name.Length - 2]);

						this.bTab[x, y] = b;
					}
				}
			}

			this.label.Font = new Font(this.label.Font.FontFamily, 10);

			this.startup();
		}

		/*
		 * Funkcja wczytująca wszystkie pionki na ich startowe pozycje,
		 * oraz wymazująca wszystko co było na planszy przed rozpoczęciem
		 * działania tej metody.
		 */
		private void startup()
		{
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					this.bTab[i, j].Text = "";
					this.bTab[i, j].Font = new Font(this.bTab[i, j].Font.FontFamily, 100);
					this.bTab[i, j].Image = default(Image);
				}
			}

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (j < 3)
					{
						if ((j % 2 == 0 && i % 2 != 0) || (j % 2 != 0 && i % 2 == 0))
						{
							if (!this.xCheat)
							{
								this.bTab[i, j].Text = "X";
								this.bTab[i, j].Image = Image.FromFile(Form1.BLACK_PAWN);
								//this.bTab[i, j].Font = new Font(this.bTab[i, j].Font.FontFamily, 14);
							}
							else
							{
								this.bTab[i, j].Text = "XX";
								this.bTab[i, j].Image = Image.FromFile(Form1.BLACK_QUEEN);
							}
						}
					}

					if (j > 4)
					{
						if ((j % 2 != 0 && i % 2 == 0) || (j % 2 == 0 && i % 2 != 0))
						{
							if (!this.oCheat)
							{
								this.bTab[i, j].Text = "O";
								this.bTab[i, j].Image = Image.FromFile(Form1.RED_PAWN);
								//this.bTab[i, j].Font = new Font(this.bTab[i, j].Font.FontFamily, 14);
							}
							else
							{
								this.bTab[i, j].Text = "OO";
								this.bTab[i, j].Image = Image.FromFile(Form1.RED_QUEEN);
							}
						}
					}
				}
			}

			board = new Board(ref this.bTab, ref this.label);
		}

		/*
		 * Metoda zapisująca w liście wszystkie kontrolki,
		 * w tym nasze guziki.
		 */
		private void listControls(Control c, List<Control> list)
		{
			foreach (Control control in c.Controls)
			{
				list.Add(control);

				if (control.GetType() == typeof(Panel))
				{
					listControls(control, list);
				}
			}
		}

		/*
		 * Metoda wczytująca stan gry z pliku tekstowego wybranego przez użytkownika.
		 * W przypadku, gdy nie może otworzyć pliku, lub plik jest uszkodzony
		 * wyda ona komunikat o błędzie.
		 */
		private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FileStream myStream = null;
			OpenFileDialog theDialog = new OpenFileDialog();
			theDialog.Title = "Load Game";
			theDialog.Filter = "Text files (*.txt)|*.txt|All files|*.*";

			if (theDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					for (int i = 0; i < 8; i++)
					{
						for (int j = 0; j < 8; j++)
						{
							this.bTab[i, j].Text = "";
							this.bTab[i, j].Image = default(Image);
						}
					}

					if ((myStream = (FileStream)theDialog.OpenFile()) != null)
					{
						StreamReader sr = new StreamReader(myStream);

						for (int i = 0; i < 8; i++)
						{
							String c = sr.ReadLine();

							for (int j = 0; j < c.Length; j++)
							{
								int x = (int)char.GetNumericValue(c[j]);
								j++;
								j++;

								int y = (int)char.GetNumericValue(c[j]);
								j++;
								j++;

								String sign = "";

								if (!c[j + 1].Equals(' '))
								{
									sign = sign + c[j] + c[j + 1];
									j++;
								}
								else
								{
									sign = sign + c[j];
								}
								j++;

								if (sign.Equals("E"))
								{
									sign = "";
								}
								this.bTab[x, y].Text = sign;

								if (sign.Equals("X"))
								{
									this.bTab[x, y].Image = Image.FromFile(Form1.BLACK_PAWN);
								}
								else if (sign.Equals("O"))
								{
									this.bTab[x, y].Image = Image.FromFile(Form1.RED_PAWN);
								}
								else if (sign.Equals("XX"))
								{
									this.bTab[x, y].Image = Image.FromFile(Form1.BLACK_QUEEN);
								}
								else if (sign.Equals("OO"))
								{
									this.bTab[x, y].Image = Image.FromFile(Form1.RED_QUEEN);
								}
							}
						}

						String cx = sr.ReadLine();

						if (cx[0].ToString().Equals("t"))
						{
							this.board.setTurn(true);
						}

						else
						{
							this.board.setTurn(false);
						}

						for (int i = 0; i < 8; i++)
						{
							for (int j = 0; j < 8; j++)
							{
								if (this.bTab[i, j].Text.Equals("XX") || this.bTab[i, j].Text.Equals("OO"))
								{
									this.bTab[i, j].Font = new Font(this.bTab[i, j].Font.FontFamily, 100);
								}
							}
						}

						this.board.highlight();
						this.board.updateLabel();

						sr.Close();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk.");
					this.startup();
				}
			}
		}

		/*
		 * Metoda wykonująca sie za każdym naciśnięciem przycisku na planszy.
		 * Sprawdza ona czy poprzednia gra się nie skończyła, i czy nie powinna
		 * zacząć nowej. Poza tym podświetla ona pionki, których ruch jest wymuszony
		 * (jeżeli mają bicie) i aktualizuje stan etykiety z aktualną turą gracza.
		 * 
		 * Więcej informacji o działaniu tej metody znajdziemy w klasie Board.
		 */
		private void button62_Click(object sender, EventArgs e)
		{
			this.board.move((Button)sender);

			this.board.updateLabel();
			this.board.highlight();

			if (this.board.win())
			{
				this.startup();
			}
		}

		/*
		 * Metoda rozpoczynająca nową grę.
		 */
		private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			startup();
		}

		/* 
		 * Metoda zamykająca program.
		 */
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.Application.Exit();
		}

		/*
		 * Metoda zapisująca aktualny stan gry do pliku tekstowego
		 * wybranego przez użytkownika.
		 */
		private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FileStream myStream;

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Text files (*.txt)|*.txt";
			saveFileDialog.Title = "Save Game";

			saveFileDialog.FilterIndex = 2;
			saveFileDialog.RestoreDirectory = true;

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				if ((myStream = (FileStream)saveFileDialog.OpenFile()) != null)
				{
					StreamWriter sw = new StreamWriter(myStream);

					for (int i = 0; i < 8; i++)
					{
						for (int j = 0; j < 8; j++)
						{
							String c = "";
							c = c + i.ToString() + " " + j.ToString() + " ";

							if (this.bTab[i, j].Text.Equals(""))
							{
								c = c + "E ";
							}
							else
							{
								c = c + this.bTab[i, j].Text + " ";
							}

							sw.Write(c);
						}
						sw.Write("\n");
					}

					if (board.getTurn())
					{
						sw.Write("t");
					}
					else
					{
						sw.Write("f");
					}

					sw.Close();
				}
			}
		}

		/*
		 * Metoda otwierająca zasady do gry w warcaby.
		 */
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.kurnik.pl/warcaby/zasady.phtml");
		}
		
		/*
		 * Metoda wyświetlająca autorów programu.
		 */
		private void authorsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Created by :\n\n\tAleksandra Matuszewska\n\tWojciech Pruszak");
		}

		/*
		 * Ustawienia cheat'ów dla gracza czerwonego.
		 */
		private void bluePlayerCheatToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.oCheat = !this.oCheat;
			this.startup();
		}

		/*
		 * Ustawienia cheat'ów dla gracza czarnego.
		 */
		private void blackPlayerCheatToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.xCheat = !this.xCheat;
			this.startup();
		}
	}
}
