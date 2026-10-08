#region chú thích tài liệu
/*
────────────────────────────────────────────
     TRIỂN KHAI MÃ HÓA CEASAR BẰNG C#  
           TÁC GIẢ: TRƯỜNG CHINH
────────────────────────────────────────────


Chương trình nhỏ mô phỏng thuật toán mã hóa Ceasar (dịch chuyển ký tự) tự build


- Giới thiệu:
  + Mã hóa Ceasar: là thuật toán mã hóa cổ điển đơn giản nhất, do Hoàng đế La Mã Julius Caesar sáng tạo
    - Nguyên tắc: mỗi ký tự trong bản rõ được thay thế bằng ký tự đứng sau nó k vị trí trong bảng chữ cái
    - Mã hóa thì dịch tiến, giải mã thì dịch lùi đúng số vị trí đó
    - Chỉ cần 1 khóa k nhưng kết quả luôn đúng theo chiều ngược lại
  => Mỗi ký tự bị thay bằng ký tự khác theo một quy luật cố định, chỉ người có khóa k mới biết quy luật
  -> Vì dùng 1 khóa cho 2 chiều mã hóa và giải mã nên đây cũng là 1 loại mã hóa đối xứng (dùng 1 khóa) 

  + Hàm băm (hash): là khái niệm 1 chiều, độ dài không đổi tùy thuật toán sẽ cho ra độ dài cố định khác nhau
    - Dùng để kiểm tra tính toàn vẹn dữ liệu (so hash bản rõ và bản rõ sau giải mã)


- Thành phần:
  + P: nội dung (bản rõ)
  + C: nội dung sau mã hóa (bản mã)
  + k: khóa mã (số vị trí dịch chuyển mỗi ký tự)
  + n: kích thước bảng ký tự (chữ thường 26, chữ hoa 26, số 10)


- Các phép biến đổi:
  + mod: phép chia lấy dư (%)
  + dịch chuyển vòng: ký tự khi vượt quá cuối bảng sẽ quay ngược về đầu bảng


- Công thức mã hóa:
  + C = (P + k) mod n   Dịch ký tự P tiến k vị trí trong bảng
  [*] Công thức chung: C = (P + k) mod n


- Công thức giải mã:
  + P = (C - k) mod n   Dịch ký tự C lùi k vị trí trong bảng
  [*] Công thức chung: P = (C - k) mod n


- Quá trình:
  + Nhập khóa k trong khoảng 0 -> 25
  + Quá trình mã hóa dùng khóa k để dịch ký tự tiến lên
  + Quá trình giải mã dùng chính khóa k để dịch ký tự lùi lại
  + Chữ thường và chữ hoa được dịch trên bảng riêng của mình
  + Chữ số được dịch trên bảng 10 ký tự riêng
  + Ký tự đặc biệt (dấu cách, chấm, phẩy...) giữ nguyên
  + Sau khi giải mã có thể dùng SHA-256 để kiểm tra nội dung trước và sau có giống nhau hay không


- Ví dụ:
  + Bản rõ P = "HELLO"
  + Khóa: k = 3
  + Mã hóa:
    C[i] = (P[i] + 3) mod 26
    H -> K
    E -> H
    L -> O
    L -> O
    O -> R
    C = "KHOOR"
  + Giải mã:
    P[i] = (C[i] - 3) mod 26
    K -> H
    H -> E
    O -> L
    O -> L
    R -> O
    P = "HELLO"
    -> Nội dung sau khi giải mã quay trở lại đúng bản rõ ban đầu


- Đút kết:
  + Ceasar chỉ dùng 1 khóa duy nhất nên được gọi là mã hóa đối xứng
  + Khóa k vừa dùng để mã hóa vừa dùng để giải mã
  + Đặc điểm đơn giản, dễ cài đặt nhưng kém an toàn
    vì chỉ có nhiều nhất 26 trường hợp khóa cần thử để phá mã

    -> Với công thức là k với k là số vị trí dịch chuyển của khóa
    VD: dùng khóa k = 3 thì chỉ cần thử tối đa 26 khóa là tìm ra bản rõ

        MÃ HÓA     │     GIẢI MÃ
     ──────────────┼───────────────
      - khóa k     │    - khóa k
      - bản rõ P   │    - bản mã C
      - (P+k) mod n│    - (C-k) mod n
      - ra C       │    - ra P

*/
#endregion

using Spectre.Console;

public class Program {
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

    public static bool nhap_khoa(ref int khoa){
        Console.Write("\t(?) Nhập khóa: ");
        int.TryParse(Console.ReadLine()!, out khoa);
        if(khoa < 0 || khoa > 25){
            Console.WriteLine("\t\t(!) Khóa phải từ 0 -> 25");
            khoa = 0;
            return false;
        }
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
        string noidung = "", bangoc = "", banma = "";
        int khoa = 0;
        string duongdan_vao = "", duongdan_ra = "";

        Ceasar ceasar = new Ceasar();

        string banner = @"┌──────────────────────────────┐
│       Mã hóa ứng dụng        │
│ Mô phỏng mã hóa Ceasar       │
│ Tác giả: Trường Chinh        │
│ Github: Github.com/trgchinhh │
└──────────────────────────────┘
";
        Console.Clear();
        Console.WriteLine(banner);

        // phần hướng dẫn dùng 
        AnsiConsole.Write(
            new Panel(
                "Dùng phím ↑ ↓ để di chuyển\n" +
                "Dùng phím Enter để chọn"
            )
            .Header("Hướng dẫn")
        );
        dungchuongtrinh();

        while(true){
            Console.Clear();
            Console.WriteLine(banner);

            // tô màu chữ nội dung 
            Console.Write("[-] Nội dung: ");
            if(string.IsNullOrEmpty(noidung)) Mau.tomau("Chưa có", Mau.maudo, true);
            else Mau.tomau(noidung, Mau.mauxanhla, true);

            Console.WriteLine("\n  MENU");
            var luachon = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                .AddChoices(1, 2, 3, 4, 5, 6)
                .WrapAround(true)
                .HighlightStyle(new Style(Mau.mauxanhla))
                .UseConverter(x => x switch {
                    1 => Markup.Escape("[01] Nhập nội dung"),
                    2 => Markup.Escape("[02] Mã hóa nội dung "),
                    3 => Markup.Escape("[03] Giải mã nội dung"),
                    4 => Markup.Escape("[04] Mã hóa nội dung file"),
                    5 => Markup.Escape("[05] Giải mã nội dung file"),
                    6 => Markup.Escape("[06] Thoát"),
                    _ => ""
                })
            );

            if(luachon == 1){
                Console.WriteLine("\n[Nhập nội dung]\n");
                if(nhap_noidung(ref noidung)){
                    Console.WriteLine("\t(*) Đã ghi nhận nội dung mới !");
                } 
                else {
                    Console.WriteLine("\t(!) Ghi nội dung mới chưa thành công !");
                }
            }
            else if(luachon == 2){
                Console.WriteLine("\n[Mã hóa]\n");
                if(!nhap_khoa(ref khoa)){
                    Console.WriteLine("\t(!) Chưa có khóa vui lòng nhập khóa !");
                    continue;
                }
                if(!string.IsNullOrEmpty(noidung)){
                    banma = ceasar.mahoa(noidung, khoa);
                    ceasar.in_mahoa(banma);
                }
                else {
                    Console.WriteLine("\t(!) Chưa có nội dung vui lòng chọn [1] để nhập !");
                }
            }
            else if(luachon == 3){
                Console.WriteLine("\n[Giải mã]\n");
                if(!nhap_khoa(ref khoa)){
                    Console.WriteLine("\t(!) Chưa có khóa vui lòng nhập khóa !");
                    continue;
                }
                if(!string.IsNullOrEmpty(banma)){
                    bangoc = ceasar.giaima(banma, khoa);
                    ceasar.in_giaima(bangoc);
                }
                else {
                    Console.WriteLine("\t(!) Chưa có nội dung bản mã vui lòng chọn [2] để mã hóa !");
                }
            }
            else if(luachon == 4){
                Console.WriteLine("\n[Mã hóa nội dung file]\n");
                if(!nhap_khoa(ref khoa)){
                    Console.WriteLine("\t(!) Chưa có khóa vui lòng nhập khóa !");
                    continue;
                }
                if(nhap_duongdan(ref duongdan_vao, ref duongdan_ra)){
                    ceasar.mahoa_file(duongdan_vao, duongdan_ra, khoa);
                }
            }
            else if(luachon == 5){
                Console.WriteLine("\n[Giải mã nội dung file]\n");
                if(!nhap_khoa(ref khoa)){
                    Console.WriteLine("\t(!) Chưa có khóa vui lòng nhập khóa !");
                    continue;
                }
                if(nhap_duongdan(ref duongdan_vao, ref duongdan_ra)){
                    ceasar.giaima_file(duongdan_vao, duongdan_ra, khoa);
                }
            }
            else if(luachon == 6){
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

