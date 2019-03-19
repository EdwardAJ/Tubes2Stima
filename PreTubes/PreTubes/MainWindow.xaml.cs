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
using ta = System.Windows.Forms.Integration;
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
        private int top;
        private int level_ID = 0; //Menentukan indeks X rumah pada suatu level

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
        public int getLevel_ID()
        {
            return level_ID;
        }
        public int getTop()
        {
            return top;
        }
        public void setNum(int n)
        {
            num = n;
        }
        public void setLevel(int l)
        {
            level = l;
        }
        public void setLevel_ID(int ID)
        {
            level_ID = ID;
        }
        public void setTop(int t)
        {
            top = t;
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
        }
        public void DFSSetLevel(int num_from)
        {
            AntahBerantah[num_from].setLevel(leveliterator);
            arrID[leveliterator]++;
            AntahBerantah[num_from].setLevel_ID(arrID[leveliterator]);
            leveliterator++;
            foreach(int idxSearch in AntahBerantah[num_from].listWays())
            {
                if (AntahBerantah[idxSearch].getLevel() == 0) //Kalau belum dikunjungi
                {
                    DFSSetLevel(AntahBerantah[idxSearch].getNum());
                }
                if (AntahBerantah[idxSearch].getLevel() == leveliterator - 2)
                {
                    AntahBerantah[num_from].setTop(idxSearch);
                }
            }
            AntahBerantah[num_from].removeWays(AntahBerantah[num_from].getTop());
            leveliterator--;
        }
    }
    public partial class MainWindow : Window
    {
        public AntahBerantahClass AB;
        public List<int> urutanSimpul = new List<int>();
        public List<int> urutanSimpulFinal = new List<int>();
        public float size = 0;
        public float updown_pan = 0; //Translasi graf terhadap sumbu y
        public float ratio = 5; //Rasio adalah skala perbesaran saat slider digeser
        public string queueQuery; //Query File yang ngantri untuk diklik next
        public string pathQueryFile;
        public bool found;
        public bool canDraw;

        //Deklarasi brush, untuk membuat lingkaran.
        public td.SolidBrush myBrush = new td.SolidBrush(td.ColorTranslator.FromHtml("#002638"));

        //Deklarasi brush, untuk membuat font.
        public td.SolidBrush myFontBrush = new td.SolidBrush(td.ColorTranslator.FromHtml("#00deff"));

        //Deklarasi brush, untuk mengganti warna font.
        public td.SolidBrush myFontBrushNew = new td.SolidBrush(td.ColorTranslator.FromHtml("#00ff66"));

        //Deklarasi Pen, untuk membuat garis yang menghubungkan dari node ke node.
        public td.Pen myPen = new td.Pen(td.ColorTranslator.FromHtml("#00deff"), 2);

        //Deklarasi Pen, untuk membuat stroke pada setiap lingkaran.
        public td.Pen PenStroke = new td.Pen(td.ColorTranslator.FromHtml("#00deff"), 3);

        //Deklarasi Pen, untuk mengganti warna garis yang menghubungkan dari node ke node.
        public td.Pen myPenNew = new td.Pen(td.ColorTranslator.FromHtml("#00ff66"), 2);

        //Deklarasi Pen, untuk mengganti warna stroke.
        public td.Pen PenStrokeNew  = new td.Pen(td.ColorTranslator.FromHtml("#00ff66"), 3);

        //Inisialisasi font yang digunakan
        public td.Font drawFont;

        //Inisialisasi font yang digunakan UNTUK menunjukkan step.
        public td.Font drawFontStep;

        public MainWindow(){}
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            string FilePath = PathFile.Text;
            if (FilePath != "")
            {
                AB = new AntahBerantahClass(FilePath);
                AB.DFSSetLevel(1); //DFS from node "1" to ALL OF THE CONNECTED NODES to set the level.
                size = (float)400 / AB.AntahBerantah.Count() + ratio * (float)Slider1.Value;
                MessageBox.Show("Map loaded");
                this.Graf.Child.Refresh();
                DFSDraw(1, 1);
            }
            else
            {
                MessageBox.Show("Isi dulu path map nya!");
            }
        }
        public void DrawPath (bool found)
        {
            //this.Graf.Child.Refresh();
            td.Graphics graphicsObj = this.Graf.Child.CreateGraphics();
            if (!found)
            {
                myFontBrushNew = new td.SolidBrush(td.ColorTranslator.FromHtml("#ff4646"));
                myPenNew = new td.Pen(td.ColorTranslator.FromHtml("#ff4646"), 2);
                PenStrokeNew = new td.Pen(td.ColorTranslator.FromHtml("#ff4646"), 3);
            } else
            {
                myFontBrushNew = new td.SolidBrush(td.ColorTranslator.FromHtml("#00ff66"));
                myPenNew = new td.Pen(td.ColorTranslator.FromHtml("#00ff66"), 2);
                PenStrokeNew = new td.Pen(td.ColorTranslator.FromHtml("#00ff66"), 3);
            }
            for (int i = 0; i < urutanSimpulFinal.Count(); i++)
            {
                //ABlevel untuk menentukan posisi y nantinya, menggunakan atribut level pada House
                int ABlevel = AB.AntahBerantah[urutanSimpulFinal[i]].getLevel();
                //ABlevelx untuk menentukan posisi x nantinya, menggunakan atribut level_ID pada House.
                int ABlevelx = AB.AntahBerantah[urutanSimpulFinal[i]].getLevel_ID();
                int posx_from = (int)(AB.AntahBerantah[urutanSimpulFinal[i]].getLevel_ID()); //posisi x asal
                int posy_from = (int)(AB.AntahBerantah[urutanSimpulFinal[i]].getLevel()) - 1; //posisi y asal

                td.Rectangle rect1 = new td.Rectangle(ABlevelx * 400 / (AB.arrID[ABlevel] + 1), (int)(((ABlevel - 1) * 2 * size) - updown_pan), (int)size, (int)size);

                if (i != urutanSimpulFinal.Count() - 1)
                {
                    int ABlevelto = AB.AntahBerantah[urutanSimpulFinal[i + 1]].getLevel();
                    int ABlevelxto = AB.AntahBerantah[urutanSimpulFinal[i + 1]].getLevel_ID();
                    int posx_to = (int)(AB.AntahBerantah[urutanSimpulFinal[i + 1]].getLevel_ID()); //posisi x menuju
                    int posy_to = (int)(AB.AntahBerantah[urutanSimpulFinal[i + 1]].getLevel()) - 1; // posisi y menuju

                    int x_from = (int)(posx_from * 400 / (AB.arrID[posy_from + 1] + 1) + 0.5 * size);
                    int y_from = (int)(posy_from * 2 * size + 0.5 * size);
                    int x_to = (int)(posx_to * 400 / (AB.arrID[posy_to + 1] + 1) + 0.5 * size);
                    int y_to = (int)(posy_to * 2 * size + 0.5 * size);

                    // (Urutan gambar #1)
                    //Gambar Line dari posisi from ke posisi posisi to
                    //graphicsObj.DrawLine(myPenNew, x_from, y_from, x_to, y_to);
                }
                // (Urutan gambar #2 )
                //Membuat outline dari lingkaran
                graphicsObj.DrawEllipse(PenStrokeNew, rect1);

                // (Urutan gambar #3)
                //Menggambar lingkaran dengan radius size (sizex) dan (sizey)
                graphicsObj.FillEllipse(myBrush, rect1);

                if (size > 0)
                    drawFont = new td.Font("Avenir Next LT Pro", size / 2, td.FontStyle.Bold);
                else
                    drawFont = new td.Font("Avenir Next LT Pro", 1, td.FontStyle.Bold);

                if (size > 0)
                    drawFontStep = new td.Font("Avenir Next LT Pro", size / 3, td.FontStyle.Bold);
                else
                    drawFontStep = new td.Font("Avenir Next LT Pro", (float)0.5, td.FontStyle.Bold);

                // (Urutan gambar #4)
                //Font diset pada posisi node.
                if (size > 1)
                {
                    graphicsObj.DrawString(urutanSimpulFinal[i].ToString(), drawFont, myFontBrushNew, (ABlevelx * 400 / (AB.arrID[ABlevel] + 1) + (float)(size / 4)), (ABlevel - 1) * 2 * size + (float)(size / 6) - updown_pan);
                    graphicsObj.DrawString((i + 1).ToString(), drawFontStep, myFontBrushNew, (ABlevelx * 400 / (AB.arrID[ABlevel] + 1) - (float)(size / 2)), (ABlevel - 1) * 2 * size + (float)(size / 6) - updown_pan);
                }

            }
        }

        public string EksekusiQuery(string[] queryString, int[] queryNum, bool isDraw)
        {
            string answer;
            bool result;
            for (int i = 0; i < queryString.Length; i++)
            {
                queryNum[i] = int.Parse(queryString[i]);
            }
            //Conditional untuk angka pertama pada query : '0' atau '1':
            if (queryNum[0]==0 || queryNum[0] == 1)
            {

                if (queryNum[0] == 0)
                    result = Mendekat(queryNum[1], queryNum[2]); // Mendekat(num_to, num_from)
                else
                    result = Menjauh(queryNum[1], queryNum[2]); // Menjauh(num_to, num_from)
                    
                foreach (int i in urutanSimpul)
                {
                    urutanSimpulFinal.Add(i);
                }
                if (result)
                {
                    if (isDraw)
                    {
                        this.Graf.Child.Refresh();
                        DFSDraw(1, 1);
                        found = true;
                        DrawPath(found);
                    } else
                    {
                       urutanSimpulFinal.Clear();
                    }
                    answer = "YA";
                }
                else
                {
                    if (isDraw)
                    {
                        this.Graf.Child.Refresh();
                        DFSDraw(1, 1);
                        found = false;
                        DrawPath(found);
                    } else
                    {
                       urutanSimpulFinal.Clear();
                    }
                
                    answer = "TIDAK";
                }
                
            }
            else
            {
                Console.WriteLine("Query harus sesuai format");
                MessageBox.Show("Query harus sesuai format");
                answer = "ERROR";
            }
            return answer;
        }
        public void insertQuery()
        {//Procedure yang dijalankan apabila query dari file
            pathQueryFile = FileQuery.Text;
            if (pathQueryFile != "")
            {
                string answer = ""; // Hasil jawaban (Ya atau Tidak), untuk ditulis di file
                StreamReader queryFile = new StreamReader(pathQueryFile); // File Query
                //Generate path file answer
                string pathAnswerFile = "answer.txt";
                StreamWriter answerFile = new StreamWriter(pathAnswerFile); // File Answer
                string temp = queryFile.ReadLine();
                int n = int.Parse(temp);
                for (int i = 0; i < n; i++)
                {
                    temp = queryFile.ReadLine();
                    queueQuery = temp;
                    string[] queryString = temp.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    int[] queryNum = new int[queryString.Length];
                    answer = EksekusiQuery(queryString, queryNum, false);
                    urutanSimpul.Clear();
                    urutanSimpulFinal.Clear();
                    answerFile.WriteLine(temp + " : " + answer);
                }
                queryFile.Close();
                answerFile.Close();
                queueQuery = "Ready"; // Dengan ini, button "Next" bisa dieksekusi
                DFSDraw(1, 1); // Reload tanpa draw path
                canDraw = false;
            }
            else
            {
                Console.WriteLine("Isi dulu path query nya ya!");
                MessageBox.Show("Isi dulu path query nya ya!");
            }
        }
        private void Insert_Query_File_Click(object sender, RoutedEventArgs e)
        {
            //Procedure yang dijalankan apabila Button "Insert Query File" diklik
            if (AB != null)
            {
                insertQuery();
                if (pathQueryFile != "")
                {
                    Console.WriteLine("Jawaban query selesai ditulis di file");
                    MessageBox.Show("Jawaban query selesai ditulis di file");
                }
            }
            else
            {
                Console.WriteLine("Jangan lupa untuk Load Map!");
                MessageBox.Show("Jangan lupa untuk Load Map!");
            }
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            canDraw = true;
            string temp;
            urutanSimpul.Clear();
            urutanSimpulFinal.Clear();
            if (pathQueryFile == null)
            {
                Console.WriteLine("Jangan lupa untuk Insert Query!");
                MessageBox.Show("Jangan lupa untuk Insert Query!");
            }
            else
            {
                StreamReader queryFile = new StreamReader(pathQueryFile);
                temp = queryFile.ReadLine(); //Baca baris pertama
                temp = queryFile.ReadLine(); //Baca query selanjutnya
                if (queueQuery == "Ready") //Jika sudah ready, dari fungsi insertQuery()
                {
                    queueQuery = temp;
                }
                else
                {
                    while (temp != null && temp != queueQuery)
                    {
                        temp = queryFile.ReadLine();
                    }
                    temp = queryFile.ReadLine();
                    if (temp == null)
                    {
                        this.Graf.Child.Refresh();
                        //Console.WriteLine("Semua query dalam file sudah diproses");
                        MessageBox.Show("Semua query dalam file sudah diproses");
                        DFSDraw(1, 1);
                    }
                    else
                    {
                        queueQuery = temp;
                    }
                }
                queryFile.Close();
                if (temp != null)
                {
                    string[] queryString = queueQuery.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    int[] queryNum = new int[queryString.Length];
                    string answer = "";
                    
                    answer = EksekusiQuery(queryString, queryNum, true);
                   // urutanSimpulFinal.ForEach(Console.Write);
                    //Console.WriteLine("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\n" + answer);
                    MessageBox.Show("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\n" + answer);
                }
            }
        }
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            //Program untuk mengecek apakah query yang diberikan dapat menghasilkan solusi
            canDraw = true;
            urutanSimpul.Clear();
            urutanSimpulFinal.Clear();
            string[] queryString = Query.Text.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int[] queryNum = new int[queryString.Length];
            //Cek apakah ada query yang diinput
            if ((queryString.Length < 3) || (queryString.Length > 3))
            {
                if (queryString.Length == 0)
                {
                    //Console.WriteLine("Query tidak boleh kosong");
                    MessageBox.Show("Query tidak boleh kosong");
                }
                else
                {
                    //Console.WriteLine("Query harus sesuai format");
                    MessageBox.Show("Query harus sesuai format");
                }
            }
            else
            {
                if (AB != null) //KALAU MAPNYA SUDAH DILOAD
                {
                    string answer = EksekusiQuery(queryString, queryNum, true);
                    if (answer != "ERROR")
                    {
                        //Console.WriteLine("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\n" + answer);
                        MessageBox.Show("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\n" + answer);
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
        public void DFSDraw(int num_from, int num_prec)
        {
            //Traversal untuk mencari SEMUA TETANGGA dari node.
            foreach (int idxSearch in AB.AntahBerantah[num_from].listWays())
            {
                //Lakukan DFS lagi pada tetangga.
                if (AB.AntahBerantah[idxSearch].getLevel() > AB.AntahBerantah[num_from].getLevel())
                    DFSDraw(AB.AntahBerantah[idxSearch].getNum(), num_from);
            }

            //Deklarasi variabel graphics
            td.Graphics graphicsObj = this.Graf.Child.CreateGraphics();

            //ABlevel untuk menentukan posisi y nantinya, menggunakan atribut level pada House
            int ABlevel = AB.AntahBerantah[num_from].getLevel();

            //ABlevelx untuk menentukan posisi x nantinya, menggunakan atribut level_ID pada House.
            int ABlevelx = AB.AntahBerantah[num_from].getLevel_ID();

            //Buat rectangle yang akan dipakai untuk method draw dan fill.
            td.Rectangle rect = new td.Rectangle(ABlevelx * 400 / (AB.arrID[ABlevel] + 1), (int)(((ABlevel - 1) * 2 * size) - updown_pan), (int)size, (int)size);

            //Cek apakah simpul asal bukan 1
            if (num_from != 1)
            {
                //Deklarasi untuk menentukan posisi drawLine
                int posx_from = (int)(AB.AntahBerantah[num_from].getLevel_ID()); //posisi x asal
                int posx_to = (int)(AB.AntahBerantah[num_prec].getLevel_ID()); //posisi x menuju
                int posy_from = (int)(AB.AntahBerantah[num_from].getLevel()) - 1; //posisi y asal
                int posy_to = (int)(AB.AntahBerantah[num_prec].getLevel()) - 1; // posisi y menuju
                // (Urutan gambar #1)
                //Gambar Line dari posisi from ke posisi posisi to
                graphicsObj.DrawLine(myPen, (int)(posx_from * 400 / (AB.arrID[posy_from + 1] + 1) + 0.5 * size), (int)(posy_from * 2 * size + 0.5 * size) - updown_pan, (int)(posx_to * 400 / (AB.arrID[posy_to + 1] + 1) + 0.5 * size), (int)(posy_to * 2 * size + 0.5 * size) - updown_pan);
            }
            // (Urutan gambar #2 )
            //Membuat outline dari lingkaran
            graphicsObj.DrawEllipse(PenStroke, rect);

            // (Urutan gambar #3)
            //Menggambar lingkaran dengan radius size (sizex) dan (sizey)
            graphicsObj.FillEllipse(myBrush, new td.Rectangle(ABlevelx * 400 / (AB.arrID[ABlevel] + 1), (int)(((ABlevel - 1) * 2 * size) - updown_pan), (int)size, (int)size));

            if (size > 0)
                drawFont = new td.Font("Avenir Next LT Pro", size / 2, td.FontStyle.Bold);
            else
                drawFont = new td.Font("Avenir Next LT Pro", 1, td.FontStyle.Bold);

            // (Urutan gambar #4)
            //Font diset pada posisi node.
            if (size > 1)
                graphicsObj.DrawString(num_from.ToString(), drawFont, myFontBrush, (ABlevelx * 400 / (AB.arrID[ABlevel] + 1) + (float)(size / 4)), (ABlevel - 1) * 2 * size - updown_pan + (float)(size / 6));
        }

        //Method untuk mem"binding" nilai dari scroll untuk zoom.
        public void TesScroll(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AB != null)
            {
                float initial_size = 400 / AB.AntahBerantah.Count();
                //Digunakan untuk merefresh UI
                this.Graf.Child.Refresh();

                // Zoom sesuai skala slider
                size = initial_size + ratio * (float)Slider1.Value;
                // Gambar lagi!
                DFSDraw(1, 1);
                if (canDraw)
                    DrawPath(found);
            }
        }

        //Method untuk mengatur panning atas-bawah
        public void UpDown_Slider(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AB != null)
            {
                this.Graf.Child.Refresh();
                updown_pan = (float)(1) * (AB.AntahBerantah.Count) * (float)UpDownSlider.Value * (float)Slider1.Value;
                DFSDraw(1, 1);
                if (canDraw)
                    DrawPath(found);
            }
        }

        public bool Mendekat(int num_to, int num_from)
        {
            bool found = false;
            urutanSimpul.Add(num_from);
            if (AB.AntahBerantah[num_from].getTop() == num_to)
            {//Kalau suatu simpul terletak di bawah num_to
                urutanSimpul.Add(num_to);
                found = true;
            }
            else if (AB.AntahBerantah[num_from].getTop() != 0)
            {//recc
                found = found || Mendekat(num_to, AB.AntahBerantah[num_from].getTop());
            }
            return found;
        }
        public bool Menjauh(int num_to, int num_from)
        {
            bool found = false;
            urutanSimpul.Add(num_from);
            if (num_from==num_to )
            {//Kalau ketemu dan titik pada query berbeda
                found = urutanSimpul.Count != 1;
            }
            else
            {//recc
                foreach (int way in AB.AntahBerantah[num_from].listWays())
                {//Sekaligus kasus basis, jika listWays kosong tetap return found = false
                    found = found || Menjauh(num_to, way);
                }
            }
            return found;
        }
    }
}