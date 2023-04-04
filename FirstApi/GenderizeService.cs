using Newtonsoft.Json;

namespace FirstApi;

public class GenderizeService
{
    private readonly HttpClient _httpClient;

    public GenderizeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetGender(string name)
    {
        var response = await _httpClient.GetAsync($"https://api.genderize.io?name={name}");

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var gender = JsonConvert.DeserializeObject<GenderResponse>(json);

        return gender.Gender;
    }
}