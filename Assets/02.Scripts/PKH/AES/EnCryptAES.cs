using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class EnCryptAES : MonoBehaviour
{
    public static string EncryptAes(string textToEncrypt, string key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            byte[] saltBytes = GenerateRandomBytes(32);
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, saltBytes, 100);

            // 블록 크기는 AES의 고정된 128 비트로 설정됩니다.
            aesAlg.Key = pdb.GetBytes(16);

            // IV(초기화 벡터) 생성
            aesAlg.GenerateIV();

            // 암호화 변환기 객체 생성
            ICryptoTransform encryptor = aesAlg.CreateEncryptor();

            // 평문을 바이트 배열로 변환
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

            // 암호화
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainText, 0, plainText.Length);

            //솔트와 암호화된 데이터와 IV(초기화 벡터)를 함께 저장
            byte[] combinedData = new byte[saltBytes.Length + aesAlg.IV.Length + cipherBytes.Length];
            Array.Copy(saltBytes, 0, combinedData, 0, saltBytes.Length);
            Array.Copy(aesAlg.IV, 0, combinedData, saltBytes.Length, aesAlg.IV.Length);
            Array.Copy(cipherBytes, 0, combinedData, saltBytes.Length + aesAlg.IV.Length, cipherBytes.Length);


            // Base64 문자열로 변환하여 출력
            return Convert.ToBase64String(combinedData); 
        }

    }

    public static string DecryptAes(string textToDecrypt, string key)
    {
        byte[] combinedData = Convert.FromBase64String(textToDecrypt);

        // 솔트 추출 (첫 32바이트)
        byte[] saltBytes = new byte[32];
        Array.Copy(combinedData, 0, saltBytes, 0, saltBytes.Length);

        // 솔트 이후의 데이터가 IV와 암호화된 데이터
        byte[] ivAndCipherText = new byte[combinedData.Length - saltBytes.Length];
        Array.Copy(combinedData, saltBytes.Length, ivAndCipherText, 0, ivAndCipherText.Length);

        // IV 추출
        byte[] iv = new byte[16];
        Array.Copy(ivAndCipherText, 0, iv, 0, iv.Length);

        // 키 파생
        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, saltBytes, 100);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = pdb.GetBytes(16);
            aesAlg.IV = iv;

            // 복호화 변환기 객체 생성
            ICryptoTransform decryptor = aesAlg.CreateDecryptor();

            // 암호화된 데이터 추출 (IV를 제외한 나머지 부분)
            byte[] cipherText = new byte[ivAndCipherText.Length - iv.Length];
            Array.Copy(ivAndCipherText, iv.Length, cipherText, 0, cipherText.Length);

            // 복호화
            byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

            // 복호화된 바이트 배열을 문자열로 변환하여 반환
            return Encoding.UTF8.GetString(decryptedBytes);
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
