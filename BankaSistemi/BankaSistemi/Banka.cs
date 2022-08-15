using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankaSistemi
{
    public class Banka
    {
        string hata=""; //Ekrana hata mesajı yazdırmak istenildiğinde kullanılıyor.
        Hesap hesap=new Hesap(); //Hesap sınıfından hesap adında bir nesne üretiliyor. 
        public static List<Hesap> hesaplar = new List<Hesap>(); //Bütün hesapların tutulduğu hesaplar adında bir liste oluşturuluyor.
        public static List<string> hesapNumaralari = new List<string>(); //Daha sonra karşılaştırabilmek için hesap numaralarını tutar.
        List<string> cekilisListesi=new List<string>(); //Çekiliş için hesap numaralarının tutulduğu liste.

        //BankaMenusu metodu, banka kullanıcısına işlem yapabileceği seçenekler sunuyor.
        public void BankaMenusu() 
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n     KTÜ BANKA SİSTEMİ\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("****************************");
            Console.WriteLine("1 - Hesap Açma İşlemleri");
            Console.WriteLine("2 - Para Yatırma");
            Console.WriteLine("3 - Para Çekme");
            Console.WriteLine("4 - Hesap Listesi");
            Console.WriteLine("5 - Hesap Durum");
            Console.WriteLine("6 - Hesap İşlem Kayıtları");
            Console.WriteLine("7 - Çekiliş");
            Console.WriteLine("8 - Programdan Çıkış Yap");
            Console.WriteLine("****************************\n");
            
            //Kullanıcı menüden bir işlem seçiyor ve bu işlem geçerli bir işlemse BankaMenuSecim metoduna gönderiliyor. 
            try
            {
                Console.Write("Lütfen Seçim Yapınız: ");
                string secim = Console.ReadLine();
                BankaMenuSecim(Convert.ToInt32(secim));
            }
            catch
            {
                HataGoster("* Bir hata oluştu. Lütfen doğru seçim yaptığınızdan emin olun.");
            } 

        }

        //BankaMenuSecim metodu kendisine gelen secim işlemine göre hareket eder.
        public void BankaMenuSecim(int secim)
        {
            if (secim == 1)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\n                                     KTÜ BANKA SİSTEMİ\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("*******************************************************************************************");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("                                       Hesap Türleri                   ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("                                      ---------------                   ");
                Console.WriteLine("1 - Kısa Vadeli Hesap");
                Console.WriteLine("    - Kâr oranı yıllık %15'tir.Hesabın açılabilmesi için en az 5.000 TL bakiye gereklidir.\n");
                Console.WriteLine("2 - Uzun Vadeli Hesap");
                Console.WriteLine("    - Kâr oranı yıllık %17'dir.Hesabın açılabilmesi için en az 10.000 TL bakiye gereklidir.\n");
                Console.WriteLine("3 - Özel Hesap");
                Console.WriteLine("    - Kâr oranı yıllık %10'dur.Hesabın açılabilmesi için minimum tutar zorunluluğu yoktur.\n");
                Console.WriteLine("4 - Cari Hesap");
                Console.WriteLine("    - Kâr getirisi yoktur.");
                Console.WriteLine("*******************************************************************************************\n");

                try
                {
                    Console.Write("Lütfen hesap türü seçiniz: ");
                    int hesapTuru = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Lütfen adınızı ve soyadınızı giriniz: ");
                    string hesapSahibi = Console.ReadLine();

                    Console.Write("Lütfen yatırmak istediğiniz tutarı giriniz: ");
                    decimal tutar = Convert.ToDecimal(Console.ReadLine());

                    Console.Write("Lütfen hesap açma tarihi giriniz(Örn:30/11/2021): ");
                    string tarihGirdisi = Console.ReadLine();

                    //Kullanıcının girdiği tarihi DateTime türüne dönüştürmeye çalışıyor.
                    DateTime tarih = DateTime.ParseExact(tarihGirdisi, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (hesapTuru == 1 && tutar >= 5000 && !string.IsNullOrEmpty(hesapSahibi))
                    {
                        HesapOlustur(hesapTuru, tutar, hesapSahibi, tarih);

                    }
                    else if (hesapTuru == 2 && tutar >= 10000 && !string.IsNullOrEmpty(hesapSahibi))
                    {
                        HesapOlustur(hesapTuru, tutar, hesapSahibi, tarih);
                    }
                    else if (hesapTuru == 3 && !string.IsNullOrEmpty(hesapSahibi) || hesapTuru == 4 && !string.IsNullOrEmpty(hesapSahibi))
                    {
                        HesapOlustur(hesapTuru, tutar, hesapSahibi, tarih);

                    }
                    else
                    {
                        HataGoster("* Bir hata oluştu. Lütfen bilgileri kontrol edip tekrar giriniz.");
                    }
                }
                catch
                {
                    HataGoster("* Bir hata oluştu. Lütfen bilgileri kontrol edip tekrar giriniz.");
                }
                
                

            }
            else if (secim == 2)
            {
                ParaYatir();
                
            }
            else if (secim == 3)
            {
                ParaCek();
            }
            else if (secim == 4)
            {
                HesapListesi();
            }
            else if (secim == 5)
            {
                Console.WriteLine("Lütfen hesap durumunu öğrenmek istediğiniz hesabın numarasını giriniz: ");
                string hesapNo = Console.ReadLine();
                hesap.hesapDurum(hesapNo,"anamenu");
            }
            else if (secim == 6)
            {
                Console.WriteLine("Lütfen işlem kayıtlarını görmek istediğiniz hesabın numarasını giriniz: ");
                string hesapNo = Console.ReadLine();
                hesap.hesapOzeti(hesapNo,"anamenu"); 
            }
            else if (secim == 7)
            {
                Cekilis();
            }
            else if(secim==8)
            {
                System.Environment.Exit(1);
            }
            else
            {
                HataGoster("* Geçersiz bir seçim yaptınız. Lütfen tekrar deneyiniz.");
            }
        }

        //Kullanıcının girdiği bilgiler ile bir banka hesabı oluşturuyor.
        public void HesapOlustur(int hesapTuru, decimal tutar, string hesapSahibi, DateTime tarih)
        {
            string hesapTuruAdi="";

            //Kullanıcının seçtiği hesap türü ile enum listemizdeki hesap türlerini karşılaştırıp hesap türümüzün adını belirliyoruz.
            if ((int)Hesap.HesapTurleri.KisaVadeli==hesapTuru)
            {
                Console.WriteLine((int)Hesap.HesapTurleri.KisaVadeli);
                hesapTuruAdi = "Kısa Vadeli Hesap";
            }
            else if ((int)Hesap.HesapTurleri.UzunVadeli == hesapTuru)
            {
                hesapTuruAdi = "Uzun Vadeli Hesap";
            }
            else if ((int)Hesap.HesapTurleri.Ozel == hesapTuru)
            {
                hesapTuruAdi = "Özel Hesap";
            }
            else if((int)Hesap.HesapTurleri.Cari == hesapTuru)
            {
                hesapTuruAdi = "Cari Hesap";
            }
            string hesapNo = hesap.hesapNumarasiOlustur(); //Yeni bir hesap numarası oluşturuyor.
            //Kullanıcının girdiği bilgiler ile hesaplar listemize yeni bir hesap ekliyoruz.
            hesaplar.Add(new Hesap() { hesapTuru=hesapTuruAdi, hesapBakiyesi=tutar,adSoyad=hesapSahibi,hesapNumarasi=hesapNo, islemTarihi=tarih, karKontrolTarihi = tarih });
            hesapNumaralari.Add(hesapNo); //Daha sonra karşılaştırabilmek için hesap numarasını hesapNumaralari isimli listeye atıyoruz.
            int islemNo = hesap.islemNumarasiOlustur();//Yeni bir işlem numarası oluşturuyor.
            //Yeni açılan hesabın bilgilerini islemler isimli listemize kaydediyor.
            Hesap.islemler.Add(new Hesap() { islemNumarasi = islemNo, tutar = tutar, islemTuru = "Yeni Hesap", oncekiBakiye = 0, hesapBakiyesi = tutar, hesapNumarasi = hesapNo, description = "Yeni Hesap Oluşturuldu.", islemTarihi = tarih, cekilisHakki=0 });
            Console.Clear();
            hesap.HesapMenusu(hesapNo,hesapSahibi);
        }
        
        //Bankada bulunan bütün hesapları listeliyor.
        public void HesapListesi()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n                                     KTÜ BANKA SİSTEMİ\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("*******************************************************************************************");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("                                       Hesap Listesi                   ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("                                      ---------------                   ");
            if (hesaplar.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine(String.Format("{0,-10} | {1,-20} | {2,-20} | {3,-15} | {4,-10}", "Hesap No","Hesap Sahibi","Hesap Türü","Hesap Bakiyesi","Hesap Açılış Tarihi"));
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var hesap in hesaplar)
                {
                    Console.WriteLine("-----------------------------------------------------------------------------------------------");
                    Console.WriteLine(String.Format("{0,-10} | {1,-20} | {2,-20} | {3,-15} | {4,-10}",hesap.hesapNumarasi,hesap.adSoyad,hesap.hesapTuru, string.Format("{0:0,0.00} TL", hesap.hesapBakiyesi),hesap.islemTarihi.ToString("dd/MM/yyyy")));
                    
                }
                Console.WriteLine("\n"+hesaplar.Count+" adet hesap listelendi.");
                
            }
            else
            {
                Console.WriteLine("Listelenecek hesap bulunamadı.");
            }
            Console.WriteLine("\nAna menüye dönmek için klavyeden herhangi bir tuşa basınız...");
            Console.ReadKey();
            Console.Clear();
            BankaMenusu();

            
        }

        //Ana menüde kullanıcıdan alınan bilgileri Hesap sınıfındaki ParaYatir metoduna yönlendiriyor.
        public void ParaYatir()
        {
            try
            {
                Console.WriteLine("Lütfen para yatırmak istediğiniz hesap numarasını giriniz: ");
                string hesapNo = Console.ReadLine();
                Console.WriteLine("Lütfen yatırmak istediğiniz tutarı giriniz: ");
                decimal yatirilanTutar = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Lütfen para yatırma tarihi giriniz(Örn:30/11/2021): ");
                string tarih = Console.ReadLine();
                DateTime islemTarihi = DateTime.ParseExact(tarih, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                Console.WriteLine("Lütfen bir açıklama giriniz(isteğe bağlı): ");
                string aciklama = Console.ReadLine();
                if (!string.IsNullOrEmpty(hesapNo) && yatirilanTutar>0)
                {
                    hesap.ParaYatir(hesapNo, yatirilanTutar, islemTarihi, aciklama, "anamenu");
                }
            }
            catch
            {
                HataGoster("* Bir hata oluştu. Lütfen bilgileri kontrol edip tekrar giriniz.");
            }
            
            
        }

        //Ana menüde kullanıcıdan alınan bilgileri Hesap sınıfındaki ParaCek metoduna yönlendiriyor.
        public void ParaCek()
        {
            try
            {
                Console.WriteLine("Lütfen para çekmek istediğiniz hesap numarasını giriniz: ");
                string hesapNo = Console.ReadLine();
                Console.WriteLine("Lütfen çekmek istediğiniz tutarı giriniz: ");
                decimal cekilenTutar = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Lütfen bir açıklama giriniz(isteğe bağlı): ");
                string aciklama = Console.ReadLine();
                if (!string.IsNullOrEmpty(hesapNo) && cekilenTutar>0)
                {
                    hesap.ParaCek(hesapNo, cekilenTutar,aciklama, "anamenu");
                }
            }
            catch
            {
                HataGoster("* Bir hata oluştu. Lütfen bilgileri kontrol edip tekrar giriniz.");
            }
            
        }

        //Belirlenen tutarı belli koşullar karşılığında rastgele bir hesaba aktarıyor.
        public void Cekilis()
        {
            try
            {
                int count = 0;
                
                if (Hesap.islemler.Count > 0)
                {
                    foreach (var islem in Hesap.islemler)
                    {
                        //işlemlerde çekiliş hakkı 0 dan büyük olanları cekilisListesi isimli listeye atıyor.
                        if (islem.cekilisHakki > 0)
                        {
                            for (int i = 0; i < islem.cekilisHakki; i++)
                            {
                                cekilisListesi.Add(islem.hesapNumarasi);
                            }
                            count++;
                        }

                    }

                    if (count == 0)
                    {
                        HataGoster("* Çekiliş listesinde hesap bulunamadı.");
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\n                                     KTÜ BANKA SİSTEMİ\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("*******************************************************************************************\n");
                        Console.WriteLine("Lütfen çekiliş için ödül miktarı giriniz: ");
                        decimal cekilisTutari = Convert.ToDecimal(Console.ReadLine());
                        Random random = new Random();

                        //cekilisListesi isimli listenin içindeki itemleri rastgele karıştırıyoruz.
                        var karisikCekilisListesi = cekilisListesi.OrderBy(item => random.Next());
                        //Random kullanarak listenin içerisinden bir hesabı rastgele seçiyoruz.
                        string kazananHesap = karisikCekilisListesi.ToList()[random.Next(0, cekilisListesi.Count)];
                        foreach (var hesap in hesaplar)
                        {
                            if (hesap.hesapNumarasi == kazananHesap)
                            {
                                //Kazanan hesaba çekiliş tutarını yatırıyoruz.
                                hesap.ParaYatir(hesap.hesapNumarasi, cekilisTutari, DateTime.Now, "Çekiliş yapıldı, para kazandı.", "anamenu");
                            }
                        }
                    }

                }
                else
                {
                   HataGoster("* Çekiliş listesinde hesap bulunamadı.");
                }

            }
            catch
            {
                HataGoster("* Bir hata oluştu. Lütfen bilgileri kontrol edip tekrar giriniz.");
            }
            
            
        }

        //Gelen hata mesajını ekrana bastırır.
        public void HataGoster(string hata)
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(hata+"\n");
            Console.ForegroundColor = ConsoleColor.White;
            BankaMenusu();
        }

    }
}
