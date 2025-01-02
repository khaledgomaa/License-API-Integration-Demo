using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace LicenseIntegrationAPIDemo
{
    internal class Program
    {
        private static readonly string _url = "http://localhost:22143/";
        private static readonly int _client_id = 5;
        private static readonly string _api_key = "417DE17321299EFE";
        private static readonly string _event_key = "F5C7193E788D8B8EFACB7AF52E02AD4D";

        static async Task Main(string[] args)
        {
            await GetLicenses();

            await GetLicenses("01-01-2024", DateTime.Now.ToString("MM-dd-yyyy"));

            await CreateLicense();

            await UpdateLicense("152z29g44");

            await DeleteLicense("9q51u74b4");
        }

        /// <summary>
        /// Get all licenses.
        /// </summary>
        /// <param name="startDate">Date format: MM-dd-yyyy</param>
        /// <param name="endDate">Date format: MM-dd-yyyy</param>
        /// <returns></returns>
        public static async Task GetLicenses(string startDate, string endDate)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_url);

                httpClient.DefaultRequestHeaders.Add("x-client-id", _client_id.ToString());
                httpClient.DefaultRequestHeaders.Add("x-api-key", _api_key);
                httpClient.DefaultRequestHeaders.Add("x-event-key", _event_key);

                var response = await httpClient.GetAsync($"api/v1/licenses?startDate={startDate}&endDate={endDate}");

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadFromJsonAsync<LicenseDto>();
                    Console.WriteLine(JsonSerializer.Serialize(content, new JsonSerializerOptions { WriteIndented = true }));
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
                        || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var result = await response.Content.ReadFromJsonAsync<AppResponse>();

                    Console.WriteLine($"Response: {result.Message}");
                }
                else
                {
                    var appError = await response.Content.ReadFromJsonAsync<AppError>();

                    switch (appError.ErrorNumber)
                    {
                        case 1005:
                            Console.WriteLine(appError.ErrorText);
                            // Write logic to handle StartDate and EndDate Only Supported Query Parameters Error
                            break;
                        case 1006:
                            Console.WriteLine(appError.ErrorText);
                            // Write logic to handle Invalid Date Format Error
                            break;
                        case 1007:
                            Console.WriteLine(appError.ErrorText);
                            // Write logic to handle Invalid StartDate Greater Than EndDate Error
                            break;
                        default:
                            Console.WriteLine(appError.ErrorText);
                            // Write logic to handle Unexpected Error
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Get all licenses.
        /// </summary>
        /// <returns></returns>
        public static async Task GetLicenses()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_url);

                httpClient.DefaultRequestHeaders.Add("x-client-id", _client_id.ToString());
                httpClient.DefaultRequestHeaders.Add("x-api-key", _api_key);
                httpClient.DefaultRequestHeaders.Add("x-event-key", _event_key);

                var response = await httpClient.GetAsync("api/v1/licenses");

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadFromJsonAsync<LicenseDto>();
                    Console.WriteLine(JsonSerializer.Serialize(content, new JsonSerializerOptions { WriteIndented = true }));
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
                        || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var result = await response.Content.ReadFromJsonAsync<AppResponse>();

                    Console.WriteLine($"Response: {result.Message}");
                }
                else
                {
                    var appError = await response.Content.ReadFromJsonAsync<AppError>();

                    switch (appError.ErrorNumber)
                    {
                        case 1005:
                            Console.WriteLine(appError.ErrorText);
                            // Write logic to handle StartDate and EndDate Only Supported Query Parameters Error
                            break;
                        default:
                            Console.WriteLine(appError.ErrorText);
                            // Write logic to handle Unexpected Error
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Create a new license.
        /// </summary>
        /// <returns></returns>
        public static async Task CreateLicense()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_url);

                httpClient.DefaultRequestHeaders.Add("x-client-id", _client_id.ToString());
                httpClient.DefaultRequestHeaders.Add("x-api-key", _api_key);
                httpClient.DefaultRequestHeaders.Add("x-event-key", _event_key);

                var request = new LicenseCreationRequest()
                {
                    FirstName = "Fairplicity",
                    LastName = "Fairplicity",
                    Email = "Fairplicity@Fairplicity.com",
                    LeadsEmail = "Fairplicity@Fairplicity.com",
                    Booth = "111",
                    Company = "Fairplicity",
                    HowManyLicenses = 2,
                    OrderType = "Access Code",
                    Phone = "+1 (914) 801-7016"
                };

                var response = await httpClient.PostAsJsonAsync("/api/v1/licenses", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AppResponse>();

                    Console.WriteLine($"Response: {result.Message}");
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
                        || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        var result = await response.Content.ReadFromJsonAsync<AppResponse>();

                        Console.WriteLine($"Response: {result.Message}");
                    }
                    else
                    {
                        var appError = await response.Content.ReadFromJsonAsync<AppError>();

                        switch (appError.ErrorNumber)
                        {
                            case 1000:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Missing Value for Specific Property Error
                                break;
                            case 1001:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Invalid Property Length Error
                                break;
                            case 1002:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Invalid Email Error
                                break;
                            default:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Unexpected Error
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update the license for the given <paramref name="userName"/>.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static async Task UpdateLicense(string userName)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_url);

                httpClient.DefaultRequestHeaders.Add("x-client-id", _client_id.ToString());
                httpClient.DefaultRequestHeaders.Add("x-api-key", _api_key);
                httpClient.DefaultRequestHeaders.Add("x-event-key", _event_key);

                var request = new LicenseCreationRequest()
                {
                    FirstName = "Fairplicity",
                    LastName = "Fairplicity",
                    Email = "Fairplicity@Fairplicity.com",
                    LeadsEmail = "Fairplicity@Fairplicity.com",
                    Booth = "111",
                    Company = "Fairplicity",
                    HowManyLicenses = 2,
                    OrderType = "Access Code",
                    Phone = "+1 (914) 801-7016"
                };

                var response = await httpClient.PutAsJsonAsync($"/api/v1/licenses/{userName}", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AppResponse>();

                    Console.WriteLine($"Response: {result.Message}");
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
                        || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        var result = await response.Content.ReadFromJsonAsync<AppResponse>();

                        Console.WriteLine($"Response: {result.Message}");
                    }
                    else
                    {
                        var appError = await response.Content.ReadFromJsonAsync<AppError>();

                        switch (appError.ErrorNumber)
                        {
                            case 1000:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Missing Value for Specific Property Error
                                break;
                            case 1001:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Invalid Property Length Error
                                break;
                            case 1002:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Invalid Email Error
                                break;
                            case 1004:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle License Already Used Error
                                break;
                            case 1009:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Account License is not found Error
                                break;
                            case 1011:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Unable to Update License for Child Account Error
                                break;
                            default:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Unexpected Error
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Delete a license by the <paramref name="userName"/>.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static async Task DeleteLicense(string userName)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_url);

                httpClient.DefaultRequestHeaders.Add("x-client-id", _client_id.ToString());
                httpClient.DefaultRequestHeaders.Add("x-api-key", _api_key);
                httpClient.DefaultRequestHeaders.Add("x-event-key", _event_key);

                var response = await httpClient.DeleteAsync($"/api/v1/licenses/{userName}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AppResponse>();

                    Console.WriteLine($"Response: {result.Message}");
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
                        || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        var content = await response.Content.ReadFromJsonAsync<AppResponse>();

                        Console.WriteLine($"Response: {content.Message}");
                    }
                    else
                    {
                        var appError = await response.Content.ReadFromJsonAsync<AppError>();

                        switch (appError.ErrorNumber)
                        {
                            case 1008:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Manager Account Not Found Error
                                break;
                            case 1009:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Account License Not Found Error
                                break;
                            case 1010:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Unable to Delete License for Child Account Error
                                break;
                            default:
                                Console.WriteLine(appError.ErrorText);
                                // Write logic to handle Unexpected Error
                                break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns in case of success.
    /// </summary>
    public class AppResponse
    {
        public string Message { get; set; }
    }

    /// <summary>
    /// Returns in case of an error.
    /// </summary>
    public class AppError
    {
        public int ErrorNumber { get; set; }
        public string ErrorText { get; set; }
        public string ErrorExceptionText { get; set; }
        public string ErrorDate { get; set; }
    }

    public class LicenseCreationRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string LeadsEmail { get; set; }

        public string Phone { get; set; }

        public string Company { get; set; }

        public string OrderType { get; set; }

        public int HowManyLicenses { get; set; }

        public string Booth { get; set; }
    }

    /// <summary>
    /// Retrieved licenses blueprint.
    /// </summary>
    public class LicenseDto
    {
        public IEnumerable<AccountLicenseDto> Licenses { get; set; }

        public int TotalIssuedLicenses { get; set; }

        public int TotalLicensesSetup { get; set; }

        public int TotalLicensesUsed { get; set; }

        public int TotalLicensesChargeable { get; set; }

        public int TotalNonSetupLicenses { get; set; }

        public int TotalPartiallySetupLicenses { get; set; }
    }

    public class AccountLicenseDto : AccountLicenseBaseDto
    {
        public string MasterUserName { get; set; }

        /// <summary>
        ///     <remarks>Could be retrieved only if account is not Secured.</remarks>
        /// </summary>
        public string MasterUserPassword { get; set; }

        public string Company { get; set; }

        public string Booth { get; set; }

        public string Phone { get; set; }

        public int LicensesIssued { get; set; }

        public int LicensesSetup { get; set; }

        public int LicensesUsed { get; set; }

        public int LicensesChargeable { get; set; }

        /// <summary>
        ///     Indicates that number of <see cref="LicensesSetup" /> is not equal to the number if <see cref="LicensesIssued" />
        /// </summary>
        public bool PartiallySetup { get; set; }

        /// <summary>
        ///     List of <see cref="AccountLicenseBaseDto.AccessCode" /> of ChildrenAccounts.
        /// </summary>
        public IEnumerable<string> AccessCodes { get; set; } = new List<string>();

        /// <summary>
        ///     List of <see cref="ChildAccountLicenseDto" /> if current account is a parent.
        /// </summary>
        public IEnumerable<ChildAccountLicenseDto> ChildrenAccounts { get; set; } = new List<ChildAccountLicenseDto>();
    }

    public class AccountLicenseBaseDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string LeadsEmails { get; set; }

        /// <summary>
        ///     Indicates account is Secured.
        /// </summary>
        public bool Activated { get; set; }

        public bool Deleted { get; set; }

        public string OrderType { get; set; }

        public string DeviceName { get; set; }

        public string DeviceId { get; set; }
    }

    public class ChildAccountLicenseDto : AccountLicenseBaseDto
    {
        public int ParentAcctId { get; set; }

        public string AccessCode { get; set; }

        public string ExhibitorUserName { get; set; }

        /// <summary>
        ///     <remarks>Could be retrieved only if account is not Secured.</remarks>
        /// </summary>
        public string ExhibitorPassword { get; set; }
    }
}
