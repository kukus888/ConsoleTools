using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleTools
{
    public class Class1
    {
    }
    public class Menu
    {
        /// <summary>
        /// A character displayed in front of seection
        /// </summary>
        public string Selector { get; set; } = "> ";
        /// <summary>
        /// List of options to be dispayed/executed.<br/>
        /// <strong>string</strong> A string to be displayed in menu.<br/>
        /// <strong>Action</strong> An action to be performed when an option is selected.
        /// </summary>
        public List<KeyValuePair<string, Action>> Options { get; set; } = new();
        /// <summary>
        /// Controls to be used when navigating around the menu
        /// </summary>
        public Controls KeyControls = new();
        /// <summary>
        /// Styling of the element
        /// </summary>
        public Graphics ConsoleGraphics = new();
        /// <summary>
        /// Title of the Menu<br/>
        /// <strong>string.Empty</strong> for no title
        /// </summary>
        public string Title { get; set; } = string.Empty;
        public Menu()
        {
            
        }
        public void Run()
        {
            int Selection = 0;
            Console.ForegroundColor = this.ConsoleGraphics.ForegroundColor;
            Console.BackgroundColor = this.ConsoleGraphics.BackgroundColor;
            List<KeyValuePair<string, ConsoleKey>> Keys = this.KeyControls.Export();
            while (true)
            {
                Console.Clear();
                for(int i = 0;i<= Options.Count - 1; i++)
                {
                    Console.SetCursorPosition(this.ConsoleGraphics.Position.X, this.ConsoleGraphics.Position.Y+i);
                    if(Selection == i)
                    {
                        Console.Write(Selector);
                    }
                    Console.Write(Options[i].Key);
                }
                ConsoleKeyInfo Input = Console.ReadKey();
                string InputString = Keys.Find(x => x.Value == Input.Key).Key;
                switch (InputString)
                {
                    case "Select":
                        break;
                    case "Up":
                        Selection--;
                        break;
                    case "Down":
                        Selection++;
                        break;
                    default:
                        break;
                }
                //overrolls the selector to top/bottom if selection was outside the options
                if (Selection >= Options.Count)
                {
                    Selection = 0;
                }
                if (Selection <= -1)
                {
                    Selection = Options.Count - 1;
                }
            }
        }
        /// <summary>
        /// Defines <code>ConsoleKey</code>s used for controls
        /// </summary>
        public class Controls
        {
            /// <summary>
            /// Exports pairs of keys with descriptions
            /// </summary>
            /// <returns>A list of keys with descriptions</returns>
            public List<KeyValuePair<string, ConsoleKey>> Export()
            {
                List<KeyValuePair<string, ConsoleKey>> KeyList = new();
                Type type = this.GetType();
                PropertyInfo[] Props = type.GetProperties();
                foreach(PropertyInfo Prop in Props)
                {
                    ConsoleKey[] keys = (ConsoleKey[]) type.GetProperty(Prop.Name).GetValue(this);
                    foreach (ConsoleKey key in keys)
                    {
                        KeyList.Add(new KeyValuePair<string, ConsoleKey>(Prop.Name, key));
                    }
                }
                return KeyList;
            }
            /// <summary>
            /// <code>ConsoleKey</code> used for selecting options
            /// </summary>
            public ConsoleKey[] Select { get; set; } = { ConsoleKey.Enter };
            /// <summary>
            /// <code>ConsoleKey</code> used for moving up the list
            /// </summary>
            public ConsoleKey[] Up { get; set; } = { ConsoleKey.UpArrow, ConsoleKey.W };
            /// <summary>
            /// <code>ConsoleKey</code> used for moving down the list
            /// </summary>
            public ConsoleKey[] Down { get; set; } = { ConsoleKey.DownArrow, ConsoleKey.S };
        }
        #region Graphics
        /// <summary>
        /// Contains uselful settings for styling of elements
        /// </summary>
        public class Graphics
        {
            /// <summary>
            /// Sets Vertical Alignment of an element
            /// </summary>
            VAlignment VerticalAlignment { get; set; } = VAlignment.Top;
            /// <summary>
            /// Sets Horizontal Alignment of an element
            /// </summary>
            HAlignment HorizontalAlignment { get; set; } = HAlignment.Left;
            /// <summary>
            /// Vertical Alignment
            /// </summary>
            enum VAlignment
            {
                Top,
                Center,
                Bottom
            }
            /// <summary>
            /// Horizontal Alignment
            /// </summary>
            enum HAlignment
            {
                Left,
                Center,
                Right
            }
            /// <summary>
            /// Background color
            /// </summary>
            public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
            /// <summary>
            /// Foreground color
            /// </summary>
            public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;
            /// <summary>
            /// Position of the element
            /// </summary>
            public Coordinates Position { get; set; } = new Coordinates(1,1);
        }
        #endregion
        public struct Coordinates
        {
            public int X { get; set; }
            public int Y { get; set; }
            /// <summary>
            /// Creates coordinates from a string
            /// </summary>
            /// <param name="s">string of X and Y coordinates, separated by ,</param>
            public Coordinates(string s)
            {
                string[] vals = s.Split(',');
                X = Int32.Parse(vals[0]);
                Y = Int32.Parse(vals[1]);
            }
            /// <summary>
            /// Creates coordinates
            /// </summary>
            /// <param name="x">X coordinate</param>
            /// <param name="y">Y coordinate</param>
            public Coordinates(int x, int y)
            {
                X = x;
                Y = y;
            }
            public override string ToString()
            {
                return $"{X},{Y}";
            }
        }
    }
}
