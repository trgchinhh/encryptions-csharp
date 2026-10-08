using System;
using System.Text;
using System.Numerics;
using System.Collections.Generic;

public class RSA {
    // khai báo các biến cần dùng
    private BigInteger p, q, n, phi, e, d;
    private string khoacongkhai_base64 = "";
    private string khoabimat_base64 = "";

    private BigInteger gcd(BigInteger a, BigInteger b){
        while(b != 0){
            BigInteger tam = a % b;
            a = b;
            b = tam;
        }
        return a;
    }

    private BigInteger nghichdaomodulo(BigInteger e, BigInteger phi){
        BigInteger a = e, b = phi;
        BigInteger x0 = 1, x1 = 0;
        while(b != 0){
            BigInteger q = a / b;
            BigInteger tam = a % b;
            a = b;
            b = tam;
            tam = x0 - q * x1;
            x0 = x1;
            x1 = tam;
        }
        x0 %= phi;
        if(x0 < 0){
            x0 += phi;
        }
        return x0;
    }

    private BigInteger luythuamodulo(BigInteger a, BigInteger b, BigInteger mod){
        BigInteger ketqua = 1;
        while(b > 0){
            if(b % 2 == 1){
                ketqua = ketqua * a % mod;
            }
            a = a * a % mod;
            b /= 2;
        }
        return ketqua;
    }

    public void sinhcapkhoa(int sobit = 128){  // số bit mặc định là 128
        Sinh sinh = new Sinh();
        Random random = new Random();
        Console.WriteLine("Đang sinh cặp khóa {0} bit ...", sobit);
        int bit = sobit / 2;
        Console.WriteLine("\t┌───> Public key: {0} bit", bit);
        Console.WriteLine("\t└───> Private key: {0} bit", bit);
        e = 65537;
        do {
            p = sinh.sinhnguyento(bit, random);
            q = sinh.sinhnguyento(bit, random);
            phi = (p - 1) * (q - 1);
        } while(p == q || gcd(e, phi) != 1);
        n = p * q;
        d = nghichdaomodulo(e, phi);
        string khoacongkhai = e + ":" + n;
        string khoabimat = d + ":" + n;
        khoacongkhai_base64 = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(khoacongkhai)
        );
        khoabimat_base64 = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(khoabimat)
        );
        Console.WriteLine("[*] Đã sinh cặp khóa {0} bit", sobit);
    }

    public string mahoa(string noidung){
        byte[] bnoidung = Encoding.UTF8.GetBytes(noidung);
        string ketqua = "";
        for(int i = 0; i < bnoidung.Length; i++){
            ketqua += luythuamodulo(bnoidung[i], e, n) + " ";
        }
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(ketqua));
    }

    public void in_mahoa(string banma){
        Console.Write("\t[Bản mã]\n\t");
        Mau.tomau("\t"+banma, Mau.mauvang, true);
    }

    public string giaima(string banma){
        byte[] bnoidung = Convert.FromBase64String(banma);
        string str_banma = Encoding.UTF8.GetString(bnoidung);
        string[] danhsachkytu = str_banma.Trim().Split(' ');
        byte[] ketqua = new byte[danhsachkytu.Length];
        for(int i = 0; i < danhsachkytu.Length; i++){
            ketqua[i] = (byte)luythuamodulo(BigInteger.Parse(danhsachkytu[i]), d, n);
        }
        return Encoding.UTF8.GetString(ketqua);
    }

    public void in_giaima(string bangoc){
        Console.Write("\t[Bản gốc]\n\t");
        Mau.tomau("\t"+bangoc, Mau.mauvang, true);
    }

    private string thugonkhoa(string khoa, int sokytu){
        string mini = "";
        for(int i = 0; i < sokytu; i++){
            mini += khoa[i];
        }
        mini += " ... ";
        for(int i = khoa.Length - sokytu; i < khoa.Length; i++){
            mini += khoa[i];
        }
        return mini;
    }

    public void xemcapkhoa(int sokytu){
        string khoacongkhai_base64_thugon = thugonkhoa(khoacongkhai_base64, sokytu);
        string khoabimat_base64_thugon = thugonkhoa(khoabimat_base64, sokytu);
        Console.Write("\t[*] Public key: ");
        Mau.tomau(khoacongkhai_base64_thugon, Mau.mauxanhla, true);
        Console.Write("\t[*] Private key: ");
        Mau.tomau(khoabimat_base64_thugon, Mau.mauxanhla, true);

        Console.Write("\n\t(?) Ghi khóa vào file (y/n): ");
        string luachon = Console.ReadLine()!;
        if(luachon == "y"){
            string thumuc_filekhoa = "keys";
            Directory.CreateDirectory(thumuc_filekhoa);
            File.WriteAllText(
                thumuc_filekhoa+"/public.key", 
                khoacongkhai_base64
            );
            File.WriteAllText(
                thumuc_filekhoa+"/private.key",
                khoabimat_base64
            );
            Console.WriteLine("\t(*) Đã tạo thư mục chứa khóa !");
        } else {
            Console.WriteLine("\t(!) Không tạo thư mục chứa khóa !");
        }
    }

    public void mahoa_file(string duongdan_vao, string duongdan_ra){
        if(!File.Exists(duongdan_vao)){
            Console.WriteLine("\t(!) File {0} không tồn tại !", duongdan_vao);
            return;
        }
        if(File.Exists(duongdan_ra)){
            Console.WriteLine("\t(!) File {0} đã tồn tại. Vui lòng đặt tên khác !", duongdan_ra);
            return;
        }
        string noidung = File.ReadAllText(duongdan_vao);
        string hash_noidung = Sha256.hash(noidung);
        string banma = mahoa(noidung);
        File.WriteAllText(duongdan_ra, banma);
        Console.WriteLine("\t(*) Đã ghi nội dung mã hóa vào file {0}", duongdan_ra);
        Console.WriteLine("\t[Hash file gốc]");
        Sha256.in_hash(hash_noidung);
    }

    public void giaima_file(string duongdan_vao, string duongdan_ra){
        if(!File.Exists(duongdan_vao)){
            Console.WriteLine("\t(!) File {0} không tồn tại !", duongdan_vao);
            return;
        }
        if(File.Exists(duongdan_ra)){
            Console.WriteLine("\t(!) File {0} đã tồn tại. Vui lòng đặt tên khác !", duongdan_ra);
            return;
        }
        string banma = File.ReadAllText(duongdan_vao);
        string noidunggoc = giaima(banma);
        File.WriteAllText(duongdan_ra, noidunggoc);
        string hash_noidunggoc = Sha256.hash(noidunggoc);
        Console.WriteLine("\t(*) Đã ghi nội dung gốc vào file {0}", duongdan_ra);
        Console.WriteLine("\t[Hash file sau giải mã]");
        Sha256.in_hash(hash_noidunggoc);
        Console.WriteLine("\t  (so với hash file gốc để kiểm tra tính toàn vẹn dữ liệu)");
    }
}