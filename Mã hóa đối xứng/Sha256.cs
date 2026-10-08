using System;
using System.Text;
using System.Security.Cryptography;

public class Sha256 {
    // hàm hash sha256 trên mảng byte (kiểm tra tính toàn vẹn dữ liệu)
    public static string hash(byte[] bnoidung){
        byte[] hash_noidung = SHA256.HashData(bnoidung);
        return Convert.ToHexString(hash_noidung).ToLower();
    }

    // hàm hash sha256 trên chuỗi
    public static string hash(string noidung){
        return hash(Encoding.UTF8.GetBytes(noidung));
    }

    // hàm so sánh 2 hash (kiểm tra toàn vẹn)
    public static void sosanh_hash(string hash1, string hash2){
        bool sosanh = (hash1 == hash2 ? true : false);
        Console.WriteLine("\t*[Kết quả] " + (sosanh ? "Toàn vẹn" : "Không toàn vẹn"));
    }

    // hàm in hash
    public static void in_hash(string hash){
        Console.WriteLine("\t*[Hash] " + hash);
    }
}