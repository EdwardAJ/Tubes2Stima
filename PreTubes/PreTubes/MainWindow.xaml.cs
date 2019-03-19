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
//Beberapa "using" tambahan
using winintegrate = System.Windows.Forms.Integration;
using winform = System.Windows.Forms;
using draw = System.Drawing;


namespace Tubes2Stima_AingCupu
{
    /*
     * Didefinisikan class:
     * House (rumah dengan elemennya)
     * AntahBerantah (negeri dengan elemennya): Elemen AntahBerantah berupa List of House.
     * MainWindow : UI/UX
     */
    public class House
    {
        private int num; //Nomor rumah.
        private int level = 0; //Level diset ke 0 semua saat inisialisasi.
        private List<int> ways; //List tetangga simpul.
        private int top; // Menentukan atas dari simpul.
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
        public int[] arrID = new int[100001]; //Digunakan untuk tahu pada level ke-i terdapat berapa jumlah simpul.
        public int[] integers; //Digunakan untuk menyimpan query.
        public List<House> AntahBerantah; //List of List, representasi TREE.
        public int leveliterator = 1; //level awal diset ke 1.
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

        /*
         * Konstruktor untuk membuat graf. Konstruktor menerima parameter NAMA FILE */
        public AntahBerantahClass(string FilePath)
        {
            GetFile(FilePath);
            AntahBerantah = new List<House>();
            for (int i = 0; i <= integers[0]; i++)
            {
                House h = new House(i);
                AntahBerantah.Add(h); //Menambah rumah
            }
            for (int i = 1; i < integers.Length; i += 2)
            {
                AntahBerantah[integers[i]].addWays(integers[i + 1]); //Menambah tetangga
                AntahBerantah[integers[i + 1]].addWays(integers[i]);
            }
        }
        /* Prosedur untuk mengeset LEVEL dan LEVEL_ID setiap simpul
         */
        public void DFSSetLevel(int num_from)
        {
            AntahBerantah[num_from].setLevel(leveliterator); //SetLevel suatu simpul.
            arrID[leveliterator]++; //Jumlah simpul pada level tertentu ditambah
            //ID setiap simpul pada suatu level. Digunakan untuk menentukan posisi x.
            AntahBerantah[num_from].setLevel_ID(arrID[leveliterator]); 
            leveliterator++;
            foreach(int idxSearch in AntahBerantah[num_from].listWays())
            {
                if (AntahBerantah[idxSearch].getLevel() == 0) //Kalau belum dikunjungi
                {
                    DFSSetLevel(AntahBerantah[idxSearch].getNum()); //Rekurens
                }
                if (AntahBerantah[idxSearch].getLevel() == leveliterator - 2)
                {
                    AntahBerantah[num_from].setTop(idxSearch); //Set "bapak" dari simpul.
                }
            }
            //Remove "BAPAK" dari semua simpul.
            AntahBerantah[num_from].removeWays(AntahBerantah[num_from].getTop());
            leveliterator--;
        }
    }
    public partial class MainWindow : Window
    {
        //Tree of House AB
        public AntahBerantahClass AB;
        //UrutanSimpul dan UrutanSimpulFinal dipakai untuk mencatat PATH yang dilalui secara DFS.
        public List<int> urutanSimpul = new List<int>();
        public List<int> urutanSimpulFinal = new List<int>();
        public float size = 0; //Akan dimasukkan dalam perhitungan ZOOM.
        public float updown_pan = 0; //Translasi graf terhadap sumbu y
        public float ratio = 5; //Rasio adalah skala perbesaran saat slider digeser
        public string queueQuery; //Query File yang ngantri untuk diklik next
        public string pathQueryFile;
        public bool found; //Simpul ditemukan atau tidak.
        public bool canDraw; //Menentukan apakah DRAWPATH (menggambar simpul dikunjungi secara DFS) bisa dijalankan.

        //Deklarasi brush, untuk membuat lingkaran.
        public draw.SolidBrush myBrush = new draw.SolidBrush(draw.ColorTranslator.FromHtml("#002638"));

        //Deklarasi brush, untuk membuat font.
        public draw.SolidBrush myFontBrush = new draw.SolidBrush(draw.ColorTranslator.FromHtml("#00deff"));

        //Deklarasi brush, untuk mengganti warna font.
        public draw.SolidBrush myFontBrushNew = new draw.SolidBrush(draw.ColorTranslator.FromHtml("#00ff66"));

        //Deklarasi Pen, untuk membuat garis yang menghubungkan dari node ke node.
        public draw.Pen myPen = new draw.Pen(draw.ColorTranslator.FromHtml("#00deff"), 2);

        //Deklarasi Pen, untuk membuat stroke pada setiap lingkaran.
        public draw.Pen PenStroke = new draw.Pen(draw.ColorTranslator.FromHtml("#00deff"), 3);

        //Deklarasi Pen, untuk mengganti warna garis yang menghubungkan dari node ke node.
        public draw.Pen myPenNew = new draw.Pen(draw.ColorTranslator.FromHtml("#00ff66"), 2);

        //Deklarasi Pen, untuk mengganti warna stroke.
        public draw.Pen PenStrokeNew  = new draw.Pen(draw.ColorTranslator.FromHtml("#00ff66"), 3);

        //Inisialisasi font yang digunakan
        public draw.Font drawFont;

        //Inisialisasi font yang digunakan UNTUK menunjukkan step.
        public draw.Font drawFontStep;

        public MainWindow(){}

        /*
         * Method untuk meLoad File 
         */
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            string FilePath = PathFile.Text;
            if (FilePath != "")
            {
                AB = new AntahBerantahClass(FilePath);
                AB.DFSSetLevel(1); //DFS from node "1" to ALL OF THE CONNECTED NODES to set the level.
                //Ukuran setiap simpul saat nantinya digambar.
                size = (float)400 / AB.AntahBerantah.Count() + ratio * (float)Slider1.Value;
                MessageBox.Show("Map loaded");
                this.Graf.Child.Refresh(); //Update GUI.
                DFSDraw(1, 1); //Menggambar GRAF secara DFS.
            }
            else
            {
                MessageBox.Show("Isi dulu path map nya!"); //Kalau MAP belum diload.
            }
        }

        /* Prosedur menggambar PATH yang dikunjungi secara DFS */
        public void DrawPath (bool found)
        {
            //BAGIAN 1 : INISIALISASI VARIABEL.
            draw.Graphics graphicsObj = this.Graf.Child.CreateGraphics();
            if (!found)
            {
                myFontBrushNew = new draw.SolidBrush(draw.ColorTranslator.FromHtml("#ff4646"));
                myPenNew = new draw.Pen(draw.ColorTranslator.FromHtml("#ff4646"), 2);
                PenStrokeNew = new draw.Pen(draw.ColorTranslator.FromHtml("#ff4646"), 3);
            } else
            {
                myFontBrushNew = new draw.SolidBrush(draw.ColorTranslator.FromHtml("#00ff66"));
                myPenNew = new draw.Pen(draw.ColorTranslator.FromHtml("#00ff66"), 2);
                PenStrokeNew = new draw.Pen(draw.ColorTranslator.FromHtml("#00ff66"), 3);
            }
            for (int i = 0; i < urutanSimpulFinal.Count(); i++)
            {
                //ABlevel untuk menentukan posisi y nantinya, menggunakan atribut level pada House
                int ABlevel = AB.AntahBerantah[urutanSimpulFinal[i]].getLevel();
                //ABlevelx untuk menentukan posisi x nantinya, menggunakan atribut level_ID pada House.
                int ABlevelx = AB.AntahBerantah[urutanSimpulFinal[i]].getLevel_ID();
                int posx_from = (int)(AB.AntahBerantah[urutanSimpulFinal[i]].getLevel_ID()); //posisi x asal
                int posy_from = (int)(AB.AntahBerantah[urutanSimpulFinal[i]].getLevel()) - 1; //posisi y asal

                draw.Rectangle rect1 = new draw.Rectangle(ABlevelx * 400 / (AB.arrID[ABlevel] + 1), (int)(((ABlevel - 1) * 2 * size) - updown_pan), (int)size, (int)size);

                if (i != urutanSimpulFinal.Count() - 1)
                {
                    int ABlevelto = AB.AntahBerantah[urutanSimpulFinal[i + 1]].getLevel();
                    int ABlevelxto = AB.AntahBerantah[urutanSimpulFinal[i + 1]].getLevel_ID();
                    int posx_to = (int)(AB.AntahBerantah[urutanSimpulFinal[i + 1]].getLevel_ID()); //posisi x menuju
                    int posy_to = (int)(AB.AntahBerantah[urutanSimpulFinal[i + 1]].getLevel()) - 1; // posisi y menuju

                    //400 adalah panjang panel. Posisi x adalah panjang panel / (jumlah simpul pada suatu level tertentu + 1) + radius simpul.
                    int x_from = (int)(posx_from * 400 / (AB.arrID[posy_from + 1] + 1) + 0.5 * size);
                    int y_from = (int)(posy_from * 2 * size + 0.5 * size);
                    int x_to = (int)(posx_to * 400 / (AB.arrID[posy_to + 1] + 1) + 0.5 * size);
                    int y_to = (int)(posy_to * 2 * size + 0.5 * size);

                    // BAGIAN 2 : IMPLEMENTASI GAMBAR

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

                if (size > 0) //Karena ada scroll, ada kemungkinan size bisa di bawah 0.
                    drawFont = new draw.Font("Avenir Next LT Pro", size / 2, draw.FontStyle.Bold);
                else
                    drawFont = new draw.Font("Avenir Next LT Pro", 1, draw.FontStyle.Bold);

                if (size > 0)
                    drawFontStep = new draw.Font("Avenir Next LT Pro", size / 3, draw.FontStyle.Bold);
                else
                    drawFontStep = new draw.Font("Avenir Next LT Pro", (float)0.5, draw.FontStyle.Bold);

                // (Urutan gambar #4)
                //Font diset pada posisi node.
                if (size > 1)
                {
                    graphicsObj.DrawString(urutanSimpulFinal[i].ToString(), drawFont, myFontBrushNew, (ABlevelx * 400 / (AB.arrID[ABlevel] + 1) + (float)(size / 4)), (ABlevel - 1) * 2 * size + (float)(size / 6) - updown_pan);
                    graphicsObj.DrawString((i + 1).ToString(), drawFontStep, myFontBrushNew, (ABlevelx * 400 / (AB.arrID[ABlevel] + 1) - (float)(size / 2)), (ABlevel - 1) * 2 * size + (float)(size / 6) - updown_pan);
                }

            }
        }

        /*
         * Fungsi untuk menjalankan Query yang diinput user dengan memanggil DFS. 
         */
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
                    urutanSimpulFinal.Add(i); //Masukkan path ke dalam urutanSimpulFinal.
                }
                //Kondisi jika simpul yang dicari ditemukan!
                if (result)
                {
                    if (isDraw) //Kalau perlu untuk digambar.
                    {
                        this.Graf.Child.Refresh();
                        DFSDraw(1, 1);
                        found = true;
                        DrawPath(found);
                    } else
                    {
                       urutanSimpulFinal.Clear(); //Bersihkan path. 
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

        //Procedure yang dijalankan apabila query dari file
        public void insertQuery()
        {
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
                canDraw = false; //Path yang ditelusuri melalui DFS tidak perlu digambar
            }
            else
            {
                //Jika file query kosong.
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
            canDraw = true; //Path yang ditelusuri dari DFS dapat digambar.
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
                        this.Graf.Child.Refresh(); //Update UI
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
                    MessageBox.Show("Jawaban pertanyaan " + queryString[0] + " " + queryString[1] + " " + queryString[2] + " :\n" + answer);
                }
            }
        }

        //Method yang dijalankan saat tombol "Check" diklik.
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            canDraw = true; //Path yang ditelusuri melalui DFS perlu digambar
            urutanSimpul.Clear();
            urutanSimpulFinal.Clear();
            string[] queryString = Query.Text.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int[] queryNum = new int[queryString.Length];
            //Cek apakah ada query yang diinput
            if ((queryString.Length < 3) || (queryString.Length > 3))
            {
                if (queryString.Length == 0)
                {
                    MessageBox.Show("Query tidak boleh kosong");
                }
                else
                {
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
            draw.Graphics graphicsObj = this.Graf.Child.CreateGraphics();

            //ABlevel untuk menentukan posisi y nantinya, menggunakan atribut level pada House
            int ABlevel = AB.AntahBerantah[num_from].getLevel();

            //ABlevelx untuk menentukan posisi x nantinya, menggunakan atribut level_ID pada House.
            int ABlevelx = AB.AntahBerantah[num_from].getLevel_ID();

            //Buat rectangle yang akan dipakai untuk method draw dan fill.
            draw.Rectangle rect = new draw.Rectangle(ABlevelx * 400 / (AB.arrID[ABlevel] + 1), (int)(((ABlevel - 1) * 2 * size) - updown_pan), (int)size, (int)size);

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
            graphicsObj.FillEllipse(myBrush, new draw.Rectangle(ABlevelx * 400 / (AB.arrID[ABlevel] + 1), (int)(((ABlevel - 1) * 2 * size) - updown_pan), (int)size, (int)size));

            if (size > 0)
                drawFont = new draw.Font("Avenir Next LT Pro", size / 2, draw.FontStyle.Bold);
            else
                drawFont = new draw.Font("Avenir Next LT Pro", 1, draw.FontStyle.Bold);

            // (Urutan gambar #4)
            //Font diset pada posisi node.
            if (size > 1)
                graphicsObj.DrawString(num_from.ToString(), drawFont, myFontBrush, (ABlevelx * 400 / (AB.arrID[ABlevel] + 1) + (float)(size / 4)), (ABlevel - 1) * 2 * size - updown_pan + (float)(size / 6));
        }

        //Method untuk mem"binding" nilai dari slider di bawah panel untuk zoom.
        public void Zoom_Slider(object sender, RoutedPropertyChangedEventArgs<double> e)
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

        //Method untuk mengatur panning atas-bawah dengan slider.
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

        /*
         * Method untuk melakukan pencarian secara mendekat
         */
        public bool Mendekat(int num_to, int num_from)
        {
            bool found = false;
            urutanSimpul.Add(num_from);
            //Basis : Kalau suatu simpul terletak di bawah num_to
            if (AB.AntahBerantah[num_from].getTop() == num_to)
            {
                urutanSimpul.Add(num_to);
                found = true;
            }
            else if (AB.AntahBerantah[num_from].getTop() != 0)
            {//recc
                found = found || Mendekat(num_to, AB.AntahBerantah[num_from].getTop());
            }
            return found;
        }

        /*
         * Method untuk melakukan pencarian secara menjauh
         */
        public bool Menjauh(int num_to, int num_from)
        {
            bool found = false;
            urutanSimpul.Add(num_from);
            if (num_from==num_to )
            {   //Kalau ketemu dan titik pada query berbeda
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