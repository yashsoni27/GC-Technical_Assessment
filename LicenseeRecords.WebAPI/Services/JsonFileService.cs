using Newtonsoft.Json;

namespace LicenseeRecords.WebAPI.Services
{
    public class JsonFileService<T>
    {
        private readonly string _filePath;

        public JsonFileService(string filePath)
        {
            _filePath = filePath;
        }

        public List<T> ReadJson()
        {
            if (!File.Exists(_filePath)) return new List<T>();
            var jsonData = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<T>>(jsonData);
        }

        public void WriteJson(List<T> data)
        {
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_filePath, jsonData);
        }
    }

}
