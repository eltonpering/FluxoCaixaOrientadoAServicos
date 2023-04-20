using Newtonsoft.Json;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        int currentPage = 0;
        int totalPages = 0;

        while (currentPage <= totalPages)
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={currentPage}";

            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                dynamic data = JsonConvert.DeserializeObject(content);

                totalPages = data.total_pages;

                foreach (var match in data.data)
                {
                    totalGoals += (int)match["team1goals"];
                }
            }

            currentPage++;
        }

        return totalGoals;
    }
}