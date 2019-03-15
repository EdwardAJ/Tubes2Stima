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
using tw = System.Windows.Forms;
using td = System.Drawing;



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
        private bool visit = false;

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
        public bool getVisit()
        {
            return visit;
        }
        public void setVisit(bool _visit)
        {
            visit = _visit;
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
            //show();
        }
        public void show() //Fungsi untuk debugging.
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
                    }
                }
                else
                {
                    Console.Write("E");
                }
                Console.Write("   ");
            }
            Console.WriteLine();
            */
        }

        public void DFSSetLevel(int num_from)
        {
            AntahBerantah[num_from].setLevel(leveliterator);
            leveliterator++;
            for (int i = 0; i < AntahBerantah[num_from].listWays().Count; i++)
            {
                int idxSearch = AntahBerantah[num_from].listWays()[i];
                if (AntahBerantah[idxSearch].getLevel() == 0) //Kalau belum dikunjungi
                {
                    DFSSetLevel(AntahBerantah[idxSearch].getNum());
                }

            }
            leveliterator--;
        }


        public bool DFS(int num_from, int num_to)
        {
            bool found = false;
            
                if (AntahBerantah[num_from].listWays().Contains(num_to)) //Kalau suatu simpul terhubung LANGSUNG dengan num_to
                {
                    urutanSimpul.Add(num_from);
                    //cek apakah simpul num_to menjauh.
                    if (AntahBerantah[num_to].getLevel() < AntahBerantah[num_from].getLevel())
                    {
                        urutanSimpul.Add(num_to);
                        foreach (int i in urutanSimpul)
                        {
                            urutanSimpulFinal.Add(i);
                        }

                        //urutanSimpul.ForEach(Console.Write) ;
                        found = true;
                        urutanSimpul.Remove(num_to);
                        /*
                        Console.Write("TesLAGI: ");
                        urutanSimpulFinal.ForEach(Console.Write);
                        */
                    }
                    urutanSimpul.Remove(num_from);
                }
                else
                
                {   //rekurens
                        for (int i = 0; i < AntahBerantah[num_from].listWays().Count; i++)
                        {
                            urutanSimpul.Add(num_from);
                            int idxSearch = AntahBerantah[num_from].listWays()[i];
                            if (AntahBerantah[idxSearch].getLevel() < AntahBerantah[num_from].getLevel()) //cek apakah simpul num_to menjauh.
                               found = found || DFS(idxSearch, num_to);
                            urutanSimpul.Remove(num_from);
                        }
                }
             return found;
        }
       

        public void eksekusi(string[] queryString, int[] queryNum, string[] answer)
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
                    //Console.WriteLine("Urutan Simpul yang Anda lalui:");
                    //urutanSimpulFinal.ForEach(Console.WriteLine);
                    urutanSimpulFinal.Clear();
                    answer[0] = "YA";
                }
                else
                {                  
                    answer[0] = "TIDAK";
                }
            }
            else if (queryNum[0] == 1)
            {
                if (DFS(queryNum[1], queryNum[2]))
                {
                    Console.WriteLine("Urutan Simpul yang Anda lalui:");
                    //urutanSimpulFinal.Reverse();
                    //urutanSimpulFinal.ForEach(Console.WriteLine);
                    urutanSimpulFinal.Clear();
                    answer[0] = "YA";
                }
                else
                {
                    answer[0] = "TIDAK";
                }
            }
            else
            {
                Console.WriteLine("Query harus sesuai format");
                MessageBox.Show("Query harus sesuai format");
                answer[0] = "ERROR";
            }
        }
        public void insertQuery()
        {
            //Procedure yang dijalankan apabila query dari file
            //StreamReader queryFile = new StreamReader(@"D:\Kuliah Semester 4\Strategi Algoritma\Tubes 2\Tubes2Stima\PreTubes\PreTubes\bin\Debug\query.txt"); // File Query
            StreamReader queryFile = new StreamReader(@"D:\INFORMATIKA ITB\Semester 4\IF2211 - Strategi Algoritma\TUBES2\Tubes2Stima\PreTubes\PreTubes\bin\Debug\query50k.txt");
            //StreamWriter answerFile = new StreamWriter(@"D:\Kuliah Semester 4\Strategi Algoritma\Tubes 2\Tubes2Stima\PreTubes\PreTubes\bin\Debug\answer.txt"); // File Answer
            StreamWriter answerFile = new StreamWriter(@"D:\INFORMATIKA ITB\Semester 4\IF2211 - Strategi Algoritma\TUBES2\Tubes2Stima\PreTubes\PreTubes\bin\Debug\answer_100k.txt"); // File Answer
            string temp = queryFile.ReadLine();
            string[] answer = new string[1]; // Hasil jawaban (Ya atau Tidak), untuk ditulis di file
            int n = int.Parse(temp);
            for (int i = 0; i < n; i++)
            {
                temp = queryFile.ReadLine();
                string[] queryString = temp.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int[] queryNum = new int[queryString.Length];
                eksekusi(queryString, queryNum, answer);
                answerFile.WriteLine(temp + " : " + answer[0]);
            }
            queryFile.Close();
            answerFile.Close();
        }
    }
    public partial class MainWindow : Window
    {
        public AntahBerantahClass AB;
        public float zoom = 1f;
        public int size = 0;
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
            size = 400 / AB.AntahBerantah.Count();
            MessageBox.Show("Map loaded");
            
            //this.InvalidateVisual();
            TesDraw(size,size);
            
            // Connect the Paint event of the PictureBox to the event handler method.
            //Graf.Paint += new System.Windows.Forms.PaintEventHandler(this.TesDraw);
            // Add the PictureBox control to the Form.
            //this.Controls.Add(TesDraw);

            /*
            RectangleF bounds = new RectangleF(x, y, width, height);
            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                graphicsObj.DrawText("Number", SystemFonts.Default, Brushes.Black, bounds, format);
            }
            */

        }
        private void Insert_Query_File_Click(object sender, RoutedEventArgs e)
        {
            //Procedure yang dijalankan apabila Button "Insert Query File" diklik
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
                    string[] answer;
                    answer = new string[1];
                    AB.eksekusi(queryString, queryNum, answer);
                    if (answer[0] != "ERROR")
                    {
                        Console.WriteLine("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\n" + answer[0]);
                        MessageBox.Show("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\n" + answer[0]);
                    }
                }
                else
                {
                    Console.WriteLine("Jangan lupa untuk Load Map!");
                    MessageBox.Show("Jangan lupa untuk Load Map!");
                }
            }
        }
        //tw.PaintEventArgs e
        
        private void TesDraw(int sizex, int sizey)
        {
            //td.Graphics graphicsObj = e.Graphics;
            td.Graphics graphicsObj = this.Graf.Child.CreateGraphics();
            //graphicsObj.ScaleTransform(zoom, zoom);
            int jum = AB.AntahBerantah.Count();
            for (int i = 1; i < AB.AntahBerantah.Count(); i++)
            {
                td.SolidBrush myBrush = new td.SolidBrush(td.Color.Blue);
                graphicsObj.FillEllipse(myBrush, new td.Rectangle((i-1)*sizex, (i-1)* sizey, sizex, sizey));
            }
            //myBrush.Dispose();
            //graphicsObj.Dispose();
        }

        
        private void TesScroll(object sender, EventArgs e)
        {
            //zoom = (float)Slider1.Value/100f;
            Graf.InvalidateVisual();
            size += (int)Slider1.Value;
            TesDraw(size, size);
        }
        
        
    }
}