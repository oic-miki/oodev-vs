using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VendingMachine
{
    public partial class Form1 : Form
    {

		private Form2 form2;
		private VendingMachine vendingMachine = new VendingMachine();
		private List<System.Windows.Forms.Label> soldOutLabels =
			new List<System.Windows.Forms.Label>();
		private List<System.Windows.Forms.Button> buttonViews =
			new List<System.Windows.Forms.Button>();

		public Form1()
        {
            InitializeComponent();

			form2 = new Form2(this);

			soldOutLabels.Add(label5);
			soldOutLabels.Add(label6);
			soldOutLabels.Add(label7);
			soldOutLabels.Add(label8);

//			vendingMachine.addSoldOutLabels(soldOutLabels);

			buttonViews.Add(button1);
			buttonViews.Add(button2);
			buttonViews.Add(button3);
			buttonViews.Add(button4);

			//			vendingMachine.addButtonViews(buttonViews);

			vendingMachine.init(form2, soldOutLabels, buttonViews);

		}

		private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

		private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

			Drink drink = new Drink("オレンジジュース", 120);

			vendingMachine.addDrink(1, drink);

			label1.Text = drink.getName();

			button1.Text = drink.getPrice().ToString();

		}

        private void button1_Click(object sender, EventArgs e)
        {

			vendingMachine.pushButton(1);

		}

		private void button9_Click(object sender, EventArgs e)
        {

			vendingMachine.addMoney(10);

		}

        private void button10_Click(object sender, EventArgs e)
        {

			vendingMachine.addMoney(50);

		}

		private void button11_Click(object sender, EventArgs e)
		{

			vendingMachine.addMoney(100);

		}

		private void button12_Click(object sender, EventArgs e)
        {

			vendingMachine.addMoney(500);

		}

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
		public int refund()
        {

			return vendingMachine.refund();

        }

	}
	public class VendingMachine
	{

		private Form2 form2;
		private Hashtable laneMap = new Hashtable();
		private Hashtable buttonMap = new Hashtable();
		private Hashtable soldOutLabelMap = new Hashtable();
		private int money = 0;
		private List<System.Windows.Forms.Label> soldOutLabels;
		private List<System.Windows.Forms.Button> buttonViews;

		private List<UpdateListener> updateListeners = new List<UpdateListener>();
		public VendingMachine()
		{

		}

		public void init(
			Form2 form2,
			List<System.Windows.Forms.Label> soldOutLabels,
			List<System.Windows.Forms.Button> buttonViews)
        {

			this.form2 = form2;
			form2.Hide();

			int laneNumber = 1;

			for (int count = 0; count < soldOutLabels.Count; count++)
			{

				Lane lane = new Lane(this, laneNumber++);

				laneMap.Add(lane.getNumber(), lane);

				buttonMap.Add(lane.getNumber(), new Button(
					lane,
					buttonViews[count]));

				soldOutLabelMap.Add(lane.getNumber(), new SoldOutLabel(
					lane,
					soldOutLabels[count]));

			}

			form2.Show();

		}

		public void addUpdateListener(UpdateListener updateListener)
        {

			updateListeners.Add(updateListener);

		}
		public void addSoldOutLabels(List<System.Windows.Forms.Label> soldOutLabels)
        {

			this.soldOutLabels = soldOutLabels;


		}

		public void addButtonViews(List<System.Windows.Forms.Button> buttonViews)
        {

			this.buttonViews = buttonViews;


		}

		public Drink addDrink(int laneNumber, Drink drink)
		{

			((Lane)laneMap[laneNumber]).addDrink(drink);

			update();

			return drink;

		}

		public int getMoney()
		{

			return money;

		}

		public int addMoney(int money)
		{

			this.money += money;

			update();

			return this.money;

		}

        private void update()
        {

			form2.getLabel().Text = money.ToString();

/*
			foreach (Button button in buttonMap.Values)
			{

				button.update();

			}

			foreach (SoldOutLabel soldOutLabel in soldOutLabelMap.Values)
			{

				soldOutLabel.update();

			}

*/
			foreach (UpdateListener updateListener in updateListeners)
			{

				updateListener.update();

			}
			
		}

		public bool isLightOn(int laneNumber)
		{

			return ((Button) buttonMap[laneNumber]).isLightOn();

		}

		public Drink pushButton(int laneNumber)
		{

			Drink drink = ((Lane)laneMap[laneNumber]).removeDrink();

			money -= drink.getPrice();

			return drink;

		}

		public int refund()
		{

			int refundMoney = money;

			// 投入額をゼロにする
			money = 0;

			update();

			return refundMoney;

		}

	}
	public class Lane
	{

		private VendingMachine vendingMachine;
		private int number;
		private List<Drink> drinks = new List<Drink>();

		public Lane(VendingMachine vendingMachine, int number)
		{

			this.vendingMachine = vendingMachine;
			this.number = number;

		}

		public VendingMachine getVendingMachine()
		{

			return vendingMachine;

		}

		public int getNumber()
		{

			return number;

		}

		public bool hasDrink()
		{

			return drinks.Count() > 0;

		}

		public int getPrice()
		{

			return drinks[0].getPrice();

		}

		public Drink addDrink(Drink drink)
		{

			drinks.Add(drink);

			return drink;

		}

		public Drink removeDrink()
		{

			return drinks[drinks.Count() - 1];

		}

	}
	public class Button : UpdateListener
	{

		private Lane lane;
		private System.Windows.Forms.Button view;
		private bool soldOut = false;
		private bool sale = false;
		private bool able = false;

		public Button(
			Lane lane,
			System.Windows.Forms.Button view)
		{

			this.lane = lane;
			this.view = view;

			lane.getVendingMachine().addUpdateListener(this);

			changeSoldOut();

		}

		public Lane getLane()
		{

			return lane;

		}

		public void update()
		{

			if (lane.hasDrink())
			{

				changeSale();

				if (lane.getVendingMachine().getMoney() >= lane.getPrice())
                {

					changeAble();

				}
				else
                {

					changeDisable();

				}

			}
			else
			{

				changeSoldOut();

			}

		}

		private void changeAble()
		{

			able = true;
			view.BackColor = Color.FromArgb(255, 255, 192);

		}

		private void changeDisable()
		{

			able = false;
			view.BackColor = Color.FromArgb(255, 255, 255);

		}

		private void changeSoldOut()
		{

			changeDisable();
			sale = false;
			soldOut = !sale;

		}

		private void changeSale()
		{

			soldOut = false;
			sale = !soldOut;

		}

		public bool isLightOn()
		{

			return able;

		}

	}
	public class SoldOutLabel : UpdateListener
	{

		private Lane lane;
		private System.Windows.Forms.Label view;
		private bool soldOut = false;
		private bool sale = false;
		private bool able = false;

		public SoldOutLabel(
			Lane lane,
			System.Windows.Forms.Label view)
		{

			this.lane = lane;
			this.view = view;

			lane.getVendingMachine().addUpdateListener(this);

			changeSoldOut();

		}

		public Lane getLane()
		{

			return lane;

		}

		public void update()
		{

			if (lane.hasDrink())
			{

				changeSale();

			}
			else
			{

				changeSoldOut();

			}

		}

		private void changeSoldOut()
		{

			view.Show();
			sale = false;
			soldOut = !sale;

		}

		private void changeSale()
		{

			view.Hide();
			soldOut = false;
			sale = !soldOut;

		}

		public bool isLightOn()
		{

			return able;

		}

	}
	public class Drink
	{

		private String name;
		private int price;

		public Drink(String name, int price)
		{

			this.name = name;
			this.price = price;

		}

		public String getName()
		{

			return name;

		}

		public int getPrice()
		{

			return price;

		}

	}

	public interface UpdateListener
    {
		void update();

	}

}
