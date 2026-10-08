using System;
using System.Text;

public class GiaiMa {
    // hàm dịch bit sang phải (đẩy về bên trái khi hết chổ)
    public static byte ROTR(byte x, int r){
        r %= 8; // mặc định làm việc 8 bit 
        if(r == 0) return x;
        return (byte)((x >> r) | (x << (8 - r)));
    }

    // hàm giải mã 
    public static string giaima(string vanbanmahoa, string khoa1, string khoa2, int vonglap){
        byte[] bvanbanmahoa = Convert.FromBase64String(vanbanmahoa);
        byte[] bkhoa1 = Encoding.UTF8.GetBytes(khoa1);
        byte[] bkhoa2 = Encoding.UTF8.GetBytes(khoa2);
        for(int k = vonglap; k > 0; k--){
            for(int i = 0; i < bvanbanmahoa.Length; i++){
                bvanbanmahoa[i] ^= bkhoa2[(i+k) % bkhoa2.Length];
                bvanbanmahoa[i] = ROTR(bvanbanmahoa[i], 3);
                bvanbanmahoa[i] = (byte)~bvanbanmahoa[i];
                bvanbanmahoa[i] ^= bkhoa1[(i+k) % bkhoa1.Length];
            }
        }
        return Encoding.UTF8.GetString(bvanbanmahoa);
    }

    // hàm in kết quả giải mã
    public static void in_giaima(string noidunggiaima){
        Console.WriteLine("\t*[Giải mã] " + noidunggiaima);
    }
}