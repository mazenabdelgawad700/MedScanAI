using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SharedResponse;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MedScanAI.Service.Implementation
{
    internal class AIService : IAIService
    {
        private static readonly HttpClient _httpClient = new();

        public async Task<ReturnBase<ModelResponse>> GetBrainTumorModelResponseAsync(IFormFile image, string userRole)
        {
            var result = await GetModelResponseAsync(image, userRole, "8001");
            return result;
        }

        public async Task<ReturnBase<ModelResponse>> GetBreastCancerModelResponseAsync(IFormFile image, string userRole)
        {
            var result = await GetModelResponseAsync(image, userRole, "8006");
            return result;
        }

        public async Task<ReturnBase<ChatbotResponse>> GetChatbotResponseAsync(string message, string userRole)
        {
            try
            {
                var jsonRequest = JsonSerializer.Serialize(new { message });
                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                var url = $"http://localhost:8005/chat?user_role={userRole}";

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return ReturnBaseHandler.Failed<ChatbotResponse>($"API Error {response.StatusCode}: {error}");
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };

                var modelResponse = JsonSerializer.Deserialize<ChatbotResponse>(responseJson, options);

                if (modelResponse is null)
                    return ReturnBaseHandler.Failed<ChatbotResponse>("Failed to deserialize model response");

                return ReturnBaseHandler.Success(modelResponse);
            }
            catch (HttpRequestException ex)
            {
                return ReturnBaseHandler.Failed<ChatbotResponse>($"Connection failed — is FastAPI running on port 8005? {ex.Message}");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<ChatbotResponse>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<ModelResponse>> GetDermatologyModelResponseAsync(IFormFile image, string userRole)
        {
            var result = await GetModelResponseAsync(image, userRole, "8000");
            return result;
        }

        public async Task<ReturnBase<LabModelResponse>> GetLabResultsModelResponseAsync(IFormFile image, string userRole)
        {
            try
            {
                await using var imageStream = image.OpenReadStream();
                var imageBytes = new byte[image.Length];
                await imageStream.ReadAsync(imageBytes, 0, (int)image.Length);

                using var formData = new MultipartFormDataContent();
                var imageContent = new ByteArrayContent(imageBytes);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType ?? "image/jpeg");

                formData.Add(imageContent, "file", image.FileName);

                var url = $"http://localhost:8005/images/analyze?user_role={userRole}";

                var response = await _httpClient.PostAsync(url, formData);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return ReturnBaseHandler.Failed<LabModelResponse>($"API Error {response.StatusCode}: {error}");
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };

                var modelResponse = JsonSerializer.Deserialize<LabModelResponse>(responseJson, options);

                if (modelResponse is null)
                    return ReturnBaseHandler.Failed<LabModelResponse>("Failed to deserialize model response");

                return ReturnBaseHandler.Success(modelResponse);
            }
            catch (HttpRequestException ex)
            {
                return ReturnBaseHandler.Failed<LabModelResponse>($"Connection failed — is FastAPI running on port 8001? {ex.Message}");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<LabModelResponse>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<ModelResponse>> GetXRayModelResponseAsync(IFormFile image, string userRole)
        {
            var result = await GetModelResponseAsync(image, userRole, "8002");
            return result;
        }

        private async Task<ReturnBase<ModelResponse>> GetModelResponseAsync(IFormFile image, string userRole, string port)
        {
            try
            {
                await using var imageStream = image.OpenReadStream();
                var imageBytes = new byte[image.Length];
                await imageStream.ReadAsync(imageBytes, 0, (int)image.Length);

                using var formData = new MultipartFormDataContent();
                var imageContent = new ByteArrayContent(imageBytes);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType ?? "image/jpeg");

                formData.Add(imageContent, "file", image.FileName);

                var url = $"http://localhost:{port}/predict?user_role={userRole}";

                var response = await _httpClient.PostAsync(url, formData);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return ReturnBaseHandler.Failed<ModelResponse>($"API Error {response.StatusCode}: {error}");
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };

                var modelResponse = JsonSerializer.Deserialize<ModelResponse>(responseJson, options);

                if (modelResponse is null)
                    return ReturnBaseHandler.Failed<ModelResponse>("Failed to deserialize model response");

                return ReturnBaseHandler.Success(modelResponse);
            }
            catch (HttpRequestException ex)
            {
                return ReturnBaseHandler.Failed<ModelResponse>($"Connection failed — is FastAPI running on port 8001? {ex.Message}");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<ModelResponse>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
