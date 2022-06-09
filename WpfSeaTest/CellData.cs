using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSeaTest
{
	class CellData
	{
		public int x { get; set; }
		public int y { get; set; }
		public bool busy { get; set; }
		public int length { get; set; }
		public int number { get; set; }
		public int health { get; set; }

		public CellData()
		{
			x = -1;
			y = -1;
			busy = false;
			length = 0;
			number = 0;
			health = 0;
		}

		public CellData(int x, int y, bool busy, int length, int number, int health) //x = j, y = i (y - столбцы, x - строки)
		{
			this.x = x;
			this.y = y;
			this.busy = busy;
			this.length = length;
			this.number = number;
			this.health = health;
		}
	}
}