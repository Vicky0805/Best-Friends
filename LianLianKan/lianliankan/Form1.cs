using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LianLianKan
{


	public partial class Frm : Form
	{


		Button[,] btn;//
		List<Button> btnList = new List<Button>();
		List<Button> allBtn = new List<Button>();   //场上未消除的所有按钮
		List<Button> temp = new List<Button>();
		Button tempLastBtn;
		int blood=20000;
		int step=25;
		


		public Frm()
		{
			InitializeComponent();
		}
		private void Frm_LianLianKan_Load(object sender, EventArgs e)
		{
			btn = new Button[Data.width, Data.height];
			ImageInit();
			GridInit();
			RandomApplyImage();
			label1.Text = ""+25 ;
			label2.Text = "20000";
			label2.Location = new Point(264,30);
			label3.Text = "";
			
		}
		private void btn_start_Click(object sender, EventArgs e)
		{

			if (true)//重排列
			{
				panel_gameArea.BackgroundImage = null;
				for (int i = 0; i < Data.height; i++)
				{
					for (int ii = 0; ii < Data.width; ii++)
					{
						panel_gameArea.Controls.Remove(btn[ii, i]);
					}
				}
			}
			ImageInit();
			GridInit();
			RandomApplyImage();
			panel_gameArea.BackgroundImage = Bitmap.FromFile(Application.StartupPath + @"\img\bg.jpg");
			for (int i = 0; i < Data.height; i++)
			{
				for (int ii = 0; ii < Data.width; ii++)
				{
					allBtn.Add(btn[ii, i]);
				}
			}
		}
		private void btnClick(object sender, EventArgs e)
		{
			Button tempCurrentBtn = (Button)sender;
			if (temp.Count == 0)//选中第一个element的情况
			{
				tempCurrentBtn.Enabled = false;
				tempCurrentBtn.FlatAppearance.BorderSize = 5;
				tempCurrentBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255);
				tempLastBtn = tempCurrentBtn;
				temp.Add(tempLastBtn);
				tempCurrentBtn = null;
			}
			else if (temp.Count >= 1) //第一个选中然后选第二个
			{
				if (GetDistance(tempLastBtn,tempCurrentBtn) < 87.0)
				{
					
					if (tempLastBtn.BackgroundImage.Tag.Equals(tempCurrentBtn.BackgroundImage.Tag))
					{
						tempCurrentBtn.Enabled = false;
						tempCurrentBtn.FlatAppearance.BorderSize = 5;
						tempCurrentBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255);
						tempLastBtn = tempCurrentBtn;
						temp.Add(tempLastBtn);
					}
					else
					{
						if (temp.Count >= 3)
						{
							for (int i = 0; i < temp.Count; i++)
							{
								temp[i].BackgroundImage=null;
								temp[i].Enabled = true;
								temp[i].FlatAppearance.BorderSize = 0;
								temp[i].FlatAppearance.BorderColor = Color.White;
								
								 }
							blood -= 80*temp.Count;
							label2.Text = ""+blood;
							step--;
							label1.Text = "" + step;
							if (step > 0)
							{
								if (blood <= 0)
								{
									MessageBox.Show("Congratulations!");
								}
							}
							else
							{
								if (blood <= 0)
								{
									blood = 0;
									MessageBox.Show("Congratulations!");
								}
								else
								{
									MessageBox.Show("Sorry! GameOver!");
								}
							}
							down();
							filled();
							temp.Clear();
						}
						else
						{
							for (int i = 0; i <= temp.Count - 1; i++)
							{
								temp[i].Enabled = true;
								temp[i].FlatAppearance.BorderSize = 0;
								temp[i].FlatAppearance.BorderColor = Color.White;
							}
							temp.Clear();
						}
					}
                }
                else if(temp.Count >= 3)
                {
					for (int i = 0; i < temp.Count; i++)
					{
						temp[i].BackgroundImage = null;
						temp[i].Enabled = true;
						temp[i].FlatAppearance.BorderSize = 0;
						temp[i].FlatAppearance.BorderColor = Color.White;
					}
					step--;
					blood -= 80 * temp.Count;
					label2.Text = "" + blood;
					label1.Text = "" + step;
					if (step > 0)
					{
                        if (blood <= 0)
                        {
							blood = 0;
						MessageBox.Show("Congratulations!");
                        }	
					}
					else
					{

						if (blood <= 0)
						{
							blood = 0;
							MessageBox.Show("Congratulations!");
						}
						else
						{
							MessageBox.Show("Sorry! GameOver!");
						}
					}
					temp.Clear();
					down();
					filled();	
				}
                else
                {
					for (int i = 0; i <= temp.Count - 1; i++)
					{
						temp[i].Enabled = true;
						temp[i].FlatAppearance.BorderSize = 0;
						temp[i].FlatAppearance.BorderColor = Color.White;	
					}
					temp.Clear();
				}
			}	
		}		
		
		private void ImageInit()//获取四种图片
		{
			Data.images = new Image[Data.imageCount];
			for (int i = 0; i < Data.imageCount; i++)
			{
				Data.images[i] = Image.FromFile(Application.StartupPath + @"\img\" + i + ".png");
			}
		}

		private void GridInit()//按钮布局
		{
			for (int i = 0; i < Data.height; i++)
			{
				for (int ii = 0; ii < Data.width; ii++)
				{
					Button bt = new Button();
					bt.Click += new EventHandler(btnClick);
					bt.Name = ii.ToString() + "_" + i.ToString();
					bt.Text = "";
					bt.Location = new Point(1 + ii * (Data.imageSize + Data.offset), 1 + i * (Data.imageSize + Data.offset)); // 按钮屏幕位置
					bt.Size = new Size(Data.imageSize, Data.imageSize);
					bt.Parent = panel_gameArea;
					bt.BackgroundImageLayout = ImageLayout.Stretch;
					bt.FlatStyle = FlatStyle.Flat;
					bt.FlatAppearance.BorderSize = 0;
					bt.FlatAppearance.BorderColor = Color.White;
					panel_gameArea.Controls.Add(bt);

					btn[ii, i] = bt;
					btnList.Add(btn[ii, i]);
				}
			}

		}

		private void RandomApplyImage()//随机分布
		{
			
			int l = Data.height * Data.width / 2;
			int a = l / Data.imageCount;
			int r = l % Data.imageCount;

			Random random = new Random();

			for (int i = 0; i < Data.imageCount; i++)
			{
				for (int ii = 0; ii < a; ii++)
				{
					for (int iii = 0; iii < 2; iii++)
					{
						Button tempBtn = btnList[random.Next(0, btnList.Count)];
						tempBtn.BackgroundImage = Image.FromFile(Application.StartupPath + @"\img\" + i + ".png");
						tempBtn.BackgroundImage.Tag = i;
						btnList.Remove(tempBtn);
					}
				}
			}
			//如果有多余的
			if (r != 0)
			{
				int ran;
				for (int i = 0; i < r; i++)
				{
					ran = random.Next(0, Data.imageCount);
					for (int ii = 0; ii < 2; ii++)
					{
						Button tempBtn = btnList[random.Next(0, btnList.Count)];
						tempBtn.BackgroundImage = Image.FromFile(Application.StartupPath + @"\img\" + ran + ".png");
						tempBtn.BackgroundImage.Tag = ran;
						btnList.Remove(tempBtn);
					}
				}
			}
		}

		private double GetDistance(Button button1, Button button2)
		{  
			int x = button1.Location.X - button2.Location.X;
			int y = button1.Location.Y - button2.Location.Y;
			return Math.Sqrt(x * x + y * y);
		}

		private void down()
		{
			for (int iii = 0; iii < Data.height; iii++)
			{
				for (int i = 1; i < Data.height; i++)
				{
					for (int ii = 0; ii < Data.width; ii++)
					{
						if (btn[ii, i].BackgroundImage == null)
						{
							btn[ii, i].BackgroundImage = btn[ii, i - 1].BackgroundImage;
							btn[ii, i - 1].BackgroundImage = null;

						}
						
					}

				}	
			}
			
		}
		
			private void filled()
			{
			int a = 1;
			for (int i = 0; i < Data.imageCount; i++)
			{
				for (int ii = 0; ii < Data.height; ii++)
				{
					for (int iii = 0; iii < Data.width; iii++)
					{
                        if (btn[iii, ii].BackgroundImage == null)
                        {
							
							btn[iii, ii].BackgroundImage = Data.images[a];
							btn[iii, ii].BackgroundImage.Tag =a;
						}
						a= (a + 1)%4;

					}
				}

            }

        }			
			#region 菜单栏
			private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
						{
							Application.Exit();
						}

						#endregion


					}
				}

