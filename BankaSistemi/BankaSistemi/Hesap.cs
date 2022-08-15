using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace BankaSistemi
{
    public class Hesap
    {
        public string adSoyad;//Hesaplar listesinde ad soyad bilgisini tutuyor.
        public string hesapNumarasi;//Hesaplar listesinde hesap numarası bilgisini tutuyor.
        public decimal hesapBakiyesi;//Hesaplar listesinde hesap bakiyesi bilgisini tutuyor.
        public int islemNumarasi=1; //İşlem numarası oluştururken kullanılıyor.
        public DateTime islemTarihi;//Hesaplar ve İşlemler listesinde işlem tarihi bilgisini tutuyor.
        public DateTime karKontrolTarihi;//hesaplar listesinde para çekme işlemi esnasında kâr hesabı yaparken kullanılacak kontrol tarihi bilgisini tutuyor.
        public string hesapTuru;//Hesaplar listesinde hesap türü bilgisini tutuyor.
        public string islemTuru;//İşlemler listesinde işlem türü bilgisini tutuyor.
        public decimal tutar;//İşlemler listesinde yatırılan veya çekilen tutar bilgisini tutuyor.
        public decimal oncekiBakiye;//İşlemler listesinde işlem yapılmadan önceki hesap bakiyesi bilgisini tutuyor.
        public string description; //İşlemler listesinde açıklama bilgisini tutuyor.
        public int cekilisHakki; //İşlemler listesinde çekiliş hakkı bilgisini tutuyor.
        string hata = ""; //Ekrana hata mesajı yazdırmak istenildiğinde kullanılıyor.
        public static int yeniIslemNumarasi=999; //İşlem numarası oluştururken kullanılıyor.
        public static List<Hesap> islemler = new List<Hesap>(); //Hesap açma, para yatırma ve çekme işlemlerinin kaydını tutan bir liste oluşturuyor.
        decimal bakiye = 0; //Bakiye bilgisini tutuyor.
        decimal toplamBakiye = 0; //Bakiye ve kâr toplamı bilgisini tutuyor.
        decimal toplamKar = 0; //Kâr bilgisini tutuyor.




        //Hesap türlerini enum olarak tutuyor.
        public enum HesapTurleri
        {
            KisaVadeli=1,
            UzunVadeli=2,
            Ozel=3,
            Cari=4
        }
        
        //Hesap sahibine yapabileceği işlemleri listeliyor.
        public void HesapMenusu(string hesapNo,string hesapSahibi)
        {
            //Bakiye ve kâr bilgilerini çekiyor.
            foreach(var hesap in Banka.hesaplar)
            {
                if (hesap.hesapNumarasi == hesapNo)
                {
                    bakiye = hesap.hesapBakiyesi;
                    toplamBakiye= karTutari(hesap.hesapNumarasi, hesap.hesapBakiyesi, hesap.karKontrolTarihi, hesap.hesapTuru);
                    toplamKar = toplamBakiye - hesap.hesapBakiyesi;

                }
            }
            hesapNumarasi = hesapNo;
            adSoyad = hesapSahibi;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n     KTÜ BANKA SİSTEMİ\n");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Sayın "+hesapSahibi+" hoşgeldiniz. Hesap numaranız: "+hesapNo+". Bakiyeniz: " + string.Format("{0:0,0.00} TL", bakiye) + ". Kâr oranınız: " + string.Format("{0:0,0.00} TL", toplamKar) + ". Toplam bakiyeniz: " + string.Format("{0:0,0.00} TL", toplamBakiye));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("***************************");
            Console.WriteLine("1 - Para Yatırma");
            Console.WriteLine("2 - Para Çekme");
            Console.WriteLine("3 - Hesap Durumu");
            Console.WriteLine("4 - Hesap İşlem Kayıtları & Dekontları");
            Console.WriteLine("5 - Kar Tutarı");
            Console.WriteLine("7 - Ana Menü");
            Console.WriteLine("8 - Programdan Çıkış Yap");
            Console.WriteLine("***************************");

            //Kullanıcı menüden bir işlem seçiyor ve bu işlem geçerli bir işlemse BankaMenuSecim metoduna gönderiliyor. 
            try
            {
                Console.Write("Lütfen Seçim Yapınız: ");
                string secim = Console.ReadLine();
                HesapMenuSecim(Convert.ToInt32(secim));
            }
            catch
            {
                HataGoster("* Bir hata oluştu. Lütfen doğru seçim yaptığınızdan emin olun.","");
            }
        }

        //Hesap sahibinin seçtiği menülere göre işlem gerçekleştiriyor.
        public void HesapMenuSecim(int secim)
        {
            if (secim == 1)
            {
                try
                {
                    Console.WriteLine("Lütfen yatırmak istediğiniz tutarı giriniz: ");
                    decimal yatirilanTutar = Convert.ToDecimal(Console.ReadLine());
                    Console.WriteLine("Lütfen para yatırma tarihi giriniz(Örn:30/11/2021): ");
                    string tarih = Console.ReadLine();
                    DateTime islemTarihi = DateTime.ParseExact(tarih, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    Console.WriteLine("Lütfen bir açıklama giriniz(isteğe bağlı): ");
                    string aciklama = Console.ReadLine();
                    if (yatirilanTutar>0)
                    {
                        ParaYatir(hesapNumarasi, yatirilanTutar, islemTarihi, aciklama, "");
                    }
                }
                catch
                {
                    HataGoster("* Bir hata oluştu. Lütfen bilgileri kontrol edip tekrar giriniz.","");
                }    
            }
            else if (secim == 2)
            {
                try
                {
                    Console.WriteLine("Lütfen çekmek istediğiniz tutarı giriniz: ");
                    decimal cekilenTutar = Convert.ToDecimal(Console.ReadLine());
                    Console.WriteLine("Lütfen bir açıklama giriniz(isteğe bağlı): ");
                    string aciklama = Console.ReadLine();
                    if (cekilenTutar>0)
                    {
                        ParaCek(hesapNumarasi, cekilenTutar,aciklama, "");
                    }
                }
                catch
                {
                    HataGoster("* Bir hata oluştu. Lütfen bilgileri kontrol edip tekrar giriniz.","");
                }
                
            }
            else if (secim == 3)
            {
                hesapDurum(hesapNumarasi,"");
            }
            else if (secim == 4)
            {
                hesapOzeti(hesapNumarasi,"");
            }
            else if (secim == 5)
            {
                
            }
            else if (secim == 6)
            {
                Console.Write("Lütfen görüntülemek istediğiniz işlemin numarasını giriniz: ");
                string islemNo = Console.ReadLine();
                //islemDekontu(Convert.ToInt32(islemNo),"");
            }
            else if (secim == 7)
            {
                Console.Clear();
                Banka banka = new Banka();
                banka.BankaMenusu();
            }
            else if (secim == 8)
            {
                System.Environment.Exit(1);
            }
            else
            {
                HataGoster("* Geçersiz bir seçim yaptınız. Lütfen tekrar deneyiniz.","");
            }
        }
       
        public void ParaYatir(string hesapNo, decimal yatirilanTutar, DateTime islemTarihi,string aciklama,string menuSecim) 
        {
            foreach (var hesap in Banka.hesaplar)
            {
                //Para yatırma işlemini tarihi, kar kontrol tarihinden büyük veya eşitse ve bugünün tarihinden küçük veya eşitse işlemi gerçekleştiriyoruz.
                if (islemTarihi >= hesap.karKontrolTarihi && islemTarihi<=DateTime.Now.Date)
                {
                    if (hesap.hesapNumarasi == hesapNo)
                    {
                        int islemNo = islemNumarasiOlustur(); //islemNumarasiOlustur metoduyla yeni ve benzersiz bir işlem numarası oluşturuyoruz.
                        int cekilisHakki = Convert.ToInt32(yatirilanTutar) / 1000; //Yatırılan tutarı 1000 e bölerek kaç çekiliş hakkı elde ettiğini buluyoruz ve çıkan sonucu işlemler kısmında cekilisHakki kısmına ekliyoruz.

                        //Para yatırma işlemini işlemler listesine ekliyoruz.
                        islemler.Add(new Hesap() { islemNumarasi = islemNo, tutar = yatirilanTutar, islemTuru = "Para Yatırma", oncekiBakiye = hesap.hesapBakiyesi, hesapBakiyesi = hesap.hesapBakiyesi + yatirilanTutar, hesapNumarasi = hesap.hesapNumarasi, description = aciklama, islemTarihi = islemTarihi, cekilisHakki=cekilisHakki });
                        hesap.hesapBakiyesi = hesap.hesapBakiyesi + yatirilanTutar; //Hesap bakiyesine yatırılan tutarı ekliyoruz.
                        islemDekontu(islemNo,hesap.hesapNumarasi,hesap.adSoyad, menuSecim); //Yapılan işlemin dekontunu ekrana yazdırıyoruz.
                    }
                    else
                    {
                        hata = "* Hesap numarasını yanlış girdiniz. Lütfen kontrol edip tekrar deneyiniz.";
                    }
                }
                else
                {
                    hata = "* Girdiğiniz tarih hesap açılış tarihinden veya son para çektiğiniz tarihten eski. Lütfen daha yeni bir tarih girerek tekrar deneyiniz.";
                }
            }
            if (!string.IsNullOrEmpty(hata))
            {
                HataGoster(hata, menuSecim);
            }
            
        }

        //İlgili hesaptan para çekme işlemini gerçekleştirir.
        public void ParaCek(string hesapNo, decimal cekilenTutar, string aciklama, string menuSecim)
        {
            Banka banka = new Banka();
            decimal toplamHesapBakiyesi=0;
            DateTime islemTarihi=DateTime.Now.Date;
            foreach (var hesap in Banka.hesaplar)
            {
                if (hesap.hesapNumarasi == hesapNo)
                {
                    //Kar hesabını bulmak için hesap bilgilerini karTutarı isimli metodumuza gönderiyoruz ve sonucu alıyoruz.
                    toplamHesapBakiyesi =karTutari(hesap.hesapNumarasi,hesap.hesapBakiyesi,hesap.karKontrolTarihi,hesap.hesapTuru);
                   
                    //Kar hesabınında eklendiği toplam hesap bakiyesinden, çekilmek istenen tutarın büyük olup olmadığı kontrol ediyoruz. 
                    if (toplamHesapBakiyesi >= cekilenTutar)
                    {
                        int islemNo = islemNumarasiOlustur(); //islemNumarasiOlustur metoduyla yeni ve benzersiz bir işlem numarası oluşturuyoruz.
                        int cekilisHakki = Convert.ToInt32(cekilenTutar) / 1000; //Çekilen tutarı 1000 e bölerek kaç çekiliş hakkı elde ettiğini buluyoruz ve çıkan sonucu işlemler kısmında cekilisHakki kısmına ekliyoruz.
                        hesap.hesapBakiyesi = toplamHesapBakiyesi - cekilenTutar; //Hesap bakiyesinden çekilmek istenen tutarı çıkarıyoruz.
                        hesap.karKontrolTarihi = DateTime.Now.Date; // kar kontrol tarihine paranın  çekildiği yani bugünün tarihini atıyoruz.

                        //Para çekme işlemini işlemler listesine ekliyoruz.
                        islemler.Add(new Hesap() { islemNumarasi = islemNo, tutar = cekilenTutar, islemTuru = "Para Çekme", oncekiBakiye = toplamHesapBakiyesi, hesapBakiyesi = hesap.hesapBakiyesi, hesapNumarasi = hesap.hesapNumarasi, description = aciklama, islemTarihi = islemTarihi, cekilisHakki = cekilisHakki });
                        islemDekontu(islemNo, hesap.hesapNumarasi, hesap.adSoyad, menuSecim); //Yapılan işlemin dekontunu ekrana yazdırıyoruz.
                    }
                    else
                    {
                        hata = "* Hesap bakiyeniniz yetersiz. Lütfen hesabınızdaki parayı kontrol edip tekrar deneyiniz.";
                    }
                    
                }
                else
                {
                    hata = "* Hesap numarasını yanlış girdiniz veya bugün içinde para çekme işlemi gerçekleştirdiniz. Lütfen kontrol edip tekrar deneyiniz.";
                }
            }
            if (!string.IsNullOrEmpty(hata))
            {
                HataGoster(hata, menuSecim);
            }

        }
        public void hesapDurum(string hesapNo,string menuSecim)
        {
            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n     KTÜ BANKA SİSTEMİ\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("***************************");
            if (Banka.hesaplar.Count != 0 && !string.IsNullOrEmpty(hesapNo))
            {
                foreach (var hesap in Banka.hesaplar)
                {
                    if (hesap.hesapNumarasi == hesapNo)
                    {
                        //Hesabın güncel bilgilerini ekrana yazdırıyoruz.
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("       Hesap Durumu");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("       ---------------                   ");
                        Console.WriteLine("Hesap No           : " + hesap.hesapNumarasi);
                        Console.WriteLine("Hesap Sahibi       : " + hesap.adSoyad);
                        Console.WriteLine("Hesap Türü         : " + hesap.hesapTuru);
                        Console.WriteLine("Bakiye             : " + string.Format("{0:0,0.00}", bakiye)+" TL");
                        Console.WriteLine("Kar                : " + string.Format("{0:0,0.00}", toplamKar) + " TL");
                        Console.WriteLine("Toplam Bakiye      : " + string.Format("{0:0,0.00}", toplamBakiye) + " TL");
                        Console.WriteLine("Hesap Açılış Tarihi: " + hesap.islemTarihi.ToString("dd/MM/yyyy"));
                        hata = "";
                    }
                    else
                    {
                        hata = "* Hesap numarasını yanlış girdiniz. Lütfen kontrol edip tekrar deneyiniz.";
                    }
                }
            }
            else
            {
                hata = "* Görüntülenecek bir hesap bulunamadı. Lütfen hesap numarasını kontrol edip tekrar deneyiniz.";
            }

            if (!string.IsNullOrEmpty(hata))
            {
                HataGoster(hata, menuSecim);
            }

            Console.WriteLine("\nMenüye dönmek için klavyeden herhangi bir tuşa basınız...");
            Console.ReadKey();
            Console.Clear();

            if (menuSecim=="anamenu")
            {
                Banka banka = new Banka();
                banka.BankaMenusu();
            }
            else
            {
                HesapMenusu(hesapNumarasi, adSoyad);
            }
        }

        //Random fonksiyonu ile rastgele 10000000, 100000000 sayıları arasında 8 haneli bir hesap numarası oluşturuyoruz. 
        public string hesapNumarasiOlustur()
        {
            Random random = new Random();
            string hesapNo = random.Next(10000000, 100000000).ToString();
            
            //Hesap numarasının benzer olmaması için hesapNumaralari isimli listemizdeki hesap numaraları içerisinde aratıyoruz.Eğer benzer numara varsa metodumuzu yeniden çalıştırıyoruz.
            if (Banka.hesapNumaralari.IndexOf(hesapNo)!=-1)
            { 
                hesapNumarasiOlustur();
            }
            return hesapNo;
        }

        //1000 den başlayıp her işlemde sırayla artan bir işlem numarası oluşturuyoruz.
        public int islemNumarasiOlustur()
        {
            yeniIslemNumarasi = islemNumarasi + yeniIslemNumarasi;
            return yeniIslemNumarasi;
        }

        //Para çekme işlemi esnasında hesabın türüne göre kâr oranını hesaplar ve toplam bakiyeyi sonuç olarak döndürür.
        public decimal karTutari(string hesapNo, decimal hesapBakiyesi, DateTime islemTarihi,string hesapTuru)
        {
            decimal toplamHesapBakiyesi = hesapBakiyesi;
            decimal karYuzdesi=0;
            DateTime mevcutIslemTarihi = islemTarihi;
            decimal yillikFaiz;
            decimal gunlukFaiz;
            decimal kar;

            //Hesap türüne göre kâr yüzdesini belirliyoruz.Eğer cari hesap ise kar hesabı yapmadan direkt hesap bakiyesini yönlendiriyoruz. 
            if (hesapTuru=="Kısa Vadeli Hesap")
            {
                karYuzdesi = 15;
            }
            else if(hesapTuru == "Uzun Vadeli Hesap"){
                karYuzdesi = 17;
            }
            else if(hesapTuru == "Özel Hesap"){
                karYuzdesi = 10;
            }
            else if(hesapTuru == "Cari Hesap"){
                return hesapBakiyesi;
            }
            
            //İşlemleri tarih eskiden yeniye sırasına göre sıralıyoruz.
            foreach (var islem in islemler.OrderBy(d=>d.islemTarihi))
            {
                //Hesap numarası eşleşiyorsa ve işlem türü para yatırma işlemiyse iki tarih aralığındaki kâr oranını hesaplıyoruz.
                if (islem.hesapNumarasi == hesapNo && islem.islemTuru=="Para Yatırma")
                {
                    //iki tarih arasındaki gün sayısını buluyoruz.
                    TimeSpan gunSayisi = islem.islemTarihi.Subtract(mevcutIslemTarihi);

                    //Gün sayısı 0 dan büyükse kâr oranını hesaplama işlemi yapıyoruz ve toplam bakiyeye ekliyoruz.
                    if (gunSayisi.Days >0)
                    {
                        yillikFaiz = islem.oncekiBakiye * (karYuzdesi / 100);
                        gunlukFaiz = yillikFaiz / 365;
                        kar = gunlukFaiz * Convert.ToDecimal(gunSayisi.Days);
                        toplamHesapBakiyesi = toplamHesapBakiyesi + kar;
                        mevcutIslemTarihi = islem.islemTarihi;
                    }
                }
            }

            //Son para yatırma işlemi ile bugünün tarihi arasındaki gün sayısını bulup, kâr oranını hesaplayıp toplam bakiyeye ekliyoruz.
            TimeSpan gun = DateTime.Now.Subtract(mevcutIslemTarihi);
            if (gun.Days > 0)
            {               
                yillikFaiz = hesapBakiyesi * (karYuzdesi / 100);
                gunlukFaiz = yillikFaiz / 365;
                kar = gunlukFaiz * Convert.ToDecimal(gun.Days);
                toplamHesapBakiyesi = toplamHesapBakiyesi + kar;
            }


            //Toplam bakiye değerini döndürüyoruz.
            return Math.Floor(toplamHesapBakiyesi);
        }

        //İlgili hesabın yaptığı bütün işlemleri listeler.
        public void hesapOzeti(string hesapNo,string menuSecim)
        {
            int count = 0;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n                KTÜ BANKA SİSTEMİ\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("***************************************************************");
            foreach (var hesap in Banka.hesaplar)
            {
                if (hesap.hesapNumarasi == hesapNo)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("                  Hesap Özeti");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("                 ---------------  ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine(String.Format("{0,-10} | {1,-13} | {2,-15} | {3,-10}","İşlem No","İşlem Türü","İşlem Tutarı","İşlem Tarihi"));
                    Console.ForegroundColor = ConsoleColor.White;
                    foreach (var islem in islemler)
                    {
                        if (islem.hesapNumarasi == hesapNo)
                        {
                            Console.WriteLine("--------------------------------------------------------");
                           // Console.WriteLine("Bakiye             : " + string.Format("{0:0,0.00}", hesap.hesapBakiyesi) + " TL");
                            Console.WriteLine(String.Format("{0,-10} | {1,-13} | {2,-15} | {3,-10}", islem.islemNumarasi,islem.islemTuru, string.Format("{0:0,0.00} TL", islem.tutar),islem.islemTarihi.ToString("dd/MM/yyyy")));
                            count++;
                        }
                    }
                    Console.WriteLine("\n" + count + " adet hesap listelendi.");
                    hata = "";
                    break;
                }
                else
                {
                    hata = "* Hesap numarasını yanlış girdiniz. Lütfen kontrol edip tekrar deneyiniz.";
                }
            }
            if (!string.IsNullOrEmpty(hata))
            {
                HataGoster(hata, menuSecim);
            }

            Console.WriteLine("\nİşlem dekontu almak istiyorsanız lütfen space tuşuna basınız.");

            //Kullanıcı eğer space tuşuna basarsa, işlem numarası girebileceği bir seçenek çıkacak ve bu sayede istediği işlemin dekontunu alabilecek.
            if(Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                try
                {
                    Console.Write("\nDekontunu görmek istediğiniz işlemin işlem numarasını giriniz: ");
                    int islemNo = Convert.ToInt32(Console.ReadLine());
                    islemDekontu(islemNo,hesapNo,adSoyad,menuSecim);
                }
                catch
                {
                    HataGoster("* Girdiğiniz işlem numarası geçersiz. Lütfen kontrol edip tekrar deneyiniz.",menuSecim);
                }
                
            }
            else
            {
                Console.WriteLine("\nMenüye dönmek için klavyeden herhangi bir tuşa basınız...");
                Console.ReadKey();
                Console.Clear();

                if (menuSecim == "anamenu")
                {
                    Banka banka = new Banka();
                    banka.BankaMenusu();
                }
                else
                {
                    HesapMenusu(hesapNo, adSoyad);
                }
            }
 
        }

        //Yeni hesap, para yatırma ve çekme işlemlerinin dekontunu oluşturur. İstenilen dekonta ulaşmak için hesap işlem kayıtları menüsüne girilmelidir.  
        public void islemDekontu(int islemNo,string hesapNo, string adSoyad, string menuSecim)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n     KTÜ BANKA SİSTEMİ\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("***************************");
            foreach (var islem in islemler)
            {
                if (islem.islemNumarasi==islemNo)
                {
                    //İşlem bilgilerini ekrana bastırır.
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("       İşlem Dekontu");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("      ---------------  ");
                    Console.WriteLine("İşlem Tarihi   : "+islem.islemTarihi.ToString("dd/MM/yyyy"));
                    Console.WriteLine("İşlem Numarası : "+islem.islemNumarasi);
                    Console.WriteLine("İşlem Türü     : "+islem.islemTuru);
                    Console.WriteLine("İşlem Tutarı   : " + string.Format("{0:0,0.00}", islem.tutar) + " TL"); //string format ile paramızın gösterim şeklini belirliyoruz.
                    Console.WriteLine("Önceki Bakiye  : " + string.Format("{0:0,0.00}", islem.oncekiBakiye) + " TL");
                    Console.WriteLine("Sonraki Bakiye : " + string.Format("{0:0,0.00}", islem.hesapBakiyesi) + " TL");
                    Console.WriteLine("Açıklama       : "+islem.description);
                }
            }
            Console.WriteLine("\nMenüye dönmek için klavyeden herhangi bir tuşa basınız...");
            Console.ReadKey();
            Console.Clear();

            if (menuSecim == "anamenu")
            {
                Banka banka = new Banka();
                banka.BankaMenusu();
            }
            else
            {
                HesapMenusu(hesapNo, adSoyad);
            }
        }

        //Gelen hata mesajını ekrana bastırır. Kullanıcı en son hangi menüdeyse ona yönlendirir.
        public void HataGoster(string hata,string menu)
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(hata + "\n");
            Console.ForegroundColor = ConsoleColor.White;
            if (menu == "anamenu")
            {
                Banka banka=new Banka();
                banka.BankaMenusu();
            }
            HesapMenusu(hesapNumarasi,adSoyad);
        }
    }
}
