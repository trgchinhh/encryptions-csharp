/*
NÂNG CẤP 
- THÊM KÝ TỰ SỐ TỪ 0 -> 9
- THÊM CHỮ CÁI TIẾNG VIỆT 
- MA TRẬN KHÓA TỪ 5x5 -> 14x14 
*/

using System;
using System.Text;

public class Playfair {
    private const int N = 14;
    private string banro = "";
    private string khoa = "";
    private int sobuocquay = 0;
    private Random random = new Random();
    private char[] bangkytu = {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
        'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',

        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',

        'Đ',
        'À', 'Á', 'Ả', 'Ã', 'Ạ',
        'Ă', 'Ằ', 'Ắ', 'Ẳ', 'Ẵ', 'Ặ',
        'Â', 'Ầ', 'Ấ', 'Ẩ', 'Ẫ', 'Ậ',
        'È', 'É', 'Ẻ', 'Ẽ', 'Ẹ',
        'Ê', 'Ề', 'Ế', 'Ể', 'Ễ', 'Ệ',
        'Ì', 'Í', 'Ỉ', 'Ĩ', 'Ị',
        'Ò', 'Ó', 'Ỏ', 'Õ', 'Ọ',
        'Ô', 'Ồ', 'Ố', 'Ổ', 'Ỗ', 'Ộ',
        'Ơ', 'Ờ', 'Ớ', 'Ở', 'Ỡ', 'Ợ',
        'Ù', 'Ú', 'Ủ', 'Ũ', 'Ụ',
        'Ư', 'Ừ', 'Ứ', 'Ử', 'Ữ', 'Ự',
        'Ỳ', 'Ý', 'Ỷ', 'Ỹ', 'Ỵ',

        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
        'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',

        'đ',
        'à', 'á', 'ả', 'ã', 'ạ',
        'ă', 'ằ', 'ắ', 'ẳ', 'ẵ', 'ặ',
        'â', 'ầ', 'ấ', 'ẩ', 'ẫ', 'ậ',
        'è', 'é', 'ẻ', 'ẽ', 'ẹ',
        'ê', 'ề', 'ế', 'ể', 'ễ', 'ệ',
        'ì', 'í', 'ỉ', 'ĩ', 'ị',
        'ò', 'ó', 'ỏ', 'õ', 'ọ',
        'ô', 'ồ', 'ố', 'ổ', 'ỗ', 'ộ',
        'ơ', 'ờ', 'ớ', 'ở', 'ỡ', 'ợ',
        'ù', 'ú', 'ủ', 'ũ', 'ụ',
        'ư', 'ừ', 'ứ', 'ử', 'ữ', 'ự',
        'ỳ', 'ý', 'ỷ', 'ỹ', 'ỵ'
    };
    // dãy khóa đã chọn lọc từ trùng nhưng là 1 dãy nằm ngang 
    // tiện cho việc sắp xếp và chọn lọc
    private List<char> daykhoa = new List<char>();
    // sau đó bỏ dãy vào ma trận 14 x 14
    private List<List<char>> matrankhoa = new List<List<char>>();

    public Playfair(){
        this.BanRo = "";
        this.Khoa = "";
        this.KhoiTaoMaTranKhoa();
    }

    public Playfair(string banro, string khoa){
        this.BanRo = banro;
        this.Khoa = khoa;
        this.KhoiTaoMaTranKhoa();
        this.BoKhoaVaoMaTran();
    }

    public string BanRo {
        get { return this.banro; }
        set {
            if(!string.IsNullOrEmpty(value)){
                // this.banro = value.ToUpper().Replace(" ", "");
                this.banro = this.LocKyTu(value);
            }
        }
    }

    public string Khoa {
        get { return this.khoa; }
        set {
            if(!string.IsNullOrEmpty(value)){
                this.khoa = this.LocKyTu(value);
            }
        }
    }

    public int SoBuocQuay {
        get { return this.sobuocquay; }
        set { 
            this.sobuocquay = value; 
        }
    }

    private void KhoiTaoMaTranKhoa(){
        for(int i = 0; i < N; i++){
            this.matrankhoa.Add(new List<char>());
            for(int j = 0; j < N; j++){
                this.matrankhoa[i].Add(' ');
            }
        }
    }

    public void NhapThongTin(){
        try {
            string banro_tam, khoa_tam;
            while(true){
                Console.Write("Nhập bản rõ: ");
                banro_tam = Console.ReadLine()!;
                if(string.IsNullOrEmpty(banro_tam)){
                    Console.WriteLine("Không để trống thông tin !");
                    continue;
                }

                Console.Write("Nhập khóa: ");
                khoa_tam = Console.ReadLine()!;
                if(string.IsNullOrEmpty(khoa_tam)){
                    Console.WriteLine("Không để trống thông tin !");
                    continue;
                }
                Console.Write("Nhập số bước xoay (có thể bỏ trống): ");
                int.TryParse(Console.ReadLine(), out int sobuocquay_tam);
                this.SoBuocQuay = sobuocquay_tam;
                this.BanRo = banro_tam;
                this.Khoa = khoa_tam;
                this.BoKhoaVaoMaTran();
                break;
            }
        } catch(Exception ex){
            Console.WriteLine($"Lỗi: {ex.Message}");
        }
    }

    private string LocKyTu(string value){
        StringBuilder sb = new StringBuilder();
        foreach(char c in value.Normalize(NormalizationForm.FormC)){
            if(Array.IndexOf(this.bangkytu, c) >= 0){
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    private void BoKhoaVaoMaTran(){
        // bỏ khóa vào dãy khóa trước
        for(int i = 0; i < this.khoa.Length; i++){
            // nếu là chữ cái đầu thì bỏ vào luôn 
            if(i == 0){
                this.daykhoa.Add(this.khoa[i]);
                continue;                
            } 
            // từ chữ cái thứ 2 trở đi phải kiểm tra có trùng ko 
            // nếu không trùng mới add vào dãy khóa
            if(!this.daykhoa.Contains(this.khoa[i])) {
                this.daykhoa.Add(this.khoa[i]);
            } 
        }

        // bỏ các từ còn lại trong bảng chữ cái vào dãy khóa 
        for(int i = 0; i < this.bangkytu.Length; i++){
            // nếu chữ cái nào không trùng với những từ đã có trong dãy khóa
            // thì thêm vào cho đủ 
            if(!this.daykhoa.Contains(this.bangkytu[i])){
                this.daykhoa.Add(this.bangkytu[i]);
            }
        }

        // chuyển dãy khóa (mảng 1 chiều) thành bảng khóa (ma trận)
        for(int i = 0; i < N; i++){
            for(int j = 0; j < N; j++){
                this.matrankhoa[i][j] = this.daykhoa[i * N + j];
            }
        }
    }



    public string MaHoa(){
        string banma = "";
        string banro_tam = this.banro;
        //string banro_tam = this.banro.ToUpper();
        // thay những chữ J thành I và I/J chung 1 ô nên quy về I 
        //banro_tam = banro_tam.Replace('J', 'I');
        for(int i = 0; i < banro_tam.Length; i+=2){
            char a = banro_tam[i];
            char b;
            // nếu từ kế bên không có thì cho a + với chữ X
            // sao cho mỗi cặp đều đủ 2 chữ cái
            // VD: SAYHAI -> SA YH I -> SA YH IX
            if(i + 1 >= banro_tam.Length){
                b = 'X';
            }
            // nếu 1 cặp có 2 từ giống nhau thì thay X vào giữa
            else if(a == banro_tam[i + 1]){
                b = 'X';
                i--;
            }
            else {
                b = banro_tam[i + 1];
            }

            int dong1 = 0, cot1 = 0;
            int dong2 = 0, cot2 = 0;

            for(int j = 0; j < N; j++){
                for(int k = 0; k < N; k++){
                    // gán dòng và cột cho từng chữ cái trong 1 cặp
                    if(this.matrankhoa[j][k] == a){
                        dong1 = j; cot1 = k;
                    }
                    if(this.matrankhoa[j][k] == b){
                        dong2 = j; cot2 = k;
                    }
                }
            }

            // nếu cùng dòng thì xích sang phải 1 ký tự 
            // dòng giữ nguyên, cột xích 1  
            // % 5 là để quay lại đầu dòng đó  
            if(dong1 == dong2){
                banma += this.matrankhoa[dong1][(cot1 + 1) % N];
                banma += this.matrankhoa[dong2][(cot2 + 1) % N];
            }
            // nếu cùng cột thì xích xuống 1 hàng 
            // dòng xích 1, cột giữ nguyên 
            else if(cot1 == cot2){
                banma += this.matrankhoa[(dong1 + 1) % N][cot1];
                banma += this.matrankhoa[(dong2 + 1) % N][cot2];
            }
            // nếu khác dòng khác cột 
            // thì giữ nguyên dòng đổi cột
            else {
                banma += this.matrankhoa[dong1][cot2];
                banma += this.matrankhoa[dong2][cot1];
            }
        }
        Xaotron xaotron = new Xaotron(this.sobuocquay);
        return xaotron.Xao(banma);
        //return banma;
    }

    public string GiaiMa(string banma){
        string banro = "";
        Xaotron xaotron = new Xaotron(this.sobuocquay);
        string banma_tam = xaotron.GiaiXao(banma);
        //string banma_tam = banma;
        // nếu số ký tự lẽ (có 1 cặp lẻ)
        if(banma_tam.Length % 2 != 0){
            return "Mã hóa không đủ ký tự trong cặp để giải mã !";
        }
        for(int i = 0; i < banma_tam.Length; i+=2){
            char a = banma_tam[i];
            char b = banma_tam[i + 1];
            int dong1 = 0, cot1 = 0;
            int dong2 = 0, cot2 = 0;

            for(int j = 0; j < N; j++){
                for(int k = 0; k < N; k++){
                    // gán dòng và cột cho từng chữ cái trong 1 cặp
                    if(this.matrankhoa[j][k] == a){
                        dong1 = j; cot1 = k;
                    }
                    if(this.matrankhoa[j][k] == b){
                        dong2 = j; cot2 = k;
                    }
                }
            }

            // nếu cùng dòng thì tăng thêm 4 chữ để quay lại chữ ban đầu
            // VD: A  B  C  D  E  đang ở E muốn quay về D thì (Vị trí hiện tại + 4) % 5 
            // --> 1  2  3  4  0  lúc này nó đã quay về D
            if(dong1 == dong2){
                banro += matrankhoa[dong1][(cot1 + N - 1) % N];
                banro += matrankhoa[dong2][(cot2 + N - 1) % N];
            }
            // cùng cột làm tương tự 
            else if(cot1 == cot2){
                banro += matrankhoa[(dong1 + N - 1) % N][cot1];
                banro += matrankhoa[(dong2 + N - 1) % N][cot2];
            }
            // bản mã giữ dòng đổi cột
            // bản rõ cũng làm tương tự để giữ nguyên vị trí ban đầu 
            else{
                banro += matrankhoa[dong1][cot2];
                banro += matrankhoa[dong2][cot1];
            }

        }
        this.XoaKyTuThua(ref banro);
        return banro;
    }

    // xóa đi ký tự 'X' lúc đầu (nếu có thêm vào)
    private void XoaKyTuThua(ref string banro){
        for(int i = 1; i < banro.Length - 1; i++){
            // nếu 'X' tại vị trí i và chữ cái trước và sau X giống nhau
            // thì 'X' đó được đệm thêm 
            // bỏ 'X' và giảm i để tiếp tục kiểm tra 
            if(banro[i] == 'X' && banro[i - 1] == banro[i + 1]){
                banro = banro.Remove(i, 1);
                i--;
            }
        }
        // nếu 'X' nằm cuối chuỗi thì xóa 
        // vì đó là 'X' đệm cho đủ cặp
        if(banro.EndsWith("X")){
            banro = banro.Remove(banro.Length - 1);
        }
    }

    // đảo vị trí: swap ký tự trong cặp và đảo vị trí các cặp đối xứng (từ ngoài vào)
    // dùng chung cho mã hóa và giải mã 
    private string DaoViTri(string noidung){
        int socap = noidung.Length / 2;
        char[] ketqua = new char[noidung.Length];
        for(int i = 0; i < socap; i++){
            int vitridoixung = socap - i - 1;
            // swap ký tự trong cặp
            ketqua[vitridoixung * 2] = noidung[i * 2 + 1];
            ketqua[vitridoixung * 2 + 1] = noidung[i * 2];
        }
        return new string(ketqua);
    }

    // hàm tách cặp chữ để in ra 
    private string InTachCap(string noidung){
        string ketquatachcap = "";
        for(int i = 0; i < noidung.Length; i += 2){
            ketquatachcap += noidung[i];
            if(i + 1 < noidung.Length){
                ketquatachcap += noidung[i + 1];
            }
            ketquatachcap += " ";
        }
        return ketquatachcap;
    }

    public void InMaTranKhoa(){
        Console.WriteLine($"\nMa trận khóa {N}x{N}");
        for(int i = 0; i < N; i++){
            for(int j = 0; j < N; j++){
                Console.Write(this.matrankhoa[i][j] + " ");
            }
            Console.WriteLine();
        }
    }

    public void InThongTin(string banma, string banro){
        Console.WriteLine($"\nBản rõ (ban đầu): {this.banro}");
        Console.WriteLine($"Nội dung khóa: {this.khoa}");
        Console.WriteLine($"Bản mã: {this.InTachCap(banma)} ({banma})");
        Console.WriteLine($"Bản rõ (giải mã): {this.InTachCap(banro)} ({banro})");
    }
}