using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;


namespace PreTubes
{
    public class House
    {
        private int num;
        private int level;
        private List<int> ways;
        public House(int n)
        {
            num = n;
            ways = new List<int>();
        }
        public int getNum()
        {
            return num;
        }
        public int getLevel()
        {
            return level;
        }
        public void setNum(int n)
        {
            num = n;
        }
        public void setLevel(int l)
        {
            level = l;
        }
        public List<int> listWays()
        {
            return ways;
        }
        public void addWays(int input)
        {
            ways.Add(input);
        }
        public void removeWays(int input)
        {
            ways.Remove(input);
        }
    }
    public class AntahBerantahClass
    {
        private int[] integers;
        private House king;
        private List<House> AntahBerantah;
        public void GetFile()
        {
            string fileContent = File.ReadAllText(@"C:\Users\FtN\source\repos\PreTubes\PreTubes\bin\Debug\test.txt");
            string[] integerStrings = fileContent.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            integers = new int[integerStrings.Length];
            for (int n = 0; n < integerStrings.Length; n++)
            {
                integers[n] = int.Parse(integerStrings[n]);
            }
        }
        public void show()
        {
            foreach (House h in AntahBerantah)
            {
                Console.Write(h.getNum());
                Console.Write("_");
                Console.Write(h.getLevel());
                Console.Write("_");
                if (h.listWays().Count != 0)
                {
                    foreach (int way in h.listWays())
                    {
                        Console.Write(way);
                    }
                }
                else
                {
                    Console.Write("E");
                }
                Console.Write("   ");
            }
            Console.Write("\n");
        }
        public int searchIdxHouse(int num)
        {
            int i;
            bool found = false;
            for (i = 0; i < AntahBerantah.Count && !found; i++)
            {
                if (AntahBerantah[i].getNum() == num)
                {
                    found = true;
                }
            }
            if (!found)
            {
                i = -1;
            }
            return i - 1;
        }
        public AntahBerantahClass()
        {
            GetFile();
            AntahBerantah = new List<House>();
            for (int i = 1; i <= integers[0]; i++)
            {
                House h = new House(i);
                AntahBerantah.Add(h);
            }
            for (int i = 1; i < integers.Length; i += 2)
            {
                AntahBerantah[integers[i] - 1].addWays(integers[i + 1]);
                AntahBerantah[integers[i + 1] - 1].addWays(integers[i]);
            }
            foreach (House h in AntahBerantah)
            {
                h.setLevel(999);
            }
            show();
        }
        public void SortAntahBerantah()
        {
            AntahBerantah[0].setLevel(0);
            king = AntahBerantah[0];
            foreach (int num in AntahBerantah[0].listWays())
            {
                foreach (House h in AntahBerantah)
                {
                    if (h.getNum() == num)
                    {
                        h.setLevel(1);
                    }
                }
            }
            AntahBerantah.Remove(king);
            AntahBerantah.Add(king);
            show();
            int levelnow = 1;
            bool error = false;
            while (!AntahBerantah[0].Equals(king) && !error)
            {
                int idx = 0;
                while (AntahBerantah[idx] != king)
                {
                    if (AntahBerantah[idx].getLevel() == levelnow)
                    {
                        AntahBerantah[idx].addWays(0);
                        int idxway = 0;
                        while (AntahBerantah[idx].listWays()[idxway] != 0)
                        {
                            int idxHouse = searchIdxHouse(AntahBerantah[idx].listWays()[idxway]); //get idxHouse from num in ways
                            if (AntahBerantah[idxHouse].getLevel() < levelnow)
                            {
                                AntahBerantah[idx].removeWays(AntahBerantah[idx].listWays()[idxway]);
                            }
                            else if (AntahBerantah[idxHouse].getLevel() == levelnow)
                            {
                                error = true;
                            }
                            else
                            {
                                AntahBerantah[idxHouse].setLevel(levelnow + 1);
                                idxway++;
                            }
                        }
                        AntahBerantah[idx].removeWays(0);
                        AntahBerantah.Add(AntahBerantah[idx]);
                        AntahBerantah.Remove(AntahBerantah[idx]);
                    }
                    else
                    {
                        idx++;
                    }
                }
                levelnow++;
                show();
            }
        }
        public bool DFS(int num_from, int num_to)
        {
            bool found = false;
            int idx = searchIdxHouse(num_from);
            House from = AntahBerantah[idx];
            if (from.listWays().Contains(num_to))
            {
                found = true;
            }
            else
            {
                foreach(int way in from.listWays())
                {
                    found = found || DFS(way, num_to);
                }
            }
            return found;
        }
    }
    public partial class MainWindow : Window
    {
        public AntahBerantahClass AB;
        public MainWindow()
        {
            Console.WriteLine("Press any key to exit.");
            System.Console.Read();
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            AB = new AntahBerantahClass();
            AB.SortAntahBerantah();
            MessageBox.Show("MAP LOADED");
        }
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            string[] queryString = Query.Text.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int[] queryNum = new int[queryString.Length];
            for(int i = 0; i<queryString.Length; i++)
            {
                queryNum[i] = int.Parse(queryString[i]);
            }
            if (queryNum[0] == 0)
            {
                if(AB.DFS(queryNum[1], queryNum[2]))
                {
                    Console.WriteLine("YEY");
                    MessageBox.Show("YEY");
                }
                else
                {
                    Console.WriteLine("NAI");
                    MessageBox.Show("NAI");
                }
            }
            else if(queryNum[0] == 1)
            {
                if (AB.DFS(queryNum[2], queryNum[1]))
                {
                    Console.WriteLine("YEY");
                    MessageBox.Show("YEY");
                }
                else
                {
                    Console.WriteLine("NAI");
                    MessageBox.Show("NAI");
                }
            }
            else
            {
                Console.WriteLine("ANONE");
                MessageBox.Show("ANONE");
            }   
        }       
    }
}