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
        public int getElmt(int i)
        {
            return ways[i];
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
        public List<int> urutanSimpul = new List<int>();
        public List<int> urutanSimpulFinal = new List<int>();
        public int[] integers;
        public List<House> AntahBerantah;
        public int leveliterator = 1;
        public void GetFile(string FilePath)
        {
            //Path File
            string fileContent = File.ReadAllText(@FilePath);
            string[] integerStrings = fileContent.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            //Menyimpan nilai integer sebanyak jumlah angka di dalam File
            integers = new int[integerStrings.Length];
            for (int n = 0; n < integerStrings.Length; n++)
            {
                integers[n] = int.Parse(integerStrings[n]);
            }
        }
        public AntahBerantahClass(string FilePath)
        {
            GetFile(FilePath);
            AntahBerantah = new List<House>();
            for (int i = 0; i <= integers[0]; i++)
            {
                House h = new House(i);
                AntahBerantah.Add(h);
            }
            for (int i = 1; i < integers.Length; i += 2)
            {
                AntahBerantah[integers[i]].addWays(integers[i + 1]);
                AntahBerantah[integers[i + 1]].addWays(integers[i]);
            }
            show();
        }
        public void show() //Fungsi untuk debugging.
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
            Console.WriteLine();
        }

        public void DFSSetLevel(int num_from) //Prosedur untuk setLevel setiap simpul.
        {
            AntahBerantah[num_from].setLevel(leveliterator);
            leveliterator++;
            AntahBerantah[num_from].addWays(0);
            int i = 0;
            int idxSearch;
            while (AntahBerantah[num_from].getElmt(i) != 0)
            {//untuk setiap rumah (rumahSearch) yang terhubung dengan rumah yang sedang diperiksa (rumahNow)
                idxSearch = AntahBerantah[num_from].getElmt(i);
                if (AntahBerantah[idxSearch].getLevel() == 0) //kalau (rumahSearch) belum dikunjungi
                {
                    DFSSetLevel(AntahBerantah[idxSearch].getNum()); //lakukan setLevel pada rumah tersebut
                    i++;
                }
                else //kalau (rumahSearch) sudah dikunjungi
                {
                    AntahBerantah[num_from].removeWays(idxSearch); //hapus dari daftar rumah yang terhubung dengan (rumahNow)
                }
            }
            AntahBerantah[num_from].removeWays(0);
            leveliterator--;
        }

        public bool DFS(int num_from, int num_to)
        {
            bool found = false;
            if (AntahBerantah[num_from].listWays().Contains(num_to)) //Kalau suatu simpul terhubung LANGSUNG dengan num_to
            {
                urutanSimpul.Add(num_from);
                urutanSimpul.Add(num_to);
                foreach (int i in urutanSimpul)
                {
                    urutanSimpulFinal.Add(i);
                }
                found = true;
                urutanSimpul.Remove(num_to);
                urutanSimpul.Remove(num_from);
            }
            else
            {   //rekurens
                foreach (int idxSearch in AntahBerantah[num_from].listWays())
                {
                    urutanSimpul.Add(num_from);
                    found = found || DFS(idxSearch, num_to);
                    urutanSimpul.Remove(num_from);
                }
            }
            return found;
        }
        public void eksekusi(string[] queryString, int[] queryNum)
        {
            for (int i = 0; i < queryString.Length; i++)
            {
                queryNum[i] = int.Parse(queryString[i]);
            }

            //Conditinal untuk angka pertama pada query : '0' atau '1':
            if (queryNum[0] == 0)
            {
                if (DFS(queryNum[2], queryNum[1])) //DFS(from,to)
                {
                    Console.WriteLine("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nYA");
                    MessageBox.Show("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nYA");
                    Console.WriteLine("Urutan Simpul yang Anda lalui:");
                    urutanSimpulFinal.ForEach(Console.WriteLine);
                    urutanSimpulFinal.Clear();
                }
                else
                {
                    Console.WriteLine("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nTIDAK");
                    MessageBox.Show("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nTIDAK");
                }
            }
            else if (queryNum[0] == 1)
            {
                if (DFS(queryNum[1], queryNum[2]))
                {
                    Console.WriteLine("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nYA");
                    MessageBox.Show("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nYA");
                    Console.WriteLine("Urutan Simpul yang Anda lalui:");
                    urutanSimpulFinal.Reverse();
                    urutanSimpulFinal.ForEach(Console.WriteLine);
                    urutanSimpulFinal.Clear();
                }
                else
                {
                    Console.WriteLine("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nTIDAK");
                    MessageBox.Show("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nTIDAK");
                }
            }
            else
            {
                Console.WriteLine("Query harus sesuai format");
                MessageBox.Show("Query harus sesuai format");
            }
        }
        public void insertQuery()
        {
            StreamReader sr = new StreamReader(@"D:\Kuliah Semester 4\Strategi Algoritma\Tubes 2\Tubes2Stima\PreTubes\PreTubes\bin\Debug\query.txt"); // File Query
            string temp = sr.ReadLine(); 
            int n = int.Parse(temp);
            for (int i = 0; i < n; i++)
            {
                temp = sr.ReadLine();
                string[] queryString = temp.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int[] queryNum = new int[queryString.Length];
                eksekusi(queryString, queryNum);
            }
            sr.Close();
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
            string FilePath = PathFile.Text;
            AB = new AntahBerantahClass(FilePath);
            AB.DFSSetLevel(1); //DFS from node "1" to ALL OF THE CONNECTED NODES to set the level.
            AB.show();
            MessageBox.Show("Map loaded");
        }
        private void Insert_Query_File_Click(object sender, RoutedEventArgs e)
        {
            if (AB != null)
            {
                AB.insertQuery();
                Console.WriteLine("Insert Query selesai dieksekusi");
                MessageBox.Show("Insert Query selesai dieksekusi");
            }
            else
            {
                Console.WriteLine("Jangan lupa untuk Load Map!");
                MessageBox.Show("Jangan lupa untuk Load Map!");
            }
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
                }
                else
                {
                    Console.WriteLine("Query harus sesuai format");
                    MessageBox.Show("Query harus sesuai format");
                }
            }
            else
            {
                if (AB != null) //KALAU MAPNYA SUDAH DILOAD
                {
                    AB.eksekusi(queryString, queryNum);
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