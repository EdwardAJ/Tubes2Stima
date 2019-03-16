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
        private int level = 0;
        private List<int> ways;
        private bool visit = false;
        private int level_ID = 0;



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
        public int getLevel_ID()
        {
            return level_ID;
        }
        public void setLevel_ID(int ID)
        {
            level_ID = ID;
        }

    }
    public class AntahBerantahClass
    {
        public List<int> urutanSimpul = new List<int>();
        public List<int> urutanSimpulFinal = new List<int>();
        public int[] arrID = new int[100001];
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
                Console.Write(h.getLevel_ID());
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
            arrID[leveliterator] += 1;
            AntahBerantah[num_from].setLevel_ID(arrID[leveliterator]);
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
                    //Console.WriteLine("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nYA");
                    //MessageBox.Show("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nYA");
                    //Console.WriteLine("Urutan Simpul yang Anda lalui:");
                    //urutanSimpulFinal.ForEach(Console.WriteLine);
                    urutanSimpulFinal.Clear();
                    answer[0] = "YA";
                }
                else
                {
                    //Console.WriteLine("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nTIDAK");
                    //MessageBox.Show("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nTIDAK");
                    answer[0] = "TIDAK";
                }
            }
            else if (queryNum[0] == 1)
            {
                if (DFS(queryNum[1], queryNum[2]))
                {
                    //Console.WriteLine("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nYA");
                    //MessageBox.Show("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nYA");
                    Console.WriteLine("Urutan Simpul yang Anda lalui:");
                    //urutanSimpulFinal.Reverse();
                    //urutanSimpulFinal.ForEach(Console.WriteLine);
                    urutanSimpulFinal.Clear();
                    answer[0] = "YA";
                }
                else
                {
                    //Console.WriteLine("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nTIDAK");
                    //MessageBox.Show("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\nTIDAK");
                    answer[0] = "TIDAK";
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
        public float size = 0;
        public double temp = 0;
        //public int sizex = 0;
        //public int sizey = 0;

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
            size = (float)400 / AB.AntahBerantah.Count();
            MessageBox.Show("Map loaded");

            //Invalidate();
            this.Graf.InvalidateVisual();
            //TesDraw((int)size,(int)size);
            DFSDraw(1, 1, size, size);

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

        //Ini adalah method untuk menggambar Grafik secara DFS.
        //Urutan gambar: Garis penghubung antar node -> Simpul (lingkaran) -> Font.
        //Tujuan pengurutan gambar: Agar font terletak di atas simpul dan simpul terletak di atas garis.
        private void DFSDraw(int num_from, int num_temp, float sizex, float sizey)
        {
            //Traversal untuk mencari SEMUA TETANGGA dari node.
            for (int i = 0; i < AB.AntahBerantah[num_from].listWays().Count; i++)
            {
                int idxSearch = AB.AntahBerantah[num_from].listWays()[i];
                //Cari dari "atas" ke "bawah", maksudnya, cari tetangga yang memiliki level lebih dalam dari node asal.
                if (AB.AntahBerantah[idxSearch].getLevel() > AB.AntahBerantah[num_from].getLevel())
                    //Lakukan DFS lagi pada tetangga.
                    DFSDraw(AB.AntahBerantah[idxSearch].getNum(), num_from, sizex, sizey);
            }

            //Deklarasi variabel graphics
            td.Graphics graphicsObj = this.Graf.Child.CreateGraphics();

            //Deklarasi brush, untuk membuat lingkaran.
            td.SolidBrush myBrush = new td.SolidBrush(td.ColorTranslator.FromHtml("#002638"));

            //Deklarasi brush, untuk membuat font.
            td.SolidBrush myFontBrush = new td.SolidBrush(td.ColorTranslator.FromHtml("#00deff"));

            //Deklarasi Pen, untuk membuat garis yang menghubungkan dari node ke node.
            td.Pen myPen = new td.Pen(td.ColorTranslator.FromHtml("#00deff"), 2);

            //Deklarasi Pen, untuk membuat stroke pada setiap lingkaran.
            td.Pen PenStroke = new td.Pen(td.ColorTranslator.FromHtml("#00deff"), 3);

            //ABlevel untuk menentukan posisi y nantinya, menggunakan atribut level pada House
            int ABlevel = AB.AntahBerantah[num_from].getLevel();

            //ABlevelx untuk menentukan posisi x nantinya, menggunakan atribut level_ID pada House.
            int ABlevelx = (AB.AntahBerantah[num_from].getLevel_ID());

            //Buat rectangle yang akan dipakai untuk method draw dan fill.
            td.Rectangle rect = new td.Rectangle(ABlevelx * 400 / (AB.arrID[ABlevel] + 1), (int)((ABlevel - 1) * 2 * sizey), (int)sizex, (int)sizey);

            //Cek apakah simpul asal bukan 1
            if (num_from != 1)
            {
                //Deklarasi untuk menentukan posisi drawLine
                int posx_from = (int)(AB.AntahBerantah[num_from].getLevel_ID()); //posisi x asal
                int posx_to = (int)(AB.AntahBerantah[num_temp].getLevel_ID()); //posisi x menuju
                int posy_from = (int)(AB.AntahBerantah[num_from].getLevel()) - 1; //posisi y asal
                int posy_to = (int)(AB.AntahBerantah[num_temp].getLevel()) - 1; // posisi y menuju
                // (Urutan gambar #1) Ggambar Line dari posisi from ke posisi posisi to
                graphicsObj.DrawLine(myPen, (int)(posx_from * 400 / (AB.arrID[posy_from + 1] + 1) + 0.5 * size), (int)(posy_from * 2 * sizey + 0.5 * size), (int)(posx_to * 400 / (AB.arrID[posy_to + 1] + 1) + 0.5 * size), (int)(posy_to * 2 * sizey + 0.5 * size));
            }
            // (Urutan gambar #2 )
            //Membuat outline dari lingkaran
            graphicsObj.DrawEllipse(PenStroke, rect);

            // (Urutan gambar #3)
            //Menggambar lingkaran dengan radius size (sizex) dan (sizey)
            graphicsObj.FillEllipse(myBrush, new td.Rectangle(ABlevelx * 400 / (AB.arrID[ABlevel] + 1), (int)((ABlevel - 1) * 2 * sizey), (int)sizex, (int)sizey));

            //Inisialisasi font yang digunakan
            td.Font drawFont = new td.Font("Avenir Next LT Pro", size / 2, td.FontStyle.Bold);

            // (Urutan gambar #4)
            //Font diset pada posisi node.
            graphicsObj.DrawString(num_from.ToString(), drawFont, myFontBrush, (ABlevelx * 400 / (AB.arrID[ABlevel] + 1) + (int)(size / 4)), (ABlevel - 1) * 2 * sizey + (int)(size / 6));
        }

        //Method untuk mem"binding" nilai dari scroll UNTUK ZOOM.
        private void TesScroll(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Digunakan untuk merefresh UI, namun masih belum berhasil refresh.
            this.Graf.InvalidateVisual();

            if (Slider1.Value > temp)
                //Zoom in
                size += (float)Slider1.Value;
            else
                size -= (float)(temp - Slider1.Value); //Zoom out

            //size = (int)Slider1.Value;
            //TesDraw((int)size, (int)size);

            // Gambar lagi!
            DFSDraw(1, 1, size, size);
            temp = (float)Slider1.Value; //Simpan temp = value slider, nantinya akan dibandingkan lagi.
        }
    }
}
