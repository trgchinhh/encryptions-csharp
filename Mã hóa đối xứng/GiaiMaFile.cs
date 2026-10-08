using System;
using System.IO;

public class GiaiMaFile {
    // hàm giải mã file 
    // đọc nội dung đã mã hóa từ file vào và giải mã ra file xuất kèm hash để check bảo toàn nội dung file 
    public static void giaima_file(
        string duongdan_vao,
        string duongdan_ra, 
        string noidung, string khoa1, string khoa2, int vonglap){
        if(!File.Exists(duongdan_vao)){
            Console.WriteLine("File {0} không tồn tại !", duongdan_vao);
            return;
        }
        if(File.Exists(duongdan_ra)){
            Console.WriteLine("File {0} đã tồn tại. Vui lòng đặt tên khác !", duongdan_ra);
            return;
        }
        noidung = File.ReadAllText(duongdan_vao);
        string noidunggiaima = GiaiMa.giaima(noidung, khoa1, khoa2, vonglap);
        string hash_noidunggiaima = Sha256.hash(noidunggiaima);
        File.WriteAllText(duongdan_ra, noidunggiaima);
        Console.WriteLine("\n\t[Hash file sau giải mã] ");
        Sha256.in_hash(hash_noidunggiaima);
        Console.WriteLine("\t  (so với hash file gốc lúc mã hóa để kiểm tra toàn vẹn dữ liệu)");
    }
}