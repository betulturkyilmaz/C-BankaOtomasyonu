using System;
using System.Collections;

namespace BankaSistemi
{
    class Program
    {
        static public void Main()
        {
            //Banka sınıfından banka adında bir nesne üretiliyor ve bu nesne ile BankaMenusu metodu çalıştırılıyor. 
            Banka banka = new Banka();
            banka.BankaMenusu();
            Console.ReadKey();
        }

    }
}