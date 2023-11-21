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

            // ��� ũ��� AES�� ������ 128 ��Ʈ�� �����˴ϴ�.
            aesAlg.Key = pdb.GetBytes(16);

            // IV(�ʱ�ȭ ����) ����
            aesAlg.GenerateIV();

            // ��ȣȭ ��ȯ�� ��ü ����
            ICryptoTransform encryptor = aesAlg.CreateEncryptor();

            // ���� ����Ʈ �迭�� ��ȯ
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

            // ��ȣȭ
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainText, 0, plainText.Length);

            //��Ʈ�� ��ȣȭ�� �����Ϳ� IV(�ʱ�ȭ ����)�� �Բ� ����
            byte[] combinedData = new byte[saltBytes.Length + aesAlg.IV.Length + cipherBytes.Length];
            Array.Copy(saltBytes, 0, combinedData, 0, saltBytes.Length);
            Array.Copy(aesAlg.IV, 0, combinedData, saltBytes.Length, aesAlg.IV.Length);
            Array.Copy(cipherBytes, 0, combinedData, saltBytes.Length + aesAlg.IV.Length, cipherBytes.Length);


            // Base64 ���ڿ��� ��ȯ�Ͽ� ���
            return Convert.ToBase64String(combinedData); 
        }

    }

    public static string DecryptAes(string textToDecrypt, string key)
    {
        byte[] combinedData = Convert.FromBase64String(textToDecrypt);

        // ��Ʈ ���� (ù 32����Ʈ)
        byte[] saltBytes = new byte[32];
        Array.Copy(combinedData, 0, saltBytes, 0, saltBytes.Length);

        // ��Ʈ ������ �����Ͱ� IV�� ��ȣȭ�� ������
        byte[] ivAndCipherText = new byte[combinedData.Length - saltBytes.Length];
        Array.Copy(combinedData, saltBytes.Length, ivAndCipherText, 0, ivAndCipherText.Length);

        // IV ����
        byte[] iv = new byte[16];
        Array.Copy(ivAndCipherText, 0, iv, 0, iv.Length);

        // Ű �Ļ�
        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, saltBytes, 100);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = pdb.GetBytes(16);
            aesAlg.IV = iv;

            // ��ȣȭ ��ȯ�� ��ü ����
            ICryptoTransform decryptor = aesAlg.CreateDecryptor();

            // ��ȣȭ�� ������ ���� (IV�� ������ ������ �κ�)
            byte[] cipherText = new byte[ivAndCipherText.Length - iv.Length];
            Array.Copy(ivAndCipherText, iv.Length, cipherText, 0, cipherText.Length);

            // ��ȣȭ
            byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

            // ��ȣȭ�� ����Ʈ �迭�� ���ڿ��� ��ȯ�Ͽ� ��ȯ
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }

    //������ ����Ʈ ����
    private static byte[] GenerateRandomBytes(int length)
    {
        //RNGCryptoServiceProvider(.NET�� ���� ��ȭ�� ���� ������)
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] randomBytes = new byte[length];
            rng.GetBytes(randomBytes);
            return randomBytes;
        }
    }
}
