using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
using biscuit.ABControl;
using Y3.Utils;
using biscuit.Data.Crypt;
using biscuit.Utils;

namespace Y3Decoder
{
    public static class Y3Dec
    {
        public static void DecodeSave(AssetBundleInfo info, byte[] data)
        {
            string text = string.Format("{0}/{1}/{2}", PathUtil.AssetPath(), "assets", info.name);
            ICrypt gk = Y3GK.Instance.GetBinaryCrypter();
            string directoryName = Path.GetDirectoryName(text);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            if (!File.Exists(text))
            {
                return;
            }
            if (info.encrypt && data != null)
            {
                if (string.IsNullOrEmpty(info.encrypt_type))
                {
                    data = AES.DecryptAes(data);
                }
                else
                {
                    CRYPT_TYPE crypt_TYPE = (CRYPT_TYPE)((int)Enum.Parse(typeof(CRYPT_TYPE), info.encrypt_type));
                    if (crypt_TYPE != CRYPT_TYPE.AES)
                    {
                        if (crypt_TYPE == CRYPT_TYPE.GK)
                        {
                            if (gk != null)
                            {
                                gk.SetKey(LHBParam.YUYUYU.key, LHBParam.YUYUYU.iv);
                                data = gk.FromLFB(data);
                            }
                        }
                    }
                    else
                    {
                        data = AES.DecryptAes(data);
                    }
                }
            }
            File.WriteAllBytes(text + ".unity3d", data);
        }
    }
}
