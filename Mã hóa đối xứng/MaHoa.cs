using System;
using System.Text;
using System.Security.Cryptography;

public class MaHoa {
    // hàm dịch bit sang trái (đẩy về bên phải khi hết chổ)
    public static byte ROTL(byte x, int r){
        r %= 8; // mặc định làm việc 8 bit 
        if(r == 0) return x;
        return (byte)((x << r) | (x >> (8 - r)));
    }

    // hàm mã hóa 
    public static byte[] mahoa(string noidung, string khoa1, string khoa2, int vonglap){
        byte[] bkhoa1 = Encoding.UTF8.GetBytes(khoa1);
        byte[] bkhoa2 = Encoding.UTF8.GetBytes(khoa2);
        byte[] bnoidung = Encoding.UTF8.GetBytes(noidung);
        byte[] bvanbanmahoa = new byte[bnoidung.Length];
        for (int i = 0; i < bnoidung.Length; i++){
            bvanbanmahoa[i] = bnoidung[i];
        }
        for(int k = 1; k <= vonglap; k++){
            for(int i = 0; i < bnoidung.Length; i++){
                bvanbanmahoa[i] ^= bkhoa1[(i+k) % bkhoa1.Length];
                bvanbanmahoa[i] = (byte)~bvanbanmahoa[i];
                bvanbanmahoa[i] = ROTL(bvanbanmahoa[i], 3);
                bvanbanmahoa[i] ^= bkhoa2[(i+k) % bkhoa2.Length];
            }  
        }
        return bvanbanmahoa;
    }

    // hàm in mã hóa 
    public static void in_mahoa(byte[] bvanbanmahoa){
        string base64_bvanbanmahoa = Convert.ToBase64String(bvanbanmahoa);
        Console.WriteLine("\n\t*[Mã hóa] " + base64_bvanbanmahoa);
        
    }
}