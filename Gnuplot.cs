using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace GnuplotBindUtil
{
    public class Gnuplot
    {
        protected string gnuplot_path;
        protected string save_path;
        protected List<string> line_colors;
        protected List<string> line_names;
        protected List<string> line_plot_types;
        protected List<List<double>> line_coords;
        protected Process gnuplot;

        public bool Exited
        {
            get
            {
                return gnuplot.HasExited;
            }
        }

        protected void write_coords()
        {
            using (StreamWriter coord_writer = File.AppendText(save_path))
            {
                for (int i = 0; i < line_coords.Count; i++)
                {
                    for (int j = 0; j < line_coords[i].Count; j += 2)
                    {
                        coord_writer.WriteLine("{0} {1}", line_coords[i][j], line_coords[i][j + 1]);
                    }
                    coord_writer.WriteLine('\n');
                }
            }
        }

        public void plot()
        {
            write_coords();
            gnuplot = new Process();
            string plot_args = "\'" + save_path + "\' ";


            gnuplot.StartInfo.FileName = gnuplot_path;
            gnuplot.StartInfo.UseShellExecute = false;
            gnuplot.StartInfo.RedirectStandardInput = true;
            gnuplot.Start();

            StreamWriter gnuci = gnuplot.StandardInput;
            gnuci.WriteLine("set terminal png");
            gnuci.WriteLine("set output \'{0}.png\'", save_path.Substring(0, save_path.LastIndexOf('.')));
            for (int i = 0; i < line_colors.Count; i++)
            {
                gnuci.WriteLine("set style line {0} \\\n    linecolor rgb '{1}' \\\n    linetype 1 linewidth 2 \\\n    pointtype 7 pointsize 0.5", i + 1, line_colors[i]);
            }
            for (int i = 0; i < line_colors.Count; i++)
            {
                plot_args += "index " + i.ToString() + " title \'" + line_names[i] + "\' with " + line_plot_types[i] + " linestyle " + (i + 1).ToString();
                if (i < line_colors.Count - 1)
                {
                    plot_args += ", \\\n\t \'\'\t\t\t\t\t";
                }
            }

            gnuci.WriteLine("plot {0}", plot_args);
            gnuci.WriteLine("exit");
        }

        public void create_lines(string[] names)
        {
            line_names.AddRange(names);
            line_colors.AddRange(new string[names.Length]);
            line_coords.AddRange(new List<double>[names.Length]);
            line_coords[line_coords.Count - 1] = new List<double>();
            line_plot_types.AddRange(new string[names.Length]);
            generate_line_colors();
            init_coord_list();
            set_default_plot_type_linespoints();
        }

        public void set_line_HEX_color(int line_id, string hex_value)
        {
            if (line_id >= 0 && line_id < line_colors.Count)
            {
                line_colors[line_id] = hex_value;
            }
        }

        public void set_line_name(int line_id, string name)
        {
            if (line_id >= 0 && line_id < line_names.Count)
            {
                line_names[line_id] = name;
            }
        }

        public void add_coords(double x, double y, int line_id)
        {
            if (line_id >= 0 && line_id < line_coords.Count)
            {
                line_coords[line_id].Add(x);
                line_coords[line_id].Add(y);
            }
        }

        protected void clear_txt_data()
        {
            if (File.Exists(save_path))
            {
                File.WriteAllText(save_path, string.Empty);
            }
        }

        protected void init_coord_list()
        {
            for (int i = 0; i < line_coords.Count; i++)
            {
                if (line_coords[i] == null)
                {
                    line_coords[i] = new List<double>();
                }
            }
        }

        protected void generate_line_colors()
        {
            Random rand = new Random();
            for (int i = 0; i < line_colors.Count; i++)
            {
                line_colors[i] = "#";
                byte[] rgb = new byte[3];
                rand.NextBytes(rgb);
                rgb[0] = rgb[0] > 128 ? (byte)128 : rgb[0];
                rgb[1] = rgb[1] > 128 ? (byte)128 : rgb[1];
                rgb[2] = rgb[2] > 128 ? (byte)128 : rgb[2];
                line_colors[i] += BitConverter.ToString(rgb).Replace("-", "");
            }
        }

        public void init_to_coords(double x, double y)
        {
            for (int i = 0; i < line_coords.Count; i++)
            {
                add_coords(x, y, i);
            }
        }

        public void set_plot_type(string plot_type, int line_id)
        {
            if (line_id >= 0 && line_id < line_plot_types.Count)
            {
                line_plot_types[line_id] = plot_type.ToLower();
            }
        }

        public void set_default_plot_type_linespoints()
        {
            for (int i = 0; i < line_plot_types.Count; i++)
            {
                if (line_plot_types[i] == null)
                {
                    line_plot_types[i] = "linespoints";
                }
            }
        }

        public Gnuplot(string plot_save_path)
        {
            save_path = plot_save_path;
            line_colors = new List<string>();
            line_names = new List<string>();
            line_plot_types = new List<string>();
            line_coords = new List<List<double>>();

            clear_txt_data();
            set_default_plot_type_linespoints();
        }

        public Gnuplot(string plot_save_path, int line_count, bool generate_colors)
        {
            save_path = plot_save_path;
            line_colors = new List<string>(new string[line_count]);
            line_names = new List<string>(new string[line_count]);
            line_coords = new List<List<double>>(new List<double>[line_count]);
            line_plot_types = new List<string>(new string[line_count]);
            clear_txt_data();
            init_coord_list();
            set_default_plot_type_linespoints();
            if (generate_colors)
            {
                generate_line_colors();
            }
        }

        public Gnuplot(string gp_path, string plot_save_path, string[] colors)
        {
            gnuplot_path = gp_path;
            save_path = plot_save_path;
            line_colors = new List<string>(colors);
            line_coords = new List<List<double>>(new List<double>[colors.Length]);
            line_names = new List<string>(new string[colors.Length]);
            line_plot_types = new List<string>(new string[colors.Length]);
            clear_txt_data();
            init_coord_list();
            set_default_plot_type_linespoints();
        }

        public Gnuplot(string gp_path, string plot_save_path, string[] names, bool generate_colors)
        {
            gnuplot_path = gp_path;
            save_path = plot_save_path;
            line_colors = new List<string>(new string[names.Length]);
            line_coords = new List<List<double>>(new List<double>[names.Length]);
            line_names = new List<string>(names);
            line_plot_types = new List<string>(new string[names.Length]);
            clear_txt_data();
            init_coord_list();
            if (generate_colors)
            {
                generate_line_colors();
            }
            set_default_plot_type_linespoints();
        }

        public Gnuplot(string gp_path, string plot_save_path, string[] colors, string[] names)
        {
            gnuplot_path = gp_path;
            save_path = plot_save_path;
            line_colors = new List<string>(colors);
            line_coords = new List<List<double>>(new List<double>[colors.Length]);
            line_names = new List<string>(names);
            line_plot_types = new List<string>(new string[names.Length]);
            clear_txt_data();
            init_coord_list();
            set_default_plot_type_linespoints();
        }
    }
}
