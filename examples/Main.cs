using System;
using GnuplotBindUtil;

namespace ExampleSpace
{
	class MainClass
	{
		static void Main()
		{
			// RANDOM COLORS EXAMPLE
			{
				string[] names = {
					"Points",		// will have id 0
					"Lines"			// will have id 1
				};
				Gnuplot plotobj = new Gnuplot("gnuplot", "plot.txt", names, true);
				Random rand = new Random();
				
				plotobj.set_plot_type("points", 0);
				plotobj.set_plot_type("lines", 1);
				
				for(int i = 0; i < 100; i += 10){
					int x = rand.Next(i, i + 10);
					int y = rand.Next(0, i);
					plotobj.add_coords(x, y, 0);		// add coordinates to "Points"
					plotobj.add_coords(x, y, 1);		// add coordinates to "Lines"
				}
				
				plotobj.plot();
			}
			// MANUAL COLORS EXAMPLE
			{
				string[] colors = {
					"#0000ff",		// RGB(255,0,255)
					"#ffff00"		// RGB(255,255,0)
				};
				string[] names = {
					"Lines",		// will have id 0
					"Points"		// will have id 1
				};
				Gnuplot plotobj = new Gnuplot("gnuplot", "plotmc.txt", colors, names);
				
				Random rand = new Random();
				
				plotobj.set_plot_type("lines", 0);
				plotobj.set_plot_type("points", 1);
				
				for(int i = 0; i < 200; i += 20){
					int x = rand.Next(i, i + 20);
					int y = rand.Next(0, i / 2);
					plotobj.add_coords(x, y, 0);		// add coordinates to "Lines"
					plotobj.add_coords(x, y, 1);		// add coordinates to "Points"
				}
				
				plotobj.plot();
			}
		}
	}
}