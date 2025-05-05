using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using TriggersModels;

namespace Triggers.Controllers
{
    public class AgendaController : Controller
    {
        private readonly HttpClient _httpClient;
        public AgendaController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7098"); // API project URL
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("https://localhost:7098/api/Agenda");
            if (response.IsSuccessStatusCode)
            {
                var usersJson = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<Agenda>>(usersJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(users);
            }

            return View(new List<Agenda>());
        }
        /// <summary>
        /// Show The edit view given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Agenda/Edit
        public async Task<IActionResult> Edit(string? id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7098/api/Agenda/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var agenda = JsonSerializer.Deserialize<Agenda>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(agenda);
        }
        /// <summary>
        /// Send Put Method to update the record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: /Agenda/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Agenda model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"https://localhost:7098/api/Agenda/{model.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // If API call fails
            ModelState.AddModelError("", "Could not Update the agenda item.");
            return View("Edit", model);
        }
        /// <summary>
        /// Show the Create view
        /// </summary>
        /// <returns></returns>
        // GET: Agenda/Create
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Send Post Method to Insert the record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: /Agenda/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agenda model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7098/api/Agenda", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // If API call fails
            ModelState.AddModelError("", "Could not save the agenda item.");
            return View(model);
        }
        /// <summary>
        /// Show the delete view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7098/api/Agenda/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var agenda = JsonSerializer.Deserialize<Agenda>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(agenda);
        }

        /// <summary>
        /// Send Post Method to Delete the record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Agenda model)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7098/api/agenda/{model.Id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to delete the agenda item.");
            return View(model);
        }
    }
}
