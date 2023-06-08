using System;
using MySqlConnector;

class Program
{
    static void Main()
    {
        string connectionString = "server=localhost;port=3306;database=kutuphane;uid=root;pwd=serhat123;";

        Console.WriteLine("Üye Adı: ");
        string uyeAd = Console.ReadLine();

        Console.WriteLine("Üye Soyadı: ");
        string uyeSoyad = Console.ReadLine();

        Console.WriteLine("Üye E-posta: ");
        string uyeEmail = Console.ReadLine();

        Console.WriteLine("Kitap Adı: ");
        string kitapAd = Console.ReadLine();

        Console.WriteLine("Yazar Adı: ");
        string yazarAd = Console.ReadLine();

        Console.WriteLine("Kategori Adı: ");
        string kategoriAd = Console.ReadLine();

        Console.WriteLine("Ödünç Tarihi (yyyy-MM-dd): ");
        string oduncTarihiStr = Console.ReadLine();
        DateTime oduncTarihi = DateTime.Parse(oduncTarihiStr);

        Console.WriteLine("Teslim Tarihi (yyyy-MM-dd): ");
        string teslimTarihiStr = Console.ReadLine();
        DateTime teslimTarihi = DateTime.Parse(teslimTarihiStr);

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Üye ekleme
            int uyeId = InsertUye(connection, uyeAd, uyeSoyad, uyeEmail);

            // Yazar ekleme
            int yazarId = InsertYazar(connection, yazarAd);

            // Kategori ekleme
            int kategoriId = InsertKategori(connection, kategoriAd);

            // Kitap ekleme
            int kitapId = InsertKitap(connection, kitapAd, yazarId, kategoriId);

            // Ödünç ekleme
            InsertOdunc(connection, uyeId, kitapId, oduncTarihi, teslimTarihi);
        }

        Console.WriteLine("Kayıt eklendi.");
        Console.ReadLine();
    }

    static int InsertUye(MySqlConnection connection, string uyeAd, string uyeSoyad, string uyeEmail)
    {
        string query = "INSERT INTO Uyeler (uye_ad, uye_soyad, uye_email) VALUES (@uyeAd, @uyeSoyad, @uyeEmail); SELECT LAST_INSERT_ID();";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@uyeAd", uyeAd);
            command.Parameters.AddWithValue("@uyeSoyad", uyeSoyad);
            command.Parameters.AddWithValue("@uyeEmail", uyeEmail);

            int uyeId = Convert.ToInt32(command.ExecuteScalar());
            return uyeId;
        }
    }

    static int InsertYazar(MySqlConnection connection, string yazarAd)
    {
        string query = "INSERT INTO Yazarlar (yazar_ad) VALUES (@yazarAd); SELECT LAST_INSERT_ID();";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@yazarAd", yazarAd);

            int yazarId = Convert.ToInt32(command.ExecuteScalar());
            return yazarId;
        }
    }

    static int InsertKategori(MySqlConnection connection, string kategoriAd)
    {
        string query = "INSERT INTO Kategoriler (kategori_ad) VALUES (@kategoriAd); SELECT LAST_INSERT_ID();";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@kategoriAd", kategoriAd);

            int kategoriId = Convert.ToInt32(command.ExecuteScalar());
            return kategoriId;
        }
    }

    static int InsertKitap(MySqlConnection connection, string kitapAd, int yazarId, int kategoriId)
    {
        string query = "INSERT INTO Kitaplar (kitap_ad, yazar_id, kategori_id) VALUES (@kitapAd, @yazarId, @kategoriId); SELECT LAST_INSERT_ID();";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@kitapAd", kitapAd);
            command.Parameters.AddWithValue("@yazarId", yazarId);
            command.Parameters.AddWithValue("@kategoriId", kategoriId);

            int kitapId = Convert.ToInt32(command.ExecuteScalar());
            return kitapId;
        }
    }

    static void InsertOdunc(MySqlConnection connection, int uyeId, int kitapId, DateTime oduncTarihi, DateTime teslimTarihi)
    {
        string query = "INSERT INTO Odunc (uye_id, kitap_id, odunc_tarihi, teslim_tarihi) VALUES (@uyeId, @kitapId, @oduncTarihi, @teslimTarihi)";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@uyeId", uyeId);
            command.Parameters.AddWithValue("@kitapId", kitapId);
            command.Parameters.AddWithValue("@oduncTarihi", oduncTarihi);
            command.Parameters.AddWithValue("@teslimTarihi", teslimTarihi);

            command.ExecuteNonQuery();
        }
    }
}

