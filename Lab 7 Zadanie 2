using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Data.SqlClient;

class Program
{
    static List<Osoba> osoby = new List<Osoba>();

    static void Main()
    {
        WczytajDaneZPliku();

        while (true)
        {
            Console.WriteLine("Menu główne:");
            Console.WriteLine("1. Wyświetl dane");
            Console.WriteLine("2. Dodaj osobę");
            Console.WriteLine("3. Modyfikuj osobę");
            Console.WriteLine("4. Usuń osobę");
            Console.WriteLine("5. Wyjście z programu");

            Console.Write("Wybierz opcję (1-5): ");
            string wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    WyswietlDane();
                    break;

                case "2":
                    DodajOsobe();
                    break;

                case "3":
                    ModyfikujOsobe();
                    break;

                case "4":
                    UsunOsobe();
                    break;

                case "5":
                    ZapiszDaneDoPliku();
                    ZapiszDaneDoBazyDanych();
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }

    static void WyswietlDane()
    {
        Console.WriteLine("Dane osób:");

        foreach (var osoba in osoby)
        {
            Console.WriteLine($"Imię: {osoba.Imie}, Nazwisko: {osoba.Nazwisko}, PESEL: {osoba.Pesel}, Email: {osoba.Email}, Adres: {osoba.Adres.Ulica}, {osoba.Adres.Miasto}");
        }

        Console.WriteLine();
    }

    static void DodajOsobe()
    {
        try
        {
            Osoba nowaOsoba = new Osoba();

            Console.Write("Podaj imię: ");
            nowaOsoba.Imie = Console.ReadLine();

            Console.Write("Podaj nazwisko: ");
            nowaOsoba.Nazwisko = Console.ReadLine();

            Console.Write("Podaj ulicę zamieszkania: ");
            nowaOsoba.Adres.Ulica = Console.ReadLine();

            Console.Write("Podaj miasto zamieszkania: ");
            nowaOsoba.Adres.Miasto = Console.ReadLine();

            Console.Write("Podaj PESEL (11 znaków): ");
            nowaOsoba.Pesel = Console.ReadLine();

            if (nowaOsoba.Pesel.Length != 11)
            {
                throw new Exception("Błędny PESEL. PESEL powinien mieć 11 znaków.");
            }

            Console.Write("Podaj adres email: ");
            nowaOsoba.Email = Console.ReadLine();

            if (!nowaOsoba.Email.Contains("@"))
            {
                throw new Exception("Błędny format adresu email. Brak znaku @.");
            }

            osoby.Add(nowaOsoba);
            Console.WriteLine("Osoba dodana do bazy.");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
            Console.WriteLine("Dodawanie osoby nie powiodło się. Spróbuj ponownie.");
        }
    }

    static void ModyfikujOsobe()
    {
        Console.Write("Podaj PESEL osoby do modyfikacji: ");
        string pesel = Console.ReadLine();

        Osoba osobaDoModyfikacji = osoby.FirstOrDefault(o => o.Pesel == pesel);

        if (osobaDoModyfikacji != null)
        {
            Console.WriteLine($"Znaleziono osobę: {osobaDoModyfikacji.Imie} {osobaDoModyfikacji.Nazwisko}");

            Console.Write("Podaj nowe imię: ");
            osobaDoModyfikacji.Imie = Console.ReadLine();

            Console.Write("Podaj nowe nazwisko: ");
            osobaDoModyfikacji.Nazwisko = Console.ReadLine();

            Console.Write("Podaj nową ulicę zamieszkania: ");
            osobaDoModyfikacji.Adres.Ulica = Console.ReadLine();

            Console.Write("Podaj nowe miasto zamieszkania: ");
            osobaDoModyfikacji.Adres.Miasto = Console.ReadLine();

            Console.Write("Podaj nowy adres PESEL: ");
            osobaDoModyfikacji.Pesel = Console.ReadLine();

            Console.Write("Podaj nowy adres email: ");
            osobaDoModyfikacji.Email = Console.ReadLine();

            Console.WriteLine("Osoba została zaktualizowana.");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Nie znaleziono osoby o podanym PESELu.");
            Console.WriteLine();
        }
    }

    static void UsunOsobe()
    {
        Console.Write("Podaj PESEL osoby do usunięcia: ");
        string pesel = Console.ReadLine();

        Osoba osobaDoUsuniecia = osoby.FirstOrDefault(o => o.Pesel == pesel);

        if (osobaDoUsuniecia != null)
        {
            osoby.Remove(osobaDoUsuniecia);
            Console.WriteLine("Osoba została usunięta z bazy.");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Nie znaleziono osoby o podanym PESELu.");
            Console.WriteLine();
        }
    }

    static void WczytajDaneZPliku()
    {
        try
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using (var reader = new StreamReader("dane.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                osoby = csv.GetRecords<Osoba>().ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas wczytywania danych z pliku: {ex.Message}");
        }
    }

    static void ZapiszDaneDoPliku()
    {
        try
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using (var writer = new StreamWriter("dane.csv"))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(osoby);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas zapisywania danych do pliku: {ex.Message}");
        }
    }

    static void ZapiszDaneDoBazyDanych()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("YourConnectionString")) // Zastąp odpowiednią wartością
            {
                connection.Open();

                // Usuń poprzednie dane
                using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM Osoby", connection))
                {
                    deleteCommand.ExecuteNonQuery();
                }

                // Zapisz nowe dane
                foreach (Osoba osoba in osoby)
                {
                    using (SqlCommand insertCommand = new SqlCommand(
                        "INSERT INTO Osoby (Imie, Nazwisko, Ulica, Miasto, Pesel, Email) " +
                        "VALUES (@Imie, @Nazwisko, @Ulica, @Miasto, @Pesel, @Email)", connection))
                    {
                        insertCommand.Parameters.AddWithValue("@Imie", osoba.Imie);
                        insertCommand.Parameters.AddWithValue("@Nazwisko", osoba.Nazwisko);
                        insertCommand.Parameters.AddWithValue("@Ulica", osoba.Adres.Ulica);
                        insertCommand.Parameters.AddWithValue("@Miasto", osoba.Adres.Miasto);
                        insertCommand.Parameters.AddWithValue("@Pesel", osoba.Pesel);
                        insertCommand.Parameters.AddWithValue("@Email", osoba.Email);

                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas zapisywania danych do bazy danych: {ex.Message}");
        }
    }
}

public class Osoba
{
    public string Imie { get; set; }
    public string Nazwisko { get; set; }
    public Adres Adres { get; set; } = new Adres();
    public string Pesel { get; set; }
    public string Email { get; set; }
}

public class Adres
{
    public string Ulica { get; set; }
    public string Miasto { get; set; }
}
