#region chú thích tài liệu
/*
────────────────────────────────────────────
   TRIỂN KHAI MÃ HÓA BẤT ĐỐI XỨNG BẰNG C#  
           TÁC GIẢ: TRƯỜNG CHINH
────────────────────────────────────────────


Chương trình nhỏ mô phỏng thuật toán mã hóa bất đối xứng RSA tự build


- Giới thiệu:
  + Mã hóa bất đối xứng: là khái niệm mã hóa sử dụng 2 khóa khác nhau gồm khóa công khai (public key) và khóa riêng tư (private key)
    - Dữ liệu được mã hóa bằng khóa công khai thì dùng khóa riêng tư để giải mã
    - Khóa công khai có thể chia sẻ cho mọi người
      + Trong hệ thống blockchain nó được hiểu như địa chỉ ví để mọi người có thể chuyển tiền cho bạn 
    - Khóa riêng tư phải được giữ bí mật
      + Nó như mật khẩu vì chỉ có nó mới có thể giải mã nội dung mà khóa công khai mã hóa 
  => Vì thế nó được xem là mã hóa bất đối xứng (ngược nhau)

  + Hàm băm (hash): là khái niệm 1 chiều, độ dài không đổi tùy thuật toán sẽ cho ra độ dài cố định khác nhau
    - Dùng để kiểm tra tính toàn vẹn dữ liệu


- Thành phần:
  + P: nội dung (bản rõ)
  + C: nội dung sau mã hóa (bản mã)
  + p: số nguyên tố thứ nhất
  + q: số nguyên tố thứ hai
  + n: tích của p và q
  + φ(n): hàm phi Euler
  + e: khóa công khai
  + d: khóa riêng tư


- Các phép biến đổi:
  + gcd: tìm ước chung lớn nhất của 2 số
  + mod: phép chia lấy dư (%)
  + nghịch đảo modulo: tìm số d sao cho (e × d) mod φ(n) = 1
  + lũy thừa modulo: tính (a^b) mod n mà không cần tính trực tiếp a^b


- Công thức tạo khóa:
  + n = p × q
  + φ(n) = (p - 1) × (q - 1)
  + gcd(e, φ(n)) = 1
  + d = e⁻¹ mod φ(n)

    -> trong đó p, q là 2 số nguyên tố khác nhau
    -> n là modulo được sử dụng trong quá trình mã hóa và giải mã
    -> φ(n) là số lượng số nguyên dương nhỏ hơn n và nguyên tố cùng nhau với n
    -> e là khóa công khai
    -> d là khóa riêng tư

  ? Vì sao e phải nguyên tố cùng nhau với φ(n)
  Vì cần tồn tại nghịch đảo modulo của e để tìm được d

  ? Vì sao phải có p và q
  Vì RSA dựa trên tích của 2 số nguyên tố lớn p × q, việc phân tích n để tìm lại p và q là bài toán khó khi số đủ lớn

  VD:
      p = 61
      q = 53
      n = p × q = 3233
      φ(n) = (61 - 1) × (53 - 1) = 3120
      Chọn e = 17
        Vì gcd(17, 3120) = 1 nên e hợp lệ
      Tìm d sao cho:
        (e × d) mod φ(n) = 1
        (17 × d) mod 3120 = 1
      → d = 2753
      (17 × 2753) mod 3120 = 1
      e = 17
      d = 2753


- Công thức mã hóa:
  + C = P^e mod n   Lấy nội dung P lũy thừa với khóa công khai e chia lấy dư với n 
  [*] Công thức chung: C = P^e mod n


- Công thức giải mã:
  + P = C^d mod n   Lấy bản mã C lũy thừa với khóa riêng tư d
  [*] Công thức chung: P = C^d mod n


- Quá trình:
  + Quá trình tạo khóa sẽ sinh ra 2 số nguyên tố lớn p và q
  + Từ p và q tính ra n và φ(n)
  + Chọn e sao cho gcd(e, φ(n)) = 1                (tính khóa công khai)
  + Từ e và φ(n) tính ra d bằng nghịch đảo modulo  (tính khóa riêng tư)
  + Khóa công khai gồm e và n  (e;n)
  + Khóa riêng tư gồm d và n   (d;n)
  + Quá trình mã hóa sử dụng khóa công khai
  + Quá trình giải mã sử dụng khóa riêng tư
  + Sau khi giải mã có thể dùng SHA-256 để kiểm tra nội dung trước và sau có giống nhau hay không


- Ví dụ:
  + Bản rõ P = 65
  + Khóa công khai: e = 17, n = 3233
  + Khóa riêng tư: d = 2753, n = 3233
  + Mã hóa:
    C = 65^17 mod 3233
    C = 2790
  + Giải mã:
    P = 2790^2753 mod 3233
    P = 65
    -> Nội dung sau khi giải mã quay trở lại đúng bản rõ ban đầu


- Lưu khóa:
  + Khóa công khai được lưu tại: keys/public.key
  + Khóa riêng tư được lưu tại: keys/private.key


- Đút kết:
  + RSA sử dụng 2 khóa khác nhau nên được gọi là mã hóa bất đối xứng
  + Khóa công khai dùng để mã hóa, khóa riêng tư dùng để giải mã
  + Điểm hay của RSA là chỉ cần gửi gửi khóa công khai của mình để mã hóa dữ liệu 
    mà không lo bên thứ 3 có thể đánh cắp và giải mã ngược lại vì độ phức tạp cực lớn 

    -> Với công thức là 2^n với n là số bit của cặp khóa 
    VD: nếu dùng cặp khóa 4096 bit thì có số trường hợp biểu diễn là 2^4096 

        MÃ HÓA     │     GIẢI MÃ
     ──────────────┼───────────────
      - khóa e     │    - khóa d
      - bản rõ P   │    - bản mã C
      - P^e mod n  │    - C^d mod n
      - ra C       │    - ra P

*/
#endregion

using Spectre.Console;

public class Program {
    public static void nhap_dodaikhoa(string banner, ref int chieudaicapkhoa){
        // danh sách độ dài khóa dựa trên tiêu chuẩn mã hóa hiện nay 
        // Demo khuyên dùng 128 -> 2048 cho nhanh 
        int[] danhsachbit_hople = {
            128, 256, 512, 1024, 2048, 3072, 4096
        };

        string menu_bit = @"
[Lưu ý] 
  - Độ bảo mật mạnh nằm ở chiều dài khóa 
  - Khóa càng dài sinh khóa và giải mã càng lâu 

[01] 128: Rất yếu
[02] 256: Rất yếu 
[03] 512: Yếu 
[04] 1024: Trung bình
[05] 2048: Mạnh 
[06] 3072: Rất mạnh
[07] 4096: Rất mạnh 
[08] Thoát 
        ";

        while(true){
            Console.Clear();
            Console.WriteLine(banner);
            Console.WriteLine("\n  MENU");
            var luachon = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                .AddChoices(1, 2, 3, 4, 5, 6, 7, 8)
                .WrapAround(true)
                .HighlightStyle(new Style(Mau.mauxanhla))
                .UseConverter(x => x switch {
                    1 => Markup.Escape("[01] 128: Rất yếu"),
                    2 => Markup.Escape("[02] 256: Rất yếu"),
                    3 => Markup.Escape("[03] 512: Yếu"),
                    4 => Markup.Escape("[04] 1024: Trung bình"),
                    5 => Markup.Escape("[05] 2048: Mạnh"),
                    6 => Markup.Escape("[06] 3072: Rất mạnh"),
                    7 => Markup.Escape("[07] 4096: Rất mạnh"),
                    8 => Markup.Escape("[08] Thoát"),
                    _ => ""
                })
            );
            if(luachon > danhsachbit_hople.Length || luachon < 1){
                if(luachon == 8){
                    Console.WriteLine("[Thoát]");
                    Environment.Exit(0);
                }
                Console.WriteLine("\t(!) Vui lòng chọn hợp lệ theo menu !");
            } 
            else {
                // nếu tìm được số bit từ lựa chọn 
                if(danhsachbit_hople.Contains(danhsachbit_hople[luachon - 1])){
                    chieudaicapkhoa = danhsachbit_hople[luachon - 1];
                    break;
                } 
                else{
                    Console.WriteLine("\t(!) Không tìm thấy chiều dài khóa hợp lệ !");
                }
            }
            dungchuongtrinh();
        }
    }

    public static bool nhap_noidung(ref string noidung){
        Console.Write("\t(?) Nhập nội dung: ");
        string str_noidung = Console.ReadLine()!;
        if(string.IsNullOrEmpty(str_noidung)){
            Console.WriteLine("\t\t(!) Không để nội dung trống !");
            return false;
        }
        noidung = str_noidung;
        return true;
    }

    public static bool nhap_duongdan(ref string duongdan_vao, ref string duongdan_ra){
        Console.Write("\t(?) Đường dẫn file vào: ");
        duongdan_vao = Console.ReadLine()!;
        if(!File.Exists(duongdan_vao)){
            Console.WriteLine("\t(!) File {0} không tồn tại !", duongdan_vao);
            return false;
        }
        Console.Write("\t(?) Đường dẫn file ra: ");
        duongdan_ra = Console.ReadLine()!;
        if(File.Exists(duongdan_ra)){
            Console.WriteLine("\t(!) File {0} đã tồn tại !", duongdan_ra);
        }
        return true;
    }

    public static void dungchuongtrinh(){
        Console.Write("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    public static void Main(){
        // biến dùng chung 
        int chieudaicapkhoa = 0;
        string noidung = "", banma = "", bangoc = "";
        string duongdan_vao = "", duongdan_ra = "";

        string banner = @"┌──────────────────────────────┐
│       Mã hóa ứng dụng        │
│ Mô phỏng mã hóa bất đối xứng │
│ Tác giả: Trường Chinh        │
│ Github: Github.com/trgchinhh │
└──────────────────────────────┘
";
        nhap_dodaikhoa(banner, ref chieudaicapkhoa);

        RSA rsa = new RSA();
        rsa.sinhcapkhoa(chieudaicapkhoa);
        dungchuongtrinh();



        while(true){
            Console.Clear();
            Console.WriteLine(banner);

            // tô màu chữ nội dung 
            Console.Write("[-] Nội dung: ");
            if(string.IsNullOrEmpty(noidung)) Mau.tomau("Chưa có", Mau.maudo, true);
            else Mau.tomau(noidung, Mau.mauxanhla, true);

            // tô màu chữ độ dài bit mạnh -> yếu 
            Console.Write("[^] Độ dài cặp khóa " + chieudaicapkhoa + " (");
            if(chieudaicapkhoa >= 3072) Mau.tomau("Rất mạnh", Mau.mauxanhla);
            else if(chieudaicapkhoa == 2048) Mau.tomau("Mạnh", Mau.mauxanhla);
            else if(chieudaicapkhoa == 1024) Mau.tomau("Trung bình ", Mau.mauvang);
            else if(chieudaicapkhoa == 512) Mau.tomau("Yếu", Mau.maucam);
            else Mau.tomau("Rất yếu", Mau.maudo);
            Console.WriteLine(")");

            Console.WriteLine("\n  MENU");
            var luachon = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                .AddChoices(1, 2, 3, 4, 5, 6, 7)
                .WrapAround(true)
                .HighlightStyle(new Style(Mau.mauxanhla))
                .UseConverter(x => x switch {
                    1 => Markup.Escape("[01] Nhập nội dung"),
                    2 => Markup.Escape("[02] Mã hóa nội dung "),
                    3 => Markup.Escape("[03] Giải mã nội dung"),
                    4 => Markup.Escape("[04] Mã hóa nội dung file"),
                    5 => Markup.Escape("[05] Giải mã nội dung file"),
                    6 => Markup.Escape("[06] Xem cặp khóa"),
                    7 => Markup.Escape("[07] Thoát"),
                    _ => ""
                })
            );

            if(luachon == 1){
                Console.WriteLine("\n[Nhập nội dung]");
                if(nhap_noidung(ref noidung)){
                    Console.WriteLine("\t(*) Đã ghi nhận nội dung mới !");
                } 
                else {
                    Console.WriteLine("\t(!) Ghi nội dung mới chưa thành công !");
                }
            }
            else if(luachon == 2){
                Console.WriteLine("\n[Mã hóa]");
                if(!string.IsNullOrEmpty(noidung)){
                    banma = rsa.mahoa(noidung);
                    rsa.in_mahoa(banma);
                }
                else {
                    Console.WriteLine("\t(!) Chưa có nội dung vui lòng chọn [1] để nhập !");
                }
            }
            else if(luachon == 3){
                Console.WriteLine("\n[Giải mã]");
                if(!string.IsNullOrEmpty(banma)){
                    bangoc = rsa.giaima(banma);
                    rsa.in_giaima(bangoc);
                }
                else {
                    Console.WriteLine("\t(!) Chưa có nội dung bản mã vui lòng chọn [2] để mã hóa !");
                }
            }
            else if(luachon == 4){
                if(nhap_duongdan(ref duongdan_vao, ref duongdan_ra)){
                    rsa.mahoa_file(duongdan_vao, duongdan_ra);
                }
            }
            else if(luachon == 5){
                if(nhap_duongdan(ref duongdan_vao, ref duongdan_ra)){
                    rsa.giaima_file(duongdan_vao, duongdan_ra);
                }
            }
            else if(luachon == 6){
                Console.WriteLine("\n[Cặp khóa]");
                rsa.xemcapkhoa(20);
            } 
            else if(luachon == 7){
                Console.WriteLine("\n[Thoát]");
                break;
            }
            else {
                Console.WriteLine("\n\t(!) Vui lòng nhập lựa chọn hợp lệ !");
            }
            dungchuongtrinh();
        }
    }
}