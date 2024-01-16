using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public interface IPersonRepository
{
    void AddPerson(Person person);
    List<Person> GetAllPersons();
}

public record Person(string FirstName, string LastName, int Age);

public class FilePersonRepository : IPersonRepository
{
    private readonly string filePath;

    public FilePersonRepository(string filePath)
    {
        this.filePath = filePath;
        InitializeFile();
    }

    private void InitializeFile()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, string.Empty);
        }
    }

    public void AddPerson(Person person)
    {
        List<Person> persons = GetAllPersons();
        persons.Add(person);

        SavePersonsToFile(persons);
    }

    public List<Person> GetAllPersons()
    {
        try
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Person>>(jsonData) ?? new List<Person>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
            return new List<Person>();
        }
    }

    private void SavePersonsToFile(List<Person> persons)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(persons, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to file: {ex.Message}");
        }
    }
}

class Program
{
    static void Main()
    {
        IPersonRepository personRepository = new FilePersonRepository("persons.json");

        Person newPerson = new Person("John", "Doe", 30);
        personRepository.AddPerson(newPerson);

        List<Person> allPersons = personRepository.GetAllPersons();

        Console.WriteLine("All Persons:");
        foreach (var person in allPersons)
        {
            Console.WriteLine($"{person.FirstName} {person.LastName}, Age: {person.Age}");
        }
    }
}
