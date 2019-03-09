#include <bits/stdc++.h>

using namespace std;

#define MOD 1000000007
#define ll long long int
#define ld long double
#define pb push_back
#define mkp make_pair

//This is vector of array, mirip matriks namun dinamis.
vector <int> v[100001];
int tim = 1; //Urutan pengunjungan
int ta[100001], td[100001]; //Tabel urutan pengunjungan
//ta untuk mengunjungi tahap awal (urutan push menuju stack)
//td untuk mengunjungi tahap akhir (urutan pop dari stack)

void dfs(int x)
{
	//urutan pengunjungan ditambah
	cout << "Simpul ke " << x << endl;
	ta[x] = tim; //ketika sudah mencapai simpul x, masukkan urutan pengunjungan simpul tersebut
	tim++;
	//dengan kata lain, mark as visited dan catat.
	//pada kode di bawah,
	//size menyatakan jumlah simpul yang terhubung dengan simpul x
	for (int i = 0; i < v[x].size(); ++i) {
		if (!ta[v[x][i]]){
			dfs(v[x][i]);
		} //kalau belum visited
		//dfs lagi secara rekursif.
	}
	tim--;
	//tim++; //tambahkan urutan pengunjungan
	//td[x] = tim; //ketika simpul sudah di"pop" / backtrack, catat urutannya.
}

int main()
{
	int n, m, i, j, k, a, b, c, x, y;
	cin >> n;
	//pb adalah push_back.
	//inisialisasi graf
	for (i = 1; i < n; ++i) {
		scanf("%d %d", &x, &y);
		v[x].pb(y);
		v[y].pb(x);
	}
	//dfs dari simpul pertama.
	dfs(1);

/*
	cout << "ISI TA:" << endl;
	for (int i = 1 ; i < 10 ; i++) {
		cout << "Simpul ke- " << i << ": ";
		cout << ta[i] << endl;
	}
	*/

	/*
	cout << " ISI TD:" << endl;
	for (int i = 1 ; i < 10 ; i++) {
		cout << "Simpul ke- " << i << ": ";
		cout << td[i] << endl;
	}
	*/


	cin >> m;
	while (m--) {
		scanf("%d %d %d", &a, &b, &c);
		if (a == 1) swap(c, b);
		if (ta[b] <= ta[c] && td[c] <= td[b]) printf("YES\n");
		else printf("NO\n");
	}



	return 0;
}
