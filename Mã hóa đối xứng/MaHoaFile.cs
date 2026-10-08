using System;
using System.IO;

public class MaHoaFile {
    // hàm mã hóa file 
    // đọc nội dung từ file vào và mã hóa thông tin ghi vào file xuất 
    public static void mahoa_file(
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
        string hash_noidung = Sha256.hash(noidung);
        byte[] vanbanmahoa = MaHoa.mahoa(noidung, khoa1, khoa2, vonglap);
        string base64_vanbanmahoa = Convert.ToBase64String(vanbanmahoa);
        File.WriteAllText(duongdan_ra, base64_vanbanmahoa);
        
        Console.WriteLine("Đã ghi nội dung mã hóa vào file {0}", duongdan_ra);
        Console.WriteLine("\n\t[Hash file gốc] ");
        Sha256.in_hash(hash_noidung);
    }
}