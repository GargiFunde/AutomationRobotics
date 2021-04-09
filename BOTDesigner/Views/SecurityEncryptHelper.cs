// <copyright file=SecurityEncryptHelper company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTDesigner
{
    class SecurityEncryptHelper
    {

        private SecurityController _security;
        private static SecurityEncryptHelper securityEncryptHelper;
        static string _password = "123456789";// "+!@_)&(*^$@#^"; //Default value

        public static SecurityEncryptHelper GetSecurityEncryptHelper()
        {
            if (securityEncryptHelper == null)
                securityEncryptHelper = new SecurityEncryptHelper();
            return securityEncryptHelper;
        }
        public static string SetPassword
        {
            set
            {
                _password = value;
            }
        }
        SecurityEncryptHelper()
        {
            
            _security = new SecurityController();
        }
        public string EncryptText(string notEncryptedText)
        {
            _password = _password.Length.ToString() + _password + _password.Length.ToString();
            return _security.Encrypt(_password, notEncryptedText);
        }

        public string DecryptText(string encryptedText)
        {
            _password = _password.Length.ToString() + _password + _password.Length.ToString();
            return _security.Decrypt(_password, encryptedText);
        }
    }
}
