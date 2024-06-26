using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace DataLibrary;

/// <summary>
/// A class for working with csv files
/// </summary>
public class CSVProcessing
{
    private const string russianHeader = "global_id;Название спортивного объекта;Название спортивной зоны в зимний период;Фотография в зимний период;Административный округ;Район;Адрес;Адрес электронной почты;Адрес сайта;Справочный телефон;Добавочный номер;График работы в зимний период;Уточнение графика работы в зимний период;Возможность проката оборудования;Комментарии для проката оборудования;Наличие сервиса технического обслуживания;Комментарии для сервиса технического обслуживания;Наличие раздевалки;Наличие точки питания;Наличие туалета;Наличие точки Wi-Fi;Наличие банкомата;Наличие медпункта;Наличие звукового сопровождения;Период эксплуатации в зимний период;Размеры в зимний период;Освещение;Покрытие в зимний период;Количество оборудованных посадочных мест;Форма посещения (платность);Комментарии к стоимости посещения;Приспособленность для занятий инвалидов;Услуги предоставляемые в зимний период;geoData;geodata_center;geoarea";

    private string? PathToWriteData { get; set; }

    private static CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        Delimiter = ";",
        Encoding = Encoding.UTF8,
        MissingFieldFound = null,
    };

    public CSVProcessing(string? path = null)
    {
        PathToWriteData = path;
    }

    /// <summary>
    /// A method for reading data from a csv file into a list of objects
    /// </summary>
    /// <param name="lstWithData"></param>
    /// <param name="isWriteCorrect"></param>
    /// <returns></returns>
    public Stream Write(List<IceHillData> lstWithData, out bool isWriteCorrect)
    {
        if (PathToWriteData == null)
        {
            isWriteCorrect = false;
            return Stream.Null;
        }
        try
        {
            using StreamWriter sWriter = new StreamWriter(File.Open(PathToWriteData, FileMode.Create, FileAccess.ReadWrite));
            {
                using CsvWriter csvWriter = new CsvWriter(sWriter, configuration);
                {
                    csvWriter.WriteHeader<IceHillData>();
                    csvWriter.NextRecord();
                    sWriter.WriteLine(russianHeader);
                    foreach (IceHillData data in lstWithData)
                    {
                        csvWriter.WriteRecord<IceHillData>(data);
                        csvWriter.NextRecord();
                    }
                }
            }

            isWriteCorrect = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nError occured while handling CSV Write.");
            isWriteCorrect = false;
        }

        Stream streamOut = File.Open(PathToWriteData, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        return streamOut;
    }

    /// <summary>
    /// A method for writing data from a list of class objects to a file in csv format
    /// </summary>
    /// <param name="streamLoad"></param>
    /// <param name="IsConvertCorrect"></param>
    /// <returns></returns>
    public List<IceHillData> Read(FileStream streamLoad, out bool IsConvertCorrect)
    {
        try
        {
            List<IceHillData> lstWithData = new List<IceHillData>();
            using (StreamReader sReader = new StreamReader(streamLoad, Encoding.UTF8))
            {
                using CsvReader csvReader = new CsvReader(sReader, configuration);

                int cntRows = 0;
                while (csvReader.Read())
                {
                    cntRows++;
                    if (cntRows == 2) { continue; }
                    if (cntRows == 1)
                    {
                        csvReader.ReadHeader();
                        continue;
                    }
                    lstWithData.Add(csvReader.GetRecord<IceHillData>());
                }
            }

            IsConvertCorrect = true;
            return lstWithData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nError occured while handling CSV Read.");
            IsConvertCorrect = false;
            return new List<IceHillData>();
        }
    }
}