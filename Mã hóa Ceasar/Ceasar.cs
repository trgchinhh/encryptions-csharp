using System;
using System.Text;

public class Ceasar {
    private const int SoKyTuChu = 26;
    private const int SoKyTuSo = 10;
    private const string BangSo = "0123456789";
    private const string BangChuThuong = "abcdefghijklmnopqrstuvwxyz";
    private const string BangChuHoa = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string mahoa(string noidung, int khoa){
        string banma = "";
        foreach(char c in noidung){
            int vitri = 0;
            if(c >= 'a' && c <= 'z'){
                vitri = BangChuThuong.IndexOf(c);
                banma += BangChuThuong[(vitri + khoa) % SoKyTuChu];
            } 
            else if(c >= 'A' && c <= 'Z'){
                vitri = BangChuHoa.IndexOf(c);
                banma += BangChuHoa[(vitri + khoa) % SoKyTuChu];
            }
            else if(c >= '0' && c <= '9'){
                vitri = BangSo.IndexOf(c);
                banma += BangSo[(vitri + khoa) % SoKyTuSo];
            }
            else{
                banma += c;
            }
        }
        return banma;
    }

    public string giaima(string banma, int khoa){
        string bangoc = "";
        foreach(char c in banma){
            int vitri = 0;
            if(c >= 'a' && c <= 'z'){
                vitri = BangChuThuong.IndexOf(c);
                bangoc += BangChuThuong[(vitri - khoa + SoKyTuChu) % SoKyTuChu];
            } 
            else if(c >= 'A' && c <= 'Z'){
                vitri = BangChuHoa.IndexOf(c);
                bangoc += BangChuHoa[(vitri - khoa + SoKyTuChu) % SoKyTuChu];
            }
            else if(c >= '0' && c <= '9'){
                vitri = BangSo.IndexOf(c);
                bangoc += BangSo[(vitri - khoa + SoKyTuSo) % SoKyTuSo];
            }
            else{
                bangoc += c;
            }
        }
        return bangoc;
    }

    public void mahoa_file(string duongdan_vao, string duongdan_ra, int khoa){
        if(!File.Exists(duongdan_vao)){
            Console.WriteLine("\t(!) File {0} không tồn tại !", duongdan_vao);
            return;
        }
        if(File.Exists(duongdan_ra)){
            Console.WriteLine("\t(!) File {0} đã tồn tại. Vui lòng đặt tên khác !", duongdan_ra);
            return;
        }
        string noidung = File.ReadAllText(duongdan_vao, Encoding.UTF8);
        string banma = mahoa(noidung, khoa);
        string hash_noidung = Sha256.hash(noidung);
        File.WriteAllText(duongdan_ra, banma);
        Console.WriteLine("\t(*) Đã ghi nội dung mã hóa vào file {0}", duongdan_ra);
        Console.WriteLine("\t[Hash file gốc]");
        Sha256.in_hash(hash_noidung);
    }

    public void giaima_file(string duongdan_vao, string duongdan_ra, int khoa){
        if(!File.Exists(duongdan_vao)){
            Console.WriteLine("\t(!) File {0} không tồn tại !", duongdan_vao);
            return;
        }
        if(File.Exists(duongdan_ra)){
            Console.WriteLine("\t(!) File {0} đã tồn tại. Vui lòng đặt tên khác !", duongdan_ra);
            return;
        }
        string banma = File.ReadAllText(duongdan_vao, Encoding.UTF8);        
        string bangoc = giaima(banma, khoa);
        string hash_bangoc = Sha256.hash(bangoc);
        File.WriteAllText(duongdan_ra, bangoc);
        Console.WriteLine("\t(*) Đã ghi nội dung gốc vào file {0}", duongdan_ra);
        Console.WriteLine("\t[Hash file sau giải mã]");
        Sha256.in_hash(hash_bangoc);
        Console.WriteLine("\t  (so với hash file gốc để kiểm tra tính toàn vẹn dữ liệu)");
    }

    public void in_mahoa(string banma){
        Console.Write("\t[Bản mã]\n\t");
        Mau.tomau("\t"+banma, Mau.mauvang, true);
    }

    public void in_giaima(string bangoc){
        Console.Write("\t[Bản gốc]\n\t");
        Mau.tomau("\t"+bangoc, Mau.mauvang, true);
    }
}