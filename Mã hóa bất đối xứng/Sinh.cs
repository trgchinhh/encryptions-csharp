using System;
using System.Text;
using System.Numerics;
using System.Collections.Generic;

public class Sinh {
    private BigInteger sinhso(int sobit, Random random){
        byte[] noidung = new byte[sobit / 8];
        random.NextBytes(noidung);
        noidung[noidung.Length - 1] |= 128;
        noidung[0] |= 1;
        return new BigInteger(noidung, isUnsigned: true);
    } 

    private bool kiemtranguyento(BigInteger n){
        if (n < 2) return false;
        if (n % 2 == 0) return n == 2;
        BigInteger d = n - 1;
        int s = 0;
        while (d % 2 == 0){
            d /= 2;
            s++;
        }
        int[] coso = { 2, 3, 5, 7, 11, 13, 17, 19 };
        foreach (int a in coso){
            if (a >= n) continue;
            BigInteger x = BigInteger.ModPow(a, d, n);
            if (x == 1 || x == n - 1) continue;
            bool hople = false;
            for (int r = 1; r < s; r++){
                x = BigInteger.ModPow(x, 2, n);
                if (x == n - 1){
                    hople = true;
                    break;
                }
            }
            if (!hople) return false;
        }
        return true;
    }

    public BigInteger sinhnguyento(int sobit, Random random){
        while(true){
            BigInteger so = sinhso(sobit, random);
            if(kiemtranguyento(so)){
                return so;                
            } 
        }
    }
}