using System;
using System.Threading;
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
    /*
     * Didefinisikan class:
     * House (rumah dengan elemennya)
     * AntahBerantah (negeri dengan elemennya): Elemen AntahBerantah berupa List of House.
     * MainWindow : UI/UX
     */
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
        public int[] integers;
        public House king;
        public House[] AntahBerantah;
        //public List<House> AntahBerantah;
        public int leveliterator = 1;
        public void GetFile()
        {
            //Path File
            string fileContent = File.ReadAllText(@"D:\INFORMATIKA ITB\Semester 4\IF2211 - Strategi Algoritma\TUBES 2\PreTubes\PreTubes\bin\Debug\test50k.txt");
            //string fileContent = File.ReadAllText(@"C:\Users\FtN\source\repos\PreTubes\PreTubes\bin\Debug\test.txt");
            string[] integerStrings = fileContent.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            //Menyimpan nilai integer sebanyak jumlah angka di dalam File
            integers = new int[integerStrings.Length];
            for (int n = 0; n < integerStrings.Length; n++)
            {
                integers[n] = int.Parse(integerStrings[n]);
            }
        }
        public AntahBerantahClass()
        {
            GetFile();
           //AntahBerantah = new List<House>();
            AntahBerantah = new House[100005];
            /*
            for (int i = 1; i <= integers[0]; i++) //Traversal dari 1 hingga mencapai jumlah "path" pada file
            {
                //House h = new House(i); //Buat rumah baru
              
                //AntahBerantah.Add(h);
            }
            */

            
            for (int i = 1; i < 100005; i++)
            {
                AntahBerantah[i] = new House(i);
            }
            
            for (int i = 1; i < integers.Length; i += 2)
            {
                AntahBerantah[integers[i]].addWays(integers[i + 1]);
                AntahBerantah[integers[i + 1]].addWays(integers[i]);
           
                //AntahBerantah[integers[i] - 1].addWays(integers[i + 1]); //Menambah path dari rumah ke-i ke rumah i+1
                //AntahBerantah[integers[i + 1] - 1].addWays(integers[i]); //Menambah path dari rumah ke-i+1 ke rumah i
                //i +=2 karena dalam satu baris terdapat 2 rumah
            }
            /*
            foreach (House h in AntahBerantah)
            {
                h.setLevel(999);
            }
            */
            show();
        }
        public void show()
        {

                /*
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
                            Console.Write(" ");
                        }
                    }
                    else
                    {
                       // Console.Write("E");
                    }
                   // Console.Write("   ");
                }
                //Console.Write("\n");
                */
                /*
                for (int i = 1; i < 10; i++)
                {
                    Console.Write(AntahBerantah[i].getNum());
                    Console.Write("_");
                    Console.Write(AntahBerantah[i].getLevel());
                    //Console.Write("_");
                    Console.Write(": ");
                
                    foreach (int way in AntahBerantah[i].listWays())
                    {
                        Console.Write(way);
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
                */

        }
        /*
        public int searchIdxHouse(int num)
        {
            int i;
            bool found = false;
            //Count = menghitung jumlah "elemen"
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
        /*
        
        public void SortAntahBerantah()
        {
            //Istana raja adalah rumah beralamat 1
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
        */

        public void DFSSetLevel(int num_from)
        {
            //int idx = searchIdxHouse(num_from);
            AntahBerantah[num_from].setLevel(leveliterator);
            leveliterator++;
            for (int i = 0; i < AntahBerantah[num_from].listWays().Count; i++)
            {
                int idxSearch = AntahBerantah[num_from].listWays()[i];
                if (AntahBerantah[idxSearch].getLevel() == 0)
                {
                    DFSSetLevel(AntahBerantah[idxSearch].getNum());
                }
                
            }
            leveliterator--;
        }

        public bool DFS(int num_from, int num_to)
        {
            /*
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
            */
            bool found = false;
            //int idx = searchIdxHouse(num_from);
            //House from = AntahBerantah[idx];
            if (AntahBerantah[num_from].listWays().Contains(num_to))
            {
                if ( AntahBerantah[num_to].getLevel() < AntahBerantah[num_from].getLevel())
                    found = true;
            }
            else
            {
                for (int i = 0; i < AntahBerantah[num_from].listWays().Count; i++)
                {
                    int idxSearch = AntahBerantah[num_from].listWays()[i];
                    if (AntahBerantah[idxSearch].getLevel()< AntahBerantah[num_from].getLevel())
                    {
                        found = found || DFS(idxSearch, num_to);
                    }

                }

                /*
                foreach (int way in AntahBerantah[num_from].listWays())
                {
                    //int idxWay = searchIdxHouse(way);
                    if (AntahBerantah[idxWay].getLevel() < AntahBerantah[num_from].getLevel())
                        found = found || DFS(way, num_to);
                }
                */
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
            //var stackSize = 10000000;
            //Thread thread = new Thread((unused => Load_Click(1,1)),stackSize);
            //thread.Start(42);
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            AB = new AntahBerantahClass();
            
            //AB.DFSSetLevel(1);
            //AB.SortAntahBerantah();
            AB.show();
            MessageBox.Show("Map loaded");

        }
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            //Program untuk mengecek apakah query yang diberikan dapat menghasilkan solusi
            string[] queryString = Query.Text.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int[] queryNum = new int[queryString.Length];
            //Cek apakah ada query yang diinput
            if ((queryString.Length < 3) || (queryString.Length > 3))
            {
                if (queryString.Length == 0)
                {
                    Console.WriteLine("Query tidak boleh kosong");
                    MessageBox.Show("Query tidak boleh kosong");
                } else
                {
                    Console.WriteLine("Query harus sesuai format");
                    MessageBox.Show("Query harus sesuai format");
                }
                
            } else
            {
                if (AB != null) //KALAU MAPNYA SUDAH DILOAD
                {
                    for (int i = 0; i < queryString.Length; i++)
                    {
                        queryNum[i] = int.Parse(queryString[i]);
                    }

                    //Conditinal untuk angka pertama pada query : '0' atau '1':
                    if (queryNum[0] == 0)
                    {
                        if (AB.DFS(queryNum[2], queryNum[1])) //DFS(from,to)
                        {
                            Console.WriteLine("YA");
                            MessageBox.Show("YA");
                        }
                        else
                        {
                            Console.WriteLine("TIDAK");
                            MessageBox.Show("TIDAK");
                        }
                    }
                    else if (queryNum[0] == 1)
                    {
                        
                        if (AB.DFS(queryNum[1], queryNum[2]))
                        {
                            Console.WriteLine("YA");
                            MessageBox.Show("YA");
                        }
                        else
                        {
                            Console.WriteLine("TIDAK");
                            MessageBox.Show("TIDAK");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Query harus sesuai format");
                        MessageBox.Show("Query harus sesuai format");
                    }
                }
                else
                {
                    Console.WriteLine("Jangan lupa untuk Load Map!");
                    MessageBox.Show("Jangan lupa untuk Load Map!");
                }
                
            }
            
        }       
    }
}