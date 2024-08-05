using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;

public class Ord
{
    public string CustomerName { get; set; }
    public string Contact { get; set; }
    public List<Pos> PositionsInOrder { get; set; }
    public float TotalCost { get; set; }
    public DateTime Date { get; set; }
    public string Id { get; set; }

    public static List<Ord> orders = new List<Ord>();

}

public class Pos
{
    public string Name { get; set; }
    public float Cost { get; set; }
    public int Quantity { get; set; }
    public string category { get; set; }
    public string Id { get; set; }

    public static List<Pos> positions = new List<Pos>();
}

public class Serializer
{

    public void SerializeToJson(IEnumerable<object> collection, string filePath)
    {
        string json = JsonConvert.SerializeObject(collection, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public static List<Ord> DeserializeFromJson(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<Ord>>(json);
    }

    public void SerializeToCsv(IEnumerable<Ord> orders, string filePath)
    {
        StringBuilder csvContent = new StringBuilder();
        csvContent.AppendLine("CustomerName,Contact,TotalCost,Date,Id,PositionsInOrder,Category,PositionId");

        foreach (var order in orders)
        {
            string positions = string.Join("|", order.PositionsInOrder.Select(p => $"{p.Name},{p.Cost},{p.category},{p.Quantity},{p.Id}"));
            csvContent.AppendLine($"\"{order.CustomerName.Replace("\"", "\"\"")}\",\"{order.Contact.Replace("\"", "\"\"")}\",{order.TotalCost},{order.Date.ToString("yyyy-MM-dd")},\"{order.Id}\",\"{positions.Replace("\"", "")}\"");
        }

        File.WriteAllText(filePath, csvContent.ToString(), Encoding.UTF8);
    }
    public List<Ord> DeserializeFromCsv(string filePath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            PrepareHeaderForMatch = header => header.Header.ToLower()
        };

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, config))
        {
            var records = new List<Ord>();

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var order = csv.GetRecord<Ord>();

                var positionsInOrderField = csv.GetField(nameof(Ord.PositionsInOrder));
                var positionsInOrder = positionsInOrderField.Split('|')
                    .Select(position =>
                    {
                        var parts = position.Split(',');
                        return new Pos
                        {
                            Name = parts[0],
                            Cost = float.Parse(parts[1], CultureInfo.InvariantCulture),
                            category = parts[2],
                            Quantity = int.Parse(parts[3]),
                            Id = parts[4]
                        };
                    })
                    .ToList();

                order.PositionsInOrder = positionsInOrder;
                records.Add(order);
            }

            return records;
        }
    }
}