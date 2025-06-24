using JsonSerializer = System.Text.Json.JsonSerializer;

namespace doc_push_service.DbCtx
{
    public static class EnrollmentsDb
    {
        private static HashSet<string> enrollments = new();

        public static string DbLocation = "./enrollments.db";

        static EnrollmentsDb()
        {
            if (File.Exists(EnrollmentsDb.DbLocation))
            {
                string jsondb = File.ReadAllText(EnrollmentsDb.DbLocation);
                var dbdata = JsonSerializer.Deserialize<List<string>>(jsondb);
                if (dbdata != null ) 
                {
                    enrollments = dbdata.ToHashSet<string>();
                }
            }
        }

        public static async Task<bool> InsertAsync(string url)
        {  
            bool result = enrollments.Add(url);

            if (result)
            {
                string jsonData = JsonSerializer.Serialize(enrollments);

                await File.WriteAllTextAsync(DbLocation, jsonData);
            }

            return result;
        }

        public static async Task<bool> DeleteAsync(string url)
        {
            bool result = enrollments.Remove(url);

            if (result)
            {
                string jsonData = JsonSerializer.Serialize(enrollments);

                await File.WriteAllTextAsync(@"c:\tmp\enrollments.db", jsonData);
            }

            return result;
        }

        public static bool Exists(string url)
        {
            return enrollments.Contains(url);
        }

        public static string[] List() 
        {
            return enrollments.ToArray();
        }
    }
}
