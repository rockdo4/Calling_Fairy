using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;
//using UnityEngine.Purchasing.MiniJSON;

public struct Option
{
    public CipherMode cipherMode;
    public PaddingMode paddingMode;
    public int keySize;
    public int blockSize;
    public int IVSize;

    public Option(CipherMode cipher, PaddingMode padding, int kSize, int bSize, int ivSize)
    {
        cipherMode = cipher;
        paddingMode = padding;
        keySize = kSize;
        blockSize = bSize;
        IVSize = ivSize;
    }
}

public class EncryptRijndael
{
    private static Option option = 
        new Option(CipherMode.CBC, PaddingMode.PKCS7, 256, 128, 16);
    public static string Encrypt256(string textToEncrypt, string key)
    {
        using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
        {
            //암호화 모드
            rijndaelCipher.Mode = option.cipherMode;
            //패딩 모드 (PKCS#7 패딩 알고리즘)
            rijndaelCipher.Padding = option.paddingMode;
            //키 사이즈 (128, 192, 256비트 중 선택)
            rijndaelCipher.KeySize = option.keySize;
            //블럭 사이즈는 고정
            rijndaelCipher.BlockSize = option.blockSize;

            //무작위 솔트 (무작위성 추가 작업)
            byte[] saltBytes = GenerateRandomBytes(32);
            //키 파생 함수 (사용자키, 무작위 솔트, 반복횟수)
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, saltBytes, 1000);
            //암호화 키 (32바이트(256비트) 길이의 암호화 키 생성)
            rijndaelCipher.Key = pdb.GetBytes(32);
            //무작위 IV(초기화 벡터)
            //CBC(Cipher Block Chaining) 모드에서 첫 번째 블록을 암호화할 때 사용
            rijndaelCipher.IV = GenerateRandomBytes(option.IVSize); 

            //암호화 변환기 객체 생성 (평문을 암호문으로 변환하는 데 사용)
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            //암호화 할 문자열을 UTF-8 인코딩을 사용해여 바이트 배열로 변환
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
            //바이트 배열을 암호화
            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

            //솔트와 암호화된 데이터와 IV(초기화 벡터)를 함께 저장
            byte[] combinedData = new byte[saltBytes.Length + rijndaelCipher.IV.Length + cipherBytes.Length];
            Array.Copy(saltBytes, 0, combinedData, 0, saltBytes.Length);
            Array.Copy(rijndaelCipher.IV, 0, combinedData, saltBytes.Length, rijndaelCipher.IV.Length);
            Array.Copy(cipherBytes, 0, combinedData, saltBytes.Length + rijndaelCipher.IV.Length, cipherBytes.Length);

            return Convert.ToBase64String(combinedData);
        }
    }

    public static string Decrypt256(string textToDecrypt, string key)
    {
        using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
        {
            rijndaelCipher.Mode = option.cipherMode;
            rijndaelCipher.Padding = option.paddingMode;
            rijndaelCipher.KeySize = option.keySize;
            rijndaelCipher.BlockSize = option.blockSize;

            // Base64 인코딩된 문자열을 바이트 배열로 변환
            byte[] combinedData = Convert.FromBase64String(textToDecrypt);

            // 솔트 추출 (첫 32바이트)
            byte[] saltBytes = new byte[32];
            Array.Copy(combinedData, 0, saltBytes, 0, saltBytes.Length);


            // 솔트 이후의 데이터가 IV와 암호화된 데이터
            byte[] ivAndCipherText = new byte[combinedData.Length - saltBytes.Length];
            Array.Copy(combinedData, saltBytes.Length, ivAndCipherText, 0, ivAndCipherText.Length);

            // IV 추출
            byte[] iv = new byte[option.IVSize];
            Array.Copy(ivAndCipherText, 0, iv, 0, iv.Length);
            rijndaelCipher.IV = iv;

            // 키 파생
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, saltBytes, 1000);
            rijndaelCipher.Key = pdb.GetBytes(32);

            // 복호화 변환기 객체 생성
            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();

            // 실제 암호화된 데이터 추출 (IV를 제외한 나머지 부분)
            byte[] cipherText = new byte[ivAndCipherText.Length - iv.Length];
            Array.Copy(ivAndCipherText, iv.Length, cipherText, 0, cipherText.Length);

            // 복호화
            byte[] plainText = transform.TransformFinalBlock(cipherText, 0, cipherText.Length);

            // 복호화된 데이터를 문자열로 변환하여 반환
            return Encoding.UTF8.GetString(plainText);
        }
    }

    //무작위 바이트 생성
    private static byte[] GenerateRandomBytes(int length)
    {
        //RNGCryptoServiceProvider(.NET의 보안 강화된 난수 생성기)
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] randomBytes = new byte[length];
            rng.GetBytes(randomBytes);
            return randomBytes;
        }
    }
}

