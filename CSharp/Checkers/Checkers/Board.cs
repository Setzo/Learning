﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Checkers
{
	public class Board
	{
		private const int ERROR = 100;
		private const int BTT_R = 0;
		private const int BTT_L = 1;
		private const int UPP_R = 2;
		private const int UPP_L = 3;

		private Button last;

		private Label label;

		private int[,] highlighted;

		private bool justMoved = false;
		private bool turn = false;
		private bool requireSwitching = true;

		private Button[,] bTab;

		public Board(ref Button[,] bTab, ref Label label)
		{
			this.bTab = bTab;
			this.label = label;
			this.last = this.bTab[0, 7];

			this.highlighted = new int[8, 8];

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					this.highlighted[i, j] = 0;

					if ((j % 2 == 0 && i % 2 != 0) || (j % 2 != 0 && i % 2 == 0))
					{
						this.bTab[i, j].BackColor = Color.Silver;
					}
					else
					{
						this.bTab[i, j].BackColor = default(Color);
					}
				}
			}

			this.updateLabel();
			//this.bTab[0, 2].Font = new Font(bTab[0, 2].Font.FontFamily, 14);
			//this.bTab[0, 1].Text = "OO";
			//this.bTab[0, 1].Image = default(Image);
			//this.label.Text = bTab[0, 0].Font.Size.ToString();		// 30
			//this.bTab[4, 4].Text = "XX";
			//this.bTab[4, 4].Font = new Font(this.bTab[4, 4].Font.FontFamily, 14);
		}

		private bool isX(Button button)
		{
			return button.Text.Equals("X");
		}

		private bool isO(Button button)
		{
			return button.Text.Equals("O");
		}

		private bool isE(Button button)
		{
			return button.Text.Equals("");
		}

		private bool isXX(Button button)
		{
			return button.Text.Equals("XX");
		}

		private bool isOO(Button button)
		{
			return button.Text.Equals("OO");
		}

		private bool isOSide(Button button)
		{
			return this.isO(button) || this.isOO(button);
		}

		private bool isXSide(Button button)
		{
			return this.isX(button) || this.isXX(button);
		}

		private bool isP(Button button)
		{
			return this.isXSide(button) || this.isOSide(button);
		}

		public bool getTurn()
		{
			return this.turn;
		}

		public void setTurn(bool turn)
		{
			this.turn = turn;
		}

		private int[] getCoordinates(Button b)
		{
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (b.Equals(bTab[i, j]))
					{
						int[] t = new int[2];
						t[0] = i;
						t[1] = j;
						return t;
					}
				}
			}

			return null;
		}

		public void move(Button btn)
		{

			if ((this.last != null && !this.justMoved && this.isXSide(this.last) && this.turn)
					|| (this.last != null && !this.justMoved && this.isOSide(this.last) && !this.turn))
			{
				//this.highlight();
				//this.label.Text = (this.isXX(this.last) || this.isOO(this.last)) ? "True" : "False";

				if (!this.isE(this.last))
				{
					if (this.isMoveValid(btn))
					{
						btn.Text = this.last.Text;
						btn.Image = this.last.Image;
						//Font fnt = new Font(btn.Font.FontFamily, btn.Font.Size);
						//btn.Font = new Font(this.last.Font.FontFamily, this.last.Font.Size);
						this.last.Text = "";
						//this.last.Font = fnt;
						this.last.Image = default(Image);
						this.justMoved = true;
						this.turn = this.requireSwitching ? !this.turn : this.turn;

						if (this.isQueen(btn))
						{
							if (this.isX(btn))
							{
								//btn.Font = new Font(btn.Font.FontFamily, 14);
								btn.Text = "XX";
								btn.Image = Image.FromFile(Form1.BLACK_QUEEN);
							}
							else
							{
								//btn.Font = new Font(btn.Font.FontFamily, 14);
								btn.Text = "OO";
								btn.Image = Image.FromFile(Form1.RED_QUEEN);
							}
						}
					}
				}

				this.last = btn;
				//this.highlight();
			}
			else
			{
				if (this.isP(btn))
				{
					this.last = btn;
					this.justMoved = false;
				}
				//this.highlight();
			}
		}

		public void updateLabel()
		{
			if (this.turn)
			{
				this.label.Text = "Black Player turn.";
			}
			else
			{
				this.label.Text = "Red Player turn.";
			}
		}

		public void highlight()
		{

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if(this.turn)
					{
						if (this.isX(this.bTab[i, j]))
						{
							if (this.isColliding(i, j)[0, 0] != Board.ERROR)
							{
								this.bTab[i, j].BackColor = Color.Coral;
								this.highlighted[i, j] = 1;
								continue;
							}
						}
						else if(this.isXX(this.bTab[i, j]))
						{
							int[, ,] collide = this.isCollidingQueenVersion(i, j);

							if (collide[Board.BTT_R, 0, 0] != Board.ERROR
								|| collide[Board.BTT_L, 0, 0] != Board.ERROR
								|| collide[Board.UPP_R, 0, 0] != Board.ERROR
								|| collide[Board.UPP_L, 0, 0] != Board.ERROR)
							{
								this.bTab[i, j].BackColor = Color.Coral;
								this.highlighted[i, j] = 1;
								continue;
							}
						}
						else if (this.highlighted[i, j] == 1)
						{
							if ((j % 2 == 0 && i % 2 != 0) || (j % 2 != 0 && i % 2 == 0))
							{
								this.bTab[i, j].BackColor = Color.Silver;
							}
							else
							{
								this.bTab[i, j].BackColor = default(Color);
							}
							this.highlighted[i, j] = 0;
						}
					}
					else
					{
						if (this.isO(this.bTab[i, j]))
						{
							if (this.isColliding(i, j)[0, 0] != Board.ERROR)
							{
								this.bTab[i, j].BackColor = Color.Coral;
								this.highlighted[i, j] = 1;
								continue;
							}
						}
						else if (this.isOO(this.bTab[i, j]))
						{
							int[, ,] collide = this.isCollidingQueenVersion(i, j);

							if (collide[Board.BTT_R, 0, 0] != Board.ERROR
								|| collide[Board.BTT_L, 0, 0] != Board.ERROR
								|| collide[Board.UPP_R, 0, 0] != Board.ERROR
								|| collide[Board.UPP_L, 0, 0] != Board.ERROR)
							{
								this.bTab[i, j].BackColor = Color.Coral;
								this.highlighted[i, j] = 1;
								continue;
							}
						}
						else if (this.highlighted[i, j] == 1)
						{
							if ((j % 2 == 0 && i % 2 != 0) || (j % 2 != 0 && i % 2 == 0))
							{
								this.bTab[i, j].BackColor = Color.Silver;
							}
							else
							{
								this.bTab[i, j].BackColor = default(Color);
							}
							this.highlighted[i, j] = 0;
						}
					}
				}
			}

			return;
		}

		public bool win()
		{
			bool xAlive = false;
			bool oAlive = false;

			int cntX = 0;
			int cntO = 0;

			bool isXBlocked = this.isBlocked(true);
			bool isOBlocked = this.isBlocked(false);

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (this.isXSide(this.bTab[i, j]))
					{
						xAlive = true;
						cntX++;
					}

					if (this.isOSide(this.bTab[i, j]))
					{
						oAlive = true;
						cntO++;
					}
				}
			}

			if (!xAlive && !oAlive)
			{
				MessageBox.Show("Draw.");
				return true;
			}

			if (!xAlive)
			{
				MessageBox.Show("Red Player wins!!!");
				return true;
			}

			if (!oAlive)
			{
				MessageBox.Show("Black Player wins!!!");
				return true;
			}

			if (isXBlocked && isOBlocked)
			{
				if (cntX == cntO)
				{
					MessageBox.Show("Draw.");
				}
				else if (cntX > cntO)
				{
					MessageBox.Show("Both players are blocked, black player wins\ncause of higher quantity of pawns & queens.");
				}
				else
				{
					MessageBox.Show("Both players are blocked, red player wins\ncause of higher quantity of pawns & queens.");
				}
				return true;
			}

			if (isXBlocked)
			{
				MessageBox.Show("Red Player wins, black player is blocked!!!");
				return true;
			}

			if (isOBlocked)
			{
				MessageBox.Show("Black Player Wins, red player X is blocked!!!");
				return true;
			}

			return false;
		}

		private bool isBlocked(bool isX)
		{
			bool blocked = false;

			if (isX)
			{
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						if (this.isX(this.bTab[i, j]))
						{
							if ((i == 0 && this.isOSide(this.bTab[i + 1, j + 1 > 7 ? j : j + 1]) && this.isOSide(this.bTab[i + 2, j + 2 > 7 ? j : j + 2]))
									|| (i == 1 && this.isOSide(this.bTab[i + 1, j + 1 > 7 ? j : j + 1]) && this.isOSide(this.bTab[i + 2, j + 2 > 7 ? j : j + 2]) && this.isOSide(this.bTab[i - 1, j + 1 > 7 ? j : j + 1])))
							{
								blocked = true;
							}

							else if ((i == 7 && this.isOSide(this.bTab[i - 1, j + 1 > 7 ? j : j + 1]) && this.isOSide(this.bTab[i - 2, j + 2 > 7 ? j : j + 2]))
									|| (i == 6 && this.isOSide(this.bTab[i - 1, j + 1 > 7 ? j : j + 1]) && this.isOSide(this.bTab[i - 2, j + 2 > 7 ? j : j + 2]) && this.isOSide(this.bTab[i + 1, j + 1 > 7 ? j : j + 1])))
							{
								blocked = true;
							}

							else if (i >= 2 && i <= 5 && this.isOSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1]) && this.isOSide(this.bTab[i - 2, j - 2 < 0 ? j : j - 2]) && this.isOSide(this.bTab[i + 1, j + 1 > 7 ? j : j + 1]) && this.isOSide(this.bTab[i + 2, j + 2 > 7 ? j : j + 2]))
							{
								blocked = true;
							}
							else
							{
								return false;
							}
						}

						if (this.isXX(this.bTab[i, j]))
						{
							bool upperBlock = false;
							bool bottomBlock = false;

							if ((i == 0 && this.isOSide(this.bTab[i + 1, j + 1 > 7 ? j : j + 1]) && this.isOSide(this.bTab[i + 2, j + 2 > 7 ? j : j + 2]))
									|| (i == 1 && this.isOSide(this.bTab[i + 1, j + 1 > 7 ? j : j + 1]) && this.isOSide(this.bTab[i + 2, j + 2 > 7 ? j : j + 2]) && this.isOSide(this.bTab[i - 1, j + 1 > 7 ? j : j + 1])))
							{
								upperBlock = true;
							}

							else if ((i == 7 && this.isOSide(this.bTab[i - 1, j + 1 > 7 ? j : j + 1]) && this.isOSide(this.bTab[i - 2, j + 2 > 7 ? j : j + 2]))
									|| (i == 6 && this.isOSide(this.bTab[i - 1, j + 1 > 7 ? j : j + 1]) && this.isOSide(this.bTab[i - 2, j + 2 > 7 ? j : j + 2]) && this.isOSide(this.bTab[i + 1, j + 1 > 7 ? j : j + 1])))
							{
								upperBlock = true;
							}

							else if (i >= 2 && i <= 5 && this.isOSide(this.bTab[i - 1, j + 1 > 7 ? j : j + 1]) && this.isOSide(this.bTab[i - 2, j + 2 > 7 ? j : j + 2]) && this.isOSide(this.bTab[i + 1, j + 1 > 7 ? j : j + 1]) && this.isOSide(this.bTab[i + 2, j + 2 > 7 ? j : j + 2]))
							{
								upperBlock = true;
							}

							if ((i == 0 && this.isOSide(this.bTab[i + 1, j - 1 < 0 ? j : j - 1]) && this.isOSide(this.bTab[i + 2, j - 2 < 0 ? j : j - 2]))
									|| (i == 1 && this.isOSide(this.bTab[i + 1, j - 1 < 0 ? j : j - 1]) && this.isOSide(this.bTab[i + 2, j - 2 < 0 ? j : j - 2]) && this.isOSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1])))
							{
								bottomBlock = true;
							}

							else if ((i == 7 && this.isOSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1]) && this.isOSide(this.bTab[i - 2, j - 2 < 0 ? j : j - 2]))
									|| (i == 6 && this.isOSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1]) && this.isOSide(this.bTab[i - 2, j - 2 < 0 ? j : j - 2]) && this.isOSide(this.bTab[i + 1, j - 1 < 0 ? j : j - 1])))
							{
								bottomBlock = true;
							}

							else if (i >= 2 && i <= 5 && this.isOSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1]) && this.isOSide(this.bTab[i - 2, j - 1 < 0 ? j : j - 1]) && this.isOSide(this.bTab[i + 1, j - 1 < 0 ? j : j - 1]) && this.isOSide(this.bTab[i + 2, j - 2 < 0 ? j : j - 2]))
							{
								bottomBlock = true;
							}

							if (upperBlock && bottomBlock)
							{
								blocked = true;
							}
							else
							{
								return false;
							}
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						if (this.isO(this.bTab[i, j]))
						{
							if ((i == 0 && this.isXSide(this.bTab[i + 1, j - 1 < 0 ? j : j - 1]) && this.isXSide(this.bTab[i + 2, j - 2 < 0 ? j : j - 2]))
									|| (i == 1 && this.isXSide(this.bTab[i + 1, j - 1 < 0 ? j : j - 1]) && this.isXSide(this.bTab[i + 2, j - 2 < 0 ? j : j - 2]) && this.isXSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1])))
							{
								blocked = true;
							}

							else if ((i == 7 && this.isXSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1]) && this.isXSide(this.bTab[i - 2, j - 2 < 0 ? j : j - 2]))
									|| (i == 6 && this.isXSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1]) && this.isXSide(this.bTab[i - 2, j - 2 < 0 ? j : j - 2]) && this.isXSide(this.bTab[i + 1, j - 1 < 0 ? j : j - 1])))
							{
								blocked = true;
							}

							else if (i >= 2 && i <= 5 && this.isXSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1]) && this.isXSide(this.bTab[i - 2, j - 2 < 0 ? j : j - 2]) && this.isXSide(this.bTab[i + 1, j - 1 < 0 ? j : j - 1]) && this.isXSide(this.bTab[i + 2, j - 2 < 0 ? j : j - 2]))
							{
								blocked = true;
							}
							else
							{
								return false;
							}
						}

						if (this.isOO(this.bTab[i, j]))
						{
							bool upperBlock = false;
							bool bottomBlock = false;

							if ((i == 0 && this.isXSide(this.bTab[i + 1, j + 1 > 7 ? j : j + 1]) && this.isXSide(this.bTab[i + 2, j + 2 > 7 ? j : j + 2]))
									|| (i == 1 && this.isXSide(this.bTab[i + 1, j + 1 > 7 ? j : j + 1]) && this.isXSide(this.bTab[i + 2, j + 2 > 7 ? j : j + 2]) && this.isXSide(this.bTab[i - 1, j + 1 > 7 ? j : j + 1])))
							{
								upperBlock = true;
							}

							else if ((i == 7 && this.isXSide(this.bTab[i - 1, j + 1 > 7 ? j : j + 1]) && this.isXSide(this.bTab[i - 2, j + 2 > 7 ? j : j + 2]))
									|| (i == 6 && this.isXSide(this.bTab[i - 1, j + 1 > 7 ? j : j + 1]) && this.isXSide(this.bTab[i - 2, j + 2 > 7 ? j : j + 2]) && this.isXSide(this.bTab[i + 1, j + 1 > 7 ? j : j + 1])))
							{
								upperBlock = true;
							}

							else if (i >= 2 && i <= 5 && this.isXSide(this.bTab[i - 1, j + 1 > 7 ? j : j + 1]) && this.isXSide(this.bTab[i - 2, j + 2 > 7 ? j : j + 2]) && this.isXSide(this.bTab[i + 1, j + 1 > 7 ? j : j + 1]) && this.isXSide(this.bTab[i + 2, j + 2 > 7 ? j : j + 2]))
							{
								upperBlock = true;
							}

							if ((i == 0 && this.isXSide(this.bTab[i + 1, j - 1 < 0 ? j : j - 1]) && this.isXSide(this.bTab[i + 2, j - 2 < 0 ? j : j - 2]))
									|| (i == 1 && this.isXSide(this.bTab[i + 1, j - 1 < 0 ? j : j - 1]) && this.isXSide(this.bTab[i + 2, j - 2 < 0 ? j : j - 2]) && this.isXSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1])))
							{
								bottomBlock = true;
							}

							else if ((i == 7 && this.isXSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1]) && this.isXSide(this.bTab[i - 2, j - 2 < 0 ? j : j - 2]))
									|| (i == 6 && this.isXSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1]) && this.isXSide(this.bTab[i - 2, j - 2 < 0 ? j : j - 2]) && this.isXSide(this.bTab[i + 1, j - 1 < 0 ? j : j - 1])))
							{
								bottomBlock = true;
							}

							else if (i >= 2 && i <= 5 && this.isXSide(this.bTab[i - 1, j - 1 < 0 ? j : j - 1]) && this.isXSide(this.bTab[i - 2, j - 2 < 0 ? j : j - 2]) && this.isXSide(this.bTab[i + 1, j - 1 < 0 ? j : j - 1]) && this.isXSide(this.bTab[i + 2, j - 2 < 0 ? j : j - 2]))
							{
								bottomBlock = true;
							}

							if (upperBlock && bottomBlock)
							{
								blocked = true;
							}
							else
							{
								return false;
							}
						}
					}
				}
			}

			return blocked;
		}

		private bool isQueen(Button btn)
		{
			if (this.isX(btn))
			{
				if (this.getCoordinates(btn)[1] == 7)
				{
					return true;
				}
			}
			else if (this.isO(btn))
			{
				if (this.getCoordinates(btn)[1] == 0)
				{
					return true;
				}
			}

			return false;
		}

		private bool isCollideAffecting(bool isX)
		{
			if (isX)
			{
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						if (this.isX(this.bTab[i, j]))
						{
							if (this.isColliding(i, j)[0, 0] != Board.ERROR)
							{
								return true;
							}
						}

						if (this.isXX(this.bTab[i, j]))
						{
							int[, ,] collide = this.isCollidingQueenVersion(i, j);

							if (collide[Board.BTT_R, 0, 0] != Board.ERROR
								|| collide[Board.BTT_L, 0, 0] != Board.ERROR
								|| collide[Board.UPP_R, 0, 0] != Board.ERROR
								|| collide[Board.UPP_L, 0, 0] != Board.ERROR)
							{
								return true;
							}
						}


					}
				}
			}

			else
			{
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						if (this.isO(this.bTab[i, j]))
						{
							if (this.isColliding(i, j)[0, 0] != Board.ERROR)
							{
								return true;
							}
						}
						if (this.isOO(this.bTab[i, j]))
						{
							int[, ,] collide = this.isCollidingQueenVersion(i, j);

							if (collide[Board.BTT_R, 0, 0] != Board.ERROR
								|| collide[Board.BTT_L, 0, 0] != Board.ERROR
								|| collide[Board.UPP_R, 0, 0] != Board.ERROR
								|| collide[Board.UPP_L, 0, 0] != Board.ERROR)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}

		private int[, ,] isCollidingQueenVersion(int x, int y)
		{
			int[, ,] collide = new int[4, 7, 2];

			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					collide[i, j, 0] = Board.ERROR;
				}
			}

			bool isXX = this.isXX(this.bTab[x, y]);

			bool bottomR = x == 7 || y == 7 ? false : true;
			bool bottomL = x == 0 || y == 7 ? false : true;
			bool upperR = x == 7 || y == 0 ? false : true;
			bool upperL = x == 0 || y == 0 ? false : true;

			bool bottomRO = false;
			bool bottomLO = false;
			bool upperRO = false;
			bool upperLO = false;

			int pdx = x;
			int pdy = y;

			int mdx = x;
			int mdy = y;

			int cntBTT_R = 0;
			int cntBTT_L = 0;
			int cntUPP_R = 0;
			int cntUPP_L = 0;

			while (pdx <= 7 || pdy <= 7 || mdy >= 0 || mdx >= 0)
			{
				pdx++;
				pdy++;

				mdy--;
				mdx--;

				// ****************** BOTTOM RIGHT ******************
				if (bottomR)
				{
					if (pdx <= 7 && pdy <= 7)
					{
						if ((this.isXSide(this.bTab[pdx, pdy]) && isXX) || (this.isOSide(this.bTab[pdx, pdy]) && !isXX))
						{
							bottomR = false;
						}
						else if ((this.isOSide(this.bTab[pdx, pdy]) && isXX) || (this.isXSide(this.bTab[pdx, pdy]) && !isXX))
						{
							if (bottomRO)
							{
								bottomR = false;
							}
							bottomRO = true;

						}
						else if (bottomRO && this.isE(this.bTab[pdx, pdy]))
						{
							collide[Board.BTT_R, cntBTT_R, 0] = pdx;
							collide[Board.BTT_R, cntBTT_R, 1] = pdy;
							cntBTT_R++;
						}
					}
					else
					{
						bottomR = false;
					}
				}

				// ****************** BOTTOM LEFT ******************
				if (bottomL)
				{
					if (mdx >= 0 && pdy <= 7)
					{
						if ((this.isXSide(this.bTab[mdx, pdy]) && isXX) || (this.isOSide(this.bTab[mdx, pdy]) && !isXX))
						{
							bottomL = false;
						}
						else if ((this.isOSide(this.bTab[mdx, pdy]) && isXX) || (this.isXSide(this.bTab[mdx, pdy]) && !isXX))
						{
							if (bottomLO)
							{
								bottomL = false;
							}

							bottomLO = true;
						}
						else if (bottomLO && this.isE(this.bTab[mdx, pdy]))
						{
							collide[Board.BTT_L, cntBTT_L, 0] = mdx;
							collide[Board.BTT_L, cntBTT_L, 1] = pdy;
							cntBTT_L++;
						}
					}
					else
					{
						bottomL = false;
					}
				}

				// ****************** UPPER RIGHT ******************
				if (upperR)
				{
					if (pdx <= 7 && mdy >= 0)
					{
						if ((this.isXSide(this.bTab[pdx, mdy]) && isXX) || (this.isOSide(this.bTab[pdx, mdy]) && !isXX))
						{
							upperR = false;
						}
						else if ((this.isOSide(this.bTab[pdx, mdy]) && isXX) || (this.isXSide(this.bTab[pdx, mdy]) && !isXX))
						{
							if (upperRO)
							{
								upperR = false;
							}

							upperRO = true;
						}
						else if (upperRO && this.isE(this.bTab[pdx, mdy]))
						{
							collide[Board.UPP_R, cntUPP_R, 0] = pdx;
							collide[Board.UPP_R, cntUPP_R, 1] = mdy;
							cntUPP_R++;
						}
					}
					else
					{
						upperR = false;
					}
				}

				// ****************** UPPER LEFT ******************
				if (upperL)
				{
					if (mdx >= 0 && mdy >= 0)
					{
						if ((this.isXSide(this.bTab[mdx, mdy]) && isXX) || (this.isOSide(this.bTab[mdx, mdy]) && !isXX))
						{
							upperL = false;
						}
						if ((this.isOSide(this.bTab[mdx, mdy]) && isXX) || (this.isXSide(this.bTab[mdx, mdy]) && !isXX))
						{
							if (upperLO)
							{
								upperL = false;
							}

							upperLO = true;
						}
						else if (upperLO && this.isE(this.bTab[mdx, mdy]))
						{
							//this.label.Text = this.label.Text + String.Format(" {0} {1} ", mdx, mdy);
							collide[Board.UPP_L, cntUPP_L, 0] = mdx;
							collide[Board.UPP_L, cntUPP_L, 1] = mdy;
							cntUPP_L++;
						}
					}
					else
					{
						upperL = false;
					}
				}

			}

			return collide;
		}

		private int[,] isColliding(int lx, int ly)
		{
			int[,] tab = new int[4, 2];

			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					tab[i, j] = Board.ERROR;
				}
			}

			// this.label.Text = this.label.Text + String.Format("@ lx : {0} && ly : {1} @", lx, ly);

			int cnt = 0;

			if (this.isX(this.bTab[lx, ly]))
			{

				if (!(lx + 2 > 7) && !(ly + 2 > 7))
				{
					if ((this.isO(this.bTab[lx + 1, ly + 1]) || this.isOO(this.bTab[lx + 1, ly + 1])) && this.isE(this.bTab[lx + 2, ly + 2]))
					{
						tab[cnt, 0] = lx + 2;
						tab[cnt, 1] = ly + 2;
						cnt++;
					}
				}

				if (!(lx + 2 > 7) && !(ly - 2 < 0))
				{
					if ((this.isO(this.bTab[lx + 1, ly - 1]) || this.isOO(this.bTab[lx + 1, ly - 1])) && this.isE(this.bTab[lx + 2, ly - 2]))
					{
						tab[cnt, 0] = lx + 2;
						tab[cnt, 1] = ly - 2;
						cnt++;
					}
				}

				if (!(lx - 2 < 0) && !(ly + 2 > 7))
				{
					if ((this.isO(this.bTab[lx - 1, ly + 1]) || this.isOO(this.bTab[lx - 1, ly + 1])) && this.isE(this.bTab[lx - 2, ly + 2]))
					{
						tab[cnt, 0] = lx - 2;
						tab[cnt, 1] = ly + 2;
						cnt++;
					}
				}

				if (!(lx - 2 < 0) && !(ly - 2 < 0))
				{
					if ((this.isO(this.bTab[lx - 1, ly - 1]) || this.isOO(this.bTab[lx - 1, ly - 1])) && this.isE(this.bTab[lx - 2, ly - 2]))
					{
						tab[cnt, 0] = lx - 2;
						tab[cnt, 1] = ly - 2;
						cnt++;
					}
				}
			}
			else if (this.isO(this.bTab[lx, ly]))
			{

				if (!(lx + 2 > 7) && !(ly + 2 > 7))
				{
					if ((this.isX(this.bTab[lx + 1, ly + 1]) || this.isXX(this.bTab[lx + 1, ly + 1])) && this.isE(this.bTab[lx + 2, ly + 2]))
					{
						tab[cnt, 0] = lx + 2;
						tab[cnt, 1] = ly + 2;
						cnt++;
					}
				}

				if (!(lx + 2 > 7) && !(ly - 2 < 0))
				{
					if ((this.isX(this.bTab[lx + 1, ly - 1]) || this.isXX(this.bTab[lx + 1, ly - 1])) && this.isE(this.bTab[lx + 2, ly - 2]))
					{
						tab[cnt, 0] = lx + 2;
						tab[cnt, 1] = ly - 2;
						cnt++;
					}
				}

				if (!(lx - 2 < 0) && !(ly + 2 > 7))
				{
					if ((this.isX(this.bTab[lx - 1, ly + 1]) || this.isXX(this.bTab[lx - 1, ly + 1])) && this.isE(this.bTab[lx - 2, ly + 2]))
					{
						tab[cnt, 0] = lx - 2;
						tab[cnt, 1] = ly + 2;
						cnt++;
					}
				}

				if (!(lx - 2 < 0) && !(ly - 2 < 0))
				{
					if ((this.isX(this.bTab[lx - 1, ly - 1]) || this.isXX(this.bTab[lx - 1, ly - 1])) && this.isE(this.bTab[lx - 2, ly - 2]))
					{
						tab[cnt, 0] = lx - 2;
						tab[cnt, 1] = ly - 2;
						cnt++;
					}
				}
			}

			return tab;
		}

		private bool onSegment(int x, int y, int lx, int ly)
		{
			int cntX = 0;
			int cntY = 0;

			while (lx != x)
			{
				lx = lx < x ? lx + 1 : lx - 1;
				cntX++;
			}

			while (ly != y)
			{
				ly = ly < y ? ly + 1 : ly - 1;
				cntY++;
			}

			return cntX == cntY;
		}

		private void batchRemove(int x, int y, int lx, int ly)
		{
			while (lx != x && ly != y)
			{
				lx = lx < x ? lx + 1 : lx - 1;
				ly = ly < y ? ly + 1 : ly - 1;
				this.bTab[lx, ly].Text = "";
				this.bTab[lx, ly].Image = default(Image);
			}
		}

		private bool clear(int x, int y, int lx, int ly)
		{
			if (x == lx || y == ly)
			{
				return false;
			}

			if (!this.isE(this.bTab[x, y]))
			{
				return false;
			}

			int maxX = Math.Max(x, lx);
			int minX = Math.Min(x, lx);

			int maxY = Math.Max(y, ly);
			int minY = Math.Min(y, ly);

			minX++;
			minY++;

			while (minX != maxX && minY != maxY)
			{
				if (!this.isE(this.bTab[minX, minY]))
				{
					return false;
				}

				minX++;
				minY++;
			}

			return true;
		}

		private bool isMoveValidQueenVersion(Button btn, int x, int y, int lx, int ly)
		{
			if (x == lx || y == ly)
			{
				return false;
			}

			bool coll = this.isCollideAffecting(this.isXSide(this.last));

			if (!coll)
			{
				if (this.onSegment(x, y, lx, ly))
				{
					if (this.clear(x, y, lx, ly))
					{
						this.requireSwitching = true;
						return true;
					}
				}
			}

			if (coll)
			{
				int[, ,] collide = this.isCollidingQueenVersion(lx, ly);

				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 7; j++)
					{
						if (collide[i, j, 0] == Board.ERROR)
						{
							break;
						}

						if (collide[i, j, 0] == x && collide[i, j, 1] == y)
						{
							String tmp = this.bTab[lx, ly].Text;
							this.batchRemove(x, y, lx, ly);
							this.bTab[x, y].Text = tmp;

							if (this.isCollidingQueenVersion(x, y)[Board.BTT_R, 0, 0] != Board.ERROR
									|| this.isCollidingQueenVersion(x, y)[Board.BTT_L, 0, 0] != Board.ERROR
									|| this.isCollidingQueenVersion(x, y)[Board.UPP_R, 0, 0] != Board.ERROR
									|| this.isCollidingQueenVersion(x, y)[Board.UPP_L, 0, 0] != Board.ERROR)
							{
								this.requireSwitching = false;

								this.bTab[x, y].Text = tmp;
								return true;
							}

							this.bTab[x, y].Text = tmp;
							this.requireSwitching = true;
							return true;
						}
					}
				}
			}

			return false;
		}

		private bool isMoveValid(Button btn)
		{
			int[] btnCords = this.getCoordinates(btn);
			int[] lstCords = this.getCoordinates(this.last);

			int x = btnCords[0];
			int y = btnCords[1];
			int lx = lstCords[0];
			int ly = lstCords[1];

			if (this.isXX(this.last) || this.isOO(this.last))
			{
				return isMoveValidQueenVersion(btn, x, y, lx, ly);
			}

			int[,] collide = this.isColliding(lx, ly);

			if (collide[0, 0] == Board.ERROR && !this.isCollideAffecting(this.isXSide(this.last)))
			{

				if (this.isX(this.last))
				{
					if ((y - 1 == ly) && (x - 1 == lx || x + 1 == lx))
					{
						if (this.isE(this.bTab[x - 1 == lx ? lx + 1 : lx - 1, ly + 1]))
						{
							this.requireSwitching = true;
							return true;
						}
					}
				}
				else if (this.isO(this.last))
				{
					if ((y + 1 == ly) && (x - 1 == lx || x + 1 == lx))
					{
						if (this.isE(this.bTab[x - 1 == lx ? lx + 1 : lx - 1, ly - 1]))
						{
							this.requireSwitching = true;
							return true;
						}
					}
				}
			}
			else
			{

				for (int i = 0; i < 4; i++)
				{
					if (collide[i, 0] == Board.ERROR)
					{
						break;
					}

					if (collide[i, 0] == x && collide[i, 1] == y)
					{
						this.bTab[(x + lx) / 2, (y + ly) / 2].Text = "";
						this.bTab[(x + lx) / 2, (y + ly) / 2].Image = default(Image);
						String tmp = this.bTab[lx, ly].Text;
						Image tmi = this.bTab[lx, ly].Image;
						this.bTab[x, y].Text = this.bTab[lx, ly].Text;

						if (this.isColliding(x, y)[0, 0] != Board.ERROR)
						{
							this.requireSwitching = false;

							this.bTab[x, y].Text = tmp;
							this.bTab[x, y].Image = tmi;
							return true;
						}

						this.bTab[x, y].Text = tmp;
						this.bTab[x, y].Image = tmi;
						this.requireSwitching = true;
						return true;
					}

				}
			}

			return false;
		}
	}
}