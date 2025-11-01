using System;
using System.Collections.Generic;
using System.Linq;

class BelirsizlikKararVerme
{
    static void Main() {
        try
        {
            Console.Write("Alternatif sayısını giriniz: ");
            int satir = int.Parse(Console.ReadLine()); //parse ile tam sayıya çeviriyoruz.

            Console.Write("Olasılık sayısını giriniz: ");
            int sutun = int.Parse(Console.ReadLine());

            int[,] kararMatrisi = new int[satir, sutun]; //matris oluşturduk arkaplanda.

            Console.WriteLine("Karar matrisi değerlerini girin:"); //matris değerlerini istiyoruz.
            for (int i = 0; i < satir; i++) {
                for (int j = 0; j < sutun; j++) {
                    Console.Write($"Değer [A{i + 1}, S{j + 1}]: "); //kullanıcı, değeri hangi hücreye giriyor
                    while (!int.TryParse(Console.ReadLine(), out kararMatrisi[i, j])) //değerler tam sayı mı bakıyoruz.
                    {
                        Console.WriteLine("Geçersiz giriş! Lütfen bir tam sayı girin:"); //tam sayı değilse uyarı veriyoruz.
                        Console.Write($"Değer [A{i + 1}, S{j + 1}]: "); //yeniden o hücreyi soruyoruz.
                    }
                }
            }
            Console.WriteLine("\nGirilen Karar Matrisi:"); //matris şeklinde ekrana yazdırıyoruz.
            for (int i = 0; i < satir; i++) {
                    for (int j = 0; j < sutun; j++)
                    {
                        Console.Write($"{kararMatrisi[i, j]} ");
                    }
                    Console.WriteLine();
                }

            Console.Write("\nHurwicz ölçütü için iyimserlik katsayısı (0 ile 1 arasında bir değer) girin: ");
            double alpha;
            while (!double.TryParse(Console.ReadLine(), out alpha) || alpha < 0 || alpha > 1) //0 ile 1 arasında mı
            {
                Console.WriteLine("Geçersiz giriş! Lütfen 0 ile 1 arasında bir sayı girin:"); //değilse uyrı mesajı veriyoruz.
            }


            Console.WriteLine("\nKötümserlik Ölçütü Kararı: " + GetSatirAdi(Maximin(kararMatrisi))); //sonuçları çağırma ve yazdırma.
            Console.WriteLine("İyimserlik Ölçütü  Kararı: " + GetSatirAdi(Maximax(kararMatrisi)));
            Console.WriteLine("Eş Olasılık Ölçütü (Laplace) Kararı: " + GetSatirAdi(Laplace(kararMatrisi)));
            Console.WriteLine("Pişmanlık Ölçütü (Savage) Kararı: " + GetSatirAdi(MinimaxRegret(kararMatrisi)));
            Console.WriteLine("Uzlaşma Ölçütü (Hurwicz) Kararı: " + GetSatirAdi(Hurwicz(kararMatrisi, alpha)));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Bir hata oluştu: " + ex.Message);
        }
    }


    static string GetSatirAdi(int satirIndex) //olasılık adlarını belirler (A1, A2...)
    {
        return $"A{satirIndex + 1}";
    }

    static int Maximin(int[,] matrix)
    {
        List<int> minValues = new List<int>(); //her satırın en küçük değerini saklama listesi.
        int bestIndex = 0; //en iyi alternatifin indeksini tutuyoruz.

        for (int i = 0; i < matrix.GetLength(0); i++) 
        {
            int min = int.MaxValue;
            
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] < min) //her satırdaki değerleri kontrol edip min değeri alıyoruz.
                    min = matrix[i, j]; //min değeri, gerekirse güncelliyoruz.
            }
            minValues.Add(min); //en küçük değeri listeye ekliyoruz.
        }

        bestIndex = minValues.IndexOf(minValues.Max()); //minValues listesindeki max değerin indexini alıyoruz.
        return bestIndex; //en iyi alternatifin satır indexini döndürüyoruz.
    }

    static int Maximax(int[,] matrix)
    {
        List<int> maxValues = new List<int>(); //her satırın max değerlerini saklayan liste.
        int bestIndex = 0; //en iyi alternatifin indexini tutuyoruz.

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            int max = int.MinValue;
            
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] > max) //her satırdaki değerleri tek tek kontrol edip max değeri buluyoruz.
                    max = matrix[i, j];
            }
            maxValues.Add(max); //o satırın max değerini listeye ekliyoruz.
        }

        bestIndex = maxValues.IndexOf(maxValues.Max()); //maxValues listesindeki değerler içinden en büyüğünü buluyoruz.
        return bestIndex; //en iyi alternatifin satır indexini döndürüyoruz.
    }

    static int Laplace(int[,] matrix)
    {
        List<double> averageValues = new List<double>(); //her satırın ortalama değerini saklayan liste.
        int bestIndex = 0; //en iyi alternatifin indeksini tutuyoruz.

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            double sum = 0; //başlangıç değeri 0.
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                sum += matrix[i, j]; //satırdaki tüm değerleri topluyoruz.
            }
            averageValues.Add(sum/matrix.GetLength(1)); /*toplam değeri, olasılık (sütun) sayısına 
            bölerek ortalamasını alıyoruz ve averageValues listesine ekliyoruz.*/
        }

        bestIndex = averageValues.IndexOf(averageValues.Max()); //ortalaması en büyük olanı seçiyoruz.
        return bestIndex; //ortalama değeri en büyük olan alternatifin indexini döndürüyoruz.
    }

    static int MinimaxRegret(int[,] matrix)
    {
        //pişmanlık değerleri için regretMatrix adında yeni matris oluşturuyoruz.
        int[,] regretMatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
        List<int> maxRegrets = new List<int>();
        int bestIndex = 0; //başlangıç değeri.

        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            int columnMax = int.MinValue; //sütun max değeri olarak en küçük sayı ayarlanır.
            
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, j] > columnMax) //sütun üzerinde kıyas yaparak en büyüğü bulana kadar döngü sürüyor.
                    columnMax = matrix[i, j]; //en büyüğü için güncelleme yapıyoruz.
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                //sütun boyunca dolaşıp max değer alan hücreden diğer hücre değerlerini çıkarıyoruz.
                regretMatrix[i, j] = columnMax - matrix[i, j]; //yeni regretMatrix matrisi oluştu.
            }
        }

        for (int i = 0; i < regretMatrix.GetLength(0); i++)
        {
            int maxRegret = int.MinValue; //en düşük olarak başlangıç ayarlandı.
            for (int j = 0; j < regretMatrix.GetLength(1); j++)
            {
                //tüm satırları kontrol ederek en büyük pişmalık değerini buluyoruz.
                if (regretMatrix[i, j] > maxRegret) 
                    maxRegret = regretMatrix[i, j]; //en büyüğünü bulana kadar güncelliyoruz.
            }
            maxRegrets.Add(maxRegret); //her satırın max pişmanlık değeri maxRegrets'e eklenir.
        }
        //maxRegrets listesindeki en küçük değerin indeksini buluyoruz.
        bestIndex = maxRegrets.IndexOf(maxRegrets.Min());
        return bestIndex; //en iyi alternatifin indeksini döndürüyoruz.
    }

    static int Hurwicz(int[,] matrix, double alpha)
    {
        List<double> hurwiczValues = new List<double>(); //her satırın hurwicz değerini saklayan liste.
        int bestIndex = 0; //başlangıç değeri 0.

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            int max = int.MinValue; //her satırdaki en küçük değer
            int min = int.MaxValue; //her satırdaki en büyük değer

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                //eldeki değerleri tek tek kıyaslıyoruz ve en küçük ve en büyük değerleri buluyoruz.
                if (matrix[i, j] > max) 
                    max = matrix[i, j];
                if (matrix[i, j] < min)
                    min = matrix[i, j];
            }
            //formülü uyguluyoruz.
            double hurwiczValue = alpha * max + (1 - alpha) * min;
            hurwiczValues.Add(hurwiczValue);
        }
        //hurwicz değeri en büyük olan seçilir.
        bestIndex = hurwiczValues.IndexOf(hurwiczValues.Max());
        return bestIndex; //en iyi alternatif indexini döndürüyoruz.
    }
}