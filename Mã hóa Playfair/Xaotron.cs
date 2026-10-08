public class Xaotron {
    private int sobuoc = 0;

    public Xaotron(int sobuoc){
        this.sobuoc = sobuoc;
    }

    // dùng khi mã hóa: đổi chỗ cặp đối xứng rồi xoay phải
    public string Xao(string noidung){
        string tam = this.DoiXungCap(noidung);
        return this.Xoay(tam, true);
    }

    // dùng khi giải mã: xoay trái rồi đổi chỗ cặp đối xứng lại
    public string GiaiXao(string noidung){
        string tam = this.Xoay(noidung, false);
        return this.DoiXungCap(tam);
    }

    // đổi chỗ các cặp theo đối xứng từ ngoài vào, giữ nguyên thứ tự ký tự trong cặp
    // VD: AB CD EF -> EF CD AB, cặp giữa (nếu có) giữ nguyên
    private string DoiXungCap(string noidung){
        int socap = noidung.Length / 2;
        char[] ketqua = new char[noidung.Length];
        for(int i = 0; i < socap; i++){
            int vitridoixung = socap - i - 1;
            ketqua[vitridoixung * 2] = noidung[i * 2];
            ketqua[vitridoixung * 2 + 1] = noidung[i * 2 + 1];
        }
        return new string(ketqua);
    }

    // xoay vòng cả chuỗi sobuoc ký tự
    // sangphai = true: xoay phải, false: xoay trái (để giải mã)
    private string Xoay(string noidung, bool sangphai){
        int dodai = noidung.Length;
        if(dodai == 0){
            return noidung;
        }
        // % hai lần để số bước âm hoặc lớn hơn độ dài vẫn đúng
        int n = ((this.sobuoc % dodai) + dodai) % dodai;
        char[] ketqua = new char[dodai];
        for(int i = 0; i < dodai; i++){
            if(sangphai){
                ketqua[(i + n) % dodai] = noidung[i];
            }
            else {
                ketqua[i] = noidung[(i + n) % dodai];
            }
        }
        return new string(ketqua);
    }
}