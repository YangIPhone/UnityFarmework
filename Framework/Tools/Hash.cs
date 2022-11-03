using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;

public class Hash
{
    //MD5加密=>不可逆
    public static string MD5Hash(string content){
        byte[] b = Encoding.Default.GetBytes(content);
        MD5 md5 = new MD5CryptoServiceProvider();
        b = md5.ComputeHash(b);
        string s = BitConverter.ToString(b).Replace("-","");
        return s;
    }

    //Base64
    //编码
    public static string Base64Encode(string message){
        byte[] bytes =  Encoding.GetEncoding("utf-8").GetBytes(message);
        return Convert.ToBase64String(bytes);
    }

    //解码
    public static string Base64Decode(string hashString){
        byte[] bytes = Convert.FromBase64String(hashString);
        return Encoding.GetEncoding("utf-8").GetString(bytes);
    }
}
