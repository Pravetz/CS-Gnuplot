## CS-Gnuplot
This is a simple Gnuplot "binding" for C# in one `Gnuplot.cs` file, which provides user with Gnuplot class with possibilities plotting data to `.png` files plus some configurations like type of plots and colors.
For this utility to work, one needs gnuplot installed in his system and then provide path to it in any of the `Gnuplot` class constructors(`gp_path` argument).

## Gnuplot constructors
There's a plenty of different constructors for Gnuplot, which serve for different use-cases:
```csharp

public Gnuplot(string gp_path, string plot_save_path);
```
Is used to create Gnuplot instance with empty lists of line coordinates, colors and names, use it if you don't know your plot figures ahead of time and want to dynamically add objects to a plot.

```csharp

public Gnuplot(string gp_path, string plot_save_path, int line_count, bool generate_colors);
```

Is used to create Gnuplot instance with pre-allocated `line_count` number of lines, their corresponding colors(generates random colors if `generate_colors` is `true`) and names, use it if you know the number of figures on your plot, but don't want to specify their colors and names beforehand.


```csharp

public Gnuplot(string gp_path, string plot_save_path, string[] colors);
```

Is used to create Gnuplot instance with pre-allocated `colors.Length` number of lines, colors and names, use it if you know the colors of figures on your plot, but don't want to specify their names or explicit line count.


```csharp

public Gnuplot(string gp_path, string plot_save_path, string[] names, bool generate_colors);
```

Is used to create Gnuplot instance with pre-allocated `names.Length` number of lines and colors, use it if you know the names of figures on your plot, but don't want to specify their colors or explicit line count.

```csharp

public Gnuplot(string gp_path, string plot_save_path, string[] colors, string[] names);
```

Is used to create Gnuplot instance with pre-allocated `names.Length` number of lines and `colors.Length` number of colors, usually `names.Length == colors.Length`, but this is not mandatory for cases like ahead of time name/color definitions, for any reason user has to do so.

## Important functions
Gnuplot class offers functions to create new lines(plot objects), set their gnuplot types(e.g. `linespoints`, `points`, `lines`), hex color values and, most importantly, coordinates.

```csharp

public void create_lines(string[] names);
```

`create_lines` is used to grow list of lines and their data by `names.Length` entries, new entries immediately receive names from `names` array. Use this to dynamically add lines to your plots.

```csharp

public void set_line_HEX_color(int line_id, string hex_value);
```

`set_line_HEX_color`, obviously, sets line color. Line is specified by `line_id`, `color` is a string containing hex-value in following format: "#RRGGBB", so, for bright red `color = "#ff0000";`

```csharp

public void set_line_name(int line_id, string name);
```

`set_line_name` sets or updates `line_id`'s line name.

```csharp

public void add_coords(double x, double y, int line_id);
```

`add_coords` appends new (x, y) pair to line at index `line_id`.


```csharp

public void init_to_coords(double x, double y);
```

`init_to_coords` adds the initial (x, y) coordinate for ALL lines, that were allocated before it's call.

```csharp

public void set_plot_type(string plot_type, int line_id);
```
`set_plot_type` sets gnuplot style for line at `line_id`, you can use any style specified in gnuplot's documentation (http://gnuplot.info/docs/Plotting_Styles.html)

```csharp

public void set_default_plot_type_linespoints();
```
`set_default_plot_type_linespoints` sets `linespoints` style for all lines, that were allocated before it's call. This function is called automatically by all `Gnuplot` class constructors, that's why user usually doesn't need to explicitly call this, unless adding dynamically adding lines to `Gnuplot` and there's a need to set default style "just in case".

